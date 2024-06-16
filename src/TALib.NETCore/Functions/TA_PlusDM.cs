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
    public static Core.RetCode PlusDM<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inHigh.Length || endIdx >= inLow.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = PlusDMLookback(optInTimePeriod);
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
        int outIdx = default;
        if (optInTimePeriod == 1)
        {
            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            while (today < endIdx)
            {
                today++;
                T tempReal = inHigh[today];
                var diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                var diffM = prevLow - tempReal;
                prevLow = tempReal;
                outReal[outIdx++] = diffP > T.Zero && diffP > diffM ? diffP : T.Zero;
            }

            outNbElement = outIdx;

            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        today = startIdx - lookbackTotal;

        T timePeriod = T.CreateChecked(optInTimePeriod);
        T prevPlusDM = T.Zero, _ = T.Zero;
        prevHigh = inHigh[today];
        prevLow = inLow[today];
        for (var i = 0; i < optInTimePeriod - 1; i++)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, ref today, ref prevHigh, ref prevLow, ref _, ref prevPlusDM, ref _, ref _,
                timePeriod, applySmoothing: false);
        }

        for (var i = 0; i < Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDM); i++)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, ref today, ref prevHigh, ref prevLow, ref _, ref prevPlusDM, ref _, ref _,
                timePeriod);
        }

        outReal[0] = prevPlusDM;
        outIdx = 1;

        while (today < endIdx)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, ref today, ref prevHigh, ref prevLow, ref _, ref prevPlusDM, ref _, ref _,
                timePeriod);
            outReal[outIdx++] = prevPlusDM;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int PlusDMLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 1 ? -1 :
        optInTimePeriod > 1 ? optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDM) - 1 : 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode PlusDM<T>(
        T[] inHigh,
        T[] inLow,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        PlusDM<T>(inHigh, inLow, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
