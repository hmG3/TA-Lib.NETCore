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
    public static Core.RetCode Dx<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /*
         * The DM1 (one period) is based on the largest part of today's range that is outside of yesterday's range.
         *
         * The following 7 cases explain how the +DM and -DM are calculated on one period:
         *
         * Case 1:                       Case 2:
         *    C│                        A│
         *     │                         │ C│
         *     │ +DM1 = (C-A)           B│  │ +DM1 = 0
         *     │ -DM1 = 0                   │ -DM1 = (B-D)
         * A│  │                           D│
         *  │ D│
         * B│
         *
         * Case 3:                       Case 4:
         *    C│                           C│
         *     │                        A│  │
         *     │ +DM1 = (C-A)            │  │ +DM1 = 0
         *     │ -DM1 = 0               B│  │ -DM1 = (B-D)
         * A│  │                            │
         *  │  │                           D│
         * B│  │
         *    D│
         *
         * Case 5:                      Case 6:
         * A│                           A│ C│
         *  │ C│ +DM1 = 0                │  │  +DM1 = 0
         *  │  │ -DM1 = 0                │  │  -DM1 = 0
         *  │ D│                         │  │
         * B│                           B│ D│
         *
         *
         * Case 7:
         *
         *    C│
         * A│  │
         *  │  │ +DM=0
         * B│  │ -DM=0
         *    D│
         *
         * In case 3 and 4, the rule is that the smallest delta between (C-A) and (B-D) determine which of +DM or -DM is zero.
         *
         * In case 7, (C-A) and (B-D) are equal, so both +DM and -DM are zero.
         *
         * The rules remain the same when A=B and C=D (when the highs equal the lows).
         *
         * When calculating the DM over a period > 1, the one-period DM for the desired period are initially summed.
         * In other words, for a -DM14, sum the -DM1 for the first 14 days
         * (that's 13 values because there is no DM for the first day!)
         * Subsequent DM are calculated using Wilder's smoothing approach:
         *
         *                                    Previous -DM14
         *  Today's -DM14 = Previous -DM14 -  ────────────── + Today's -DM1
         *                                         14
         *
         * Calculation of a -DI14 is as follow:
         *
         *               -DM14
         *     -DI14 =  ────────
         *                TR14
         *
         * Calculation of the TR14 is:
         *
         *                                   Previous TR14
         *    Today's TR14 = Previous TR14 - ────────────── + Today's TR1
         *                                         14
         *
         *    The first TR14 is the summation of the first 14 TR1. See the Trange function on how to calculate the true range.
         *
         * Calculation of the DX14 is:
         *
         *    diffDI = ABS((-DI14) - (+DI14))
         *    sumDI  = (-DI14) + (+DI14)
         *
         *    DX14 = 100 * (diffDI / sumDI)
         *
         * Reference:
         *    New Concepts In Technical Trading Systems, J. Welles Wilder Jr
         */

        var lookbackTotal = DxLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);
        T prevMinusDM = T.Zero, prevPlusDM = T.Zero, prevTR = T.Zero;
        var today = startIdx - lookbackTotal;

        InitDMAndTR(inHigh, inLow, inClose, out var prevHigh, ref today, out var prevLow, out var prevClose, timePeriod, ref prevPlusDM,
            ref prevMinusDM, ref prevTR);

        // Skip the unstable period. This loop must be executed at least ONCE to calculate the first DI.
        SkipDxUnstablePeriod(inHigh, inLow, inClose, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref prevMinusDM,
            ref prevTR, timePeriod);

        if (!T.IsZero(prevTR))
        {
            var (minusDI, plusDI) = CalcDI(prevMinusDM, prevPlusDM, prevTR);
            T tempReal = minusDI + plusDI;
            outReal[0] = !T.IsZero(tempReal) ? Hundred<T>() * (T.Abs(minusDI - plusDI) / tempReal) : T.Zero;
        }
        else
        {
            outReal[0] = T.Zero;
        }

        var outIdx = 1;

        CalcAndOutputDX(inHigh, inLow, inClose, outReal, ref today, endIdx, ref prevHigh, ref prevLow, ref prevClose,
            ref prevPlusDM, ref prevMinusDM, ref prevTR, timePeriod, ref outIdx);

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int DxLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Dx);

    private static void SkipDxUnstablePeriod<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        ref int today,
        ref T prevHigh,
        ref T prevLow,
        ref T prevClose,
        ref T prevPlusDM,
        ref T prevMinusDM,
        ref T prevTR,
        T timePeriod) where T : IFloatingPointIeee754<T>
    {
        for (var i = Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Dx) + 1; i > 0; i--)
        {
            today++;
            UpdateDMAndTR(high, low, close, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref prevMinusDM,
                ref prevTR, timePeriod);
        }
    }

    private static void CalcAndOutputDX<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        Span<T> outputReal,
        ref int today,
        int endIdx,
        ref T prevHigh,
        ref T prevLow,
        ref T prevClose,
        ref T prevPlusDM,
        ref T prevMinusDM,
        ref T prevTR,
        T timePeriod,
        ref int outIdx) where T : IFloatingPointIeee754<T>
    {
        while (today < endIdx)
        {
            today++;
            T tempReal = high[today];
            T diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = low[today];
            T diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / timePeriod;
            prevPlusDM -= prevPlusDM / timePeriod;

            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            tempReal = TrueRange(prevHigh, prevLow, prevClose);
            prevTR = prevTR - prevTR / timePeriod + tempReal;
            prevClose = close[today];

            if (!T.IsZero(prevTR))
            {
                T minusDI = Hundred<T>() * (prevMinusDM / prevTR);
                T plusDI = Hundred<T>() * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                outputReal[outIdx] = !T.IsZero(tempReal) ? Hundred<T>() * (T.Abs(minusDI - plusDI) / tempReal) : outputReal[outIdx - 1];
            }
            else
            {
                outputReal[outIdx] = outputReal[outIdx - 1];
            }

            outIdx++;
        }
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Dx<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        Dx<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
