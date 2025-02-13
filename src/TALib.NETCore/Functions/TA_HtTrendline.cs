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
    /// Hilbert Transform - Instantaneous Trendline (Overlap Studies)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
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
    /// Hilbert Transform - Instantaneous Trendline is a cycle indicator designed to calculate a smooth trendline using
    /// the Hilbert Transform. It removes noise and provides an immediate trend representation by combining recent smoothed data points and
    /// extrapolating based on dominant cycles.
    /// <para>
    /// The function can be combined with <see cref="Adx{T}">ADX</see> or <see cref="Macd{T}">MACD</see> to ensure that changes
    /// in the trendline align with broader conditions, reducing the risk of acting on false signals.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Smooth the input prices using a weighted moving average (WMA) to reduce noise and ensure smoother transitions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply the Hilbert Transform to extract the in-phase (I) and quadrature (Q) components, which are used to determine the cycle properties.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the dominant cycle period using the I and Q components to estimate the cycle length dynamically.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the trendline using a weighted moving average of the smoothed values over the dominant cycle period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Store the calculated trendline values in the output span for visualization or further analysis.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The <i>trendline</i> provides a smooth representation of the market trend, filtering out short-term volatility.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Limitations</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The function is primarily effective in trending markets and may underperform in highly cyclic or volatile environments.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The algorithm assumes the presence of a dominant cycle, so markets lacking cyclic behavior may result in misleading trends.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode HtTrendline<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtTrendlineImpl(inReal, inRange, outReal, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="HtTrendline{T}">HtTrendline</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    /// <remarks>
    /// 31 inputs are skipped, for compatibility with Tradestation.
    /// See <see cref="MamaLookback">MamaLookback</see> for an explanation of the "32"
    /// </remarks>
    [PublicAPI]
    public static int HtTrendlineLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtTrendline) + 31 + 32;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HtTrendline<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtTrendlineImpl<T>(inReal, inRange, outReal, out outRange);

    private static Core.RetCode HtTrendlineImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var lookbackTotal = HtTrendlineLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        const int smoothPriceSize = 50;
        Span<T> smoothPrice = new T[smoothPriceSize];

        T iTrend2, iTrend1;
        var iTrend3 = iTrend2 = iTrend1 = T.Zero;

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

        T prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod;
        var period = prevI2 = prevQ2 = re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = T.Zero;

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

            var trendLineValue = ComputeTrendLine(inReal, ref today, smoothPeriod, ref iTrend1, ref iTrend2, ref iTrend3);

            if (today >= startIdx)
            {
                outReal[outIdx++] = trendLineValue;
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

    private static T ComputeTrendLine<T>(
        ReadOnlySpan<T> real,
        ref int today,
        T smoothPeriod,
        ref T iTrend1,
        ref T iTrend2,
        ref T iTrend3) where T : IFloatingPointIeee754<T>
    {
        var idx = today;
        var tempReal = T.Zero;
        var dcPeriod = Int32.CreateTruncating(smoothPeriod + T.CreateChecked(0.5));
        for (var i = 0; i < dcPeriod; i++)
        {
            tempReal += real[idx--];
        }

        if (dcPeriod > 0)
        {
            tempReal /= T.CreateChecked(dcPeriod);
        }

        var trendLine =
            (FunctionHelpers.Four<T>() * tempReal + FunctionHelpers.Three<T>() * iTrend1 + FunctionHelpers.Two<T>() * iTrend2 + iTrend3) /
            T.CreateChecked(10);

        iTrend3 = iTrend2;
        iTrend2 = iTrend1;
        iTrend1 = tempReal;

        return trendLine;
    }
}
