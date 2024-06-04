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
    public static Core.RetCode StochRsi<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outFastK,
        Span<T> outFastD,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2 || optInFastKPeriod < 1 || optInFastDPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackStochF = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        var lookbackTotal = StochRsiLookback(optInTimePeriod, optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        var tempArraySize = endIdx - startIdx + 1 + lookbackStochF;
        Span<T> tempRsiBuffer = new T[tempArraySize];
        var retCode = Rsi(inReal, startIdx - lookbackStochF, endIdx, tempRsiBuffer, out _, out var outNbElement1, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElement1 == 0)
        {
            return retCode;
        }

        retCode = StochF(tempRsiBuffer, tempRsiBuffer, tempRsiBuffer, 0, tempArraySize - 1, outFastK, outFastD, out _, out outNbElement,
            optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        return Core.RetCode.Success;
    }

    public static int StochRsiLookback(
        int optInTimePeriod = 14,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) =>
        optInTimePeriod < 2 || optInFastKPeriod < 1 || optInFastDPeriod < 1
            ? -1
            : RsiLookback(optInTimePeriod) + StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode StochRsi<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outFastK,
        T[] outFastD,
        int optInTimePeriod = 14,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochRsi<T>(inReal, startIdx, endIdx, outFastK, outFastD, out _, out _, optInTimePeriod, optInFastKPeriod, optInFastDPeriod,
            optInFastDMAType);
}
