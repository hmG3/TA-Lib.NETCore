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
    public static Core.RetCode IdenticalThreeCrows<T>(
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

        var lookbackTotal = IdenticalThreeCrowsLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        Span<T> shadowVeryShortPeriodTotal = new T[3];
        var shadowVeryShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        Span<T> equalPeriodTotal = new T[3];
        var equalTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Equal);
        var i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2);
            equalPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black && // 1st black
                // very short lower shadow
                LowerShadow(inClose, inOpen, inLow, i - 2) < CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black && // 2nd black
                // very short lower shadow
                LowerShadow(inClose, inOpen, inLow, i - 1) < CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                CandleColor(inClose, inOpen, i) == Core.CandleColor.Black && // 3rd black
                // very short lower shadow
                LowerShadow(inClose, inOpen, inLow, i) < CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                inClose[i - 2] > inClose[i - 1] && // three declining
                inClose[i - 1] > inClose[i] &&
                // 2nd black opens very close to 1st close
                inOpen[i - 1] <= inClose[i - 2] + CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal,
                    equalPeriodTotal[2], i - 2) &&
                inOpen[i - 1] >= inClose[i - 2] - CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal,
                    equalPeriodTotal[2], i - 2) &&
                // 3rd black opens very close to 2nd close
                inOpen[i] <= inClose[i - 1] + CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal,
                    equalPeriodTotal[1], i - 1) &&
                inOpen[i] >= inClose[i - 1] - CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal,
                    equalPeriodTotal[1], i - 1))
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            for (var totIdx = 2; totIdx >= 0; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            for (var totIdx = 2; totIdx >= 1; --totIdx)
            {
                equalPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int IdenticalThreeCrowsLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort), CandleAveragePeriod(Core.CandleSettingType.Equal)) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode IdenticalThreeCrows<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        IdenticalThreeCrows<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
