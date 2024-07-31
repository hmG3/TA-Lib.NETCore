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
    public static Core.RetCode Takuri<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        TakuriImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    [PublicAPI]
    public static int TakuriLookback() =>
        Math.Max(
            Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyDoji), CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort)),
            CandleAveragePeriod(Core.CandleSettingType.ShadowVeryLong));

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Takuri<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        TakuriImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode TakuriImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var lookbackTotal = TakuriLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var bodyDojiPeriodTotal = T.Zero;
        var bodyDojiTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        var shadowVeryShortPeriodTotal = T.Zero;
        var shadowVeryShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        var shadowVeryLongPeriodTotal = T.Zero;
        var shadowVeryLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryLong);
        var i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = shadowVeryLongTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, i);
            i++;
        }

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - doji body
         *   - open and close at the high of the day = no or very short upper shadow
         *   - very long lower shadow
         * The meaning of "doji", "very short" and "very long" is specified with CandleSettings
         * outIntType is always positive (100) but this does not mean it is bullish: takuri must be considered relatively to the trend
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsTakuriPattern(inOpen, inHigh, inLow, inClose, i, bodyDojiPeriodTotal, shadowVeryShortPeriodTotal,
                shadowVeryLongPeriodTotal)
                ? 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyDojiPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);

            shadowVeryShortPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);

            shadowVeryLongPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, shadowVeryLongTrailingIdx);

            i++;
            bodyDojiTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            shadowVeryLongTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsTakuriPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyDojiPeriodTotal,
        T shadowVeryShortPeriodTotal,
        T shadowVeryLongPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // doji boy
        RealBody(inClose, inOpen, i) <=
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) &&
        // very short upper shadow
        UpperShadow(inHigh, inClose, inOpen, i) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) &&
        // very long lower shadow
        LowerShadow(inClose, inOpen, inLow, i) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, shadowVeryLongPeriodTotal, i);
}
