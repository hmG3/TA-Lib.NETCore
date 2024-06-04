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
    public static Core.RetCode AbandonedBaby<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outInteger,
        out int outBegIdx,
        out int outNbElement,
        double optInPenetration = 0.3) where T : IFloatingPointIeee754<T>
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

        var lookbackTotal = AbandonedBabyLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyLongPeriodTotal = T.Zero;
        T bodyDojiPeriodTotal = T.Zero;
        T bodyShortPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - 2 - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var bodyDojiTrailingIdx = startIdx - 1 - CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        var bodyShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        var i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyDojiPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
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
        var penetration = T.CreateChecked(optInPenetration);
        do
        {
            if (RealBody(inClose, inOpen, i - 2) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) &&
                RealBody(inClose, inOpen, i - 1) <= CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyDojiPeriodTotal, i - 1) &&
                RealBody(inClose, inOpen, i) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i) &&
                (CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
                 CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
                 inClose[i] < inClose[i - 2] - RealBody(inClose, inOpen, i - 2) * penetration &&
                 CandleGapUp(inLow, inHigh, i - 1, i - 2) &&
                 CandleGapDown(inLow, inHigh, i, i - 1)
                 ||
                 CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
                 CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
                 inClose[i] > inClose[i - 2] +
                 RealBody(inClose, inOpen, i - 2) * penetration &&
                 CandleGapDown(inLow, inHigh, i - 1, i - 2) &&
                 CandleGapUp(inLow, inHigh, i, i - 1)
                )
               )
            {
                outInteger[outIdx++] = (int) CandleColor(inClose, inOpen, i) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                                   CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyDojiPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i - 1) -
                                   CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            bodyShortPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AbandonedBabyLookback() =>
        Math.Max(
            Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyDoji), CandleAveragePeriod(Core.CandleSettingType.BodyLong)),
            CandleAveragePeriod(Core.CandleSettingType.BodyShort)
        ) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode AbandonedBaby<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger,
        double optInPenetration = 0.3) where T : IFloatingPointIeee754<T> =>
        AbandonedBaby<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _, optInPenetration);
}
