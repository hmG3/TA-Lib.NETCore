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

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode PlusDI(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14)
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

        var lookbackTotal = PlusDILookback(optInTimePeriod);
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
        T diffP;
        T prevClose;
        T diffM;
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
                if (diffP > T.Zero && diffP > diffM)
                {
                    TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                    outReal[outIdx++] = !T.IsZero(tempReal) ? diffP / tempReal : T.Zero;
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
        T prevPlusDM = T.Zero;
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
            if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR += tempReal;
            prevClose = inClose[today];
        }

        T timePeriod = T.CreateChecked(optInTimePeriod);

        i = Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDI) + 1;
        while (i-- != 0)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;
            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;
            if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM = prevPlusDM - prevPlusDM / timePeriod + diffP;
            }
            else
            {
                prevPlusDM -= prevPlusDM / timePeriod;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / timePeriod + tempReal;
            prevClose = inClose[today];
        }

        outReal[0] = !T.IsZero(prevTR) ? THundred * (prevPlusDM / prevTR) : T.Zero;
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
            if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM = prevPlusDM - prevPlusDM / timePeriod + diffP;
            }
            else
            {
                prevPlusDM -= prevPlusDM / timePeriod;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / timePeriod + tempReal;
            prevClose = inClose[today];
            outReal[outIdx++] = !T.IsZero(prevTR) ? THundred * (prevPlusDM / prevTR) : T.Zero;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int PlusDILookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 1 ? -1 :
        optInTimePeriod > 1 ? optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDI) : 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode PlusDI(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) => PlusDI(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
