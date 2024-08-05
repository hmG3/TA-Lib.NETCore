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

        HTHelper.InitWma(inReal, startIdx, lookbackTotal, out var periodWMASub, out var periodWMASum, out var trailingWMAValue,
            out var trailingWMAIdx, 34, out var today);

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

            PerformHilbertTransform(today, circBuffer, smoothedValue, adjustedPrevPeriod, prevQ2, prevI2, ref hilbertIdx,
                ref i1ForEvenPrev3, ref i1ForOddPrev3, ref i1ForOddPrev2, out var q2, out var i2, ref i1ForEvenPrev2);

            // Adjust the period for next price bar
            HTHelper.CalcSmoothedPeriod(ref re, i2, q2, ref prevI2, ref prevQ2, ref im, ref period);

            smoothPeriod = T.CreateChecked(0.33) * period + T.CreateChecked(0.67) * smoothPeriod;

            var prevDCPhase = dcPhase;

            /* Compute Dominant Cycle Phase */
            dcPhase = ComputeDcPhase(smoothPrice, smoothPeriod, smoothPriceIdx, dcPhase);

            var prevSine = sine;
            var prevLeadSine = leadSine;

            sine = T.Sin(T.DegreesToRadians(dcPhase));
            leadSine = T.Sin(T.DegreesToRadians(dcPhase + Ninety<T>() / Two<T>()));

            // idx is used to iterate for up to 50 of the last value of smoothPrice.
            var trendLineValue = ComputeTrendLine(inReal, ref today, smoothPeriod, ref iTrend1, ref iTrend2, ref iTrend3);

            var trend = DetermineTrend(sine, leadSine, prevSine, prevLeadSine, smoothPeriod, dcPhase, prevDCPhase, smoothPrice,
                smoothPriceIdx, trendLineValue, ref daysInTrend);

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

    private static int DetermineTrend<T>(
        T sine,
        T leadSine,
        T prevSine,
        T prevLeadSine,
        T smoothPeriod,
        T dcPhase,
        T prevDCPhase,
        Span<T> smoothPrice,
        int smoothPriceIdx,
        T trendLineValue,
        ref int daysInTrend)
        where T : IFloatingPointIeee754<T>
    {
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

        var tempReal = dcPhase - prevDCPhase;
        if (!T.IsZero(smoothPeriod) && tempReal > T.CreateChecked(0.67) * Ninety<T>() * Four<T>() / smoothPeriod &&
            tempReal < T.CreateChecked(1.5) * Ninety<T>() * Four<T>() / smoothPeriod)
        {
            trend = 0;
        }

        tempReal = smoothPrice[smoothPriceIdx];
        if (!T.IsZero(trendLineValue) && T.Abs((tempReal - trendLineValue) / trendLineValue) >= T.CreateChecked(0.015))
        {
            trend = 1;
        }

        return trend;
    }
}
