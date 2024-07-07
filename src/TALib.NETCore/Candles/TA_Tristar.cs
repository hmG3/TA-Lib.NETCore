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
    public static Core.RetCode Tristar<T>(
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

        var lookbackTotal = TristarLookback();
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
        var bodyPeriodTotal = T.Zero;
        var bodyTrailingIdx = startIdx - 2 - CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        var i = bodyTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - 3 consecutive doji days
         *   - the second doji is a star
         * The meaning of "doji" is specified with CandleSettings
         * outIntType is positive (100) when bullish or negative (-100) when bearish
         */

        int outIdx = default;
        do
        {
            if (IsTristarPattern(inOpen, inHigh, inLow, inClose, i, bodyPeriodTotal))
            {
                // 3rd: doji
                outIntType[outIdx] = 0;
                if
                (
                    // 2nd gaps up
                    RealBodyGapUp(inOpen, inClose, i - 1, i - 2) &&
                    // 3rd is not higher than 2nd
                    T.Max(inOpen[i], inClose[i]) < T.Max(inOpen[i - 1], inClose[i - 1])
                )
                {
                    outIntType[outIdx] = -100;
                }

                if
                (
                    // 2nd gaps down
                    RealBodyGapDown(inOpen, inClose, i - 1, i - 2) &&
                    // 3rd is not lower than 2nd
                    T.Min(inOpen[i], inClose[i]) > T.Min(inOpen[i - 1], inClose[i - 1])
                )
                {
                    outIntType[outIdx] = 100;
                }

                outIdx++;
            }
            else
            {
                outIntType[outIdx++] = 0;
            }

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyPeriodTotal +=
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i - 2) -
                CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyTrailingIdx);

            i++;
            bodyTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TristarLookback() => CandleAveragePeriod(Core.CandleSettingType.BodyDoji) + 2;

    private static bool IsTristarPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st: doji
        RealBody(inClose, inOpen, i - 2) <=
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyPeriodTotal, i - 2) &&
        // 2nd: doji
        RealBody(inClose, inOpen, i - 1) <=
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyPeriodTotal, i - 2) &&
        // 3rd: doji
        RealBody(inClose, inOpen, i) <=
        CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyPeriodTotal, i - 2);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Tristar<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outIntType) where T : IFloatingPointIeee754<T> =>
        Tristar<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outIntType, out _, out _);
}
