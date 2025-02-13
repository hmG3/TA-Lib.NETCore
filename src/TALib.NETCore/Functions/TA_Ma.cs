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
    /// Moving Average (Overlap Studies)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
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
    /// Moving Average function calculates the average of a series of data points over a specified period,
    /// smoothing out fluctuations to highlight trends. Various types of moving averages are supported, allowing for
    /// different smoothing techniques to cater to different analytical needs.
    ///
    /// <para>
    /// <b>Supported moving average types</b> via <paramref name="optInMAType"/>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       <see cref="Sma{T}">SMA</see> (Simple Moving Average): A straightforward average of values over the specified period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="Ema{T}">EMA</see> (Exponential Moving Average): Assigns more weight to recent data,
    ///       making it more responsive to changes.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="Wma{T}">WMA</see> (Weighted Moving Average): Weights data points linearly,
    ///       with the most recent data having the highest weight.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="Dema{T}">DEMA</see> (Double Exponential Moving Average): A composite average designed to reduce lag.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="Tema{T}">TEMA</see> (Triple Exponential Moving Average): Further reduces lag while smoothing data.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="Trima{T}">TRIMA</see> (Triangular Moving Average): A double-smoothing technique.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="Kama{T}">KAMA</see> (Kaufman Adaptive Moving Average): Adjusts responsiveness based on market volatility.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="Mama{T}">MAMA</see> (MESA Adaptive Moving Average): Adapts to market cycles.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="T3{T}">T3</see> (T3 Moving Average): A smooth moving average with reduced lag.
    ///     </description>
    ///   </item>
    /// </list>
    /// </para>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A rising moving average indicates an uptrend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A falling moving average suggests a downtrend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Crossovers of different moving averages (e.g., short-term and long-term) can indicate buy or sell signals.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Ma<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MaImpl(inReal, inRange, outReal, out outRange, optInTimePeriod, optInMAType);

    /// <summary>
    /// Returns the lookback period for <see cref="Ma{T}">Ma</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <param name="optInMAType">The moving average type.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MaLookback(int optInTimePeriod = 30, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInTimePeriod switch
        {
            < 1 => -1,
            1 => 0,
            _ => optInMAType switch
            {
                Core.MAType.Sma => SmaLookback(optInTimePeriod),
                Core.MAType.Ema => EmaLookback(optInTimePeriod),
                Core.MAType.Wma => WmaLookback(optInTimePeriod),
                Core.MAType.Dema => DemaLookback(optInTimePeriod),
                Core.MAType.Tema => TemaLookback(optInTimePeriod),
                Core.MAType.Trima => TrimaLookback(optInTimePeriod),
                Core.MAType.Kama => KamaLookback(optInTimePeriod),
                Core.MAType.Mama => MamaLookback(),
                Core.MAType.T3 => T3Lookback(optInTimePeriod),
                _ => 0
            }
        };

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Ma<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MaImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod, optInMAType);

    private static Core.RetCode MaImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod,
        Core.MAType optInMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        if (optInTimePeriod == 1)
        {
            var nbElement = endIdx - startIdx + 1;
            for (int todayIdx = startIdx, outIdx = 0; outIdx < nbElement; outIdx++, todayIdx++)
            {
                outReal[outIdx] = inReal[todayIdx];
            }

            outRange = new Range(startIdx, startIdx + nbElement);

            return Core.RetCode.Success;
        }

        // Simply forward the job to the corresponding function.
        switch (optInMAType)
        {
            case Core.MAType.Sma:
                return Sma(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Ema:
                return Ema(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Wma:
                return Wma(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Dema:
                return Dema(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Tema:
                return Tema(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Trima:
                return Trima(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Kama:
                return Kama(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Mama:
                Span<T> dummyBuffer = new T[endIdx - startIdx + 1];
                return Mama(inReal, inRange, outReal, dummyBuffer, out outRange);
            case Core.MAType.T3:
                return T3(inReal, inRange, outReal, out outRange, optInTimePeriod);
            default:
                return Core.RetCode.BadParam;
        }
    }
}
