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
    public static Core.RetCode Kama(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30)
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

        var lookbackTotal = KamaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T constMax = TTwo / (T.CreateChecked(30) + T.One);
        T constDiff = TTwo / (TTwo + T.One) - constMax;

        T sumROC1 = T.Zero;
        T tempReal;
        var today = startIdx - lookbackTotal;
        var trailingIdx = today;
        var i = optInTimePeriod;
        while (i-- > 0)
        {
            tempReal = inReal[today++];
            tempReal -= inReal[today];
            sumROC1 += T.Abs(tempReal);
        }

        T prevKAMA = inReal[today - 1];

        tempReal = inReal[today];
        T tempReal2 = inReal[trailingIdx++];
        T periodROC = tempReal - tempReal2;

        T trailingValue = tempReal2;
        if (sumROC1 <= periodROC || T.IsZero(sumROC1))
        {
            tempReal = T.One;
        }
        else
        {
            tempReal = T.Abs(periodROC / sumROC1);
        }

        tempReal = tempReal * constDiff + constMax;
        tempReal *= tempReal;

        prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
        while (today <= startIdx)
        {
            tempReal = inReal[today];
            tempReal2 = inReal[trailingIdx++];
            periodROC = tempReal - tempReal2;

            sumROC1 -= T.Abs(trailingValue - tempReal2);
            sumROC1 += T.Abs(tempReal - inReal[today - 1]);

            trailingValue = tempReal2;
            if (sumROC1 <= periodROC || T.IsZero(sumROC1))
            {
                tempReal = T.One;
            }
            else
            {
                tempReal = T.Abs(periodROC / sumROC1);
            }

            tempReal = tempReal * constDiff + constMax;
            tempReal *= tempReal;

            prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
        }

        outReal[0] = prevKAMA;
        var outIdx = 1;
        outBegIdx = today - 1;
        while (today <= endIdx)
        {
            tempReal = inReal[today];
            tempReal2 = inReal[trailingIdx++];
            periodROC = tempReal - tempReal2;

            sumROC1 -= T.Abs(trailingValue - tempReal2);
            sumROC1 += T.Abs(tempReal - inReal[today - 1]);

            trailingValue = tempReal2;
            if (sumROC1 <= periodROC || T.IsZero(sumROC1))
            {
                tempReal = T.One;
            }
            else
            {
                tempReal = T.Abs(periodROC / sumROC1);
            }

            tempReal = tempReal * constDiff + constMax;
            tempReal *= tempReal;

            prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
            outReal[outIdx++] = prevKAMA;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int KamaLookback(int optInTimePeriod = 30) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Kama);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Kama(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 30) => Kama(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
