using System.Collections.Generic;

namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    private static T TA_EPSILON = T.CreateChecked(0.00000000000001);

    private static T TTwo = T.CreateChecked(2);
    private static T TThree = T.CreateChecked(3);
    private static T TFour = T.CreateChecked(4);
    private static T TNinety = T.CreateChecked(90);
    private static T THundred = T.CreateChecked(100);

    private static Core.RetCode TA_INT_EMA(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod, T optInK1)
    {
        outBegIdx = outNbElement = 0;

        int lookbackTotal = EmaLookback(optInTimePeriod);
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
            int i = optInTimePeriod;
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
        int outIdx = 1;
        while (today <= endIdx)
        {
            prevMA = (inReal[today++] - prevMA) * optInK1 + prevMA;
            outReal[outIdx++] = prevMA;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    private static Core.RetCode TA_INT_MACD(T[] inReal, int startIdx, int endIdx, T[] outMacd, T[] outMacdSignal,
            T[] outMacdHist, out int outBegIdx, out int outNbElement, int optInFastPeriod, int optInSlowPeriod, int optInSignalPeriod)
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

        int lookbackSignal = EmaLookback(optInSignalPeriod);
        int lookbackTotal = MacdLookback(optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
        var fastEMABuffer = new T[tempInteger];
        var slowEMABuffer = new T[tempInteger];

        tempInteger = startIdx - lookbackSignal;
        Core.RetCode retCode = TA_INT_EMA(inReal, tempInteger, endIdx, slowEMABuffer, out var outBegIdx1, out var outNbElement1,
            optInSlowPeriod, k1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        retCode = TA_INT_EMA(inReal, tempInteger, endIdx, fastEMABuffer, out var outBegIdx2, out var outNbElement2, optInFastPeriod,
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

        Array.Copy(fastEMABuffer, lookbackSignal, outMacd, 0, endIdx - startIdx + 1);
        retCode = TA_INT_EMA(fastEMABuffer, 0, outNbElement1 - 1, outMacdSignal, out _, out outNbElement2, optInSignalPeriod,
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

    private static Core.RetCode TA_INT_PO(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
            out int outNbElement, int optInFastPeriod, int optInSlowPeriod, Core.MAType optInMethod, T[] tempBuffer,
            bool doPercentageOutput)
    {
        outBegIdx = outNbElement = 0;

        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
        }

        Core.RetCode retCode = Ma(inReal, startIdx, endIdx, tempBuffer, out var outBegIdx2, out _, optInFastPeriod, optInMethod);
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
                outReal[i] = !TA_IsZero(tempReal) ? (tempBuffer[j] - tempReal) / tempReal * THundred : T.Zero;
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

    private static Core.RetCode TA_INT_SMA(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod)
    {
        outBegIdx = outNbElement = 0;

        int lookbackTotal = SmaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T periodTotal = T.Zero;
        int trailingIdx = startIdx - lookbackTotal;
        int i = trailingIdx;
        if (optInTimePeriod > 1)
        {
            while (i < startIdx)
            {
                periodTotal += inReal[i++];
            }
        }

        int outIdx = default;
        var tOptInTimePeriod = T.CreateChecked(optInTimePeriod);
        do
        {
            periodTotal += inReal[i++];
            T tempReal = periodTotal;
            periodTotal -= inReal[trailingIdx++];
            outReal[outIdx++] = tempReal / tOptInTimePeriod;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    private static void TA_INT_StdDevUsingPrecalcMA(T[] inReal, T[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            T[] outReal, int optInTimePeriod)
    {
        int startSum = inMovAvgBegIdx + 1 - optInTimePeriod;
        int endSum = inMovAvgBegIdx;
        T periodTotal2 = T.Zero;
        for (var outIdx = startSum; outIdx < endSum; outIdx++)
        {
            T tempReal = inReal[outIdx];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
        }

        var tOptInTimePeriod = T.CreateChecked(optInTimePeriod);
        for (var outIdx = 0; outIdx < inMovAvgNbElement; outIdx++, startSum++, endSum++)
        {
            T tempReal = inReal[endSum];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            T meanValue2 = periodTotal2 / tOptInTimePeriod;

            tempReal = inReal[startSum];
            tempReal *= tempReal;
            periodTotal2 -= tempReal;

            tempReal = inMovAvg[outIdx];
            tempReal *= tempReal;
            meanValue2 -= tempReal;

            outReal[outIdx] = !TA_IsZeroOrNeg(meanValue2) ? T.Sqrt(meanValue2) : T.Zero;
        }
    }

    private static Core.RetCode TA_INT_VAR(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
    {
        outBegIdx = outNbElement = 0;

        int lookbackTotal = VarLookback(optInTimePeriod);
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
        int trailingIdx = startIdx - lookbackTotal;
        int i = trailingIdx;
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
        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);
        do
        {
            T tempReal = inReal[i++];
            periodTotal1 += tempReal;
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            T meanValue1 = periodTotal1 / tOptInTimePeriod;
            T meanValue2 = periodTotal2 / tOptInTimePeriod;
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

    private static bool TA_IsZero(T v)=> -TA_EPSILON < v && v < TA_EPSILON;

    private static bool TA_IsZeroOrNeg(T v)=> v < TA_EPSILON;

    private static void TrueRange(T th, T tl, T yc, out T @out)
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

    private static void DoPriceWma(T[] real, ref int idx, ref T periodWMASub, ref T periodWMASum,
        ref T trailingWMAValue, T varNewPrice, out T varToStoreSmoothedValue)
    {
        periodWMASub += varNewPrice;
        periodWMASub -= trailingWMAValue;
        periodWMASum += varNewPrice * TFour;
        trailingWMAValue = real[idx++];
        varToStoreSmoothedValue = periodWMASum * T.CreateChecked(0.1);
        periodWMASum -= periodWMASub;
    }

    private static void CalcTerms(T[] inLow, T[] inHigh, T[] inClose, int day, out T trueRange,
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

    private static IDictionary<string, T> InitHilbertVariables()
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

    private static void DoHilbertTransform(IDictionary<string, T> variables, string varName, T input, string oddOrEvenId,
        int hilbertIdx, T adjustedPrevPeriod)
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

    private static void DoHilbertOdd(IDictionary<string, T> variables, string varName, T input, int hilbertIdx,
        T adjustedPrevPeriod)
    {
        DoHilbertTransform(variables, varName, input, "Odd", hilbertIdx, adjustedPrevPeriod);
    }

    private static void DoHilbertEven(IDictionary<string, T> variables, string varName, T input, int hilbertIdx,
        T adjustedPrevPeriod)
    {
        DoHilbertTransform(variables, varName, input, "Even", hilbertIdx, adjustedPrevPeriod);
    }
}
