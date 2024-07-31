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
    public static Core.RetCode Adxr<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AdxrImpl(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int AdxrLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + AdxLookback(optInTimePeriod) - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Adxr<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AdxrImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode AdxrImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = AdxrLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        Span<T> adx = new T[endIdx - startIdx + optInTimePeriod];

        var retCode = AdxImpl(inHigh, inLow, inClose, new Range(startIdx - (optInTimePeriod - 1), endIdx), adx, out outRange,
            optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var i = optInTimePeriod - 1;
        int j = default;
        int outIdx = default;
        var nbElement = endIdx - startIdx + 2;
        while (--nbElement != 0)
        {
            outReal[outIdx++] = (adx[i++] + adx[j++]) / Two<T>();
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
