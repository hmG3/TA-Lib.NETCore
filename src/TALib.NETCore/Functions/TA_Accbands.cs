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
    /// Acceleration Bands (Overlap Studies)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outRealUpperBand">A span to store the calculated upper band values.</param>
    /// <param name="outRealMiddleBand">A span to store the calculated middle band values.</param>
    /// <param name="outRealLowerBand">A span to store the calculated lower band values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Acceleration Bands is a type of volatility indicator used to identify potential breakout points and measure trend strength.
    /// These bands adjust dynamically based on price movement.
    /// <para>
    /// The function can assist in recognizing volatility changes and breakout opportunities.
    /// It is often combined with momentum or trend-following indicators such as <see cref="Adx{T}">ADX</see> or
    /// <see cref="Rsi{T}">RSI</see> to confirm signals and enhance decision-making.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the upper and lower band values for each period:
    /// <code>
    /// Upper Band = High * (1 + 4 * (High - Low) / (High + Low)).
    /// Lower Band = Low * (1 - 4 * (High - Low) / (High + Low)).
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute a moving average of the closing prices for the middle band:
    ///       <code>
    ///         Middle Band = Simple Moving Average (Close, Period).
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply a Simple Moving Average (SMA) to smooth the upper and lower band values.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Price breaking above the upper band may indicate strong upward momentum or a potential breakout.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Price breaking below the lower band may indicate strong downward momentum or a potential breakout.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The middle band provides a baseline, often used to confirm trends or as a reversion point.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Accbands<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outRealUpperBand,
        Span<T> outRealMiddleBand,
        Span<T> outRealLowerBand,
        out Range outRange,
        int optInTimePeriod = 20) where T : IFloatingPointIeee754<T> =>
        AccbandsImpl(inHigh, inLow, inClose, inRange, outRealUpperBand, outRealMiddleBand, outRealLowerBand, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Accbands{T}">Accbands</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AccbandsLookback(int optInTimePeriod = 20) => optInTimePeriod < 2 ? -1 : SmaLookback(optInTimePeriod);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Accbands<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outRealUpperBand,
        T[] outRealMiddleBand,
        T[] outRealLowerBand,
        out Range outRange,
        int optInTimePeriod = 20) where T : IFloatingPointIeee754<T> =>
        AccbandsImpl<T>(inHigh, inLow, inClose, inRange, outRealUpperBand, outRealMiddleBand, outRealLowerBand, out outRange,
            optInTimePeriod);

    private static Core.RetCode AccbandsImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outRealUpperBand,
        Span<T> outRealMiddleBand,
        Span<T> outRealLowerBand,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = AccbandsLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Buffer will contain also the lookback required for SMA to satisfy the caller requested startIdx/endIdx.
        var outputSize = endIdx - startIdx + 1;
        var bufferSize = outputSize + lookbackTotal;
        Span<T> tempBuffer1 = new T[bufferSize];
        Span<T> tempBuffer2 = new T[bufferSize];

        // Calculate the upper/lower band at the same time (no SMA yet).
        // Must start calculation back enough to cover the lookback required later for the SMA.
        for (int j = 0, i = startIdx - lookbackTotal; i <= endIdx; i++, j++)
        {
            var tempReal = inHigh[i] + inLow[i];
            if (!T.IsZero(tempReal))
            {
                tempReal = FunctionHelpers.Four<T>() * (inHigh[i] - inLow[i]) / tempReal;
                tempBuffer1[j] = inHigh[i] * (T.One + tempReal);
                tempBuffer2[j] = inLow[i] * (T.One - tempReal);
            }
            else
            {
                tempBuffer1[j] = inHigh[i];
                tempBuffer2[j] = inLow[i];
            }
        }

        // Calculate the middle band, which is a moving average of the close.
        var retCode = SmaImpl(inClose, new Range(startIdx, endIdx), outRealMiddleBand, out var dummyRange, optInTimePeriod);
        if (retCode != Core.RetCode.Success || dummyRange.End.Value - dummyRange.Start.Value != outputSize)
        {
            return retCode;
        }

        // Take the SMA for the upper band.
        retCode = SmaImpl(tempBuffer1, Range.EndAt(bufferSize - 1), outRealUpperBand, out dummyRange, optInTimePeriod);
        if (retCode != Core.RetCode.Success || dummyRange.End.Value - dummyRange.Start.Value != outputSize)
        {
            return retCode;
        }

        // Take the SMA for the lower band.
        retCode = SmaImpl(tempBuffer2, Range.EndAt(bufferSize - 1), outRealLowerBand, out dummyRange, optInTimePeriod);
        if (retCode != Core.RetCode.Success || dummyRange.End.Value - dummyRange.Start.Value != outputSize)
        {
            return retCode;
        }

        outRange = new Range(startIdx, startIdx + outputSize);

        return Core.RetCode.Success;
    }
}
