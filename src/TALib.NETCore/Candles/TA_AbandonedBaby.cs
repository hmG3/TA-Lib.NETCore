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
    /// Abandoned Baby (Pattern Recognition)
    /// </summary>
    /// <param name="inOpen">A span of input open prices.</param>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outIntType">A span to store the output pattern type for each price bar.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInPenetration">
    /// The penetration factor used to determine how deeply the third candle must close within the first candle's real body:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values require the third candle to penetrate deeper into the first candle's body,
    ///       making the pattern stricter and less sensitive.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values allow for more shallow penetration, making the pattern more flexible but potentially increasing false positives.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    ///   Typical range: <c>0.1..0.5</c>. Valid values are between <c>0.0</c> (no penetration) and <c>1.0</c> (full penetration).
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
    /// Abandoned Baby function identifies a rare three-candle reversal pattern typically identified at the end of significant
    /// uptrends or downtrends. The pattern is characterized by a distinctive gap between the Doji candle and the preceding and
    /// following candles, indicating a sharp shift in market sentiment.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Identify the first candle as <em>long</em> by comparing its real body against the average of
    ///       <see cref="Core.CandleSettingType.BodyLong">BodyLong</see> in <see cref="Core.CandleSettings">CandleSettings</see>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Verify the second candle is a Doji, adhering to the average threshold specified by
    ///       <see cref="Core.CandleSettingType.BodyDoji">BodyDoji</see>. This Doji must gap away from the first candle,
    ///       forming a clear space (upside or downside gap) that emphasizes the "abandoned" nature.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm the third candle is longer than "short", surpassing <see cref="Core.CandleSettingType.BodyShort">BodyShort</see>.
    ///      It must also gap away from the Doji in the opposite direction, effectively isolating the central Doji candle.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Verify that the third candle's closing price falls well within the real body of the first candle. The allowable depth
    ///       of penetration is determined by the <paramref>optInPenetration</paramref> parameter.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of 100 indicates a bullish Abandoned Baby pattern, suggesting a potential upward trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value of -100 indicates a bearish Abandoned Baby pattern, suggesting a potential downward trend.
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
    public static Core.RetCode AbandonedBaby<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange,
        double optInPenetration = 0.3) where T : IFloatingPointIeee754<T> =>
        AbandonedBabyImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    /// <summary>
    /// Returns the lookback period for <see cref="AbandonedBaby{T}">AbandonedBaby</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AbandonedBabyLookback() =>
        Math.Max(
            Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyDoji),
                CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong)),
            CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort)
        ) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AbandonedBaby<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange,
        double optInPenetration = 0.3) where T : IFloatingPointIeee754<T> =>
        AbandonedBabyImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange, optInPenetration);

    private static Core.RetCode AbandonedBabyImpl<T>(
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

        var lookbackTotal = AbandonedBabyLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        var bodyLongPeriodTotal = T.Zero;
        var bodyDojiPeriodTotal = T.Zero;
        var bodyShortPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - 2 - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var bodyDojiTrailingIdx = startIdx - 1 - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        var bodyShortTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        var i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyDojiPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyShortPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        var penetration = T.CreateChecked(optInPenetration);
        do
        {
            outIntType[outIdx++] = IsAbandonedBabyPattern(inOpen, inHigh, inLow, inClose, i, bodyLongPeriodTotal, bodyDojiPeriodTotal,
                bodyShortPeriodTotal, penetration)
                ? (int) CandleHelpers.CandleColor(inClose, inOpen, i) * 100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            bodyLongPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);

            bodyDojiPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i - 1) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);

            bodyShortPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);

            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsAbandonedBabyPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        T bodyLongPeriodTotal,
        T bodyDojiPeriodTotal,
        T bodyShortPeriodTotal,
        T penetration) where T : IFloatingPointIeee754<T> =>
        // 1st: long
        CandleHelpers.RealBody(inClose, inOpen, i - 2) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 2) &&
        // 2nd: doji
        CandleHelpers.RealBody(inClose, inOpen, i - 1) <=
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i - 1) &&
        // 3rd: longer than short
        CandleHelpers.RealBody(inClose, inOpen, i) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortPeriodTotal, i) &&
        (
            // 1st white
            CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
            // 3rd black
            CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
            // 3rd closes well within 1st real body
            inClose[i] < inClose[i - 2] - CandleHelpers.RealBody(inClose, inOpen, i - 2) * penetration &&
            // upside gap between 1st and 2nd
            CandleHelpers.CandleGapUp(inLow, inHigh, i - 1, i - 2) &&
            // downside gap between 2nd and 3rd
            CandleHelpers.CandleGapDown(inLow, inHigh, i, i - 1)
            ||
            // 1st black
            CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
            // 3rd white
            CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
            // 3rd closes well within 1st real body
            inClose[i] > inClose[i - 2] + CandleHelpers.RealBody(inClose, inOpen, i - 2) * penetration &&
            // downside gap between 1st and 2nd
            CandleHelpers.CandleGapDown(inLow, inHigh, i - 1, i - 2) &&
            // upside gap between 2nd and 3rd
            CandleHelpers.CandleGapUp(inLow, inHigh, i, i - 1)
        );
}
