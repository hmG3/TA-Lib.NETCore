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
    /// Commodity Channel Index (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
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
    /// Commodity Channel Index is a momentum indicator that measures the deviation of the typical price
    /// (average of high, low, and close) from its moving average over a specified time period.
    /// <para>
    /// It is commonly used to identify overbought and oversold conditions, as well as potential trend reversals.
    /// Combining it with trend indicators can help avoid reacting against strong prevailing trends.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Compute the Typical Price (TP) for each period:
    ///       <code>
    ///         TP = (High + Low + Close) / 3
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the moving average (MA) of the Typical Price over the specified time period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the Mean Deviation of the Typical Price from the moving average.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the CCI:
    ///       <code>
    ///         CCI = (TP - MA) / (0.015 * Mean Deviation)
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A positive value indicates that the price is above the moving average, suggesting bullish momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A negative value indicates that the price is below the moving average, suggesting bearish momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Values above +100 or below -100 are typically used as thresholds for overbought or oversold conditions.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Cci<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        CciImpl(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Cci{T}">Cci</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int CciLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Cci<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        CciImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode CciImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = CciLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Allocate a circular buffer equal to the requested period.
        Span<T> circBuffer = new T[optInTimePeriod];
        var circBufferIdx = 0;
        var maxIdxCircBuffer = optInTimePeriod - 1;

        // Do the MA calculation using tight loops.

        // Add-up the initial period, except for the last value. Fill up the circular buffer at the same time.
        var i = startIdx - lookbackTotal;
        while (i < startIdx)
        {
            circBuffer[circBufferIdx++] = (inHigh[i] + inLow[i] + inClose[i]) / FunctionHelpers.Three<T>();
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
        var outIdx = 0;
        do
        {
            var lastValue = (inHigh[i] + inLow[i] + inClose[i]) / FunctionHelpers.Three<T>();
            circBuffer[circBufferIdx++] = lastValue;

            // Calculate the average for the whole period.
            var theAverage = CalcAverage(circBuffer, timePeriod);

            // Do the summation of the Abs(TypePrice - average) for the whole period.
            var tempReal2 = CalcSummation(circBuffer, theAverage);

            var tempReal = lastValue - theAverage;
            var denominator = tPointZeroOneFive * (tempReal2 / timePeriod);
            outReal[outIdx++] = !T.IsZero(tempReal) && !T.IsZero(denominator) ? tempReal / denominator : T.Zero;

            // Move forward the circular buffer indexes.
            if (circBufferIdx > maxIdxCircBuffer)
            {
                circBufferIdx = 0;
            }

            i++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static T CalcAverage<T>(Span<T> circBuffer, T timePeriod) where T : IFloatingPointIeee754<T>
    {
        var theAverage = T.Zero;
        foreach (var t in circBuffer)
        {
            theAverage += t;
        }

        theAverage /= timePeriod;
        return theAverage;
    }

    private static T CalcSummation<T>(Span<T> circBuffer, T theAverage) where T : IFloatingPointIeee754<T>
    {
        var tempReal2 = T.Zero;
        foreach (var t in circBuffer)
        {
            tempReal2 += T.Abs(t - theAverage);
        }

        return tempReal2;
    }
}
