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
    public static Core.RetCode Mama<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outMAMA,
        Span<T> outFAMA,
        out Range outRange,
        double optInFastLimit = 0.5,
        double optInSlowLimit = 0.05) where T : IFloatingPointIeee754<T> =>
        MamaImpl(inReal, inRange, outMAMA, outFAMA, out outRange, optInFastLimit, optInSlowLimit);

    [PublicAPI]
    public static int MamaLookback()
    {
        /* The fix lookback is 32 and is established as follows:
         *
         * 12 price bar to be compatible with the implementation of TradeStation found in John Ehlers book.
         * 6 price bars for the Detrender
         * 6 price bars for Q1
         * 3 price bars for jI
         * 3 price bars for jQ
         * 1 price bar for Re/Im
         * 1 price bar for the Delta Phase
         * ────────
         * 32 Total
         */

        return Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Mama) + 32;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Mama<T>(
        T[] inReal,
        Range inRange,
        T[] outMAMA,
        T[] outFAMA,
        out Range outRange,
        double optInFastLimit = 0.5,
        double optInSlowLimit = 0.05) where T : IFloatingPointIeee754<T> =>
        MamaImpl<T>(inReal, inRange, outMAMA, outFAMA, out outRange, optInFastLimit, optInSlowLimit);

    private static Core.RetCode MamaImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outMAMA,
        Span<T> outFAMA,
        out Range outRange,
        double optInFastLimit,
        double optInSlowLimit) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        var startIdx = inRange.Start.Value;
        var endIdx = inRange.End.Value;

        if (endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInFastLimit < 0.01 || optInFastLimit > 0.99 || optInSlowLimit < 0.01 || optInSlowLimit > 0.99)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MamaLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;

        // Initialize the price smoother, which is simply a weighted moving average of the price.
        var trailingWMAIdx = startIdx - lookbackTotal;
        var today = trailingWMAIdx;

        // Initialization is same as WMA, except loop is unrolled for speed optimization.
        var tempReal = inReal[today++];
        var periodWMASub = tempReal;
        var periodWMASum = tempReal;
        tempReal = inReal[today++];
        periodWMASub += tempReal;
        periodWMASum += tempReal * Two<T>();
        tempReal = inReal[today++];
        periodWMASub += tempReal;
        periodWMASum += tempReal * Three<T>();

        var trailingWMAValue = T.Zero;

        var i = 9;
        do
        {
            tempReal = inReal[today++];
            // Evaluate subsequent WMA value
            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
        } while (--i != 0);

        int hilbertIdx = default;

        /* Initialize the circular buffer used by the hilbert transform logic.
         * A buffer is used for odd day and another for even days.
         * This minimizes the number of memory access and floating point operations needed
         * By using static circular buffer, no large dynamic memory allocation is needed for storing intermediate calculation.
         */
        Span<T> circBuffer = HTHelper.BufferFactory<T>();

        int outIdx = default;

        T prevI2, prevQ2, re, im, mama, fama, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, prevPhase;
        var period = prevI2 = prevQ2
            = re = im = mama = fama = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = prevPhase = T.Zero;

        // The code is speed optimized and is most likely very hard to follow if you do not already know well the original algorithm.
        while (today <= endIdx)
        {
            T tempReal2;
            T i2;
            T q2;

            var adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            var todayValue = inReal[today];
            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, todayValue,
                out var smoothedValue);
            if (today % 2 == 0)
            {
                // Do the Hilbert Transforms for even price bar
                HTHelper.CalcHilbertEven(circBuffer, smoothedValue, ref hilbertIdx, adjustedPrevPeriod, i1ForEvenPrev3, prevQ2, prevI2,
                    out i1ForOddPrev3, ref i1ForOddPrev2, out q2, out i2);

                // Put Alpha in tempReal2
                tempReal2 = !T.IsZero(i1ForEvenPrev3)
                    ? T.RadiansToDegrees(T.Atan(circBuffer[(int) HTHelper.HilbertKeys.Q1] / i1ForEvenPrev3))
                    : T.Zero;
            }
            else
            {
                // Do the Hilbert Transforms for odd price bar
                HTHelper.CalcHilbertOdd(circBuffer, smoothedValue, hilbertIdx, adjustedPrevPeriod, out i1ForEvenPrev3, prevQ2, prevI2,
                    i1ForOddPrev3, ref i1ForEvenPrev2, out q2, out i2);

                // Put Alpha in tempReal2
                tempReal2 = !T.IsZero(i1ForOddPrev3)
                    ? T.RadiansToDegrees(T.Atan(circBuffer[(int) HTHelper.HilbertKeys.Q1] / i1ForOddPrev3))
                    : T.Zero;
            }

            // Put Delta Phase into tempReal
            tempReal = prevPhase - tempReal2;
            prevPhase = tempReal2;
            if (tempReal < T.One)
            {
                tempReal = T.One;
            }

            // Put Alpha into tempReal
            if (tempReal > T.One)
            {
                tempReal = T.CreateChecked(optInFastLimit) / tempReal;
                if (tempReal < T.CreateChecked(optInSlowLimit))
                {
                    tempReal = T.CreateChecked(optInSlowLimit);
                }
            }
            else
            {
                tempReal = T.CreateChecked(optInFastLimit);
            }

            // Calculate MAMA, FAMA
            mama = tempReal * todayValue + (T.One - tempReal) * mama;
            tempReal *= T.CreateChecked(0.5);
            fama = tempReal * mama + (T.One - tempReal) * fama;
            if (today >= startIdx)
            {
                outMAMA[outIdx] = mama;
                outFAMA[outIdx++] = fama;
            }

            // Adjust the period for next price bar
            HTHelper.CalcSmoothedPeriod(ref re, i2, q2, ref prevI2, ref prevQ2, ref im, ref period);

            today++;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
