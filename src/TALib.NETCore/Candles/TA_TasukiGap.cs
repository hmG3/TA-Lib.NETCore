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

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode TasukiGap(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outInteger,
        out int outBegIdx,
        out int outNbElement)
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

        T nearPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && // upside gap
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White && // 1st: white
                CandleColor(inClose, inOpen, i) == Core.CandleColor.Black && // 2nd: black
                inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] && //      that opens within the white rb
                inClose[i] < inOpen[i - 1] && //      and closes under the white rb
                inClose[i] > T.Max(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                // size of 2 rb near the same
                T.Abs(RealBody(inClose, inOpen, i - 1) - RealBody(inClose, inOpen, i)) <
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
                ||
                RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // downside gap
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black && // 1st: black
                CandleColor(inClose, inOpen, i) == Core.CandleColor.White && // 2nd: white
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] && //      that opens within the black rb
                inClose[i] > inOpen[i - 1] && //      and closes above the black rb
                inClose[i] < T.Min(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                // size of 2 rb near the same
                T.Abs(RealBody(inClose, inOpen, i - 1) - RealBody(inClose, inOpen, i)) <
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1))
            {
                outInteger[outIdx++] = (int) CandleColor(inClose, inOpen, i - 1) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                               CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);
            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TasukiGapLookback() => CandleAveragePeriod(Core.CandleSettingType.Near) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode TasukiGap(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) => TasukiGap(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
