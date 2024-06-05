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
    public static Core.RetCode HtTrendline<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = HtTrendlineLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        const int smoothPriceSize = 50;
        Span<T> smoothPrice = new T[smoothPriceSize];

        T iTrend2, iTrend1;
        T iTrend3 = iTrend2 = iTrend1 = T.Zero;

        outBegIdx = startIdx;

        var trailingWMAIdx = startIdx - lookbackTotal;
        var today = trailingWMAIdx;

        T tempReal = inReal[today++];
        T periodWMASub = tempReal;
        T periodWMASum = tempReal;
        tempReal = inReal[today++];
        periodWMASub += tempReal;
        periodWMASum += tempReal * Two<T>();
        tempReal = inReal[today++];
        periodWMASub += tempReal;
        periodWMASum += tempReal * Three<T>();

        T trailingWMAValue = T.Zero;

        var i = 34;
        do
        {
            tempReal = inReal[today++];
            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
        } while (--i != 0);

        int hilbertIdx = default;
        int smoothPriceIdx = default;

        var hilbertVariables = InitHilbertVariables<T>();

        int outIdx = default;

        T tPointTwo = T.CreateChecked(0.2);
        T tPointEight = T.CreateChecked(0.8);

        T prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod;
        T period = prevI2 = prevQ2 = re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = T.Zero;
        while (today <= endIdx)
        {
            T i2;
            T q2;

            T adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);

            smoothPrice[smoothPriceIdx] = smoothedValue;
            if (today % 2 == 0)
            {
                DoHilbertEven(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                DoHilbertEven(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                DoHilbertEven(hilbertVariables, "jI", i1ForEvenPrev3, hilbertIdx, adjustedPrevPeriod);
                DoHilbertEven(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

                if (++hilbertIdx == 3)
                {
                    hilbertIdx = 0;
                }

                q2 = tPointTwo * (hilbertVariables["q1"] + hilbertVariables["jI"]) + tPointEight * prevQ2;
                i2 = tPointTwo * (i1ForEvenPrev3 - hilbertVariables["jQ"]) + tPointEight * prevI2;

                i1ForOddPrev3 = i1ForOddPrev2;
                i1ForOddPrev2 = hilbertVariables["detrender"];
            }
            else
            {
                DoHilbertOdd(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                DoHilbertOdd(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                DoHilbertOdd(hilbertVariables, "jI", i1ForOddPrev3, hilbertIdx, adjustedPrevPeriod);
                DoHilbertOdd(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

                q2 = tPointTwo * (hilbertVariables["q1"] + hilbertVariables["jI"]) + tPointEight * prevQ2;
                i2 = tPointTwo * (i1ForOddPrev3 - hilbertVariables["jQ"]) + tPointEight * prevI2;

                i1ForEvenPrev3 = i1ForEvenPrev2;
                i1ForEvenPrev2 = hilbertVariables["detrender"];
            }

            re = tPointTwo * (i2 * prevI2 + q2 * prevQ2) + tPointEight * re;
            im = tPointTwo * (i2 * prevQ2 - q2 * prevI2) + tPointEight * im;
            prevQ2 = q2;
            prevI2 = i2;
            tempReal = period;
            if (!T.IsZero(im) && !T.IsZero(re))
            {
                period = Ninety<T>() * Four<T>() / T.RadiansToDegrees(T.Atan(im / re));
            }

            T tempReal2 = T.CreateChecked(1.5) * tempReal;
            if (period > tempReal2)
            {
                period = tempReal2;
            }

            tempReal2 = T.CreateChecked(0.67) * tempReal;
            if (period < tempReal2)
            {
                period = tempReal2;
            }

            period = T.Clamp(period, T.CreateChecked(6), T.CreateChecked(50));

            period = tPointTwo * period + tPointEight * tempReal;

            smoothPeriod = T.CreateChecked(0.33) * period + T.CreateChecked(0.67) * smoothPeriod;

            T dcPeriod = smoothPeriod + T.CreateChecked(0.5);
            var dcPeriodInt = Int32.CreateTruncating(dcPeriod);

            var idx = today;
            tempReal = T.Zero;
            for (i = 0; i < dcPeriodInt; i++)
            {
                tempReal += inReal[idx--];
            }

            if (dcPeriodInt > 0)
            {
                tempReal /= T.CreateChecked(dcPeriodInt);
            }

            tempReal2 = (Four<T>() * tempReal + Three<T>() * iTrend1 + Two<T>() * iTrend2 + iTrend3) / T.CreateChecked(10);
            iTrend3 = iTrend2;
            iTrend2 = iTrend1;
            iTrend1 = tempReal;

            if (today >= startIdx)
            {
                outReal[outIdx++] = tempReal2;
            }

            if (++smoothPriceIdx > smoothPriceSize - 1)
            {
                smoothPriceIdx = 0;
            }

            today++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int HtTrendlineLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtTrendline) + 63;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode HtTrendline<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal) where T : IFloatingPointIeee754<T> => HtTrendline<T>(inReal, startIdx, endIdx, outReal, out _, out _);
}
