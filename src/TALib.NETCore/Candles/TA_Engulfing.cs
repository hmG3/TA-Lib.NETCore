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

public static partial class Candles
{
    /// <summary>
    /// Engulfing Pattern (Pattern Recognition)
    /// </summary>
    /// <param name="inOpen">A span of input open prices.</param>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outIntType">A span to store the output pattern type for each price bar.</param>
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
    /// Engulfing function identifies a two-candle reversal pattern where the second candle's real body fully "engulfs" the real body
    /// of the preceding candle. This pattern often signals a potential reversal when observed in an existing trend.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Identify the color of the first candle: white (bullish) or black (bearish).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Check if the second candle's real body completely overlaps the first candle's real body:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             For a bullish pattern, the second candle is white, its open is below (or at) the previous candle's close,
    ///             and its close is above (or at) the previous candle's open.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             For a bearish pattern, the second candle is black, its open is above (or at) the previous candle's close,
    ///             and its close is below (or at) the previous candle's open.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 indicates a bullish Engulfing pattern, suggesting a potential upward reversal.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 indicates a bearish Engulfing pattern, suggesting a potential downward reversal.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of 0 indicates that no pattern was detected.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Engulfing<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        EngulfingImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="Engulfing{T}">Engulfing</see>.
    /// </summary>
    /// <returns>Always 2 since there are only two prices bar required for this calculation.</returns>
    [PublicAPI]
    public static int EngulfingLookback() => 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Engulfing<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        EngulfingImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode EngulfingImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var lookbackTotal = EngulfingLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsEngulfingPattern(inOpen, inClose, i) ? (int) CandleHelpers.CandleColor(inClose, inOpen, i) * 100 : 0;

            i++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsEngulfingPattern<T>(ReadOnlySpan<T> inOpen, ReadOnlySpan<T> inClose, int i) where T : IFloatingPointIeee754<T> =>
        // white engulfs black
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
        (inClose[i] >= inOpen[i - 1] && inOpen[i] < inClose[i - 1] || inClose[i] > inOpen[i - 1] && inOpen[i] <= inClose[i - 1])
        ||
        // black engulfs white
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
        CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        (inOpen[i] >= inClose[i - 1] && inClose[i] < inOpen[i - 1] || inOpen[i] > inClose[i - 1] && inClose[i] <= inOpen[i - 1]);
}
