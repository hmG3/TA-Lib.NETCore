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
    /// Evening Star (Pattern Recognition)
    /// </summary>
    /// <param name="inOpen">A span of input open prices.</param>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outIntType">A span to store the output pattern type for each price bar.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInPenetration">
    /// Specifies the penetration factor for the third candle's closing position within the first candle's real body:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values require the third candle to close deeper into the first candle's real body,
    ///       enforcing stricter validation for the pattern.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values allow for a more shallow penetration, making the pattern more flexible but potentially increasing false positives.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Typical range: <c>0.2..0.5</c>. Valid values are between <c>0.0</c> (no penetration) and <c>1.0</c> (full penetration).
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
    /// Evening Star function identifies a three-candle bearish reversal pattern that typically appears at the top of an uptrend.
    /// It is composed of a strong white (bullish) candle, followed by a short candle that gaps upward, and concludes with a long
    /// black (bearish) candle closing well into the white candle's body. This arrangement signals waning bullish momentum and
    /// potential downward movement.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       The first candle must be long and white, exceeding the <em>long</em> the average length specified by
    ///       <see cref="Core.CandleSettingType.BodyLong">BodyLong</see> in <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The second candle must be <em>short</em> (per <see cref="Core.CandleSettingType.BodyShort">BodyShort</see>), and it must
    ///       gap up relative to the first candle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The third candle must be a black real body, longer than the <em>short</em> threshold, and must close sufficiently deep
    ///       into the first candle's body to meet the <paramref name="optInPenetration"/> requirement.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of -100 signifies the detection of an Evening Star pattern, indicating bearish sentiment.
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
    public static Core.RetCode EveningStar<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange,
        double optInPenetration = 0.3) where T : IFloatingPointIeee754<T> =>
        EveningStarImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    /// <summary>
    /// Returns the lookback period for <see cref="EveningStar{T}">EveningStar</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int EveningStarLookback() =>
        Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort),
            CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode EveningStar<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange,
        double optInPenetration = 0.3) where T : IFloatingPointIeee754<T> =>
        EveningStarImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    private static Core.RetCode EveningStarImpl<T>(
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

        var lookbackTotal = EveningStarLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var bodyLongPeriodTotal = T.Zero;
        var bodyShortPeriodTotal = T.Zero;
        var bodyShortPeriodTotal2 = T.Zero;
        var bodyLongTrailingIdx = startIdx - 2 - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var bodyShortTrailingIdx = startIdx - 1 - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        var i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyShortPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            bodyShortPeriodTotal2 += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i + 1);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsEveningStarPattern(inOpen, inHigh, inLow, inClose, optInPenetration, i, bodyLongPeriodTotal,
                bodyShortPeriodTotal, bodyShortPeriodTotal2)
                ? -100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyLongPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 2) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);

            bodyShortPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 1) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);

            bodyShortPeriodTotal2 +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx + 1);

            i++;
            bodyLongTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsEveningStarPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        double optInPenetration,
        int i,
        T bodyLongPeriodTotal,
        T bodyShortPeriodTotal,
        T bodyShortPeriodTotal2) where T : IFloatingPointIeee754<T> =>
        // 1st: long
        CandleHelpers.RealBody(inClose, inOpen, i - 2) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 2) &&
        // white
        CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
        // 2nd: short
        CandleHelpers.RealBody(inClose, inOpen, i - 1) <=
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortPeriodTotal, i - 1) &&
        // gapping up
        CandleHelpers.RealBodyGapUp(inOpen, inClose, i - 1, i - 2) &&
        // 3rd: longer than short
        CandleHelpers.RealBody(inClose, inOpen, i) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortPeriodTotal2, i) &&
        // black real body
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
        // well within 1st real body
        inClose[i] < inClose[i - 2] - CandleHelpers.RealBody(inClose, inOpen, i - 2) * T.CreateChecked(optInPenetration);
}
