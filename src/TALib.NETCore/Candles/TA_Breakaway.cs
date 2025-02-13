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
    /// Breakaway (Pattern Recognition)
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
    /// Breakaway function identifies a five-candle reversal pattern characterized by a notable gap in price action and a subsequent trend
    /// reversal on the fifth candle. It can signal a potential shift in market momentum, either bullish or bearish, depending on the
    /// configuration of the candles.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Confirm that the first candle is <em>long</em>, meaning its real body exceeds the average length specified by
    ///       <see cref="Core.CandleSettingType.BodyLong">BodyLong</see> in <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The second candle forms a gap:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             In a bearish pattern, the second candle gaps downward relative to the first.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             In a bullish pattern, the second candle gaps upward relative to the first.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The third and fourth candles continue the direction implied by the gap:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             In a bearish pattern, each candle's high and low are lower than those of the preceding candle.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             In a bullish pattern, each candle's high and low are higher than those of the preceding candle.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The fifth candle reverses the trend by closing within the gap created between the first and second candles.
    ///       The color of the fifth candle is opposite to the first candle.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 indicates a bullish Breakaway pattern, signaling potential upward momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 indicates a bearish Breakaway pattern, signaling potential downward momentum.
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
    public static Core.RetCode Breakaway<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        BreakawayImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="Breakaway{T}">Breakaway</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int BreakawayLookback() => CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong) + 4;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Breakaway<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        BreakawayImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode BreakawayImpl<T>(
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

        var lookbackTotal = BreakawayLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var bodyLongPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsBreakawayPattern(inOpen, inHigh, inLow, inClose, i, bodyLongPeriodTotal)
                ? (int) CandleHelpers.CandleColor(inClose, inOpen, i) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyLongPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);

            i++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsBreakawayPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyLongPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st long
        CandleHelpers.RealBody(inClose, inOpen, i - 4) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 4) &&
        // 1st, 2nd, 4th same color, 5th opposite
        CandleHelpers.CandleColor(inClose, inOpen, i - 4) == CandleHelpers.CandleColor(inClose, inOpen, i - 3) &&
        CandleHelpers.CandleColor(inClose, inOpen, i - 3) == CandleHelpers.CandleColor(inClose, inOpen, i - 1) &&
        (int) CandleHelpers.CandleColor(inClose, inOpen, i - 1) == -(int) CandleHelpers.CandleColor(inClose, inOpen, i) &&
        (
            // when 1st is black:
            CandleHelpers.CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.Black &&
            // 2nd gaps down
            CandleHelpers.RealBodyGapDown(inOpen, inClose, i - 3, i - 4) &&
            // 3rd has lower high and low than 2nd
            inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] &&
            // 4th has lower high and low than 3rd
            inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] < inLow[i - 2] &&
            // 5th closes inside the gap
            inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4]
            ||
            // when 1st is white:
            CandleHelpers.CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.White &&
            // 2nd gaps up
            CandleHelpers.RealBodyGapUp(inClose, inOpen, i - 3, i - 4) &&
            // 3rd has higher high and low than 2nd
            inHigh[i - 2] > inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] &&
            // 4th has higher high and low than 3rd
            inHigh[i - 1] > inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] &&
            // 5th closes inside the gap
            inClose[i] < inOpen[i - 3] && inClose[i] > inClose[i - 4]
        );
}
