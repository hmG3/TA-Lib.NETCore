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
    public static Core.RetCode ThreeLineStrike<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ThreeLineStrikeImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    [PublicAPI]
    public static int ThreeLineStrikeLookback() => CandleAveragePeriod(Core.CandleSettingType.Near) + 3;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode ThreeLineStrike<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ThreeLineStrikeImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode ThreeLineStrikeImpl<T>(
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

        var lookbackTotal = ThreeLineStrikeLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> nearPeriodTotal = new T[4];
        var nearTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[3] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 3);
            nearPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - three white soldiers (three black crows): three white (black) candlesticks with consecutively higher (lower) closes,
         *     each opening within or near the previous real body
         *   - fourth candle: black (white) candle that opens above (below) prior candle's close and closes below (above)
         *     the first candle's open
         * The meaning of "near" is specified with CandleSettings
         * outIntType is positive (100) when bullish or negative (-100) when bearish
         * it should be considered that 3-line strike is significant when it appears in a trend
         * in the same direction of the first three candles,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsThreeLineStrikePattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal)
                ? (int) CandleColor(inClose, inOpen, i - 1) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 3; totIdx >= 2; --totIdx)
            {
                nearPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - totIdx);
            }

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsThreeLineStrikePattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> nearPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // three with same color
        CandleColor(inClose, inOpen, i - 3) == CandleColor(inClose, inOpen, i - 2) &&
        CandleColor(inClose, inOpen, i - 2) == CandleColor(inClose, inOpen, i - 1) &&
        // 4th: opposite color
        (int) CandleColor(inClose, inOpen, i) == -(int) CandleColor(inClose, inOpen, i - 1) &&
        // 2nd opens within/near 1st real body
        inOpen[i - 2] >= T.Min(inOpen[i - 3], inClose[i - 3]) -
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
        inOpen[i - 2] <= T.Max(inOpen[i - 3], inClose[i - 3]) +
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
        // 3rd opens within/near 2nd real body
        inOpen[i - 1] >= T.Min(inOpen[i - 2], inClose[i - 2]) -
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
        inOpen[i - 1] <= T.Max(inOpen[i - 2], inClose[i - 2]) +
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
        (
            // if three white
            CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
            // consecutive higher closes
            inClose[i - 1] > inClose[i - 2] && inClose[i - 2] > inClose[i - 3] &&
            // 4th opens above prior close
            inOpen[i] > inClose[i - 1] &&
            // 4th closes below 1st open
            inClose[i] < inOpen[i - 3]
            ||
            // if three black
            CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
            // consecutive lower closes
            inClose[i - 1] < inClose[i - 2] && inClose[i - 2] < inClose[i - 3] &&
            // 4th opens below prior close
            inOpen[i] < inClose[i - 1] &&
            // 4th closes above 1st open
            inClose[i] > inOpen[i - 3]
        );
}
