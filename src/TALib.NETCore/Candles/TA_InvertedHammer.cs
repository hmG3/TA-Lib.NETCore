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
    public static Core.RetCode InvertedHammer<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outIntType,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = InvertedHammerLookback();
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
        var bodyPeriodTotal = T.Zero;
        var bodyTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        var shadowLongPeriodTotal = T.Zero;
        var shadowLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowLong);
        var shadowVeryShortPeriodTotal = T.Zero;
        var shadowVeryShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        var i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - small real body
         *   - long upper shadow
         *   - no, or very short, lower shadow
         *   - gap down
         * The meaning of "short", "very short" and "long" is specified with CandleSettings
         * outIntType is positive (100): inverted hammer is always bullish
         * it should be considered that an inverted hammer must appear in a downtrend,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsInvertedHammerPattern(inOpen, inHigh, inLow, inClose, i, bodyPeriodTotal, shadowLongPeriodTotal,
                shadowVeryShortPeriodTotal)
                ? 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyTrailingIdx);

            shadowLongPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx);

            shadowVeryShortPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);

            i++;
            bodyTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int InvertedHammerLookback() =>
        Math.Max(
            Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyShort), CandleAveragePeriod(Core.CandleSettingType.ShadowLong)),
            CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort)
        ) + 1;

    private static bool IsInvertedHammerPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyPeriodTotal,
        T shadowLongPeriodTotal,
        T shadowVeryShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // small real body
        RealBody(inClose, inOpen, i) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal, i) &&
        // long upper shadow
        UpperShadow(inHigh, inClose, inOpen, i) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) &&
        // very short lower shadow
        LowerShadow(inClose, inOpen, inLow, i) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) &&
        // gap down
        RealBodyGapDown(inOpen, inClose, i, i - 1);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode InvertedHammer<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outIntType) where T : IFloatingPointIeee754<T> =>
        InvertedHammer<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outIntType, out _, out _);
}
