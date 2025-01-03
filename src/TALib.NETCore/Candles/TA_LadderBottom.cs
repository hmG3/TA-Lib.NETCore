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
    public static Core.RetCode LadderBottom<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        LadderBottomImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    [PublicAPI]
    public static int LadderBottomLookback() => CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort) + 4;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode LadderBottom<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        LadderBottomImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode LadderBottomImpl<T>(
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

        var lookbackTotal = LadderBottomLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var shadowVeryShortPeriodTotal = T.Zero;
        var shadowVeryShortTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        var i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - three black candlesticks with consecutively lower opens and closes
         *   - fourth candle: black candle with an upper shadow (it's supposed to be not very short)
         *   - fifth candle: white candle that opens above prior candle's body and closes above prior candle's high
         * The meaning of "very short" is specified with CandleSettings
         * outIntType is positive (100): ladder bottom is always bullish
         * it should be considered that ladder bottom is significant when it appears in a downtrend,
         * while this function does not consider it
         */

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsLadderBottomPattern(inOpen, inHigh, inLow, inClose, i, shadowVeryShortPeriodTotal) ? 100 : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            shadowVeryShortPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                    shadowVeryShortTrailingIdx - 1);

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsLadderBottomPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T shadowVeryShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 3 black candlesticks
        CandleHelpers.CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.Black &&
        CandleHelpers.CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
        CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
        // with consecutively lower opens
        inOpen[i - 4] > inOpen[i - 3] && inOpen[i - 3] > inOpen[i - 2] &&
        // and closes
        inClose[i - 4] > inClose[i - 3] && inClose[i - 3] > inClose[i - 2] &&
        // 4th: black with an upper shadow
        CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
        CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i - 1) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal,
            i - 1) &&
        // 5th: white
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        // that opens above prior candle's body
        inOpen[i] > inOpen[i - 1] &&
        // and closes above prior candle's high
        inClose[i] > inHigh[i - 1];
}
