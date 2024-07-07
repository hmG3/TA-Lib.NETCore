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
    public static Core.RetCode MinusDM<T>(
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
         *                                     Previous -DM14
         *   Today's -DM14 = Previous -DM14 -  ────────────── + Today's -DM1
         *                                           14
         * Reference:
         *   New Concepts In Technical Trading Systems, J. Welles Wilder Jr
         */

        var lookbackTotal = MinusDMLookback(optInTimePeriod);
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

        // Indicate where the next output should be put in the outReal.
        int outIdx = default;

        // Trap the case where no smoothing is needed.
        if (optInTimePeriod == 1)
        {
            // No smoothing needed. Just do a simple DM1 for each price bar.
            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            while (today < endIdx)
            {
                today++;
                var tempReal = inHigh[today];
                var diffP = tempReal - prevHigh; // Plus Delta
                prevHigh = tempReal;
                tempReal = inLow[today];
                var diffM = prevLow - tempReal; // Minus Delta
                prevLow = tempReal;
                if (diffM > T.Zero && diffP < diffM)
                {
                    // Case 2 and 4: +DM = 0,-DM = diffM
                    outReal[outIdx++] = diffM;
                }
                else
                {
                    outReal[outIdx++] = T.Zero;
                }
            }

            outNbElement = outIdx;

            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        today = startIdx - lookbackTotal;

        var timePeriod = T.CreateChecked(optInTimePeriod);
        T prevMinusDM = T.Zero, _ = T.Zero;

        InitDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, out prevHigh, ref today, out prevLow, out var _, timePeriod, ref _,
            ref prevMinusDM, ref _);

        // Process subsequent DM

        // Skip the unstable period.
        for (var i = 0; i < Core.UnstablePeriodSettings.Get(Core.UnstableFunc.MinusDM); i++)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, ref today, ref prevHigh, ref prevLow, ref _, ref _, ref prevMinusDM, ref _,
                timePeriod);
        }

        outReal[0] = prevMinusDM;
        outIdx = 1;

        while (today < endIdx)
        {
            today++;
            UpdateDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, ref today, ref prevHigh, ref prevLow, ref _, ref _, ref prevMinusDM, ref _,
                timePeriod);
            outReal[outIdx++] = prevMinusDM;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MinusDMLookback(int optInTimePeriod = 14) => optInTimePeriod switch
    {
        < 1 => -1,
        > 1 => optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.MinusDM) - 1,
        _ => 1
    };

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MinusDM<T>(
        T[] inHigh,
        T[] inLow,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        MinusDM<T>(inHigh, inLow, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
