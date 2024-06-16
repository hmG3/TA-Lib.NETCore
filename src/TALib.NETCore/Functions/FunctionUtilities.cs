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
    private static Core.RetCode CalcExponentialMA<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod,
        T optInK1) where T : IFloatingPointIeee754<T>
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

    private static Core.RetCode CalcMACD<T>(
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
        int optInSignalPeriod) where T : IFloatingPointIeee754<T>
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
            k1 = Two<T>() / (T.CreateChecked(optInSlowPeriod) + T.One);
        }
        else
        {
            optInSlowPeriod = 26;
            k1 = T.CreateChecked(0.075);
        }

        if (optInFastPeriod != 0)
        {
            k2 = Two<T>() / (T.CreateChecked(optInFastPeriod) + T.One);
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
            Two<T>() / (T.CreateChecked(optInSignalPeriod) + T.One));
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

    private static Core.RetCode CalcPriceOscillator<T>(
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
        bool doPercentageOutput) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        // Make sure slow is really slower than the fast period! if not, swap...
        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
        }

        // Calculate the fast MA into the tempBuffer.
        var retCode = Ma(inReal, startIdx, endIdx, tempBuffer, out var outBegIdx2, out _, optInFastPeriod, optInMethod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Calculate the slow MA into the output.
        retCode = Ma(inReal, startIdx, endIdx, outReal, out var outBegIdx1, out var outNbElement1, optInSlowPeriod, optInMethod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        for (int i = 0, j = outBegIdx1 - outBegIdx2; i < outNbElement1; i++, j++)
        {
            if (doPercentageOutput)
            {
                // Calculate ((fast MA)-(slow MA))/(slow MA) in the output.
                T tempReal = outReal[i];
                outReal[i] = !T.IsZero(tempReal) ? (tempBuffer[j] - tempReal) / tempReal * Hundred<T>() : T.Zero;
            }
            else
            {
                // Calculate (fast MA)-(slow MA) in the output.
                outReal[i] = tempBuffer[j] - outReal[i];
            }
        }

        outBegIdx = outBegIdx1;
        outNbElement = outNbElement1;

        return retCode;
    }

    private static Core.RetCode CalcSimpleMA<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
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

    private static void CalcStandardDeviation<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inMovAvg,
        int inMovAvgBegIdx,
        int inMovAvgNbElement,
        Span<T> outReal,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
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

    private static Core.RetCode CalcVariance<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
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

    private static T CalcAccumulationDistribution<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        ReadOnlySpan<T> volume,
        ref int today,
        T ad) where T : IFloatingPointIeee754<T>
    {
        T h = high[today];
        T l = low[today];
        T tmp = h - l;
        T c = close[today];
        if (tmp > T.Zero)
        {
            ad += (c - l - (h - c)) / tmp * volume[today];
        }

        today++;

        return ad;
    }

    private static (int, T) CalcLowest<T>(
        ReadOnlySpan<T> input,
        int trailingIdx,
        int today,
        int lowestIdx,
        T lowest)
        where T : IFloatingPointIeee754<T>
    {
        T tmp = input[today];
        if (lowestIdx < trailingIdx)
        {
            lowestIdx = trailingIdx;
            lowest = input[lowestIdx];
            var i = lowestIdx;
            while (++i <= today)
            {
                tmp = input[i];
                if (tmp > lowest)
                {
                    continue;
                }

                lowestIdx = i;
                lowest = tmp;
            }
        }
        else if (tmp <= lowest)
        {
            lowestIdx = today;
            lowest = tmp;
        }

        return (lowestIdx, lowest);
    }

    private static (int, T) CalcHighest<T>(
        ReadOnlySpan<T> input,
        int trailingIdx,
        int today,
        int highestIdx,
        T highest)
        where T : IFloatingPointIeee754<T>
    {
        T tmp = input[today];
        if (highestIdx < trailingIdx)
        {
            highestIdx = trailingIdx;
            highest = input[highestIdx];
            var i = highestIdx;
            while (++i <= today)
            {
                tmp = input[i];
                if (tmp < highest)
                {
                    continue;
                }

                highestIdx = i;
                highest = tmp;
            }
        }
        else if (tmp >= highest)
        {
            highestIdx = today;
            highest = tmp;
        }

        return (highestIdx, highest);
    }

    private static void UpdateDMAndTR<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        ref int today,
        ref T prevHigh,
        ref T prevLow,
        ref T prevClose,
        ref T prevPlusDM,
        ref T prevMinusDM,
        ref T prevTR,
        T timePeriod,
        bool applySmoothing = true)
        where T : IFloatingPointIeee754<T>
    {
        var diffP = high[today] - prevHigh;
        var diffM = prevLow - low[today];
        prevHigh = high[today];
        prevLow = low[today];

        if (applySmoothing)
        {
            prevPlusDM -= prevPlusDM / timePeriod;
            prevMinusDM -= prevMinusDM / timePeriod;
        }

        if (diffM > T.Zero && diffP < diffM)
        {
            prevMinusDM += diffM;
        }
        else if (diffP > T.Zero && diffP > diffM)
        {
            prevPlusDM += diffP;
        }

        if (close.IsEmpty)
        {
            return;
        }

        var trueRange = TrueRange(prevHigh, prevLow, prevClose);
        prevTR = applySmoothing ? prevTR - prevTR / timePeriod + trueRange : prevTR + trueRange;
        prevClose = close[today];
    }

    private static (T minusDI, T plusDI) CalculateDI<T>(T prevMinusDM, T prevPlusDM, T prevTR) where T : IFloatingPointIeee754<T>
    {
        var minusDI = Hundred<T>() * (prevMinusDM / prevTR);
        var plusDI = Hundred<T>() * (prevPlusDM / prevTR);

        return (minusDI, plusDI);
    }

    private static T TrueRange<T>(T th, T tl, T yc) where T : IFloatingPointIeee754<T>
    {
        T range = th - tl;
        range = T.Max(range, T.Abs(th - yc));
        range = T.Max(range, T.Abs(tl - yc));

        return range;
    }

    private static void DoPriceWma<T>(
        ReadOnlySpan<T> real,
        ref int idx,
        ref T periodWMASub,
        ref T periodWMASum,
        ref T trailingWMAValue,
        T varNewPrice,
        out T varToStoreSmoothedValue) where T : IFloatingPointIeee754<T>
    {
        periodWMASub += varNewPrice;
        periodWMASub -= trailingWMAValue;
        periodWMASum += varNewPrice * Four<T>();
        trailingWMAValue = real[idx++];
        varToStoreSmoothedValue = periodWMASum * T.CreateChecked(0.1);
        periodWMASum -= periodWMASub;
    }

    private static class HTHelper
    {
        public enum HilbertKeys
        {
            Detrender = 6,
            Q1 = 17,
            JI = 28,
            JQ = 39
        }

        public static T[] HilbertBufferFactory<T>() where T : IFloatingPointIeee754<T> => new T[4 * 11];

        public static void DoHilbertOdd<T>(
            Span<T> buffer,
            HilbertKeys baseKey,
            T input,
            int hilbertIdx,
            T adjustedPrevPeriod) where T : IFloatingPointIeee754<T> =>
            DoHilbertTransform(buffer, baseKey, input, true, hilbertIdx, adjustedPrevPeriod);

        public static void DoHilbertEven<T>(
            Span<T> buffer,
            HilbertKeys baseKey,
            T input,
            int hilbertIdx,
            T adjustedPrevPeriod) where T : IFloatingPointIeee754<T> =>
            DoHilbertTransform(buffer, baseKey, input, false, hilbertIdx, adjustedPrevPeriod);

        private static void DoHilbertTransform<T>(
            Span<T> buffer,
            HilbertKeys baseKey,
            T input,
            bool isOdd,
            int hilbertIdx,
            T adjustedPrevPeriod) where T : IFloatingPointIeee754<T>
        {
            var a = T.CreateChecked(0.0962);
            var b = T.CreateChecked(0.5769);

            var hilbertTempT = a * input;

            var baseIndex = (int) baseKey;
            var hilbertIndex = baseIndex - (isOdd ? 6 : 3) + hilbertIdx;
            var prevIndex = baseIndex + (isOdd ? 1 : 2);
            var prevInputIndex = baseIndex + (isOdd ? 3 : 4);

            buffer[baseIndex] = -buffer[hilbertIndex];
            buffer[hilbertIndex] = hilbertTempT;
            buffer[baseIndex] += hilbertTempT;
            buffer[baseIndex] -= buffer[prevIndex];
            buffer[prevIndex] = b * buffer[prevInputIndex];
            buffer[baseIndex] += buffer[prevIndex];
            buffer[prevInputIndex] = input;
            buffer[baseIndex] *= adjustedPrevPeriod;
        }
    }

    private static T Two<T>() where T : IFloatingPointIeee754<T> => T.CreateChecked(2);

    private static T Three<T>() where T : IFloatingPointIeee754<T> => T.CreateChecked(3);

    private static T Four<T>() where T : IFloatingPointIeee754<T> => T.CreateChecked(4);

    private static T Ninety<T>() where T : IFloatingPointIeee754<T> => T.CreateChecked(90);

    private static T Hundred<T>() where T : IFloatingPointIeee754<T> => T.CreateChecked(100);
}
