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
    public static Core.RetCode Adx<T>(
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
         * In case 3 and 4, the rule is that the smallest delta between (C-A) and (B-D) determine
         * which of +DM or -DM is zero.
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
         * (Same thing for +DM14)
         *
         * Calculation of a -DI14 is as follows:
         *
         *               -DM14
         *     -DI14 =  ────────
         *                TR14
         *
         * (Same thing for +DI14)
         *
         * Calculation of the TR14 is:
         *
         *                                   Previous TR14
         *    Today's TR14 = Previous TR14 - ────────────── + Today's TR1
         *                                         14
         *
         *    The first TR14 is the summation of the first 14 TR1. See the TRange function on how to calculate the true range.
         *
         * Calculation of the DX14 is:
         *
         *    diffDI = ABS( (-DI14) - (+DI14) )
         *    sumDI  = (-DI14) + (+DI14)
         *
         *    DX14 = 100 * (diffDI / sumDI)
         *
         * Calculation of the first ADX:
         *
         *    ADX14 = SUM of the first 14 DX
         *
         * Calculation of subsequent ADX:
         *
         *            ((Previous ADX14)*(14-1))+ Today's DX
         *    ADX14 = ─────────────────────────────────────
         *                             14
         *
         * Reference:
         *    New Concepts In Technical Trading Systems, J. Welles Wilder Jr
         */

        var lookbackTotal = AdxLookback(optInTimePeriod);
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
        var today = startIdx;
        outBegIdx = today;
        today = startIdx - lookbackTotal;

        T prevHigh;
        T prevLow;
        T prevClose;

        InitDMAndTR(inHigh, inLow, inClose);

        T sumDX = AddAllInitialDX(inHigh, inLow, inClose);

        // Calculate the first ADX
        T prevADX = sumDX / timePeriod;

        SkipUnstablePeriod(inHigh, inLow, inClose);

        // Output the first ADX
        outReal[0] = prevADX;
        var outIdx = 1;

        CalcAndOutputSubsequentADX(inHigh, inLow, inClose, outReal);

        outNbElement = outIdx;

        return Core.RetCode.Success;

        void InitDMAndTR(ReadOnlySpan<T> high, ReadOnlySpan<T> low, ReadOnlySpan<T> close)
        {
            prevHigh = high[today];
            prevLow = low[today];
            prevClose = close[today];

            for (var i = Int32.CreateTruncating(timePeriod) - 1; i > 0; i--)
            {
                today++;

                UpdateDMAndTR(high, low, close, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref prevMinusDM,
                    ref prevTR, timePeriod, false);
            }
        }

        T AddAllInitialDX(ReadOnlySpan<T> high, ReadOnlySpan<T> low, ReadOnlySpan<T> close)
        {
            T sumDX = T.Zero;
            for (var i = Int32.CreateTruncating(timePeriod); i > 0; i--)
            {
                today++;
                UpdateDMAndTR(high, low, close, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref prevMinusDM,
                    ref prevTR, timePeriod);

                if (T.IsZero(prevTR))
                {
                    continue;
                }

                (T minusDI, T plusDI) = CalculateDI(prevMinusDM, prevPlusDM, prevTR);
                var tempReal = minusDI + plusDI;
                if (!T.IsZero(tempReal))
                {
                    sumDX += Hundred<T>() * (T.Abs(minusDI - plusDI) / tempReal);
                }
            }

            return sumDX;
        }

        void SkipUnstablePeriod(ReadOnlySpan<T> high, ReadOnlySpan<T> low, ReadOnlySpan<T> close)
        {
            for (var i = Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Adx); i > 0; i--)
            {
                today++;
                UpdateDMAndTR(high, low, close, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref prevMinusDM,
                    ref prevTR, timePeriod);

                if (T.IsZero(prevTR))
                {
                    continue;
                }

                (T minusDI, T plusDI) = CalculateDI(prevMinusDM, prevPlusDM, prevTR);
                var tempReal = minusDI + plusDI;
                if (T.IsZero(tempReal))
                {
                    continue;
                }

                tempReal = Hundred<T>() * (T.Abs(minusDI - plusDI) / tempReal);
                prevADX = (prevADX * (timePeriod - T.One) + tempReal) / timePeriod;
            }
        }

        void CalcAndOutputSubsequentADX(
            ReadOnlySpan<T> high,
            ReadOnlySpan<T> low,
            ReadOnlySpan<T> close,
            Span<T> outputReal)
        {
            while (today < endIdx)
            {
                today++;
                UpdateDMAndTR(high, low, close, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref prevMinusDM,
                    ref prevTR, timePeriod);

                if (!T.IsZero(prevTR))
                {
                    (T minusDI, T plusDI) = CalculateDI(prevMinusDM, prevPlusDM, prevTR);
                    var tempReal = minusDI + plusDI;
                    if (!T.IsZero(tempReal))
                    {
                        tempReal = Hundred<T>() * (T.Abs(minusDI - plusDI) / tempReal);
                        prevADX = (prevADX * (timePeriod - T.One) + tempReal) / timePeriod;
                    }
                }

                outputReal[outIdx++] = prevADX;
            }
        }
    }

    public static int AdxLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod * 2 + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Adx) - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Adx<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod) where T : IFloatingPointIeee754<T> =>
        Adx<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
