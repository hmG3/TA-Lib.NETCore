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
    /// Rising/Falling Three Methods (Pattern Recognition)
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
    /// Rising/Falling Three Methods function identifies a five-candle continuation pattern, signaling the likely persistence of
    /// the current market trend—either bullish (Rising Three Methods) or bearish (Falling Three Methods).
    ///
    /// <b>Calculation steps</b>:
    ///  <list type="number">
    ///   <item>
    ///     <description>
    ///       Verify that the first candle is <em>long</em> and aligns with the predominant trend direction.
    ///       Its real body must surpass the average defined by <see cref="Core.CandleSettingType.BodyLong">BodyLong</see> in
    ///       <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm the second, third, and fourth candles are <em>short</em>, moving contrary to the primary trend.
    ///       Their real bodies must remain within the range of the first candle, while each body length is below the average
    ///       specified for <see cref="Core.CandleSettingType.BodyShort">BodyShort</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Establish that the fifth candle is once again <em>long</em>, reverting to the same color as the first candle
    ///       and exceeding the average for <see cref="Core.CandleSettingType.BodyLong">BodyLong</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Ensure the fifth candle opens above (in a bullish scenario) or below (in a bearish scenario) the close
    ///       of the preceding candle and closes beyond the close of the first candle.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 represents a Rising Three Methods pattern, signaling bullish continuation.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 represents a Falling Three Methods pattern, signaling bearish continuation.
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
    public static Core.RetCode RisingFallingThreeMethods<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        RisingFallingThreeMethodsImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="RisingFallingThreeMethods{T}">RisingFallingThreeMethods</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int RisingFallingThreeMethodsLookback() =>
        Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort),
            CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 4;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode RisingFallingThreeMethods<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        RisingFallingThreeMethodsImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode RisingFallingThreeMethodsImpl<T>(
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

        var lookbackTotal = RisingFallingThreeMethodsLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> bodyPeriodTotal = new T[5];
        var bodyShortTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        var bodyLongTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[3] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 3);
            bodyPeriodTotal[2] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 2);
            bodyPeriodTotal[1] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[4] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            bodyPeriodTotal[0] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsRisingFallingThreeMethodsPattern(inOpen, inHigh, inLow, inClose, i, bodyPeriodTotal)
                ? (int) CandleHelpers.CandleColor(inClose, inOpen, i - 4) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyPeriodTotal[4] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);

            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                        bodyShortTrailingIdx - totIdx);
            }

            bodyPeriodTotal[0] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsRisingFallingThreeMethodsPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> bodyPeriodTotal) where T : IFloatingPointIeee754<T>
    {
        var fifthCandleColor = T.CreateChecked((int) CandleHelpers.CandleColor(inClose, inOpen, i - 4) * 100);

        return
            // 1st long, then 3 small, 5th long
            CandleHelpers.RealBody(inClose, inOpen, i - 4) >
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyPeriodTotal[4], i - 4) &&
            CandleHelpers.RealBody(inClose, inOpen, i - 3) <
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[3], i - 3) &&
            CandleHelpers.RealBody(inClose, inOpen, i - 2) <
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[2], i - 2) &&
            CandleHelpers.RealBody(inClose, inOpen, i - 1) <
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[1], i - 1) &&
            CandleHelpers.RealBody(inClose, inOpen, i) >
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyPeriodTotal[0], i) &&
            // white, 3 black, white or black, 3 white, black
            (int) CandleHelpers.CandleColor(inClose, inOpen, i - 4) == -(int) CandleHelpers.CandleColor(inClose, inOpen, i - 3) &&
            CandleHelpers.CandleColor(inClose, inOpen, i - 3) == CandleHelpers.CandleColor(inClose, inOpen, i - 2) &&
            CandleHelpers.CandleColor(inClose, inOpen, i - 2) == CandleHelpers.CandleColor(inClose, inOpen, i - 1) &&
            (int) CandleHelpers.CandleColor(inClose, inOpen, i - 1) == -(int) CandleHelpers.CandleColor(inClose, inOpen, i) &&
            // 2nd to 4th hold within 1st: a part of the real body must be within 1st range
            T.Min(inOpen[i - 3], inClose[i - 3]) < inHigh[i - 4] && T.Max(inOpen[i - 3], inClose[i - 3]) > inLow[i - 4] &&
            T.Min(inOpen[i - 2], inClose[i - 2]) < inHigh[i - 4] && T.Max(inOpen[i - 2], inClose[i - 2]) > inLow[i - 4] &&
            T.Min(inOpen[i - 1], inClose[i - 1]) < inHigh[i - 4] && T.Max(inOpen[i - 1], inClose[i - 1]) > inLow[i - 4] &&
            // 2nd to 4th are falling (rising)
            inClose[i - 2] * fifthCandleColor < inClose[i - 3] * fifthCandleColor &&
            inClose[i - 1] * fifthCandleColor < inClose[i - 2] * fifthCandleColor &&
            // 5th opens above (below) the prior close
            inOpen[i] * fifthCandleColor > inClose[i - 1] * fifthCandleColor &&
            // 5th closes above (below) the 1st close
            inClose[i] * fifthCandleColor > inClose[i - 4] * fifthCandleColor;
    }
}
