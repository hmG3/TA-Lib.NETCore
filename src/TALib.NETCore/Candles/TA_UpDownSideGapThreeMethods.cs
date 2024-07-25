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
    public static Core.RetCode UpDownSideGapThreeMethods<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        UpDownSideGapThreeMethodsImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    [PublicAPI]
    public static int UpDownSideGapThreeMethodsLookback() => 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode UpDownSideGapThreeMethods<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        UpDownSideGapThreeMethodsImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode UpDownSideGapThreeMethodsImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        var startIdx = inRange.Start.Value;
        var endIdx = inRange.End.Value;

        if (endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = UpDownSideGapThreeMethodsLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle: white (black) candle
         *   - second candle: white (black) candle
         *   - upside (downside) gap between the first and the second real bodies
         *   - third candle: black (white) candle that opens within the second real body and closes within the first real body
         * outIntType is negative (-100): upside gap two crows is always bearish
         * it should be considered that up/downside gap three methods is significant when it appears in a trend,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsUpDownSideGapThreeMethodsPattern(inOpen, inClose, i)
                ? (int) CandleColor(inClose, inOpen, i - 2) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            i++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsUpDownSideGapThreeMethodsPattern<T>(ReadOnlySpan<T> inOpen, ReadOnlySpan<T> inClose, int i)
        where T : IFloatingPointIeee754<T> =>
        // 1st and 2nd of same color
        CandleColor(inClose, inOpen, i - 2) == CandleColor(inClose, inOpen, i - 1) &&
        // 3rd opposite color
        (int) CandleColor(inClose, inOpen, i - 1) == -(int) CandleColor(inClose, inOpen, i) &&
        // 3rd opens within 2nd rb
        inOpen[i] < T.Max(inClose[i - 1], inOpen[i - 1]) && inOpen[i] > T.Min(inClose[i - 1], inOpen[i - 1]) &&
        // 3rd closes within 1st rb
        inClose[i] < T.Max(inClose[i - 2], inOpen[i - 2]) && inClose[i] > T.Min(inClose[i - 2], inOpen[i - 2]) &&
        (
            // when 1st is white
            CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
            // upside gap
            RealBodyGapUp(inOpen, inClose, i - 1, i - 2)
            ||
            // when 1st is black
            CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
            // downside gap
            RealBodyGapDown(inOpen, inClose, i - 1, i - 2)
        );
}
