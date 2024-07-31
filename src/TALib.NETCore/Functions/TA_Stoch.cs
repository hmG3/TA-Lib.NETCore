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
    [PublicAPI]
    public static Core.RetCode Stoch<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outSlowK,
        Span<T> outSlowD,
        out Range outRange,
        int optInFastKPeriod = 5,
        int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3,
        Core.MAType optInSlowDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochImpl(inHigh, inLow, inClose, inRange, outSlowK, outSlowD, out outRange, optInFastKPeriod, optInSlowKPeriod, optInSlowKMAType,
            optInSlowDPeriod, optInSlowDMAType);

    [PublicAPI]
    public static int StochLookback(
        int optInFastKPeriod = 5,
        int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3,
        Core.MAType optInSlowDMAType = Core.MAType.Sma)
    {
        if (optInFastKPeriod < 1 || optInSlowKPeriod < 1 || optInSlowDPeriod < 1)
        {
            return -1;
        }

        var retValue = optInFastKPeriod - 1;
        retValue += MaLookback(optInSlowKPeriod, optInSlowKMAType);
        retValue += MaLookback(optInSlowDPeriod, optInSlowDMAType);

        return retValue;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Stoch<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outSlowK,
        T[] outSlowD,
        out Range outRange,
        int optInFastKPeriod = 5,
        int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3,
        Core.MAType optInSlowDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochImpl<T>(inHigh, inLow, inClose, inRange, outSlowK, outSlowD, out outRange, optInFastKPeriod, optInSlowKPeriod,
            optInSlowKMAType, optInSlowDPeriod, optInSlowDMAType);

    private static Core.RetCode StochImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outSlowK,
        Span<T> outSlowD,
        out Range outRange,
        int optInFastKPeriod,
        int optInSlowKPeriod,
        Core.MAType optInSlowKMAType,
        int optInSlowDPeriod,
        Core.MAType optInSlowDMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInFastKPeriod < 1 || optInSlowKPeriod < 1 || optInSlowDPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* With stochastic, there is a total of 4 different lines that are defined: FastK, FastD, SlowK and SlowD.
         *
         * The D is the signal line usually drawn over its corresponding K function.
         *
         *                     (Today's Close - LowestLow)
         *   FastK(Kperiod) =  ─────────────────────────── * 100
         *                      (HighestHigh - LowestLow)
         *
         *   FastD(FastDperiod, MA type) = MA Smoothed FastK over FastDperiod
         *
         *   SlowK(SlowKperiod, MA type) = MA Smoothed FastK over SlowKperiod
         *
         *   SlowD(SlowDperiod, MA Type) = MA Smoothed SlowK over SlowDperiod
         *
         * The HighestHigh and LowestLow are the extreme values among the last 'Kperiod'.
         *
         * SlowK and FastD are equivalent when using the same period.
         *
         * The following shows how these four lines are made available in the library:
         *
         *   Stoch  : Returns the SlowK and SlowD
         *   StochF : Returns the FastK and FastD
         *
         * The Stoch function correspond to the more widely implemented version found in much software/charting package.
         * The StochF is more rarely used because its higher volatility cause often whipsaws.
         */

        var lookbackK = optInFastKPeriod - 1;
        var lookbackDSlow = MaLookback(optInSlowDPeriod, optInSlowDMAType);
        var lookbackTotal = StochLookback(optInFastKPeriod, optInSlowKPeriod, optInSlowKMAType, optInSlowDPeriod, optInSlowDMAType);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        /* Do the K calculation:
         *
         *   Kt = 100 * ((Ct - Lt) / (Ht - Lt))
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
        // (The range of k must consider all the lookback involve with the smoothing).
        var trailingIdx = startIdx - lookbackTotal;
        var today = trailingIdx + lookbackK;

        // Allocate a temporary buffer large enough to store the K.
        // If the output is the same as the input, just save one memory allocation.
        Span<T> tempBuffer;
        if (outSlowK == inHigh || outSlowK == inLow || outSlowK == inClose)
        {
            tempBuffer = outSlowK;
        }
        else if (outSlowD == inHigh || outSlowD == inLow || outSlowD == inClose)
        {
            tempBuffer = outSlowD;
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

        /* Un-smoothed K calculation completed. This K calculation is not returned to the caller.
         * It is always smoothed and then return.
         * Some documentation will refer to the smoothed version as being "K-Slow", but often this end up to be shortened to "K".
         */
        var retCode = MaImpl(tempBuffer, Range.EndAt(outIdx - 1), tempBuffer, out outRange, optInSlowKPeriod, optInSlowKMAType);
        if (retCode != Core.RetCode.Success || outRange.End.Value == 0)
        {
            return retCode;
        }

        var nbElement = outRange.End.Value - outRange.Start.Value;
        // Calculate the %D which is simply a moving average of the already smoothed %K.
        retCode = MaImpl(tempBuffer, Range.EndAt(nbElement - 1), outSlowD, out outRange, optInSlowDPeriod, optInSlowDMAType);
        nbElement = outRange.End.Value - outRange.Start.Value;

        /* Copy tempBuffer into the caller buffer.
         * (Calculation could not be done directly in the caller buffer because
         * more input data than the requested range was needed for doing %D).
         */
        tempBuffer.Slice(lookbackDSlow, nbElement).CopyTo(outSlowK);
        if (retCode != Core.RetCode.Success)
        {
            outRange = Range.EndAt(0);

            return retCode;
        }

        outRange = new Range(startIdx, startIdx + nbElement);

        return Core.RetCode.Success;
    }
}
