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

public static partial class Candles
{
    public static Core.RetCode Hikkake<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<int> outInteger,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = HikkakeLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int patternIdx = default;
        int patternResult = default;
        var i = startIdx - 3;
        while (i < startIdx)
        {
            if (inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 1st + 2nd: lower high and higher low
                (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] // (bull) 3rd: lower high and lower low
                 ||
                 inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1])) // (bear) 3rd: higher high and higher low
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
            }
            /* search for confirmation if hikkake was no more than 3 bars ago */
            else if (i <= patternIdx + 3 &&
                     (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] // close higher than the high of 2nd
                      ||
                      patternResult < 0 && inClose[i] < inLow[patternIdx - 1])) // close lower than the low of 2nd
            {
                patternIdx = 0;
            }

            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 1st + 2nd: lower high and higher low
                (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] // (bull) 3rd: lower high and lower low
                 ||
                 inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1] // (bear) 3rd: higher high and higher low
                )
               )
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
                outInteger[outIdx++] = patternResult;
            }
            /* search for confirmation if hikkake was no more than 3 bars ago */
            else if (i <= patternIdx + 3 &&
                     (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] // close higher than the high of 2nd
                      ||
                      patternResult < 0 && inClose[i] < inLow[patternIdx - 1])) // close lower than the low of 2nd
            {
                outInteger[outIdx++] = patternResult + 100 * (patternResult > 0 ? 1 : -1);
                patternIdx = 0;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int HikkakeLookback() => 5;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Hikkake<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        int[] outInteger) where T : IFloatingPointIeee754<T> =>
        Hikkake<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outInteger, out _, out _);
}
