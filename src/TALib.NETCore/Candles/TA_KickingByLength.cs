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
    /// Kicking - bull/bear determined by the longer marubozu (Pattern Recognition)
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
    /// Kicking by Length function identifies a two-candle pattern that signals a significant reversal in market sentiment,
    /// with the bullishness or bearishness of the pattern determined by the longer of the two marubozu candlesticks.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Confirm the first candle as a <em>marubozu</em>, with its real body exceeding the average for
    ///       <see cref="Core.CandleSettingType.BodyLong">BodyLong</see> in
    ///       <see cref="Core.CandleSettings">CandleSettings</see> and its shadows remaining below the average for
    ///       <see cref="Core.CandleSettingType.ShadowVeryShort">ShadowVeryShort</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Verify that the second candle is also a <em>marubozu</em> of the opposite color, satisfying the same criteria
    ///       for <see cref="Core.CandleSettingType.BodyLong">BodyLong</see> and
    ///       <see cref="Core.CandleSettingType.ShadowVeryShort">ShadowVeryShort</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Determine the overall bullish or bearish signal based on which of the two <em>marubozu</em> candles has the
    ///       longer real body. The pattern assumes the character (bullish or bearish) of that longer candle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Check for a gap between the two candles, with an upside gap (bearish-to-bullish) or a downside gap
    ///       (bullish-to-bearish) reinforcing the implied market reversal.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 indicates a bullish Kicking by Length pattern, suggesting a potential upward price movement.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 indicates a bearish Kicking by Length pattern, suggesting a potential downward price movement.
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
    public static Core.RetCode KickingByLength<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        KickingByLengthImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="KickingByLength{T}">KickingByLength</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int KickingByLengthLookback() =>
        Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort),
            CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode KickingByLength<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        KickingByLengthImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode KickingByLengthImpl<T>(
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

        var lookbackTotal = KickingByLengthLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> shadowVeryShortPeriodTotal = new T[2];
        var shadowVeryShortTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        Span<T> bodyLongPeriodTotal = new T[2];
        var bodyLongTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[1] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal[1] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1);
            bodyLongPeriodTotal[0] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            var idx = CandleHelpers.RealBody(inClose, inOpen, i) > CandleHelpers.RealBody(inClose, inOpen, i - 1) ? i : i - 1;
            outIntType[outIdx++] =
                IsKickingByLengthPattern(inOpen, inHigh, inLow, inClose, i, bodyLongPeriodTotal, shadowVeryShortPeriodTotal)
                    ? (int) CandleHelpers.CandleColor(inClose, inOpen, idx) * 100
                    : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                bodyLongPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                        bodyLongTrailingIdx - totIdx);

                shadowVeryShortPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsKickingByLengthPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> bodyLongPeriodTotal,
        Span<T> shadowVeryShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // opposite candles
        (int) CandleHelpers.CandleColor(inClose, inOpen, i - 1) == -(int) CandleHelpers.CandleColor(inClose, inOpen, i) &&
        // 1st marubozu
        CandleHelpers.RealBody(inClose, inOpen, i - 1) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal[1], i - 1) &&
        CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i - 1) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1],
            i - 1) &&
        CandleHelpers.LowerShadow(inClose, inOpen, inLow, i - 1) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1],
            i - 1) &&
        // 2nd marubozu
        CandleHelpers.RealBody(inClose, inOpen, i) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal[0], i) &&
        CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0],
            i) &&
        CandleHelpers.LowerShadow(inClose, inOpen, inLow, i) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0],
            i) &&
        // gap
        (
            CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
            CandleHelpers.CandleGapUp(inLow, inHigh, i, i - 1)
            ||
            CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
            CandleHelpers.CandleGapDown(inLow, inHigh, i, i - 1)
        );
}
