/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2025 Anatolii Siryi
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
    /// <summary>
    /// Double Exponential Moving Average (Overlap Studies)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Double Exponential Moving Average is designed to reduce the lag associated with traditional moving averages
    /// by combining a single exponential moving average with the EMA of that EMA (commonly referred to as EMA2).
    /// <para>
    /// This calculation results in a smoother average that reacts faster to price changes, making it useful for identifying
    /// trends and reversals with reduced lag. The function can improve responsiveness in trend-following strategies.
    /// Combining it with oscillators may help confirm signals and minimize delays.
    /// </para>
    ///
    /// <b>Calculation formula</b>:
    /// <code>
    ///   DEMA = 2 * EMA(t, period) - EMA(EMA(t, period), period)
    /// </code>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Dema<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        DemaImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Dema{T}">Dema</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int DemaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : EmaLookback(optInTimePeriod) * 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Dema<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        DemaImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode DemaImpl<T>(
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
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Allocate a temporary buffer for the firstEMA.
        // When possible, re-use the outputBuffer for temp calculation.
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
        var k = FunctionHelpers.Two<T>() / (T.CreateChecked(optInTimePeriod) + T.One);
        var retCode = FunctionHelpers.CalcExponentialMA(
            inReal, new Range(startIdx - lookbackEMA, endIdx), firstEMA, out var firstEMARange, optInTimePeriod, k);
        var firstEMANbElement = firstEMARange.End.Value - firstEMARange.Start.Value;
        if (retCode != Core.RetCode.Success || firstEMANbElement == 0)
        {
            return retCode;
        }

        // Allocate a temporary buffer for storing the EMA of the EMA.
        Span<T> secondEMA = new T[firstEMANbElement];
        retCode = FunctionHelpers.CalcExponentialMA(firstEMA, Range.EndAt(firstEMANbElement - 1), secondEMA, out var secondEMARange,
            optInTimePeriod, k);
        var secondEMABegIdx = secondEMARange.Start.Value;
        var secondEMANbElement = secondEMARange.End.Value - secondEMABegIdx;
        if (retCode != Core.RetCode.Success || secondEMANbElement == 0)
        {
            return retCode;
        }

        // Iterate through the second EMA and write the DEMA into the output.
        var firstEMAIdx = secondEMABegIdx;
        var outIdx = 0;
        while (outIdx < secondEMANbElement)
        {
            outReal[outIdx] = FunctionHelpers.Two<T>() * firstEMA[firstEMAIdx++] - secondEMA[outIdx];
            outIdx++;
        }

        outRange = new Range(firstEMARange.Start.Value + secondEMABegIdx, firstEMARange.Start.Value + secondEMABegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
