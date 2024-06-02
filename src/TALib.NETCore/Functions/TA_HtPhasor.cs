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
    public static Core.RetCode HtPhasor(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outInPhase,
        Span<T> outQuadrature,
        out int outBegIdx,
        out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        var lookbackTotal = HtPhasorLookback();
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

        T prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2;
        T period = prevI2 = prevQ2 = re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = T.Zero;
        while (today <= endIdx)
        {
            T i2;
            T q2;

            T adjustedPrevPeriod = T.CreateChecked(0.075) * period + T.CreateChecked(0.54);

            DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);
            if (today % 2 == 0)
            {
                DoHilbertEven(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                DoHilbertEven(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                if (today >= startIdx)
                {
                    outQuadrature[outIdx] = hilbertVariables["q1"];
                    outInPhase[outIdx++] = i1ForEvenPrev3;
                }

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
                if (today >= startIdx)
                {
                    outQuadrature[outIdx] = hilbertVariables["q1"];
                    outInPhase[outIdx++] = i1ForOddPrev3;
                }

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
                period = TNinety * TFour / T.RadiansToDegrees(T.Atan(im / re));
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
            today++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int HtPhasorLookback() => Core.UnstablePeriodSettings.Get(Core.UnstableFunc.HtPhasor) + 32;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>/
    private static Core.RetCode HtPhasor(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outInPhase,
        T[] outQuadrature) => HtPhasor(inReal, startIdx, endIdx, outInPhase, outQuadrature, out _, out _);
}
