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
    /// Hilbert Transform - Trend vs Cycle Mode (Cycle Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outInteger">A span to store the calculated mode values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Hilbert Transform - Trend vs Cycle Mode is a cycle indicator that determines whether the market is in a trending state or
    /// a cyclic state. It achieves this by analyzing dominant cycles in the data and
    /// comparing their properties against predefined thresholds.
    /// <para>
    /// The function can help select appropriate indicators for the current market mode.
    /// In trend mode, trend-following tools may be favored; in cycle mode, oscillators may yield better results.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Smooth the input prices using a weighted moving average (WMA) to reduce noise and emphasize key trends.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply the Hilbert Transform to extract in-phase (I) and quadrature (Q) components, which are used to calculate the dominant cycle properties.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the dominant cycle phase (DC Phase) and smooth the phase over successive iterations.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Generate sine and leading sine values using the smoothed DC phase for cycle mode detection.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Analyze crossings and deviations of the sine wave components to determine whether the series is in a trend or cycle mode.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A <i>trend mode (1)</i> is indicated when the market shows sustained directional movement,
    ///       with cycles playing a secondary role.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A <i>cycle mode (0)</i> is indicated when the market oscillates within a well-defined range,
    ///       and dominant cycles can be observed.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Limitations</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       This function is less effective in noisy or volatile markets where dominant cycles are hard to detect.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///        False positives may occur in markets transitioning between trending and cyclic behavior.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode HtTrendMode<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<int> outInteger,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtTrendModeImpl(inReal, inRange, outInteger, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="HtTrendMode{T}">HtTrendMode</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    /// <remarks>
    /// 31 inputs are skipped, for compatibility with Tradestation.
    /// See <see cref="MamaLookback">MamaLookback</see> for an explanation of the "32"
    /// </remarks>
    [PublicAPI]
    public static int HtTrendModeLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtTrendMode) + 31 + 32;

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

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        const int smoothPriceSize = 50;
        Span<T> smoothPrice = new T[smoothPriceSize];

        var iTrend3 = T.Zero;
        var iTrend2 = iTrend3;
        var iTrend1 = iTrend2;
        var daysInTrend = 0;
        var sine = T.Zero;
        var leadSine = T.Zero;

        var lookbackTotal = HtTrendModeLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;

        FunctionHelpers.HTHelper.InitWma(inReal, startIdx, lookbackTotal, out var periodWMASub, out var periodWMASum,
            out var trailingWMAValue, out var trailingWMAIdx, 34, out var today);

        var hilbertIdx = 0;
        var smoothPriceIdx = 0;

        /* Initialize the circular buffer used by the hilbert transform logic.
         * A buffer is used for odd day and another for even days.
         * This minimizes the number of memory access and floating point operations needed
         * By using static circular buffer, no large dynamic memory allocation is needed for storing intermediate calculation.
         */
        Span<T> circBuffer = FunctionHelpers.HTHelper.BufferFactory<T>();

        var outIdx = 0;

        T prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod, dcPhase;
        var period = prevI2 = prevQ2 =
            re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = dcPhase = T.Zero;

        // The code is speed optimized and is most likely very hard to follow if you do not already know well the original algorithm.
        while (today <= endIdx)
        {
            var adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            FunctionHelpers.DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);

            // Remember the smoothedValue into the smoothPrice circular buffer.
            smoothPrice[smoothPriceIdx] = smoothedValue;

            PerformHilbertTransform(today, circBuffer, smoothedValue, adjustedPrevPeriod, prevQ2, prevI2, ref hilbertIdx,
                ref i1ForEvenPrev3, ref i1ForOddPrev3, ref i1ForOddPrev2, out var q2, out var i2, ref i1ForEvenPrev2);

            // Adjust the period for next price bar
            FunctionHelpers.HTHelper.CalcSmoothedPeriod(ref re, i2, q2, ref prevI2, ref prevQ2, ref im, ref period);

            smoothPeriod = T.CreateChecked(0.33) * period + T.CreateChecked(0.67) * smoothPeriod;

            var prevDCPhase = dcPhase;

            /* Compute Dominant Cycle Phase */
            dcPhase = ComputeDcPhase(smoothPrice, smoothPeriod, smoothPriceIdx, dcPhase);

            var prevSine = sine;
            var prevLeadSine = leadSine;

            sine = T.Sin(T.DegreesToRadians(dcPhase));
            leadSine = T.Sin(T.DegreesToRadians(dcPhase + FunctionHelpers.Ninety<T>() / FunctionHelpers.Two<T>()));

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
        if (!T.IsZero(smoothPeriod) &&
            tempReal > T.CreateChecked(0.67) * FunctionHelpers.Ninety<T>() * FunctionHelpers.Four<T>() / smoothPeriod &&
            tempReal < T.CreateChecked(1.5) * FunctionHelpers.Ninety<T>() * FunctionHelpers.Four<T>() / smoothPeriod)
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
