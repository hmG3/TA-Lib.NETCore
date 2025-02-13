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
    /// Plus Directional Indicator (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Plus Directional Indicator is a momentum indicator that measures the strength of upward price movement over a given time period.
    /// It is part of the Directional Movement System.
    /// <para>
    /// The function can be integrated with <see cref="PlusDM{T}">+DM</see> and <see cref="Adx{T}">ADX</see> to provide
    /// a complete picture of trend strength. Confirming signals with additional indicators reduces the risk of misinterpretation.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the one-period positive directional movement (+DM1) as the difference between the current high and the previous high,
    ///       provided it exceeds the negative directional movement (-DM1).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the True Range (TR), which represents the total price movement for a period, accounting for gaps and volatility.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply Wilder's smoothing method to calculate the smoothed +DM and TR over the specified time period:
    /// <code>
    /// Today's +DM(n) = Previous +DM(n) - (Previous +DM(n) / TimePeriod) + Today's +DM1
    /// Today's TR(n)  = Previous TR(n)  - (Previous TR(n) / TimePeriod) + Today's TR
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the Plus Directional Indicator (+DI) as the ratio of smoothed +DM to smoothed TR, expressed as a percentage:
    ///       <code>
    ///         +DI = (+DM(n) / TR(n)) * 100
    ///       </code>
    ///       where <c>+DM(n)</c> and <c>TR(n)</c> are the smoothed values over the specified time period.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A rising value indicates strong upward momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A falling value suggests weakening upward momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Comparing <c>-DI</c> (from <see cref="MinusDI{T}">-DI</see>) and <c>-DI</c> can help identify trend direction:
    ///       if <c>+DI > -DI</c>, the trend is upward, and if <c>-DI > +DI</c>, the trend is downward.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode PlusDI<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        PlusDIImpl(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="PlusDI{T}">PlusDI</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int PlusDILookback(int optInTimePeriod = 14) => optInTimePeriod switch
    {
        < 1 => -1,
        > 1 => optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDI),
        _ => 1
    };

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode PlusDI<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        PlusDIImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode PlusDIImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* The DM1 (one period) is based on the largest part of today's range that is outside of yesterday's range.
         *
         * The following 7 cases explain how the +DM and -DM are calculated on one period:
         *
         * Case 1:                       Case 2:
         *    C│                        A│
         *     │                         │ C│
         *     │ +DM1 = (C-A)           B│  │ +DM1 = 0
         *     │ -DM1 = 0                   │ -DM1 = (B-D)
         * A│  │                           D│
         *  │ D│
         * B│
         *
         * Case 3:                       Case 4:
         *    C│                           C│
         *     │                        A│  │
         *     │ +DM1 = (C-A)            │  │ +DM1 = 0
         *     │ -DM1 = 0               B│  │ -DM1 = (B-D)
         * A│  │                            │
         *  │  │                           D│
         * B│  │
         *    D│
         *
         * Case 5:                      Case 6:
         * A│                           A│ C│
         *  │ C│ +DM1 = 0                │  │  +DM1 = 0
         *  │  │ -DM1 = 0                │  │  -DM1 = 0
         *  │ D│                         │  │
         * B│                           B│ D│
         *
         *
         * Case 7:
         *
         *    C│
         * A│  │
         *  │  │ +DM1=0
         * B│  │ -DM1=0
         *    D│
         *
         * In case 3 and 4, the rule is that the smallest delta between (C-A) and (B-D) determine
         * which of +DM or -DM is zero.
         *
         * In case 7, (C-A) and (B-D) are equal, so both +DM and -DM are zero.
         *
         * The rules remain the same when A=B and C=D (when the highs equal the lows).
         *
         * When calculating the DM over a period > 1, the one-period DM for the desired period are initially summed.
         * In other words, for a -DM14, sum the -DM1 for the first 14 days
         * (that's 13 values because there is no DM for the first day!)
         * Subsequent DM are calculated using Wilder's smoothing approach:
         *
         *                                     Previous +DM14
         *   Today's +DM14 = Previous +DM14 -  ────────────── + Today's +DM1
         *                                           14
         *
         * Calculation of a +DI14 is as follows:
         *
         *             +DM14
         *   +DI14 =  ────────
         *              TR14
         *
         * Calculation of the TR14 is:
         *
         *                                  Previous TR14
         *   Today's TR14 = Previous TR14 - ────────────── + Today's TR1
         *                                        14
         *
         *   The first TR14 is the summation of the first 14 TR1. See the TRange function on how to calculate the true range.
         *
         * Reference:
         *    New Concepts In Technical Trading Systems, J. Welles Wilder Jr
         */

        var lookbackTotal = PlusDILookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        if (optInTimePeriod == 1)
        {
            /* No smoothing needed. Just do the following for each price bar:
             *          +DM1
             *   +DI1 = ────
             *           TR1
             */
            return CalcPlusDIForPeriodOne(inHigh, inLow, inClose, startIdx, endIdx, outReal, out outRange);
        }

        var today = startIdx;
        var outBegIdx = today;
        today = startIdx - lookbackTotal;

        var timePeriod = T.CreateChecked(optInTimePeriod);
        T prevPlusDM = T.Zero, prevTR = T.Zero, _ = T.Zero;

        FunctionHelpers.InitDMAndTR(inHigh, inLow, inClose, out var prevHigh, ref today, out var prevLow, out var prevClose, timePeriod,
            ref prevPlusDM, ref _, ref prevTR);

        // Process subsequent DI

        // Skip the unstable period. Note that this loop must be executed at least ONCE to calculate the first DI.
        for (var i = 0; i < Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDI) + 1; i++)
        {
            today++;
            FunctionHelpers.UpdateDMAndTR(inHigh, inLow, inClose, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM,
                ref _, ref prevTR, timePeriod);
        }

        if (!T.IsZero(prevTR))
        {
            var (_, plusDI) = FunctionHelpers.CalcDI(_, prevPlusDM, prevTR);
            outReal[0] = plusDI;
        }
        else
        {
            outReal[0] = T.Zero;
        }

        var outIdx = 1;

        while (today < endIdx)
        {
            today++;
            FunctionHelpers.UpdateDMAndTR(inHigh, inLow, inClose, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM,
                ref _, ref prevTR, timePeriod);
            if (!T.IsZero(prevTR))
            {
                var (_, plusDI) = FunctionHelpers.CalcDI(_, prevPlusDM, prevTR);
                outReal[outIdx++] = plusDI;
            }
            else
            {
                outReal[outIdx++] = T.Zero;
            }
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static Core.RetCode CalcPlusDIForPeriodOne<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        var today = startIdx - 1;
        var prevHigh = high[today];
        var prevLow = low[today];
        var prevClose = close[today];
        var outIdx = 0;

        while (today < endIdx)
        {
            today++;
            var (diffP, diffM) = FunctionHelpers.CalcDeltas(high, low, today, ref prevHigh, ref prevLow);
            var tr = FunctionHelpers.TrueRange(prevHigh, prevLow, prevClose);

            outReal[outIdx++] = diffP > T.Zero && diffP > diffM && !T.IsZero(tr) ? diffP / tr : T.Zero;
            prevClose = close[today];
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
