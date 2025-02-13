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
    /// Midpoint Price over period (Overlap Studies)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
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
    /// The MidPrice indicator calculates the average of the highest high and lowest low over a specified period.
    /// It can be used to identify trends or to validate support and resistance levels.
    /// <para>
    /// It is commonly used in technical analysis to identify central price levels within a range of data. Pairing it with oscillators
    /// may highlight when price returns to or strays from its midpoint. When applied with a period of 1,
    /// this function is equivalent to <see cref="MedPrice{T}">MedPrice</see>.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Determine the highest high over the specified period:
    ///       <code>
    ///         Highest High = Max(inHigh[i] for i in range(trailingIdx, today))
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Determine the lowest low over the specified period:
    ///       <code>
    ///         Lowest Low = Min(inLow[i] for i in range(trailingIdx, today))
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the midpoint price using the formula:
    ///       <code>
    ///         MidPrice = (Highest High + Lowest Low) / 2
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The output reflects the central tendency of the price range over the period.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode MidPrice<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        MidPriceImpl(inHigh, inLow, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="MidPrice{T}">MidPrice</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MidPriceLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MidPrice<T>(
        T[] inHigh,
        T[] inLow,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        MidPriceImpl<T>(inHigh, inLow, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode MidPriceImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /* MidPrice = (Highest High + Lowest Low) / 2
         *
         * This function is equivalent to MedPrice when the period is 1.
         */

        var lookbackTotal = MidPriceLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outIdx = 0;
        var today = startIdx;
        var trailingIdx = startIdx - lookbackTotal;
        while (today <= endIdx)
        {
            var lowest = inLow[trailingIdx];
            var highest = inHigh[trailingIdx++];
            for (var i = trailingIdx; i <= today; i++)
            {
                var tmp = inLow[i];
                if (tmp < lowest)
                {
                    lowest = tmp;
                }

                tmp = inHigh[i];
                if (tmp > highest)
                {
                    highest = tmp;
                }
            }

            outReal[outIdx++] = (highest + lowest) / FunctionHelpers.Two<T>();
            today++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
