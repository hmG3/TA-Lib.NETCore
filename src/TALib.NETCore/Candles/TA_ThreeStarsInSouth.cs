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

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode ThreeStarsInSouth(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outInteger,
        out int outBegIdx,
        out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = ThreeStarsInSouthLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        Span<T> shadowVeryShortPeriodTotal = new T[2];
        T bodyLongPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        T shadowLongPeriodTotal = T.Zero;
        var shadowLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowLong);
        var shadowVeryShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        T bodyShortPeriodTotal = T.Zero;
        var bodyShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyShort);

        var i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 2);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyShortPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
                CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
                RealBody(inClose, inOpen, i - 2) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) &&
                LowerShadow(inClose, inOpen, inLow, i - 2) > CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i - 2) &&
                RealBody(inClose, inOpen, i - 1) < RealBody(inClose, inOpen, i - 2) &&
                inOpen[i - 1] > inClose[i - 2] && inOpen[i - 1] <= inHigh[i - 2] &&
                inLow[i - 1] < inClose[i - 2] &&
                inLow[i - 1] >= inLow[i - 2] &&
                LowerShadow(inClose, inOpen, inLow, i - 1) > CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                RealBody(inClose, inOpen, i) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i) &&
                LowerShadow(inClose, inOpen, inLow, i) < CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                UpperShadow(inHigh, inLow, inOpen, i) < CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                inLow[i] > inLow[i - 1] && inHigh[i] < inHigh[i - 1])
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2)
                                   - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                                       bodyLongTrailingIdx - 2);
            shadowLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 2)
                                     - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong,
                                         shadowLongTrailingIdx - 2);
            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            bodyShortPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i)
                                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int ThreeStarsInSouthLookback() =>
        Math.Max(
            Math.Max(CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort), CandleAveragePeriod(Core.CandleSettingType.ShadowLong)),
            Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyLong), CandleAveragePeriod(Core.CandleSettingType.BodyShort))
        ) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode ThreeStarsInSouth(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) => ThreeStarsInSouth(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
