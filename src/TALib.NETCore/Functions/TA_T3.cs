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
    /// T3 Moving Average (Overlap Studies)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <param name="optInVFactor">
    /// The volume factor controls the degree of smoothing and responsiveness:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values (closer to 1) result in smoother outputs with more lag.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values (closer to 0) produce outputs that are more responsive to recent price changes.
    ///     </description>
    ///   </item>
    /// </list>
    /// </param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// T3 Moving Average is a smoothing technique that improves upon traditional exponential moving averages (EMAs)
    /// by reducing lag while maintaining responsiveness to price changes. It achieves this by applying multiple EMAs to the same
    /// input data and using a volume factor (<paramref name="optInVFactor"/>) to adjust the weighting of each EMA layer.
    /// <para>
    /// T3 values represent a smoother moving average with less lag compared to conventional EMAs.
    /// This indicator is especially useful in identifying trends and reducing the impact of short-term volatility.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Compute the first EMA (E1) over the input data (<paramref name="inReal"/>) using the specified
    ///       <paramref name="optInTimePeriod"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply subsequent EMAs on top of the previous result, up to six layers:
    /// <code>
    /// E2 = EMA(E1, optInTimePeriod)
    /// E3 = EMA(E2, optInTimePeriod)
    /// ...
    /// E6 = EMA(E5, optInTimePeriod)
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Use the volume factor (<paramref name="optInVFactor"/>) to combine the weighted contributions of the six EMAs:
    ///       <code>
    ///         T3 = C1 * E6 + C2 * E5 + C3 * E4 + C4 * E3
    ///       </code>
    ///       where:
    ///       <list type="bullet">
    ///         <item><b>C1</b>, <b>C2</b>, <b>C3</b>, and <b>C4</b> are constants derived from the volume factor.</item>
    ///       </list>
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode T3<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 5,
        double optInVFactor = 0.7) where T : IFloatingPointIeee754<T> =>
        T3Impl(inReal, inRange, outReal, out outRange, optInTimePeriod, optInVFactor);

    /// <summary>
    /// Returns the lookback period for <see cref="T3{T}">T3</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int T3Lookback(int optInTimePeriod = 5) =>
        optInTimePeriod < 2 ? -1 : (optInTimePeriod - 1) * 6 + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.T3);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode T3<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 5,
        double optInVFactor = 0.7) where T : IFloatingPointIeee754<T> =>
        T3Impl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod, optInVFactor);

    private static Core.RetCode T3Impl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod,
        double optInVFactor) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2 || optInVFactor < 0.0 || optInVFactor > 1.0)
        {
            return Core.RetCode.BadParam;
        }

        /* An explanation of the function can be found at:
         *
         * Magazine articles written by Tim Tillson
         *
         * Essentially, a T3 of time series "t" is:
         *   EMA1(x, Period) = EMA(x, Period)
         *   EMA2(x, Period) = EMA(EMA1(x, Period), Period)
         *   GD(x, Period, vFactor) = (EMA1(x, Period) * (1 + vFactor)) - (EMA2(x, Period) * vFactor)
         *   T3 = GD(GD(GD(t, Period, vFactor), Period, vFactor), Period, vFactor)
         *
         * T3 offers a moving average with lesser lags than the traditional EMA.
         * T3 should not be confused with EMA3. Both are called "Triple EMA" in the literature.
         */

        var lookbackTotal = T3Lookback(optInTimePeriod);
        if (startIdx <= lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;
        var today = startIdx - lookbackTotal;

        var timePeriod = T.CreateChecked(optInTimePeriod);

        var k = FunctionHelpers.Two<T>() / (timePeriod + T.One);
        var oneMinusK = T.One - k;

        var tempReal = inReal[today++];
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            tempReal += inReal[today++];
        }

        var e1 = tempReal / timePeriod;

        tempReal = e1;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            tempReal += e1;
        }

        var e2 = tempReal / timePeriod;

        tempReal = e2;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            tempReal += e2;
        }

        var e3 = tempReal / timePeriod;

        tempReal = e3;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            tempReal += e3;
        }

        var e4 = tempReal / timePeriod;

        tempReal = e4;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            tempReal += e4;
        }

        var e5 = tempReal / timePeriod;

        tempReal = e5;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            tempReal += e5;
        }

        var e6 = tempReal / timePeriod;

        // Skip the unstable period
        while (today <= startIdx)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            e6 = k * e5 + oneMinusK * e6;
        }

        // Calculate the constants
        var vFactor = T.CreateChecked(optInVFactor);
        tempReal = vFactor * vFactor;
        var c1 = T.NegativeOne * tempReal * vFactor;
        var c2 = FunctionHelpers.Three<T>() * (tempReal - c1);
        var c3 = T.NegativeOne * FunctionHelpers.Two<T>() * FunctionHelpers.Three<T>() * tempReal - FunctionHelpers.Three<T>() * (vFactor - c1);
        var c4 = T.One + FunctionHelpers.Three<T>() * vFactor - c1 + FunctionHelpers.Three<T>() * tempReal;

        // Write the first output
        var outIdx = 0;
        outReal[outIdx++] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;

        // Calculate and output the remaining of the range.
        while (today <= endIdx)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            e6 = k * e5 + oneMinusK * e6;
            outReal[outIdx++] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }
}
