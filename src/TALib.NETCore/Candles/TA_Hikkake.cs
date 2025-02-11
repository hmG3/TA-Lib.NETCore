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
    /// Hikkake Pattern (Pattern Recognition)
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
    /// Hikkake function identifies the "Hikkake" candlestick pattern, a multi-bar formation noted for potential trend
    /// reversals or continuation signals. The pattern commences with an inside bar configuration, followed by a third candle
    /// that appears to break in one direction before ultimately reversing course.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Examine the first two candles to confirm an inside bar, where the second candle's high is lower than the first candle's
    ///       high, and its low is higher than the first candle's low.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Detect a breakout with the third candle, which must exhibit either a lower high and lower low (suggesting a bullish setup)
    ///       or a higher high and higher low (suggesting a bearish setup) in comparison to the second candle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       (Optional) Within three periods of the detected pattern, look for a confirmation bar that closes above the second
    ///       candle's high (bullish) or below its low (bearish), thereby confirming the Hikkake pattern.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 represents a bullish Hikkake pattern, indicating potential upward market momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 represents a bearish Hikkake pattern, indicating potential downward market momentum.
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
    public static Core.RetCode Hikkake<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HikkakeImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="Hikkake{T}">Hikkake</see>.
    /// </summary>
    /// <returns>Always 5 since there are only five prices bar required for this calculation.</returns>
    [PublicAPI]
    public static int HikkakeLookback() => 5;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Hikkake<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HikkakeImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode HikkakeImpl<T>(
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

        var lookbackTotal = HikkakeLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var patternIdx = 0;
        var patternResult = 0;
        var i = startIdx - 3;

        InitializeHikkake(inHigh, inLow, inClose, ref patternIdx, ref patternResult, ref i, startIdx);

        i = startIdx;

        var outIdx = 0;
        CalcHikkake(inHigh, inLow, inClose, endIdx, ref i, ref outIdx, outIntType, ref patternIdx, ref patternResult);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static void InitializeHikkake<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        ref int patternIdx,
        ref int patternResult,
        ref int i,
        int startIdx) where T : IFloatingPointIeee754<T>
    {
        while (i < startIdx)
        {
            if (IsHikkakePattern(high, low, i))
            {
                patternResult = 100 * (high[i] < high[i - 1] ? 1 : -1);
                patternIdx = i;
            }
            // search for confirmation if hikkake was no more than 3 bars ago
            else if (IsHikkakePatternConfirmation(high, low, close, i, patternIdx, patternResult))
            {
                patternIdx = 0;
            }

            i++;
        }
    }

    private static void CalcHikkake<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int endIdx,
        ref int i,
        ref int outIdx,
        Span<int> outIntType,
        ref int patternIdx,
        ref int patternResult) where T : IFloatingPointIeee754<T>
    {
        do
        {
            if (IsHikkakePattern(inHigh, inLow, i))
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
                outIntType[outIdx++] = patternResult;
            }
            // search for confirmation if hikkake was no more than 3 bars ago
            else if (IsHikkakePatternConfirmation(inHigh, inLow, inClose, i, patternIdx, patternResult))
            {
                outIntType[outIdx++] = patternResult + 100 * (patternResult > 0 ? 1 : -1);
                patternIdx = 0;
            }
            else
            {
                outIntType[outIdx++] = 0;
            }

            i++;
        } while (i <= endIdx);
    }

    private static bool IsHikkakePattern<T>(ReadOnlySpan<T> inHigh, ReadOnlySpan<T> inLow, int i) where T : IFloatingPointIeee754<T> =>
        // 1st + 2nd: lower high and higher low
        inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] &&
        (
            // (bull) 3rd: lower high and lower low
            inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1]
            ||
            // (bear) 3rd: higher high and higher low
            inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1]
        );

    private static bool IsHikkakePatternConfirmation<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        int patternIdx,
        int patternResult) where T : IFloatingPointIeee754<T> =>
        i <= patternIdx + 3 &&
        (
            // close higher than the high of 2nd
            patternResult > 0 && inClose[i] > inHigh[patternIdx - 1]
            ||
            // close lower than the low of 2nd
            patternResult < 0 && inClose[i] < inLow[patternIdx - 1]
        );
}
