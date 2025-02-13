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

public static partial class Candles
{
    /// <summary>
    /// Up/Down-gap side-by-side white lines (Pattern Recognition)
    /// </summary>
    /// <param name="inOpen">A span of input open prices.</param>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outIntType">A span to store the output pattern type for each price bar.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Gap Side-by-Side White Lines function used to identify candlestick patterns that is typically observed after a significant
    /// price gap, often signaling a continuation of the current trend. Two consecutive white candles follow the initial gap,
    /// aligning in such a way that their real bodies and openings appear nearly identical. This formation can appear in both
    /// bullish (up gap) and bearish (down gap) contexts but usually highlights strong market momentum.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Detect a price gap between the first candle and the next two candles. The gap may be upward (indicating bullish continuation)
    ///       or downward (indicating bearish continuation).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Verify that the second and third candles both have white real bodies, reflecting consistent upward movement within
    ///       this gap scenario.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm that the second and third candles have real bodies of similar size, defined as <em>near</em> by
    ///       <see cref="Core.CandleSettingType.Near">Near</see> in <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Check that the opening prices of the second and third candles are nearly <em>equal</em>, as specified by
    ///       <see cref="Core.CandleSettingType.Equal">Equal</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Ensure that the gap established by the first candle remains unfilled by the second, indicating that the trend is still intact.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 indicates Upside Gap Side-By-Side White Lines pattern, signaling bullish momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 indicates Downside Gap Side-By-Side White Lines pattern, signaling bearish momentum.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of 0 indicates that no pattern was detected.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode GapSideBySideWhiteLines<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        GapSideBySideWhiteLinesImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="GapSideBySideWhiteLines{T}">GapSideBySideWhiteLines</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int GapSideBySideWhiteLinesLookback() =>
        Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near),
            CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Equal)) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode GapSideBySideWhiteLines<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        GapSideBySideWhiteLinesImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode GapSideBySideWhiteLinesImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        var lookbackTotal = GapSideBySideWhiteLinesLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var nearPeriodTotal = T.Zero;
        var equalPeriodTotal = T.Zero;
        var nearTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near);
        var equalTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Equal);
        var i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            if (IsGapSideBySideWhiteLinesPattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal, equalPeriodTotal))
            {
                outIntType[outIdx++] = CandleHelpers.RealBodyGapUp(inOpen, inClose, i - 1, i - 2) ? 100 : -100;
            }
            else
            {
                outIntType[outIdx++] = 0;
            }

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            nearPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);

            equalPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);

            i++;
            nearTrailingIdx++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsGapSideBySideWhiteLinesPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T nearPeriodTotal,
        T equalPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // upside or downside gap between the 1st candle and both the next 2 candles
        (
            CandleHelpers.RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && CandleHelpers.RealBodyGapUp(inOpen, inClose, i, i - 2)
            ||
            CandleHelpers.RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && CandleHelpers.RealBodyGapDown(inOpen, inClose, i, i - 2)
        )
        &&
        // 2nd: white
        CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        // 3rd: white
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        // same size 2 and 3
        CandleHelpers.RealBody(inClose, inOpen, i) >= CandleHelpers.RealBody(inClose, inOpen, i - 1) -
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) &&
        CandleHelpers.RealBody(inClose, inOpen, i) <= CandleHelpers.RealBody(inClose, inOpen, i - 1) +
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) &&
        // same open 2 and 3
        inOpen[i] >= inOpen[i - 1] -
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) &&
        inOpen[i] <= inOpen[i - 1] +
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1);
}
