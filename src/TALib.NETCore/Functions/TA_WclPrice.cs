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
    /// Weighted Close Price (Price Transform)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
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
    /// Weighted Close Price is a price transform that provides a weighted average of the high, low,
    /// and close prices of a financial instrument, giving double weight to the close price.
    /// <para>
    /// This function emphasizes the closing price, which is often considered the most significant price of a trading period.
    /// It enhances analysis of closing dynamics and may highlight patterns that other transforms overlook.
    /// Pairing it with momentum or trend indicators highlights when the market consistently closes near certain levels.
    /// Pairing it with momentum indicators like <see cref="Rsi{T}">RSI</see> or trend-following tools
    /// such as <see cref="Macd{T}">MACD</see> enhances its utility.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       For each period, compute the Weighted Close Price using the formula:
    ///       <code>
    ///         WCL = (High + Low + 2 * Close) / 4.
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode WclPrice<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        WclPriceImpl(inHigh, inLow, inClose, inRange, outReal, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="WclPrice{T}">WclPrice</see>.
    /// </summary>
    /// <returns>Always 0 since no historical data is required for this calculation.</returns>
    [PublicAPI]
    public static int WclPriceLookback() => 0;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode WclPrice<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        WclPriceImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange);

    private static Core.RetCode WclPriceImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var outIdx = 0;
        for (var i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = (inHigh[i] + inLow[i] + inClose[i] * FunctionHelpers.Two<T>()) / FunctionHelpers.Four<T>();
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
