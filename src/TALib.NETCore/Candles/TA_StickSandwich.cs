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
    /// Stick Sandwich (Pattern Recognition)
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
    /// Stick Sandwich function identifies a three-candle bullish reversal pattern observed in a downtrend. It consists of two
    /// black (bearish) candles with matching closing prices separated by a white (bullish) candle that gaps above the first candle's close.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Verify the first candle is black, reflecting a bearish close.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm that the second candle is white, opening and trading above the first candle's closing price (indicating a gap).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Check that the third candle is black again, with a closing price approximately the same as the first candle's close,
    ///       as defined by <see cref="Core.CandleSettingType.Equal">Equal</see> in <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 represents a Stick Sandwich pattern, indicating a potential upward trend.
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
    public static Core.RetCode StickSandwich<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        StickSandwichImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="StickSandwich{T}">StickSandwich</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int StickSandwichLookback() => CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Equal) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode StickSandwich<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        StickSandwichImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode StickSandwichImpl<T>(
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

        var lookbackTotal = StickSandwichLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var equalPeriodTotal = T.Zero;
        var equalTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Equal);
        var i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsStickSandwichPattern(inOpen, inHigh, inLow, inClose, i, equalPeriodTotal) ? 100 : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            equalPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 2);

            i++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsStickSandwichPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T equalPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st: black
        CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
        // 2nd: white
        CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        // 3rd: black
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
        // 2nd low > prior close
        inLow[i - 1] > inClose[i - 2] &&
        // 1st and 3rd same close
        inClose[i] <= inClose[i - 2] +
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 2) &&
        inClose[i] >= inClose[i - 2] -
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 2);
}
