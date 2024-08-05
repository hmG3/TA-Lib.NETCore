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
    public static Core.RetCode HtPhasor<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outInPhase,
        Span<T> outQuadrature,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtPhasorImpl(inReal, inRange, outInPhase, outQuadrature, out outRange);

    [PublicAPI]
    public static int HtPhasorLookback() =>
        // See MamaLookback for an explanation of the "32"
        Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtPhasor) + 32;

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

        if (ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
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

        HTHelper.InitWma(inReal, startIdx, lookbackTotal, out var periodWMASub, out var periodWMASum, out var trailingWMAValue,
            out var trailingWMAIdx, 9, out var today);

        int hilbertIdx = default;

        /* Initialize the circular buffer used by the hilbert transform logic.
         * A buffer is used for odd day and another for even days.
         * This minimizes the number of memory access and floating point operations needed
         * By using static circular buffer, no large dynamic memory allocation is needed for storing intermediate calculation.
         */
        Span<T> circBuffer = HTHelper.BufferFactory<T>();

        int outIdx = default;

        T prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2;
        var period = prevI2 = prevQ2 = re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = T.Zero;

        // The code is speed optimized and is most likely very hard to follow if you do not already know well the original algorithm.
        while (today <= endIdx)
        {
            var adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);

            PerformPhasorHilbertTransform(outInPhase, outQuadrature, today, circBuffer, smoothedValue, adjustedPrevPeriod, prevQ2, prevI2,
                startIdx, ref hilbertIdx, ref i1ForEvenPrev3, ref i1ForOddPrev3, ref i1ForOddPrev2, out var q2, out var i2, ref outIdx,
                ref i1ForEvenPrev2);

            // Adjust the period for next price bar
            HTHelper.CalcSmoothedPeriod(ref re, i2, q2, ref prevI2, ref prevQ2, ref im, ref period);

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
            HTHelper.CalcHilbertEven(circBuffer, smoothedValue, ref hilbertIdx, adjustedPrevPeriod, i1ForEvenPrev3, prevQ2, prevI2,
                out i1ForOddPrev3, ref i1ForOddPrev2, out q2, out i2);

            if (today >= startIdx)
            {
                outQuadrature[outIdx] = circBuffer[(int) HTHelper.HilbertKeys.Q1];
                outInPhase[outIdx++] = i1ForEvenPrev3;
            }
        }
        else
        {
            // Do the Hilbert Transforms for odd price bar
            HTHelper.CalcHilbertOdd(circBuffer, smoothedValue, hilbertIdx, adjustedPrevPeriod, out i1ForEvenPrev3, prevQ2, prevI2,
                i1ForOddPrev3, ref i1ForEvenPrev2, out q2, out i2);

            if (today >= startIdx)
            {
                outQuadrature[outIdx] = circBuffer[(int) HTHelper.HilbertKeys.Q1];
                outInPhase[outIdx++] = i1ForOddPrev3;
            }
        }
    }
}
