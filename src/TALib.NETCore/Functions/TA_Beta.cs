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
    public static Core.RetCode Beta<T>(
        ReadOnlySpan<T> inReal0,
        ReadOnlySpan<T> inReal1,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 5) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal0.Length || endIdx >= inReal1.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = BetaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T x, y, tmpReal, sxy, sx, sy;
        T sxx = sxy = sx = sy = T.Zero;
        var trailingIdx = startIdx - lookbackTotal;
        var trailingLastPriceX = inReal0[trailingIdx];
        var lastPriceX = trailingLastPriceX;
        var trailingLastPriceY = inReal1[trailingIdx];
        var lastPriceY = trailingLastPriceY;

        var i = ++trailingIdx;
        while (i < startIdx)
        {
            tmpReal = inReal0[i];
            x = !T.IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : T.Zero;
            lastPriceX = tmpReal;

            tmpReal = inReal1[i++];
            y = !T.IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : T.Zero;
            lastPriceY = tmpReal;

            sxx += x * x;
            sxy += x * y;
            sx += x;
            sy += y;
        }

        T timePeriod = T.CreateChecked(optInTimePeriod);

        int outIdx = default;
        do
        {
            tmpReal = inReal0[i];
            x = !T.IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : T.Zero;
            lastPriceX = tmpReal;

            tmpReal = inReal1[i++];
            y = !T.IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : T.Zero;
            lastPriceY = tmpReal;

            sxx += x * x;
            sxy += x * y;
            sx += x;
            sy += y;

            tmpReal = inReal0[trailingIdx];
            x = !T.IsZero(trailingLastPriceX) ? (tmpReal - trailingLastPriceX) / trailingLastPriceX : T.Zero;
            trailingLastPriceX = tmpReal;

            tmpReal = inReal1[trailingIdx++];
            y = !T.IsZero(trailingLastPriceY) ? (tmpReal - trailingLastPriceY) / trailingLastPriceY : T.Zero;
            trailingLastPriceY = tmpReal;

            tmpReal = timePeriod * sxx - sx * sx;
            outReal[outIdx++] = !T.IsZero(tmpReal) ? (timePeriod * sxy - sx * sy) / tmpReal : T.Zero;

            sxx -= x * x;
            sxy -= x * y;
            sx -= x;
            sy -= y;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int BetaLookback(int optInTimePeriod = 5) => optInTimePeriod < 1 ? -1 : optInTimePeriod;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Beta<T>(
        T[] inReal0,
        T[] inReal1,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 5) where T : IFloatingPointIeee754<T> =>
        Beta<T>(inReal0, inReal1, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
