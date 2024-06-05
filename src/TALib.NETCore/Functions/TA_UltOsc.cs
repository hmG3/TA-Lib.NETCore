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
    public static Core.RetCode UltOsc<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod1 = 7,
        int optInTimePeriod2 = 14,
        int optInTimePeriod3 = 28) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod1 < 1 || optInTimePeriod2 < 1 || optInTimePeriod3 < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = UltOscLookback(optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        Span<bool> usedFlag = new bool[3];
        Span<int> periods = new[] { optInTimePeriod1, optInTimePeriod2, optInTimePeriod3 };
        Span<int> sortedPeriods = new int[3];

        for (var i = 0; i < 3; ++i)
        {
            int longestPeriod = default;
            int longestIndex = default;
            for (var j = 0; j < 3; j++)
            {
                if (!usedFlag[j] && periods[j] > longestPeriod)
                {
                    longestPeriod = periods[j];
                    longestIndex = j;
                }
            }

            usedFlag[longestIndex] = true;
            sortedPeriods[i] = longestPeriod;
        }

        optInTimePeriod1 = sortedPeriods[2];
        optInTimePeriod2 = sortedPeriods[1];
        optInTimePeriod3 = sortedPeriods[0];

        T trueRange;
        T closeMinusTrueLow;

        T a1Total = T.Zero;
        T b1Total = T.Zero;
        for (var i = startIdx - optInTimePeriod1 + 1; i < startIdx; ++i)
        {
            CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
            a1Total += closeMinusTrueLow;
            b1Total += trueRange;
        }

        T a2Total = T.Zero;
        T b2Total = T.Zero;
        for (var i = startIdx - optInTimePeriod2 + 1; i < startIdx; ++i)
        {
            CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
            a2Total += closeMinusTrueLow;
            b2Total += trueRange;
        }

        T a3Total = T.Zero;
        T b3Total = T.Zero;
        for (var i = startIdx - optInTimePeriod3 + 1; i < startIdx; ++i)
        {
            CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
            a3Total += closeMinusTrueLow;
            b3Total += trueRange;
        }

        T TSeven = T.CreateChecked(7);
        var today = startIdx;
        int outIdx = default;
        var trailingIdx1 = today - optInTimePeriod1 + 1;
        var trailingIdx2 = today - optInTimePeriod2 + 1;
        var trailingIdx3 = today - optInTimePeriod3 + 1;
        while (today <= endIdx)
        {
            CalcTerms(inLow, inHigh, inClose, today, out trueRange, out closeMinusTrueLow);
            a1Total += closeMinusTrueLow;
            a2Total += closeMinusTrueLow;
            a3Total += closeMinusTrueLow;
            b1Total += trueRange;
            b2Total += trueRange;
            b3Total += trueRange;

            T output = T.Zero;

            if (!T.IsZero(b1Total))
            {
                output += Four<T>() * (a1Total / b1Total);
            }

            if (!T.IsZero(b2Total))
            {
                output += Two<T>() * (a2Total / b2Total);
            }

            if (!T.IsZero(b3Total))
            {
                output += a3Total / b3Total;
            }

            CalcTerms(inLow, inHigh, inClose, trailingIdx1, out trueRange, out closeMinusTrueLow);
            a1Total -= closeMinusTrueLow;
            b1Total -= trueRange;

            CalcTerms(inLow, inHigh, inClose, trailingIdx2, out trueRange, out closeMinusTrueLow);
            a2Total -= closeMinusTrueLow;
            b2Total -= trueRange;

            CalcTerms(inLow, inHigh, inClose, trailingIdx3, out trueRange, out closeMinusTrueLow);
            a3Total -= closeMinusTrueLow;
            b3Total -= trueRange;

            outReal[outIdx++] = Hundred<T>() * (output / TSeven);
            today++;
            trailingIdx1++;
            trailingIdx2++;
            trailingIdx3++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int UltOscLookback(int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28) =>
        optInTimePeriod1 < 1 || optInTimePeriod2 < 1 || optInTimePeriod3 < 1
            ? -1
            : SmaLookback(Math.Max(Math.Max(optInTimePeriod1, optInTimePeriod2), optInTimePeriod3)) + 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode UltOsc<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod1 = 7,
        int optInTimePeriod2 = 14,
        int optInTimePeriod3 = 28) where T : IFloatingPointIeee754<T> =>
        UltOsc<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out _, out _, optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);
}
