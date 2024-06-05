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

        int outIdx = default;
        var trailingIdx = startIdx - lookbackTotal;
        var today = trailingIdx + lookbackK;
        var highestIdx = -1;
        var lowestIdx = highestIdx;
        T highest, lowest;
        T diff = highest = lowest = T.Zero;
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

        while (today <= endIdx)
        {
            T tmp = inLow[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inLow[lowestIdx];
                var i = lowestIdx;
                while (++i <= today)
                {
                    tmp = inLow[i];
                    if (tmp < lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }

                diff = (highest - lowest) / Hundred<T>();
            }
            else if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
                diff = (highest - lowest) / Hundred<T>();
            }

            tmp = inHigh[today];
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inHigh[highestIdx];
                var i = highestIdx;
                while (++i <= today)
                {
                    tmp = inHigh[i];
                    if (tmp > highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }

                diff = (highest - lowest) / Hundred<T>();
            }
            else if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
                diff = (highest - lowest) / Hundred<T>();
            }

            tempBuffer[outIdx++] = !T.IsZero(diff) ? (inClose[today] - lowest) / diff : T.Zero;

            trailingIdx++;
            today++;
        }

        var retCode = Ma(tempBuffer, 0, outIdx - 1, outFastD, out _, out outNbElement, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

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
