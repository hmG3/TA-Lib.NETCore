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
    /// Concealing Baby Swallow (Pattern Recognition)
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
    /// Concealing Baby Swallow function identifies a four-candle bullish reversal pattern generally observed during
    /// a downtrend. The pattern is characterized by two strong bearish candles followed by a sequence of two additional candles showing
    /// signs of reversal. This pattern indicates a potential shift in market sentiment.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       The first and second candles must be black Marubozu, meaning they have almost no upper or lower shadows, as specified by
    ///       <see cref="Core.CandleSettingType.ShadowVeryShort">ShadowVeryShort</see> in <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The third candle opens with a gap down from the second candle and has an upper shadow that penetrates the real body
    ///       of the second candle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The fourth candle fully engulfs the third candle, including its shadows.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 indicates a bullish Concealing Baby Swallow pattern, signaling potential upward momentum.
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
    public static Core.RetCode ConcealingBabySwallow<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ConcealingBabySwallowImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="ConcealingBabySwallow{T}">ConcealingBabySwallow</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int ConcealingBabySwallowLookback() => CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort) + 3;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode ConcealingBabySwallow<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ConcealingBabySwallowImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode ConcealingBabySwallowImpl<T>(
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

        var lookbackTotal = ConcealingBabySwallowLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> shadowVeryShortPeriodTotal = new T[4];
        var shadowVeryShortTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        var i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[3] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 3);
            shadowVeryShortPeriodTotal[2] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 1);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsConcealingBabySwallowPattern(inOpen, inHigh, inLow, inClose, i, shadowVeryShortPeriodTotal)
                ? 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsConcealingBabySwallowPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> shadowVeryShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st black
        CandleHelpers.CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
        // 2nd black
        CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
        // 3rd black
        CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
        // 4th black
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
        // 1st: marubozu
        CandleHelpers.LowerShadow(inClose, inOpen, inLow, i - 3) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3],
            i - 3) &&
        CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i - 3) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3],
            i - 3) &&
        // 2nd: marubozu
        CandleHelpers.LowerShadow(inClose, inOpen, inLow, i - 2) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2],
            i - 2) &&
        CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i - 2) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2],
            i - 2) &&
        // 3rd: opens gapping down
        CandleHelpers.RealBodyGapDown(inOpen, inClose, i - 1, i - 2) &&
        // and has an upper shadow
        CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i - 1) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1],
            i - 1) &&
        // that extends into the prior body
        inHigh[i - 1] > inClose[i - 2] &&
        // 4th: engulfs the 3rd including the shadows
        inHigh[i] > inHigh[i - 1] && inLow[i] < inLow[i - 1];
}
