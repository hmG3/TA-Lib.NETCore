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
    /// Modified Hikkake Pattern (Pattern Recognition)
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
    /// Hikkake Modified function identifies a variation of the traditional Hikkake candlestick pattern, wherein a multi-bar sequence
    /// underscores a more rigorous confirmation stage. This version commences with an inside bar and continues through a specified
    /// configuration of subsequent candles, culminating in a validated breakout or reversal signal.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Identify the second candle as an inside bar, such that its high is lower and its low is higher than the corresponding
    ///       values of the first candle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm that the third candle's range is smaller than that of the second candle. The third candle should also close
    ///       near its top (in anticipation of a bullish move) or near its bottom (in anticipation of a bearish move).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Detect a breakout in the fourth candle, which must present a lower high and lower low (bullish) or a higher high
    ///       and higher low (bearish) when compared to the third candle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Optionally, observe the next three bars for a confirming candle. A close exceeding the third candle's high (bullish)
    ///       or falling below its low (bearish) substantiates the Modified Hikkake pattern.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 represents a bullish Modified Hikkake pattern, indicating potential upward market momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 represents a bearish Modified Hikkake pattern, indicating potential downward market momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       An additional confirmation bar adds or subtracts 100 to the respective pattern value when observed within
    ///       three periods of the pattern's detection.
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
    public static Core.RetCode HikkakeModified<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HikkakeModifiedImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="HikkakeModified{T}">HikkakeModified</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int HikkakeModifiedLookback() => Math.Max(1, CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near)) + 5;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HikkakeModified<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HikkakeModifiedImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode HikkakeModifiedImpl<T>(
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

        var lookbackTotal = HikkakeModifiedLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var nearPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - 3 - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = nearTrailingIdx;
        while (i < startIdx - 3)
        {
            nearPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            i++;
        }

        var patternIdx = 0;
        var patternResult = 0;
        i = startIdx - 3;
        InitializeHikkakeModified(inOpen, inHigh, inLow, inClose, i, startIdx, ref nearPeriodTotal, ref patternResult, ref patternIdx,
            ref nearTrailingIdx);

        i = startIdx;

        var outIdx = 0;
        CalcHikkakeModified(inOpen, inHigh, inLow, inClose, outIntType, i, nearPeriodTotal, patternResult, patternIdx, ref outIdx,
            nearTrailingIdx, endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static void InitializeHikkakeModified<T>(
        ReadOnlySpan<T> open,
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        int i,
        int startIdx,
        ref T nearPeriodTotal,
        ref int patternResult,
        ref int patternIdx,
        ref int nearTrailingIdx) where T : IFloatingPointIeee754<T>
    {
        while (i < startIdx)
        {
            if (IsHikkakeModifiedPattern(open, high, low, close, i, nearPeriodTotal))
            {
                patternResult = 100 * (high[i] < high[i - 1] ? 1 : -1);
                patternIdx = i;
            }
            // search for confirmation if modified hikkake was no more than 3 bars ago
            else if (IsHikkakeModifiedPatternConfirmation(high, low, close, i, patternIdx, patternResult))
            {
                patternIdx = 0;
            }

            nearPeriodTotal +=
                CandleHelpers.CandleRange(open, high, low, close, Core.CandleSettingType.Near, i - 2) -
                CandleHelpers.CandleRange(open, high, low, close, Core.CandleSettingType.Near, nearTrailingIdx - 2);

            nearTrailingIdx++;
            i++;
        }
    }

    private static void CalcHikkakeModified<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Span<int> outIntType,
        int i,
        T nearPeriodTotal,
        int patternResult,
        int patternIdx,
        ref int outIdx,
        int nearTrailingIdx,
        int endIdx) where T : IFloatingPointIeee754<T>
    {
        var pResult = patternResult;
        var pIdx = patternIdx;
        do
        {
            if (IsHikkakeModifiedPattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal))
            {
                pResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                pIdx = i;
                outIntType[outIdx++] = pResult;
            }
            // search for confirmation if modified hikkake was no more than 3 bars ago
            else if (IsHikkakeModifiedPatternConfirmation(inHigh, inLow, inClose, i, pIdx, pResult))
            {
                outIntType[outIdx++] = pResult + 100 * (pResult > 0 ? 1 : -1);
                pIdx = 0;
            }
            else
            {
                outIntType[outIdx++] = 0;
            }

            nearPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 2);

            nearTrailingIdx++;
            i++;
        } while (i <= endIdx);
    }

    private static bool IsHikkakeModifiedPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T nearPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 2nd: lower high and higher low than 1st
        inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] &&
        // 3rd: lower high and higher low than 2nd
        inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] &&
        (
            // (bull) 4th: lower high and lower low
            inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] &&
            inClose[i - 2] <= inLow[i - 2] +
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)
            ||
            // (bear) 4th: higher high and higher low
            inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1] &&
            inClose[i - 2] >= inHigh[i - 2] -
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)
        );

    private static bool IsHikkakeModifiedPatternConfirmation<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        int patternIdx,
        int patternResult) where T : IFloatingPointIeee754<T> =>
        i <= patternIdx + 3 &&
        (
            // close higher than the high of 3rd
            patternResult > 0 && inClose[i] > inHigh[patternIdx - 1]
            ||
            // close lower than the low of 3rd
            patternResult < 0 && inClose[i] < inLow[patternIdx - 1]
        );
}
