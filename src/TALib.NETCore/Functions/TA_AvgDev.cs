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
    /// Average Deviation (Price Transform)
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
    /// Average Deviation calculates how far each data point deviates from the mean, serving as a measure of volatility.
    /// It provides insights into the variability or volatility of a dataset.
    /// <para>
    /// The function can help assess market stability or variability.
    /// Incorporating it into strategies with trend or momentum indicators may refine volatility-informed decisions.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>Compute the mean (average) of the values over the specified time period:
    ///       <code>
    ///         Mean = Sum of values / Time Period
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>Calculate the absolute deviation of each value from the mean:
    ///       <code>
    ///         Deviation[i] = abs(Value[i] - Mean)
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the average of these deviations to get the AvgDev:
    ///       <code>
    ///         AvgDev = Sum of Deviations / Time Period
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A higher value indicates greater variability.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A lower value indicates more stability.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode AvgDev<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AvgDevImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="AvgDev{T}">AvgDev</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AvgDevLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AvgDev<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AvgDevImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode AvgDevImpl<T>(
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

        var lookbackTotal = AvgDevLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        var today = startIdx;
        if (today > endIdx)
        {
            return Core.RetCode.Success;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        var outBegIdx = today;

        var outIdx = 0;
        while (today <= endIdx)
        {
            var todaySum = T.Zero;
            for (var i = 0; i < optInTimePeriod; i++)
            {
                todaySum += inReal[today - i];
            }

            var todayDev = T.Zero;
            for (var i = 0; i < optInTimePeriod; i++)
            {
                todayDev += T.Abs(inReal[today - i] - todaySum / timePeriod);
            }

            outReal[outIdx++] = todayDev / timePeriod;
            today++;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
