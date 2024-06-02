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
    public static Core.RetCode Wma(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = WmaLookback(optInTimePeriod);
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

        T periodSub = T.Zero;
        T periodSum = periodSub;
        var inIdx = trailingIdx;
        var i = 1;
        while (inIdx < startIdx)
        {
            T tempReal = inReal[inIdx++];
            periodSub += tempReal;
            periodSum += tempReal * T.CreateChecked(i);
            i++;
        }

        T trailingValue = T.Zero;

        T divider = T.CreateChecked((optInTimePeriod * (optInTimePeriod + 1)) >> 1);
        T timePeriod = T.CreateChecked(optInTimePeriod);
        while (inIdx <= endIdx)
        {
            T tempReal = inReal[inIdx++];
            periodSub += tempReal;
            periodSub -= trailingValue;
            periodSum += tempReal * timePeriod;
            trailingValue = inReal[trailingIdx++];
            outReal[outIdx++] = periodSum / divider;
            periodSum -= periodSub;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int WmaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Wma(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 30) => Wma(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
