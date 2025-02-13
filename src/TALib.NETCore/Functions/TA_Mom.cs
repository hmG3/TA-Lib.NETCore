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
    /// Momentum (Momentum Indicators)
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
    /// Momentum function calculates the rate of change in price over a specified period, highlighting
    /// the speed at which prices are moving in a given direction. It is a simple but effective momentum indicator.
    /// <para>
    /// The Momentum function is not normalized, meaning its values are absolute differences and not percentage changes.
    /// Due to its non-normalized nature, it should be avoided when comparing different time series with varying price levels.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Identify the value from the specified number of periods ago:
    ///       <code>
    ///         previousValue = data[today - optInTimePeriod]
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the difference between the current value and the value from <paramref name="optInTimePeriod"/> ago:
    ///       <code>
    ///         Momentum = data[today] - previousValue
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A positive value indicates upward momentum, implying prices are rising.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A negative value indicates downward momentum, suggesting prices are falling.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value close to zero indicates little or no price movement over the specified period.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Mom<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 10) where T : IFloatingPointIeee754<T> =>
        MomImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Mom{T}">Mom</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MomLookback(int optInTimePeriod = 10) => optInTimePeriod < 1 ? -1 : optInTimePeriod;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Mom<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 10) where T : IFloatingPointIeee754<T> =>
        MomImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode MomImpl<T>(
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

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        // The Mom function is the only one who is not normalized, and thus
        // should be avoided for comparing different time series of prices.

        var lookbackTotal = MomLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Calculate Momentum. Just subtract the value from 'period' ago from current value.
        var outIdx = 0;
        var inIdx = startIdx;
        var trailingIdx = startIdx - lookbackTotal;
        while (inIdx <= endIdx)
        {
            outReal[outIdx++] = inReal[inIdx++] - inReal[trailingIdx++];
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
