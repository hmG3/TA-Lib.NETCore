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
    /// Beta (Statistic Functions)
    /// </summary>
    /// <param name="inReal0">A span of input security or stock prices.</param>
    /// <param name="inReal1">A span of input benchmark or market prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
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
    /// Beta function calculates the beta coefficient, which measures the volatility of a security relative to a benchmark index.
    /// <para>
    /// The function can be used in risk management and portfolio selection.
    /// Combining it with other risk measures or normalization techniques may optimize instrument choices.
    /// </para>
    ///
    /// <b>Calculation formula</b>:
    /// <code>
    ///   Beta = Covariance(Security, Market) / Variance(Market)
    /// </code>
    /// where the covariance and variance are derived from the percentage changes in the security's and market's prices
    /// over a specified time period.
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 1 indicates that the security's price moves with the market.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value greater than 1 suggests that the security is more volatile than the market.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value less than 1 indicates that the security is less volatile than the market.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Beta<T>(
        ReadOnlySpan<T> inReal0,
        ReadOnlySpan<T> inReal1,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 5) where T : IFloatingPointIeee754<T> =>
        BetaImpl(inReal0, inReal1, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Beta{T}">Beta</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int BetaLookback(int optInTimePeriod = 5) => optInTimePeriod < 1 ? -1 : optInTimePeriod;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Beta<T>(
        T[] inReal0,
        T[] inReal1,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 5) where T : IFloatingPointIeee754<T> =>
        BetaImpl<T>(inReal0, inReal1, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode BetaImpl<T>(
        ReadOnlySpan<T> inReal0,
        ReadOnlySpan<T> inReal1,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal0.Length, inReal1.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* The Beta 'algorithm' is a measure of a stocks volatility vs from index. The stock prices are given in inReal0 and
         * the index prices are give in inReal1. The size of these vectors should be equal.
         * The algorithm is to calculate the change between prices in both vectors and then 'plot' these changes
         * are points in the Euclidean plane. The x value of the point is market return and the y value is the security return.
         * The beta value is the slope of a linear regression through these points. A beta of 1 is simple the line y=x,
         * so the stock varies precisely with the market. A beta of less than one means the stock varies less than
         * the market and a beta of more than one means the stock varies more than market.
         */

        var lookbackTotal = BetaLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T sxy, sx, sy;
        var sxx = sxy = sx = sy = T.Zero;
        var trailingIdx = startIdx - lookbackTotal;
        var trailingLastPriceX = inReal0[trailingIdx]; // same as lastPriceX except used to remove elements from the trailing summation
        var lastPriceX = trailingLastPriceX; // the last price read from inReal0
        var trailingLastPriceY = inReal1[trailingIdx]; // same as lastPriceY except used to remove elements from the trailing summation
        var lastPriceY = trailingLastPriceY; /* the last price read from inReal1 */

        var i = ++trailingIdx;
        while (i < startIdx)
        {
            UpdateSummation(inReal0, inReal1, ref lastPriceX, ref lastPriceY, ref i, ref sxx, ref sxy, ref sx, ref sy);
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        var outIdx = 0;
        do
        {
            UpdateSummation(inReal0, inReal1, ref lastPriceX, ref lastPriceY, ref i, ref sxx, ref sxy, ref sx, ref sy);

            UpdateTrailingSummation(inReal0, inReal1, ref trailingLastPriceX, ref trailingLastPriceY, ref trailingIdx, ref sxx, ref sxy,
                ref sx, ref sy, timePeriod, outReal, ref outIdx);
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static void UpdateSummation<T>(
        ReadOnlySpan<T> real0,
        ReadOnlySpan<T> real1,
        ref T lastPriceX,
        ref T lastPriceY,
        ref int idx,
        ref T sxx,
        ref T sxy,
        ref T sx,
        ref T sy) where T : IFloatingPointIeee754<T>
    {
        var tmpReal = real0[idx];
        var x = !T.IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : T.Zero;
        lastPriceX = tmpReal;

        tmpReal = real1[idx++];
        var y = !T.IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : T.Zero;
        lastPriceY = tmpReal;

        sxx += x * x;
        sxy += x * y;
        sx += x;
        sy += y;
    }

    private static void UpdateTrailingSummation<T>(
        ReadOnlySpan<T> real0,
        ReadOnlySpan<T> real1,
        ref T trailingLastPriceX,
        ref T trailingLastPriceY,
        ref int trailingIdx,
        ref T sxx,
        ref T sxy,
        ref T sx,
        ref T sy,
        T timePeriod,
        Span<T> outReal,
        ref int outIdx) where T : IFloatingPointIeee754<T>
    {
        // Always read the trailing before writing the output because the input and output buffer can be the same.
        var tmpReal = real0[trailingIdx];
        var x = !T.IsZero(trailingLastPriceX) ? (tmpReal - trailingLastPriceX) / trailingLastPriceX : T.Zero;
        trailingLastPriceX = tmpReal;

        tmpReal = real1[trailingIdx++];
        var y = !T.IsZero(trailingLastPriceY) ? (tmpReal - trailingLastPriceY) / trailingLastPriceY : T.Zero;
        trailingLastPriceY = tmpReal;

        tmpReal = timePeriod * sxx - sx * sx;
        outReal[outIdx++] = !T.IsZero(tmpReal) ? (timePeriod * sxy - sx * sy) / tmpReal : T.Zero;

        // Remove the calculation starting with the trailingIdx.
        sxx -= x * x;
        sxy -= x * y;
        sx -= x;
        sy -= y;
    }
}
