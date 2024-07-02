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
    public static Core.RetCode Sar<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        double optInAcceleration = 0.02,
        double optInMaximum = 0.2) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inHigh.Length || endIdx >= inLow.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInAcceleration < 0.0 || optInMaximum < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        /* Implementation of the SAR has been a little bit open to interpretation since Wilder (the original author)
         * did not define a precise algorithm on how to bootstrap the algorithm.
         * Take any existing software application, and you will see slight variation on how the algorithm was adapted.
         *
         * What is the initial trade direction? Long or short?
         * ───────────────────────────────────────────────────
         * The interpretation of what should be the initial SAR values is open to interpretation,
         * particularly since the caller to the function does not specify the initial direction of the trade.
         *
         * The following logic is used:
         *  - Calculate +DM and -DM between the first and second bar.
         *    The highest directional indication will indicate the assumed direction of the trade for the second price bar.
         *  - In the case of a tie between +DM and -DM, the direction is LONG by default.
         *
         * What is the initial "extreme point" and thus SAR?
         * ─────────────────────────────────────────────────
         * The following shows how different people took different approach:
         *  - Metastock use the first price bar high/low depending on the direction.
         *    No SAR is calculated for the first price bar.
         *  - Tradestation use the closing price of the second bar.
         *    No SAR are calculated for the first price bar.
         *  - Wilder (the original author) use the SIP from the previous trade
         *    (cannot be implemented here since the direction and length of the previous trade is unknown).
         *  - The Magazine TASC seems to follow Wilder approach which is not practical here.
         *
         * The library "consume" the first price bar and use its high/low as the initial SAR of the second price bar.
         * It has found that approach to be the closest to Wilder's idea of having
         * the first entry day use the previous extreme point, except that here the extreme point is
         * derived solely from the first price bar. I found the same approach to be used by Metastock.
         */

        var lookbackTotal = SarLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Make sure the acceleration and maximum are coherent. If not, correct the acceleration.
        optInAcceleration = Math.Min(optInAcceleration, optInMaximum);
        var af = optInAcceleration;

        // Identify if the initial direction is long or short.
        // (ep is just used as a temp buffer here, the name of the parameter is not significant).
        Span<T> epTemp = new T[1];
        var retCode = MinusDM(inHigh, inLow, startIdx, startIdx, epTemp, out _, out _, 1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        outBegIdx = startIdx;
        int outIdx = default;

        var todayIdx = startIdx;

        var newHigh = inHigh[todayIdx - 1];
        var newLow = inLow[todayIdx - 1];

        var isLong = epTemp[0] <= T.Zero;

        var sar = InitializeSar(inHigh, inLow, isLong, todayIdx, newLow, newHigh, out var ep);

        // Cheat on the newLow and newHigh for the first iteration.
        newLow = inLow[todayIdx];
        newHigh = inHigh[todayIdx];

        while (todayIdx <= endIdx)
        {
            var prevLow = newLow;
            var prevHigh = newHigh;
            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];
            todayIdx++;

            if (isLong)
            {
                // Switch to short if the low penetrates the SAR value.
                if (newLow <= sar)
                {
                    // Switch and override the SAR with the ep
                    isLong = false;
                    sar = SwitchToShort(ref ep, prevHigh, newLow, newHigh, out af, optInAcceleration, T.NegativeOne, ref outIdx, outReal);
                }
                else
                {
                    // No switch
                    // Output the SAR (was calculated in the previous iteration)
                    outReal[outIdx++] = sar;

                    sar = ProcessLongPosition(ref ep, prevLow, newLow, newHigh, ref af, optInAcceleration, optInMaximum, sar);
                }
            }
            /* Switch to long if the high penetrates the SAR value. */
            else if (newHigh >= sar)
            {
                /* Switch and override the SAR with the ep */
                isLong = true;
                sar = SwitchToLong(ref ep, prevLow, newLow, newHigh, out af, optInAcceleration, T.NegativeOne, ref outIdx, outReal);
            }
            else
            {
                // No switch
                // Output the SAR (was calculated in the previous iteration)
                outReal[outIdx++] = sar;

                sar = ProcessShortPosition(ref ep, prevHigh, newLow, newHigh, ref af, optInAcceleration, optInMaximum, sar);
            }
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int SarLookback() => 1;

    private static T InitializeSar<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        bool isLong,
        int todayIdx,
        T newLow,
        T newHigh,
        out T ep) where T : IFloatingPointIeee754<T>
    {
        T sar;
        if (isLong)
        {
            ep = inHigh[todayIdx];
            sar = newLow;
        }
        else
        {
            ep = inLow[todayIdx];
            sar = newHigh;
        }

        return sar;
    }

    private static T SwitchToShort<T>(
        ref T ep,
        T prevHigh,
        T newLow,
        T newHigh,
        out double af,
        double optInAcceleration,
        T optInOffsetOnReverse,
        ref int outIdx,
        Span<T> outReal) where T : IFloatingPointIeee754<T>
    {
        var sar = ep;

        // Make sure the overridden SAR is within yesterday's and today's range.
        sar = T.Max(sar, prevHigh);
        sar = T.Max(sar, newHigh);

        if (optInOffsetOnReverse > T.Zero)
        {
            sar += sar * optInOffsetOnReverse;
        }

        // Output the overridden SAR
        outReal[outIdx++] = sar * (optInOffsetOnReverse < T.Zero ? T.One : T.NegativeOne);

        // Adjust af and ep
        af = optInAcceleration;
        ep = newLow;

        sar += T.CreateChecked(af) * (ep - sar);

        // Make sure the new SAR is within yesterday's and today's range.
        sar = T.Max(sar, prevHigh);
        sar = T.Max(sar, newHigh);

        return sar;
    }

    private static T ProcessLongPosition<T>(
        ref T ep,
        T prevLow,
        T newLow,
        T newHigh,
        ref double af,
        double optInAcceleration,
        double optInMaximum,
        T sar) where T : IFloatingPointIeee754<T>
    {
        // Adjust af and ep.
        if (newHigh > ep)
        {
            ep = newHigh;
            af += optInAcceleration;
            af = Math.Min(af, optInMaximum);
        }

        // Calculate the new SAR
        sar += T.CreateChecked(af) * (ep - sar);

        // Make sure the new SAR is within yesterday's and today's range.
        sar = T.Min(sar, prevLow);
        sar = T.Min(sar, newLow);

        return sar;
    }

    private static T SwitchToLong<T>(
        ref T ep,
        T prevLow,
        T newLow,
        T newHigh,
        out double af,
        double optInAcceleration,
        T optInOffsetOnReverse,
        ref int outIdx,
        Span<T> outReal) where T : IFloatingPointIeee754<T>
    {
        var sar = ep;

        // Make sure the overridden SAR is within yesterday's and today's range.
        sar = T.Min(sar, prevLow);
        sar = T.Min(sar, newLow);

        if (optInOffsetOnReverse > T.Zero)
        {
            sar -= sar * optInOffsetOnReverse;
        }

        // Output the overridden SAR
        outReal[outIdx++] = sar;

        /* Adjust af and ep */
        af = optInAcceleration;
        ep = newHigh;

        sar += T.CreateChecked(af) * (ep - sar);

        // Make sure the new SAR is within yesterday's and today's range.
        sar = T.Min(sar, prevLow);
        sar = T.Min(sar, newLow);

        return sar;
    }

    private static T ProcessShortPosition<T>(
        ref T ep,
        T prevHigh,
        T newLow,
        T newHigh,
        ref double af,
        double optInAcceleration,
        double optInMaximum,
        T sar) where T : IFloatingPointIeee754<T>
    {
        // Adjust af and ep.
        if (newLow < ep)
        {
            ep = newLow;
            af += optInAcceleration;
            af = Math.Min(af, optInMaximum);
        }

        // Calculate the new SAR
        sar += T.CreateChecked(af) * (ep - sar);

        // Make sure the new SAR is within yesterday's and today's range.
        sar = T.Max(sar, prevHigh);
        sar = T.Max(sar, newHigh);

        return sar;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Sar<T>(
        T[] inHigh,
        T[] inLow,
        int startIdx,
        int endIdx,
        T[] outReal,
        double optInAcceleration = 0.02,
        double optInMaximum = 0.2) where T : IFloatingPointIeee754<T> =>
        Sar<T>(inHigh, inLow, startIdx, endIdx, outReal, out _, out _, optInAcceleration, optInMaximum);
}
