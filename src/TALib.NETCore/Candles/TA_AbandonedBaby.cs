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
    public static Core.RetCode AbandonedBaby<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange,
        double optInPenetration = 0.3) where T : IFloatingPointIeee754<T> =>
        AbandonedBabyImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    [PublicAPI]
    public static int AbandonedBabyLookback() =>
        Math.Max(
            Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyDoji), CandleAveragePeriod(Core.CandleSettingType.BodyLong)),
            CandleAveragePeriod(Core.CandleSettingType.BodyShort)
        ) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AbandonedBaby<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange,
        double optInPenetration = 0.3) where T : IFloatingPointIeee754<T> =>
        AbandonedBabyImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    private static Core.RetCode AbandonedBabyImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange,
        double optInPenetration) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInPenetration < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = AbandonedBabyLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var bodyLongPeriodTotal = T.Zero;
        var bodyDojiPeriodTotal = T.Zero;
        var bodyShortPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - 2 - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var bodyDojiTrailingIdx = startIdx - 1 - CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        var bodyShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        var i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyDojiPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyShortPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle: long white (black) real body
         *   - second candle: doji
         *   - third candle: black (white) real body that moves well within the first candle's real body
         *   - upside (downside) gap between the first candle and the doji (the shadows of the two candles don't touch)
         *   - downside (upside) gap between the doji and the third candle (the shadows of the two candles don't touch)
         * The meaning of "doji" and "long" is specified with CandleSettings
         * The meaning of "moves well within" is specified with optInPenetration and "moves" should mean
         * the real body should not be short ("short" is specified with CandleSettings) -
         * Greg Morris wants it to be long, someone else wants it to be relatively long
         * outIntType is positive (100) when it's an abandoned baby bottom or negative (-100) when it's an abandoned baby top
         * it should be considered that an abandoned baby is significant when it appears in an uptrend or downtrend,
         * while this function does not consider the trend
         */

        int outIdx = default;
        var penetration = T.CreateChecked(optInPenetration);
        do
        {
            outIntType[outIdx++] = IsAbandonedBabyPattern(inOpen, inHigh, inLow, inClose, i, bodyLongPeriodTotal, bodyDojiPeriodTotal,
                bodyShortPeriodTotal, penetration)
                ? (int) CandleColor(inClose, inOpen, i) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyLongPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);

            bodyDojiPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i - 1) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);

            bodyShortPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);

            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsAbandonedBabyPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyLongPeriodTotal,
        T bodyDojiPeriodTotal,
        T bodyShortPeriodTotal,
        T penetration) where T : IFloatingPointIeee754<T> =>
        // 1st: long
        RealBody(inClose, inOpen, i - 2) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 2) &&
        // 2nd: doji
        RealBody(inClose, inOpen, i - 1) <=
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i - 1) &&
        // 3rd: longer than short
        RealBody(inClose, inOpen, i) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortPeriodTotal, i) &&
        (
            // 1st white
            CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
            // 3rd black
            CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
            // 3rd closes well within 1st real body
            inClose[i] < inClose[i - 2] - RealBody(inClose, inOpen, i - 2) * penetration &&
            // upside gap between 1st and 2nd
            CandleGapUp(inLow, inHigh, i - 1, i - 2) &&
            // downside gap between 2nd and 3rd
            CandleGapDown(inLow, inHigh, i, i - 1)
            ||
            // 1st black
            CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
            // 3rd white
            CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
            // 3rd closes well within 1st real body
            inClose[i] > inClose[i - 2] + RealBody(inClose, inOpen, i - 2) * penetration &&
            // downside gap between 1st and 2nd
            CandleGapDown(inLow, inHigh, i - 1, i - 2) &&
            // upside gap between 2nd and 3rd
            CandleGapUp(inLow, inHigh, i, i - 1)
        );
}
