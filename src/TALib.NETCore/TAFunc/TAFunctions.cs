using System.Collections.Generic;

namespace TALib;

public static partial class Functions
{
    private const double TA_EPSILON = 0.00000000000001;

    private static Core.RetCode TA_INT_EMA(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod, double optInK1)
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
        double prevMA;
        if (Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Default)
        {
            today = startIdx - lookbackTotal;
            int i = optInTimePeriod;
            double tempReal = default;
            while (i-- > 0)
            {
                tempReal += inReal[today++];
            }

            prevMA = tempReal / optInTimePeriod;
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

    private static Core.RetCode TA_INT_EMA(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod, decimal optInK1)
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
        decimal prevMA;
        if (Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Default)
        {
            today = startIdx - lookbackTotal;
            int i = optInTimePeriod;
            decimal tempReal = default;
            while (i-- > 0)
            {
                tempReal += inReal[today++];
            }

            prevMA = tempReal / optInTimePeriod;
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

    private static Core.RetCode TA_INT_MACD(double[] inReal, int startIdx, int endIdx, double[] outMacd, double[] outMacdSignal,
            double[] outMacdHist, out int outBegIdx, out int outNbElement, int optInFastPeriod, int optInSlowPeriod, int optInSignalPeriod)
    {
        outBegIdx = outNbElement = 0;

        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
        }

        double k1;
        double k2;
        if (optInSlowPeriod != 0)
        {
            k1 = 2.0 / (optInSlowPeriod + 1);
        }
        else
        {
            optInSlowPeriod = 26;
            k1 = 0.075;
        }

        if (optInFastPeriod != 0)
        {
            k2 = 2.0 / (optInFastPeriod + 1);
        }
        else
        {
            optInFastPeriod = 12;
            k2 = 0.15;
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
        var fastEMABuffer = new double[tempInteger];
        var slowEMABuffer = new double[tempInteger];

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
            2.0 / (optInSignalPeriod + 1));
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

    private static Core.RetCode TA_INT_MACD(decimal[] inReal, int startIdx, int endIdx, decimal[] outMacd, decimal[] outMacdSignal,
            decimal[] outMacdHist, out int outBegIdx, out int outNbElement, int optInFastPeriod, int optInSlowPeriod, int optInSignalPeriod)
    {
        outBegIdx = outNbElement = 0;

        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
        }

        decimal k1;
        decimal k2;
        if (optInSlowPeriod != 0)
        {
            k1 = 2m / (optInSlowPeriod + 1);
        }
        else
        {
            optInSlowPeriod = 26;
            k1 = 0.075m;
        }

        if (optInFastPeriod != 0)
        {
            k2 = 2m / (optInFastPeriod + 1);
        }
        else
        {
            optInFastPeriod = 12;
            k2 = 0.15m;
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
        var fastEMABuffer = new decimal[tempInteger];
        var slowEMABuffer = new decimal[tempInteger];

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
            2m / (optInSignalPeriod + 1));
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

    private static Core.RetCode TA_INT_PO(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInFastPeriod, int optInSlowPeriod, Core.MAType optInMethod, double[] tempBuffer,
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
                double tempReal = outReal[i];
                outReal[i] = !TA_IsZero(tempReal) ? (tempBuffer[j] - tempReal) / tempReal * 100.0 : 0.0;
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

    private static Core.RetCode TA_INT_PO(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInFastPeriod, int optInSlowPeriod, Core.MAType optInMethod, decimal[] tempBuffer,
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
                decimal tempReal = outReal[i];
                outReal[i] = !TA_IsZero(tempReal) ? (tempBuffer[j] - tempReal) / tempReal * 100m : Decimal.Zero;
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

    private static Core.RetCode TA_INT_SMA(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
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

        double periodTotal = default;
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
        do
        {
            periodTotal += inReal[i++];
            double tempReal = periodTotal;
            periodTotal -= inReal[trailingIdx++];
            outReal[outIdx++] = tempReal / optInTimePeriod;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    private static Core.RetCode TA_INT_SMA(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
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

        decimal periodTotal = default;
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
        do
        {
            periodTotal += inReal[i++];
            decimal tempReal = periodTotal;
            periodTotal -= inReal[trailingIdx++];
            outReal[outIdx++] = tempReal / optInTimePeriod;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    private static void TA_INT_StdDevUsingPrecalcMA(double[] inReal, double[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            double[] outReal, int optInTimePeriod)
    {
        int startSum = inMovAvgBegIdx + 1 - optInTimePeriod;
        int endSum = inMovAvgBegIdx;
        double periodTotal2 = default;
        for (int outIdx = startSum; outIdx < endSum; outIdx++)
        {
            double tempReal = inReal[outIdx];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
        }

        for (var outIdx = 0; outIdx < inMovAvgNbElement; outIdx++, startSum++, endSum++)
        {
            double tempReal = inReal[endSum];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            double meanValue2 = periodTotal2 / optInTimePeriod;

            tempReal = inReal[startSum];
            tempReal *= tempReal;
            periodTotal2 -= tempReal;

            tempReal = inMovAvg[outIdx];
            tempReal *= tempReal;
            meanValue2 -= tempReal;

            outReal[outIdx] = !TA_IsZeroOrNeg(meanValue2) ? Math.Sqrt(meanValue2) : 0.0;
        }
    }

    private static void TA_INT_StdDevUsingPrecalcMA(decimal[] inReal, decimal[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            decimal[] outReal, int optInTimePeriod)
    {
        int startSum = inMovAvgBegIdx + 1 - optInTimePeriod;
        int endSum = inMovAvgBegIdx;
        decimal periodTotal2 = default;
        for (int outIdx = startSum; outIdx < endSum; outIdx++)
        {
            decimal tempReal = inReal[outIdx];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
        }

        for (var outIdx = 0; outIdx < inMovAvgNbElement; outIdx++, startSum++, endSum++)
        {
            decimal tempReal = inReal[endSum];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            decimal meanValue2 = periodTotal2 / optInTimePeriod;

            tempReal = inReal[startSum];
            tempReal *= tempReal;
            periodTotal2 -= tempReal;

            tempReal = inMovAvg[outIdx];
            tempReal *= tempReal;
            meanValue2 -= tempReal;

            outReal[outIdx] = !TA_IsZeroOrNeg(meanValue2) ? DecimalMath.Sqrt(meanValue2) : Decimal.Zero;
        }
    }

    private static Core.RetCode TA_INT_VAR(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
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

        double periodTotal1 = default;
        double periodTotal2 = default;
        int trailingIdx = startIdx - lookbackTotal;
        int i = trailingIdx;
        if (optInTimePeriod > 1)
        {
            while (i < startIdx)
            {
                double tempReal = inReal[i++];
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
            }
        }

        int outIdx = default;
        do
        {
            double tempReal = inReal[i++];
            periodTotal1 += tempReal;
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            double meanValue1 = periodTotal1 / optInTimePeriod;
            double meanValue2 = periodTotal2 / optInTimePeriod;
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

    private static Core.RetCode TA_INT_VAR(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
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

        decimal periodTotal1 = default;
        decimal periodTotal2 = default;
        int trailingIdx = startIdx - lookbackTotal;
        int i = trailingIdx;
        if (optInTimePeriod > 1)
        {
            while (i < startIdx)
            {
                decimal tempReal = inReal[i++];
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
            }
        }

        int outIdx = default;
        do
        {
            decimal tempReal = inReal[i++];
            periodTotal1 += tempReal;
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            decimal meanValue1 = periodTotal1 / optInTimePeriod;
            decimal meanValue2 = periodTotal2 / optInTimePeriod;
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

    private static bool TA_IsZero(double v) => -TA_EPSILON < v && v < TA_EPSILON;

    private static bool TA_IsZero(decimal v) => -(decimal) TA_EPSILON < v && v < (decimal) TA_EPSILON;

    private static bool TA_IsZeroOrNeg(double v) => v < TA_EPSILON;

    private static bool TA_IsZeroOrNeg(decimal v) => v < (decimal) TA_EPSILON;

    private static void TrueRange(double th, double tl, double yc, out double @out)
    {
        @out = th - tl;
        double tempDouble = Math.Abs(th - yc);
        if (tempDouble > @out)
        {
            @out = tempDouble;
        }

        tempDouble = Math.Abs(tl - yc);
        if (tempDouble > @out)
        {
            @out = tempDouble;
        }
    }

    private static void TrueRange(decimal th, decimal tl, decimal yc, out decimal @out)
    {
        @out = th - tl;
        decimal tempDecimal = Math.Abs(th - yc);
        if (tempDecimal > @out)
        {
            @out = tempDecimal;
        }

        tempDecimal = Math.Abs(tl - yc);
        if (tempDecimal > @out)
        {
            @out = tempDecimal;
        }
    }

    private static void DoPriceWma(double[] real, ref int idx, ref double periodWMASub, ref double periodWMASum,
        ref double trailingWMAValue, double varNewPrice, out double varToStoreSmoothedValue)
    {
        periodWMASub += varNewPrice;
        periodWMASub -= trailingWMAValue;
        periodWMASum += varNewPrice * 4.0;
        trailingWMAValue = real[idx++];
        varToStoreSmoothedValue = periodWMASum * 0.1;
        periodWMASum -= periodWMASub;
    }

    private static void DoPriceWma(decimal[] real, ref int idx, ref decimal periodWMASub, ref decimal periodWMASum,
        ref decimal trailingWMAValue, decimal varNewPrice, out decimal varToStoreSmoothedValue)
    {
        periodWMASub += varNewPrice;
        periodWMASub -= trailingWMAValue;
        periodWMASum += varNewPrice * 4m;
        trailingWMAValue = real[idx++];
        varToStoreSmoothedValue = periodWMASum * 0.1m;
        periodWMASum -= periodWMASub;
    }


    private static void CalcTerms(double[] inLow, double[] inHigh, double[] inClose, int day, out double trueRange,
        out double closeMinusTrueLow)
    {
        double tempLT = inLow[day];
        double tempHT = inHigh[day];
        double tempCY = inClose[day - 1];
        double trueLow = Math.Min(tempLT, tempCY);
        closeMinusTrueLow = inClose[day] - trueLow;
        trueRange = tempHT - tempLT;
        double tempDouble = Math.Abs(tempCY - tempHT);
        if (tempDouble > trueRange)
        {
            trueRange = tempDouble;
        }

        tempDouble = Math.Abs(tempCY - tempLT);
        if (tempDouble > trueRange)
        {
            trueRange = tempDouble;
        }
    }

    private static void CalcTerms(decimal[] inLow, decimal[] inHigh, decimal[] inClose, int day, out decimal trueRange,
        out decimal closeMinusTrueLow)
    {
        decimal tempLT = inLow[day];
        decimal tempHT = inHigh[day];
        decimal tempCY = inClose[day - 1];
        decimal trueLow = Math.Min(tempLT, tempCY);
        closeMinusTrueLow = inClose[day] - trueLow;
        trueRange = tempHT - tempLT;
        decimal tempDecimal = Math.Abs(tempCY - tempHT);
        if (tempDecimal > trueRange)
        {
            trueRange = tempDecimal;
        }

        tempDecimal = Math.Abs(tempCY - tempLT);
        if (tempDecimal > trueRange)
        {
            trueRange = tempDecimal;
        }
    }

    private static IDictionary<string, T> InitHilbertVariables<T>() where T : struct, IComparable<T>
    {
        var variables = new Dictionary<string, T>(4 * 11);

        new List<string> { "detrender", "q1", "jI", "jQ" }.ForEach(varName =>
        {
            variables.Add($"{varName}Odd0", default);
            variables.Add($"{varName}Odd1", default);
            variables.Add($"{varName}Odd2", default);
            variables.Add($"{varName}Even0", default);
            variables.Add($"{varName}Even1", default);
            variables.Add($"{varName}Even2", default);
            variables.Add(varName, default);
            variables.Add($"prev{varName}Odd", default);
            variables.Add($"prev{varName}Even", default);
            variables.Add($"prev{varName}InputOdd", default);
            variables.Add($"prev{varName}InputEven", default);
        });

        return variables;
    }

    private static void DoHilbertTransform(IDictionary<string, double> variables, string varName, double input, string oddOrEvenId,
        int hilbertIdx, double adjustedPrevPeriod)
    {
        const double a = 0.0962;
        const double b = 0.5769;

        double hilbertTempDouble = a * input;
        variables[varName] = -variables[$"{varName}{oddOrEvenId}{hilbertIdx}"];
        variables[$"{varName}{oddOrEvenId}{hilbertIdx}"] = hilbertTempDouble;
        variables[varName] += hilbertTempDouble;
        variables[varName] -= variables[$"prev{varName}{oddOrEvenId}"];
        variables[$"prev{varName}{oddOrEvenId}"] = b * variables[$"prev{varName}Input{oddOrEvenId}"];
        variables[varName] += variables[$"prev{varName}{oddOrEvenId}"];
        variables[$"prev{varName}Input{oddOrEvenId}"] = input;
        variables[varName] *= adjustedPrevPeriod;
    }

    private static void DoHilbertTransform(IDictionary<string, decimal> variables, string varName, decimal input, string oddOrEvenId,
        int hilbertIdx, decimal adjustedPrevPeriod)
    {
        const decimal a = 0.0962m;
        const decimal b = 0.5769m;

        decimal hilbertTempDecimal = a * input;
        variables[varName] = -variables[$"{varName}{oddOrEvenId}{hilbertIdx}"];
        variables[$"{varName}{oddOrEvenId}{hilbertIdx}"] = hilbertTempDecimal;
        variables[varName] += hilbertTempDecimal;
        variables[varName] -= variables[$"prev{varName}{oddOrEvenId}"];
        variables[$"prev{varName}{oddOrEvenId}"] = b * variables[$"prev{varName}Input{oddOrEvenId}"];
        variables[varName] += variables[$"prev{varName}{oddOrEvenId}"];
        variables[$"prev{varName}Input{oddOrEvenId}"] = input;
        variables[varName] *= adjustedPrevPeriod;
    }

    private static void DoHilbertOdd(IDictionary<string, double> variables, string varName, double input, int hilbertIdx,
        double adjustedPrevPeriod)
    {
        DoHilbertTransform(variables, varName, input, "Odd", hilbertIdx, adjustedPrevPeriod);
    }

    private static void DoHilbertOdd(IDictionary<string, decimal> variables, string varName, decimal input, int hilbertIdx,
        decimal adjustedPrevPeriod)
    {
        DoHilbertTransform(variables, varName, input, "Odd", hilbertIdx, adjustedPrevPeriod);
    }

    private static void DoHilbertEven(IDictionary<string, double> variables, string varName, double input, int hilbertIdx,
        double adjustedPrevPeriod)
    {
        DoHilbertTransform(variables, varName, input, "Even", hilbertIdx, adjustedPrevPeriod);
    }

    private static void DoHilbertEven(IDictionary<string, decimal> variables, string varName, decimal input, int hilbertIdx,
        decimal adjustedPrevPeriod)
    {
        DoHilbertTransform(variables, varName, input, "Even", hilbertIdx, adjustedPrevPeriod);
    }
}
