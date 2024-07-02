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
    public static Core.RetCode StochF<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outFastK,
        Span<T> outFastD,
        out int outBegIdx,
        out int outNbElement,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInFastKPeriod < 1 || optInFastDPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* With stochastic, there is a total of 4 different lines that are defined: FastK, FastD, SlowK and SlowD.
         *
         * The D is the signal line usually drawn over its corresponding K function.
         *
         *                    (Today's Close - LowestLow)
         *  FastK(Kperiod) =  ─────────────────────────── * 100
         *                     (HighestHigh - LowestLow)
         *
         *  FastD(FastDperiod, MA type) = MA Smoothed FastK over FastDperiod
         *
         *  SlowK(SlowKperiod, MA type) = MA Smoothed FastK over SlowKperiod
         *
         *  SlowD(SlowDperiod, MA Type) = MA Smoothed SlowK over SlowDperiod
         *
         * The HighestHigh and LowestLow are the extreme values among the last 'Kperiod'.
         *
         * SlowK and FastD are equivalent when using the same period.
         *
         * The following shows how these four lines are made available in the library:
         *
         *  Stoch  : Returns the SlowK and SlowD
         *  StochF : Returns the FastK and FastD
         *
         * The Stoch function correspond to the more widely implemented version found in much software/charting package.
         * The StochF is more rarely used because its higher volatility cause often whipsaws.
         */

        var lookbackK = optInFastKPeriod - 1;
        var lookbackFastD = MaLookback(optInFastDPeriod, optInFastDMAType);
        var lookbackTotal = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        /* Do the K calculation:
         *
         *    Kt = 100 * ((Ct - Lt) / (Ht - Lt))
         *
         * Kt is today stochastic
         * Ct is today closing price.
         * Lt is the lowest price of the last K Period (including today)
         * Ht is the highest price of the last K Period (including today)
         */

        // Proceed with the calculation for the requested range.
        // The algorithm allows the input and output to be the same buffer.
        int outIdx = default;

        // Calculate just enough K for ending up with the caller requested range.
        // (The range of k must consider allthe lookback involve with the smoothing).
        var trailingIdx = startIdx - lookbackTotal;
        var today = trailingIdx + lookbackK;

        // Allocate a temporary buffer large enough to store the K.
        // If the output is the same as the input, just save one memory allocation.
        Span<T> tempBuffer;
        if (outFastK == inHigh || outFastK == inLow || outFastK == inClose)
        {
            tempBuffer = outFastK;
        }
        else if (outFastD == inHigh || outFastD == inLow || outFastD == inClose)
        {
            tempBuffer = outFastD;
        }
        else
        {
            tempBuffer = new T[endIdx - today + 1];
        }

        // Do the K calculation
        int highestIdx = -1, lowestIdx = -1;
        T highest = T.Zero, lowest = T.Zero;
        while (today <= endIdx)
        {
            (lowestIdx, lowest) = CalcLowest(inLow, trailingIdx, today, lowestIdx, lowest);
            (highestIdx, highest) = CalcHighest(inHigh, trailingIdx, today, highestIdx, highest);

            var diff = (highest - lowest) / Hundred<T>();

            // Calculate stochastic.
            tempBuffer[outIdx++] = !T.IsZero(diff) ? (inClose[today] - lowest) / diff : T.Zero;

            trailingIdx++;
            today++;
        }

        // Fast-K calculation completed. This K calculation is returned to the caller. It is smoothed to become Fast-D.
        var retCode = Ma(tempBuffer, 0, outIdx - 1, outFastD, out _, out outNbElement, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        /* Copy tempBuffer into the caller buffer.
         * (Calculation could not be done directly in the caller buffer because
         * more input data than the requested range was needed for doing %D).
         */
        tempBuffer.Slice(lookbackFastD, outNbElement).CopyTo(outFastK);
        outBegIdx = startIdx;

        return Core.RetCode.Success;
    }

    public static int StochFLookback(int optInFastKPeriod = 5, int optInFastDPeriod = 3, Core.MAType optInFastDMAType = Core.MAType.Sma) =>
        optInFastKPeriod < 1 || optInFastDPeriod < 1 ? -1 : optInFastKPeriod - 1 + MaLookback(optInFastDPeriod, optInFastDMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode StochF<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outFastK,
        T[] outFastD,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochF<T>(inHigh, inLow, inClose, startIdx, endIdx, outFastK, outFastD, out _, out _, optInFastKPeriod, optInFastDPeriod,
            optInFastDMAType);
}
