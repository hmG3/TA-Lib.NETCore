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
    public static Core.RetCode Mfi(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length || endIdx >= inVolume.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MfiLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;

        var moneyFlow = new (T negative, T positive)[optInTimePeriod];

        int mflowIdx = default;
        var maxIdxMflow = optInTimePeriod - 1;

        int today = startIdx - lookbackTotal;
        T prevValue = (inHigh[today] + inLow[today] + inClose[today]) / TThree;

        T posSumMF = T.Zero;
        T negSumMF = T.Zero;
        today++;
        for (var i = optInTimePeriod; i > 0; i--)
        {
            T tempValue1 = (inHigh[today] + inLow[today] + inClose[today]) / TThree;
            T tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;
            tempValue1 *= inVolume[today++];
            if (tempValue2 < T.Zero)
            {
                moneyFlow[mflowIdx].negative = tempValue1;
                negSumMF += tempValue1;
                moneyFlow[mflowIdx].positive = T.Zero;
            }
            else if (tempValue2 > T.Zero)
            {
                moneyFlow[mflowIdx].positive = tempValue1;
                posSumMF += tempValue1;
                moneyFlow[mflowIdx].negative = T.Zero;
            }
            else
            {
                moneyFlow[mflowIdx].positive = T.Zero;
                moneyFlow[mflowIdx].negative = T.Zero;
            }

            if (++mflowIdx > maxIdxMflow)
            {
                mflowIdx = 0;
            }
        }

        if (today > startIdx)
        {
            T tempValue1 = posSumMF + negSumMF;
            outReal[outIdx++] = tempValue1 >= T.One ? THundred * (posSumMF / tempValue1) : T.Zero;
        }
        else
        {
            while (today < startIdx)
            {
                posSumMF -= moneyFlow[mflowIdx].positive;
                negSumMF -= moneyFlow[mflowIdx].negative;

                T tempValue1 = (inHigh[today] + inLow[today] + inClose[today]) / TThree;
                T tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;
                tempValue1 *= inVolume[today++];
                if (tempValue2 < T.Zero)
                {
                    moneyFlow[mflowIdx].negative = tempValue1;
                    negSumMF += tempValue1;
                    moneyFlow[mflowIdx].positive = T.Zero;
                }
                else if (tempValue2 > T.Zero)
                {
                    moneyFlow[mflowIdx].positive = tempValue1;
                    posSumMF += tempValue1;
                    moneyFlow[mflowIdx].negative = T.Zero;
                }
                else
                {
                    moneyFlow[mflowIdx].positive = T.Zero;
                    moneyFlow[mflowIdx].negative = T.Zero;
                }

                if (++mflowIdx > maxIdxMflow)
                {
                    mflowIdx = 0;
                }
            }
        }

        while (today <= endIdx)
        {
            posSumMF -= moneyFlow[mflowIdx].positive;
            negSumMF -= moneyFlow[mflowIdx].negative;

            T tempValue1 = (inHigh[today] + inLow[today] + inClose[today]) / TThree;
            T tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;
            tempValue1 *= inVolume[today++];
            if (tempValue2 < T.Zero)
            {
                moneyFlow[mflowIdx].negative = tempValue1;
                negSumMF += tempValue1;
                moneyFlow[mflowIdx].positive = T.Zero;
            }
            else if (tempValue2 > T.Zero)
            {
                moneyFlow[mflowIdx].positive = tempValue1;
                posSumMF += tempValue1;
                moneyFlow[mflowIdx].negative = T.Zero;
            }
            else
            {
                moneyFlow[mflowIdx].positive = T.Zero;
                moneyFlow[mflowIdx].negative = T.Zero;
            }

            tempValue1 = posSumMF + negSumMF;
            outReal[outIdx++] = tempValue1 >= T.One ? THundred * (posSumMF / tempValue1) : T.Zero;

            if (++mflowIdx > maxIdxMflow)
            {
                mflowIdx = 0;
            }
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MfiLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Mfi);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Mfi(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        T[] inVolume,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) => Mfi(inHigh, inLow, inClose, inVolume, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
