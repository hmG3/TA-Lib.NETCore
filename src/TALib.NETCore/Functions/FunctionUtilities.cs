﻿/*
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
    private static T TTwo = T.CreateChecked(2);
    private static T TThree = T.CreateChecked(3);
    private static T TFour = T.CreateChecked(4);
    private static T TNinety = T.CreateChecked(90);
    private static T THundred = T.CreateChecked(100);

    private static Core.RetCode CalcExponentialMA(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod,
        T optInK1)
    {
        outBegIdx = outNbElement = 0;

        var lookbackTotal = EmaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;

        int today;
        T prevMA;
        if (Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Default)
        {
            today = startIdx - lookbackTotal;
            var i = optInTimePeriod;
            T tempReal = T.Zero;
            while (i-- > 0)
            {
                tempReal += inReal[today++];
            }

            prevMA = tempReal / T.CreateChecked(optInTimePeriod);
        }
        else
        {
            prevMA = inReal[0];
            today = 1;
        }

        while (today <= startIdx)
        {
            prevMA = (inReal[today++] - prevMA) * optInK1 + prevMA;
        }

        outReal[0] = prevMA;
        var outIdx = 1;
        while (today <= endIdx)
        {
            prevMA = (inReal[today++] - prevMA) * optInK1 + prevMA;
            outReal[outIdx++] = prevMA;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    private static Core.RetCode CalcMACD(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMacd,
        Span<T> outMacdSignal,
        Span<T> outMacdHist,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod,
        int optInSlowPeriod,
        int optInSignalPeriod)
    {
        outBegIdx = outNbElement = 0;

        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
        }

        T k1;
        T k2;
        if (optInSlowPeriod != 0)
        {
            k1 = TTwo / (T.CreateChecked(optInSlowPeriod) + T.One);
        }
        else
        {
            optInSlowPeriod = 26;
            k1 = T.CreateChecked(0.075);
        }

        if (optInFastPeriod != 0)
        {
            k2 = TTwo / (T.CreateChecked(optInFastPeriod) + T.One);
        }
        else
        {
            optInFastPeriod = 12;
            k2 = T.CreateChecked(0.15);
        }

        var lookbackSignal = EmaLookback(optInSignalPeriod);
        var lookbackTotal = MacdLookback(optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
        Span<T> fastEMABuffer = new T[tempInteger];
        Span<T> slowEMABuffer = new T[tempInteger];

        tempInteger = startIdx - lookbackSignal;
        var retCode = CalcExponentialMA(inReal, tempInteger, endIdx, slowEMABuffer, out var outBegIdx1, out var outNbElement1,
            optInSlowPeriod, k1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        retCode = CalcExponentialMA(inReal, tempInteger, endIdx, fastEMABuffer, out var outBegIdx2, out var outNbElement2, optInFastPeriod,
            k2);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
            outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
        {
            return Core.RetCode.InternalError;
        }

        for (var i = 0; i < outNbElement1; i++)
        {
            fastEMABuffer[i] -= slowEMABuffer[i];
        }

        fastEMABuffer.Slice(lookbackSignal, endIdx - startIdx + 1).CopyTo(outMacd);
        retCode = CalcExponentialMA(fastEMABuffer, 0, outNbElement1 - 1, outMacdSignal, out _, out outNbElement2, optInSignalPeriod,
            TTwo / (T.CreateChecked(optInSignalPeriod) + T.One));
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        for (var i = 0; i < outNbElement2; i++)
        {
            outMacdHist[i] = outMacd[i] - outMacdSignal[i];
        }

        outBegIdx = startIdx;
        outNbElement = outNbElement2;

        return Core.RetCode.Success;
    }

    private static Core.RetCode CalcPriceOscillator(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod,
        int optInSlowPeriod,
        Core.MAType optInMethod,
        Span<T> tempBuffer,
        bool doPercentageOutput)
    {
        outBegIdx = outNbElement = 0;

        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
        }

        var retCode = Ma(inReal, startIdx, endIdx, tempBuffer, out var outBegIdx2, out _, optInFastPeriod, optInMethod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        retCode = Ma(inReal, startIdx, endIdx, outReal, out var outBegIdx1, out var outNbElement1, optInSlowPeriod, optInMethod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        for (int i = 0, j = outBegIdx1 - outBegIdx2; i < outNbElement1; i++, j++)
        {
            if (doPercentageOutput)
            {
                T tempReal = outReal[i];
                outReal[i] = !T.IsZero(tempReal) ? (tempBuffer[j] - tempReal) / tempReal * THundred : T.Zero;
            }
            else
            {
                outReal[i] = tempBuffer[j] - outReal[i];
            }
        }

        outBegIdx = outBegIdx1;
        outNbElement = outNbElement1;

        return retCode;
    }

    private static Core.RetCode CalcSimpleMA(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod)
    {
        outBegIdx = outNbElement = 0;

        var lookbackTotal = SmaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T periodTotal = T.Zero;
        var trailingIdx = startIdx - lookbackTotal;
        var i = trailingIdx;
        if (optInTimePeriod > 1)
        {
            while (i < startIdx)
            {
                periodTotal += inReal[i++];
            }
        }

        int outIdx = default;
        var timePeriod = T.CreateChecked(optInTimePeriod);
        do
        {
            periodTotal += inReal[i++];
            T tempReal = periodTotal;
            periodTotal -= inReal[trailingIdx++];
            outReal[outIdx++] = tempReal / timePeriod;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    private static void CalcStandardDeviation(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inMovAvg,
        int inMovAvgBegIdx,
        int inMovAvgNbElement,
        Span<T> outReal,
        int optInTimePeriod)
    {
        var startSum = inMovAvgBegIdx + 1 - optInTimePeriod;
        var endSum = inMovAvgBegIdx;
        T periodTotal2 = T.Zero;
        for (var outIdx = startSum; outIdx < endSum; outIdx++)
        {
            T tempReal = inReal[outIdx];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);
        for (var outIdx = 0; outIdx < inMovAvgNbElement; outIdx++, startSum++, endSum++)
        {
            T tempReal = inReal[endSum];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            T meanValue2 = periodTotal2 / timePeriod;

            tempReal = inReal[startSum];
            tempReal *= tempReal;
            periodTotal2 -= tempReal;

            tempReal = inMovAvg[outIdx];
            tempReal *= tempReal;
            meanValue2 -= tempReal;

            outReal[outIdx] = meanValue2 > T.Zero ? T.Sqrt(meanValue2) : T.Zero;
        }
    }

    private static Core.RetCode CalcVariance(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod)
    {
        outBegIdx = outNbElement = 0;

        var lookbackTotal = VarLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T periodTotal1 = T.Zero;
        T periodTotal2 = T.Zero;
        var trailingIdx = startIdx - lookbackTotal;
        var i = trailingIdx;
        if (optInTimePeriod > 1)
        {
            while (i < startIdx)
            {
                T tempReal = inReal[i++];
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
            }
        }

        int outIdx = default;
        T timePeriod = T.CreateChecked(optInTimePeriod);
        do
        {
            T tempReal = inReal[i++];
            periodTotal1 += tempReal;
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            T meanValue1 = periodTotal1 / timePeriod;
            T meanValue2 = periodTotal2 / timePeriod;
            tempReal = inReal[trailingIdx++];
            periodTotal1 -= tempReal;
            tempReal *= tempReal;
            periodTotal2 -= tempReal;
            outReal[outIdx++] = meanValue2 - meanValue1 * meanValue1;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    private static void TrueRange(
        T th,
        T tl,
        T yc,
        out T @out)
    {
        @out = th - tl;
        T tempT = T.Abs(th - yc);
        if (tempT > @out)
        {
            @out = tempT;
        }

        tempT = T.Abs(tl - yc);
        if (tempT > @out)
        {
            @out = tempT;
        }
    }

    private static void DoPriceWma(
        ReadOnlySpan<T> real,
        ref int idx,
        ref T periodWMASub,
        ref T periodWMASum,
        ref T trailingWMAValue,
        T varNewPrice,
        out T varToStoreSmoothedValue)
    {
        periodWMASub += varNewPrice;
        periodWMASub -= trailingWMAValue;
        periodWMASum += varNewPrice * TFour;
        trailingWMAValue = real[idx++];
        varToStoreSmoothedValue = periodWMASum * T.CreateChecked(0.1);
        periodWMASum -= periodWMASub;
    }

    private static void CalcTerms(
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inClose,
        int day,
        out T trueRange,
        out T closeMinusTrueLow)
    {
        T tempLT = inLow[day];
        T tempHT = inHigh[day];
        T tempCY = inClose[day - 1];
        T trueLow = T.Min(tempLT, tempCY);
        closeMinusTrueLow = inClose[day] - trueLow;
        trueRange = tempHT - tempLT;
        T tempT = T.Abs(tempCY - tempHT);
        if (tempT > trueRange)
        {
            trueRange = tempT;
        }

        tempT = T.Abs(tempCY - tempLT);
        if (tempT > trueRange)
        {
            trueRange = tempT;
        }
    }

    private static Dictionary<string, T> InitHilbertVariables()
    {
        var variables = new Dictionary<string, T>(4 * 11);

        new List<string> { "detrender", "q1", "jI", "jQ" }.ForEach(varName =>
        {
            variables.Add($"{varName}Odd0", T.Zero);
            variables.Add($"{varName}Odd1", T.Zero);
            variables.Add($"{varName}Odd2", T.Zero);
            variables.Add($"{varName}Even0", T.Zero);
            variables.Add($"{varName}Even1", T.Zero);
            variables.Add($"{varName}Even2", T.Zero);
            variables.Add(varName, T.Zero);
            variables.Add($"prev{varName}Odd", T.Zero);
            variables.Add($"prev{varName}Even", T.Zero);
            variables.Add($"prev{varName}InputOdd", T.Zero);
            variables.Add($"prev{varName}InputEven", T.Zero);
        });

        return variables;
    }

    private static void DoHilbertTransform(
        IDictionary<string, T> variables,
        string varName,
        T input,
        string oddOrEvenId,
        int hilbertIdx,
        T adjustedPrevPeriod)
    {
        T a = T.CreateChecked(0.0962);
        T b = T.CreateChecked(0.5769);

        T hilbertTempT = a * input;
        variables[varName] = -variables[$"{varName}{oddOrEvenId}{hilbertIdx}"];
        variables[$"{varName}{oddOrEvenId}{hilbertIdx}"] = hilbertTempT;
        variables[varName] += hilbertTempT;
        variables[varName] -= variables[$"prev{varName}{oddOrEvenId}"];
        variables[$"prev{varName}{oddOrEvenId}"] = b * variables[$"prev{varName}Input{oddOrEvenId}"];
        variables[varName] += variables[$"prev{varName}{oddOrEvenId}"];
        variables[$"prev{varName}Input{oddOrEvenId}"] = input;
        variables[varName] *= adjustedPrevPeriod;
    }

    private static void DoHilbertOdd(
        IDictionary<string, T> variables,
        string varName,
        T input,
        int hilbertIdx,
        T adjustedPrevPeriod)
    {
        DoHilbertTransform(variables, varName, input, "Odd", hilbertIdx, adjustedPrevPeriod);
    }

    private static void DoHilbertEven(
        IDictionary<string, T> variables,
        string varName,
        T input,
        int hilbertIdx,
        T adjustedPrevPeriod)
    {
        DoHilbertTransform(variables, varName, input, "Even", hilbertIdx, adjustedPrevPeriod);
    }
}