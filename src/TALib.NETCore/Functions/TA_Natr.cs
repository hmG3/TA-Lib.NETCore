/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2024 Anatolii Siryi
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
    [PublicAPI]
    public static Core.RetCode Natr<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        NatrImpl(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int NatrLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 1 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Natr);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Natr<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        NatrImpl<T>(inHigh, inLow, inClose, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode NatrImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        /* This function is very similar as ATR, except it is being normalized as follows:
         *
         *   NATR = (ATR(period) / Close) * 100
         *
         *
         * Normalization make the ATR function more relevant in the following scenario:
         *   - Long term analysis where the price changes drastically.
         *   - Cross-market or cross-security ATR comparison.
         *
         * More Info:
         *   Technical Analysis of Stock & Commodities (TASC)
         *   May 2006 by John Forman
         */

        /* Average True Range is the greatest of the following:
         *
         *  val1 = distance from today's high to today's low.
         *  val2 = distance from yesterday's close to today's high.
         *  val3 = distance from yesterday's close to today's low.
         *
         * These value are averaged for the specified period using Wilder method.
         * The method has an unstable period comparable to and Exponential Moving Average (EMA).
         */

        var lookbackTotal = NatrLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        if (optInTimePeriod == 1)
        {
            // No smoothing needed. Just do a TRange.
            return TRange(inHigh, inLow, inClose, inRange, outReal, out outRange);
        }

        Span<T> tempBuffer = new T[lookbackTotal + (endIdx - startIdx) + 1];

        // Do TRange in the intermediate buffer.
        var retCode = TRangeImpl(inHigh, inLow, inClose, new Range(startIdx - lookbackTotal + 1, endIdx), tempBuffer, out _);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        Span<T> prevATRTemp = new T[1];

        // First value of the ATR is a simple Average of the TRange output for the specified period.
        retCode = CalcSimpleMA(tempBuffer, new Range(optInTimePeriod - 1, optInTimePeriod - 1), prevATRTemp, out _, optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        var prevATR = prevATRTemp[0];

        /* Subsequent value are smoothed using the previous ATR value (Wilder's approach).
         *   1) Multiply the previous ATR by 'period-1'.
         *   2) Add today TR value.
         *   3) Divide by 'period'.
         */
        var today = optInTimePeriod;
        var outIdx = Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Natr);
        // Skip the unstable period.
        while (outIdx != 0)
        {
            prevATR *= timePeriod - T.One;
            prevATR += tempBuffer[today++];
            prevATR /= timePeriod;
            outIdx--;
        }

        outIdx = 1;
        var tempValue = inClose[today];
        outReal[0] = !T.IsZero(tempValue) ? prevATR / tempValue * Hundred<T>() : T.Zero;

        // Do the number of requested ATR.
        var nbATR = endIdx - startIdx + 1;

        while (--nbATR != 0)
        {
            prevATR *= timePeriod - T.One;
            prevATR += tempBuffer[today++];
            prevATR /= timePeriod;
            tempValue = inClose[today];
            if (!T.IsZero(tempValue))
            {
                outReal[outIdx] = prevATR / tempValue * Hundred<T>();
            }
            else
            {
                outReal[0] = T.Zero;
            }

            outIdx++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return retCode;
    }
}
