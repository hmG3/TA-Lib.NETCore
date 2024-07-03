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
    public static Core.RetCode ConcealingBabySwallow<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outInteger,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = ConcealingBabySwallowLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> shadowVeryShortPeriodTotal = new T[4];
        var shadowVeryShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        var i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[3] += CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 3);
            shadowVeryShortPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 1);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle: black marubozu (very short shadows)
         *   - second candle: black marubozu (very short shadows)
         *   - third candle: black candle that opens gapping down but has an upper shadow that extends into the prior body
         *   - fourth candle: black candle that completely engulfs the third candle, including the shadows
         * The meanings of "very short shadow" are specified with CandleSettings;
         * outInteger is positive (1 to 100): concealing baby swallow is always bullish;
         * the user should consider that concealing baby swallow is significant when it appears in downtrend,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outInteger[outIdx++] = IsConcealingBabySwallowPattern(inOpen, inHigh, inLow, inClose, i, shadowVeryShortPeriodTotal) ? 100 : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int ConcealingBabySwallowLookback() => CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort) + 3;

    private static bool IsConcealingBabySwallowPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> shadowVeryShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st black
        CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
        // 2nd black
        CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
        // 3rd black
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
        // 4th black
        CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
        // 1st: marubozu
        LowerShadow(inClose, inOpen, inLow, i - 3) <
        CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3], i - 3) &&
        UpperShadow(inHigh, inClose, inOpen, i - 3) <
        CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3], i - 3) &&
        // 2nd: marubozu
        LowerShadow(inClose, inOpen, inLow, i - 2) <
        CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
        UpperShadow(inHigh, inClose, inOpen, i - 2) <
        CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
        // 3rd: opens gapping down
        RealBodyGapDown(inOpen, inClose, i - 1, i - 2) &&
        // and has an upper shadow
        UpperShadow(inHigh, inClose, inOpen, i - 1) >
        CandleAverage(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
        // that extends into the prior body
        inHigh[i - 1] > inClose[i - 2] &&
        // 4th: engulfs the 3rd including the shadows
        inHigh[i] > inHigh[i - 1] && inLow[i] < inLow[i - 1];

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode ConcealingBabySwallow<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        ConcealingBabySwallow<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
