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
    /// Chaikin A/D Line (Volume Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inVolume">A span of input volume data.</param>
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
    /// Chaikin Accumulation/Distribution Line measures the cumulative flow of money into or out of a security
    /// by considering price and volume, providing insight into underlying buying or selling pressure.
    /// <para>
    /// The function can confirm price trends and detect early signs of potential reversals.
    /// It is often employed alongside volume-based or momentum indicators to strengthen interpretation.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the Money Flow Multiplier (MFM) for each period:
    ///       <code>
    ///         MFM = ((Close - Low) - (High - Close)) / (High - Low)
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the Money Flow Volume (MFV) by multiplying the MFM by the volume:
    ///       <code>
    ///         MFV = MFM * Volume
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Cumulatively sum the MFV over time to calculate the A/D Line:
    ///       <code>
    ///         A/D Line = Cumulative Sum(MFV)
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A rising A/D Line indicates buying pressure, suggesting that demand exceeds supply.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A falling A/D Line indicates selling pressure, suggesting that supply exceeds demand.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Divergences between the A/D Line and price movement may indicate potential trend reversals or confirm trends.
    ///     </description>
    ///   </item>
    /// </list>
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
    /// Returns the lookback period for <see cref="Ad{T}">Ad</see>.
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

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length, inVolume.Length) is not
            { } rangeIndices)
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
        var outIdx = 0;
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
