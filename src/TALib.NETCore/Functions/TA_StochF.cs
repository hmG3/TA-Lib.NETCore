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
    /// Stochastic Fast (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outFastK">A span to store the calculated %K line values.</param>
    /// <param name="outFastD">A span to store the calculated %D line values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInFastKPeriod">The time period for calculating the Fast %K line.</param>
    /// <param name="optInFastDPeriod">The time period for smoothing the Fast %K line.</param>
    /// <param name="optInFastDMAType">The moving average type used for smoothing the Fast %D line.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Fast Stochastic Oscillator is a momentum indicator that measures the relative position of the closing price
    /// within a defined range over a given number of periods. It helps identify overbought or oversold conditions
    /// and potential trend reversals. Unlike the standard Stochastic Oscillator, this implementation directly calculates
    /// the raw %K line and applies minimal smoothing to generate the %D line.
    /// <para>
    /// Stochastic Fast is more sensitive to price movements than the standard <see cref="Stoch{T}">Stoch</see>,
    /// making it more prone to false signals.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the raw %K value for each period:
    ///       <code>
    ///         %K = 100 * ((Close - LowestLow) / (HighestHigh - LowestLow))
    ///       </code>
    ///       where:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             <b>Close</b> is the closing price of the current period.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             <b>LowestLow</b> is the lowest low over the Fast %K period.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             <b>HighestHigh</b> is the highest high over the Fast %K period.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Smooth the %K line over the Fast %D period using the specified moving average type to produce the %D line.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Values above 80 typically indicate overbought conditions, suggesting a potential reversal to the downside.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Values below 20 typically indicate oversold conditions, suggesting a potential reversal to the upside.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Crossovers between the %K and %D lines are often used as trading signals:
    ///       - A %K line crossing above %D may signal a buy.
    ///       - A %K line crossing below %D may signal a sell.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The Fast Stochastic Oscillator is more sensitive to price changes than the Slow Stochastic Oscillator,
    ///       making it more prone to false signals in volatile markets.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode StochF<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outFastK,
        Span<T> outFastD,
        out Range outRange,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochFImpl(inHigh, inLow, inClose, inRange, outFastK, outFastD, out outRange, optInFastKPeriod, optInFastDPeriod, optInFastDMAType);

    /// <summary>
    /// Returns the lookback period for <see cref="StochF{T}">StochF</see>.
    /// </summary>
    /// <param name="optInFastKPeriod">The time period for calculating the Fast %K line.</param>
    /// <param name="optInFastDPeriod">The time period for smoothing the Fast %K line.</param>
    /// <param name="optInFastDMAType">The moving average type used for smoothing the Fast %D line.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int StochFLookback(int optInFastKPeriod = 5, int optInFastDPeriod = 3, Core.MAType optInFastDMAType = Core.MAType.Sma) =>
        optInFastKPeriod < 1 || optInFastDPeriod < 1 ? -1 : optInFastKPeriod - 1 + MaLookback(optInFastDPeriod, optInFastDMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode StochF<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outFastK,
        T[] outFastD,
        out Range outRange,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochFImpl<T>(inHigh, inLow, inClose, inRange, outFastK, outFastD, out outRange, optInFastKPeriod, optInFastDPeriod,
            optInFastDMAType);

    private static Core.RetCode StochFImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outFastK,
        Span<T> outFastD,
        out Range outRange,
        int optInFastKPeriod,
        int optInFastDPeriod,
        Core.MAType optInFastDMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInFastKPeriod < 1 || optInFastDPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* With stochastic, there is a total of 4 different lines that are defined: FastK, FastD, SlowK and SlowD.
         *
         * The D is the signal line usually drawn over its corresponding K function.
         *
         *                     (Today's Close - LowestLow)
         *   FastK(Kperiod) =  ─────────────────────────── * 100
         *                      (HighestHigh - LowestLow)
         *
         *   FastD(FastDperiod, MA type) = MA Smoothed FastK over FastDperiod
         *
         *   SlowK(SlowKperiod, MA type) = MA Smoothed FastK over SlowKperiod
         *
         *   SlowD(SlowDperiod, MA Type) = MA Smoothed SlowK over SlowDperiod
         *
         * The HighestHigh and LowestLow are the extreme values among the last 'Kperiod'.
         *
         * SlowK and FastD are equivalent when using the same period.
         *
         * The following shows how these four lines are made available in the library:
         *
         *   Stoch  : Returns the SlowK and SlowD
         *   StochF : Returns the FastK and FastD
         *
         * The Stoch function correspond to the more widely implemented version found in much software/charting package.
         * The StochF is more rarely used because its higher volatility cause often whipsaws.
         */

        var lookbackK = optInFastKPeriod - 1;
        var lookbackFastD = MaLookback(optInFastDPeriod, optInFastDMAType);
        var lookbackTotal = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        /* Do the K calculation:
         *
         *   Kt = 100 * ((Ct - Lt) / (Ht - Lt))
         *
         * Kt is today stochastic
         * Ct is today closing price.
         * Lt is the lowest price of the last K Period (including today)
         * Ht is the highest price of the last K Period (including today)
         */

        // Proceed with the calculation for the requested range.
        // The algorithm allows the input and output to be the same buffer.
        var outIdx = 0;

        // Calculate just enough K for ending up with the caller requested range.
        // (The range of k must consider allthe lookback involve with the smoothing).
        var trailingIdx = startIdx - lookbackTotal;
        var today = trailingIdx + lookbackK;

        // Allocate a temporary buffer large enough to store the K.
        // If the output is the same as the input, just save one memory allocation.
        Span<T> tempBuffer;
        if (outFastK == inHigh || outFastK == inLow || outFastK == inClose)
        {
            tempBuffer = outFastK;
        }
        else if (outFastD == inHigh || outFastD == inLow || outFastD == inClose)
        {
            tempBuffer = outFastD;
        }
        else
        {
            tempBuffer = new T[endIdx - today + 1];
        }

        // Do the K calculation
        int highestIdx = -1, lowestIdx = -1;
        T highest = T.Zero, lowest = T.Zero;
        while (today <= endIdx)
        {
            (lowestIdx, lowest) = FunctionHelpers.CalcLowest(inLow, trailingIdx, today, lowestIdx, lowest);
            (highestIdx, highest) = FunctionHelpers.CalcHighest(inHigh, trailingIdx, today, highestIdx, highest);

            var diff = (highest - lowest) / FunctionHelpers.Hundred<T>();

            // Calculate stochastic.
            tempBuffer[outIdx++] = !T.IsZero(diff) ? (inClose[today] - lowest) / diff : T.Zero;

            trailingIdx++;
            today++;
        }

        // Fast-K calculation completed. This K calculation is returned to the caller. It is smoothed to become Fast-D.
        var retCode = MaImpl(tempBuffer, Range.EndAt(outIdx - 1), outFastD, out outRange, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outRange.End.Value == 0)
        {
            return retCode;
        }

        var nbElement = outRange.End.Value - outRange.Start.Value;

        /* Copy tempBuffer into the caller buffer.
         * (Calculation could not be done directly in the caller buffer because
         * more input data than the requested range was needed for doing %D).
         */
        tempBuffer.Slice(lookbackFastD, nbElement).CopyTo(outFastK);
        outRange = new Range(startIdx, startIdx + nbElement);

        return Core.RetCode.Success;
    }
}
