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
    /// Hilbert Transform - Dominant Cycle Phase (Cycle Indicators)
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
    /// Hilbert Transform - Dominant Cycle Phase determines the phase of the dominant price cycle,
    /// locating the current position within the cycle.
    /// This phase angle provides insights into the position within the cycle, which can be used for timing and trend analysis.
    /// <para>
    /// The function is useful in identifying cycles and their phases in financial data.
    /// It helps in identifying overbought and oversold conditions and potential reversals in price movements.
    /// The function can enhance timing when combined with <see cref="HtDcPeriod{T}">HT DC Period</see> or other cycle tools.
    /// Adding trend and momentum measures can refine decisions further.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Smooth the input prices using a weighted moving average (WMA) to remove noise and stabilize the data.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Use the Hilbert Transform to compute the in-phase (I) and quadrature (Q) components for even and odd price bars.
    ///       These components form the basis of the phase calculation.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the real and imaginary parts of the dominant cycle phase using trigonometric operations over the smoothed prices.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Derive the phase angle from the real and imaginary parts. Adjust the phase angle for small imaginary components
    ///       and account for one-bar lags introduced by the WMA.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Perform final phase adjustments to ensure the result is within the expected range.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value provides the current position within a market cycle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A rising value may indicate the beginning of a bullish trend, while a falling phase may signal bearish trends.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A falling value may signal bearish trends.
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
    public static Core.RetCode HtDcPhase<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtDcPhaseImpl(inReal, inRange, outReal, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="HtDcPhase{T}">HtDcPhase</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    /// <remarks>
    /// 31 inputs are skipped, for compatibility with Tradestation.
    /// See <see cref="MamaLookback">MamaLookback</see> for an explanation of the "32"
    /// </remarks>
    [PublicAPI]
    public static int HtDcPhaseLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtDcPhase) + 31 + 32;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HtDcPhase<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        HtDcPhaseImpl<T>(inReal, inRange, outReal, out outRange);

    private static Core.RetCode HtDcPhaseImpl<T>(
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

        var lookbackTotal = HtDcPhaseLookback();
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
                outReal[outIdx++] = dcPhase;
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

    private static T ComputeDcPhase<T>(
        Span<T> smoothPrice,
        T smoothPeriod,
        int smoothPriceIdx,
        T dcPhase) where T : IFloatingPointIeee754<T>
    {
        var dcPeriod = smoothPeriod + T.CreateChecked(0.5);
        var dcPeriodInt = Int32.CreateTruncating(dcPeriod);
        var realPart = T.Zero;
        var imagPart = T.Zero;

        var idx = smoothPriceIdx;
        for (var i = 0; i < dcPeriodInt; i++)
        {
            var tempReal = T.CreateChecked(i) * FunctionHelpers.Two<T>() * T.Pi / T.CreateChecked(dcPeriodInt);
            var tempReal2 = smoothPrice[idx];
            realPart += T.Sin(tempReal) * tempReal2;
            imagPart += T.Cos(tempReal) * tempReal2;
            idx = idx == 0 ? smoothPrice.Length - 1 : idx - 1;
        }

        dcPhase = CalcDcPhase(realPart, imagPart, dcPhase, smoothPeriod);

        return dcPhase;
    }

    private static T CalcDcPhase<T>(
        T realPart,
        T imagPart,
        T dcPhase,
        T smoothPeriod) where T : IFloatingPointIeee754<T>
    {
        var tempReal = T.Abs(imagPart);
        T dcPhaseValue = T.Zero;
        if (tempReal > T.Zero)
        {
            dcPhaseValue = T.RadiansToDegrees(T.Atan(realPart / imagPart));
        }
        else if (tempReal <= T.CreateChecked(0.01))
        {
            dcPhaseValue = AdjustPhaseForSmallImaginaryPart(realPart, dcPhase);
        }

        dcPhase = FinalPhaseAdjustments(imagPart, dcPhaseValue, smoothPeriod);

        return dcPhase;
    }

    private static T AdjustPhaseForSmallImaginaryPart<T>(T realPart, T dcPhase) where T : IFloatingPointIeee754<T>
    {
        if (realPart < T.Zero)
        {
            dcPhase -= FunctionHelpers.Ninety<T>();
        }
        else if (realPart > T.Zero)
        {
            dcPhase += FunctionHelpers.Ninety<T>();
        }

        return dcPhase;
    }

    private static T FinalPhaseAdjustments<T>(T imagPart, T dcPhase, T smoothPeriod) where T : IFloatingPointIeee754<T>
    {
        dcPhase += FunctionHelpers.Ninety<T>();
        // Compensate for one bar lag of the weighted moving average
        dcPhase += FunctionHelpers.Ninety<T>() * FunctionHelpers.Four<T>() / smoothPeriod;

        if (imagPart < T.Zero)
        {
            dcPhase += FunctionHelpers.Ninety<T>() * FunctionHelpers.Two<T>();
        }

        if (dcPhase > FunctionHelpers.Ninety<T>() * T.CreateChecked(3.5))
        {
            dcPhase -= FunctionHelpers.Ninety<T>() * FunctionHelpers.Four<T>();
        }

        return dcPhase;
    }
}
