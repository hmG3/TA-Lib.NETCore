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
    public static Core.RetCode Engulfing<T>(
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

        var lookbackTotal = EngulfingLookback();
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
        var i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first: black (white) real body
         *   - second: white (black) real body that engulfs the prior real body
         * outIntType is positive (100) when bullish or negative (-100) when bearish:
         *   - 100 is returned when the second candle's real body begins before and ends after the first candle's real body
         * it should be considered that an engulfing must appear in a downtrend if bullish or in an uptrend if bearish,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsEngulfingPattern(inOpen, inClose, i) ? (int) CandleColor(inClose, inOpen, i) * 100 : 0;

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int EngulfingLookback() => 2;

    private static bool IsEngulfingPattern<T>(ReadOnlySpan<T> inOpen, ReadOnlySpan<T> inClose, int i) where T : IFloatingPointIeee754<T> =>
        // white engulfs black
        CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
        (inClose[i] >= inOpen[i - 1] && inOpen[i] < inClose[i - 1] || inClose[i] > inOpen[i - 1] && inOpen[i] <= inClose[i - 1])
        ||
        // black engulfs white
        CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        (inOpen[i] >= inClose[i - 1] && inClose[i] < inOpen[i - 1] || inOpen[i] > inClose[i - 1] && inClose[i] <= inOpen[i - 1]);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Engulfing<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outIntType) where T : IFloatingPointIeee754<T> =>
        Engulfing<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outIntType, out _, out _);
}
