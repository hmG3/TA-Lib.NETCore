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
    /// Three Black Crows (Pattern Recognition)
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
    /// Three Black Crows function identifies a bearish reversal formation, which commonly appears after a bullish or neutral market phase.
    /// This pattern is highlighted by three successive black (bearish) candlesticks, each featuring a small or negligible lower shadow
    /// and opening within the body of the preceding black candle.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Check that the pattern begins with a white (bullish) candle, followed by three consecutive black (bearish) candles.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm each black candle has a <em>very short</em> lower shadow, per the threshold for
    ///       <see cref="Core.CandleSettingType.ShadowVeryShort">ShadowVeryShort</see> in
    ///       <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Verify the second black candle opens within the first black candle's real body and the third black candle
    ///       opens within the second black candle's real body.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Establish that the closing prices of these three black candles consistently decline.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of -100 represents a Three Black Crows pattern, signaling a potential bearish reversal.
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
    public static Core.RetCode ThreeBlackCrows<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ThreeBlackCrowsImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="ThreeBlackCrows{T}">ThreeBlackCrows</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int ThreeBlackCrowsLookback() => CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort) + 3;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode ThreeBlackCrows<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ThreeBlackCrowsImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode ThreeBlackCrowsImpl<T>(
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

        var lookbackTotal = ThreeBlackCrowsLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> shadowVeryShortPeriodTotal = new T[3];
        var shadowVeryShortTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        var i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[2] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsThreeBlackCrowsPattern(inOpen, inHigh, inLow, inClose, i, shadowVeryShortPeriodTotal) ? -100 : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 2; totIdx >= 0; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsThreeBlackCrowsPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> shadowVeryShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // white
        CandleHelpers.CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.White &&
        // 1st: black
        CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
        // very short lower shadow
        CandleHelpers.LowerShadow(inClose, inOpen, inLow, i - 2) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2],
            i - 2) &&
        // 2nd: black
        CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
        // very short lower shadow
        CandleHelpers.LowerShadow(inClose, inOpen, inLow, i - 1) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1],
            i - 1) &&
        // 3rd: black
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
        // very short lower shadow
        CandleHelpers.LowerShadow(inClose, inOpen, inLow, i) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0],
            i) &&
        // 2nd black opens within 1st black's real body
        inOpen[i - 1] < inOpen[i - 2] && inOpen[i - 1] > inClose[i - 2] &&
        // 3rd black opens within 2nd black's real body
        inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
        // 1st black closes under prior candle's high
        inHigh[i - 3] > inClose[i - 2] &&
        // three declining
        inClose[i - 2] > inClose[i - 1] &&
        // three declining
        inClose[i - 1] > inClose[i];
}
