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
    /// MESA Adaptive Moving Average (Overlap Studies)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outMAMA">A span to store the calculated MAMA values.</param>
    /// <param name="outFAMA">A span to store the calculated FAMA values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInFastLimit">
    /// The upper bound for the adaptive factor:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values increase responsiveness to price changes, making the MAMA more sensitive to market trends.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values reduce responsiveness, smoothing the MAMA output.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Valid range is <c>0.01..0.99</c>. Values near 0.99 maximize sensitivity, while values closer to 0.01 prioritize smoothing.
    /// </para>
    /// </param>
    /// <param name="optInSlowLimit">
    /// The lower bound for the adaptive factor:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values reduce the minimum responsiveness, adding stability to the MAMA during market consolidations.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values increase minimum responsiveness, allowing faster reactions to market changes.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Valid range is <c>0.01..0.99</c>. Values near 0.99 reduce noise, while values closer to 0.01 allow greater flexibility.
    /// </para>
    /// </param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// MESA Adaptive Moving Average dynamically adjusts its responsiveness based on the dominant cycle in the market.
    /// It utilizes a combination of the Hilbert Transform and alpha factor to adapt to changing market conditions, producing
    /// two outputs: the MAMA and the FAMA (Following Adaptive Moving Average).
    /// <para>
    /// The function's adaptability allows it to respond quickly during trends while minimizing false signals in consolidation phases.
    /// Combining it with <see cref="Adx{T}">ADX</see>, <see cref="Rsi{T}">RSI</see>,
    /// or volatility measures like <see cref="Atr{T}">ATR</see> can refine strategy development.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Apply a Weighted Moving Average (WMA) to smooth the input prices and reduce noise.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Perform the Hilbert Transform on the smoothed data to extract in-phase (I) and quadrature (Q) components.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the dominant cycle period based on phase differences between successive in-phase and quadrature values.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the alpha factor using the fast and slow limits, which determines the level of responsiveness:
    ///       <code>
    ///         Alpha = FastLimit / DeltaPhase
    ///       </code>
    ///       Adjustments are made to ensure alpha stays within the range defined by the slow and fast limits.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Update the MAMA using the current price and alpha, and calculate the FAMA as a smoothed version of the MAMA.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       <i>MAMA</i> tracks the dominant trend with reduced lag.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <i>FAMA</i> provides additional smoothing, acting as a signal line to identify changes in trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Crossovers between MAMA and FAMA can indicate potential buy or sell signals: a bullish crossover occurs when MAMA crosses
    ///       above FAMA, and a bearish crossover occurs when MAMA crosses below FAMA.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Returns the lookback period for <see cref="Mama{T}">Mama</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    /// <remarks>
    /// The fix lookback is 32 and is established as follows:
    /// <list type="bullet">
    /// <item><description>12 price bar to be compatible with the implementation of TradeStation found in John Ehlers book.</description></item>
    /// <item><description>6 price bars for the <c>Detrender</c></description></item>
    /// <item><description>6 price bars for <c>Q1</c></description></item>
    /// <item><description>3 price bars for <c>JI</c></description></item>
    /// <item><description>3 price bars for <c>JQ</c></description></item>
    /// <item><description>1 price bar for <c>Re</c>/<c>Im</c></description></item>
    /// <item><description>1 price bar for the <c>Delta Phase</c></description></item>
    /// <item><description>————————</description></item>
    /// <item><description>32 total</description></item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static int MamaLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Mama) + 32;

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

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

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

        T prevI2, prevQ2, re, im, mama, fama, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, prevPhase;
        var period = prevI2 = prevQ2
            = re = im = mama = fama = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = prevPhase = T.Zero;

        // The code is speed optimized and is most likely very hard to follow if you do not already know well the original algorithm.
        while (today <= endIdx)
        {
            var adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            var todayValue = inReal[today];
            FunctionHelpers.DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, todayValue,
                out var smoothedValue);

            var tempReal2 = PerformMAMAHilbertTransform(today, circBuffer, smoothedValue, ref hilbertIdx, adjustedPrevPeriod,
                ref i1ForOddPrev3, ref i1ForEvenPrev3, ref i1ForOddPrev2, ref i1ForEvenPrev2, prevQ2, prevI2, out var i2, out var q2);

            // Put Delta Phase into tempReal
            var tempReal = prevPhase - tempReal2;
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
            FunctionHelpers.HTHelper.CalcSmoothedPeriod(ref re, i2, q2, ref prevI2, ref prevQ2, ref im, ref period);

            today++;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static T PerformMAMAHilbertTransform<T>(
        int today,
        Span<T> circBuffer,
        T smoothedValue,
        ref int hilbertIdx,
        T adjustedPrevPeriod,
        ref T i1ForOddPrev3,
        ref T i1ForEvenPrev3,
        ref T i1ForOddPrev2,
        ref T i1ForEvenPrev2,
        T prevQ2,
        T prevI2,
        out T i2,
        out T q2) where T : IFloatingPointIeee754<T>
    {
        T tempReal2;
        if (today % 2 == 0)
        {
            FunctionHelpers.HTHelper.CalcHilbertEven(circBuffer, smoothedValue, ref hilbertIdx, adjustedPrevPeriod, i1ForEvenPrev3, prevQ2,
                prevI2,
                out i1ForOddPrev3, ref i1ForOddPrev2, out q2, out i2);

            tempReal2 = !T.IsZero(i1ForEvenPrev3)
                ? T.RadiansToDegrees(T.Atan(circBuffer[(int) FunctionHelpers.HTHelper.HilbertKeys.Q1] / i1ForEvenPrev3))
                : T.Zero;
        }
        else
        {
            FunctionHelpers.HTHelper.CalcHilbertOdd(circBuffer, smoothedValue, hilbertIdx, adjustedPrevPeriod, out i1ForEvenPrev3, prevQ2,
                prevI2,
                i1ForOddPrev3, ref i1ForEvenPrev2, out q2, out i2);

            tempReal2 = !T.IsZero(i1ForOddPrev3)
                ? T.RadiansToDegrees(T.Atan(circBuffer[(int) FunctionHelpers.HTHelper.HilbertKeys.Q1] / i1ForOddPrev3))
                : T.Zero;
        }

        return tempReal2;
    }
}
