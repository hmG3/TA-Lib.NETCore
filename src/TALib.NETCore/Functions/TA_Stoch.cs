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
    public static Core.RetCode Stoch<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outSlowK,
        Span<T> outSlowD,
        out int outBegIdx,
        out int outNbElement,
        int optInFastKPeriod = 5,
        int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3,
        Core.MAType optInSlowDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInFastKPeriod < 1 || optInSlowKPeriod < 1 || optInSlowDPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackK = optInFastKPeriod - 1;
        var lookbackDSlow = MaLookback(optInSlowDPeriod, optInSlowDMAType);
        var lookbackTotal = StochLookback(optInFastKPeriod, optInSlowKPeriod, optInSlowKMAType, optInSlowDPeriod, optInSlowDMAType);
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
        if (outSlowK == inHigh || outSlowK == inLow || outSlowK == inClose)
        {
            tempBuffer = outSlowK.ToArray();
        }
        else if (outSlowD == inHigh || outSlowD == inLow || outSlowD == inClose)
        {
            tempBuffer = outSlowD.ToArray();
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

        var retCode = Ma(tempBuffer, 0, outIdx - 1, tempBuffer, out _, out outNbElement, optInSlowKPeriod, optInSlowKMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        retCode = Ma(tempBuffer, 0, outNbElement - 1, outSlowD, out _, out outNbElement, optInSlowDPeriod, optInSlowDMAType);
        tempBuffer.Slice(lookbackDSlow, outNbElement).CopyTo(outSlowK);
        if (retCode != Core.RetCode.Success)
        {
            outNbElement = 0;

            return retCode;
        }

        outBegIdx = startIdx;

        return Core.RetCode.Success;
    }

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
        int startIdx,
        int endIdx,
        T[] outSlowK,
        T[] outSlowD,
        int optInFastKPeriod = 5,
        int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3,
        Core.MAType optInSlowDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        Stoch<T>(inHigh, inLow, inClose, startIdx, endIdx, outSlowK, outSlowD, out _, out _, optInFastKPeriod, optInSlowKPeriod,
            optInSlowKMAType, optInSlowDPeriod, optInSlowDMAType);
}
