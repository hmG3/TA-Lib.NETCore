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
    /// Advance Block (Pattern Recognition)
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
    /// Advance Block function identifies a three-candle bearish reversal pattern often observed at the end of an uptrend.
    /// It is characterized by three consecutive white candles with progressively smaller real bodies and indications of market exhaustion,
    /// such as relatively long upper shadows, suggesting weakening bullish momentum.
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Identify three consecutive white candlesticks. Each candle must close higher than the previous close.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Confirm that:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             The second candle's opening price is within or <em>near</em> the real body of the first candle
    ///             (<see cref="Core.CandleSettingType.Near">Near</see> in <see cref="Core.CandleSettings">CandleSettings</see>
    ///             defines what "near" means).
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             The third candle's opening price is likewise within or near the real body of the second candle.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Validate that the first candle has a <em>long</em> real body (body length exceeds the average length specified by
    ///       <see cref="Core.CandleSettingType.BodyLong">BodyLong</see>), and that its upper shadow is <em>short</em>
    ///       (shadow length is less than the average length specified by <see cref="Core.CandleSettingType.ShadowShort">ShadowShort</see>).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Evaluate the second and third candles, or only the third candle, for signs of waning momentum:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             Their white real bodies may be progressively smaller than those of preceding candles, referencing
    ///             <see cref="Core.CandleSettingType.Far">Far</see> or <see cref="Core.CandleSettingType.Near">Near</see> to gauge
    ///             the difference.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             Their upper shadows may be relatively long, signifying resistance to further upward movement
    ///             (evaluated using <see cref="Core.CandleSettingType.ShadowLong">ShadowLong</see> or
    ///             <see cref="Core.CandleSettingType.ShadowShort">ShadowShort</see>).
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value of -100 indicates the presence of an Advance Block pattern, signaling a potential bearish reversal.
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
    public static Core.RetCode AdvanceBlock<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<int> outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        AdvanceBlockImpl(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    /// <summary>
    /// Returns the lookback period for <see cref="AdvanceBlock{T}">AdvanceBlock</see>.
    /// </summary>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AdvanceBlockLookback() =>
        Math.Max(
            Math.Max(
                Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowLong),
                    CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowShort)),
                Math.Max(CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Far),
                    CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near))),
            CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong)
        ) + 2;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AdvanceBlock<T>(
        T[] inOpen,
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        int[] outIntType,
        out Range outRange) where T : IFloatingPointIeee754<T> =>
        AdvanceBlockImpl<T>(inOpen, inHigh, inLow, inClose, inRange, outIntType, out outRange);

    private static Core.RetCode AdvanceBlockImpl<T>(
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

        var lookbackTotal = AdvanceBlockLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        Span<T> shadowShortPeriodTotal = new T[3];
        var shadowShortTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowShort);
        Span<T> shadowLongPeriodTotal = new T[2];
        var shadowLongTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.ShadowLong);
        Span<T> nearPeriodTotal = new T[3];
        var nearTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Near);
        Span<T> farPeriodTotal = new T[3];
        var farTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.Far);
        var bodyLongPeriodTotal = T.Zero;
        var bodyLongTrailingIdx = startIdx - CandleHelpers.CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        var i = shadowShortTrailingIdx;
        while (i < startIdx)
        {
            shadowShortPeriodTotal[2] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - 2);
            shadowShortPeriodTotal[1] +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - 1);
            shadowShortPeriodTotal[0] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal[1] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 1);
            shadowLongPeriodTotal[0] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[2] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            nearPeriodTotal[1] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = farTrailingIdx;
        while (i < startIdx)
        {
            farPeriodTotal[2] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - 2);
            farPeriodTotal[1] += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2);
            i++;
        }

        i = startIdx;

        var outIdx = 0;
        do
        {
            outIntType[outIdx++] = IsAdvanceBlockPattern(inOpen, inHigh, inLow, inClose, i, nearPeriodTotal, bodyLongPeriodTotal,
                shadowShortPeriodTotal, farPeriodTotal, shadowLongPeriodTotal)
                ? -100
                : 0;

            // add the current range and subtract the first range: this is done after the pattern recognition
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
            for (var totIdx = 2; totIdx >= 0; --totIdx)
            {
                shadowShortPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort,
                        shadowShortTrailingIdx - totIdx);
            }

            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                shadowLongPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong,
                        shadowLongTrailingIdx - totIdx);
            }

            for (var totIdx = 2; totIdx >= 1; --totIdx)
            {
                farPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, farTrailingIdx - totIdx);

                nearPeriodTotal[totIdx] +=
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx) -
                    CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - totIdx);
            }

            bodyLongPeriodTotal +=
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                CandleHelpers.CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 2);

            i++;
            shadowShortTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool IsAdvanceBlockPattern<T>(
        ReadOnlySpan<T> inOpen,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int i,
        Span<T> nearPeriodTotal,
        T bodyLongPeriodTotal,
        Span<T> shadowShortPeriodTotal,
        Span<T> farPeriodTotal,
        Span<T> shadowLongPeriodTotal) where T : IFloatingPointIeee754<T> =>
        // 1st white
        CandleHelpers.CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
        // 2nd white
        CandleHelpers.CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
        // 3rd white
        CandleHelpers.CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
        // consecutive higher closes
        inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] &&
        // 2nd opens within/near 1st real body
        inOpen[i - 1] > inOpen[i - 2] &&
        // 3rd opens within/near 2nd real body
        inOpen[i - 1] <= inClose[i - 2] +
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
        inOpen[i] > inOpen[i - 1] &&
        inOpen[i] <= inClose[i - 1] +
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[1], i - 1) &&
        // 1st: long real body
        CandleHelpers.RealBody(inClose, inOpen, i - 2) >
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 2) &&
        // 1st: short upper shadow
        CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i - 2) <
        CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[2], i - 2) &&
        (
            // (2 far smaller than 1 && 3 not longer than 2)
            // advance blocked with the 2nd, 3rd must not carry on the advance
            CandleHelpers.RealBody(inClose, inOpen, i - 1) < CandleHelpers.RealBody(inClose, inOpen, i - 2) -
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, farPeriodTotal[2], i - 2) &&
            CandleHelpers.RealBody(inClose, inOpen, i) < CandleHelpers.RealBody(inClose, inOpen, i - 1) +
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[1], i - 1)
            ||
            // 3 far smaller than 2
            // advance blocked with the 3rd
            CandleHelpers.RealBody(inClose, inOpen, i) < CandleHelpers.RealBody(inClose, inOpen, i - 1) -
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, farPeriodTotal[1], i - 1)
            ||
            // (3 smaller than 2 && 2 smaller than 1 && (3 or 2 not short upper shadow))
            // advance blocked with progressively smaller real bodies and some upper shadows
            CandleHelpers.RealBody(inClose, inOpen, i) < CandleHelpers.RealBody(inClose, inOpen, i - 1) &&
            CandleHelpers.RealBody(inClose, inOpen, i - 1) < CandleHelpers.RealBody(inClose, inOpen, i - 2) &&
            (
                CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i) >
                CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[0],
                    i)
                ||
                CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i - 1) >
                CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[1],
                    i - 1)
            )
            ||
            // (3 smaller than 2 && 3 long upper shadow)
            // advance blocked with 3rd candle's long upper shadow and smaller body
            CandleHelpers.RealBody(inClose, inOpen, i) < CandleHelpers.RealBody(inClose, inOpen, i - 1) &&
            CandleHelpers.UpperShadow(inHigh, inClose, inOpen, i) >
            CandleHelpers.CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal[0], i)
        );
}
