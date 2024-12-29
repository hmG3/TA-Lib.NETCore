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
    /// <summary>
    /// Chaikin A/D Oscillator (Volume Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inVolume">A span of input volume data.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInFastPeriod">The time period for calculating the fast moving average.</param>
    /// <param name="optInSlowPeriod">The time period for calculating the slow moving average.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Chaikin A/D Oscillator measures the momentum of the Accumulation/Distribution line by comparing shorter and
    /// longer-term money flow intervals.
    /// <para>
    /// The function can highlight underlying changes in buying or selling pressure before such shifts become apparent in price data.
    /// It can confirm trends identified by price action and other technical indicators or serve as
    /// a leading indicator for potential breakouts.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Compute the A/D Line for the given period using the formula:
    ///       <code>
    ///         A/D Line = ((Close - Low) - (High - Close)) / (High - Low) * Volume
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the fast EMA of the A/D Line using the specified fast period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the slow EMA of the A/D Line using the specified slow period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the Chaikin A/D Oscillator as the difference between the fast and slow EMAs:
    ///       <code>
    ///         AdOsc = EMA(Fast, A/D Line) - EMA(Slow, A/D Line)
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A rising value indicates an increasing momentum in buying pressure, suggesting potential upward price movement.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A falling value indicates increasing selling pressure, suggesting potential downward price movement.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Divergences between the AdOsc and the price trend can signal potential reversals or trend continuations.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode AdOsc<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInFastPeriod = 3,
        int optInSlowPeriod = 10) where T : IFloatingPointIeee754<T> =>
        AdOscImpl(inHigh, inLow, inClose, inVolume, inRange, outReal, out outRange, optInFastPeriod, optInSlowPeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="AdOsc{T}">AdOsc</see>.
    /// </summary>
    /// <param name="optInFastPeriod">The time period for calculating the fast moving average.</param>
    /// <param name="optInSlowPeriod">The time period for calculating the slow moving average.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AdOscLookback(int optInFastPeriod = 3, int optInSlowPeriod = 10)
    {
        var slowestPeriod = optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod;

        return optInFastPeriod < 2 || optInSlowPeriod < 2 ? -1 : EmaLookback(slowestPeriod);
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AdOsc<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        T[] inVolume,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInFastPeriod = 3,
        int optInSlowPeriod = 10) where T : IFloatingPointIeee754<T> =>
        AdOscImpl<T>(inHigh, inLow, inClose, inVolume, inRange, outReal, out outRange, optInFastPeriod, optInSlowPeriod);

    private static Core.RetCode AdOscImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInFastPeriod,
        int optInSlowPeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length, inVolume.Length) is not
            { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInFastPeriod < 2 || optInSlowPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /* The fastEMA variable is not necessarily the fastest EMA.
         * In the same way, slowEMA is not necessarily the slowest EMA.
         *
         * The AdOsc is always the (fastEMA - slowEMA) regardless of the period specified. In other word:
         *   ADOSC(3, 10) = EMA(3, AD) - EMA(10, AD)
         * while
         *   ADOSC(10, 3) = EMA(10, AD)- EMA(3, AD)
         *
         * In the first case the EMA(3) is truly a faster EMA, while in the second case,
         * the EMA(10) is still call fastEMA in the algorithm, even if it is in fact slower.
         *
         * This allows for more flexibility, enabling experimentation with unusual parameter settings.
         */

        var lookbackTotal = AdOscLookback(optInFastPeriod, optInSlowPeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;
        var today = startIdx - lookbackTotal;

        var fastK = FunctionHelpers.Two<T>() / (T.CreateChecked(optInFastPeriod) + T.One);
        var oneMinusFastK = T.One - fastK;

        var slowK = FunctionHelpers.Two<T>() / (T.CreateChecked(optInSlowPeriod) + T.One);
        var oneMinusSlowK = T.One - slowK;

        // Use the same range of initialization inputs for both EMA and simply seed with the first A/D value.
        var ad = T.Zero;
        ad = FunctionHelpers.CalcAccumulationDistribution(inHigh, inLow, inClose, inVolume, ref today, ad);

        var fastEMA = ad;
        var slowEMA = ad;

        // Initialize the EMA and skip the unstable period.
        while (today < startIdx)
        {
            ad = FunctionHelpers.CalcAccumulationDistribution(inHigh, inLow, inClose, inVolume, ref today, ad);
            fastEMA = fastK * ad + oneMinusFastK * fastEMA;
            slowEMA = slowK * ad + oneMinusSlowK * slowEMA;
        }

        // Perform the calculation for the requested range
        var outIdx = 0;
        while (today <= endIdx)
        {
            ad = FunctionHelpers.CalcAccumulationDistribution(inHigh, inLow, inClose, inVolume, ref today, ad);
            fastEMA = fastK * ad + oneMinusFastK * fastEMA;
            slowEMA = slowK * ad + oneMinusSlowK * slowEMA;

            outReal[outIdx++] = fastEMA - slowEMA;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
