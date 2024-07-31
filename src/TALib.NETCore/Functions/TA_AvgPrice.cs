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
    public static Core.RetCode AvgPrice<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        AvgPriceImpl(inOpen, inHigh, inLow, inClose, inRange, outReal, out outRange);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [PublicAPI]
    public static int AvgPriceLookback() => 0;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AvgPrice<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        AvgPriceImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outReal, out outRange);

    private static Core.RetCode AvgPriceImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        int outIdx = default;
        for (var i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = (inHigh[i] + inLow[i] + inClose[i] + inOpen[i]) / Four<T>();
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
