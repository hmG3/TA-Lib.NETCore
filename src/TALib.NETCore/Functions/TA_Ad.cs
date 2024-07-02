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
    public static Core.RetCode Ad<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length || endIdx >= inVolume.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        /* Note:
         * Results from this function might vary slightly when using float instead of double and
         * this cause a different floating-point precision to be used.
         * For most function, this is not an apparent difference but for function using large cumulative values
         * (like this AD function), minor imprecision adds up and becomes significant.
         * For better precision, use double in calculations.
         */

        var nbBar = endIdx - startIdx + 1;
        outBegIdx = startIdx;
        outNbElement = nbBar;
        var currentBar = startIdx;
        int outIdx = default;
        var ad = T.Zero;

        while (nbBar != 0)
        {
            ad = CalcAccumulationDistribution(inHigh, inLow, inClose, inVolume, ref currentBar, ad);
            outReal[outIdx++] = ad;
            nbBar--;
        }

        return Core.RetCode.Success;
    }

    public static int AdLookback() => default;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Ad<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        T[] inVolume,
        int startIdx,
        int endIdx,
        T[] outReal) where T : IFloatingPointIeee754<T> =>
        Ad<T>(inHigh, inLow, inClose, inVolume, startIdx, endIdx, outReal, out _, out _);
}
