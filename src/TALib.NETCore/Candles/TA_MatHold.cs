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
    public static Core.RetCode MatHold<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outInteger,
        out int outBegIdx,
        out int outNbElement,
        double optInPenetration = 0.5) where T : IFloatingPointIeee754<T>
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

        var lookbackTotal = MatHoldLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        Span<T> bodyPeriodTotal = new T[5];
        var bodyShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        var bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[3] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 3);
            bodyPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 2);
            bodyPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[4] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        var penetration = T.CreateChecked(optInPenetration);
        do
        {
            if ( // 1st long, then 3 small
                RealBody(inClose, inOpen, i - 4) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyPeriodTotal[4], i - 4) &&
                RealBody(inClose, inOpen, i - 3) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[3], i - 3) &&
                RealBody(inClose, inOpen, i - 2) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[2], i - 2) &&
                RealBody(inClose, inOpen, i - 1) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[1], i - 1) &&
                // white, black, 2 black or white, white
                CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.White &&
                CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
                CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
                // upside gap 1st to 2nd
                RealBodyGapUp(inOpen, inClose, i - 3, i - 4) &&
                // 3rd to 4th hold within 1st: a part of the real body must be within 1st real body
                T.Min(inOpen[i - 2], inClose[i - 2]) < inClose[i - 4] &&
                T.Min(inOpen[i - 1], inClose[i - 1]) < inClose[i - 4] &&
                // reaction days penetrate first body less than optInPenetration percent
                T.Min(inOpen[i - 2], inClose[i - 2]) > inClose[i - 4] - RealBody(inClose, inOpen, i - 4) * penetration &&
                T.Min(inOpen[i - 1], inClose[i - 1]) > inClose[i - 4] - RealBody(inClose, inOpen, i - 4) * penetration &&
                // 2nd to 4th are falling
                T.Max(inClose[i - 2], inOpen[i - 2]) < inOpen[i - 3] &&
                T.Max(inClose[i - 1], inOpen[i - 1]) < T.Max(inClose[i - 2], inOpen[i - 2]) &&
                // 5th opens above the prior close
                inOpen[i] > inClose[i - 1] &&
                // 5th closes above the highest high of the reaction days
                inClose[i] > T.Max(T.Max(inHigh[i - 3], inHigh[i - 2]), inHigh[i - 1])
               )
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
            bodyPeriodTotal[4] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                                  CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MatHoldLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyShort), CandleAveragePeriod(Core.CandleSettingType.ShadowLong)) + 4;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode MatHold<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger,
        double optInPenetration = 0.5) where T : IFloatingPointIeee754<T> =>
        MatHold<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _, optInPenetration);
}
