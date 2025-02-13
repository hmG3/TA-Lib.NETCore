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
    /// Average Directional Movement Index Rating (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
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
    /// Average Directional Movement Index Rating is a smoothed version of the <see cref="Adx{T}">ADX</see> indicator,
    /// used to measure the strength of a trend. It is calculated as the average of the current ADX value and the ADX value from a
    /// previous period. The ADXR further smooths the ADX, making it less volatile and easier to interpret.
    /// <para>
    /// The function is commonly used in trend-following strategies to confirm trend strength and filter out periods of low volatility.
    /// It is less reactive to short-term price fluctuations, making it particularly useful for identifying longer-term trends.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the ADX values over the specified time period. Refer to the ADX function for details on the calculation.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       For each ADX value, average it with the ADX value from a previous period (offset by the time period):
    ///       <code>
    ///         ADXR = (Current ADX + ADX from Previous Period) / 2
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Values above 25 indicate a strong trend, while values below 20 suggest a weak or non-existent trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A rising value suggests a strengthening trend, while a falling value indicates a weakening trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Like ADX, the ADXR does not indicate the direction of the trend but measures its strength.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Adxr<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AdxrImpl(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Adxr{T}">Adxr</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AdxrLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + AdxLookback(optInTimePeriod) - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Adxr<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AdxrImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode AdxrImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length, inClose.Length) is not
            { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = AdxrLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        Span<T> adx = new T[endIdx - startIdx + optInTimePeriod];

        var retCode = AdxImpl(inHigh, inLow, inClose, new Range(startIdx - (optInTimePeriod - 1), endIdx), adx, out outRange,
            optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var i = optInTimePeriod - 1;
        var j = 0;
        var outIdx = 0;
        var nbElement = endIdx - startIdx + 2;
        while (--nbElement != 0)
        {
            outReal[outIdx++] = (adx[i++] + adx[j++]) / FunctionHelpers.Two<T>();
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
