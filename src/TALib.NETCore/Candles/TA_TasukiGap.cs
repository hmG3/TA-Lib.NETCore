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
    public static Core.RetCode TasukiGap<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<Core.CandlePatternType> outType,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = TasukiGapLookback();
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
        var nearPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - upside (downside) gap
         *   - first candle after the window: white (black) candlestick
         *   - second candle: black (white) candlestick that opens within the previous real body and closes under (above)
         *     the previous real body inside the gap
         *   - the size of two real bodies should be near the same
         * The meaning of "near" is specified with CandleSettings
         * outType is Bullish or Bearish;
         * the user should consider that tasuki gap is significant when it appears in a trend,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outType[outIdx++] = IsTasukiGapPattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal)
                ? (Core.CandlePatternType) ((int) CandleColor(inClose, inOpen, i - 1) * 100)
                : Core.CandlePatternType.None;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            nearPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TasukiGapLookback() => CandleAveragePeriod(Core.CandleSettingType.Near) + 2;

    private static bool IsTasukiGapPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T nearPeriodTotal) where T : IFloatingPointIeee754<T> =>
        (
            // upside gap
            RealBodyGapUp(inOpen, inClose, i - 1, i - 2) &&
            // 1st: white
            CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
            // 2nd: black
            CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
            // that opens within the white real body
            inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] &&
            // and closes under the white real body
            inClose[i] < inOpen[i - 1] &&
            // inside the gap
            inClose[i] > T.Max(inClose[i - 2], inOpen[i - 2]) &&
            // size of 2 real body near the same
            T.Abs(RealBody(inClose, inOpen, i - 1) - RealBody(inClose, inOpen, i)) <
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
        )
        ||
        (
            // downside gap
            RealBodyGapDown(inOpen, inClose, i - 1, i - 2) &&
            // 1st: black
            CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
            // 2nd: white
            CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
            // that opens within the black rb
            inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
            // and closes above the black rb
            inClose[i] > inOpen[i - 1] &&
            // inside the gap
            inClose[i] < T.Min(inClose[i - 2], inOpen[i - 2]) &&
            // size of 2 real body near the same
            T.Abs(RealBody(inClose, inOpen, i - 1) - RealBody(inClose, inOpen, i)) <
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
        );

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode TasukiGap<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        Core.CandlePatternType[] outType) where T : IFloatingPointIeee754<T> =>
        TasukiGap<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outType, out _, out _);
}
