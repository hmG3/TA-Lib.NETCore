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
    /// Simple Moving Average (Overlap Studies)
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
    /// Simple Moving Average is a basic moving average calculation that smooths data over a specified time period
    /// by calculating the unweighted average of the data points within that period.
    /// <para>
    /// SMA is a lagging indicator, meaning it reacts to past price changes. Its simplicity and effectiveness make it a cornerstone
    /// of technical analysis, often used to identify support and resistance levels, confirm trends, and generate trade signals.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Identify the specified time period (<paramref name="optInTimePeriod"/>) over which the average will be calculated.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the sum of the data points within the time period.
    ///       <code>
    ///         Sum = data[t] + data[t-1] + ... + data[t-(optInTimePeriod-1)]
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Divide the sum by the number of periods to calculate the average:
    ///       <code>
    ///         SMA = Sum / optInTimePeriod
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Slide the time window forward by one period, repeating the calculation for subsequent data points to produce the SMA series.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       An upward-sloping SMA indicates a positive trend, with prices consistently rising over the specified period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A downward-sloping SMA indicates a negative trend, with prices consistently falling over the specified period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       SMA crossovers (e.g., short-term SMA crossing above or below a long-term SMA) are commonly used as buy or sell signals.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The SMA provides support and resistance levels where price action might reverse or consolidate.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Sma<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        SmaImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Sma{T}">Sma</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int SmaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Sma<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        SmaImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode SmaImpl<T>(
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

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        return FunctionHelpers.CalcSimpleMA(inReal, new Range(rangeIndices.startIndex, rangeIndices.endIndex), outReal, out outRange,
            optInTimePeriod);
    }
}
