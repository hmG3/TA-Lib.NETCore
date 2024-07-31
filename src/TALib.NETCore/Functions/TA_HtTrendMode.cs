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
    public static Core.RetCode HtTrendMode<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<int> outInteger,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtTrendModeImpl(inReal, inRange, outInteger, out outRange);

    [PublicAPI]
    public static int HtTrendModeLookback() =>
        /*  31 input are skip
         * +32 output are skip to account for misc lookback
         * ──────────────────
         *  63 Total Lookback
         *
         * See MamaLookback for an explanation of the "32"
         */
        Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtTrendMode) + 63;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HtTrendMode<T>(
        T[] inReal,
        Range inRange,
        int[] outInteger,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtTrendModeImpl<T>(inReal, inRange, outInteger, out outRange);

    private static Core.RetCode HtTrendModeImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<int> outInteger,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        const int smoothPriceSize = 50;
        Span<T> smoothPrice = new T[smoothPriceSize];

        var iTrend3 = T.Zero;
        var iTrend2 = iTrend3;
        var iTrend1 = iTrend2;
        int daysInTrend = default;
        var sine = T.Zero;
        var leadSine = T.Zero;

        var lookbackTotal = HtTrendModeLookback();
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

        var i = 34;
        do
        {
            tempReal = inReal[today++];
            // Evaluate subsequent WMA value
            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
        } while (--i != 0);

        int hilbertIdx = default;
        int smoothPriceIdx = default;

        /* Initialize the circular buffer used by the hilbert transform logic.
         * A buffer is used for odd day and another for even days.
         * This minimizes the number of memory access and floating point operations needed
         * By using static circular buffer, no large dynamic memory allocation is needed for storing intermediate calculation.
         */
        Span<T> circBuffer = HTHelper.BufferFactory<T>();

        int outIdx = default;

        T prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod, dcPhase;
        var period = prevI2 = prevQ2 =
            re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = dcPhase = T.Zero;

        // The code is speed optimized and is most likely very hard to follow if you do not already know well the original algorithm.
        while (today <= endIdx)
        {
            var adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);

            // Remember the smoothedValue into the smoothPrice circular buffer.
            smoothPrice[smoothPriceIdx] = smoothedValue;

            T q2;
            T i2;
            if (today % 2 == 0)
            {
                // Do the Hilbert Transforms for even price bar
                HTHelper.CalcHilbertEven(circBuffer, smoothedValue, ref hilbertIdx, adjustedPrevPeriod, i1ForEvenPrev3, prevQ2, prevI2,
                    out i1ForOddPrev3, ref i1ForOddPrev2, out q2, out i2);
            }
            else
            {
                // Do the Hilbert Transforms for odd price bar
                HTHelper.CalcHilbertOdd(circBuffer, smoothedValue, hilbertIdx, adjustedPrevPeriod, out i1ForEvenPrev3, prevQ2, prevI2,
                    i1ForOddPrev3, ref i1ForEvenPrev2, out q2, out i2);
            }

            // Adjust the period for next price bar
            HTHelper.CalcSmoothedPeriod(ref re, i2, q2, ref prevI2, ref prevQ2, ref im, ref period);

            smoothPeriod = T.CreateChecked(0.33) * period + T.CreateChecked(0.67) * smoothPeriod;

            var prevDCPhase = dcPhase;

            /* Compute Dominant Cycle Phase */
            var dcPeriod = smoothPeriod + T.CreateChecked(0.5);
            var dcPeriodInt = Int32.CreateTruncating(dcPeriod);
            var realPart = T.Zero;
            var imagPart = T.Zero;

            // idx is used to iterate for up to 50 of the last value of smoothPrice.
            var idx = smoothPriceIdx;
            for (i = 0; i < dcPeriodInt; i++)
            {
                tempReal = T.CreateChecked(i) * Two<T>() * T.Pi / T.CreateChecked(dcPeriodInt);
                var tempReal2 = smoothPrice[idx];
                realPart += T.Sin(tempReal) * tempReal2;
                imagPart += T.Cos(tempReal) * tempReal2;
                if (idx == 0)
                {
                    idx = smoothPriceSize - 1;
                }
                else
                {
                    idx--;
                }
            }

            tempReal = T.Abs(imagPart);
            if (tempReal > T.Zero)
            {
                dcPhase = T.RadiansToDegrees(T.Atan(realPart / imagPart));
            }
            else if (tempReal <= T.CreateChecked(0.01))
            {
                if (realPart < T.Zero)
                {
                    dcPhase -= Ninety<T>();
                }
                else if (realPart > T.Zero)
                {
                    dcPhase += Ninety<T>();
                }
            }
            dcPhase += Ninety<T>();

            // Compensate for one bar lag of the weighted moving average
            dcPhase += Ninety<T>() * Four<T>() / smoothPeriod;
            if (imagPart < T.Zero)
            {
                dcPhase += Ninety<T>() * Two<T>();
            }

            if (dcPhase > Ninety<T>() * T.CreateChecked(3.5))
            {
                dcPhase -= Ninety<T>() * Four<T>();
            }

            var prevSine = sine;
            var prevLeadSine = leadSine;
            sine = T.Sin(T.DegreesToRadians(dcPhase));
            leadSine = T.Sin(T.DegreesToRadians(dcPhase + Ninety<T>() / Two<T>()));

            // idx is used to iterate for up to 50 of the last value of smoothPrice.
            idx = today;
            tempReal = T.Zero;
            for (i = 0; i < dcPeriodInt; i++)
            {
                tempReal += inReal[idx--];
            }

            if (dcPeriodInt > 0)
            {
                tempReal /= T.CreateChecked(dcPeriodInt);
            }

            // Compute Trendline
            var trendLine = (Four<T>() * tempReal + Three<T>() * iTrend1 + Two<T>() * iTrend2 + iTrend3) / T.CreateChecked(10);
            iTrend3 = iTrend2;
            iTrend2 = iTrend1;
            iTrend1 = tempReal;

            // Compute the Trend Mode and assume trend by default
            var trend = 1;

            // Measure days in trend from last crossing of the SineWave Indicator lines
            if (sine > leadSine && prevSine <= prevLeadSine || sine < leadSine && prevSine >= prevLeadSine)
            {
                daysInTrend = 0;
                trend = 0;
            }

            if (T.CreateChecked(++daysInTrend) < T.CreateChecked(0.5) * smoothPeriod)
            {
                trend = 0;
            }

            tempReal = dcPhase - prevDCPhase;
            if (!T.IsZero(smoothPeriod) && tempReal > T.CreateChecked(0.67) * Ninety<T>() * Four<T>() / smoothPeriod &&
                tempReal < T.CreateChecked(1.5) * Ninety<T>() * Four<T>() / smoothPeriod)
            {
                trend = 0;
            }

            tempReal = smoothPrice[smoothPriceIdx];
            if (!T.IsZero(trendLine) && T.Abs((tempReal - trendLine) / trendLine) >= T.CreateChecked(0.015))
            {
                trend = 1;
            }

            if (today >= startIdx)
            {
                outInteger[outIdx++] = trend;
            }

            if (++smoothPriceIdx > smoothPriceSize - 1)
            {
                smoothPriceIdx = 0;
            }

            today++;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
