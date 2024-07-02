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
    public static Core.RetCode Mom<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 10) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

         // The Mom function is the only one who is not normalized, and thus
         // should be avoided for comparing different time series of prices.

        var lookbackTotal = MomLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Calculate Momentum. Just subtract the value from 'period' ago from current value.
        int outIdx = default;
        var inIdx = startIdx;
        var trailingIdx = startIdx - lookbackTotal;
        while (inIdx <= endIdx)
        {
            outReal[outIdx++] = inReal[inIdx++] - inReal[trailingIdx++];
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MomLookback(int optInTimePeriod = 10) => optInTimePeriod < 1 ? -1 : optInTimePeriod;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Mom<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 10) where T : IFloatingPointIeee754<T> =>
        Mom<T>(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
