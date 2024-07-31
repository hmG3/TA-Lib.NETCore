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
    public static Core.RetCode WillR<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        WillRImpl(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int WillRLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode WillR<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        WillRImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode WillRImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = WillRLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Proceed with the calculation for the requested range.
        // The algorithm allows the input and output to be the same buffer.
        int outIdx = default;
        var today = startIdx;
        var trailingIdx = startIdx - lookbackTotal;

        int highestIdx = -1, lowestIdx = -1;
        T highest = T.Zero, lowest = T.Zero;
        while (today <= endIdx)
        {
            (lowestIdx, lowest) = CalcLowest(inLow, trailingIdx, today, lowestIdx, lowest);
            (highestIdx, highest) = CalcHighest(inHigh, trailingIdx, today, highestIdx, highest);

            var diff = (highest - lowest) / (T.NegativeOne * Hundred<T>());

            outReal[outIdx++] = !T.IsZero(diff) ? (highest - inClose[today]) / diff : T.Zero;

            trailingIdx++;
            today++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
