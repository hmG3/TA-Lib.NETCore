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
    /// Bollinger Bands (Overlap Studies)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outRealUpperBand">A span to store the calculated upper band values.</param>
    /// <param name="outRealMiddleBand">A span to store the calculated middle band values.</param>
    /// <param name="outRealLowerBand">A span to store the calculated lower band values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <param name="optInNbDevUp">
    /// Multiplier for the standard deviation to calculate the upper band:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values increase the distance from the middle band, reducing sensitivity to minor price fluctuations.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values reduce the distance, increasing responsiveness to price changes.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Values above 5 are rarely used as they lose practical significance.
    /// </para>
    /// </param>
    /// <param name="optInNbDevDn">
    /// Multiplier for the standard deviation to calculate the lower band:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values increase the distance from the middle band, reducing the likelihood of oversold signals.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values reduce the distance, increasing sensitivity to price declines.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Values above 5 are rarely used as they lose practical significance.
    /// </para>
    /// </param>
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
    /// Bollinger Bands are a volatility-based indicator that uses a moving average and standard deviations
    /// to form upper and lower "bands" around the price. These bands expand and contract with market volatility,
    /// providing insights into potential overbought or oversold conditions, as well as periods of consolidation and breakout.
    /// <para>
    /// The function is often used in trading strategies for identifying breakout opportunities, trend continuation, or reversals.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the middle band as a moving average of the input values over the specified time period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the standard deviation of the input values over the same time period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Determine the upper and lower bands by adding/subtracting a multiple of the standard deviation to/from the middle band:
    /// <code>
    /// Upper Band = Middle Band + (Standard Deviation * NbDevUp)
    /// Lower Band = Middle Band - (Standard Deviation * NbDevDn)
    /// </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value Interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       When prices move close to the upper band, the market may be approaching overbought levels, potentially signaling a retracement.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       When prices move close to the lower band, the market may be approaching oversold levels, potentially signaling a rally.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       During periods of low volatility, the bands contract, indicating potential breakouts. During high volatility, the bands expand.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Bbands<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outRealUpperBand,
        Span<T> outRealMiddleBand,
        Span<T> outRealLowerBand,
        out Range outRange,
        int optInTimePeriod = 5,
        double optInNbDevUp = 2.0,
        double optInNbDevDn = 2.0,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        BbandsImpl(inReal, inRange, outRealUpperBand, outRealMiddleBand, outRealLowerBand, out outRange, optInTimePeriod, optInNbDevUp,
            optInNbDevDn, optInMAType);

    /// <summary>
    /// Returns the lookback period for <see cref="Bbands{T}">Bbands</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <param name="optInMAType">The moving average type.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int BbandsLookback(int optInTimePeriod = 5, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInTimePeriod < 2 ? -1 : MaLookback(optInTimePeriod, optInMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Bbands<T>(
        T[] inReal,
        Range inRange,
        T[] outRealUpperBand,
        T[] outRealMiddleBand,
        T[] outRealLowerBand,
        out Range outRange,
        int optInTimePeriod = 5,
        double optInNbDevUp = 2.0,
        double optInNbDevDn = 2.0,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        BbandsImpl<T>(inReal, inRange, outRealUpperBand, outRealMiddleBand, outRealLowerBand, out outRange, optInTimePeriod, optInNbDevUp,
            optInNbDevDn, optInMAType);

    private static Core.RetCode BbandsImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outRealUpperBand,
        Span<T> outRealMiddleBand,
        Span<T> outRealLowerBand,
        out Range outRange,
        int optInTimePeriod,
        double optInNbDevUp,
        double optInNbDevDn,
        Core.MAType optInMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (_, endIdx) = rangeIndices;

        if (optInTimePeriod < 2 || optInNbDevUp < 0 || optInNbDevDn < 0)
        {
            return Core.RetCode.BadParam;
        }

        /* Identify two temporary buffers among the outputs.
         * These temporary buffers allows to perform the calculation without any memory allocation.
         * Whenever possible, make the tempBuffer1 be the middle band output. This will save one copy operation.
         */

        Span<T> tempBuffer1 = outRealMiddleBand;
        Span<T> tempBuffer2;

        if (inReal == outRealUpperBand)
        {
            tempBuffer2 = outRealLowerBand;
        }
        else
        {
            tempBuffer2 = outRealUpperBand;

            if (inReal == outRealMiddleBand)
            {
                tempBuffer1 = outRealLowerBand;
            }
        }

        // Check that the caller is not doing tricky things (like using the input buffer in two output)
        if (tempBuffer1 == inReal || tempBuffer2 == inReal)
        {
            return Core.RetCode.BadParam;
        }

        // Calculate the middle band, which is a moving average.
        // The other two bands will simply add/subtract the standard deviation from this middle band.
        var retCode = MaImpl(inReal, inRange, tempBuffer1, out outRange, optInTimePeriod, optInMAType);
        if (retCode != Core.RetCode.Success || outRange.End.Value == 0)
        {
            return retCode;
        }

        var nbElement = outRange.End.Value - outRange.Start.Value;
        if (optInMAType == Core.MAType.Sma)
        {
            // A small speed optimization by re-using the already calculated SMA.
            CalcStandardDeviation(inReal, tempBuffer1, outRange, tempBuffer2, optInTimePeriod);
        }
        else
        {
            // Calculate the Standard Deviation
            retCode = StdDevImpl(inReal, new Range(outRange.Start.Value, endIdx), tempBuffer2, out outRange, optInTimePeriod, 1.0);
            if (retCode != Core.RetCode.Success)
            {
                outRange = Range.EndAt(0);

                return retCode;
            }
        }

        // Copy the MA calculation into the middle band output, unless the calculation was done into it already
        if (tempBuffer1 != outRealMiddleBand)
        {
            tempBuffer1[..nbElement].CopyTo(outRealMiddleBand);
        }

        var nbDevUp = T.CreateChecked(optInNbDevUp);
        var nbDevDn = T.CreateChecked(optInNbDevDn);

        /* Do a tight loop to calculate the upper/lower band at the same time.
         *
         * All the following 5 loops are doing the same,
         * except there is an attempt to speed optimize by eliminating unneeded multiplication.
         */
        if (optInNbDevUp.Equals(optInNbDevDn))
        {
            CalcEqualBands(tempBuffer2, outRealMiddleBand, outRealUpperBand, outRealLowerBand, nbElement, nbDevUp);
        }
        else
        {
            CalcDistinctBands(tempBuffer2, outRealMiddleBand, outRealUpperBand, outRealLowerBand, nbElement, nbDevUp, nbDevDn);
        }

        return Core.RetCode.Success;
    }

    private static void CalcStandardDeviation<T>(
        ReadOnlySpan<T> real,
        ReadOnlySpan<T> movAvg,
        Range movAvgRange,
        Span<T> outReal,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        var startSum = movAvgRange.Start.Value + 1 - optInTimePeriod;
        var endSum = movAvgRange.Start.Value;
        var periodTotal2 = T.Zero;
        for (var outIdx = startSum; outIdx < endSum; outIdx++)
        {
            var tempReal = real[outIdx];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);
        for (var outIdx = 0; outIdx < movAvgRange.End.Value - movAvgRange.Start.Value; outIdx++, startSum++, endSum++)
        {
            var tempReal = real[endSum];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            var meanValue2 = periodTotal2 / timePeriod;

            tempReal = real[startSum];
            tempReal *= tempReal;
            periodTotal2 -= tempReal;

            tempReal = movAvg[outIdx];
            tempReal *= tempReal;
            meanValue2 -= tempReal;

            outReal[outIdx] = meanValue2 > T.Zero ? T.Sqrt(meanValue2) : T.Zero;
        }
    }

    private static void CalcEqualBands<T>(
        ReadOnlySpan<T> tempBuffer,
        ReadOnlySpan<T> realMiddleBand,
        Span<T> realUpperBand,
        Span<T> realLowerBand,
        int nbElement,
        T nbDevUp) where T : IFloatingPointIeee754<T>
    {
        if (nbDevUp.Equals(T.One))
        {
            // No standard deviation multiplier needed.
            for (var i = 0; i < nbElement; i++)
            {
                var tempReal = tempBuffer[i];
                var tempReal2 = realMiddleBand[i];
                realUpperBand[i] = tempReal2 + tempReal;
                realLowerBand[i] = tempReal2 - tempReal;
            }
        }
        else
        {
            // Upper/lower band use the same standard deviation multiplier.
            for (var i = 0; i < nbElement; i++)
            {
                var tempReal = tempBuffer[i] * nbDevUp;
                var tempReal2 = realMiddleBand[i];
                realUpperBand[i] = tempReal2 + tempReal;
                realLowerBand[i] = tempReal2 - tempReal;
            }
        }
    }

    private static void CalcDistinctBands<T>(
        ReadOnlySpan<T> tempBuffer,
        ReadOnlySpan<T> realMiddleBand,
        Span<T> realUpperBand,
        Span<T> realLowerBand,
        int nbElement,
        T nbDevUp,
        T nbDevDn) where T : IFloatingPointIeee754<T>
    {
        if (nbDevUp.Equals(T.One))
        {
            // Only lower band has a standard deviation multiplier.
            for (var i = 0; i < nbElement; i++)
            {
                var tempReal = tempBuffer[i];
                var tempReal2 = realMiddleBand[i];
                realUpperBand[i] = tempReal2 + tempReal;
                realLowerBand[i] = tempReal2 - tempReal * nbDevDn;
            }
        }
        else if (nbDevDn.Equals(T.One))
        {
            // Only upper band has a standard deviation multiplier.
            for (var i = 0; i < nbElement; i++)
            {
                var tempReal = tempBuffer[i];
                var tempReal2 = realMiddleBand[i];
                realLowerBand[i] = tempReal2 - tempReal;
                realUpperBand[i] = tempReal2 + tempReal * nbDevUp;
            }
        }
        else
        {
            // Upper/lower band have distinctive standard deviation multiplier.
            for (var i = 0; i < nbElement; i++)
            {
                var tempReal = tempBuffer[i];
                var tempReal2 = realMiddleBand[i];
                realUpperBand[i] = tempReal2 + tempReal * nbDevUp;
                realLowerBand[i] = tempReal2 - tempReal * nbDevDn;
            }
        }
    }
}
