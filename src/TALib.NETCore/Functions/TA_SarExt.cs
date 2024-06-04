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
    public static Core.RetCode SarExt<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        double optInStartValue = 0.0,
        double optInOffsetOnReverse = 0.0,
        double optInAccelerationInitLong = 0.02,
        double optInAccelerationLong = 0.02,
        double optInAccelerationMaxLong = 0.2,
        double optInAccelerationInitShort = 0.02,
        double optInAccelerationShort = 0.02,
        double optInAccelerationMaxShort = 0.2) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inHigh.Length || endIdx >= inLow.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInOffsetOnReverse < 0.0 || optInAccelerationInitLong < 0.0 || optInAccelerationLong < 0.0 ||
            optInAccelerationMaxLong < 0.0 || optInAccelerationInitShort < 0.0 || optInAccelerationShort < 0.0 ||
            optInAccelerationMaxShort < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = SarExtLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T sar;
        T ep;
        bool isLong;

        var afLong = optInAccelerationInitLong;
        var afShort = optInAccelerationInitShort;
        if (afLong > optInAccelerationMaxLong)
        {
            optInAccelerationInitLong = optInAccelerationMaxLong;
            afLong = optInAccelerationInitLong;
        }

        if (optInAccelerationLong > optInAccelerationMaxLong)
        {
            optInAccelerationLong = optInAccelerationMaxLong;
        }

        if (afShort > optInAccelerationMaxShort)
        {
            optInAccelerationInitShort = optInAccelerationMaxShort;
            afShort = optInAccelerationInitShort;
        }

        if (optInAccelerationShort > optInAccelerationMaxShort)
        {
            optInAccelerationShort = optInAccelerationMaxShort;
        }

        if (optInStartValue.Equals(0.0))
        {
            Span<T> epTemp = new T[1];
            var retCode = MinusDM(inHigh, inLow, startIdx, startIdx, epTemp, out _, out _, 1);
            if (retCode != Core.RetCode.Success)
            {
                return retCode;
            }

            isLong = epTemp[0] <= T.Zero;
        }
        else if (optInStartValue > 0.0)
        {
            isLong = true;
        }
        else
        {
            isLong = false;
        }

        outBegIdx = startIdx;
        int outIdx = default;

        var todayIdx = startIdx;

        T newHigh = inHigh[todayIdx - 1];
        T newLow = inLow[todayIdx - 1];
        if (optInStartValue.Equals(0.0))
        {
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
        }
        else if (optInStartValue > 0.0)
        {
            ep = inHigh[todayIdx];
            sar = T.CreateChecked(optInStartValue);
        }
        else
        {
            ep = inLow[todayIdx];
            sar = T.CreateChecked(Math.Abs(optInStartValue));
        }

        newLow = inLow[todayIdx];
        newHigh = inHigh[todayIdx];

        while (todayIdx <= endIdx)
        {
            T prevLow = newLow;
            T prevHigh = newHigh;
            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx++];
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

                    if (!optInOffsetOnReverse.Equals(0.0))
                    {
                        sar += sar * T.CreateChecked(optInOffsetOnReverse);
                    }

                    outReal[outIdx++] = -sar;

                    afShort = optInAccelerationInitShort;
                    ep = newLow;

                    sar += T.CreateChecked(afShort) * (ep - sar);

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
                        afLong += optInAccelerationLong;
                        if (afLong > optInAccelerationMaxLong)
                        {
                            afLong = optInAccelerationMaxLong;
                        }
                    }

                    sar += T.CreateChecked(afLong) * (ep - sar);

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

                if (!optInOffsetOnReverse.Equals(0.0))
                {
                    sar -= sar * T.CreateChecked(optInOffsetOnReverse);
                }

                outReal[outIdx++] = sar;

                afLong = optInAccelerationInitLong;
                ep = newHigh;

                sar += T.CreateChecked(afLong) * (ep - sar);

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
                outReal[outIdx++] = -sar;
                if (newLow < ep)
                {
                    ep = newLow;
                    afShort += optInAccelerationShort;
                    if (afShort > optInAccelerationMaxShort)
                    {
                        afShort = optInAccelerationMaxShort;
                    }
                }

                sar += T.CreateChecked(afShort) * (ep - sar);

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

    public static int SarExtLookback() => 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode SarExt<T>(
        T[] inHigh,
        T[] inLow,
        int startIdx,
        int endIdx,
        T[] outReal,
        double optInStartValue = 0.0,
        double optInOffsetOnReverse = 0.0,
        double optInAccelerationInitLong = 0.02,
        double optInAccelerationLong = 0.02,
        double optInAccelerationMaxLong = 0.2,
        double optInAccelerationInitShort = 0.02,
        double optInAccelerationShort = 0.02,
        double optInAccelerationMaxShort = 0.2) where T : IFloatingPointIeee754<T> =>
        SarExt<T>(inHigh, inLow, startIdx, endIdx, outReal, out _, out _, optInStartValue, optInOffsetOnReverse, optInAccelerationInitLong,
            optInAccelerationLong, optInAccelerationMaxLong, optInAccelerationInitShort, optInAccelerationShort, optInAccelerationMaxShort);
}
