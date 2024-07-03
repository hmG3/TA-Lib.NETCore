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
    public static Core.RetCode LadderBottom<T>(
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

        var lookbackTotal = LadderBottomLookback();
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
        var shadowVeryShortPeriodTotal = T.Zero;
        var shadowVeryShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        var i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - three black candlesticks with consecutively lower opens and closes
         *   - fourth candle: black candle with an upper shadow (it's supposed to be not very short)
         *   - fifth candle: white candle that opens above prior candle's body and closes above prior candle's high
         * The meaning of "very short" is specified with CandleSettings
         * outInteger is positive (1 to 100): ladder bottom is always bullish;
         * the user should consider that ladder bottom is significant when it appears in a downtrend,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outInteger[outIdx++] = IsLadderBottomPattern(inOpen, inHigh, inLow, inClose, i, shadowVeryShortPeriodTotal) ? 100 : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            shadowVeryShortPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx - 1);

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int LadderBottomLookback() => CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort) + 4;

    private static bool IsLadderBottomPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T shadowVeryShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 3 black candlesticks
        CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.Black &&
        CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
        CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
        // with consecutively lower opens
        inOpen[i - 4] > inOpen[i - 3] && inOpen[i - 3] > inOpen[i - 2] &&
        // and closes
        inClose[i - 4] > inClose[i - 3] && inClose[i - 3] > inClose[i - 2] &&
        // 4th: black with an upper shadow
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
        UpperShadow(inHigh, inClose, inOpen, i - 1) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i - 1) &&
        // 5th: white
        CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        // that opens above prior candle's body
        inOpen[i] > inOpen[i - 1] &&
        // and closes above prior candle's high
        inClose[i] > inHigh[i - 1];

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode LadderBottom<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        LadderBottom<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
