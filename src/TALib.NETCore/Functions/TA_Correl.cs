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
    public static Core.RetCode Correl(
        ReadOnlySpan<T> inReal0,
        ReadOnlySpan<T> inReal1,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30)
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

        var lookbackTotal = CorrelLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        var trailingIdx = startIdx - lookbackTotal;

        T sumX, sumY, sumX2, sumY2;
        T sumXY = sumX = sumY = sumX2 = sumY2 = T.Zero;
        int today;
        for (today = trailingIdx; today <= startIdx; today++)
        {
            T x = inReal0[today];
            sumX += x;
            sumX2 += x * x;

            T y = inReal1[today];
            sumXY += x * y;
            sumY += y;
            sumY2 += y * y;
        }

        T timePeriod = T.CreateChecked(optInTimePeriod);

        T trailingX = inReal0[trailingIdx];
        T trailingY = inReal1[trailingIdx++];
        T tempReal = (sumX2 - sumX * sumX / timePeriod) * (sumY2 - sumY * sumY / timePeriod);
        outReal[0] = tempReal > T.Zero ? (sumXY - sumX * sumY / timePeriod) / T.Sqrt(tempReal) : T.Zero;

        var outIdx = 1;
        while (today <= endIdx)
        {
            sumX -= trailingX;
            sumX2 -= trailingX * trailingX;

            sumXY -= trailingX * trailingY;
            sumY -= trailingY;
            sumY2 -= trailingY * trailingY;

            T x = inReal0[today];
            sumX += x;
            sumX2 += x * x;

            T y = inReal1[today++];
            sumXY += x * y;
            sumY += y;
            sumY2 += y * y;

            trailingX = inReal0[trailingIdx];
            trailingY = inReal1[trailingIdx++];
            tempReal = (sumX2 - sumX * sumX / timePeriod) * (sumY2 - sumY * sumY / timePeriod);
            outReal[outIdx++] = tempReal > T.Zero ? (sumXY - sumX * sumY / timePeriod) / T.Sqrt(tempReal) : T.Zero;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CorrelLookback(int optInTimePeriod = 30) => optInTimePeriod < 1 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Correl(
        T[] inReal0,
        T[] inReal1,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 30) => Correl(inReal0, inReal1, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
