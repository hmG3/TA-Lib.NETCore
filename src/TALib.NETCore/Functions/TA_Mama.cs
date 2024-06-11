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
    public static Core.RetCode Mama<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMAMA,
        Span<T> outFAMA,
        out int outBegIdx,
        out int outNbElement,
        double optInFastLimit = 0.5,
        double optInSlowLimit = 0.05) where T : IFloatingPointIeee754<T>
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
        periodWMASum += tempReal * Two<T>();
        tempReal = inReal[today++];
        periodWMASub += tempReal;
        periodWMASum += tempReal * Three<T>();

        T trailingWMAValue = T.Zero;
        var i = 9;
        do
        {
            tempReal = inReal[today++];
            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
        } while (--i != 0);

        int hilbertIdx = default;
        Span<T> hilbertVariables = HTHelper.HilbertVariablesFactory<T>();

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
                HTHelper.DoHilbertEven(hilbertVariables, HTHelper.HilbertKeys.Detrender, smoothedValue, hilbertIdx, adjustedPrevPeriod);
                HTHelper.DoHilbertEven(hilbertVariables, HTHelper.HilbertKeys.Q1, hilbertVariables[(int) HTHelper.HilbertKeys.Detrender],
                    hilbertIdx, adjustedPrevPeriod);
                HTHelper.DoHilbertEven(hilbertVariables, HTHelper.HilbertKeys.JI, i1ForEvenPrev3, hilbertIdx, adjustedPrevPeriod);
                HTHelper.DoHilbertEven(hilbertVariables, HTHelper.HilbertKeys.JQ, hilbertVariables[(int) HTHelper.HilbertKeys.Q1],
                    hilbertIdx, adjustedPrevPeriod);

                if (++hilbertIdx == 3)
                {
                    hilbertIdx = 0;
                }

                q2 = tPointTwo * (hilbertVariables[(int) HTHelper.HilbertKeys.Q1] + hilbertVariables[(int) HTHelper.HilbertKeys.JI]) +
                     tPointEight * prevQ2;
                i2 = tPointTwo * (i1ForEvenPrev3 - hilbertVariables[(int) HTHelper.HilbertKeys.JQ]) + tPointEight * prevI2;

                i1ForOddPrev3 = i1ForOddPrev2;
                i1ForOddPrev2 = hilbertVariables[(int) HTHelper.HilbertKeys.Detrender];

                tempReal2 = !T.IsZero(i1ForEvenPrev3)
                    ? T.RadiansToDegrees(T.Atan(hilbertVariables[(int) HTHelper.HilbertKeys.Q1] / i1ForEvenPrev3))
                    : T.Zero;
            }
            else
            {
                HTHelper.DoHilbertOdd(hilbertVariables, HTHelper.HilbertKeys.Detrender, smoothedValue, hilbertIdx, adjustedPrevPeriod);
                HTHelper.DoHilbertOdd(hilbertVariables, HTHelper.HilbertKeys.Q1, hilbertVariables[(int) HTHelper.HilbertKeys.Detrender],
                    hilbertIdx, adjustedPrevPeriod);
                HTHelper.DoHilbertOdd(hilbertVariables, HTHelper.HilbertKeys.JI, i1ForOddPrev3, hilbertIdx, adjustedPrevPeriod);
                HTHelper.DoHilbertOdd(hilbertVariables, HTHelper.HilbertKeys.JQ, hilbertVariables[(int) HTHelper.HilbertKeys.Q1],
                    hilbertIdx, adjustedPrevPeriod);

                q2 = tPointTwo * (hilbertVariables[(int) HTHelper.HilbertKeys.Q1] + hilbertVariables[(int) HTHelper.HilbertKeys.JI]) +
                     tPointEight * prevQ2;
                i2 = tPointTwo * (i1ForOddPrev3 - hilbertVariables[(int) HTHelper.HilbertKeys.JQ]) + tPointEight * prevI2;

                i1ForEvenPrev3 = i1ForEvenPrev2;
                i1ForEvenPrev2 = hilbertVariables[(int) HTHelper.HilbertKeys.Detrender];
                tempReal2 = !T.IsZero(i1ForOddPrev3)
                    ? T.RadiansToDegrees(T.Atan(hilbertVariables[(int) HTHelper.HilbertKeys.Q1] / i1ForOddPrev3))
                    : T.Zero;
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
                period = Ninety<T>() * Four<T>() / T.RadiansToDegrees(T.Atan(im / re));
            }

            tempReal2 = T.CreateChecked(1.5) * tempReal;
            period = T.Min(period, tempReal2);

            tempReal2 = T.CreateChecked(0.67) * tempReal;
            period = T.Max(period, tempReal2);
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
    [UsedImplicitly]
    private static Core.RetCode Mama<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outMAMA,
        T[] outFAMA,
        double optInFastLimit = 0.5,
        double optInSlowLimit = 0.05) where T : IFloatingPointIeee754<T> =>
        Mama<T>(inReal, startIdx, endIdx, outMAMA, outFAMA, out _, out _, optInFastLimit, optInSlowLimit);
}
