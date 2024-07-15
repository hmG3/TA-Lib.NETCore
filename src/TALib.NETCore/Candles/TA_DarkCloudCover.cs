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
    public static Core.RetCode DarkCloudCover<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outIntType,
        out int outBegIdx,
        out int outNbElement,
        double optInPenetration = 0.5) where T : IFloatingPointIeee754<T> =>
        DarkCloudCoverImpl(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outIntType, out outBegIdx, out outNbElement, optInPenetration);

    [PublicAPI]
    public static int DarkCloudCoverLookback() => CandleAveragePeriod(Core.CandleSettingType.BodyLong) + 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode DarkCloudCover<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outIntType,
        out int outBegIdx,
        out int outNbElement,
        double optInPenetration = 0.5) where T : IFloatingPointIeee754<T> =>
        DarkCloudCoverImpl<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outIntType, out outBegIdx, out outNbElement,
            optInPenetration);

    private static Core.RetCode DarkCloudCoverImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outIntType,
        out int outBegIdx,
        out int outNbElement,
        double optInPenetration) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInPenetration < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = DarkCloudCoverLookback();
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
        var bodyLongPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle: long white candle
         *   - second candle: black candle that opens above previous day high and closes within previous day real body
         * Greg Morris wants the close to be below the midpoint of the previous real body
         * The meaning of "long" is specified with CandleSettings,
         * the penetration of the first real body is specified with optInPenetration
         * outIntType is negative (-100): dark cloud cover is always bearish
         * it should be considered that a dark cloud cover is significant when it appears in an uptrend,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outIntType[outIdx++] = IsDarkCloudCoverPattern(inOpen, inHigh, inLow, inClose, optInPenetration, i, bodyLongPeriodTotal)
                ? -100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyLongPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 1);

            i++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    private static bool IsDarkCloudCoverPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        double optInPenetration,
        int i,
        T bodyLongPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st: white
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        // long body
        RealBody(inClose, inOpen, i - 1) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 1) &&
        // 2nd: black
        CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
        // open above prior high
        inOpen[i] > inHigh[i - 1] &&
        // close within prior body
        inClose[i] > inOpen[i - 1] &&
        inClose[i] < inClose[i - 1] - RealBody(inClose, inOpen, i - 1) * T.CreateChecked(optInPenetration);
}
