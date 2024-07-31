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
    public static Core.RetCode Correl<T>(
        ReadOnlySpan<T> inReal0,
        ReadOnlySpan<T> inReal1,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        CorrelImpl(inReal0, inReal1, inRange, outReal, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int CorrelLookback(int optInTimePeriod = 30) => optInTimePeriod < 1 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Correl<T>(
        T[] inReal0,
        T[] inReal1,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        CorrelImpl<T>(inReal0, inReal1, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode CorrelImpl<T>(
        ReadOnlySpan<T> inReal0,
        ReadOnlySpan<T> inReal1,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inReal0.Length, inReal1.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = CorrelLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;
        var trailingIdx = startIdx - lookbackTotal;

        // Calculate the initial values.
        T sumX, sumY, sumX2, sumY2;
        var sumXY = sumX = sumY = sumX2 = sumY2 = T.Zero;
        int today;
        for (today = trailingIdx; today <= startIdx; today++)
        {
            var x = inReal0[today];
            sumX += x;
            sumX2 += x * x;

            var y = inReal1[today];
            sumXY += x * y;
            sumY += y;
            sumY2 += y * y;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        // Write the first output.
        // Save first the trailing values since the input and output might be the same array.
        var trailingX = inReal0[trailingIdx];
        var trailingY = inReal1[trailingIdx++];
        var tempReal = (sumX2 - sumX * sumX / timePeriod) * (sumY2 - sumY * sumY / timePeriod);
        outReal[0] = tempReal > T.Zero ? (sumXY - sumX * sumY / timePeriod) / T.Sqrt(tempReal) : T.Zero;

        // Tight loop to do subsequent values.
        var outIdx = 1;
        while (today <= endIdx)
        {
            // Remove trailing values
            sumX -= trailingX;
            sumX2 -= trailingX * trailingX;

            sumXY -= trailingX * trailingY;
            sumY -= trailingY;
            sumY2 -= trailingY * trailingY;

            // Add new values
            var x = inReal0[today];
            sumX += x;
            sumX2 += x * x;

            var y = inReal1[today++];
            sumXY += x * y;
            sumY += y;
            sumY2 += y * y;

            // Output new coefficient.
            // Save first the trailing values since the input and output might be the same array.
            trailingX = inReal0[trailingIdx];
            trailingY = inReal1[trailingIdx++];
            tempReal = (sumX2 - sumX * sumX / timePeriod) * (sumY2 - sumY * sumY / timePeriod);
            outReal[outIdx++] = tempReal > T.Zero ? (sumXY - sumX * sumY / timePeriod) / T.Sqrt(tempReal) : T.Zero;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
