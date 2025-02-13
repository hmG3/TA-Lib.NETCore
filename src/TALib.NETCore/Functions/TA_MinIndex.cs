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
    /// Index of lowest value over a specified period (Math Operators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outInteger">A span to store the calculated index values.</param>
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
    /// MinIndex function calculates the index of the lowest value in a data series over a specified period.
    /// It is typically used in technical analysis to determine where the minimum value occurred in a rolling window of data.
    /// <para>
    /// The <see cref="Min{T}">Min</see> function can be used to get the lowest value itself rather than its index.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Identify the range of indices to evaluate for the lowest value based on the input range and time period:
    ///       <code>
    ///         Range = [trailingIdx, today]
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Determine the index of the lowest value in the range:
    ///       <code>
    ///         LowestIndex = IndexOfMin(inReal[i] for i in Range)
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The output provides the relative index within the input range where the lowest value was found during each time window.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode MinIndex<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<int> outInteger,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        MinIndexImpl(inReal, inRange, outInteger, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="MinIndex{T}">MinIndex</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MinIndexLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MinIndex<T>(
        T[] inReal,
        Range inRange,
        int[] outInteger,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        MinIndexImpl<T>(inReal, inRange, outInteger, out outRange, optInTimePeriod);

    private static Core.RetCode MinIndexImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<int> outInteger,
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

        var lookbackTotal = MinIndexLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Proceed with the calculation for the requested range.
        // The algorithm allows the input and output to be the same buffer.
        var outIdx = 0;
        var today = startIdx;
        var trailingIdx = startIdx - lookbackTotal;

        var lowestIdx = -1;
        var lowest = T.Zero;
        while (today <= endIdx)
        {
            (lowestIdx, lowest) = FunctionHelpers.CalcLowest(inReal, trailingIdx, today, lowestIdx, lowest);

            outInteger[outIdx++] = lowestIdx;
            trailingIdx++;
            today++;
        }

        // Keep the outBegIdx relative to the caller input before returning.
        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
