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
    /// Index of highest value over a specified period (Math Operators)
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
    /// Highest Value Index function identifies the index of the highest value over a specified period.
    /// This function is useful for pinpointing the exact position of peaks in a time series within a rolling window.
    /// <para>
    /// The <see cref="Max{T}">Max</see> function can be used to get the highest value itself rather than its index.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Initialize the highest value and its index for the first period in the specified range.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       For each subsequent period, update the highest value and its index by comparing the new value with the current highest.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The output represents the index of the highest value within the rolling window defined by <paramref name="optInTimePeriod"/>.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode MaxIndex<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<int> outInteger,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        MaxIndexImpl(inReal, inRange, outInteger, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="MaxIndex{T}">MaxIndex</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MaxIndexLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MaxIndex<T>(
        T[] inReal,
        Range inRange,
        int[] outInteger,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        MaxIndexImpl<T>(inReal, inRange, outInteger, out outRange, optInTimePeriod);

    private static Core.RetCode MaxIndexImpl<T>(
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

        var lookbackTotal = MaxIndexLookback(optInTimePeriod);
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

        var highestIdx = -1;
        var highest = T.Zero;
        while (today <= endIdx)
        {
            (highestIdx, highest) = FunctionHelpers.CalcHighest(inReal, trailingIdx, today, highestIdx, highest);

            outInteger[outIdx++] = highestIdx;
            trailingIdx++;
            today++;
        }

        // Keep the outBegIdx relative to the caller input before returning.
        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
