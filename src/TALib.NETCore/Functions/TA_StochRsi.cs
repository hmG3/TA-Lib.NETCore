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
    public static Core.RetCode StochRsi<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outFastK,
        Span<T> outFastD,
        out Range outRange,
        int optInTimePeriod = 14,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochRsiImpl(inReal, inRange, outFastK, outFastD, out outRange, optInTimePeriod, optInFastKPeriod, optInFastDPeriod,
            optInFastDMAType);

    [PublicAPI]
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
    [UsedImplicitly]
    private static Core.RetCode StochRsi<T>(
        T[] inReal,
        Range inRange,
        T[] outFastK,
        T[] outFastD,
        out Range outRange,
        int optInTimePeriod = 14,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochRsiImpl<T>(inReal, inRange, outFastK, outFastD, out outRange, optInTimePeriod, optInFastKPeriod, optInFastDPeriod,
            optInFastDMAType);

    private static Core.RetCode StochRsiImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outFastK,
        Span<T> outFastD,
        out Range outRange,
        int optInTimePeriod,
        int optInFastKPeriod,
        int optInFastDPeriod,
        Core.MAType optInFastDMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        var startIdx = inRange.Start.Value;
        var endIdx = inRange.End.Value;

        if (endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2 || optInFastKPeriod < 1 || optInFastDPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* Reference: "Stochastic RSI and Dynamic Momentum Index"
         *            by Tushar Chande and Stanley Kroll
         *            Stock&Commodities V.11:5 (189-199)
         *
         * The version offer flexibility beyond what is explained in the Stock&Commodities article.
         *
         * To calculate the "Unsmoothed stochastic RSI" with symmetry like explain in the article,
         * keep the optInTimePeriod and optInFastKPeriod equal. Example:
         *
         *   un-smoothed stoch RSI 14 : optInTimePeriod   = 14
         *                              optInFastK_Period = 14
         *                              optInFastD_Period = 'x'
         *
         * The outFastK is the un-smoothed RSI discuss in the article.
         *
         * optInFastDPeriod can be set to smooth the RSI. The smooth* version will be found in outFastD.
         * The outFastK will still contain the un-smoothed stoch RSI.
         * If the smoothing of the StochRSI is not a concern, optInFastDPeriod should be left at 1, and outFastD can be ignored.
         */

        var lookbackStochF = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        var lookbackTotal = StochRsiLookback(optInTimePeriod, optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;
        var tempArraySize = endIdx - startIdx + 1 + lookbackStochF;
        Span<T> tempRsiBuffer = new T[tempArraySize];
        var retCode = RsiImpl(inReal, new Range(startIdx - lookbackStochF, endIdx), tempRsiBuffer, out var outRange1, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outRange1.End.Value == 0)
        {
            return retCode;
        }

        retCode = StochFImpl(tempRsiBuffer, tempRsiBuffer, tempRsiBuffer, Range.EndAt(tempArraySize - 1), outFastK, outFastD, out outRange,
            optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outRange.End.Value == 0)
        {
            return retCode;
        }

        outRange = new Range(outBegIdx, outBegIdx + outRange.End.Value - outRange.Start.Value);

        return Core.RetCode.Success;
    }
}
