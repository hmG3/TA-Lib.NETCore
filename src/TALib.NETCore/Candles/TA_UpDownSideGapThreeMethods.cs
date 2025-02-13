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
    /// Upside/Downside Gap Three Methods (Pattern Recognition)
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
    /// Upside/Downside Gap Three Methods Pattern function identifies a bearish or bullish continuation pattern.
    /// This pattern typically occurs during a trend and consists of three candles where the first two are of the same color,
    /// followed by a third candle of the opposite color, with a noticeable gap between the first two candles' real bodies.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Verify that the first two candles share the same color (both white or both black).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm a gap exists between the real bodies of the first two candles:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             If the first candle is white, ensure an <em>upside</em> gap, signifying a bullish push.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             If the first candle is black, ensure a <em>downside</em> gap, emphasizing continued bearish pressure.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Check the third candle's color, which must be opposite that of the first two. Its open should lie within
    ///       the real body of the second candle, while its close should fall within the real body of the first candle,
    ///       suggesting only a partial retracement against the ongoing trend.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 indicates Upside Gap Three Methods pattern, signaling a bullish continuation.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 indicates Downside Gap Three Methods pattern, signaling a bearish continuation.
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
    public static Core.RetCode UpDownSideGapThreeMethods<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        UpDownSideGapThreeMethodsImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="UpDownSideGapThreeMethods{T}">UpDownSideGapThreeMethods</see>.
    /// </summary>
    /// <returns>Always 2 since there are only two prices bar required for this calculation.</returns>
    [PublicAPI]
    public static int UpDownSideGapThreeMethodsLookback() => 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode UpDownSideGapThreeMethods<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        UpDownSideGapThreeMethodsImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode UpDownSideGapThreeMethodsImpl<T>(
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

        var lookbackTotal = UpDownSideGapThreeMethodsLookback();
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
            outIntType[outIdx++] = IsUpDownSideGapThreeMethodsPattern(inOpen, inClose, i)
                ? (int) CandleHelpers.CandleColor(inClose, inOpen, i - 2) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            i++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsUpDownSideGapThreeMethodsPattern<T>(ReadOnlySpan<T> inOpen, ReadOnlySpan<T> inClose, int i)
        where T : IFloatingPointIeee754<T> =>
        // 1st and 2nd of same color
        CandleHelpers.CandleColor(inClose, inOpen, i - 2) == CandleHelpers.CandleColor(inClose, inOpen, i - 1) &&
        // 3rd opposite color
        (int) CandleHelpers.CandleColor(inClose, inOpen, i - 1) == -(int) CandleHelpers.CandleColor(inClose, inOpen, i) &&
        // 3rd opens within 2nd rb
        inOpen[i] < T.Max(inClose[i - 1], inOpen[i - 1]) && inOpen[i] > T.Min(inClose[i - 1], inOpen[i - 1]) &&
        // 3rd closes within 1st rb
        inClose[i] < T.Max(inClose[i - 2], inOpen[i - 2]) && inClose[i] > T.Min(inClose[i - 2], inOpen[i - 2]) &&
        (
            // when 1st is white
            CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
            // upside gap
            CandleHelpers.RealBodyGapUp(inOpen, inClose, i - 1, i - 2)
            ||
            // when 1st is black
            CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
            // downside gap
            CandleHelpers.RealBodyGapDown(inOpen, inClose, i - 1, i - 2)
        );
}
