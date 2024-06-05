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
    public static Core.RetCode HikkakeModified<T>(
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

        var lookbackTotal = HikkakeModifiedLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T nearPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - 3 - CandleAveragePeriod(Core.CandleSettingType.Near);
        var i = nearTrailingIdx;
        while (i < startIdx - 3)
        {
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            i++;
        }

        int patternIdx = default;
        int patternResult = default;
        i = startIdx - 3;
        while (i < startIdx)
        {
            if (inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 2nd: lower high and higher low than 1st
                inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 3rd: lower high and higher low than 2nd
                (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] && // (bull) 4th: lower high and lower low
                 inClose[i - 2] <= inLow[i - 2] +
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)
                 ||
                 inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1] && // (bear) 4th: higher high and higher low
                 inClose[i - 2] >= inHigh[i - 2] -
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)))
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
            }
            else
                /* search for confirmation if modified hikkake was no more than 3 bars ago */
            if (i <= patternIdx + 3 &&
                (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] // close higher than the high of 3rd
                 ||
                 patternResult < 0 && inClose[i] < inLow[patternIdx - 1])) // close lower than the low of 3rd
            {
                patternIdx = 0;
            }

            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2) -
                               CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 2);
            nearTrailingIdx++;
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 2nd: lower high and higher low than 1st
                inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 3rd: lower high and higher low than 2nd
                (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] && // (bull) 4th: lower high and lower low
                 inClose[i - 2] <= inLow[i - 2] +
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)
                 ||
                 inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1] && // (bear) 4th: higher high and higher low
                 inClose[i - 2] >= inHigh[i - 2] -
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)))
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
                outInteger[outIdx++] = patternResult;
            }
            else
                /* search for confirmation if modified hikkake was no more than 3 bars ago */
            if (i <= patternIdx + 3 &&
                (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] // close higher than the high of 3rd
                 ||
                 patternResult < 0 && inClose[i] < inLow[patternIdx - 1])) // close lower than the low of 3rd
            {
                outInteger[outIdx++] = patternResult + 100 * (patternResult > 0 ? 1 : -1);
                patternIdx = 0;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2) -
                               CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 2);
            nearTrailingIdx++;
            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int HikkakeModifiedLookback() => Math.Max(1, CandleAveragePeriod(Core.CandleSettingType.Near)) + 5;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HikkakeModified<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        HikkakeModified<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
