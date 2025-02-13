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
    /// Average True Range (Volatility Indicators)
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
    /// Average True Range is a volatility indicator that measures the degree of price movement over a specified time period.
    /// <para>
    /// The function is widely used for setting stop-loss levels, managing position sizes, and assessing volatility conditions.
    /// It may be integrated with trend or momentum indicators to enhance risk management.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Compute the True Range (TR), which is the greatest of the following:
    ///       <code>
    ///         TR = max[(High - Low), abs(High - Previous Close), abs(Low - Previous Close)]
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       For the first ATR value, calculate the simple average of the TR values over the specified time period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       For subsequent ATR values, use Wilder's smoothing method:
    ///       <code>
    ///         ATR = [(Previous ATR * (Time Period - 1)) + Current TR] / Time Period
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A higher value indicates higher market volatility.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A lower value suggests reduced volatility.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       ATR does not indicate the direction of price movement but reflects the degree of volatility over the specified period.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Atr<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AtrImpl(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Atr{T}">Atr</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int AtrLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 1 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Atr);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Atr<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        AtrImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode AtrImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* Average True Range is the greatest of the following:
         *
         *   val1 = distance from today's high to today's low.
         *   val2 = distance from yesterday's close to today's high.
         *   val3 = distance from yesterday's close to today's low.
         *
         * These value are averaged for the specified period using Wilder method.
         * The method has an unstable period comparable to and Exponential Moving Average (EMA).
         */

        var lookbackTotal = AtrLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Trap the case where no smoothing is needed.
        if (optInTimePeriod == 1)
        {
            return TRange(inHigh, inLow, inClose, inRange, outReal, out outRange);
        }

        Span<T> tempBuffer = new T[lookbackTotal + (endIdx - startIdx) + 1];
        var retCode = TRangeImpl(inHigh, inLow, inClose, new Range(startIdx - lookbackTotal + 1, endIdx), tempBuffer, out _);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // First value of the ATR is a simple Average of the TRange output for the specified period.
        Span<T> prevATRTemp = new T[1];
        retCode = FunctionHelpers.CalcSimpleMA(tempBuffer, new Range(optInTimePeriod - 1, optInTimePeriod - 1), prevATRTemp, out _,
            optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        var prevATR = prevATRTemp[0];

        /* Subsequent value are smoothed using the previous ATR value (Wilder's approach).
         *   1) Multiply the previous ATR by 'period - 1'.
         *   2) Add today TR value.
         *   3) Divide by 'period'.
         */
        var today = optInTimePeriod;
        var outIdx = Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Atr);
        // Skip the unstable period.
        while (outIdx != 0)
        {
            prevATR *= timePeriod - T.One;
            prevATR += tempBuffer[today++];
            prevATR /= timePeriod;
            outIdx--;
        }

        outIdx = 1;
        outReal[0] = prevATR;

        // Do the number of requested ATR.
        var nbATR = endIdx - startIdx + 1;

        while (--nbATR != 0)
        {
            prevATR *= timePeriod - T.One;
            prevATR += tempBuffer[today++];
            prevATR /= timePeriod;
            outReal[outIdx++] = prevATR;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return retCode;
    }
}
