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
    /// Moving Average Convergence/Divergence Fix 12/26 (Momentum Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outMACD">A span to store the calculated MACD line values.</param>
    /// <param name="outMACDSignal">A span to store the calculated Signal line values.</param>
    /// <param name="outMACDHist">A span to store the calculated MACD Histogram values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInSignalPeriod">The time period for calculating the Signal line.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Moving Average Convergence Divergence Fix is a simplified version of the MACD indicator
    /// with fixed fast and slow periods (12 and 26, respectively). This version focuses on the Signal line
    /// calculation with a configurable period for flexibility in identifying momentum shifts.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Compute the fast exponential moving average (EMA) of the input values with a period of 12.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the slow EMA of the input values with a period of 26.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the MACD line as the difference between the fast EMA and the slow EMA:
    ///       <code>
    ///         MACD = EMA(FastPeriod) - EMA(SlowPeriod)
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the Signal line as the EMA of the MACD line using the specified `optInSignalPeriod`.
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
    public static Core.RetCode MacdFix<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out Range outRange,
        int optInSignalPeriod = 9) where T : IFloatingPointIeee754<T> =>
        MacdFixImpl(inReal, inRange, outMACD, outMACDSignal, outMACDHist, out outRange, optInSignalPeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="MacdFix{T}">MacdFix</see>.
    /// </summary>
    /// <param name="optInSignalPeriod">The time period for calculating the Signal line.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MacdFixLookback(int optInSignalPeriod = 9) => EmaLookback(26) + EmaLookback(optInSignalPeriod);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MacdFix<T>(
        T[] inReal,
        Range inRange,
        T[] outMACD,
        T[] outMACDSignal,
        T[] outMACDHist,
        out Range outRange,
        int optInSignalPeriod = 9) where T : IFloatingPointIeee754<T> =>
        MacdFixImpl<T>(inReal, inRange, outMACD, outMACDSignal, outMACDHist, out outRange, optInSignalPeriod);

    private static Core.RetCode MacdFixImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out Range outRange,
        int optInSignalPeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        if (optInSignalPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        return FunctionHelpers.CalcMACD(inReal, new Range(rangeIndices.startIndex, rangeIndices.endIndex), outMACD, outMACDSignal,
            outMACDHist, out outRange,
            0, /* 0 indicate fix 12 == 0.15  for optInFastPeriod */
            0, /* 0 indicate fix 26 == 0.075 for optInSlowPeriod */
            optInSignalPeriod);
    }
}
