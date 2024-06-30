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

public static partial class Functions
{
    public static Core.RetCode MinusDI<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MinusDILookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int today;
        T prevLow;
        T prevHigh;
        T prevClose;
        int outIdx = default;
        if (optInTimePeriod == 1)
        {
            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            prevClose = inClose[today];
            while (today < endIdx)
            {
                today++;
                var tempReal = inHigh[today];
                var diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                var diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffM > T.Zero && diffP < diffM)
                {
                    tempReal = TrueRange(prevHigh, prevLow, prevClose);
                    outReal[outIdx++] = !T.IsZero(tempReal) ? diffM / tempReal : T.Zero;
                }
                else
                {
                    outReal[outIdx++] = T.Zero;
                }

                prevClose = inClose[today];
            }

            outNbElement = outIdx;

            return Core.RetCode.Success;
        }

        today = startIdx;
        outBegIdx = today;
        today = startIdx - lookbackTotal;

        var timePeriod = T.CreateChecked(optInTimePeriod);
        T prevMinusDM = T.Zero, prevTR = T.Zero, _ = T.Zero;
        prevHigh = inHigh[today];
        prevLow = inLow[today];
        prevClose = inClose[today];

        for (var i = 0; i < optInTimePeriod - 1; i++)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, inClose, ref today, ref prevHigh, ref prevLow, ref prevClose, ref _, ref prevMinusDM, ref prevTR,
                timePeriod, applySmoothing: false);
        }

        for (var i = 0; i < Core.UnstablePeriodSettings.Get(Core.UnstableFunc.MinusDI) + 1; i++)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, inClose, ref today, ref prevHigh, ref prevLow, ref prevClose, ref _, ref prevMinusDM, ref prevTR,
                timePeriod);
        }

        if (!T.IsZero(prevTR))
        {
            var (minusDI, _) = CalcDI(prevMinusDM, _, prevTR);
            outReal[0] = minusDI;
        }
        else
        {
            outReal[0] = T.Zero;
        }

        outIdx = 1;

        while (today < endIdx)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, inClose, ref today, ref prevHigh, ref prevLow, ref prevClose, ref _, ref prevMinusDM, ref prevTR,
                timePeriod);
            if (!T.IsZero(prevTR))
            {
                var (minusDI, _) = CalcDI(prevMinusDM, _, prevTR);
                outReal[outIdx++] = minusDI;
            }
            else
            {
                outReal[outIdx++] = T.Zero;
            }
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MinusDILookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 1 ? -1 :
        optInTimePeriod > 1 ? optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.MinusDI) : 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MinusDI<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod) where T : IFloatingPointIeee754<T> =>
        MinusDI<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
