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
    /// Percentage Price Oscillator (Momentum Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInFastPeriod">The time period for calculating the fast moving average.</param>
    /// <param name="optInSlowPeriod">The time period for calculating the slow moving average.</param>
    /// <param name="optInMAType">The moving average type.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Percentage Price Oscillator function calculates the percentage difference between two moving averages
    /// of a time series. It is commonly used to identify trends and assess the strength of price movements.
    /// <para>
    /// The function is similar to <see cref="Macd{T}">MACD</see> but expressed as a percentage,
    /// facilitating easier comparisons across instruments. The function aids in normalized momentum analysis.
    /// Combining it with <see cref="Rsi{T}">RSI</see>, <see cref="Bbands{T}">Bollinger Bands</see>,
    /// or volume indicators can improve the robustness of momentum-based signals.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the fast and slow moving averages (MA) using the specified
    ///       <paramref name="optInFastPeriod"/> and <paramref name="optInSlowPeriod"/>:
    /// <code>
    /// FastMA = MA(data, optInFastPeriod, optInMAType)
    /// SlowMA = MA(data, optInSlowPeriod, optInMAType)
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the PPO as the percentage difference between the fast and slow moving averages:
    ///       <code>
    ///         PPO = ((FastMA - SlowMA) / SlowMA) * 100
    ///       </code>
    ///       where <c>FastMA</c> and <c>SlowMA</c> are the moving averages calculated in the previous step.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Positive values indicate that the fast moving average is above the slow moving average, signaling upward momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Negative values indicate that the fast moving average is below the slow moving average, signaling downward momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Higher absolute values suggest stronger momentum in the corresponding direction.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Ppo<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInFastPeriod = 12,
        int optInSlowPeriod = 26,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        PpoImpl(inReal, inRange, outReal, out outRange, optInFastPeriod, optInSlowPeriod, optInMAType);

    /// <summary>
    /// Returns the lookback period for <see cref="Ppo{T}">Ppo</see>.
    /// </summary>
    /// <param name="optInFastPeriod">The time period for calculating the fast moving average.</param>
    /// <param name="optInSlowPeriod">The time period for calculating the slow moving average.</param>
    /// <param name="optInMAType">The moving average type.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int PpoLookback(int optInFastPeriod = 12, int optInSlowPeriod = 26, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInFastPeriod < 2 || optInSlowPeriod < 2 ? -1 : MaLookback(Math.Max(optInSlowPeriod, optInFastPeriod), optInMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Ppo<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInFastPeriod = 12,
        int optInSlowPeriod = 26,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        PpoImpl<T>(inReal, inRange, outReal, out outRange, optInFastPeriod, optInSlowPeriod, optInMAType);

    private static Core.RetCode PpoImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInFastPeriod,
        int optInSlowPeriod,
        Core.MAType optInMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInFastPeriod < 2 || optInSlowPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        Span<T> tempBuffer = new T[endIdx - startIdx + 1];

        return FunctionHelpers.CalcPriceOscillator(inReal, new Range(startIdx, endIdx), outReal, out outRange, optInFastPeriod,
            optInSlowPeriod, optInMAType, tempBuffer, true);
    }
}
