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
    [PublicAPI]
    public static Core.RetCode MatHold<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange,
        double optInPenetration = 0.5) where T : IFloatingPointIeee754<T> =>
        MatHoldImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    [PublicAPI]
    public static int MatHoldLookback() =>
        Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort),
            CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowLong)) + 4;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MatHold<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange,
        double optInPenetration = 0.5) where T : IFloatingPointIeee754<T> =>
        MatHoldImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    private static Core.RetCode MatHoldImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange,
        double optInPenetration) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInPenetration < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MatHoldLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> bodyPeriodTotal = new T[5];
        var bodyShortTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        var bodyLongTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[3] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 3);
            bodyPeriodTotal[2] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 2);
            bodyPeriodTotal[1] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[4] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first candle: long white candle
         *   - upside gap between the first and the second bodies
         *   - second candle: small black candle
         *   - third and fourth candles: falling small real body candlesticks (commonly black) that hold within the long
         *     white candle's body and are higher than the reaction days of the rising three methods
         *   - fifth candle: white candle that opens above the previous small candle's close and closes higher than
         *     the high of the highest reaction day
         * The meaning of "short" and "long" is specified with CandleSettings
         * "hold within" means "a part of the real body must be within"
         * optInPenetration is the maximum percentage of the first white body the reaction days can penetrate
         * (it is to specify how much the reaction days should be "higher than the reaction days of the rising three methods")
         * outIntType is positive (100): mat hold is always bullish
         */

        var outIdx = 0;
        var penetration = T.CreateChecked(optInPenetration);
        do
        {
            outIntType[outIdx++] = IsMatHoldPattern(inOpen, inHigh, inLow, inClose, i, bodyPeriodTotal, penetration) ? 100 : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyPeriodTotal[4] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);

            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                        bodyShortTrailingIdx - totIdx);
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsMatHoldPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> bodyPeriodTotal,
        T penetration) where T : IFloatingPointIeee754<T> =>
        // 1st long, then 3 small
        CandleHelpers.RealBody(inClose, inOpen, i - 4) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyPeriodTotal[4], i - 4) &&
        CandleHelpers.RealBody(inClose, inOpen, i - 3) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[3], i - 3) &&
        CandleHelpers.RealBody(inClose, inOpen, i - 2) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[2], i - 2) &&
        CandleHelpers.RealBody(inClose, inOpen, i - 1) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[1], i - 1) &&
        // white, black, 2 black or white, white
        CandleHelpers.CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.White &&
        CandleHelpers.CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        // upside gap 1st to 2nd
        CandleHelpers.RealBodyGapUp(inOpen, inClose, i - 3, i - 4) &&
        // 3rd to 4th hold within 1st: a part of the real body must be within 1st real body
        T.Min(inOpen[i - 2], inClose[i - 2]) < inClose[i - 4] &&
        T.Min(inOpen[i - 1], inClose[i - 1]) < inClose[i - 4] &&
        // reaction days penetrate first body less than optInPenetration percent
        T.Min(inOpen[i - 2], inClose[i - 2]) > inClose[i - 4] - CandleHelpers.RealBody(inClose, inOpen, i - 4) * penetration &&
        T.Min(inOpen[i - 1], inClose[i - 1]) > inClose[i - 4] - CandleHelpers.RealBody(inClose, inOpen, i - 4) * penetration &&
        // 2nd to 4th are falling
        T.Max(inClose[i - 2], inOpen[i - 2]) < inOpen[i - 3] &&
        T.Max(inClose[i - 1], inOpen[i - 1]) < T.Max(inClose[i - 2], inOpen[i - 2]) &&
        // 5th opens above the prior close
        inOpen[i] > inClose[i - 1] &&
        // 5th closes above the highest high of the reaction days
        inClose[i] > T.Max(T.Max(inHigh[i - 3], inHigh[i - 2]), inHigh[i - 1]);
}
