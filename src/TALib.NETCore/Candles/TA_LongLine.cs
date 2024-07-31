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
    public static Core.RetCode LongLine<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        LongLineImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    [PublicAPI]
    public static int LongLineLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyLong), CandleAveragePeriod(Core.CandleSettingType.ShadowShort));

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode LongLine<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        LongLineImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode LongLineImpl<T>(
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

        var lookbackTotal = LongLineLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var bodyPeriodTotal = T.Zero;
        var bodyTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var shadowPeriodTotal = T.Zero;
        var shadowTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowShort);
        var i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = shadowTrailingIdx;
        while (i < startIdx)
        {
            shadowPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i);
            i++;
        }

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - long real body
         *   - short upper and lower shadow
         * The meaning of "long" and "short" is specified with CandleSettings
         * outIntType is positive (100) when white (bullish), negative (-100) when black (bearish)
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsLongLinePattern(inOpen, inHigh, inLow, inClose, i, bodyPeriodTotal, shadowPeriodTotal)
                ? (int) CandleColor(inClose, inOpen, i) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyTrailingIdx);

            shadowPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowTrailingIdx);

            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsLongLinePattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyPeriodTotal,
        T shadowPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // long body
        RealBody(inClose, inOpen, i) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyPeriodTotal, i) &&
        // short upper shadow
        UpperShadow(inHigh, inClose, inOpen, i) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowPeriodTotal, i) &&
        // short lower shadow
        LowerShadow(inClose, inOpen, inLow, i) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowPeriodTotal, i);
}
