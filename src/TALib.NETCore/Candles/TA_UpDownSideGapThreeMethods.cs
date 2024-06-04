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
    public static Core.RetCode UpDownSideGapThreeMethods<T>(
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

        var lookbackTotal = UpDownSideGapThreeMethodsLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var i = startIdx;
        int outIdx = default;
        do
        {
            if (CandleColor(inClose, inOpen, i - 2) == CandleColor(inClose, inOpen, i - 1) && // 1st and 2nd of same color
                (int) CandleColor(inClose, inOpen, i - 1) == -(int) CandleColor(inClose, inOpen, i) && // 3rd opposite color
                inOpen[i] < T.Max(inClose[i - 1], inOpen[i - 1]) && // 3rd opens within 2nd rb
                inOpen[i] > T.Min(inClose[i - 1], inOpen[i - 1]) &&
                inClose[i] < T.Max(inClose[i - 2], inOpen[i - 2]) && // 3rd closes within 1st rb
                inClose[i] > T.Min(inClose[i - 2], inOpen[i - 2]) &&
                (CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White && // when 1st is white
                 RealBodyGapUp(inOpen, inClose, i - 1, i - 2) // upside gap
                 ||
                 CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black && // when 1st is black
                 RealBodyGapDown(inOpen, inClose, i - 1, i - 2))) // downside gap
            {
                outInteger[outIdx++] = (int) CandleColor(inClose, inOpen, i - 2) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int UpDownSideGapThreeMethodsLookback() => 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode UpDownSideGapThreeMethods<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        UpDownSideGapThreeMethods<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
