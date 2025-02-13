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
    /// Time Series Forecast (Statistic Functions)
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
    /// Time Series Forecast function calculates a linear regression forecast, projecting a future value based on past data.
    /// It applies the least squares method to determine the best-fitting straight line for the specified period.
    /// <para>
    /// The function can assist in anticipating near-term price movements.
    /// Aligning these projections with trend or momentum indicators ensures that forecasts are consistent with broader conditions.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the sum of the input values and their corresponding indices over the specified period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Determine the slope (m) and intercept (b) of the linear regression line using the least squares method:
    /// <code>
    /// Slope (m) = (N * Σ(xy) - Σ(x) * Σ(y)) / (N * Σ(x²) - (Σ(x))²).
    /// Intercept (b) = (Σ(y) - m * Σ(x)) / N.
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the forecasted value for the next time step using the equation <c>y = b + m * x</c>,
    ///       where x is the next index in the sequence.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>A rising value indicates an upward trend.</description>
    ///   </item>
    ///   <item>
    ///     <description>A falling value indicates a downward trend.</description>
    ///   </item>
    ///   <item>
    ///     <description>The TSF can be used to project future values and assess trend strength.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Tsf<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        TsfImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Tsf{T}">Tsf</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int TsfLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Tsf<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        TsfImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode TsfImpl<T>(
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

        /* Linear Regression is a concept also known as the "least squares method" or "best fit."
         * Linear Regression attempts to fit a straight line between several data points in such a way that
         * distance between each data point and the line is minimized.
         *
         * For each point, a straight line over the specified previous bar period is determined in terms of y = b + m * x:
         *
         * Returns b + m * (period)
         */

        var lookbackTotal = TsfLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outIdx = 0;
        var today = startIdx;

        var timePeriod = T.CreateChecked(optInTimePeriod);

        var sumX = timePeriod * (timePeriod - T.One) * T.CreateChecked(0.5);
        var sumXSqr = timePeriod * (timePeriod - T.One) * (timePeriod * FunctionHelpers.Two<T>() - T.One) / T.CreateChecked(6);
        var divisor = sumX * sumX - timePeriod * sumXSqr;
        while (today <= endIdx)
        {
            var sumXY = T.Zero;
            var sumY = T.Zero;
            for (var i = optInTimePeriod; i-- != 0;)
            {
                var tempValue1 = inReal[today - i];
                sumY += tempValue1;
                sumXY += T.CreateChecked(i) * tempValue1;
            }

            var m = (timePeriod * sumXY - sumX * sumY) / divisor;
            var b = (sumY - m * sumX) / timePeriod;
            outReal[outIdx++] = b + m * timePeriod;
            today++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
