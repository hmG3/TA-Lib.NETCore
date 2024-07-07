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
    public static Core.RetCode PlusDI<T>(
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

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* The DM1 (one period) is based on the largest part of today's range that is outside of yesterday's range.
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
         *  │  │ +DM1=0
         * B│  │ -DM1=0
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
         *                                     Previous +DM14
         *   Today's +DM14 = Previous +DM14 -  ────────────── + Today's +DM1
         *                                           14
         *
         * Calculation of a +DI14 is as follows:
         *
         *             +DM14
         *   +DI14 =  ────────
         *              TR14
         *
         * Calculation of the TR14 is:
         *
         *                                  Previous TR14
         *   Today's TR14 = Previous TR14 - ────────────── + Today's TR1
         *                                        14
         *
         *   The first TR14 is the summation of the first 14 TR1. See the TRange function on how to calculate the true range.
         *
         * Reference:
         *    New Concepts In Technical Trading Systems, J. Welles Wilder Jr
         */

        var lookbackTotal = PlusDILookback(optInTimePeriod);
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
            /* No smoothing needed. Just do the following for each price bar:
             *          +DM1
             *   +DI1 = ────
             *           TR1
             */
            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            prevClose = inClose[today];
            while (today < endIdx)
            {
                today++;
                var tempReal = inHigh[today];
                var diffP = tempReal - prevHigh; // Plus Delta
                prevHigh = tempReal;
                tempReal = inLow[today];
                var diffM = prevLow - tempReal; // Minus Delta
                prevLow = tempReal;
                if (diffP > T.Zero && diffP > diffM)
                {
                    // Case 1 and 3: +DM = diffP ,-DM = 0
                    tempReal = TrueRange(prevHigh, prevLow, prevClose);
                    outReal[outIdx++] = !T.IsZero(tempReal) ? diffP / tempReal : T.Zero;
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
        T prevPlusDM = T.Zero, prevTR = T.Zero, _ = T.Zero;

        InitDMAndTR(inHigh, inLow, inClose, out prevHigh, ref today, out prevLow, out prevClose, timePeriod, ref prevPlusDM, ref _,
            ref prevTR);

        // Process subsequent DI

        // Skip the unstable period. Note that this loop must be executed at least ONCE to calculate the first DI.
        for (var i = 0; i < Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDI) + 1; i++)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, inClose, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref _, ref prevTR,
                timePeriod);
        }

        if (!T.IsZero(prevTR))
        {
            var (_, plusDI) = CalcDI(_, prevPlusDM, prevTR);
            outReal[0] = plusDI;
        }
        else
        {
            outReal[0] = T.Zero;
        }

        outIdx = 1;

        while (today < endIdx)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, inClose, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref _, ref prevTR,
                timePeriod);
            if (!T.IsZero(prevTR))
            {
                var (_, plusDI) = CalcDI(_, prevPlusDM, prevTR);
                outReal[outIdx++] = plusDI;
            }
            else
            {
                outReal[outIdx++] = T.Zero;
            }
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int PlusDILookback(int optInTimePeriod = 14) => optInTimePeriod switch
    {
        < 1 => -1,
        > 1 => optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDI),
        _ => 1
    };

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode PlusDI<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        PlusDI<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
