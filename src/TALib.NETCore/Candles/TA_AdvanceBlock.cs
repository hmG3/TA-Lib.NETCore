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
    public static Core.RetCode AdvanceBlock<T>(
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

        var lookbackTotal = AdvanceBlockLookback();
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
        Span<T> shadowShortPeriodTotal = new T[3];
        var shadowShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowShort);
        Span<T> shadowLongPeriodTotal = new T[2];
        var shadowLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowLong);
        Span<T> nearPeriodTotal = new T[3];
        var nearTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Near);
        Span<T> farPeriodTotal = new T[3];
        var farTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Far);
        var bodyLongPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = shadowShortTrailingIdx;
        while (i < startIdx)
        {
            shadowShortPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - 2);
            shadowShortPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - 1);
            shadowShortPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 1);
            shadowLongPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            nearPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = farTrailingIdx;
        while (i < startIdx)
        {
            farPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - 2);
            farPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - three white candlesticks with consecutively higher closes
         *   - each candle opens within or near the previous white real body
         *   - first candle: long white with no or very short upper shadow (a short shadow is accepted too for more flexibility)
         *   - second and third candles, or only third candle, show signs of weakening: progressively smaller white real bodies
         * and/or relatively long upper shadows; see below for specific conditions
         * The meanings of "long body", "short shadow", "far" and "near" are specified with CandleSettings;
         * outInteger is negative (-1 to -100): advance block is always bearish;
         * the user should consider that advance block is significant when it appears in uptrend,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outInteger[outIdx++] = IsAdvanceBlockPattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal, bodyLongPeriodTotal,
                shadowShortPeriodTotal, farPeriodTotal, shadowLongPeriodTotal)
                ? -100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 2; totIdx >= 0; --totIdx)
            {
                shadowShortPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowShortTrailingIdx - totIdx);
            }

            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                shadowLongPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx - totIdx);
            }

            for (var totIdx = 2; totIdx >= 1; --totIdx)
            {
                farPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, farTrailingIdx - totIdx);

                nearPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - totIdx);
            }

            bodyLongPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 2);

            i++;
            shadowShortTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AdvanceBlockLookback() =>
        Math.Max(
            Math.Max(
                Math.Max(CandleAveragePeriod(Core.CandleSettingType.ShadowLong), CandleAveragePeriod(Core.CandleSettingType.ShadowShort)),
                Math.Max(CandleAveragePeriod(Core.CandleSettingType.Far), CandleAveragePeriod(Core.CandleSettingType.Near))),
            CandleAveragePeriod(Core.CandleSettingType.BodyLong)
        ) + 2;

    private static bool IsAdvanceBlockPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> nearPeriodTotal,
        T bodyLongPeriodTotal,
        Span<T> shadowShortPeriodTotal,
        Span<T> farPeriodTotal,
        Span<T> shadowLongPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st white
        CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
        // 2nd white
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        // 3rd white
        CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        // consecutive higher closes
        inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] &&
        // 2nd opens within/near 1st real body
        inOpen[i - 1] > inOpen[i - 2] &&
        // 3rd opens within/near 2nd real body
        inOpen[i - 1] <= inClose[i - 2] +
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
        inOpen[i] > inOpen[i - 1] &&
        inOpen[i] <= inClose[i - 1] +
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[1], i - 1) &&
        // 1st: long real body
        RealBody(inClose, inOpen, i - 2) >
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 2) &&
        // 1st: short upper shadow
        UpperShadow(inHigh, inClose, inOpen, i - 2) <
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[2], i - 2) &&
        (
            // (2 far smaller than 1 && 3 not longer than 2)
            // advance blocked with the 2nd, 3rd must not carry on the advance
            RealBody(inClose, inOpen, i - 1) < RealBody(inClose, inOpen, i - 2) -
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, farPeriodTotal[2], i - 2) &&
            RealBody(inClose, inOpen, i) < RealBody(inClose, inOpen, i - 1) +
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[1], i - 1)
            ||
            // 3 far smaller than 2
            // advance blocked with the 3rd
            RealBody(inClose, inOpen, i) < RealBody(inClose, inOpen, i - 1) -
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, farPeriodTotal[1], i - 1)
            ||
            // (3 smaller than 2 && 2 smaller than 1 && (3 or 2 not short upper shadow))
            // advance blocked with progressively smaller real bodies and some upper shadows
            RealBody(inClose, inOpen, i) < RealBody(inClose, inOpen, i - 1) &&
            RealBody(inClose, inOpen, i - 1) < RealBody(inClose, inOpen, i - 2) &&
            (
                UpperShadow(inHigh, inClose, inOpen, i) >
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[0], i) ||
                UpperShadow(inHigh, inClose, inOpen, i - 1) >
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[1], i - 1)
            )
            ||
            // (3 smaller than 2 && 3 long upper shadow)
            // advance blocked with 3rd candle's long upper shadow and smaller body
            RealBody(inClose, inOpen, i) < RealBody(inClose, inOpen, i - 1) &&
            UpperShadow(inHigh, inClose, inOpen, i) >
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal[0], i)
        );

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AdvanceBlock<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        AdvanceBlock<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
