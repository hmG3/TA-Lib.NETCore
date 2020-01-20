using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode HtTrendMode(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement,
            int[] outInteger)
        {
            double smoothedValue;
            const double a = 0.0962;
            const double b = 0.5769;
            var detrenderOdd = new double[3];
            var detrenderEven = new double[3];
            var q1Odd = new double[3];
            var q1Even = new double[3];
            var jIOdd = new double[3];
            var jIEven = new double[3];
            var jQOdd = new double[3];
            var jQEven = new double[3];
            int smoothPriceIdx = default;
            const int maxIdxSmoothPrice = 49;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            var smoothPrice = new double[maxIdxSmoothPrice + 1];

            double iTrend3 = default;
            double iTrend2 = iTrend3;
            double iTrend1 = iTrend2;
            int daysInTrend = default;
            double dcPhase = default;
            double prevDCPhase = dcPhase;
            double sine = default;
            double prevSine = sine;
            double leadSine = default;
            double prevLeadSine = leadSine;
            double tempReal = Math.Atan(1.0);
            double rad2Deg = 45.0 / tempReal;
            double deg2Rad = 1.0 / rad2Deg;
            double constDeg2RadBy360 = tempReal * 8.0;
            int lookbackTotal = (int) Globals.UnstablePeriod[(int) FuncUnstId.HtTrendMode] + 63;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int trailingWMAIdx = startIdx - lookbackTotal;
            int today = trailingWMAIdx;
            tempReal = inReal[today];
            today++;
            double periodWMASub = tempReal;
            double periodWMASum = tempReal;
            tempReal = inReal[today];
            today++;
            periodWMASub += tempReal;
            periodWMASum += tempReal * 2.0;
            tempReal = inReal[today];
            today++;
            periodWMASub += tempReal;
            periodWMASum += tempReal * 3.0;
            double trailingWMAValue = default;
            int i = 34;
            do
            {
                tempReal = inReal[today];
                today++;
                periodWMASub += tempReal;
                periodWMASub -= trailingWMAValue;
                periodWMASum += tempReal * 4.0;
                trailingWMAValue = inReal[trailingWMAIdx];
                trailingWMAIdx++;
                smoothedValue = periodWMASum * 0.1;
                periodWMASum -= periodWMASub;
                i--;
            } while (i != 0);

            int hilbertIdx = default;
            double prevDetrenderOdd = default;
            double prevDetrenderEven = default;
            double prevDetrenderInputOdd = default;
            double prevDetrenderInputEven = default;
            double prevQ1Odd = default;
            double prevQ1Even = default;
            double prevQ1InputOdd = default;
            double prevQ1InputEven = default;
            double prevJIOdd = default;
            double prevJIEven = default;
            double prevJIInputOdd = default;
            double prevJIInputEven = default;
            double prevJQOdd = default;
            double prevJQEven = default;
            double prevJQInputOdd = default;
            double prevJQInputEven = default;
            double period = default;
            int outIdx = default;
            double prevQ2 = default;
            double prevI2 = prevQ2;
            double im = default;
            double re = im;
            double i1ForEvenPrev3 = default;
            double i1ForOddPrev3 = i1ForEvenPrev3;
            double i1ForEvenPrev2 = default;
            double i1ForOddPrev2 = i1ForEvenPrev2;
            double smoothPeriod = default;
            i = 0;
            while (i < 50)
            {
                smoothPrice[i] = 0.0;
                i++;
            }

            dcPhase = 0.0;
            while (true)
            {
                double hilbertTempReal;
                double i2;
                double q2;
                if (today > endIdx)
                {
                    outNBElement = outIdx;
                    return RetCode.Success;
                }

                double adjustedPrevPeriod = 0.075 * period + 0.54;
                double todayValue = inReal[today];
                periodWMASub += todayValue;
                periodWMASub -= trailingWMAValue;
                periodWMASum += todayValue * 4.0;
                trailingWMAValue = inReal[trailingWMAIdx];
                trailingWMAIdx++;
                smoothedValue = periodWMASum * 0.1;
                periodWMASum -= periodWMASub;
                smoothPrice[smoothPriceIdx] = smoothedValue;
                double detrender;
                double q1;
                double jI;
                double jQ;
                if (today % 2 == 0)
                {
                    hilbertTempReal = a * smoothedValue;
                    detrender = -detrenderEven[hilbertIdx];
                    detrenderEven[hilbertIdx] = hilbertTempReal;
                    detrender += hilbertTempReal;
                    detrender -= prevDetrenderEven;
                    prevDetrenderEven = b * prevDetrenderInputEven;
                    detrender += prevDetrenderEven;
                    prevDetrenderInputEven = smoothedValue;
                    detrender *= adjustedPrevPeriod;
                    hilbertTempReal = a * detrender;
                    q1 = -q1Even[hilbertIdx];
                    q1Even[hilbertIdx] = hilbertTempReal;
                    q1 += hilbertTempReal;
                    q1 -= prevQ1Even;
                    prevQ1Even = b * prevQ1InputEven;
                    q1 += prevQ1Even;
                    prevQ1InputEven = detrender;
                    q1 *= adjustedPrevPeriod;
                    hilbertTempReal = a * i1ForEvenPrev3;
                    jI = -jIEven[hilbertIdx];
                    jIEven[hilbertIdx] = hilbertTempReal;
                    jI += hilbertTempReal;
                    jI -= prevJIEven;
                    prevJIEven = b * prevJIInputEven;
                    jI += prevJIEven;
                    prevJIInputEven = i1ForEvenPrev3;
                    jI *= adjustedPrevPeriod;
                    hilbertTempReal = a * q1;
                    jQ = -jQEven[hilbertIdx];
                    jQEven[hilbertIdx] = hilbertTempReal;
                    jQ += hilbertTempReal;
                    jQ -= prevJQEven;
                    prevJQEven = b * prevJQInputEven;
                    jQ += prevJQEven;
                    prevJQInputEven = q1;
                    jQ *= adjustedPrevPeriod;
                    hilbertIdx++;
                    if (hilbertIdx == 3)
                    {
                        hilbertIdx = 0;
                    }

                    q2 = 0.2 * (q1 + jI) + 0.8 * prevQ2;
                    i2 = 0.2 * (i1ForEvenPrev3 - jQ) + 0.8 * prevI2;
                    i1ForOddPrev3 = i1ForOddPrev2;
                    i1ForOddPrev2 = detrender;
                }
                else
                {
                    hilbertTempReal = a * smoothedValue;
                    detrender = -detrenderOdd[hilbertIdx];
                    detrenderOdd[hilbertIdx] = hilbertTempReal;
                    detrender += hilbertTempReal;
                    detrender -= prevDetrenderOdd;
                    prevDetrenderOdd = b * prevDetrenderInputOdd;
                    detrender += prevDetrenderOdd;
                    prevDetrenderInputOdd = smoothedValue;
                    detrender *= adjustedPrevPeriod;
                    hilbertTempReal = a * detrender;
                    q1 = -q1Odd[hilbertIdx];
                    q1Odd[hilbertIdx] = hilbertTempReal;
                    q1 += hilbertTempReal;
                    q1 -= prevQ1Odd;
                    prevQ1Odd = b * prevQ1InputOdd;
                    q1 += prevQ1Odd;
                    prevQ1InputOdd = detrender;
                    q1 *= adjustedPrevPeriod;
                    hilbertTempReal = a * i1ForOddPrev3;
                    jI = -jIOdd[hilbertIdx];
                    jIOdd[hilbertIdx] = hilbertTempReal;
                    jI += hilbertTempReal;
                    jI -= prevJIOdd;
                    prevJIOdd = b * prevJIInputOdd;
                    jI += prevJIOdd;
                    prevJIInputOdd = i1ForOddPrev3;
                    jI *= adjustedPrevPeriod;
                    hilbertTempReal = a * q1;
                    jQ = -jQOdd[hilbertIdx];
                    jQOdd[hilbertIdx] = hilbertTempReal;
                    jQ += hilbertTempReal;
                    jQ -= prevJQOdd;
                    prevJQOdd = b * prevJQInputOdd;
                    jQ += prevJQOdd;
                    prevJQInputOdd = q1;
                    jQ *= adjustedPrevPeriod;
                    q2 = 0.2 * (q1 + jI) + 0.8 * prevQ2;
                    i2 = 0.2 * (i1ForOddPrev3 - jQ) + 0.8 * prevI2;
                    i1ForEvenPrev3 = i1ForEvenPrev2;
                    i1ForEvenPrev2 = detrender;
                }

                re = 0.2 * (i2 * prevI2 + q2 * prevQ2) + 0.8 * re;
                im = 0.2 * (i2 * prevQ2 - q2 * prevI2) + 0.8 * im;
                prevQ2 = q2;
                prevI2 = i2;
                tempReal = period;
                if (!im.Equals(0.0) && !re.Equals(0.0))
                {
                    period = 360.0 / (Math.Atan(im / re) * rad2Deg);
                }

                double tempReal2 = 1.5 * tempReal;
                if (period > tempReal2)
                {
                    period = tempReal2;
                }

                tempReal2 = 0.67 * tempReal;
                if (period < tempReal2)
                {
                    period = tempReal2;
                }

                if (period < 6.0)
                {
                    period = 6.0;
                }
                else if (period > 50.0)
                {
                    period = 50.0;
                }

                period = 0.2 * period + 0.8 * tempReal;
                smoothPeriod = 0.33 * period + 0.67 * smoothPeriod;
                prevDCPhase = dcPhase;
                double dcPeriod = smoothPeriod + 0.5;
                int dcPeriodInt = (int) dcPeriod;
                double realPart = default;
                double imagPart = default;
                int idx = smoothPriceIdx;
                i = 0;
                while (i < dcPeriodInt)
                {
                    tempReal = i * constDeg2RadBy360 / dcPeriodInt;
                    tempReal2 = smoothPrice[idx];
                    realPart += Math.Sin(tempReal) * tempReal2;
                    imagPart += Math.Cos(tempReal) * tempReal2;
                    if (idx == 0)
                    {
                        idx = 49;
                    }
                    else
                    {
                        idx--;
                    }

                    i++;
                }

                tempReal = Math.Abs(imagPart);
                if (tempReal > 0.0)
                {
                    dcPhase = Math.Atan(realPart / imagPart) * rad2Deg;
                }
                else if (tempReal <= 0.01)
                {
                    if (realPart < 0.0)
                    {
                        dcPhase -= 90.0;
                    }
                    else if (realPart > 0.0)
                    {
                        dcPhase += 90.0;
                    }
                }

                dcPhase += 90.0;
                dcPhase += 360.0 / smoothPeriod;
                if (imagPart < 0.0)
                {
                    dcPhase += 180.0;
                }

                if (dcPhase > 315.0)
                {
                    dcPhase -= 360.0;
                }

                prevSine = sine;
                prevLeadSine = leadSine;
                sine = Math.Sin(dcPhase * deg2Rad);
                leadSine = Math.Sin((dcPhase + 45.0) * deg2Rad);
                dcPeriod = smoothPeriod + 0.5;
                dcPeriodInt = (int) dcPeriod;
                idx = today;
                tempReal = 0.0;
                for (i = 0; i < dcPeriodInt; i++)
                {
                    tempReal += inReal[idx];
                    idx--;
                }

                if (dcPeriodInt > 0)
                {
                    tempReal /= dcPeriodInt;
                }

                double trendline = (4.0 * tempReal + 3.0 * iTrend1 + 2.0 * iTrend2 + iTrend3) / 10.0;
                iTrend3 = iTrend2;
                iTrend2 = iTrend1;
                iTrend1 = tempReal;
                int trend = 1;
                if (sine > leadSine && prevSine <= prevLeadSine ||
                    sine < leadSine && prevSine >= prevLeadSine)
                {
                    daysInTrend = 0;
                    trend = 0;
                }

                daysInTrend++;
                if (daysInTrend < 0.5 * smoothPeriod)
                {
                    trend = 0;
                }

                tempReal = dcPhase - prevDCPhase;
                if (!smoothPeriod.Equals(0.0) && tempReal > 241.20000000000002 / smoothPeriod &&
                    tempReal < 540.0 / smoothPeriod)
                {
                    trend = 0;
                }

                tempReal = smoothPrice[smoothPriceIdx];
                if (!trendline.Equals(0.0) && Math.Abs((tempReal - trendline) / trendline) >= 0.015)
                {
                    trend = 1;
                }

                if (today >= startIdx)
                {
                    outInteger[outIdx] = trend;
                    outIdx++;
                }

                smoothPriceIdx++;
                if (smoothPriceIdx > maxIdxSmoothPrice)
                {
                    smoothPriceIdx = 0;
                }

                today++;
            }
        }

        public static RetCode HtTrendMode(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement,
            int[] outInteger)
        {
            decimal smoothedValue;
            const decimal a = 0.0962m;
            const decimal b = 0.5769m;
            var detrenderOdd = new decimal[3];
            var detrenderEven = new decimal[3];
            var q1Odd = new decimal[3];
            var q1Even = new decimal[3];
            var jIOdd = new decimal[3];
            var jIEven = new decimal[3];
            var jQOdd = new decimal[3];
            var jQEven = new decimal[3];
            int smoothPriceIdx = default;
            const int maxIdxSmoothPrice = 49;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            var smoothPrice = new decimal[maxIdxSmoothPrice + 1];

            decimal iTrend3 = default;
            decimal iTrend2 = iTrend3;
            decimal iTrend1 = iTrend2;
            int daysInTrend = default;
            decimal dcPhase = default;
            decimal prevDCPhase = dcPhase;
            decimal sine = default;
            decimal prevSine = sine;
            decimal leadSine = default;
            decimal prevLeadSine = leadSine;
            decimal tempReal = DecimalMath.Atan(Decimal.One);
            decimal rad2Deg = 45m / tempReal;
            decimal deg2Rad = 1m / rad2Deg;
            decimal constDeg2RadBy360 = tempReal * 8m;
            int lookbackTotal = (int) Globals.UnstablePeriod[(int) FuncUnstId.HtTrendMode] + 63;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int trailingWMAIdx = startIdx - lookbackTotal;
            int today = trailingWMAIdx;
            tempReal = inReal[today];
            today++;
            decimal periodWMASub = tempReal;
            decimal periodWMASum = tempReal;
            tempReal = inReal[today];
            today++;
            periodWMASub += tempReal;
            periodWMASum += tempReal * 2m;
            tempReal = inReal[today];
            today++;
            periodWMASub += tempReal;
            periodWMASum += tempReal * 3m;
            decimal trailingWMAValue = default;
            int i = 34;
            do
            {
                tempReal = inReal[today];
                today++;
                periodWMASub += tempReal;
                periodWMASub -= trailingWMAValue;
                periodWMASum += tempReal * 4m;
                trailingWMAValue = inReal[trailingWMAIdx];
                trailingWMAIdx++;
                smoothedValue = periodWMASum * 0.1m;
                periodWMASum -= periodWMASub;
                i--;
            } while (i != 0);

            int hilbertIdx = default;
            decimal prevDetrenderOdd = default;
            decimal prevDetrenderEven = default;
            decimal prevDetrenderInputOdd = default;
            decimal prevDetrenderInputEven = default;
            decimal prevQ1Odd = default;
            decimal prevQ1Even = default;
            decimal prevQ1InputOdd = default;
            decimal prevQ1InputEven = default;
            decimal prevJIOdd = default;
            decimal prevJIEven = default;
            decimal prevJIInputOdd = default;
            decimal prevJIInputEven = default;
            decimal prevJQOdd = default;
            decimal prevJQEven = default;
            decimal prevJQInputOdd = default;
            decimal prevJQInputEven = default;
            decimal period = default;
            int outIdx = default;
            decimal prevQ2 = default;
            decimal prevI2 = prevQ2;
            decimal im = default;
            decimal re = im;
            decimal i1ForEvenPrev3 = default;
            decimal i1ForOddPrev3 = i1ForEvenPrev3;
            decimal i1ForEvenPrev2 = default;
            decimal i1ForOddPrev2 = i1ForEvenPrev2;
            decimal smoothPeriod = default;
            i = 0;
            while (i < 50)
            {
                smoothPrice[i] = Decimal.Zero;
                i++;
            }

            dcPhase = Decimal.Zero;
            while (true)
            {
                decimal hilbertTempReal;
                decimal i2;
                decimal q2;
                if (today > endIdx)
                {
                    outNBElement = outIdx;
                    return RetCode.Success;
                }

                decimal adjustedPrevPeriod = 0.075m * period + 0.54m;
                decimal todayValue = inReal[today];
                periodWMASub += todayValue;
                periodWMASub -= trailingWMAValue;
                periodWMASum += todayValue * 4m;
                trailingWMAValue = inReal[trailingWMAIdx];
                trailingWMAIdx++;
                smoothedValue = periodWMASum * 0.1m;
                periodWMASum -= periodWMASub;
                smoothPrice[smoothPriceIdx] = smoothedValue;
                decimal detrender;
                decimal q1;
                decimal jI;
                decimal jQ;
                if (today % 2 == 0)
                {
                    hilbertTempReal = a * smoothedValue;
                    detrender = -detrenderEven[hilbertIdx];
                    detrenderEven[hilbertIdx] = hilbertTempReal;
                    detrender += hilbertTempReal;
                    detrender -= prevDetrenderEven;
                    prevDetrenderEven = b * prevDetrenderInputEven;
                    detrender += prevDetrenderEven;
                    prevDetrenderInputEven = smoothedValue;
                    detrender *= adjustedPrevPeriod;
                    hilbertTempReal = a * detrender;
                    q1 = -q1Even[hilbertIdx];
                    q1Even[hilbertIdx] = hilbertTempReal;
                    q1 += hilbertTempReal;
                    q1 -= prevQ1Even;
                    prevQ1Even = b * prevQ1InputEven;
                    q1 += prevQ1Even;
                    prevQ1InputEven = detrender;
                    q1 *= adjustedPrevPeriod;
                    hilbertTempReal = a * i1ForEvenPrev3;
                    jI = -jIEven[hilbertIdx];
                    jIEven[hilbertIdx] = hilbertTempReal;
                    jI += hilbertTempReal;
                    jI -= prevJIEven;
                    prevJIEven = b * prevJIInputEven;
                    jI += prevJIEven;
                    prevJIInputEven = i1ForEvenPrev3;
                    jI *= adjustedPrevPeriod;
                    hilbertTempReal = a * q1;
                    jQ = -jQEven[hilbertIdx];
                    jQEven[hilbertIdx] = hilbertTempReal;
                    jQ += hilbertTempReal;
                    jQ -= prevJQEven;
                    prevJQEven = b * prevJQInputEven;
                    jQ += prevJQEven;
                    prevJQInputEven = q1;
                    jQ *= adjustedPrevPeriod;
                    hilbertIdx++;
                    if (hilbertIdx == 3)
                    {
                        hilbertIdx = 0;
                    }

                    q2 = 0.2m * (q1 + jI) + 0.8m * prevQ2;
                    i2 = 0.2m * (i1ForEvenPrev3 - jQ) + 0.8m * prevI2;
                    i1ForOddPrev3 = i1ForOddPrev2;
                    i1ForOddPrev2 = detrender;
                }
                else
                {
                    hilbertTempReal = a * smoothedValue;
                    detrender = -detrenderOdd[hilbertIdx];
                    detrenderOdd[hilbertIdx] = hilbertTempReal;
                    detrender += hilbertTempReal;
                    detrender -= prevDetrenderOdd;
                    prevDetrenderOdd = b * prevDetrenderInputOdd;
                    detrender += prevDetrenderOdd;
                    prevDetrenderInputOdd = smoothedValue;
                    detrender *= adjustedPrevPeriod;
                    hilbertTempReal = a * detrender;
                    q1 = -q1Odd[hilbertIdx];
                    q1Odd[hilbertIdx] = hilbertTempReal;
                    q1 += hilbertTempReal;
                    q1 -= prevQ1Odd;
                    prevQ1Odd = b * prevQ1InputOdd;
                    q1 += prevQ1Odd;
                    prevQ1InputOdd = detrender;
                    q1 *= adjustedPrevPeriod;
                    hilbertTempReal = a * i1ForOddPrev3;
                    jI = -jIOdd[hilbertIdx];
                    jIOdd[hilbertIdx] = hilbertTempReal;
                    jI += hilbertTempReal;
                    jI -= prevJIOdd;
                    prevJIOdd = b * prevJIInputOdd;
                    jI += prevJIOdd;
                    prevJIInputOdd = i1ForOddPrev3;
                    jI *= adjustedPrevPeriod;
                    hilbertTempReal = a * q1;
                    jQ = -jQOdd[hilbertIdx];
                    jQOdd[hilbertIdx] = hilbertTempReal;
                    jQ += hilbertTempReal;
                    jQ -= prevJQOdd;
                    prevJQOdd = b * prevJQInputOdd;
                    jQ += prevJQOdd;
                    prevJQInputOdd = q1;
                    jQ *= adjustedPrevPeriod;
                    q2 = 0.2m * (q1 + jI) + 0.8m * prevQ2;
                    i2 = 0.2m * (i1ForOddPrev3 - jQ) + 0.8m * prevI2;
                    i1ForEvenPrev3 = i1ForEvenPrev2;
                    i1ForEvenPrev2 = detrender;
                }

                re = 0.2m * (i2 * prevI2 + q2 * prevQ2) + 0.8m * re;
                im = 0.2m * (i2 * prevQ2 - q2 * prevI2) + 0.8m * im;
                prevQ2 = q2;
                prevI2 = i2;
                tempReal = period;
                if (im != Decimal.Zero && re != Decimal.Zero)
                {
                    period = 360m / (DecimalMath.Atan(im / re) * rad2Deg);
                }

                decimal tempReal2 = 1.5m * tempReal;
                if (period > tempReal2)
                {
                    period = tempReal2;
                }

                tempReal2 = 0.67m * tempReal;
                if (period < tempReal2)
                {
                    period = tempReal2;
                }

                if (period < 6m)
                {
                    period = 6m;
                }
                else if (period > 50m)
                {
                    period = 50m;
                }

                period = 0.2m * period + 0.8m * tempReal;
                smoothPeriod = 0.33m * period + 0.67m * smoothPeriod;
                prevDCPhase = dcPhase;
                decimal dcPeriod = smoothPeriod + 0.5m;
                int dcPeriodInt = (int) dcPeriod;
                decimal realPart = default;
                decimal imagPart = default;
                int idx = smoothPriceIdx;
                i = 0;
                while (i < dcPeriodInt)
                {
                    tempReal = i * constDeg2RadBy360 / dcPeriodInt;
                    tempReal2 = smoothPrice[idx];
                    realPart += DecimalMath.Sin(tempReal) * tempReal2;
                    imagPart += DecimalMath.Cos(tempReal) * tempReal2;
                    if (idx == 0)
                    {
                        idx = 49;
                    }
                    else
                    {
                        idx--;
                    }

                    i++;
                }

                tempReal = Math.Abs(imagPart);
                if (tempReal > Decimal.Zero)
                {
                    dcPhase = DecimalMath.Atan(realPart / imagPart) * rad2Deg;
                }
                else if (tempReal <= 0.01m)
                {
                    if (realPart < Decimal.Zero)
                    {
                        dcPhase -= 90m;
                    }
                    else if (realPart > Decimal.Zero)
                    {
                        dcPhase += 90;
                    }
                }

                dcPhase += 90m;
                dcPhase += 360m / smoothPeriod;
                if (imagPart < Decimal.Zero)
                {
                    dcPhase += 180m;
                }

                if (dcPhase > 315m)
                {
                    dcPhase -= 360m;
                }

                prevSine = sine;
                prevLeadSine = leadSine;
                sine = DecimalMath.Sin(dcPhase * deg2Rad);
                leadSine = DecimalMath.Sin((dcPhase + 45m) * deg2Rad);
                dcPeriod = smoothPeriod + 0.5m;
                dcPeriodInt = (int) dcPeriod;
                idx = today;
                tempReal = Decimal.Zero;
                for (i = 0; i < dcPeriodInt; i++)
                {
                    tempReal += inReal[idx];
                    idx--;
                }

                if (dcPeriodInt > 0)
                {
                    tempReal /= dcPeriodInt;
                }

                decimal trendline = (4m * tempReal + 3m * iTrend1 + 2m * iTrend2 + iTrend3) / 10m;
                iTrend3 = iTrend2;
                iTrend2 = iTrend1;
                iTrend1 = tempReal;
                int trend = 1;
                if (sine > leadSine && prevSine <= prevLeadSine ||
                    sine < leadSine && prevSine >= prevLeadSine)
                {
                    daysInTrend = 0;
                    trend = 0;
                }

                daysInTrend++;
                if (daysInTrend < 0.5m * smoothPeriod)
                {
                    trend = 0;
                }

                tempReal = dcPhase - prevDCPhase;
                if (smoothPeriod != Decimal.Zero && tempReal > 241.20000000000002m / smoothPeriod &&
                    tempReal < 540m / smoothPeriod)
                {
                    trend = 0;
                }

                tempReal = smoothPrice[smoothPriceIdx];
                if (trendline != Decimal.Zero && Math.Abs((tempReal - trendline) / trendline) >= 0.015m)
                {
                    trend = 1;
                }

                if (today >= startIdx)
                {
                    outInteger[outIdx] = trend;
                    outIdx++;
                }

                smoothPriceIdx++;
                if (smoothPriceIdx > maxIdxSmoothPrice)
                {
                    smoothPriceIdx = 0;
                }

                today++;
            }
        }

        public static int HtTrendModeLookback()
        {
            return (int) Globals.UnstablePeriod[(int) FuncUnstId.HtTrendMode] + 63;
        }
    }
}
