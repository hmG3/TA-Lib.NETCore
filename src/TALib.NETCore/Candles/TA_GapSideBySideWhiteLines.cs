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
    public static Core.RetCode GapSideBySideWhiteLines<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outIntType,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = GapSideBySideWhiteLinesLookback();
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
        var nearPeriodTotal = T.Zero;
        var equalPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Near);
        var equalTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Equal);
        var i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - upside or downside gap (between the bodies)
         *   - first candle after the window: white candlestick
         *   - second candle after the window: white candlestick with similar size (near the same) and
         *     about the same open (equal) of the previous candle
         *   - the second candle does not close the window
         * The meaning of "near" and "equal" is specified with CandleSettings
         * outIntType is positive (100) or negative (-100):
         * it should be considered that upside or downside gap side-by-side white lines is significant when it appears in a trend,
         * while this function does not consider the trend
         */

        int outIdx = default;
        do
        {
            if (IsGapSideBySideWhiteLinesPattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal, equalPeriodTotal))
            {
                outIntType[outIdx++] = RealBodyGapUp(inOpen, inClose, i - 1, i - 2) ? 100 : -100;
            }
            else
            {
                outIntType[outIdx++] = 0;
            }

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            nearPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);

            equalPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);

            i++;
            nearTrailingIdx++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int GapSideBySideWhiteLinesLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.Near), CandleAveragePeriod(Core.CandleSettingType.Equal)) + 2;

    private static bool IsGapSideBySideWhiteLinesPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T nearPeriodTotal,
        T equalPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // upside or downside gap between the 1st candle and both the next 2 candles
        (
            RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && RealBodyGapUp(inOpen, inClose, i, i - 2)
            ||
            RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && RealBodyGapDown(inOpen, inClose, i, i - 2)
        )
        &&
        // 2nd: white
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        // 3rd: white
        CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        // same size 2 and 3
        RealBody(inClose, inOpen, i) >= RealBody(inClose, inOpen, i - 1) -
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) &&
        RealBody(inClose, inOpen, i) <= RealBody(inClose, inOpen, i - 1) +
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) &&
        // same open 2 and 3
        inOpen[i] >= inOpen[i - 1] -
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) &&
        inOpen[i] <= inOpen[i - 1] +
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode GapSideBySideWhiteLines<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outIntType) where T : IFloatingPointIeee754<T> =>
        GapSideBySideWhiteLines<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outIntType, out _, out _);
}
