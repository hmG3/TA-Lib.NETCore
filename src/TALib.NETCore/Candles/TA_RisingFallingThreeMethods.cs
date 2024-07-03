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
    public static Core.RetCode RisingFallingThreeMethods<T>(
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

        var lookbackTotal = RisingFallingThreeMethodsLookback();
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
            bodyPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle: long white (black) candlestick
         *   - then: group of falling (rising) small real body candlesticks (commonly black (white)) that hold within
         *     the prior long candle's range: ideally they should be three but two or more than three are ok too
         *   - final candle: long white (black) candle that opens above (below) the previous small candle's close
         *     and closes above (below) the first long candle's close
         * The meaning of "short" and "long" is specified with CandleSettings;
         * here only patterns with 3 small candles are considered;
         * outInteger is positive (1 to 100) or negative (-1 to -100)
         */

        int outIdx = default;
        do
        {
            outInteger[outIdx++] = IsRisingFallingThreeMethodsPattern(inOpen, inHigh, inLow, inClose, i, bodyPeriodTotal)
                ? (int) CandleColor(inClose, inOpen, i - 4) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyPeriodTotal[4] +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);

            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - totIdx) -
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
            }

            bodyPeriodTotal[0] +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int RisingFallingThreeMethodsLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyShort), CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 4;

    private static bool IsRisingFallingThreeMethodsPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> bodyPeriodTotal) where T : IFloatingPointIeee754<T>
    {
        var fifthCandleColor = T.CreateChecked((int) CandleColor(inClose, inOpen, i - 4) * 100);

        return
            // 1st long, then 3 small, 5th long
            RealBody(inClose, inOpen, i - 4) >
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyPeriodTotal[4], i - 4) &&
            RealBody(inClose, inOpen, i - 3) <
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[3], i - 3) &&
            RealBody(inClose, inOpen, i - 2) <
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[2], i - 2) &&
            RealBody(inClose, inOpen, i - 1) <
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[1], i - 1) &&
            RealBody(inClose, inOpen, i) >
            CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyPeriodTotal[0], i) &&
            // white, 3 black, white || black, 3 white, black
            (int) CandleColor(inClose, inOpen, i - 4) == -(int) CandleColor(inClose, inOpen, i - 3) &&
            CandleColor(inClose, inOpen, i - 3) == CandleColor(inClose, inOpen, i - 2) &&
            CandleColor(inClose, inOpen, i - 2) == CandleColor(inClose, inOpen, i - 1) &&
            (int) CandleColor(inClose, inOpen, i - 1) == -(int) CandleColor(inClose, inOpen, i) &&
            // 2nd to 4th hold within 1st: a part of the real body must be within 1st range
            T.Min(inOpen[i - 3], inClose[i - 3]) < inHigh[i - 4] && T.Max(inOpen[i - 3], inClose[i - 3]) > inLow[i - 4] &&
            T.Min(inOpen[i - 2], inClose[i - 2]) < inHigh[i - 4] && T.Max(inOpen[i - 2], inClose[i - 2]) > inLow[i - 4] &&
            T.Min(inOpen[i - 1], inClose[i - 1]) < inHigh[i - 4] && T.Max(inOpen[i - 1], inClose[i - 1]) > inLow[i - 4] &&
            // 2nd to 4th are falling (rising)
            inClose[i - 2] * fifthCandleColor < inClose[i - 3] * fifthCandleColor &&
            inClose[i - 1] * fifthCandleColor < inClose[i - 2] * fifthCandleColor &&
            // 5th opens above (below) the prior close
            inOpen[i] * fifthCandleColor > inClose[i - 1] * fifthCandleColor &&
            // 5th closes above (below) the 1st close
            inClose[i] * fifthCandleColor > inClose[i - 4] * fifthCandleColor;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode RisingFallingThreeMethods<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        RisingFallingThreeMethods<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
