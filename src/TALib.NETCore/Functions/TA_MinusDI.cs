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
    public static Core.RetCode MinusDI<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MinusDILookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int today;
        T prevLow;
        T prevHigh;
        T diffM;
        T prevClose;
        T diffP;
        int outIdx = default;
        if (optInTimePeriod == 1)
        {
            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            prevClose = inClose[today];
            while (today < endIdx)
            {
                today++;
                T tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffM > T.Zero && diffP < diffM)
                {
                    TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                    outReal[outIdx++] = !T.IsZero(tempReal) ? diffM / tempReal : T.Zero;
                }
                else
                {
                    outReal[outIdx++] = T.Zero;
                }

                prevClose = inClose[today];
            }

            outNbElement = outIdx;

            return Core.RetCode.Success;
        }

        today = startIdx;
        outBegIdx = today;
        T prevMinusDM = T.Zero;
        T prevTR = T.Zero;
        today = startIdx - lookbackTotal;
        prevHigh = inHigh[today];
        prevLow = inLow[today];
        prevClose = inClose[today];
        var i = optInTimePeriod - 1;
        while (i-- > 0)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;
            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR += tempReal;
            prevClose = inClose[today];
        }

        T timePeriod = T.CreateChecked(optInTimePeriod);

        i = Core.UnstablePeriodSettings.Get(Core.UnstableFunc.MinusDI) + 1;
        while (i-- != 0)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;
            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;
            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM = prevMinusDM - prevMinusDM / timePeriod + diffM;
            }
            else
            {
                prevMinusDM -= prevMinusDM / timePeriod;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / timePeriod + tempReal;
            prevClose = inClose[today];
        }

        outReal[0] = !T.IsZero(prevTR) ? Hundred<T>() * (prevMinusDM / prevTR) : T.Zero;
        outIdx = 1;

        while (today < endIdx)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;
            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;
            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM = prevMinusDM - prevMinusDM / timePeriod + diffM;
            }
            else
            {
                prevMinusDM -= prevMinusDM / timePeriod;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / timePeriod + tempReal;
            prevClose = inClose[today];
            outReal[outIdx++] = !T.IsZero(prevTR) ? Hundred<T>() * (prevMinusDM / prevTR) : T.Zero;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MinusDILookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 1 ? -1 :
        optInTimePeriod > 1 ? optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.MinusDI) : 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode MinusDI<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod) where T : IFloatingPointIeee754<T> =>
        MinusDI<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
