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
    public static Core.RetCode HikkakeModified<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HikkakeModifiedImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

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

        int patternIdx = default;
        int patternResult = default;
        i = startIdx - 3;
        InitializeHikkakeModified(inOpen, inHigh, inLow, inClose, i, startIdx, ref nearPeriodTotal, ref patternResult, ref patternIdx,
            ref nearTrailingIdx);

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle
         *   - second candle: candle with range less than first candle and close near the bottom (near the top)
         *   - third candle: lower high and higher low than 2nd
         *   - fourth candle: lower high and lower low (higher high and higher low) than 3rd
         * outInteger[hikkake bar] is positive (100) or negative (-100) meaning bullish or bearish hikkake
         * Confirmation could come in the next 3 days with:
         *   - a day that closes higher than the high (lower than the low) of the 3rd candle
         * outIntType[confirmationbar] is equal to 100 + the bullish hikkake result or -100 - the bearish hikkake result
         * Note: if confirmation and a new hikkake come at the same bar, only the new hikkake is reported
         * (the new hikkake overwrites the confirmation of the old hikkake)
         * it should be considered that modified hikkake is a reversal pattern,
         * while hikkake could be both a reversal or a continuation pattern,
         * so bullish (bearish) modified hikkake is significant when appearing in a downtrend (uptrend)
         */

        int outIdx = default;
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
