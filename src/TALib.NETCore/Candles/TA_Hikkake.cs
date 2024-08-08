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
        int patternIdx = default;
        int patternResult = default;
        var i = startIdx - 3;

        InitializeHikkake(inHigh, inLow, inClose, ref patternIdx, ref patternResult, ref i, startIdx);

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first and second candle: inside bar (2nd has lower high and higher low than 1st)
         *   - third candle: lower high and lower low than 2nd (higher high and higher low than 2nd)
         * outIntType[hikkakebar] is positive (100) or negative (-100) meaning bullish or bearish hikkake
         * Confirmation could come in the next 3 days with:
         *   - a day that closes higher than the high (lower than the low) of the 2nd candle
         * outIntType[confirmationbar] is equal to 100 + the bullish hikkake result or -100 - the bearish hikkake result
         * Note: if confirmation and a new hikkake come at the same bar, only the new hikkake is reported
         * (the new hikkake overwrites the confirmation of the old hikkake)
         */

        int outIdx = default;
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
