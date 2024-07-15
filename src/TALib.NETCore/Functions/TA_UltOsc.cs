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
        int optInTimePeriod3 = 28) where T : IFloatingPointIeee754<T> =>
        UltOscImpl(inHigh, inLow, inClose, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod1,
            optInTimePeriod2, optInTimePeriod3);

    [PublicAPI]
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
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod1 = 7,
        int optInTimePeriod2 = 14,
        int optInTimePeriod3 = 28) where T : IFloatingPointIeee754<T> =>
        UltOscImpl<T>(inHigh, inLow, inClose, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod1,
            optInTimePeriod2, optInTimePeriod3);

    private static Core.RetCode UltOscImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod1,
        int optInTimePeriod2,
        int optInTimePeriod3) where T : IFloatingPointIeee754<T>
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
        // Sort to ensure that the time periods are ordered from shortest to longest.
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

        var totals1 = CalcPrimeTotals(inLow, inHigh, inClose, optInTimePeriod1);
        var totals2 = CalcPrimeTotals(inLow, inHigh, inClose, optInTimePeriod2);
        var totals3 = CalcPrimeTotals(inLow, inHigh, inClose, optInTimePeriod3);

        var TSeven = T.CreateChecked(7);

        // Calculate oscillator
        var today = startIdx;
        int outIdx = default;
        var trailingIdx1 = today - optInTimePeriod1 + 1;
        var trailingIdx2 = today - optInTimePeriod2 + 1;
        var trailingIdx3 = today - optInTimePeriod3 + 1;
        while (today <= endIdx)
        {
            // Add on today's terms
            var terms = CalcTerms(inLow, inHigh, inClose, today);
            totals1.aTotal += terms.closeMinusTrueLow;
            totals2.aTotal += terms.closeMinusTrueLow;
            totals3.aTotal += terms.closeMinusTrueLow;
            totals1.bTotal += terms.trueRange;
            totals2.bTotal += terms.trueRange;
            totals3.bTotal += terms.trueRange;

            // Calculate the oscillator value for today
            var output = T.Zero;

            if (!T.IsZero(totals1.bTotal))
            {
                output += Four<T>() * (totals1.aTotal / totals1.bTotal);
            }

            if (!T.IsZero(totals2.bTotal))
            {
                output += Two<T>() * (totals2.aTotal / totals2.bTotal);
            }

            if (!T.IsZero(totals3.bTotal))
            {
                output += totals3.aTotal / totals3.bTotal;
            }

            // Remove the trailing terms to prepare for next day
            terms = CalcTerms(inLow, inHigh, inClose, trailingIdx1);
            totals1.aTotal -= terms.closeMinusTrueLow;
            totals1.bTotal -= terms.trueRange;

            terms = CalcTerms(inLow, inHigh, inClose, trailingIdx2);
            totals2.aTotal -= terms.closeMinusTrueLow;
            totals2.bTotal -= terms.trueRange;

            terms = CalcTerms(inLow, inHigh, inClose, trailingIdx3);
            totals3.aTotal -= terms.closeMinusTrueLow;
            totals3.bTotal -= terms.trueRange;

            /* Last operation is to write the output.
             * Must be done after the trailing index have all been taken care of because
             * the caller is allowed to have the input array to be also the output array.
             */
            outReal[outIdx++] = Hundred<T>() * (output / TSeven);
            today++;
            trailingIdx1++;
            trailingIdx2++;
            trailingIdx3++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;

        (T trueRange, T closeMinusTrueLow) CalcTerms(
            ReadOnlySpan<T> low,
            ReadOnlySpan<T> high,
            ReadOnlySpan<T> close,
            int day)
        {
            var tempLT = low[day];
            var tempHT = high[day];
            var tempCY = close[day - 1];
            var trueLow = T.Min(tempLT, tempCY);
            var closeMinusTrueLow = close[day] - trueLow;
            var trueRange = TrueRange(tempHT, tempLT, tempCY);

            return (trueRange, closeMinusTrueLow);
        }

        (T aTotal, T bTotal) CalcPrimeTotals(
            ReadOnlySpan<T> low,
            ReadOnlySpan<T> high,
            ReadOnlySpan<T> close,
            int period)
        {
            T aTotal = T.Zero, bTotal = T.Zero;
            for (var i = startIdx - period + 1; i < startIdx; ++i)
            {
                var terms = CalcTerms(low, high, close, i);
                aTotal += terms.closeMinusTrueLow;
                bTotal += terms.trueRange;
            }

            return (aTotal, bTotal);
        }
    }
}
