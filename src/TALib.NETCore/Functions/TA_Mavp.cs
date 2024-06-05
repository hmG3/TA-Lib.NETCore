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
    public static Core.RetCode Mavp<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inPeriods,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInMinPeriod = 2,
        int optInMaxPeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInMinPeriod < 2 || optInMaxPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MavpLookback(optInMaxPeriod, optInMAType);
        if (inPeriods.Length < lookbackTotal)
        {
            return Core.RetCode.BadParam;
        }

        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInt = lookbackTotal > startIdx ? lookbackTotal : startIdx;
        if (tempInt > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outputSize = endIdx - tempInt + 1;
        Span<T> localOutputArray = new T[outputSize];
        Span<int> localPeriodArray = new int[outputSize];

        for (var i = 0; i < outputSize; i++)
        {
            var period = Int32.CreateTruncating(inPeriods[startIdx + i]);
            localPeriodArray[i] = Math.Clamp(period, optInMinPeriod, optInMaxPeriod);
        }

        for (var i = 0; i < outputSize; i++)
        {
            var curPeriod = localPeriodArray[i];
            if (curPeriod != 0)
            {
                var retCode = Ma(inReal, startIdx, endIdx, localOutputArray, out _, out _, curPeriod, optInMAType);
                if (retCode != Core.RetCode.Success)
                {
                    return retCode;
                }

                outReal[i] = localOutputArray[i];
                for (var j = i + 1; j < outputSize; j++)
                {
                    if (localPeriodArray[j] == curPeriod)
                    {
                        localPeriodArray[j] = 0;
                        outReal[j] = localOutputArray[j];
                    }
                }
            }
        }

        outBegIdx = startIdx;
        outNbElement = outputSize;

        return Core.RetCode.Success;
    }

    public static int MavpLookback(int optInMaxPeriod = 30, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInMaxPeriod < 2 ? -1 : MaLookback(optInMaxPeriod, optInMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Mavp<T>(
        T[] inReal,
        T[] inPeriods,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInMinPeriod = 2,
        int optInMaxPeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        Mavp<T>(inReal, inPeriods, startIdx, endIdx, outReal, out _, out _, optInMinPeriod, optInMaxPeriod, optInMAType);
}
