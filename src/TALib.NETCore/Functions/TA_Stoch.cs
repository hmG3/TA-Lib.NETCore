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
    /// Stochastic (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outSlowK">A span to store the calculated %K line values.</param>
    /// <param name="outSlowD">A span to store the calculated %D line values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInFastKPeriod">The time period for calculating the Fast %K line.</param>
    /// <param name="optInSlowKPeriod">The time period for smoothing the Fast %K line into the Slow %K line.</param>
    /// <param name="optInSlowKMAType">The moving average type used for smoothing the Fast %K line.</param>
    /// <param name="optInSlowDPeriod">The time period for calculating the Slow %D line.</param>
    /// <param name="optInSlowDMAType">The moving average type used for smoothing the Slow %D line.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Stochastic Oscillator is a momentum indicator used in technical analysis to determine the relative position of the
    /// closing price within a defined range over a given number of periods. It helps identify overbought/oversold conditions
    /// and potential reversals.
    /// <para>
    /// The indicator consists of two lines:
    /// <c>%K</c>, the raw Stochastic, and <c>%D</c>, the signal line derived from %K via smoothing. This function implements
    /// the "slow" version of the Stochastic Oscillator, which applies additional smoothing to %K before calculating %D.
    /// <paramref name="optInFastKPeriod"/> parameter controls the period for calculating the raw %K line.
    /// <paramref name="optInSlowKPeriod"/> and <paramref name="optInSlowKMAType"/> parameters control the smoothing of the %K line.
    /// <paramref name="optInSlowDPeriod"/> and <paramref name="optInSlowDMAType"/> parameters control the smoothing of the %D line.
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
    ///       Smooth %K over the Slow %K period using the specified moving average type to produce the Slow %K line.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Smooth the Slow %K line over the Slow %D period using the specified moving average type to produce the Slow %D line.
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
    ///       A divergence between the Stochastic Oscillator and the price may indicate weakening momentum and a potential trend reversal.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Stoch<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outSlowK,
        Span<T> outSlowD,
        out Range outRange,
        int optInFastKPeriod = 5,
        int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3,
        Core.MAType optInSlowDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochImpl(inHigh, inLow, inClose, inRange, outSlowK, outSlowD, out outRange, optInFastKPeriod, optInSlowKPeriod, optInSlowKMAType,
            optInSlowDPeriod, optInSlowDMAType);

    /// <summary>
    /// Returns the lookback period for <see cref="Stoch{T}">Stoch</see>.
    /// </summary>
    /// <param name="optInFastKPeriod">The time period for calculating the Fast %K line.</param>
    /// <param name="optInSlowKPeriod">The time period for smoothing the Fast %K line into the Slow %K line.</param>
    /// <param name="optInSlowKMAType">The moving average type used for smoothing the Fast %K line.</param>
    /// <param name="optInSlowDPeriod">The time period for calculating the Slow %D line.</param>
    /// <param name="optInSlowDMAType">The moving average type used for smoothing the Slow %D line.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int StochLookback(
        int optInFastKPeriod = 5,
        int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3,
        Core.MAType optInSlowDMAType = Core.MAType.Sma)
    {
        if (optInFastKPeriod < 1 || optInSlowKPeriod < 1 || optInSlowDPeriod < 1)
        {
            return -1;
        }

        var retValue = optInFastKPeriod - 1;
        retValue += MaLookback(optInSlowKPeriod, optInSlowKMAType);
        retValue += MaLookback(optInSlowDPeriod, optInSlowDMAType);

        return retValue;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Stoch<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outSlowK,
        T[] outSlowD,
        out Range outRange,
        int optInFastKPeriod = 5,
        int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3,
        Core.MAType optInSlowDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochImpl<T>(inHigh, inLow, inClose, inRange, outSlowK, outSlowD, out outRange, optInFastKPeriod, optInSlowKPeriod,
            optInSlowKMAType, optInSlowDPeriod, optInSlowDMAType);

    private static Core.RetCode StochImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outSlowK,
        Span<T> outSlowD,
        out Range outRange,
        int optInFastKPeriod,
        int optInSlowKPeriod,
        Core.MAType optInSlowKMAType,
        int optInSlowDPeriod,
        Core.MAType optInSlowDMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInFastKPeriod < 1 || optInSlowKPeriod < 1 || optInSlowDPeriod < 1)
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
        var lookbackDSlow = MaLookback(optInSlowDPeriod, optInSlowDMAType);
        var lookbackTotal = StochLookback(optInFastKPeriod, optInSlowKPeriod, optInSlowKMAType, optInSlowDPeriod, optInSlowDMAType);
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
        // (The range of k must consider all the lookback involve with the smoothing).
        var trailingIdx = startIdx - lookbackTotal;
        var today = trailingIdx + lookbackK;

        // Allocate a temporary buffer large enough to store the K.
        // If the output is the same as the input, just save one memory allocation.
        Span<T> tempBuffer;
        if (outSlowK == inHigh || outSlowK == inLow || outSlowK == inClose)
        {
            tempBuffer = outSlowK;
        }
        else if (outSlowD == inHigh || outSlowD == inLow || outSlowD == inClose)
        {
            tempBuffer = outSlowD;
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

        /* Un-smoothed K calculation completed. This K calculation is not returned to the caller.
         * It is always smoothed and then return.
         * Some documentation will refer to the smoothed version as being "K-Slow", but often this end up to be shortened to "K".
         */
        var retCode = MaImpl(tempBuffer, Range.EndAt(outIdx - 1), tempBuffer, out outRange, optInSlowKPeriod, optInSlowKMAType);
        if (retCode != Core.RetCode.Success || outRange.End.Value == 0)
        {
            return retCode;
        }

        var nbElement = outRange.End.Value - outRange.Start.Value;
        // Calculate the %D which is simply a moving average of the already smoothed %K.
        retCode = MaImpl(tempBuffer, Range.EndAt(nbElement - 1), outSlowD, out outRange, optInSlowDPeriod, optInSlowDMAType);
        nbElement = outRange.End.Value - outRange.Start.Value;

        /* Copy tempBuffer into the caller buffer.
         * (Calculation could not be done directly in the caller buffer because
         * more input data than the requested range was needed for doing %D).
         */
        tempBuffer.Slice(lookbackDSlow, nbElement).CopyTo(outSlowK);
        if (retCode != Core.RetCode.Success)
        {
            outRange = Range.EndAt(0);

            return retCode;
        }

        outRange = new Range(startIdx, startIdx + nbElement);

        return Core.RetCode.Success;
    }
}
