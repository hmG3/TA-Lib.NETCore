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
    /// Mat Hold (Pattern Recognition)
    /// </summary>
    /// <param name="inOpen">A span of input open prices.</param>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outIntType">A span to store the output pattern type for each price bar.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInPenetration">
    /// Specifies the penetration factor for the reaction days within the first candle's real body:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A higher value allows the reaction days to penetrate deeper into the first candle's real body,
    ///       relaxing the validation for the pattern.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A lower value restricts the reaction days to stay closer to the first candle's upper body, enforcing stricter validation.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Typical range: <c>0.2..0.6</c>. Valid values are between <c>0.0</c> (no penetration) and <c>1.0</c> (full penetration).
    /// </para>
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
    /// Mat Hold function identifies a continuation formation observed during an established uptrend. This pattern is characterized
    /// by a strong bullish candle, followed by a short period of consolidation with smaller candles remaining largely within the first
    /// candle's real body, and culminating in another robust bullish candle that reaffirms upward momentum.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Verify that the first candle is white and <em>long</em>, exceeding the average defined by
    ///       <see cref="Core.CandleSettingType.BodyLong">BodyLong</see> in <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm the second candle is black (bearish) and establishes an upside gap versus the first candle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Check the third and fourth candles are relatively <em>short</em> (black or white), determined by
    ///       <see cref="Core.CandleSettingType.BodyShort">BodyShort</see>. Their real bodies must lie within the real body of
    ///       the first candle. The depth of penetration into the first
    ///       candle's real body must remain under the specified <paramref>optInPenetration</paramref> factor.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Verify that the fifth candle is white, opens above the fourth candle's close, and surpasses the highest high
    ///       of the intervening (reaction) candles.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 indicates the presence of a Mat Hold pattern, suggesting a continuation of the bullish trend.
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
    public static Core.RetCode MatHold<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange,
        double optInPenetration = 0.5) where T : IFloatingPointIeee754<T> =>
        MatHoldImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    /// <summary>
    /// Returns the lookback period for <see cref="MatHold{T}">MatHold</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MatHoldLookback() =>
        Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort),
            CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowLong)) + 4;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MatHold<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange,
        double optInPenetration = 0.5) where T : IFloatingPointIeee754<T> =>
        MatHoldImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    private static Core.RetCode MatHoldImpl<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange,
        double optInPenetration) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inOpen.Length, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInPenetration < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MatHoldLookback();
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
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        var penetration = T.CreateChecked(optInPenetration);
        do
        {
            outIntType[outIdx++] = IsMatHoldPattern(inOpen, inHigh, inLow, inClose, i, bodyPeriodTotal, penetration) ? 100 : 0;

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

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsMatHoldPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> bodyPeriodTotal,
        T penetration) where T : IFloatingPointIeee754<T> =>
        // 1st long, then 3 small
        CandleHelpers.RealBody(inClose, inOpen, i - 4) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyPeriodTotal[4], i - 4) &&
        CandleHelpers.RealBody(inClose, inOpen, i - 3) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[3], i - 3) &&
        CandleHelpers.RealBody(inClose, inOpen, i - 2) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[2], i - 2) &&
        CandleHelpers.RealBody(inClose, inOpen, i - 1) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal[1], i - 1) &&
        // white, black, 2 black or white, white
        CandleHelpers.CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.White &&
        CandleHelpers.CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        // upside gap 1st to 2nd
        CandleHelpers.RealBodyGapUp(inOpen, inClose, i - 3, i - 4) &&
        // 3rd to 4th hold within 1st: a part of the real body must be within 1st real body
        T.Min(inOpen[i - 2], inClose[i - 2]) < inClose[i - 4] &&
        T.Min(inOpen[i - 1], inClose[i - 1]) < inClose[i - 4] &&
        // reaction days penetrate first body less than optInPenetration percent
        T.Min(inOpen[i - 2], inClose[i - 2]) > inClose[i - 4] - CandleHelpers.RealBody(inClose, inOpen, i - 4) * penetration &&
        T.Min(inOpen[i - 1], inClose[i - 1]) > inClose[i - 4] - CandleHelpers.RealBody(inClose, inOpen, i - 4) * penetration &&
        // 2nd to 4th are falling
        T.Max(inClose[i - 2], inOpen[i - 2]) < inOpen[i - 3] &&
        T.Max(inClose[i - 1], inOpen[i - 1]) < T.Max(inClose[i - 2], inOpen[i - 2]) &&
        // 5th opens above the prior close
        inOpen[i] > inClose[i - 1] &&
        // 5th closes above the highest high of the reaction days
        inClose[i] > T.Max(T.Max(inHigh[i - 3], inHigh[i - 2]), inHigh[i - 1]);
}
