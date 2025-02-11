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

public static partial class Candles
{
    /// <summary>
    /// Tasuki Gap (Pattern Recognition)
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
    /// Tasuki Gap function identifies a three-candle continuation pattern suggesting the market will persist in its current trend
    /// (bullish or bearish). This pattern features a gap that remains unfilled by the subsequent candle, backed up by two candles
    /// whose similar real-body sizes confirm the existing directional bias.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Verify that a gap forms between the first candle and the prior candle (either an upside or downside gap).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm that the second candle continues in the direction of the gap:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>White (bullish) for an upside gap.</description>
    ///         </item>
    ///         <item>
    ///           <description>Black (bearish) for a downside gap.</description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Check that the third candle is of the opposite color, opens within the second candle's real body, and closes within,
    ///       but does not fill, the gap.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Ensure the second and third candles have real bodies of similar size, adhering to
    ///       <see cref="Core.CandleSettingType.Near">Near</see> in <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 represents a bullish Tasuki Gap pattern, signaling a continuation of an upward trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 represents a bearish Tasuki Gap pattern, signaling a continuation of a downward trend.
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
    public static Core.RetCode TasukiGap<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        TasukiGapImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="TasukiGap{T}">TasukiGap</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int TasukiGapLookback() => CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode TasukiGap<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        TasukiGapImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode TasukiGapImpl<T>(
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

        var lookbackTotal = TasukiGapLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var nearPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsTasukiGapPattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal)
                ? (int) CandleHelpers.CandleColor(inClose, inOpen, i - 1) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            nearPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsTasukiGapPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T nearPeriodTotal) where T : IFloatingPointIeee754<T> =>
        (
            // upside gap
            CandleHelpers.RealBodyGapUp(inOpen, inClose, i - 1, i - 2) &&
            // 1st: white
            CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
            // 2nd: black
            CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
            // that opens within the white real body
            inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] &&
            // and closes under the white real body
            inClose[i] < inOpen[i - 1] &&
            // inside the gap
            inClose[i] > T.Max(inClose[i - 2], inOpen[i - 2]) &&
            // size of 2 real body near the same
            T.Abs(CandleHelpers.RealBody(inClose, inOpen, i - 1) - CandleHelpers.RealBody(inClose, inOpen, i)) <
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
        )
        ||
        (
            // downside gap
            CandleHelpers.RealBodyGapDown(inOpen, inClose, i - 1, i - 2) &&
            // 1st: black
            CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
            // 2nd: white
            CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
            // that opens within the black rb
            inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
            // and closes above the black rb
            inClose[i] > inOpen[i - 1] &&
            // inside the gap
            inClose[i] < T.Min(inClose[i - 2], inOpen[i - 2]) &&
            // size of 2 real body near the same
            T.Abs(CandleHelpers.RealBody(inClose, inOpen, i - 1) - CandleHelpers.RealBody(inClose, inOpen, i)) <
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
        );
}
