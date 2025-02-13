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
    /// Aroon Oscillator (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
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
    /// Aroon Oscillator is a momentum indicator that measures the difference between the Aroon Up and Aroon Down indicators
    /// to assess the strength and direction of a trend.
    /// <para>
    /// The function is useful for identifying trend strength and direction and can also signal potential reversals
    /// when crossing above or below the zero line. It is commonly used in conjunction with other technical indicators
    /// to improve the reliability of trading signals.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Identify the highest high and lowest low within the specified time period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate Aroon Up and Aroon Down:
    /// <code>
    /// Aroon Up = 100 * (Time Period - Days Since Highest High) / Time Period
    /// Aroon Down = 100 * (Time Period - Days Since Lowest Low) / Time Period
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the Aroon Oscillator as the difference between Aroon Up and Aroon Down:
    ///       <code>
    ///         Aroon Oscillator = Aroon Up - Aroon Down
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Positive values indicate that the Aroon Up is stronger than the Aroon Down, suggesting an upward trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Negative values indicate that the Aroon Down is stronger than the Aroon Up, suggesting a downward trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The closer the value is to +100 or -100, the stronger the respective trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Values around 0 suggest a lack of clear trend or a sideways market.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode AroonOsc<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AroonOscImpl(inHigh, inLow, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="AroonOsc{T}">AroonOsc</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AroonOscLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AroonOsc<T>(
        T[] inHigh,
        T[] inLow,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AroonOscImpl<T>(inHigh, inLow, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode AroonOscImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /* This code is almost identical to the Aroon function except that
         * instead of outputting AroonUp and AroonDown individually, an oscillator is build from both.
         *
         *   AroonOsc = AroonUp - AroonDown
         *
         */

        // This function is using a speed optimized algorithm for the min/max logic.
        //It might be needed to first look at how Min/Max works and this function will become easier to understand.

        var lookbackTotal = AroonOscLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Proceed with the calculation for the requested range.
        // The algorithm allows the input and output to be the same buffer.
        var outIdx = 0;
        var today = startIdx;
        var trailingIdx = startIdx - lookbackTotal;

        int highestIdx = -1, lowestIdx = -1;
        T highest = T.Zero, lowest = T.Zero;
        var factor = FunctionHelpers.Hundred<T>() / T.CreateChecked(optInTimePeriod);
        while (today <= endIdx)
        {
            (lowestIdx, lowest) = FunctionHelpers.CalcLowest(inLow, trailingIdx, today, lowestIdx, lowest);
            (highestIdx, highest) = FunctionHelpers.CalcHighest(inHigh, trailingIdx, today, highestIdx, highest);

            /* The oscillator is the following:
             *   AroonUp   = factor * (optInTimePeriod - (today - highestIdx))
             *   AroonDown = factor * (optInTimePeriod - (today - lowestIdx))
             *   AroonOsc  = AroonUp - AroonDown
             *
             * An arithmetic simplification gives:
             *   Aroon = factor * (highestIdx - lowestIdx)
             */
            var arron = factor * T.CreateChecked(highestIdx - lowestIdx);

            //Input and output buffer can be the same, so writing to the output is the last thing being done here.
            outReal[outIdx++] = arron;

            trailingIdx++;
            today++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
