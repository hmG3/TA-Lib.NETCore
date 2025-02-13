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
    /// Median Price (Price Transform)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Median Price function calculates the average of the high and low prices for each data point in the range.
    /// This function provides a simple representation of the midpoint of the price bar for the given time period.
    /// <para>
    /// It is commonly used as a base value for further calculations or as a smoothing input to reduce price noise.
    /// The function operates on a single price bar (current high and low) rather than aggregating values over multiple bars.
    /// For calculations involving multiple price bars (e.g., highest high and lowest low), consider using
    /// <see cref="MidPrice{T}">MidPrice</see> function instead.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       For each data point within the specified range, sum the <paramref name="inHigh"/> and <paramref name="inLow"/> prices.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Divide the sum by 2 to compute the median price for the current price bar.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The output provides a simplified view of the central value for each price bar.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode MedPrice<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        MedPriceImpl(inHigh, inLow, inRange, outReal, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="MedPrice{T}">MedPrice</see>.
    /// </summary>
    /// <returns>Always 0 since no historical data is required for this calculation.</returns>
    [PublicAPI]
    public static int MedPriceLookback() => 0;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MedPrice<T>(
        T[] inHigh,
        T[] inLow,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        MedPriceImpl<T>(inHigh, inLow, inRange, outReal, out outRange);

    private static Core.RetCode MedPriceImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        /* MedPrice = (High + Low ) / 2
         * This is the high and low of the same price bar.
         *
         * See MidPrice to use instead the highest high and lowest low over multiple price bar.
         */

        var outIdx = 0;
        for (var i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = (inHigh[i] + inLow[i]) / FunctionHelpers.Two<T>();
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
