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
    public static Core.RetCode MacdExt<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod = 12,
        Core.MAType optInFastMAType = Core.MAType.Sma,
        int optInSlowPeriod = 26,
        Core.MAType optInSlowMAType = Core.MAType.Sma,
        int optInSignalPeriod = 9,
        Core.MAType optInSignalMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MacdExtImpl(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, optInFastPeriod,
            optInFastMAType, optInSlowPeriod, optInSlowMAType, optInSignalPeriod, optInSignalMAType);

    [PublicAPI]
    public static int MacdExtLookback(
        int optInFastPeriod = 12,
        Core.MAType optInFastMAType = Core.MAType.Sma,
        int optInSlowPeriod = 26,
        Core.MAType optInSlowMAType = Core.MAType.Sma,
        int optInSignalPeriod = 9,
        Core.MAType optInSignalMAType = Core.MAType.Sma)
    {
        if (optInFastPeriod < 2 || optInSlowPeriod < 2 || optInSignalPeriod < 1)
        {
            return -1;
        }

        var lookbackLargest = MaLookback(optInFastPeriod, optInFastMAType);
        var tempInteger = MaLookback(optInSlowPeriod, optInSlowMAType);
        if (tempInteger > lookbackLargest)
        {
            lookbackLargest = tempInteger;
        }

        return lookbackLargest + MaLookback(optInSignalPeriod, optInSignalMAType);
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MacdExt<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outMACD,
        T[] outMACDSignal,
        T[] outMACDHist,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod = 12,
        Core.MAType optInFastMAType = Core.MAType.Sma,
        int optInSlowPeriod = 26,
        Core.MAType optInSlowMAType = Core.MAType.Sma,
        int optInSignalPeriod = 9,
        Core.MAType optInSignalMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MacdExtImpl<T>(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, optInFastPeriod,
            optInFastMAType, optInSlowPeriod, optInSlowMAType, optInSignalPeriod, optInSignalMAType);

    private static Core.RetCode MacdExtImpl<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod,
        Core.MAType optInFastMAType,
        int optInSlowPeriod,
        Core.MAType optInSlowMAType,
        int optInSignalPeriod,
        Core.MAType optInSignalMAType) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInFastPeriod < 2 || optInSlowPeriod < 2 || optInSignalPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        // Make sure slow is really slower than the fast period. if not, swap.
        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            (optInSlowMAType, optInFastMAType) = (optInFastMAType, optInSlowMAType);
        }

        // Add the lookback needed for the signal line
        var lookbackSignal = MaLookback(optInSignalPeriod, optInSignalMAType);
        var lookbackTotal = MacdExtLookback(optInFastPeriod, optInFastMAType, optInSlowPeriod, optInSlowMAType, optInSignalPeriod,
            optInSignalMAType);

        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Allocate intermediate buffer for fast/slow MA.
        var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
        Span<T> fastMABuffer = new T[tempInteger];
        Span<T> slowMABuffer = new T[tempInteger];

        /* Calculate the slow MA.
         *
         * Move back the startIdx to get enough data for the signal period.
         * That way, once the signal calculation is done, all the output will start at the requested 'startIdx'.
         */
        tempInteger = startIdx - lookbackSignal;
        var retCode = Ma(inReal, tempInteger, endIdx, slowMABuffer, out _, out var outNbElement1, optInSlowPeriod, optInSlowMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Calculate the fast MA.
        retCode = Ma(inReal, tempInteger, endIdx, fastMABuffer, out _, out var outNbElement2, optInFastPeriod, optInFastMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Calculate (fast MA) - (slow MA).
        for (var i = 0; i < outNbElement1; i++)
        {
            fastMABuffer[i] -= slowMABuffer[i];
        }

        // Copy the result into the output for the caller.
        fastMABuffer.Slice(lookbackSignal, endIdx - startIdx + 1).CopyTo(outMACD);

        // Calculate the signal/trigger line.
        retCode = Ma(fastMABuffer, 0, outNbElement1 - 1, outMACDSignal, out _, out outNbElement2, optInSignalPeriod, optInSignalMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Calculate the histogram.
        for (var i = 0; i < outNbElement2; i++)
        {
            outMACDHist[i] = outMACD[i] - outMACDSignal[i];
        }

        outBegIdx = startIdx;
        outNbElement = outNbElement2;

        return Core.RetCode.Success;
    }
}
