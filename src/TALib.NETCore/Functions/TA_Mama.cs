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
    public static Core.RetCode Mama(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMAMA,
        Span<T> outFAMA,
        out int outBegIdx,
        out int outNbElement,
        double optInFastLimit = 0.5,
        double optInSlowLimit = 0.05)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInFastLimit < 0.01 || optInFastLimit > 0.99 || optInSlowLimit < 0.01 || optInSlowLimit > 0.99)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MamaLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;

        var trailingWMAIdx = startIdx - lookbackTotal;
        var today = trailingWMAIdx;

        T tempReal = inReal[today++];
        T periodWMASub = tempReal;
        T periodWMASum = tempReal;
        tempReal = inReal[today++];
        periodWMASub += tempReal;
        periodWMASum += tempReal * TTwo;
        tempReal = inReal[today++];
        periodWMASub += tempReal;
        periodWMASum += tempReal * TThree;

        T trailingWMAValue = T.Zero;
        var i = 9;
        do
        {
            tempReal = inReal[today++];
            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
        } while (--i != 0);

        int hilbertIdx = default;
        var hilbertVariables = InitHilbertVariables();

        int outIdx = default;

        T tPointTwo = T.CreateChecked(0.2);
        T tPointEight = T.CreateChecked(0.8);

        T prevI2, prevQ2, re, im, mama, fama, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, prevPhase;
        T period = prevI2 = prevQ2
            = re = im = mama = fama = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = prevPhase = T.Zero;
        while (today <= endIdx)
        {
            T tempReal2;
            T i2;
            T q2;

            T adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            T todayValue = inReal[today];
            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, todayValue,
                out var smoothedValue);
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

                tempReal2 = !T.IsZero(i1ForEvenPrev3) ? T.RadiansToDegrees(T.Atan(hilbertVariables["q1"] / i1ForEvenPrev3)) : T.Zero;
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
                tempReal2 = !T.IsZero(i1ForOddPrev3) ? T.RadiansToDegrees(T.Atan(hilbertVariables["q1"] / i1ForOddPrev3)) : T.Zero;
            }

            tempReal = prevPhase - tempReal2;
            prevPhase = tempReal2;
            if (tempReal < T.One)
            {
                tempReal = T.One;
            }

            if (tempReal > T.One)
            {
                tempReal = T.CreateChecked(optInFastLimit) / tempReal;
                if (tempReal < T.CreateChecked(optInSlowLimit))
                {
                    tempReal = T.CreateChecked(optInSlowLimit);
                }
            }
            else
            {
                tempReal = T.CreateChecked(optInFastLimit);
            }

            mama = tempReal * todayValue + (T.One - tempReal) * mama;
            tempReal *= T.CreateChecked(0.5);
            fama = tempReal * mama + (T.One - tempReal) * fama;
            if (today >= startIdx)
            {
                outMAMA[outIdx] = mama;
                outFAMA[outIdx++] = fama;
            }

            re = tPointTwo * (i2 * prevI2 + q2 * prevQ2) + tPointEight * re;
            im = tPointTwo * (i2 * prevQ2 - q2 * prevI2) + tPointEight * im;
            prevQ2 = q2;
            prevI2 = i2;
            tempReal = period;
            if (!T.IsZero(im) && !T.IsZero(re))
            {
                period = TNinety * TFour / T.RadiansToDegrees(T.Atan(im / re));
            }

            tempReal2 = T.CreateChecked(1.5) * tempReal;
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
            today++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MamaLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Mama) + 32;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Mama(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outMAMA,
        T[] outFAMA,
        double optInFastLimit = 0.5,
        double optInSlowLimit = 0.05) =>
        Mama(inReal, startIdx, endIdx, outMAMA, outFAMA, out _, out _, optInFastLimit, optInSlowLimit);
}
