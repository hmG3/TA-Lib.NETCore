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
    /// Hilbert Transform - SineWave (Cycle Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outSine">A span to store the sine wave values.</param>
    /// <param name="outLeadSine">A span to store the leading sine wave values.</param>
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
    /// Hilbert Transform - SineWave identifies and visualizes the cyclic behavior of time series data by calculating
    /// the sine and leading sine components of the dominant cycle. These components are particularly useful for analyzing market trends
    /// and detecting potential reversals.
    /// <para>
    /// This function can assist in timing entries or exits in cyclic conditions. Confirming identified turning points with trend analysis,
    /// oscillators like <see cref="Rsi{T}">RSI</see>, or cycle-based tools such as <see cref="HtDcPeriod{T}">HT DC Period</see> can improve reliability.
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
    ///       Apply the Hilbert Transform to extract in-phase (I) and quadrature (Q) components for even and odd bars, capturing cycle properties.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the dominant cycle phase (DCPhase) using the I and Q components. This provides the current position in the cycle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the sine of the DCPhase (outSine) and the leading sine (outLeadSine), which is the phase shifted by 45 degrees.
    ///       These values provide insights into the cyclic movements and potential turning points.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation*</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The <i>sine wave</i> represents the current phase of the dominant cycle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The <i>leading sine wave</i> is the sine wave shifted forward by 45 degrees, helping identify early phase transitions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       When the sine wave and leading sine wave intersect, it may indicate a potential cycle peak or trough,
    ///       signaling possible market reversals.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Limitations</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The function is most effective in cyclic markets and may produce unreliable signals in trending or highly volatile markets.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       It is sensitive to noise in the input data; therefore, appropriate smoothing is critical for accurate results.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode HtSine<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outSine,
        Span<T> outLeadSine,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtSineImpl(inReal, inRange, outSine, outLeadSine, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="HtSine{T}">HtSine</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    /// <remarks>
    /// 31 inputs are skipped, for compatibility with Tradestation.
    /// See <see cref="MamaLookback">MamaLookback</see> for an explanation of the "32"
    /// </remarks>
    [PublicAPI]
    public static int HtSineLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtSine) + 31 + 32;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HtSine<T>(
        T[] inReal,
        Range inRange,
        T[] outSine,
        T[] outLeadSine,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtSineImpl<T>(inReal, inRange, outSine, outLeadSine, out outRange);

    private static Core.RetCode HtSineImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outSine,
        Span<T> outLeadSine,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var lookbackTotal = HtSineLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        const int smoothPriceSize = 50;
        Span<T> smoothPrice = new T[smoothPriceSize];

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

            dcPhase = ComputeDcPhase(smoothPrice, smoothPeriod, smoothPriceIdx, dcPhase);

            if (today >= startIdx)
            {
                outSine[outIdx] = T.Sin(T.DegreesToRadians(dcPhase));
                outLeadSine[outIdx++] = T.Sin(T.DegreesToRadians(dcPhase + T.CreateChecked(45)));
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
