/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2024 Anatolii Siryi
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
    /// Chaikin A/D Line (Volume Indicators)
    /// </summary>
    /// <typeparam name="T">
    /// The numeric data type, typically <see cref="float"/> or <see cref="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inVolume">A span of input volume data.</param>
    /// <param name="inRange">A range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">The span in which to store the calculated values.</param>
    /// <param name="outRange">The range of indices that represent the valid portion of data within the output span.</param>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// The function computes the Accumulation/Distribution line by calculating the cumulative flow of money into and out of the security.
    /// The formula takes into account the high, low, and close prices, as well as the volume, to assess the buying and selling pressure.
    /// This indicator can help in identifying divergences between price and volume,
    /// which may indicate potential changes in the market trend.
    /// <para>
    /// Due to the nature of cumulative calculations, using double precision is recommended for better accuracy,
    /// especially when dealing with large datasets.
    /// </para>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Ad<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        AdImpl(inHigh, inLow, inClose, inVolume, inRange, outReal, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="Ad{T}"/>.
    /// </summary>
    /// <returns>Always 0 since no historical data is required for this calculation.</returns>
    [PublicAPI]
    public static int AdLookback() => 0;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Ad<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        T[] inVolume,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        AdImpl<T>(inHigh, inLow, inClose, inVolume, inRange, outReal, out outRange);

    private static Core.RetCode AdImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length, inVolume.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        /* Results from this function might vary slightly when using float instead of double and
         * this cause a different floating-point precision to be used.
         * For most function, this is not an apparent difference but for function using large cumulative values
         * (like this AD function), minor imprecision adds up and becomes significant.
         * For better precision, use double in calculations.
         */

        var nbBar = endIdx - startIdx + 1;
        outRange = new Range(startIdx, startIdx + nbBar);
        var currentBar = startIdx;
        int outIdx = default;
        var ad = T.Zero;

        while (nbBar != 0)
        {
            ad = FunctionHelpers.CalcAccumulationDistribution(inHigh, inLow, inClose, inVolume, ref currentBar, ad);
            outReal[outIdx++] = ad;
            nbBar--;
        }

        return Core.RetCode.Success;
    }
}
