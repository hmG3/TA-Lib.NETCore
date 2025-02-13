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
    /// Aroon (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outAroonDown">A span to store the calculated Aroon Down values.</param>
    /// <param name="outAroonUp">A span to store the calculated Aroon Up values.</param>
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
    /// The Aroon indicator is a momentum-based indicator that measures the time between highs and lows
    /// over a specified time period to identify the strength and direction of a trend. It consists of two components:
    /// Aroon Up and Aroon Down, which are calculated separately.
    /// <para>
    /// The function is particularly useful for identifying trend reversals and assessing trend strength.
    /// It can also be used in conjunction with other technical indicators to confirm trading signals or filter out noise.
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
    ///       Calculate Aroon Up:
    ///       <code>
    ///         Aroon Up = 100 * (Time Period - Days Since Highest High) / Time Period
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate Aroon Down:
    ///       <code>
    ///         Aroon Down = 100 * (Time Period - Days Since Lowest Low) / Time Period
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Values close to 100 indicate a strong upward trend, while values near 0 indicate no recent highs.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Values close to 100 indicate a strong downward trend, while values near 0 indicate no recent lows.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       When Aroon Up crosses above Aroon Down, it may signal the beginning of an upward trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       When Aroon Down crosses above Aroon Up, it may signal the beginning of a downward trend.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Aroon<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outAroonDown,
        Span<T> outAroonUp,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AroonImpl(inHigh, inLow, inRange, outAroonDown, outAroonUp, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Aroon{T}">Aroon</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AroonLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Aroon<T>(
        T[] inHigh,
        T[] inLow,
        Range inRange,
        T[] outAroonDown,
        T[] outAroonUp,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AroonImpl<T>(inHigh, inLow, inRange, outAroonDown, outAroonUp, out outRange, optInTimePeriod);

    private static Core.RetCode AroonImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outAroonDown,
        Span<T> outAroonUp,
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

        // This function is using a speed optimized algorithm for the min/max logic.
        // It might be needed to first look at how Min/Max works and this function will become easier to understand.

        var lookbackTotal = AroonLookback(optInTimePeriod);
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

            outAroonUp[outIdx] = factor * T.CreateChecked(optInTimePeriod - (today - highestIdx));
            outAroonDown[outIdx] = factor * T.CreateChecked(optInTimePeriod - (today - lowestIdx));

            outIdx++;
            trailingIdx++;
            today++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
