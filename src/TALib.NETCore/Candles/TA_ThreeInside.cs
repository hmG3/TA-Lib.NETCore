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
    public static Core.RetCode ThreeInside<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ThreeInsideImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    [PublicAPI]
    public static int ThreeInsideLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyShort), CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode ThreeInside<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        ThreeInsideImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode ThreeInsideImpl<T>(
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

        var lookbackTotal = ThreeInsideLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var bodyLongPeriodTotal = T.Zero;
        var bodyShortPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - 2 - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var bodyShortTrailingIdx = startIdx - 1 - CandleAveragePeriod(Core.CandleSettingType.BodyShort);

        var i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyShortPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle: long white (black) real body
         *   - second candle: short real body totally engulfed by the first
         *   - third candle: black (white) candle that closes lower (higher) than the first candle's open
         * The meaning of "short" and "long" is specified with CandleSettings
         * outIntType is positive (100) for the three inside up or negative (-100) for the three inside down
         * it should be considered that a three inside up is significant when it appears in a downtrend and
         * a three inside down is significant when it appears in an uptrend,
         * while this function does not consider the trend
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsThreeInsidePattern(inOpen, inHigh, inLow, inClose, i, bodyLongPeriodTotal, bodyShortPeriodTotal)
                ? -(int) CandleColor(inClose, inOpen, i - 2) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyLongPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyShortPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 1) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);

            i++;
            bodyLongTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsThreeInsidePattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyLongPeriodTotal,
        T bodyShortPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st: long
        RealBody(inClose, inOpen, i - 2) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 2) &&
        // 2nd: short
        RealBody(inClose, inOpen, i - 1) <=
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortPeriodTotal, i - 1) &&
        // engulfed by 1st
        T.Max(inClose[i - 1], inOpen[i - 1]) < T.Max(inClose[i - 2], inOpen[i - 2]) &&
        T.Min(inClose[i - 1], inOpen[i - 1]) > T.Min(inClose[i - 2], inOpen[i - 2]) &&
        (
            // 3rd: opposite to 1st
            CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
            CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
            inClose[i] < inOpen[i - 2]
            ||
            // and closing out
            CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
            CandleColor(inClose, inOpen, i) == Core.CandleColor.White && inClose[i] > inOpen[i - 2]
        );
}
