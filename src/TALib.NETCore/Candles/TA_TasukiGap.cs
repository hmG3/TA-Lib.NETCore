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
    [PublicAPI]
    public static Core.RetCode TasukiGap<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        TasukiGapImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    [PublicAPI]
    public static int TasukiGapLookback() => CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode TasukiGap<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        TasukiGapImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode TasukiGapImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var lookbackTotal = TasukiGapLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var nearPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
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
         * outIntType is positive (100) when bullish or negative (-100) when bearish
         * it should be considered that tasuki gap is significant when it appears in a trend,
         * while this function does not consider it
         */

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsTasukiGapPattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal)
                ? (int) CandleHelpers.CandleColor(inClose, inOpen, i - 1) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            nearPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsTasukiGapPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T nearPeriodTotal) where T : IFloatingPointIeee754<T> =>
        (
            // upside gap
            CandleHelpers.RealBodyGapUp(inOpen, inClose, i - 1, i - 2) &&
            // 1st: white
            CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
            // 2nd: black
            CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
            // that opens within the white real body
            inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] &&
            // and closes under the white real body
            inClose[i] < inOpen[i - 1] &&
            // inside the gap
            inClose[i] > T.Max(inClose[i - 2], inOpen[i - 2]) &&
            // size of 2 real body near the same
            T.Abs(CandleHelpers.RealBody(inClose, inOpen, i - 1) - CandleHelpers.RealBody(inClose, inOpen, i)) <
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
        )
        ||
        (
            // downside gap
            CandleHelpers.RealBodyGapDown(inOpen, inClose, i - 1, i - 2) &&
            // 1st: black
            CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
            // 2nd: white
            CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
            // that opens within the black rb
            inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
            // and closes above the black rb
            inClose[i] > inOpen[i - 1] &&
            // inside the gap
            inClose[i] < T.Min(inClose[i - 2], inOpen[i - 2]) &&
            // size of 2 real body near the same
            T.Abs(CandleHelpers.RealBody(inClose, inOpen, i - 1) - CandleHelpers.RealBody(inClose, inOpen, i)) <
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
        );
}
