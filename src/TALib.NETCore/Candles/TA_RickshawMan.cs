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
    public static Core.RetCode RickshawMan<T>(
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

        var lookbackTotal = RickshawManLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyDojiPeriodTotal = T.Zero;
        var bodyDojiTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        T shadowLongPeriodTotal = T.Zero;
        var shadowLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowLong);
        T nearPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (RealBody(inClose, inOpen, i) <=
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) && // doji
                LowerShadow(inClose, inOpen, inLow, i) > CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long shadow
                UpperShadow(inHigh, inClose, inOpen, i) > CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && T.Min(inOpen[i], inClose[i]) <= inLow[i] +
                HighLowRange(inHigh, inLow, i) / T.CreateChecked(2) + CandleAverage(inOpen,
                    inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i) && T.Max(inOpen[i], inClose[i]) >= inLow[i] +
                HighLowRange(inHigh, inLow, i) / T.CreateChecked(2) - CandleAverage(inOpen,
                    inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i))
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyDojiPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            shadowLongPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx);
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i) -
                               CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx);

            i++;
            bodyDojiTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
        } while (i <= endIdx);


        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int RickshawManLookback() =>
        Math.Max(
            Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyDoji), CandleAveragePeriod(Core.CandleSettingType.ShadowLong)),
            CandleAveragePeriod(Core.CandleSettingType.Near));

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode RickshawMan<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        RickshawMan<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
