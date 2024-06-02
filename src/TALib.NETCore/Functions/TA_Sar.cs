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

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Sar(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        double optInAcceleration = 0.02,
        double optInMaximum = 0.2)
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

        var lookbackTotal = SarLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var af = optInAcceleration;
        if (af > optInMaximum)
        {
            af = optInAcceleration = optInMaximum;
        }

        Span<T> epTemp = new T[1];
        var retCode = MinusDM(inHigh, inLow, startIdx, startIdx, epTemp, out _, out _, 1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        outBegIdx = startIdx;
        T sar;
        T ep;
        int outIdx = default;

        var todayIdx = startIdx;

        T newHigh = inHigh[todayIdx - 1];
        T newLow = inLow[todayIdx - 1];
        var isLong = epTemp[0] <= T.Zero;
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

        newLow = inLow[todayIdx];
        newHigh = inHigh[todayIdx];

        while (todayIdx <= endIdx)
        {
            T prevLow = newLow;
            T prevHigh = newHigh;
            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];
            todayIdx++;

            if (isLong)
            {
                if (newLow <= sar)
                {
                    isLong = false;
                    sar = ep;

                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }

                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }

                    outReal[outIdx++] = sar;

                    af = optInAcceleration;
                    ep = newLow;

                    sar += T.CreateChecked(af) * (ep - sar);
                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }

                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }
                }
                else
                {
                    outReal[outIdx++] = sar;
                    if (newHigh > ep)
                    {
                        ep = newHigh;
                        af += optInAcceleration;
                        if (af > optInMaximum)
                        {
                            af = optInMaximum;
                        }
                    }

                    sar += T.CreateChecked(af) * (ep - sar);
                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }

                    if (sar > newLow)
                    {
                        sar = newLow;
                    }
                }
            }
            else if (newHigh >= sar)
            {
                isLong = true;
                sar = ep;

                if (sar > prevLow)
                {
                    sar = prevLow;
                }

                if (sar > newLow)
                {
                    sar = newLow;
                }

                outReal[outIdx++] = sar;

                af = optInAcceleration;
                ep = newHigh;

                sar += T.CreateChecked(af) * (ep - sar);
                if (sar > prevLow)
                {
                    sar = prevLow;
                }

                if (sar > newLow)
                {
                    sar = newLow;
                }
            }
            else
            {
                outReal[outIdx++] = sar;

                if (newLow < ep)
                {
                    ep = newLow;
                    af += optInAcceleration;
                    if (af > optInMaximum)
                    {
                        af = optInMaximum;
                    }
                }

                sar += T.CreateChecked(af) * (ep - sar);

                if (sar < prevHigh)
                {
                    sar = prevHigh;
                }

                if (sar < newHigh)
                {
                    sar = newHigh;
                }
            }
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int SarLookback() => 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Sar(
        T[] inHigh,
        T[] inLow,
        int startIdx,
        int endIdx,
        T[] outReal,
        double optInAcceleration = 0.02,
        double optInMaximum = 0.2) => Sar(inHigh, inLow, startIdx, endIdx, outReal, out _, out _, optInAcceleration, optInMaximum);
}
