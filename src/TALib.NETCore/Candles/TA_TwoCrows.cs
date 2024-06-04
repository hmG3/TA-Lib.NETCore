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
    public static Core.RetCode TwoCrows<T>(
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

        var lookbackTotal = TwoCrowsLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyLongPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - 2 - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
                RealBody(inClose, inOpen, i - 2) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) &&
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
                RealBodyGapUp(inOpen, inClose, i - 1, i - 2) &&
                CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
                inClose[i] > inOpen[i - 2] && inClose[i] < inClose[i - 2])
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                                   CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TwoCrowsLookback() => CandleAveragePeriod(Core.CandleSettingType.BodyLong) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode TwoCrows<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        TwoCrows<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
