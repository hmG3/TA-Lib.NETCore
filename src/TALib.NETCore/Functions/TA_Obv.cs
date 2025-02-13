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
    /// On Balance Volume (Volume Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inVolume">A span of input volume values.</param>
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
    /// On Balance Volume function calculates a cumulative measure of volume flow to determine whether volume is flowing into
    /// or out of a security. This indicator is based on the principle that volume precedes price movements.
    /// <para>
    /// The function is most useful when combined with other indicators or patterns, as it alone does not provide
    /// definitive buy or sell signals. Large spikes in volume can cause significant changes in OBV, potentially distorting trends.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Initialize the `OBV` with the first volume value in the range.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       For each subsequent price value:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             If today's price is greater than yesterday's price, add today's volume to the `OBV`.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             If today's price is less than yesterday's price, subtract today's volume from the `OBV`.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             If today's price equals yesterday's price, the `OBV` remains unchanged.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Store the cumulative `OBV` value for each period in the output span.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A rising value indicates positive volume flow, suggesting accumulation.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A falling value indicates negative volume flow, suggesting distribution.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Sideways movement in the OBV suggests no significant accumulation or distribution.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Obv<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inVolume,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ObvImpl(inReal, inVolume, inRange, outReal, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="Obv{T}">Obv</see>.
    /// </summary>
    /// <returns>Always 0 since no historical data is required for this calculation.</returns>
    [PublicAPI]
    public static int ObvLookback() => 0;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Obv<T>(
        T[] inReal,
        T[] inVolume,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ObvImpl<T>(inReal, inVolume, inRange, outReal, out outRange);

    private static Core.RetCode ObvImpl<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inVolume,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length, inVolume.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var prevOBV = inVolume[startIdx];
        var prevReal = inReal[startIdx];
        var outIdx = 0;

        for (var i = startIdx; i <= endIdx; i++)
        {
            var tempReal = inReal[i];
            if (tempReal > prevReal)
            {
                prevOBV += inVolume[i];
            }
            else if (tempReal < prevReal)
            {
                prevOBV -= inVolume[i];
            }

            outReal[outIdx++] = prevOBV;
            prevReal = tempReal;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
