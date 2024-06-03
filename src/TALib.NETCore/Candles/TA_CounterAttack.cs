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
    public static Core.RetCode CounterAttack(
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

        var lookbackTotal = CounterAttackLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T equalPeriodTotal = T.Zero;
        var equalTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Equal);
        Span<T> bodyLongPeriodTotal = new T[2];
        var bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1);
            bodyLongPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if ((int) CandleColor(inClose, inOpen, i - 1) == -(int) CandleColor(inClose, inOpen, i) && // opposite candles
                RealBody(inClose, inOpen, i - 1) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal[1], i - 1) && // 1st long
                RealBody(inClose, inOpen, i) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal[0], i) && // 2nd long
                inClose[i] <= inClose[i - 1] +
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) && // equal closes
                inClose[i] >= inClose[i - 1] -
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1)
               )
            {
                outInteger[outIdx++] = (int) CandleColor(inClose, inOpen, i) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            equalPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                bodyLongPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - totIdx);
            }

            i++;
            equalTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CounterAttackLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.Equal), CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode CounterAttack(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) => CounterAttack(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
