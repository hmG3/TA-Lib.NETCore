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
    /// Standard Deviation (Statistic Functions)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <param name="optInNbDev">The number of deviations.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Standard Deviation is a statistical measure that quantifies the dispersion or variability of a data set
    /// relative to its mean. In technical analysis, it is commonly used to assess the volatility of a financial instrument.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the arithmetic mean of the data points in a rolling window of length `optInTimePeriod`.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the variance by summing the squared differences between each data point and the mean, then dividing by the number of data points:
    ///       <code>
    ///         Variance = Σ((Value - Mean)^2) / TimePeriod
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Take the square root of the variance to obtain the standard deviation:
    ///       <code>
    ///         StdDev = √Variance
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Multiply the standard deviation by the specified number of deviations (`optInNbDev`) to scale the result if needed.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A higher value indicates greater variability in the data, often corresponding to increased volatility in financial markets.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A lower value suggests that the data points are closer to the mean, indicating less variability or volatility.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode StdDev<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 5,
        double optInNbDev = 1.0) where T : IFloatingPointIeee754<T> =>
        StdDevImpl(inReal, inRange, outReal, out outRange, optInTimePeriod, optInNbDev);

    /// <summary>
    /// Returns the lookback period for <see cref="StdDev{T}">StdDev</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int StdDevLookback(int optInTimePeriod = 5) => optInTimePeriod < 2 ? -1 : VarLookback(optInTimePeriod);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode StdDev<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 5,
        double optInNbDev = 1.0) where T : IFloatingPointIeee754<T> =>
        StdDevImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod, optInNbDev);

    private static Core.RetCode StdDevImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod,
        double optInNbDev) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var retCode = FunctionHelpers.CalcVariance(inReal, new Range(rangeIndices.startIndex, rangeIndices.endIndex), outReal, out outRange,
            optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var nbElement = outRange.End.Value - outRange.Start.Value;
        // Calculate the square root of each variance, this is the standard deviation.
        // Multiply also by the ratio specified.
        if (!optInNbDev.Equals(1.0))
        {
            for (var i = 0; i < nbElement; i++)
            {
                var tempReal = outReal[i];
                outReal[i] = tempReal > T.Zero ? T.Sqrt(tempReal) * T.CreateChecked(optInNbDev) : T.Zero;
            }
        }
        else
        {
            for (var i = 0; i < nbElement; i++)
            {
                var tempReal = outReal[i];
                outReal[i] = tempReal > T.Zero ? T.Sqrt(tempReal) : T.Zero;
            }
        }

        return Core.RetCode.Success;
    }
}
