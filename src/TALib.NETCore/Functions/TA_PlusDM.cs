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
    /// Plus Directional Movement (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
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
    /// Plus Directional Movement function calculates the positive directional movement over a specified time period.
    /// It is a component of the directional movement system and is used to assess the strength of upward price movement.
    /// <para>
    /// Used in conjunction with <see cref="MinusDM{T}">-DM</see> to calculate the Directional Indicators
    /// (<see cref="PlusDI{T}">+DI</see>, <see cref="MinusDI{T}">-DI</see>) or the Average Directional Index (<see cref="Adx{T}">ADX</see>).
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the positive directional movement for each period as <c>+DM = Current High - Previous High</c> if this value is
    ///       greater than the corresponding positive movement (<c>Previous Low - Current Low</c>) and is positive;
    ///       otherwise, <c>+DM = 0</c>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       If the time period is greater than 1, sum the <c>+DM</c> values for the specified period,
    ///       and smooth the results using Wilder's smoothing method:
    ///       <code>
    ///         +DM(n) = (Previous +DM(n-1) * (Period - 1) + Current +DM) / Period
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A high output value indicates strong upward momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///        A low or zero output value suggests weak or no upward momentum.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode PlusDM<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        PlusDMImpl(inHigh, inLow, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="PlusDM{T}">PlusDM</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int PlusDMLookback(int optInTimePeriod = 14) => optInTimePeriod switch
    {
        < 1 => -1,
        > 1 => optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDM) - 1,
        _ => 1
    };

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode PlusDM<T>(
        T[] inHigh,
        T[] inLow,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        PlusDMImpl<T>(inHigh, inLow, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode PlusDMImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length) is not { } rangeIndices)
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

        var lookbackTotal = PlusDMLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Trap the case where no smoothing is needed.
        if (optInTimePeriod == 1)
        {
            // No smoothing needed. Just do a simple DM1 for each price bar.
            return CalcPlusDMForPeriodOne(inHigh, inLow, startIdx, endIdx, outReal, out outRange);
        }

        var outBegIdx = startIdx;
        var today = startIdx - lookbackTotal;

        var timePeriod = T.CreateChecked(optInTimePeriod);
        T prevPlusDM = T.Zero, _ = T.Zero;

        FunctionHelpers.InitDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, out var prevHigh, ref today, out var prevLow, out var _,
            timePeriod, ref prevPlusDM, ref _, ref _);

        // Process subsequent DM

        // Skip the unstable period.
        for (var i = 0; i < Core.UnstablePeriodSettings.Get(Core.UnstableFunc.PlusDM); i++)
        {
            today++;
            FunctionHelpers.UpdateDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, ref today, ref prevHigh, ref prevLow, ref _, ref prevPlusDM,
                ref _, ref _, timePeriod);
        }

        outReal[0] = prevPlusDM;
        var outIdx = 1;

        while (today < endIdx)
        {
            today++;
            FunctionHelpers.UpdateDMAndTR(inHigh, inLow, ReadOnlySpan<T>.Empty, ref today, ref prevHigh, ref prevLow, ref _, ref prevPlusDM,
                ref _, ref _, timePeriod);
            outReal[outIdx++] = prevPlusDM;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static Core.RetCode CalcPlusDMForPeriodOne<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        var today = startIdx - 1;
        var prevHigh = high[today];
        var prevLow = low[today];
        var outIdx = 0;

        while (today < endIdx)
        {
            today++;
            var (diffP, diffM) = FunctionHelpers.CalcDeltas(high, low, today, ref prevHigh, ref prevLow);
            outReal[outIdx++] = diffP > T.Zero && diffP > diffM ? diffP : T.Zero;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
