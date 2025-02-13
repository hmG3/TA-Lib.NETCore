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
    /// Hilbert Transform - Dominant Cycle Period (Cycle Indicators)
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
    /// Hilbert Transform - Dominant Cycle Period identifies the dominant cycle length in market data.
    /// It applies a series of transformations to identify periodic patterns and their dominant frequency.
    /// <para>
    /// The function is used in advanced technical analysis to identify cycles in financial time series data, helping to determine
    /// market trends and timing for entry or exit positions. The function can be integrated into cycle-based approaches
    /// to time entries or exits.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Smooth the input data using a weighted moving average (WMA) to reduce noise while preserving the underlying signal.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply the Hilbert Transform to compute in-phase (I) and quadrature (Q) components for odd and even price bars.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the arctangent of Q and I components to compute phase changes between successive price bars.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Use the phase change to estimate the instantaneous period of the dominant cycle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply a smoothing factor to stabilize the calculated period and reduce the impact of noise and outliers.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The output represents the dominant cycle period in the data.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A smaller period indicates faster market cycles, while a larger period signals slower cycles.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Limitations</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The function is more effective in cyclical or ranging markets and may produce unreliable results in strong trending conditions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The output is sensitive to noisy data; smoothing techniques, such as WMA, help mitigate this.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode HtDcPeriod<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtDcPeriodImpl(inReal, inRange, outReal, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="HtDcPeriod{T}">HtDcPeriod</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    /// <remarks>See <see cref="MamaLookback">MamaLookback</see> for an explanation of the "32"</remarks>
    [PublicAPI]
    public static int HtDcPeriodLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtDcPeriod) + 32;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HtDcPeriod<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtDcPeriodImpl<T>(inReal, inRange, outReal, out outRange);

    private static Core.RetCode HtDcPeriodImpl<T>(
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

        var lookbackTotal = HtDcPeriodLookback();
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

        T prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod;
        var period = prevI2 = prevQ2 = re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = T.Zero;

        // The code is speed optimized and is most likely very hard to follow if you do not already know well the original algorithm.
        while (today <= endIdx)
        {
            var adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            FunctionHelpers.DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);

            PerformHilbertTransform(today, circBuffer, smoothedValue, adjustedPrevPeriod, prevQ2, prevI2, ref hilbertIdx,
                ref i1ForEvenPrev3, ref i1ForOddPrev3, ref i1ForOddPrev2, out var q2, out var i2, ref i1ForEvenPrev2);

            // Adjust the period for next price bar
            FunctionHelpers.HTHelper.CalcSmoothedPeriod(ref re, i2, q2, ref prevI2, ref prevQ2, ref im, ref period);

            smoothPeriod = T.CreateChecked(0.33) * period + T.CreateChecked(0.67) * smoothPeriod;

            if (today >= startIdx)
            {
                outReal[outIdx++] = smoothPeriod;
            }

            today++;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static void PerformHilbertTransform<T>(
        int today,
        Span<T> circBuffer,
        T smoothedValue,
        T adjustedPrevPeriod,
        T prevQ2,
        T prevI2,
        ref int hilbertIdx,
        ref T i1ForEvenPrev3,
        ref T i1ForOddPrev3,
        ref T i1ForOddPrev2,
        out T q2,
        out T i2,
        ref T i1ForEvenPrev2) where T : IFloatingPointIeee754<T>
    {
        if (today % 2 == 0)
        {
            // Do the Hilbert Transforms for even price bar
            FunctionHelpers.HTHelper.CalcHilbertEven(circBuffer, smoothedValue, ref hilbertIdx, adjustedPrevPeriod, i1ForEvenPrev3, prevQ2,
                prevI2,
                out i1ForOddPrev3, ref i1ForOddPrev2, out q2, out i2);
        }
        else
        {
            // Do the Hilbert Transforms for odd price bar
            FunctionHelpers.HTHelper.CalcHilbertOdd(circBuffer, smoothedValue, hilbertIdx, adjustedPrevPeriod, out i1ForEvenPrev3, prevQ2,
                prevI2,
                i1ForOddPrev3, ref i1ForEvenPrev2, out q2, out i2);
        }
    }
}
