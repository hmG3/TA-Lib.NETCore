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
    /// Three-Line Strike (Pattern Recognition)
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
    /// Three-Line Strike function identifies a four-candle configuration that may confirm existing bullish or bearish momentum or signal
    /// an impending shift in sentiment. It begins with three consecutive candles in the same direction, followed by a fourth candle
    /// whose move counters and largely negates the gains or losses of the prior three candles.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Ensure the first three candles share the same color (all white for a bullish scenario or all black for a bearish scenario).
    ///       Each candle must successively close beyond the previous candle’s close, confirming trend acceleration.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm the fourth candle is the opposite color to the first three. For an uptrend of three white candles,
    ///       the fourth must be black. For a downtrend of three black candles, the fourth must be white.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Verify that the second and third candles' opens lie "near" the preceding candle’s real body. Here,
    ///       <em>near</em> is governed by <see cref="Core.CandleSettingType.Near">Near</see> in
    ///       <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Check that the fourth candle opens beyond the third candle's close and closes across the open of the first candle.
    ///       This "strike" reclaims much of the prior progression.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 represents a bullish Three-Line Strike pattern, signaling a potential upward continuation.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 represents a bearish Three-Line Strike pattern, signaling a potential downward continuation.
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
    public static Core.RetCode ThreeLineStrike<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ThreeLineStrikeImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="ThreeLineStrike{T}">ThreeLineStrike</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int ThreeLineStrikeLookback() => CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near) + 3;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode ThreeLineStrike<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ThreeLineStrikeImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode ThreeLineStrikeImpl<T>(
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

        var lookbackTotal = ThreeLineStrikeLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> nearPeriodTotal = new T[4];
        var nearTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[3] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 3);
            nearPeriodTotal[2] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsThreeLineStrikePattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal)
                ? (int) CandleHelpers.CandleColor(inClose, inOpen, i - 1) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 3; totIdx >= 2; --totIdx)
            {
                nearPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - totIdx);
            }

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsThreeLineStrikePattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> nearPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // three with same color
        CandleHelpers.CandleColor(inClose, inOpen, i - 3) == CandleHelpers.CandleColor(inClose, inOpen, i - 2) &&
        CandleHelpers.CandleColor(inClose, inOpen, i - 2) == CandleHelpers.CandleColor(inClose, inOpen, i - 1) &&
        // 4th: opposite color
        (int) CandleHelpers.CandleColor(inClose, inOpen, i) == -(int) CandleHelpers.CandleColor(inClose, inOpen, i - 1) &&
        // 2nd opens within/near 1st real body
        inOpen[i - 2] >= T.Min(inOpen[i - 3], inClose[i - 3]) -
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
        inOpen[i - 2] <= T.Max(inOpen[i - 3], inClose[i - 3]) +
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
        // 3rd opens within/near 2nd real body
        inOpen[i - 1] >= T.Min(inOpen[i - 2], inClose[i - 2]) -
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
        inOpen[i - 1] <= T.Max(inOpen[i - 2], inClose[i - 2]) +
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
        (
            // if three white
            CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
            // consecutive higher closes
            inClose[i - 1] > inClose[i - 2] && inClose[i - 2] > inClose[i - 3] &&
            // 4th opens above prior close
            inOpen[i] > inClose[i - 1] &&
            // 4th closes below 1st open
            inClose[i] < inOpen[i - 3]
            ||
            // if three black
            CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
            // consecutive lower closes
            inClose[i - 1] < inClose[i - 2] && inClose[i - 2] < inClose[i - 3] &&
            // 4th opens below prior close
            inOpen[i] < inClose[i - 1] &&
            // 4th closes above 1st open
            inClose[i] > inOpen[i - 3]
        );
}
