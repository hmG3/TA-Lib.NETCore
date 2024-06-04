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

public static partial class Functions
{
    public static Core.RetCode MinMax<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMin,
        Span<T> outMax,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MinMaxLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        var today = startIdx;
        var trailingIdx = startIdx - lookbackTotal;
        var highestIdx = -1;
        T highest = T.Zero;
        var lowestIdx = -1;
        T lowest = T.Zero;

        while (today <= endIdx)
        {
            T tmpHigh = inReal[today];
            T tmpLow = tmpHigh;
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inReal[highestIdx];
                var i = highestIdx;
                while (++i <= today)
                {
                    tmpHigh = inReal[i];
                    if (tmpHigh > highest)
                    {
                        highestIdx = i;
                        highest = tmpHigh;
                    }
                }
            }
            else if (tmpHigh >= highest)
            {
                highestIdx = today;
                highest = tmpHigh;
            }

            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inReal[lowestIdx];
                var i = lowestIdx;
                while (++i <= today)
                {
                    tmpLow = inReal[i];
                    if (tmpLow < lowest)
                    {
                        lowestIdx = i;
                        lowest = tmpLow;
                    }
                }
            }

            if (tmpLow <= lowest)
            {
                lowestIdx = today;
                lowest = tmpLow;
            }

            outMax[outIdx] = highest;
            outMin[outIdx++] = lowest;
            trailingIdx++;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MinMaxLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode MinMax<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outMin,
        T[] outMax,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        MinMax<T>(inReal, startIdx, endIdx, outMin, outMax, out _, out _, optInTimePeriod);
}
