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
    public static Core.RetCode ThreeOutside<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<Core.CandlePatternType> outType,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = ThreeOutsideLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         *   - first: black (white) real body
         *   - second: white (black) real body that engulfs the prior real body
         *   - third: candle that closes higher (lower) than the second candle
         * outType is Bullish for the three outside up or Bearish for the three outside down;
         * the user should consider that a three outside up must appear in a downtrend and three outside down must appear in an uptrend,
         * while this function does not consider it
         */

        int outIdx = default;
        do
        {
            outType[outIdx++] = IsThreeOutsidePattern(inOpen, inClose, i)
                ? (Core.CandlePatternType) ((int) CandleColor(inClose, inOpen, i - 1) * 100)
                : Core.CandlePatternType.None;

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int ThreeOutsideLookback() => 3;

    private static bool IsThreeOutsidePattern<T>(ReadOnlySpan<T> inOpen, ReadOnlySpan<T> inClose, int i)
        where T : IFloatingPointIeee754<T> =>
        // white engulfs black
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
        inClose[i - 1] > inOpen[i - 2] && inOpen[i - 1] < inClose[i - 2] &&
        // third candle higher
        inClose[i] > inClose[i - 1]
        ||
        // black engulfs white
        CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
        CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
        inOpen[i - 1] > inClose[i - 2] && inClose[i - 1] < inOpen[i - 2] &&
        // third candle lower
        inClose[i] < inClose[i - 1];

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode ThreeOutside<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        Core.CandlePatternType[] outType) where T : IFloatingPointIeee754<T> =>
        ThreeOutside<T>(inOpen, inHigh, inLow, inClose, startIdx, endIdx, outType, out _, out _);
}
