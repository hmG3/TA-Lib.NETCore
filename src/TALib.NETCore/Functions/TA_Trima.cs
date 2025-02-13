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
    /// Triangular Moving Average (Overlap Studies)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
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
    /// Triangular Moving Average is a weighted moving average designed to put greater weight on
    /// data points near the center of the specified period. TRIMA applies symmetrical weighting,
    /// emphasizing the middle portion of the data set to provide a smooth average.
    ///<para>
    /// The function can yield a smoother trend measure than <see cref="Sma{T}">SMA</see> or <see cref="Ema{T}">EMA</see>.
    /// Integrating it with momentum indicators may offer clearer signals by reducing noise.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       For each period, compute the weighted moving average as follows:
    ///       - For an odd period, the TRIMA is equivalent to a Simple Moving Average (SMA) of another SMA.
    ///       - For an even period, the TRIMA uses adjusted weights to smooth the data.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Update the numerator dynamically for each subsequent calculation by subtracting trailing values,
    ///       adding new values, and adjusting weights accordingly.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Normalize the weighted sum by dividing it by the total weight factor for the period.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Trima<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        TrimaImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Trima{T}">Trima</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int TrimaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Trima<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        TrimaImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode TrimaImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /* The triangular MA is a weighted moving average. Instead of the WMA who put more weight on the latest price bar,
         * the triangular put more weight on the data in the middle of the specified period.
         *
         * Examples:
         *   For TimeSeries={a, b, c, d, e, f...} ("a" is the older price)
         *
         *   1st value for TRIMA 4-Period is: ((1 * a) + (2 * b) + (2 * c) + (1 * d)) / 6
         *   2nd value for TRIMA 4-Period is: ((1 * b) + (2 * c) + (2 * d) + (1 * e)) / 6
         *
         *   1st value for TRIMA 5-Period is: ((1 * a) + (2 * b) + (3 * c) + (2 * d) + (1 * e)) / 9
         *   2nd value for TRIMA 5-Period is: ((1 * b) + (2 * c) + (3 * d) + (2 * e) + (1 * f)) / 9
         *
         * Generally accepted implementation
         * ─────────────────────────────────
         * Using algebra, it can be demonstrated that the TRIMA is equivalent to doing a SMA of a SMA.
         * The following explain the rules:
         *
         *   (1) When the period is even, TRIMA(x, period) = SMA(SMA(x, period / 2), (period / 2) + 1)
         *   (2) When the period is odd,  TRIMA(x, period) = SMA(SMA(x, (period + 1) / 2), (period + 1) / 2)
         *
         * In other word:
         *   (1) A period of 4 becomes TRIMA(x, 4) = SMA(SMA(x, 2), 3)
         *   (2) A period of 5 becomes TRIMA(x, 5) = SMA(SMA(x, 3), 3)
         *
         * The SMA of a SMA is the algorithm generally found in books.
         *
         * Library's Implementation
         * ──────────────
         * Output is also the same as the generally accepted implementation.
         *
         * For speed optimization and avoid memory allocation, the library uses a better algorithm than the usual SMA of a SMA.
         *
         * The calculation from one TRIMA value to the next is done by doing 4 little adjustment (the following show a TRIMA 4-period):
         *
         * TRIMA at time "d": ((1 * a) + (2 * b) + (2 * c) + (1 * d)) / 6
         * TRIMA at time "e": ((1 * b) + (2 * c) + (2 * d) + (1 * e)) / 6
         *
         * To go from TRIMA "d" to "d", the following is done:
         *   1) "a" and "b" are subtracted from the numerator.
         *   2) "d" is added to the numerator.
         *   3) "e" is added to the numerator.
         *   4) Calculate TRIMA by doing numerator / 6
         *   5) Repeat sequence for next output
         *
         * These operations are the same steps done by the library:
         *   1) is done by numeratorSub
         *   2) is done by numeratorAdd.
         *   3) is obtained from the latest input
         *   4) Calculate and write TRIMA in the output
         *   5) Repeat for next output.
         *
         * numeratorAdd and numeratorSub needs to be adjusted for each iteration.
         *
         * The update of numeratorSub needs values from the input at the trailingIdx and middleIdx position.
         *
         * The update of numeratorAdd needs values from the input at the middleIdx and todayIdx.
         */

        var lookbackTotal = TrimaLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx;
        if (optInTimePeriod % 2 != 0)
        {
            ProcessOdd(inReal, startIdx, endIdx, optInTimePeriod, lookbackTotal, outReal, out outIdx);
        }
        else
        {
            ProcessEven(inReal, startIdx, endIdx, optInTimePeriod, lookbackTotal, outReal, out outIdx);
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static void ProcessOdd<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        int optInTimePeriod,
        int lookbackTotal,
        Span<T> outReal,
        out int outIdx) where T : IFloatingPointIeee754<T>
    {
        /* Calculate the factor which is 1 divided by the summation of the weight.
         *
         * The sum of the weight is calculated as follows:
         *
         * The simple summation series 1 + 2 + 3 + ... + n can be express as n * (n + 1) / 2
         *
         * From this logic, a "triangular" summation formula can be found depending on if the period is odd or even.
         *
         * Odd period formula:
         *   period = 5 and with n = (int)(period / 2)
         *   the formula for a "triangular" series is:
         *     1 + 2 + 3 + 2 + 1 = (n * (n + 1)) + n + 1
         *                       = (n + 1) * (n + 1)
         *                       = 3 * 3 = 9
         *
         * Even period formula:
         *   period = 6 and with n=(int)(period/2)
         *   the formula for a "triangular" series is:
         *     1 + 2 + 3 + 3 + 2 + 1 = n * (n + 1)
         *                           = 3 * 4 = 12
         */

        // Entirely done with int and becomes double only on assignment to the factor variable.
        var i = optInTimePeriod >> 1;
        var ti = T.CreateChecked(i);
        var factor = (ti + T.One) * (ti + T.One);
        factor = T.One / factor;

        var trailingIdx = startIdx - lookbackTotal;
        var middleIdx = trailingIdx + i;
        var todayIdx = middleIdx + i;
        T numerator = T.Zero, numeratorSub = T.Zero;
        T tempReal;
        for (i = middleIdx; i >= trailingIdx; i--)
        {
            tempReal = inReal[i];
            numeratorSub += tempReal;
            numerator += numeratorSub;
        }

        var numeratorAdd = T.Zero;
        middleIdx++;
        for (i = middleIdx; i <= todayIdx; i++)
        {
            tempReal = inReal[i];
            numeratorAdd += tempReal;
            numerator += numeratorAdd;
        }

        // The value at the trailingIdx was saved in tempReal to account for the case when
        // output and input can point to the same buffer.
        outIdx = 0;
        tempReal = inReal[trailingIdx++];
        outReal[outIdx++] = numerator * factor;
        todayIdx++;

        while (todayIdx <= endIdx)
        {
            numerator -= numeratorSub;
            numeratorSub -= tempReal;
            tempReal = inReal[middleIdx++];
            numeratorSub += tempReal;

            numerator += numeratorAdd;
            numeratorAdd -= tempReal;
            tempReal = inReal[todayIdx++];
            numeratorAdd += tempReal;

            numerator += tempReal;

            tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = numerator * factor;
        }
    }

    private static void ProcessEven<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        int optInTimePeriod,
        int lookbackTotal,
        Span<T> outReal,
        out int outIdx) where T : IFloatingPointIeee754<T>
    {
        /* Very similar to the odd logic, except:
         *   - calculation of the factor is different.
         *   - the coverage of the numeratorSub and numeratorAdd is slightly different.
         *   - Adjustment of numeratorAdd is different.
         */
        var i = optInTimePeriod >> 1;
        var ti = T.CreateChecked(i);
        var factor = ti * (ti + T.One);
        factor = T.One / factor;

        var trailingIdx = startIdx - lookbackTotal;
        var middleIdx = trailingIdx + i - 1;
        var todayIdx = middleIdx + i;
        T numerator = T.Zero, numeratorSub = T.Zero;
        T tempReal;
        for (i = middleIdx; i >= trailingIdx; i--)
        {
            tempReal = inReal[i];
            numeratorSub += tempReal;
            numerator += numeratorSub;
        }

        var numeratorAdd = T.Zero;
        middleIdx++;
        for (i = middleIdx; i <= todayIdx; i++)
        {
            tempReal = inReal[i];
            numeratorAdd += tempReal;
            numerator += numeratorAdd;
        }

        // The value at the trailingIdx was saved in tempReal to account for the case where
        // output and input can point the same buffer.
        outIdx = 0;
        tempReal = inReal[trailingIdx++];
        outReal[outIdx++] = numerator * factor;
        todayIdx++;

        while (todayIdx <= endIdx)
        {
            numerator -= numeratorSub;
            numeratorSub -= tempReal;
            tempReal = inReal[middleIdx++];
            numeratorSub += tempReal;

            numeratorAdd -= tempReal;
            numerator += numeratorAdd;
            tempReal = inReal[todayIdx++];
            numeratorAdd += tempReal;

            numerator += tempReal;

            tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = numerator * factor;
        }
    }
}
