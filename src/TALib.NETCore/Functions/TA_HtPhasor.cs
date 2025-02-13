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
    /// Hilbert Transform - Phasor Components (Cycle Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outInPhase">A span to store the calculated "in-phase" component values.</param>
    /// <param name="outQuadrature">A span to store the calculated "quadrature" component values.</param>
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
    /// Hilbert Transform - Phasor Components is a technical indicator that decomposes price data into its
    /// in-phase and quadrature components. These components represent the real and imaginary parts of the signal, respectively,
    /// and are essential for analyzing cyclic properties of price data.
    /// <para>
    /// The function is generally used in cycle-focused analysis. Integrating it with conventional trend or momentum indicators
    /// can validate cyclical signals.
    /// The function is useful in identifying cycles and their phases in financial data, enabling to anticipate price
    /// reversals or continuations.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Smooth the input prices using a weighted moving average (WMA) to minimize noise and stabilize the underlying data.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply the Hilbert Transform to determine the in-phase (I) and quadrature (Q) components for both even and odd bars.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the real and imaginary parts of the phase using trigonometric calculations applied to the smoothed data.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Derive the phase angle from the real and imaginary parts, adjusting for small imaginary values and any WMA-induced lag.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Perform final adjustments to the phase angle to ensure it fits within the expected range of values.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The <i>in-phase component</i> represents the price data's position within a cycle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The <i>quadrature component</i> captures the signal's delay or lag relative to the cycle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       By plotting these components on a polar graph, the cyclic behavior can be observed, and the phase shifts can be identified,
    ///       which may indicate trend reversals or transitions.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Limitations</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The function is most effective in markets with cyclic behavior and
    ///       may produce unreliable results in trending or highly volatile markets.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The accuracy of the components depends on the quality of the smoothed input data.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode HtPhasor<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outInPhase,
        Span<T> outQuadrature,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtPhasorImpl(inReal, inRange, outInPhase, outQuadrature, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="HtPhasor{T}">HtPhasor</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    /// <remarks>
    /// See <see cref="MamaLookback">MamaLookback</see> for an explanation of the "32"
    /// </remarks>
    [PublicAPI]
    public static int HtPhasorLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtPhasor) + 32;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HtPhasor<T>(
        T[] inReal,
        Range inRange,
        T[] outInPhase,
        T[] outQuadrature,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtPhasorImpl<T>(inReal, inRange, outInPhase, outQuadrature, out outRange);

    private static Core.RetCode HtPhasorImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outInPhase,
        Span<T> outQuadrature,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var lookbackTotal = HtPhasorLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;

        FunctionHelpers.HTHelper.InitWma(inReal, startIdx, lookbackTotal, out var periodWMASub, out var periodWMASum,
            out var trailingWMAValue, out var trailingWMAIdx, 9, out var today);

        var hilbertIdx = 0;

        /* Initialize the circular buffer used by the hilbert transform logic.
         * A buffer is used for odd day and another for even days.
         * This minimizes the number of memory access and floating point operations needed
         * By using static circular buffer, no large dynamic memory allocation is needed for storing intermediate calculation.
         */
        Span<T> circBuffer = FunctionHelpers.HTHelper.BufferFactory<T>();

        var outIdx = 0;

        T prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2;
        var period = prevI2 = prevQ2 = re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = T.Zero;

        // The code is speed optimized and is most likely very hard to follow if you do not already know well the original algorithm.
        while (today <= endIdx)
        {
            var adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            FunctionHelpers.DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);

            PerformPhasorHilbertTransform(outInPhase, outQuadrature, today, circBuffer, smoothedValue, adjustedPrevPeriod, prevQ2, prevI2,
                startIdx, ref hilbertIdx, ref i1ForEvenPrev3, ref i1ForOddPrev3, ref i1ForOddPrev2, out var q2, out var i2, ref outIdx,
                ref i1ForEvenPrev2);

            // Adjust the period for next price bar
            FunctionHelpers.HTHelper.CalcSmoothedPeriod(ref re, i2, q2, ref prevI2, ref prevQ2, ref im, ref period);

            today++;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static void PerformPhasorHilbertTransform<T>(
        Span<T> outInPhase,
        Span<T> outQuadrature,
        int today,
        Span<T> circBuffer,
        T smoothedValue,
        T adjustedPrevPeriod,
        T prevQ2,
        T prevI2,
        int startIdx,
        ref int hilbertIdx,
        ref T i1ForEvenPrev3,
        ref T i1ForOddPrev3,
        ref T i1ForOddPrev2,
        out T q2,
        out T i2,
        ref int outIdx,
        ref T i1ForEvenPrev2)
        where T : IFloatingPointIeee754<T>
    {
        if (today % 2 == 0)
        {
            // Do the Hilbert Transforms for even price bar
            FunctionHelpers.HTHelper.CalcHilbertEven(circBuffer, smoothedValue, ref hilbertIdx, adjustedPrevPeriod, i1ForEvenPrev3, prevQ2,
                prevI2, out i1ForOddPrev3, ref i1ForOddPrev2, out q2, out i2);

            if (today >= startIdx)
            {
                outQuadrature[outIdx] = circBuffer[(int) FunctionHelpers.HTHelper.HilbertKeys.Q1];
                outInPhase[outIdx++] = i1ForEvenPrev3;
            }
        }
        else
        {
            // Do the Hilbert Transforms for odd price bar
            FunctionHelpers.HTHelper.CalcHilbertOdd(circBuffer, smoothedValue, hilbertIdx, adjustedPrevPeriod, out i1ForEvenPrev3, prevQ2,
                prevI2, i1ForOddPrev3, ref i1ForEvenPrev2, out q2, out i2);

            if (today >= startIdx)
            {
                outQuadrature[outIdx] = circBuffer[(int) FunctionHelpers.HTHelper.HilbertKeys.Q1];
                outInPhase[outIdx++] = i1ForOddPrev3;
            }
        }
    }
}
