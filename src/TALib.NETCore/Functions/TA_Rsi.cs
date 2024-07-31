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
    public static Core.RetCode Rsi<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        RsiImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int RsiLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod < 2)
        {
            return -1;
        }

        var retValue = optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Rsi);
        if (Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            retValue--;
        }

        return retValue;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Rsi<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        RsiImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode RsiImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /* The following algorithm is base on the original work from Wilder's and shall represent
         * the original idea behind the classic RSI.
         *
         * Metastock is starting the calculation one price bar earlier.
         * To make this possible, they assume that the very first bar will be identical to the previous one (no gain or loss).
         */

        var lookbackTotal = RsiLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        int outIdx = default;

        // Accumulate Wilder's "Average Gain" and "Average Loss" among the initial period.
        var today = startIdx - lookbackTotal;
        var prevValue = inReal[today];
        T prevGain;
        T prevLoss;

        // If there is no unstable period, calculate the 'additional' initial price bar who is particular to Metastock.
        // If there is an unstable period, no need to calculate since this first value will be surely skip.
        if (Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Rsi) == 0 &&
            Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            // Preserve prevValue because it may get overwritten by the output.
            var savePrevValue = prevValue;
            T tempValue1;
            T tempValue2;

            // No unstable period, so must calculate first output particular to Metastock.
            prevGain = T.Zero;
            prevLoss = T.Zero;
            for (var i = optInTimePeriod; i > 0; i--)
            {
                tempValue1 = inReal[today++];
                tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;
                if (tempValue2 < T.Zero)
                {
                    prevLoss -= tempValue2;
                }
                else
                {
                    prevGain += tempValue2;
                }
            }

            tempValue1 = prevLoss / timePeriod;
            tempValue2 = prevGain / timePeriod;

            tempValue1 = tempValue2 + tempValue1;
            outReal[outIdx++] = !T.IsZero(tempValue1) ? Hundred<T>() * (tempValue2 / tempValue1) : T.Zero;

            if (today > endIdx)
            {
                outRange = new Range(startIdx, startIdx + outIdx);
                return Core.RetCode.Success;
            }

            // Start over for the next price bar.
            today -= optInTimePeriod;
            prevValue = savePrevValue;
        }

        // Remaining of the processing is identical.
        prevGain = T.Zero;
        prevLoss = T.Zero;
        today++;
        for (var i = optInTimePeriod; i > 0; i--)
        {
            var tempValue1 = inReal[today++];
            var tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;
            if (tempValue2 < T.Zero)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }
        }

        /* Subsequent prevLoss and prevGain are smoothed using the previous values (Wilder's approach).
         *   1) Multiply the previous by 'period - 1'.
         *   2) Add today value.
         *   3) Divide by 'period'.
         */
        prevLoss /= timePeriod;
        prevGain /= timePeriod;

        /* Often documentation present the RSI calculation as follows:
         *   RSI = 100 - (100 / 1 + (prevGain / prevLoss))
         *
         * The following is equivalent:
         *   RSI = 100 * (prevGain / (prevGain + prevLoss))
         *
         * The second equation is used here for speed optimization.
         */
        if (today > startIdx)
        {
            var tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !T.IsZero(tempValue1) ? Hundred<T>() * (prevGain / tempValue1) : T.Zero;
        }
        else
        {
            // Skip the unstable period. Do the processing but do not write it in the output.
            while (today < startIdx)
            {
                var tempValue1 = inReal[today];
                var tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;

                prevLoss *= timePeriod - T.One;
                prevGain *= timePeriod - T.One;
                if (tempValue2 < T.Zero)
                {
                    prevLoss -= tempValue2;
                }
                else
                {
                    prevGain += tempValue2;
                }

                prevLoss /= timePeriod;
                prevGain /= timePeriod;

                today++;
            }
        }

        // Unstable period skipped... now continue processing if needed.
        while (today <= endIdx)
        {
            var tempValue1 = inReal[today++];
            var tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;

            prevLoss *= timePeriod - T.One;
            prevGain *= timePeriod - T.One;
            if (tempValue2 < T.Zero)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }

            prevLoss /= timePeriod;
            prevGain /= timePeriod;
            tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !T.IsZero(tempValue1) ? Hundred<T>() * (prevGain / tempValue1) : T.Zero;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
