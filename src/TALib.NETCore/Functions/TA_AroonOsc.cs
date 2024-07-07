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

        /* This code is almost identical to the Aroon function except that
         * instead of outputting AroonUp and AroonDown individually, an oscillator is build from both.
         *
         *   AroonOsc = AroonUp - AroonDown
         *
         */

        // This function is using a speed optimized algorithm for the min/max logic.
        //It might be needed to first look at how Min/Max works and this function will become easier to understand.

        var lookbackTotal = AroonOscLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Proceed with the calculation for the requested range.
        // The algorithm allows the input and output to be the same buffer.
        int outIdx = default;
        var today = startIdx;
        var trailingIdx = startIdx - lookbackTotal;

        int highestIdx = -1, lowestIdx = -1;
        T highest = T.Zero, lowest = T.Zero;
        var factor = Hundred<T>() / T.CreateChecked(optInTimePeriod);
        while (today <= endIdx)
        {
            (lowestIdx, lowest) = CalcLowest(inLow, trailingIdx, today, lowestIdx, lowest);
            (highestIdx, highest) = CalcHighest(inHigh, trailingIdx, today, highestIdx, highest);

            /* The oscillator is the following:
             *   AroonUp   = factor * (optInTimePeriod - (today - highestIdx))
             *   AroonDown = factor * (optInTimePeriod - (today - lowestIdx))
             *   AroonOsc  = AroonUp - AroonDown
             *
             * An arithmetic simplification gives:
             *   Aroon = factor * (highestIdx - lowestIdx)
             */
            var arron = factor * T.CreateChecked(highestIdx - lowestIdx);

            //Input and output buffer can be the same, so writing to the output is the last thing being done here.
            outReal[outIdx++] = arron;

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
    [UsedImplicitly]
    private static Core.RetCode AroonOsc<T>(
        T[] inHigh,
        T[] inLow,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AroonOsc<T>(inHigh, inLow, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
