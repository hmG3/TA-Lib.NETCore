using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode HtDcPeriod(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
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

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            double rad2Deg = 180.0 / (4.0 * Math.Atan(1.0));
            int lookbackTotal = (int) Globals.UnstablePeriod[(int) FuncUnstId.HtDcPeriod] + 32;
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
            double tempReal = inReal[today];
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
            int i = 9;
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
            while (true)
            {
                double hilbertTempReal;
                double i2;
                double q2;
                if (today > endIdx)
                {
                    break;
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
                if (today >= startIdx)
                {
                    outReal[outIdx] = smoothPeriod;
                    outIdx++;
                }

                today++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode HtDcPeriod(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal)
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

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            decimal rad2Deg = 180m / (4m * DecimalMath.Atan(Decimal.One));
            int lookbackTotal = (int) Globals.UnstablePeriod[(int) FuncUnstId.HtDcPeriod] + 32;
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
            decimal tempReal = inReal[today];
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
            int i = 9;
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
            while (true)
            {
                decimal hilbertTempReal;
                decimal I2;
                decimal Q2;
                if (today > endIdx)
                {
                    break;
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

                    Q2 = 0.2m * (q1 + jI) + 0.8m * prevQ2;
                    I2 = 0.2m * (i1ForEvenPrev3 - jQ) + 0.8m * prevI2;
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
                    Q2 = 0.2m * (q1 + jI) + 0.8m * prevQ2;
                    I2 = 0.2m * (i1ForOddPrev3 - jQ) + 0.8m * prevI2;
                    i1ForEvenPrev3 = i1ForEvenPrev2;
                    i1ForEvenPrev2 = detrender;
                }

                re = 0.2m * (I2 * prevI2 + Q2 * prevQ2) + 0.8m * re;
                im = 0.2m * (I2 * prevQ2 - Q2 * prevI2) + 0.8m * im;
                prevQ2 = Q2;
                prevI2 = I2;
                tempReal = period;
                if (im != Decimal.Zero && re != Decimal.Zero)
                {
                    period = 360m / DecimalMath.Atan(im / re) * rad2Deg;
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
                if (today >= startIdx)
                {
                    outReal[outIdx] = smoothPeriod;
                    outIdx++;
                }

                today++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int HtDcPeriodLookback()
        {
            return (int) Globals.UnstablePeriod[(int) FuncUnstId.HtDcPeriod] + 32;
        }
    }
}
