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
    public static Core.RetCode Dema<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
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

        /* An explanation of the function, can be found at
         *
         * Stocks & Commodities V. 12:1 (11-19):
         *   Smoothing Data With Faster Moving Averages
         * Stocks & Commodities V. 12:2 (72-80):
         *   Smoothing Data With Less Lag
         *
         * Both magazine articles written by Patrick G. Mulloy
         *
         * Essentially, a DEMA of time series "t" is:
         *   EMA2 = EMA(EMA(t, period), period)
         *   DEMA = 2 * EMA(t, period) - EMA2
         *
         * DEMA offers a moving average with lesser lags than the traditional EMA.
         *
         * Do not confuse a DEMA with the EMA2. Both are called "Double EMA" in the literature,
         * but EMA2 is a simple EMA of an EMA, while DEMA is a composite of a single EMA with EMA2.
         *
         * TEMA is very similar (and from the same author).
         */

        var lookbackEMA = EmaLookback(optInTimePeriod);
        var lookbackTotal = DemaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        /* Allocate a temporary buffer for the firstEMA.
         *
         * When possible, re-use the outputBuffer for temp calculation.
         */
        Span<T> firstEMA;
        if (inReal == outReal)
        {
            firstEMA = outReal;
        }
        else
        {
            var tempInt = lookbackTotal + (endIdx - startIdx) + 1;
            firstEMA = new T[tempInt];
        }

        // Calculate the first EMA
        var k = Two<T>() / (T.CreateChecked(optInTimePeriod) + T.One);
        var retCode = CalcExponentialMA(inReal, startIdx - lookbackEMA, endIdx, firstEMA, out var firstEMABegIdx,
            out var firstEMANbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || firstEMANbElement == 0)
        {
            return retCode;
        }

        // Allocate a temporary buffer for storing the EMA of the EMA.
        Span<T> secondEMA = new T[firstEMANbElement];
        retCode = CalcExponentialMA(firstEMA, 0, firstEMANbElement - 1, secondEMA, out var secondEMABegIdx, out var secondEMANbElement,
            optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || secondEMANbElement == 0)
        {
            return retCode;
        }

        // Iterate through the second EMA and write the DEMA into the output.
        var firstEMAIdx = secondEMABegIdx;
        int outIdx = default;
        while (outIdx < secondEMANbElement)
        {
            outReal[outIdx] = Two<T>() * firstEMA[firstEMAIdx++] - secondEMA[outIdx];
            outIdx++;
        }

        outBegIdx = firstEMABegIdx + secondEMABegIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int DemaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : EmaLookback(optInTimePeriod) * 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Dema<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        Dema<T>(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
