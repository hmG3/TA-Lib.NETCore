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
    /// True Range (Volatility Indicators)
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
    /// TRANGE calculates the maximum of several range metrics, serving as a fundamental measure in volatility indicators like ATR.
    /// <para>
    /// The function alone measures volatility. Using it as input for <see cref="Atr{T}">ATR</see> or
    /// other volatility-based strategies can improve adjustments to changing market conditions.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Determine the current high-low difference: <c>val1 = High[today] - Low[today]</c>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the difference between yesterday's close and today's high: <c>val2 = |High[today] - Close[yesterday]|</c>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the difference between yesterday's close and today's low: <c>val3 = |Low[today] - Close[yesterday]|</c>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Select the largest of these three values: <c>TrueRange = Max(val1, val2, val3)</c>.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A higher value indicates higher market volatility.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A lower value indicates a more stable or less volatile market.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode TRange<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        TRangeImpl(inHigh, inLow, inClose, inRange, outReal, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="TRange{T}">TRange</see>.
    /// </summary>
    /// <returns>Always 1 since there is only one price bar required for this calculation.</returns>
    [PublicAPI]
    public static int TRangeLookback() => 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode TRange<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        TRangeImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange);

    private static Core.RetCode TRangeImpl<T>(
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

        /* True Range is the greatest of the following:
         *
         *   val1 = distance from today's high to today's low.
         *   val2 = distance from yesterday's close to today's high.
         *   val3 = distance from yesterday's close to today's low.
         *
         * Some books and software makes the first TR value to be the (high - low) of the first bar.
         * This function instead ignores the first price bar, and only outputs starting at the second price bar are valid.
         * This is done for avoiding inconsistency.
         */

        var lookbackTotal = TRangeLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outIdx = 0;
        var today = startIdx;
        while (today <= endIdx)
        {
            var tempHT = inHigh[today];
            var tempLT = inLow[today];
            var tempCY = inClose[today - 1];

            outReal[outIdx++] = FunctionHelpers.TrueRange(tempHT, tempLT, tempCY);
            today++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
