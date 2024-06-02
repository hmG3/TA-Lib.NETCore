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

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Rsi(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = RsiLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T timePeriod = T.CreateChecked(optInTimePeriod);

        int outIdx = default;
        var today = startIdx - lookbackTotal;
        T prevValue = inReal[today];
        T prevGain;
        T prevLoss;
        if (Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Rsi) == 0 &&
            Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            T savePrevValue = prevValue;
            T tempValue1;
            T tempValue2;
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
            outReal[outIdx++] = !T.IsZero(tempValue1) ? THundred * (tempValue2 / tempValue1) : T.Zero;

            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNbElement = outIdx;
                return Core.RetCode.Success;
            }

            today -= optInTimePeriod;
            prevValue = savePrevValue;
        }

        prevGain = T.Zero;
        prevLoss = T.Zero;
        today++;
        for (var i = optInTimePeriod; i > 0; i--)
        {
            T tempValue1 = inReal[today++];
            T tempValue2 = tempValue1 - prevValue;
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

        prevLoss /= timePeriod;
        prevGain /= timePeriod;

        if (today > startIdx)
        {
            T tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !T.IsZero(tempValue1) ? THundred * (prevGain / tempValue1) : T.Zero;
        }
        else
        {
            while (today < startIdx)
            {
                T tempValue1 = inReal[today];
                T tempValue2 = tempValue1 - prevValue;
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

        while (today <= endIdx)
        {
            T tempValue1 = inReal[today++];
            T tempValue2 = tempValue1 - prevValue;
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
            outReal[outIdx++] = !T.IsZero(tempValue1) ? THundred * (prevGain / tempValue1) : T.Zero;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

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
    private static Core.RetCode Rsi(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) => Rsi(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
