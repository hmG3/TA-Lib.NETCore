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
    public static Core.RetCode AroonOsc<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inHigh.Length || endIdx >= inLow.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = AroonOscLookback(optInTimePeriod);
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
        var lowestIdx = -1;
        var highestIdx = -1;
        T lowest = T.Zero;
        T highest = T.Zero;
        T factor = Hundred<T>() / T.CreateChecked(optInTimePeriod);

        while (today <= endIdx)
        {
            T tmp = inLow[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inLow[lowestIdx];
                var i = lowestIdx;
                while (++i <= today)
                {
                    tmp = inLow[i];
                    if (tmp <= lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }
            }
            else if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
            }

            tmp = inHigh[today];
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inHigh[highestIdx];
                var i = highestIdx;
                while (++i <= today)
                {
                    tmp = inHigh[i];
                    if (tmp >= highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }
            }
            else if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
            }

            outReal[outIdx++] = factor * T.CreateChecked(highestIdx - lowestIdx);

            trailingIdx++;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AroonOscLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode AroonOsc<T>(
        T[] inHigh,
        T[] inLow,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AroonOsc<T>(inHigh, inLow, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
