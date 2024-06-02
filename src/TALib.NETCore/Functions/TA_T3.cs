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
    public static Core.RetCode T3(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 5,
        double optInVFactor = 0.7)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2 || optInVFactor < 0.0 || optInVFactor > 1.0)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = T3Lookback(optInTimePeriod);
        if (startIdx <= lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        var today = startIdx - lookbackTotal;

        T timePeriod = T.CreateChecked(optInTimePeriod);

        T k = TTwo / (timePeriod + T.One);
        T oneMinusK = T.One - k;

        T tempReal = inReal[today++];
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            tempReal += inReal[today++];
        }

        T e1 = tempReal / timePeriod;

        tempReal = e1;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            tempReal += e1;
        }

        T e2 = tempReal / timePeriod;

        tempReal = e2;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            tempReal += e2;
        }

        T e3 = tempReal / timePeriod;

        tempReal = e3;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            tempReal += e3;
        }

        T e4 = tempReal / timePeriod;

        tempReal = e4;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            tempReal += e4;
        }

        T e5 = tempReal / timePeriod;

        tempReal = e5;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            tempReal += e5;
        }

        T e6 = tempReal / timePeriod;

        while (today <= startIdx)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            e6 = k * e5 + oneMinusK * e6;
        }

        T vFactor = T.CreateChecked(optInVFactor);
        tempReal = vFactor * vFactor;
        T c1 = T.NegativeOne * tempReal * vFactor;
        T c2 = TThree * (tempReal - c1);
        T c3 = T.NegativeOne * TTwo * TThree * tempReal - TThree * (vFactor - c1);
        T c4 = T.One + TThree * vFactor - c1 + TThree * tempReal;

        int outIdx = default;
        outReal[outIdx++] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;

        while (today <= endIdx)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            e6 = k * e5 + oneMinusK * e6;
            outReal[outIdx++] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int T3Lookback(int optInTimePeriod = 5) =>
        optInTimePeriod < 2 ? -1 : (optInTimePeriod - 1) * 6 + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.T3);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode T3(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 5,
        double optInVFactor = 0.7) => T3(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod, optInVFactor);
}
