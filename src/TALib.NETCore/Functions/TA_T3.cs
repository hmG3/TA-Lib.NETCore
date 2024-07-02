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
    public static Core.RetCode T3<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 5,
        double optInVFactor = 0.7) where T : IFloatingPointIeee754<T>
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

        /* An explanation of the function can be found at:
         *
         * Magazine articles written by Tim Tillson
         *
         * Essentially, a T3 of time series "t" is:
         *   EMA1(x, Period) = EMA(x, Period)
         *   EMA2(x, Period) = EMA(EMA1(x, Period), Period)
         *   GD(x, Period, vFactor) = (EMA1(x, Period) * (1 + vFactor)) - (EMA2(x, Period) * vFactor)
         *   T3 = GD(GD(GD(t, Period, vFactor), Period, vFactor), Period, vFactor);
         *
         * T3 offers a moving average with lesser lags than the traditional EMA.
         * T3 should not be confused with EMA3. Both are called "Triple EMA" in the literature.
         */

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

        var timePeriod = T.CreateChecked(optInTimePeriod);

        var k = Two<T>() / (timePeriod + T.One);
        var oneMinusK = T.One - k;

        var tempReal = inReal[today++];
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            tempReal += inReal[today++];
        }

        var e1 = tempReal / timePeriod;

        tempReal = e1;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            tempReal += e1;
        }

        var e2 = tempReal / timePeriod;

        tempReal = e2;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            tempReal += e2;
        }

        var e3 = tempReal / timePeriod;

        tempReal = e3;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            tempReal += e3;
        }

        var e4 = tempReal / timePeriod;

        tempReal = e4;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            tempReal += e4;
        }

        var e5 = tempReal / timePeriod;

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

        var e6 = tempReal / timePeriod;

        // Skip the unstable period
        while (today <= startIdx)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            e6 = k * e5 + oneMinusK * e6;
        }

        // Calculate the constants
        var vFactor = T.CreateChecked(optInVFactor);
        tempReal = vFactor * vFactor;
        var c1 = T.NegativeOne * tempReal * vFactor;
        var c2 = Three<T>() * (tempReal - c1);
        var c3 = T.NegativeOne * Two<T>() * Three<T>() * tempReal - Three<T>() * (vFactor - c1);
        var c4 = T.One + Three<T>() * vFactor - c1 + Three<T>() * tempReal;

        // Write the first output
        int outIdx = default;
        outReal[outIdx++] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;

        // Calculate and output the remaining of the range.
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
    [UsedImplicitly]
    private static Core.RetCode T3<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 5,
        double optInVFactor = 0.7) where T : IFloatingPointIeee754<T> =>
        T3<T>(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod, optInVFactor);
}
