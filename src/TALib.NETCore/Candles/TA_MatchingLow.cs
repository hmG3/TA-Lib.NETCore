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
    public static Core.RetCode MatchingLow<T>(
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

        var lookbackTotal = MatchingLowLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T equalPeriodTotal = T.Zero;
        var equalTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Equal);
        var i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black && // first black
                CandleColor(inClose, inOpen, i) == Core.CandleColor.Black && // second black
                inClose[i] <= inClose[i - 1] +
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 1) && // 1st and 2nd same close
                inClose[i] >= inClose[i - 1] -
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1)
               )
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            equalPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            i++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MatchingLowLookback() => CandleAveragePeriod(Core.CandleSettingType.Equal) + 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MatchingLow<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        MatchingLow<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
