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
    public static Core.RetCode Breakaway<T>(
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

        var lookbackTotal = BreakawayLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyLongPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (RealBody(inClose, inOpen, i - 4) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 4) && // 1st long
                CandleColor(inClose, inOpen, i - 4) ==
                CandleColor(inClose, inOpen, i - 3) && // 1st, 2nd, 4th same color, 5th opposite
                CandleColor(inClose, inOpen, i - 3) == CandleColor(inClose, inOpen, i - 1) &&
                (int) CandleColor(inClose, inOpen, i - 1) == -(int) CandleColor(inClose, inOpen, i) &&
                (CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.Black && // when 1st is black:
                 RealBodyGapDown(inOpen, inClose, i - 3, i - 4) && // 2nd gaps down
                 inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] && // 3rd has lower high and low than 2nd
                 inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] < inLow[i - 2] && // 4th has lower high and low than 3rd
                 inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4] // 5th closes inside the gap
                 ||
                 CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.White && // when 1st is white:
                 RealBodyGapUp(inClose, inOpen, i - 3, i - 4) && // 2nd gaps up
                 inHigh[i - 2] > inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 3rd has higher high and low than 2nd
                 inHigh[i - 1] > inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 4th has higher high and low than 3rd
                 inClose[i] < inOpen[i - 3] && inClose[i] > inClose[i - 4])) // 5th closes inside the gap
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
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4)
                                   - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                                       bodyLongTrailingIdx - 4);
            i++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int BreakawayLookback() => CandleAveragePeriod(Core.CandleSettingType.BodyLong) + 4;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Breakaway<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        Breakaway<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
