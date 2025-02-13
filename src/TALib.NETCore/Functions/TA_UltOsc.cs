/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2025 Anatolii Siryi
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
    /// <summary>
    /// Ultimate Oscillator (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod1">The short-term time period.</param>
    /// <param name="optInTimePeriod2">The medium-term time period.</param>
    /// <param name="optInTimePeriod3">The long-term time period.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Ultimate Oscillator is a momentum oscillator designed to capture the relationship between
    /// buying and selling pressure over multiple time frames. It helps identify potential price reversals
    /// and overbought/oversold conditions by combining short-term, medium-term, and long-term averages
    /// of true ranges and buying pressure.
    /// <para>
    /// The Ultimate Oscillator combines multiple time frames to minimize false signals caused by short-term
    /// market fluctuations while still reacting quickly enough to significant price movements.
    /// The function provides a more balanced momentum view.
    /// Integrating it with trend confirmation or volume indicators can reinforce signal credibility.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       For each period, calculate the true range (TR) and the buying pressure (BP):
    /// <code>
    /// True Range (TR) = Maximum(High - Low, Absolute(High - Previous Close), Absolute(Low - Previous Close)).
    /// Buying Pressure (BP) = Close - Minimum(Low, Previous Close).
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the averages of BP and TR for the short-term, medium-term, and long-term periods.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the Ultimate Oscillator value as a weighted average of the three time periods:
    ///       <code>
    ///         UO = 100 * [(4 * (Short BP/TR)) + (2 * (Medium BP/TR)) + (1 * (Long BP/TR))] / (4 + 2 + 1).
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Values above 70 suggest overbought conditions, indicating potential selling opportunities.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Values below 30 suggest oversold conditions, indicating potential buying opportunities.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The oscillator can confirm trends when used with price action or other indicators.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode UltOsc<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod1 = 7,
        int optInTimePeriod2 = 14,
        int optInTimePeriod3 = 28) where T : IFloatingPointIeee754<T> =>
        UltOscImpl(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);

    /// <summary>
    /// Returns the lookback period for <see cref="UltOsc{T}">UltOsc</see>.
    /// </summary>
    /// <param name="optInTimePeriod1">The short-term time period.</param>
    /// <param name="optInTimePeriod2">The medium-term time period.</param>
    /// <param name="optInTimePeriod3">The long-term time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
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
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod1 = 7,
        int optInTimePeriod2 = 14,
        int optInTimePeriod3 = 28) where T : IFloatingPointIeee754<T> =>
        UltOscImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);

    private static Core.RetCode UltOscImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod1,
        int optInTimePeriod2,
        int optInTimePeriod3) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod1 < 1 || optInTimePeriod2 < 1 || optInTimePeriod3 < 1)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = UltOscLookback(optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        SortTimePeriods(ref optInTimePeriod1, ref optInTimePeriod2, ref optInTimePeriod3);

        var totals1 = CalcPrimeTotals(inLow, inHigh, inClose, optInTimePeriod1, startIdx);
        var totals2 = CalcPrimeTotals(inLow, inHigh, inClose, optInTimePeriod2, startIdx);
        var totals3 = CalcPrimeTotals(inLow, inHigh, inClose, optInTimePeriod3, startIdx);

        var TSeven = T.CreateChecked(7);

        // Calculate oscillator
        var today = startIdx;
        var outIdx = 0;
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
                output += FunctionHelpers.Four<T>() * (totals1.aTotal / totals1.bTotal);
            }

            if (!T.IsZero(totals2.bTotal))
            {
                output += FunctionHelpers.Two<T>() * (totals2.aTotal / totals2.bTotal);
            }

            if (!T.IsZero(totals3.bTotal))
            {
                output += totals3.aTotal / totals3.bTotal;
            }

            // Remove the trailing terms to prepare for next day
            UpdateTrailingTotals(inLow, inHigh, inClose, trailingIdx1, ref totals1);
            UpdateTrailingTotals(inLow, inHigh, inClose, trailingIdx2, ref totals2);
            UpdateTrailingTotals(inLow, inHigh, inClose, trailingIdx3, ref totals3);

            /* Last operation is to write the output.
             * Must be done after the trailing index have all been taken care of because
             * the caller is allowed to have the input array to be also the output array.
             */
            outReal[outIdx++] = FunctionHelpers.Hundred<T>() * (output / TSeven);
            today++;
            trailingIdx1++;
            trailingIdx2++;
            trailingIdx3++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static void SortTimePeriods(ref int optInTimePeriod1, ref int optInTimePeriod2, ref int optInTimePeriod3)
    {
        Span<bool> usedFlag = stackalloc bool[3];
        Span<int> periods = [optInTimePeriod1, optInTimePeriod2, optInTimePeriod3];
        Span<int> sortedPeriods = stackalloc int[3];

        for (var i = 0; i < 3; ++i)
        {
            var longestPeriod = 0;
            var longestIndex = 0;
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
    }

    private static void UpdateTrailingTotals<T>(
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> close,
        int trailingIdx,
        ref (T aTotal, T bTotal) totals) where T : IFloatingPointIeee754<T>
    {
        var terms = CalcTerms(low, high, close, trailingIdx);
        totals.aTotal -= terms.closeMinusTrueLow;
        totals.bTotal -= terms.trueRange;
    }

    private static (T trueRange, T closeMinusTrueLow) CalcTerms<T>(
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> close,
        int day) where T : IFloatingPointIeee754<T>
    {
        var tempLT = low[day];
        var tempHT = high[day];
        var tempCY = close[day - 1];
        var trueLow = T.Min(tempLT, tempCY);
        var closeMinusTrueLow = close[day] - trueLow;
        var trueRange = FunctionHelpers.TrueRange(tempHT, tempLT, tempCY);

        return (trueRange, closeMinusTrueLow);
    }

    private static (T aTotal, T bTotal) CalcPrimeTotals<T>(
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> close,
        int period,
        int startIdx) where T : IFloatingPointIeee754<T>
    {
        T aTotal = T.Zero, bTotal = T.Zero;
        for (var i = startIdx - period + 1; i < startIdx; ++i)
        {
            var tempLT = low[i];
            var tempHT = high[i];
            var tempCY = close[i - 1];
            var trueLow = T.Min(tempLT, tempCY);
            var closeMinusTrueLow = close[i] - trueLow;
            var trueRange = FunctionHelpers.TrueRange(tempHT, tempLT, tempCY);
            var terms = (trueRange, closeMinusTrueLow);
            aTotal += terms.closeMinusTrueLow;
            bTotal += terms.trueRange;
        }

        return (aTotal, bTotal);
    }
}
