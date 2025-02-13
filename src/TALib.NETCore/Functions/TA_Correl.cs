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
    /// Pearson's Correlation Coefficient (r) (Statistic Functions)
    /// </summary>
    /// <param name="inReal0">A span of input values for the first dataset.</param>
    /// <param name="inReal1">A span of input values for the second dataset.</param>
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
    /// Pearson's Correlation Coefficient (r) measures the linear correlation between two datasets, showing how closely they move together.
    /// <para>
    /// The function is useful for portfolio construction, pair strategies, and diversification.
    /// It can be paired with relative strength or spread indicators to identify correlation breakdowns or convergences.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Compute the sums of the two datasets and their respective squares over the specified time period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the product of the datasets for each time period and calculate the sum of these products.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the covariance and the standard deviations of the datasets.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Divide the covariance by the product of the standard deviations to compute the correlation coefficient.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       -1 value indicates a perfect negative linear relationship.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       0 indicates no linear relationship.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       1 value indicates a perfect positive linear relationship.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Correl<T>(
        ReadOnlySpan<T> inReal0,
        ReadOnlySpan<T> inReal1,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        CorrelImpl(inReal0, inReal1, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Correl{T}">Correl</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int CorrelLookback(int optInTimePeriod = 30) => optInTimePeriod < 1 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Correl<T>(
        T[] inReal0,
        T[] inReal1,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        CorrelImpl<T>(inReal0, inReal1, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode CorrelImpl<T>(
        ReadOnlySpan<T> inReal0,
        ReadOnlySpan<T> inReal1,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal0.Length, inReal1.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = CorrelLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;
        var trailingIdx = startIdx - lookbackTotal;

        // Calculate the initial values.
        T sumX, sumY, sumX2, sumY2;
        var sumXY = sumX = sumY = sumX2 = sumY2 = T.Zero;
        int today;
        for (today = trailingIdx; today <= startIdx; today++)
        {
            var x = inReal0[today];
            sumX += x;
            sumX2 += x * x;

            var y = inReal1[today];
            sumXY += x * y;
            sumY += y;
            sumY2 += y * y;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        // Write the first output.
        // Save first the trailing values since the input and output might be the same span.
        var trailingX = inReal0[trailingIdx];
        var trailingY = inReal1[trailingIdx++];
        var tempReal = (sumX2 - sumX * sumX / timePeriod) * (sumY2 - sumY * sumY / timePeriod);
        outReal[0] = tempReal > T.Zero ? (sumXY - sumX * sumY / timePeriod) / T.Sqrt(tempReal) : T.Zero;

        // Tight loop to do subsequent values.
        var outIdx = 1;
        while (today <= endIdx)
        {
            // Remove trailing values
            sumX -= trailingX;
            sumX2 -= trailingX * trailingX;

            sumXY -= trailingX * trailingY;
            sumY -= trailingY;
            sumY2 -= trailingY * trailingY;

            // Add new values
            var x = inReal0[today];
            sumX += x;
            sumX2 += x * x;

            var y = inReal1[today++];
            sumXY += x * y;
            sumY += y;
            sumY2 += y * y;

            // Output new coefficient.
            // Save first the trailing values since the input and output might be the same span.
            trailingX = inReal0[trailingIdx];
            trailingY = inReal1[trailingIdx++];
            tempReal = (sumX2 - sumX * sumX / timePeriod) * (sumY2 - sumY * sumY / timePeriod);
            outReal[outIdx++] = tempReal > T.Zero ? (sumXY - sumX * sumY / timePeriod) / T.Sqrt(tempReal) : T.Zero;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
