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
    public static Core.RetCode Mavp<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inPeriods,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInMinPeriod = 2,
        int optInMaxPeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MavpImpl(inReal, inPeriods, inRange, outReal, out outRange, optInMinPeriod, optInMaxPeriod, optInMAType);

    [PublicAPI]
    public static int MavpLookback(int optInMaxPeriod = 30, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInMaxPeriod < 2 ? -1 : MaLookback(optInMaxPeriod, optInMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Mavp<T>(
        T[] inReal,
        T[] inPeriods,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInMinPeriod = 2,
        int optInMaxPeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MavpImpl<T>(inReal, inPeriods, inRange, outReal, out outRange, optInMinPeriod, optInMaxPeriod, optInMAType);

    private static Core.RetCode MavpImpl<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inPeriods,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInMinPeriod,
        int optInMaxPeriod,
        Core.MAType optInMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        var startIdx = inRange.Start.Value;
        var endIdx = inRange.End.Value;

        if (endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInMinPeriod < 2 || optInMaxPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MavpLookback(optInMaxPeriod, optInMAType);
        if (inPeriods.Length < lookbackTotal)
        {
            return Core.RetCode.BadParam;
        }

        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInt = lookbackTotal > startIdx ? lookbackTotal : startIdx;
        if (tempInt > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outputSize = endIdx - tempInt + 1;

        // Allocate intermediate local buffer.
        Span<T> localOutputArray = new T[outputSize];
        Span<int> localPeriodArray = new int[outputSize];

        // Copy caller array of period into local buffer. At the same time, truncate to min/max.
        for (var i = 0; i < outputSize; i++)
        {
            var period = Int32.CreateTruncating(inPeriods[startIdx + i]);
            localPeriodArray[i] = Math.Clamp(period, optInMinPeriod, optInMaxPeriod);
        }

        var intermediateOutput = outReal == inReal ? new T[outputSize] : outReal;

        /* Process each element of the input.
         * For each possible period value, the MA is calculated only once.
         * The outReal is then fill up for all element with the same period.
         * A local flag (value 0) is set in localPeriodArray to avoid doing a second time the same calculation.
         */
        for (var i = 0; i < outputSize; i++)
        {
            var curPeriod = localPeriodArray[i];
            if (curPeriod == 0)
            {
                continue;
            }

            // Calculation of the MA required.
            var retCode = MaImpl(inReal, new Range(startIdx, endIdx), localOutputArray, out _, curPeriod, optInMAType);
            if (retCode != Core.RetCode.Success)
            {
                return retCode;
            }

            intermediateOutput[i] = localOutputArray[i];
            for (var j = i + 1; j < outputSize; j++)
            {
                if (localPeriodArray[j] == curPeriod)
                {
                    localPeriodArray[j] = 0; // Flag to avoid recalculation
                    intermediateOutput[j] = localOutputArray[j];
                }
            }
        }

        // Copy intermediate buffer to output buffer if necessary.
        if (intermediateOutput != outReal)
        {
            intermediateOutput[..outputSize].CopyTo(outReal);
        }

        outRange = new Range(startIdx, startIdx + outputSize);

        return Core.RetCode.Success;
    }
}
