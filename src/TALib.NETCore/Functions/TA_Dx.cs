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
    public static Core.RetCode Dx<T>(
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

        var lookbackTotal = DxLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        T prevMinusDM = T.Zero;
        T prevPlusDM = T.Zero;
        T prevTR = T.Zero;
        var today = startIdx - lookbackTotal;
        T prevHigh = inHigh[today];
        T prevLow = inLow[today];
        T prevClose = inClose[today];
        var i = optInTimePeriod - 1;
        while (i-- > 0)
        {
            today++;
            T tempReal = inHigh[today];
            T diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            T diffM = prevLow - tempReal;
            prevLow = tempReal;

            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            tempReal = TrueRange(prevHigh, prevLow, prevClose);
            prevTR += tempReal;
            prevClose = inClose[today];
        }

        T timePeriod = T.CreateChecked(optInTimePeriod);

        i = Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Dx) + 1;
        while (i-- != 0)
        {
            today++;
            T tempReal = inHigh[today];
            T diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            T diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / timePeriod;
            prevPlusDM -= prevPlusDM / timePeriod;

            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            tempReal = TrueRange(prevHigh, prevLow, prevClose);
            prevTR = prevTR - prevTR / timePeriod + tempReal;
            prevClose = inClose[today];
        }

        if (!T.IsZero(prevTR))
        {
            T minusDI = Hundred<T>() * (prevMinusDM / prevTR);
            T plusDI = Hundred<T>() * (prevPlusDM / prevTR);
            T tempReal = minusDI + plusDI;
            outReal[0] = !T.IsZero(tempReal) ? Hundred<T>() * (T.Abs(minusDI - plusDI) / tempReal) : T.Zero;
        }
        else
        {
            outReal[0] = T.Zero;
        }

        var outIdx = 1;
        while (today < endIdx)
        {
            today++;
            T tempReal = inHigh[today];
            T diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            T diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / timePeriod;
            prevPlusDM -= prevPlusDM / timePeriod;

            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            tempReal = TrueRange(prevHigh, prevLow, prevClose);
            prevTR = prevTR - prevTR / timePeriod + tempReal;
            prevClose = inClose[today];

            if (!T.IsZero(prevTR))
            {
                T minusDI = Hundred<T>() * (prevMinusDM / prevTR);
                T plusDI = Hundred<T>() * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (!T.IsZero(tempReal))
                {
                    outReal[outIdx] = Hundred<T>() * (T.Abs(minusDI - plusDI) / tempReal);
                }
                else
                {
                    outReal[outIdx] = outReal[outIdx - 1];
                }
            }
            else
            {
                outReal[outIdx] = outReal[outIdx - 1];
            }

            outIdx++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int DxLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Dx);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Dx<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        Dx<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
