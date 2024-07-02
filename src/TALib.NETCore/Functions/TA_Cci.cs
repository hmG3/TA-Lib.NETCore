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
    public static Core.RetCode Cci<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = CciLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Allocate a circular buffer equal to the requested period.
        Span<T> circBuffer = new T[optInTimePeriod];
        int circBufferIdx = default;
        var maxIdxCircBuffer = optInTimePeriod - 1;

        // Do the MA calculation using tight loops.

        // Add-up the initial period, except for the last value. Fill up the circular buffer at the same time.
        var i = startIdx - lookbackTotal;
        while (i < startIdx)
        {
            circBuffer[circBufferIdx++] = (inHigh[i] + inLow[i] + inClose[i]) / Three<T>();
            i++;
            if (circBufferIdx > maxIdxCircBuffer)
            {
                circBufferIdx = 0;
            }
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);
        var tPointZeroOneFive = T.CreateChecked(0.015);

        // Proceed with the calculation for the requested range.
        // The algorithm allows the input and output to be the same buffer.
        int outIdx = default;
        do
        {
            var lastValue = (inHigh[i] + inLow[i] + inClose[i]) / Three<T>();
            circBuffer[circBufferIdx++] = lastValue;

            // Calculate the average for the whole period.
            var theAverage = T.Zero;
            for (var j = 0; j < optInTimePeriod; j++)
            {
                theAverage += circBuffer[j];
            }

            theAverage /= timePeriod;

            // Do the summation of the Abs(TypePrice-average) for the whole period.
            var tempReal2 = T.Zero;
            for (var j = 0; j < optInTimePeriod; j++)
            {
                tempReal2 += T.Abs(circBuffer[j] - theAverage);
            }

            var tempReal = lastValue - theAverage;
            outReal[outIdx++] = !T.IsZero(tempReal) && !T.IsZero(tempReal2)
                ? tempReal / (tPointZeroOneFive * (tempReal2 / timePeriod))
                : T.Zero;

            // Move forward the circular buffer indexes.
            if (circBufferIdx > maxIdxCircBuffer)
            {
                circBufferIdx = 0;
            }

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CciLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Cci<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        Cci<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
