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
            var tempReal = T.Zero;
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

        // Make sure slow is really slower than the fast period. if not, swap.
        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
        }

        T k1;
        T k2;
        // Catch special case for fix 26/12 MACD.
        if (optInSlowPeriod != 0)
        {
            k1 = Two<T>() / (T.CreateChecked(optInSlowPeriod) + T.One);
        }
        else
        {
            optInSlowPeriod = 26;
            k1 = T.CreateChecked(0.075); // Fix 26
        }

        if (optInFastPeriod != 0)
        {
            k2 = Two<T>() / (T.CreateChecked(optInFastPeriod) + T.One);
        }
        else
        {
            optInFastPeriod = 12;
            k2 = T.CreateChecked(0.15); // Fix 12
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

        // Allocate intermediate buffer for fast/slow EMA.
        var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
        Span<T> fastEMABuffer = new T[tempInteger];
        Span<T> slowEMABuffer = new T[tempInteger];

        /* Calculate the slow EMA.
         *
         * Move back the startIdx to get enough data for the signal period.
         * That way, once the signal calculation is done, all the output will start at the requested 'startIdx'.
         */
        tempInteger = startIdx - lookbackSignal;
        var retCode = CalcExponentialMA(inReal, tempInteger, endIdx, slowEMABuffer, out var outBegIdx1, out var outNbElement1,
            optInSlowPeriod, k1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Calculate the fast EMA.
        retCode = CalcExponentialMA(inReal, tempInteger, endIdx, fastEMABuffer, out var outBegIdx2, out var outNbElement2, optInFastPeriod,
            k2);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Calculate (fast EMA) - (slow EMA)
        for (var i = 0; i < outNbElement1; i++)
        {
            fastEMABuffer[i] -= slowEMABuffer[i];
        }

        // Copy the result into the output for the caller.
        fastEMABuffer.Slice(lookbackSignal, endIdx - startIdx + 1).CopyTo(outMacd);

        // Calculate the signal/trigger line.
        retCode = CalcExponentialMA(fastEMABuffer, 0, outNbElement1 - 1, outMacdSignal, out _, out outNbElement2, optInSignalPeriod,
            Two<T>() / (T.CreateChecked(optInSignalPeriod) + T.One));
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Calculate the histogram.
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
                var tempReal = outReal[i];
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

        var periodTotal = T.Zero;
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
            var tempReal = periodTotal;
            periodTotal -= inReal[trailingIdx++];
            outReal[outIdx++] = tempReal / timePeriod;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
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

        var periodTotal1 = T.Zero;
        var periodTotal2 = T.Zero;
        var trailingIdx = startIdx - lookbackTotal;
        var i = trailingIdx;
        if (optInTimePeriod > 1)
        {
            while (i < startIdx)
            {
                var tempReal = inReal[i++];
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
            }
        }

        int outIdx = default;
        var timePeriod = T.CreateChecked(optInTimePeriod);
        do
        {
            var tempReal = inReal[i++];
            periodTotal1 += tempReal;
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            var meanValue1 = periodTotal1 / timePeriod;
            var meanValue2 = periodTotal2 / timePeriod;
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
        var h = high[today];
        var l = low[today];
        var tmp = h - l;
        var c = close[today];
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
        var tmp = input[today];
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
        var tmp = input[today];
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

    private static void InitDMAndTR<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        out T prevHigh,
        ref int today,
        out T prevLow,
        out T prevClose,
        T timePeriod,
        ref T prevPlusDM,
        ref T prevMinusDM,
        ref T prevTR) where T : IFloatingPointIeee754<T>
    {
        prevHigh = high[today];
        prevLow = low[today];
        prevClose = close[today];

        for (var i = Int32.CreateTruncating(timePeriod) - 1; i > 0; i--)
        {
            today++;

            UpdateDMAndTR(high, low, close, ref today, ref prevHigh, ref prevLow, ref prevClose, ref prevPlusDM, ref prevMinusDM,
                ref prevTR, timePeriod, false);
        }
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
            // Case 2 and 4: +DM = 0, -DM = diffM
            prevMinusDM += diffM;
        }
        else if (diffP > T.Zero && diffP > diffM)
        {
            // Case 1 and 3: +DM = diffP, -DM = 0
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

    private static (T minusDI, T plusDI) CalcDI<T>(T prevMinusDM, T prevPlusDM, T prevTR) where T : IFloatingPointIeee754<T>
    {
        var minusDI = Hundred<T>() * (prevMinusDM / prevTR);
        var plusDI = Hundred<T>() * (prevPlusDM / prevTR);

        return (minusDI, plusDI);
    }

    private static T TrueRange<T>(T th, T tl, T yc) where T : IFloatingPointIeee754<T>
    {
        var range = th - tl;
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
            // DetrenderOdd = 0-2
            // DetrenderEven = 3-5
            Detrender = 6,
            // PrevDetrenderOdd = 7
            // PrevDetrenderEven = 8
            // PrevDetrenderInputOdd = 9
            // PrevDetrenderInputEven = 10

            // Q1Odd = 11-13
            // Q1Even = 14-16
            Q1 = 17,
            // PrevQ1Odd = 18
            // PrevQ1Even = 19
            // PrevQ1InputOdd = 20
            // PrevQ1InputEven = 21

            // JIOdd = 22-24
            // JIEven = 25-27
            JI = 28,
            // PrevJIOdd = 29
            // PrevJIEven = 30
            // PrevJIInputOdd = 31
            // PrevJIInputEven = 32

            // JQOdd = 33-35
            // JQEven = 36-38
            JQ = 39
            // PrevJQOdd = 40
            // PrevJQEven = 41
            // PrevJQInputOdd = 42
            // PrevJQInputEven = 43
        }

        public static T[] BufferFactory<T>() where T : IFloatingPointIeee754<T> => new T[4 * 11];

        public static void CalcHilbertOdd<T>(
            Span<T> hilbertBuffer,
            T smoothedValue,
            int hilbertIdx,
            T adjustedPrevPeriod,
            out T i1ForEvenPrev3,
            T prevQ2,
            T prevI2,
            T i1ForOddPrev3,
            ref T i1ForEvenPrev2,
            out T q2,
            out T i2) where T : IFloatingPointIeee754<T>
        {
            var tPointTwo = T.CreateChecked(0.2);
            var tPointEight = T.CreateChecked(0.8);

            DoHilbertTransform(hilbertBuffer, HilbertKeys.Detrender, smoothedValue, true, hilbertIdx, adjustedPrevPeriod);
            var input = hilbertBuffer[(int) HilbertKeys.Detrender];
            DoHilbertTransform(hilbertBuffer, HilbertKeys.Q1, input, true, hilbertIdx, adjustedPrevPeriod);
            DoHilbertTransform(hilbertBuffer, HilbertKeys.JI, i1ForOddPrev3, true, hilbertIdx, adjustedPrevPeriod);
            var input1 = hilbertBuffer[(int) HilbertKeys.Q1];
            DoHilbertTransform(hilbertBuffer, HilbertKeys.JQ, input1, true, hilbertIdx, adjustedPrevPeriod);

            q2 = tPointTwo * (hilbertBuffer[(int) HilbertKeys.Q1] + hilbertBuffer[(int) HilbertKeys.JI]) + tPointEight * prevQ2;
            i2 = tPointTwo * (i1ForOddPrev3 - hilbertBuffer[(int) HilbertKeys.JQ]) + tPointEight * prevI2;

            // The variable I1 is the detrender delayed for 3 price bars.
            // Save the current detrender value for being used by the "even" logic later.
            i1ForEvenPrev3 = i1ForEvenPrev2;
            i1ForEvenPrev2 = hilbertBuffer[(int) HilbertKeys.Detrender];
        }

        public static void CalcHilbertEven<T>(
            Span<T> hilbertBuffer,
            T smoothedValue,
            ref int hilbertIdx,
            T adjustedPrevPeriod,
            T i1ForEvenPrev3,
            T prevQ2,
            T prevI2,
            out T i1ForOddPrev3,
            ref T i1ForOddPrev2,
            out T q2,
            out T i2) where T : IFloatingPointIeee754<T>
        {
            var tPointTwo = T.CreateChecked(0.2);
            var tPointEight = T.CreateChecked(0.8);

            DoHilbertTransform(hilbertBuffer, HilbertKeys.Detrender, smoothedValue, false, hilbertIdx, adjustedPrevPeriod);
            var input = hilbertBuffer[(int) HilbertKeys.Detrender];
            DoHilbertTransform(hilbertBuffer, HilbertKeys.Q1, input, false, hilbertIdx, adjustedPrevPeriod);
            DoHilbertTransform(hilbertBuffer, HilbertKeys.JI, i1ForEvenPrev3, false, hilbertIdx, adjustedPrevPeriod);
            var input1 = hilbertBuffer[(int) HilbertKeys.Q1];
            DoHilbertTransform(hilbertBuffer, HilbertKeys.JQ, input1, false, hilbertIdx, adjustedPrevPeriod);

            if (++hilbertIdx == 3)
            {
                hilbertIdx = 0;
            }

            q2 = tPointTwo * (hilbertBuffer[(int) HilbertKeys.Q1] + hilbertBuffer[(int) HilbertKeys.JI]) + tPointEight * prevQ2;
            i2 = tPointTwo * (i1ForEvenPrev3 - hilbertBuffer[(int) HilbertKeys.JQ]) + tPointEight * prevI2;

            // The variable i1 is the detrender delayed for 3 price bars.
            // Save the current detrender value for being used by the "odd" logic later.
            i1ForOddPrev3 = i1ForOddPrev2;
            i1ForOddPrev2 = hilbertBuffer[(int) HilbertKeys.Detrender];
        }

        public static void CalcSmoothedPeriod<T>(
            ref T re,
            T i2,
            T q2,
            ref T prevI2,
            ref T prevQ2,
            ref T im,
            ref T period) where T : IFloatingPointIeee754<T>
        {
            var tPointTwo = T.CreateChecked(0.2);
            var tPointEight = T.CreateChecked(0.8);

            re = tPointTwo * (i2 * prevI2 + q2 * prevQ2) + tPointEight * re;
            im = tPointTwo * (i2 * prevQ2 - q2 * prevI2) + tPointEight * im;
            prevQ2 = q2;
            prevI2 = i2;
            var tempReal1 = period;
            if (!T.IsZero(im) && !T.IsZero(re))
            {
                period = Ninety<T>() * Four<T>() / T.RadiansToDegrees(T.Atan(im / re));
            }

            var tempReal2 = T.CreateChecked(1.5) * tempReal1;
            period = T.Min(period, tempReal2);

            tempReal2 = T.CreateChecked(0.67) * tempReal1;
            period = T.Max(period, tempReal2);
            period = T.Clamp(period, T.CreateChecked(6), T.CreateChecked(50));
            period = tPointTwo * period + tPointEight * tempReal1;
        }

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
