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
    public static Core.RetCode Breakaway<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        BreakawayImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    [PublicAPI]
    public static int BreakawayLookback() => CandleAveragePeriod(Core.CandleSettingType.BodyLong) + 4;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Breakaway<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        BreakawayImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode BreakawayImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var lookbackTotal = BreakawayLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var bodyLongPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle: long black (white)
         *   - second candle: black (white) day whose body gaps down (up)
         *   - third candle: black or white day with lower (higher) high and lower (higher) low than prior candle's
         *   - fourth candle: black (white) day with lower (higher) high and lower (higher) low than prior candle's
         *   - fifth candle: white (black) day that closes inside the gap, erasing the prior 3 days
         * The meaning of "long" is specified with CandleSettings
         * outIntType is positive (100) when bullish or negative (-100) when bearish
         * it should be considered that breakaway is significant in a trend opposite to the last candle,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsBreakawayPattern(inOpen, inHigh, inLow, inClose, i, bodyLongPeriodTotal)
                ? (int) CandleColor(inClose, inOpen, i) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyLongPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);

            i++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsBreakawayPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyLongPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st long
        RealBody(inClose, inOpen, i - 4) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 4) &&
        // 1st, 2nd, 4th same color, 5th opposite
        CandleColor(inClose, inOpen, i - 4) == CandleColor(inClose, inOpen, i - 3) &&
        CandleColor(inClose, inOpen, i - 3) == CandleColor(inClose, inOpen, i - 1) &&
        (int) CandleColor(inClose, inOpen, i - 1) == -(int) CandleColor(inClose, inOpen, i) &&
        (
            // when 1st is black:
            CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.Black &&
            // 2nd gaps down
            RealBodyGapDown(inOpen, inClose, i - 3, i - 4) &&
            // 3rd has lower high and low than 2nd
            inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] &&
            // 4th has lower high and low than 3rd
            inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] < inLow[i - 2] &&
            // 5th closes inside the gap
            inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4]
            ||
            // when 1st is white:
            CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.White &&
            // 2nd gaps up
            RealBodyGapUp(inClose, inOpen, i - 3, i - 4) &&
            // 3rd has higher high and low than 2nd
            inHigh[i - 2] > inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] &&
            // 4th has higher high and low than 3rd
            inHigh[i - 1] > inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] &&
            // 5th closes inside the gap
            inClose[i] < inOpen[i - 3] && inClose[i] > inClose[i - 4]
        );
}
