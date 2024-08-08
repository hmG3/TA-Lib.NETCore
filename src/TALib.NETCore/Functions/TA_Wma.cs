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
    public static Core.RetCode Wma<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        WmaImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int WmaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Wma<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        WmaImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode WmaImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /* The algo uses a very basic property of multiplication/addition: (x * 2) = x + x
         *
         * As an example, a 3 period weighted can be interpreted in two ways:
         *   (x1 * 1) + (x2 * 2) + (x3 * 3)
         *     OR
         *   x1 + x2 + x2 + x3 + x3 + x3 (this is the periodSum)
         *
         * When moving forward in the time series the periodSum can be quickly adjusted for the period by subtracting:
         *   x1 + x2 + x3 (This is the periodSub)
         * Making the new periodSum equals to:
         *   x2 + x3 + x3
         *
         * Then the new price bar can be added which is x4 + x4 + x4 giving:
         *   x2 + x3 + x3 + x4 + x4 + x4
         *
         * At this point, one iteration is completed, and it can be seen that step 1 of this example is reached again.
         *
         * The number of memory access and floating point operations are kept to a minimum with this algo.
         */

        var lookbackTotal = WmaLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        var trailingIdx = startIdx - lookbackTotal;

        var periodSub = T.Zero;
        var periodSum = periodSub;
        var inIdx = trailingIdx;
        var i = 1;
        while (inIdx < startIdx)
        {
            var tempReal = inReal[inIdx++];
            periodSub += tempReal;
            periodSum += tempReal * T.CreateChecked(i);
            i++;
        }

        var trailingValue = T.Zero;

        // Calculate the divider (always an integer value).
        // By induction: 1 + 2 + 3 + 4 + ... + n = n * (n + 1) / 2
        var divider = T.CreateChecked((optInTimePeriod * (optInTimePeriod + 1)) >> 1);
        var timePeriod = T.CreateChecked(optInTimePeriod);

        // Tight loop for the requested range.
        while (inIdx <= endIdx)
        {
            // Add the current price bar to the sum carried through the iterations.
            var tempReal = inReal[inIdx++];
            periodSub += tempReal;
            periodSub -= trailingValue;
            periodSum += tempReal * timePeriod;

            // Save the trailing value for being subtracted at the next iteration.
            // Do this because input and output can point to the same buffer.
            trailingValue = inReal[trailingIdx++];

            // Calculate the WMA for this price bar.
            outReal[outIdx++] = periodSum / divider;

            periodSum -= periodSub;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
