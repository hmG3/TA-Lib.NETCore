/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2024 Anatolii Siryi
 *
 * This file is part of Technical Analysis Library for .NET.
 *
 * Technical Analysis Library for .NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Technical Analysis Library for .NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with Technical Analysis Library for .NET. If not, see <https://www.gnu.org/licenses/>.
 */

namespace TALib;

public static partial class Candles
{
    public static Core.RetCode Kicking<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outInteger,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = KickingLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> shadowVeryShortPeriodTotal = new T[2];
        var shadowVeryShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        Span<T> bodyLongPeriodTotal = new T[2];
        var bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1);
            bodyLongPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            outInteger[outIdx++] = IsKickingPattern(inOpen, inHigh, inLow, inClose, i, bodyLongPeriodTotal, shadowVeryShortPeriodTotal)
                ? (int) CandleColor(inClose, inOpen, i) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                bodyLongPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - totIdx);

                shadowVeryShortPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outNbElement = outIdx;
        outBegIdx = startIdx;

        return Core.RetCode.Success;
    }

    public static int KickingLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort), CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 1;

    private static bool IsKickingPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> bodyLongPeriodTotal,
        Span<T> shadowVeryShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // opposite candles
        (int) CandleColor(inClose, inOpen, i - 1) == -(int) CandleColor(inClose, inOpen, i) &&
        // 1st marubozu
        RealBody(inClose, inOpen, i - 1) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal[1], i - 1) &&
        UpperShadow(inHigh, inClose, inOpen, i - 1) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
        LowerShadow(inClose, inOpen, inLow, i - 1) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
        // 2nd marubozu
        RealBody(inClose, inOpen, i) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal[0], i) &&
        UpperShadow(inHigh, inClose, inOpen, i) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
        LowerShadow(inClose, inOpen, inLow, i) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
        // gap
        (
            CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black && CandleGapUp(inLow, inHigh, i, i - 1) ||
            CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White && CandleGapDown(inLow, inHigh, i, i - 1)
        );

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Kicking<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        Kicking<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
