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
    /// MACD with controllable MA type (Momentum Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outMACD">A span to store the calculated MACD line values.</param>
    /// <param name="outMACDSignal">A span to store the calculated Signal line values.</param>
    /// <param name="outMACDHist">A span to store the calculated MACD Histogram values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInFastPeriod">The time period for calculating the fast moving average.</param>
    /// <param name="optInFastMAType">The moving average type used for the fast moving average.</param>
    /// <param name="optInSlowPeriod">The time period for calculating the slow moving average.</param>
    /// <param name="optInSlowMAType">The moving average type used for the slow moving average.</param>
    /// <param name="optInSignalPeriod">The time period for calculating the Signal line.</param>
    /// <param name="optInSignalMAType">The moving average type used for the Signal line.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Extended Moving Average Convergence Divergence allows customization of moving average types
    /// for calculating the MACD line, Signal line, and MACD Histogram. This flexibility allows the indicator to be adapted
    /// to various market conditions and analysis strategies.
    /// <para>
    /// The choice of moving average types (e.g., SMA, EMA) for the fast, slow, and Signal lines affects the indicator's sensitivity.
    /// The function can be adapted to suit certain assets or volatility conditions. Combining it with volume indicators
    /// may strengthen momentum-based signals.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Compute the slow moving average using the specified <paramref name="optInSlowPeriod"/> and <paramref name="optInSlowMAType"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the fast moving average using the specified <paramref name="optInFastPeriod"/> and <paramref name="optInFastMAType"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the MACD line as the difference between the fast and slow moving averages:
    ///       <code>
    ///         MACD = FastMA - SlowMA
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the Signal line as the moving average of the MACD line using the specified <paramref name="optInSignalPeriod"/>
    ///       and <paramref name="optInSignalMAType"/>..
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the MACD Histogram as the difference between the MACD line and the Signal line:
    ///       <code>
    ///         MACDHist = MACD - Signal
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A positive MACD line indicates upward momentum, while a negative MACD line indicates downward momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The Signal line is used to identify potential buy or sell signals, with a bullish crossover occurring when the MACD line
    ///       crosses above the Signal line, and a bearish crossover occurring when the MACD line crosses below the Signal line.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The MACD Histogram reflects the strength of momentum, with larger bars indicating stronger momentum in the direction
    ///       of the MACD line and shrinking bars signaling a potential reversal or weakening momentum.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode MacdExt<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out Range outRange,
        int optInFastPeriod = 12,
        Core.MAType optInFastMAType = Core.MAType.Sma,
        int optInSlowPeriod = 26,
        Core.MAType optInSlowMAType = Core.MAType.Sma,
        int optInSignalPeriod = 9,
        Core.MAType optInSignalMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MacdExtImpl(inReal, inRange, outMACD, outMACDSignal, outMACDHist, out outRange, optInFastPeriod, optInFastMAType, optInSlowPeriod,
            optInSlowMAType, optInSignalPeriod, optInSignalMAType);

    /// <summary>
    /// Returns the lookback period for <see cref="MacdExt{T}">MacdExt</see>.
    /// </summary>
    /// <param name="optInFastPeriod">The time period for calculating the fast moving average.</param>
    /// <param name="optInFastMAType">The moving average type used for the fast moving average.</param>
    /// <param name="optInSlowPeriod">The time period for calculating the slow moving average.</param>
    /// <param name="optInSlowMAType">The moving average type used for the slow moving average.</param>
    /// <param name="optInSignalPeriod">The time period for calculating the Signal line.</param>
    /// <param name="optInSignalMAType">The moving average type used for the Signal line.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MacdExtLookback(
        int optInFastPeriod = 12,
        Core.MAType optInFastMAType = Core.MAType.Sma,
        int optInSlowPeriod = 26,
        Core.MAType optInSlowMAType = Core.MAType.Sma,
        int optInSignalPeriod = 9,
        Core.MAType optInSignalMAType = Core.MAType.Sma)
    {
        if (optInFastPeriod < 2 || optInSlowPeriod < 2 || optInSignalPeriod < 1)
        {
            return -1;
        }

        var lookbackLargest = MaLookback(optInFastPeriod, optInFastMAType);
        var tempInteger = MaLookback(optInSlowPeriod, optInSlowMAType);
        if (tempInteger > lookbackLargest)
        {
            lookbackLargest = tempInteger;
        }

        return lookbackLargest + MaLookback(optInSignalPeriod, optInSignalMAType);
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MacdExt<T>(
        T[] inReal,
        Range inRange,
        T[] outMACD,
        T[] outMACDSignal,
        T[] outMACDHist,
        out Range outRange,
        int optInFastPeriod = 12,
        Core.MAType optInFastMAType = Core.MAType.Sma,
        int optInSlowPeriod = 26,
        Core.MAType optInSlowMAType = Core.MAType.Sma,
        int optInSignalPeriod = 9,
        Core.MAType optInSignalMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MacdExtImpl<T>(inReal, inRange, outMACD, outMACDSignal, outMACDHist, out outRange, optInFastPeriod, optInFastMAType,
            optInSlowPeriod, optInSlowMAType, optInSignalPeriod, optInSignalMAType);

    private static Core.RetCode MacdExtImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out Range outRange,
        int optInFastPeriod,
        Core.MAType optInFastMAType,
        int optInSlowPeriod,
        Core.MAType optInSlowMAType,
        int optInSignalPeriod,
        Core.MAType optInSignalMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInFastPeriod < 2 || optInSlowPeriod < 2 || optInSignalPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        // Make sure slow is really slower than the fast period. if not, swap.
        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            (optInSlowMAType, optInFastMAType) = (optInFastMAType, optInSlowMAType);
        }

        // Add the lookback needed for the signal line
        var lookbackSignal = MaLookback(optInSignalPeriod, optInSignalMAType);
        var lookbackTotal = MacdExtLookback(optInFastPeriod, optInFastMAType, optInSlowPeriod, optInSlowMAType, optInSignalPeriod,
            optInSignalMAType);

        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Allocate intermediate buffer for fast/slow MA.
        var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
        Span<T> fastMABuffer = new T[tempInteger];
        Span<T> slowMABuffer = new T[tempInteger];

        /* Calculate the slow MA.
         *
         * Move back the startIdx to get enough data for the signal period.
         * That way, once the signal calculation is done, all the output will start at the requested 'startIdx'.
         */
        tempInteger = startIdx - lookbackSignal;
        var retCode = MaImpl(inReal, new Range(tempInteger, endIdx), slowMABuffer, out var outRange1, optInSlowPeriod, optInSlowMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Calculate the fast MA.
        retCode = MaImpl(inReal, new Range(tempInteger, endIdx), fastMABuffer, out _, optInFastPeriod, optInFastMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var nbElement1 = outRange1.End.Value - outRange1.Start.Value;
        // Calculate (fast MA) - (slow MA).
        for (var i = 0; i < nbElement1; i++)
        {
            fastMABuffer[i] -= slowMABuffer[i];
        }

        // Copy the result into the output for the caller.
        fastMABuffer.Slice(lookbackSignal, endIdx - startIdx + 1).CopyTo(outMACD);

        // Calculate the signal/trigger line.
        retCode = MaImpl(fastMABuffer, Range.EndAt(nbElement1 - 1), outMACDSignal, out var outRange2, optInSignalPeriod, optInSignalMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var nbElement2 = outRange2.End.Value - outRange2.Start.Value;
        // Calculate the histogram.
        for (var i = 0; i < nbElement2; i++)
        {
            outMACDHist[i] = outMACD[i] - outMACDSignal[i];
        }

        outRange = new Range(startIdx, startIdx + nbElement2);

        return Core.RetCode.Success;
    }
}
