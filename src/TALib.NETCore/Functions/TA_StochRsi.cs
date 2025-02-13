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
    /// Stochastic Relative Strength Index (Momentum Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outFastK">A span to store the calculated Fast %K line values.</param>
    /// <param name="outFastD">A span to store the calculated Fast %D line values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
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
    /// Stochastic Relative Strength Index is a momentum indicator that combines the Stochastic Oscillator (<see cref="Stoch{T}">Stoch</see>)
    /// and the Relative Strength Index (<see cref="Rsi{T}">RSI</see>) to determine overbought and oversold conditions in an asset.
    /// Unlike the standard RSI, StochRSI provides more sensitivity and finer detail by applying the Stochastic Oscillator
    /// formula to the RSI values instead of raw price data.
    /// <para>
    /// The StochRSI is particularly useful in identifying overbought or oversold conditions with
    /// higher sensitivity than traditional momentum indicators.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the RSI values over the specified <paramref name="optInTimePeriod"/>:
    ///       <code>
    ///         RSI = 100 - (100 / (1 + RS))
    ///       </code>
    ///       where RS is the ratio of average gains to average losses over the <paramref name="optInTimePeriod"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the Fast %K line using the Stochastic Oscillator formula applied to the RSI values:
    ///       <code>
    ///         %K = 100 * ((RSI - LowestRSI) / (HighestRSI - LowestRSI))
    ///       </code>
    ///       where:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             <b>LowestRSI</b> is the lowest RSI value over the <paramref name="optInFastKPeriod"/>.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             <b>HighestRSI</b> is the highest RSI value over the <paramref name="optInFastKPeriod"/>.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Smooth the Fast %K line over the <paramref name="optInFastDPeriod"/> using the specified
    ///       <paramref name="optInFastDMAType"/> to produce the Fast %D line.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Values near 0 indicate that the RSI is at its lowest levels in the lookback period, suggesting oversold conditions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Values near 100 indicate that the RSI is at its highest levels in the lookback period, suggesting overbought conditions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Crossovers of the %K and %D lines are used as potential buy or sell signals:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             A %K line crossing above the %D line may signal a buy opportunity.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             A %K line crossing below the %D line may signal a sell opportunity.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       StochRSI provides higher sensitivity than RSI but may result in more false signals due to its volatility.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode StochRsi<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outFastK,
        Span<T> outFastD,
        out Range outRange,
        int optInTimePeriod = 14,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochRsiImpl(inReal, inRange, outFastK, outFastD, out outRange, optInTimePeriod, optInFastKPeriod, optInFastDPeriod,
            optInFastDMAType);

    /// <summary>
    /// Returns the lookback period for <see cref="StochRsi{T}">StochRsi</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <param name="optInFastKPeriod">The time period for calculating the Fast %K line.</param>
    /// <param name="optInFastDPeriod">The time period for smoothing the Fast %K line.</param>
    /// <param name="optInFastDMAType">The moving average type used for smoothing the Fast %D line.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int StochRsiLookback(
        int optInTimePeriod = 14,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) =>
        optInTimePeriod < 2 || optInFastKPeriod < 1 || optInFastDPeriod < 1
            ? -1
            : RsiLookback(optInTimePeriod) + StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode StochRsi<T>(
        T[] inReal,
        Range inRange,
        T[] outFastK,
        T[] outFastD,
        out Range outRange,
        int optInTimePeriod = 14,
        int optInFastKPeriod = 5,
        int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        StochRsiImpl<T>(inReal, inRange, outFastK, outFastD, out outRange, optInTimePeriod, optInFastKPeriod, optInFastDPeriod,
            optInFastDMAType);

    private static Core.RetCode StochRsiImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outFastK,
        Span<T> outFastD,
        out Range outRange,
        int optInTimePeriod,
        int optInFastKPeriod,
        int optInFastDPeriod,
        Core.MAType optInFastDMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2 || optInFastKPeriod < 1 || optInFastDPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* Reference: "Stochastic RSI and Dynamic Momentum Index"
         *            by Tushar Chande and Stanley Kroll
         *            Stock&Commodities V.11:5 (189-199)
         *
         * The version offer flexibility beyond what is explained in the Stock&Commodities article.
         *
         * To calculate the "Unsmoothed stochastic RSI" with symmetry like explain in the article,
         * keep the optInTimePeriod and optInFastKPeriod equal. Example:
         *
         *   un-smoothed stoch RSI 14 : optInTimePeriod   = 14
         *                              optInFastK_Period = 14
         *                              optInFastD_Period = 'x'
         *
         * The outFastK is the un-smoothed RSI discuss in the article.
         *
         * optInFastDPeriod can be set to smooth the RSI. The smooth* version will be found in outFastD.
         * The outFastK will still contain the un-smoothed stoch RSI.
         * If the smoothing of the StochRSI is not a concern, optInFastDPeriod should be left at 1, and outFastD can be ignored.
         */

        var lookbackStochF = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        var lookbackTotal = StochRsiLookback(optInTimePeriod, optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;
        var tempArraySize = endIdx - startIdx + 1 + lookbackStochF;
        Span<T> tempRsiBuffer = new T[tempArraySize];
        var retCode = RsiImpl(inReal, new Range(startIdx - lookbackStochF, endIdx), tempRsiBuffer, out var outRange1, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outRange1.End.Value == 0)
        {
            return retCode;
        }

        retCode = StochFImpl(tempRsiBuffer, tempRsiBuffer, tempRsiBuffer, Range.EndAt(tempArraySize - 1), outFastK, outFastD, out outRange,
            optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outRange.End.Value == 0)
        {
            return retCode;
        }

        outRange = new Range(outBegIdx, outBegIdx + outRange.End.Value - outRange.Start.Value);

        return Core.RetCode.Success;
    }
}
