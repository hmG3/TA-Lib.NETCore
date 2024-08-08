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
    [PublicAPI]
    public static Core.RetCode Tema<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        TemaImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int TemaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : EmaLookback(optInTimePeriod) * 3;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Tema<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        TemaImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode TemaImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /* An explanation of the function can be found at:
         *
         * Stocks & Commodities V. 12:1 (11-19):
         *   Smoothing Data With Faster Moving Averages
         * Stocks & Commodities V. 12:2 (72-80):
         *   Smoothing Data With Less Lag
         *
         * Both magazine articles written by Patrick G. Mulloy
         *
         * Essentially, a TEMA of time series "t" is:
         *   EMA1 = EMA(t, period)
         *   EMA2 = EMA(EMA(t, period), period)
         *   EMA3 = EMA(EMA(EMA(t, period), period))
         *   TEMA = 3 * EMA1 - 3 * EMA2 + EMA3
         *
         * TEMA offers a moving average with lesser lags than the traditional EMA.
         *
         * TEMA should not be confused with EMA3. Both are called "Triple EMA" in the literature.
         * DEMA is very similar (and from the same author).
         */

        var lookbackEMA = EmaLookback(optInTimePeriod);
        var lookbackTotal = TemaLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInt = lookbackTotal + (endIdx - startIdx) + 1;
        var k = FunctionHelpers.Two<T>() / (T.CreateChecked(optInTimePeriod) + T.One);

        Span<T> firstEMA = new T[tempInt];
        var retCode = FunctionHelpers.CalcExponentialMA(inReal, new Range(startIdx - lookbackEMA * 2, endIdx), firstEMA,
            out var firstEMARange, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || firstEMARange.End.Value == 0)
        {
            return retCode;
        }

        var firstEMANbElement = firstEMARange.End.Value - firstEMARange.Start.Value;
        Span<T> secondEMA = new T[firstEMANbElement];
        retCode = FunctionHelpers.CalcExponentialMA(firstEMA, Range.EndAt(firstEMANbElement - 1), secondEMA, out var secondEMARange,
            optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || secondEMARange.End.Value == 0)
        {
            return retCode;
        }

        var secondEMANbElement = secondEMARange.End.Value - secondEMARange.Start.Value;
        retCode = FunctionHelpers.CalcExponentialMA(secondEMA, Range.EndAt(secondEMANbElement - 1), outReal, out var thirdEMARange,
            optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || thirdEMARange.End.Value == 0)
        {
            return retCode;
        }

        var firstEMAIdx = thirdEMARange.Start.Value + secondEMARange.Start.Value;
        var secondEMAIdx = thirdEMARange.Start.Value;
        var outBegIdx = firstEMAIdx + firstEMARange.Start.Value;

        var thirdEMANbElement = thirdEMARange.End.Value - thirdEMARange.Start.Value;
        // Iterate through the EMA3 (output buffer) and adjust the value by using the EMA2 and EMA1.
        int outIdx = default;
        while (outIdx < thirdEMANbElement)
        {
            outReal[outIdx++] += FunctionHelpers.Three<T>() * firstEMA[firstEMAIdx++] -
                                 FunctionHelpers.Three<T>() * secondEMA[secondEMAIdx++];
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
