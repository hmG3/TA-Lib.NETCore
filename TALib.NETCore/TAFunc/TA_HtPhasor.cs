using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode HtPhasor(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement,
            double[] outInPhase, double[] outQuadrature)
        {
            double smoothedValue;
            const double a = 0.0962;
            const double b = 0.5769;
            double[] detrender_Odd = new double[3];
            double[] detrender_Even = new double[3];
            double[] Q1_Odd = new double[3];
            double[] Q1_Even = new double[3];
            double[] jI_Odd = new double[3];
            double[] jI_Even = new double[3];
            double[] jQ_Odd = new double[3];
            double[] jQ_Even = new double[3];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (outInPhase == null)
            {
                return RetCode.BadParam;
            }

            if (outQuadrature == null)
            {
                return RetCode.BadParam;
            }

            double rad2Deg = 180.0 / (4.0 * Math.Atan(1.0));
            int lookbackTotal = ((int) Globals.unstablePeriod[8]) + 0x20;
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
            double trailingWMAValue = 0.0;
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

            int hilbertIdx = 0;
            detrender_Odd[0] = 0.0;
            detrender_Odd[1] = 0.0;
            detrender_Odd[2] = 0.0;
            detrender_Even[0] = 0.0;
            detrender_Even[1] = 0.0;
            detrender_Even[2] = 0.0;
            double detrender = 0.0;
            double prev_detrender_Odd = 0.0;
            double prev_detrender_Even = 0.0;
            double prev_detrender_input_Odd = 0.0;
            double prev_detrender_input_Even = 0.0;
            Q1_Odd[0] = 0.0;
            Q1_Odd[1] = 0.0;
            Q1_Odd[2] = 0.0;
            Q1_Even[0] = 0.0;
            Q1_Even[1] = 0.0;
            Q1_Even[2] = 0.0;
            double Q1 = 0.0;
            double prev_Q1_Odd = 0.0;
            double prev_Q1_Even = 0.0;
            double prev_Q1_input_Odd = 0.0;
            double prev_Q1_input_Even = 0.0;
            jI_Odd[0] = 0.0;
            jI_Odd[1] = 0.0;
            jI_Odd[2] = 0.0;
            jI_Even[0] = 0.0;
            jI_Even[1] = 0.0;
            jI_Even[2] = 0.0;
            double jI = 0.0;
            double prev_jI_Odd = 0.0;
            double prev_jI_Even = 0.0;
            double prev_jI_input_Odd = 0.0;
            double prev_jI_input_Even = 0.0;
            jQ_Odd[0] = 0.0;
            jQ_Odd[1] = 0.0;
            jQ_Odd[2] = 0.0;
            jQ_Even[0] = 0.0;
            jQ_Even[1] = 0.0;
            jQ_Even[2] = 0.0;
            double jQ = 0.0;
            double prev_jQ_Odd = 0.0;
            double prev_jQ_Even = 0.0;
            double prev_jQ_input_Odd = 0.0;
            double prev_jQ_input_Even = 0.0;
            double period = 0.0;
            int outIdx = 0;
            double prevQ2 = 0.0;
            double prevI2 = prevQ2;
            double Im = 0.0;
            double Re = Im;
            double I1ForEvenPrev3 = 0.0;
            double I1ForOddPrev3 = I1ForEvenPrev3;
            double I1ForEvenPrev2 = 0.0;
            double I1ForOddPrev2 = I1ForEvenPrev2;
            while (true)
            {
                double hilbertTempReal;
                double I2;
                double Q2;
                if (today > endIdx)
                {
                    break;
                }

                double adjustedPrevPeriod = (0.075 * period) + 0.54;
                double todayValue = inReal[today];
                periodWMASub += todayValue;
                periodWMASub -= trailingWMAValue;
                periodWMASum += todayValue * 4.0;
                trailingWMAValue = inReal[trailingWMAIdx];
                trailingWMAIdx++;
                smoothedValue = periodWMASum * 0.1;
                periodWMASum -= periodWMASub;
                if ((today % 2) == 0)
                {
                    hilbertTempReal = a * smoothedValue;
                    detrender = -detrender_Even[hilbertIdx];
                    detrender_Even[hilbertIdx] = hilbertTempReal;
                    detrender += hilbertTempReal;
                    detrender -= prev_detrender_Even;
                    prev_detrender_Even = b * prev_detrender_input_Even;
                    detrender += prev_detrender_Even;
                    prev_detrender_input_Even = smoothedValue;
                    detrender *= adjustedPrevPeriod;
                    hilbertTempReal = a * detrender;
                    Q1 = -Q1_Even[hilbertIdx];
                    Q1_Even[hilbertIdx] = hilbertTempReal;
                    Q1 += hilbertTempReal;
                    Q1 -= prev_Q1_Even;
                    prev_Q1_Even = b * prev_Q1_input_Even;
                    Q1 += prev_Q1_Even;
                    prev_Q1_input_Even = detrender;
                    Q1 *= adjustedPrevPeriod;
                    if (today >= startIdx)
                    {
                        outQuadrature[outIdx] = Q1;
                        outInPhase[outIdx] = I1ForEvenPrev3;
                        outIdx++;
                    }

                    hilbertTempReal = a * I1ForEvenPrev3;
                    jI = -jI_Even[hilbertIdx];
                    jI_Even[hilbertIdx] = hilbertTempReal;
                    jI += hilbertTempReal;
                    jI -= prev_jI_Even;
                    prev_jI_Even = b * prev_jI_input_Even;
                    jI += prev_jI_Even;
                    prev_jI_input_Even = I1ForEvenPrev3;
                    jI *= adjustedPrevPeriod;
                    hilbertTempReal = a * Q1;
                    jQ = -jQ_Even[hilbertIdx];
                    jQ_Even[hilbertIdx] = hilbertTempReal;
                    jQ += hilbertTempReal;
                    jQ -= prev_jQ_Even;
                    prev_jQ_Even = b * prev_jQ_input_Even;
                    jQ += prev_jQ_Even;
                    prev_jQ_input_Even = Q1;
                    jQ *= adjustedPrevPeriod;
                    hilbertIdx++;
                    if (hilbertIdx == 3)
                    {
                        hilbertIdx = 0;
                    }

                    Q2 = (0.2 * (Q1 + jI)) + (0.8 * prevQ2);
                    I2 = (0.2 * (I1ForEvenPrev3 - jQ)) + (0.8 * prevI2);
                    I1ForOddPrev3 = I1ForOddPrev2;
                    I1ForOddPrev2 = detrender;
                }
                else
                {
                    hilbertTempReal = a * smoothedValue;
                    detrender = -detrender_Odd[hilbertIdx];
                    detrender_Odd[hilbertIdx] = hilbertTempReal;
                    detrender += hilbertTempReal;
                    detrender -= prev_detrender_Odd;
                    prev_detrender_Odd = b * prev_detrender_input_Odd;
                    detrender += prev_detrender_Odd;
                    prev_detrender_input_Odd = smoothedValue;
                    detrender *= adjustedPrevPeriod;
                    hilbertTempReal = a * detrender;
                    Q1 = -Q1_Odd[hilbertIdx];
                    Q1_Odd[hilbertIdx] = hilbertTempReal;
                    Q1 += hilbertTempReal;
                    Q1 -= prev_Q1_Odd;
                    prev_Q1_Odd = b * prev_Q1_input_Odd;
                    Q1 += prev_Q1_Odd;
                    prev_Q1_input_Odd = detrender;
                    Q1 *= adjustedPrevPeriod;
                    if (today >= startIdx)
                    {
                        outQuadrature[outIdx] = Q1;
                        outInPhase[outIdx] = I1ForOddPrev3;
                        outIdx++;
                    }

                    hilbertTempReal = a * I1ForOddPrev3;
                    jI = -jI_Odd[hilbertIdx];
                    jI_Odd[hilbertIdx] = hilbertTempReal;
                    jI += hilbertTempReal;
                    jI -= prev_jI_Odd;
                    prev_jI_Odd = b * prev_jI_input_Odd;
                    jI += prev_jI_Odd;
                    prev_jI_input_Odd = I1ForOddPrev3;
                    jI *= adjustedPrevPeriod;
                    hilbertTempReal = a * Q1;
                    jQ = -jQ_Odd[hilbertIdx];
                    jQ_Odd[hilbertIdx] = hilbertTempReal;
                    jQ += hilbertTempReal;
                    jQ -= prev_jQ_Odd;
                    prev_jQ_Odd = b * prev_jQ_input_Odd;
                    jQ += prev_jQ_Odd;
                    prev_jQ_input_Odd = Q1;
                    jQ *= adjustedPrevPeriod;
                    Q2 = (0.2 * (Q1 + jI)) + (0.8 * prevQ2);
                    I2 = (0.2 * (I1ForOddPrev3 - jQ)) + (0.8 * prevI2);
                    I1ForEvenPrev3 = I1ForEvenPrev2;
                    I1ForEvenPrev2 = detrender;
                }

                Re = (0.2 * ((I2 * prevI2) + (Q2 * prevQ2))) + (0.8 * Re);
                Im = (0.2 * ((I2 * prevQ2) - (Q2 * prevI2))) + (0.8 * Im);
                prevQ2 = Q2;
                prevI2 = I2;
                tempReal = period;
                if ((Im != 0.0) && (Re != 0.0))
                {
                    period = 360.0 / (Math.Atan(Im / Re) * rad2Deg);
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

                period = (0.2 * period) + (0.8 * tempReal);
                today++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode HtPhasor(int startIdx, int endIdx, float[] inReal, ref int outBegIdx, ref int outNBElement,
            double[] outInPhase, double[] outQuadrature)
        {
            double smoothedValue;
            const double a = 0.0962;
            const double b = 0.5769;
            double[] detrender_Odd = new double[3];
            double[] detrender_Even = new double[3];
            double[] Q1_Odd = new double[3];
            double[] Q1_Even = new double[3];
            double[] jI_Odd = new double[3];
            double[] jI_Even = new double[3];
            double[] jQ_Odd = new double[3];
            double[] jQ_Even = new double[3];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (outInPhase == null)
            {
                return RetCode.BadParam;
            }

            if (outQuadrature == null)
            {
                return RetCode.BadParam;
            }

            double rad2Deg = 180.0 / (4.0 * Math.Atan(1.0));
            int lookbackTotal = ((int) Globals.unstablePeriod[8]) + 0x20;
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
            double trailingWMAValue = 0.0;
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

            int hilbertIdx = 0;
            detrender_Odd[0] = 0.0;
            detrender_Odd[1] = 0.0;
            detrender_Odd[2] = 0.0;
            detrender_Even[0] = 0.0;
            detrender_Even[1] = 0.0;
            detrender_Even[2] = 0.0;
            double detrender = 0.0;
            double prev_detrender_Odd = 0.0;
            double prev_detrender_Even = 0.0;
            double prev_detrender_input_Odd = 0.0;
            double prev_detrender_input_Even = 0.0;
            Q1_Odd[0] = 0.0;
            Q1_Odd[1] = 0.0;
            Q1_Odd[2] = 0.0;
            Q1_Even[0] = 0.0;
            Q1_Even[1] = 0.0;
            Q1_Even[2] = 0.0;
            double Q1 = 0.0;
            double prev_Q1_Odd = 0.0;
            double prev_Q1_Even = 0.0;
            double prev_Q1_input_Odd = 0.0;
            double prev_Q1_input_Even = 0.0;
            jI_Odd[0] = 0.0;
            jI_Odd[1] = 0.0;
            jI_Odd[2] = 0.0;
            jI_Even[0] = 0.0;
            jI_Even[1] = 0.0;
            jI_Even[2] = 0.0;
            double jI = 0.0;
            double prev_jI_Odd = 0.0;
            double prev_jI_Even = 0.0;
            double prev_jI_input_Odd = 0.0;
            double prev_jI_input_Even = 0.0;
            jQ_Odd[0] = 0.0;
            jQ_Odd[1] = 0.0;
            jQ_Odd[2] = 0.0;
            jQ_Even[0] = 0.0;
            jQ_Even[1] = 0.0;
            jQ_Even[2] = 0.0;
            double jQ = 0.0;
            double prev_jQ_Odd = 0.0;
            double prev_jQ_Even = 0.0;
            double prev_jQ_input_Odd = 0.0;
            double prev_jQ_input_Even = 0.0;
            double period = 0.0;
            int outIdx = 0;
            double prevQ2 = 0.0;
            double prevI2 = prevQ2;
            double Im = 0.0;
            double Re = Im;
            double I1ForEvenPrev3 = 0.0;
            double I1ForOddPrev3 = I1ForEvenPrev3;
            double I1ForEvenPrev2 = 0.0;
            double I1ForOddPrev2 = I1ForEvenPrev2;
            while (true)
            {
                double hilbertTempReal;
                double I2;
                double Q2;
                if (today > endIdx)
                {
                    break;
                }

                double adjustedPrevPeriod = (0.075 * period) + 0.54;
                double todayValue = inReal[today];
                periodWMASub += todayValue;
                periodWMASub -= trailingWMAValue;
                periodWMASum += todayValue * 4.0;
                trailingWMAValue = inReal[trailingWMAIdx];
                trailingWMAIdx++;
                smoothedValue = periodWMASum * 0.1;
                periodWMASum -= periodWMASub;
                if ((today % 2) == 0)
                {
                    hilbertTempReal = a * smoothedValue;
                    detrender = -detrender_Even[hilbertIdx];
                    detrender_Even[hilbertIdx] = hilbertTempReal;
                    detrender += hilbertTempReal;
                    detrender -= prev_detrender_Even;
                    prev_detrender_Even = b * prev_detrender_input_Even;
                    detrender += prev_detrender_Even;
                    prev_detrender_input_Even = smoothedValue;
                    detrender *= adjustedPrevPeriod;
                    hilbertTempReal = a * detrender;
                    Q1 = -Q1_Even[hilbertIdx];
                    Q1_Even[hilbertIdx] = hilbertTempReal;
                    Q1 += hilbertTempReal;
                    Q1 -= prev_Q1_Even;
                    prev_Q1_Even = b * prev_Q1_input_Even;
                    Q1 += prev_Q1_Even;
                    prev_Q1_input_Even = detrender;
                    Q1 *= adjustedPrevPeriod;
                    if (today >= startIdx)
                    {
                        outQuadrature[outIdx] = Q1;
                        outInPhase[outIdx] = I1ForEvenPrev3;
                        outIdx++;
                    }

                    hilbertTempReal = a * I1ForEvenPrev3;
                    jI = -jI_Even[hilbertIdx];
                    jI_Even[hilbertIdx] = hilbertTempReal;
                    jI += hilbertTempReal;
                    jI -= prev_jI_Even;
                    prev_jI_Even = b * prev_jI_input_Even;
                    jI += prev_jI_Even;
                    prev_jI_input_Even = I1ForEvenPrev3;
                    jI *= adjustedPrevPeriod;
                    hilbertTempReal = a * Q1;
                    jQ = -jQ_Even[hilbertIdx];
                    jQ_Even[hilbertIdx] = hilbertTempReal;
                    jQ += hilbertTempReal;
                    jQ -= prev_jQ_Even;
                    prev_jQ_Even = b * prev_jQ_input_Even;
                    jQ += prev_jQ_Even;
                    prev_jQ_input_Even = Q1;
                    jQ *= adjustedPrevPeriod;
                    hilbertIdx++;
                    if (hilbertIdx == 3)
                    {
                        hilbertIdx = 0;
                    }

                    Q2 = (0.2 * (Q1 + jI)) + (0.8 * prevQ2);
                    I2 = (0.2 * (I1ForEvenPrev3 - jQ)) + (0.8 * prevI2);
                    I1ForOddPrev3 = I1ForOddPrev2;
                    I1ForOddPrev2 = detrender;
                }
                else
                {
                    hilbertTempReal = a * smoothedValue;
                    detrender = -detrender_Odd[hilbertIdx];
                    detrender_Odd[hilbertIdx] = hilbertTempReal;
                    detrender += hilbertTempReal;
                    detrender -= prev_detrender_Odd;
                    prev_detrender_Odd = b * prev_detrender_input_Odd;
                    detrender += prev_detrender_Odd;
                    prev_detrender_input_Odd = smoothedValue;
                    detrender *= adjustedPrevPeriod;
                    hilbertTempReal = a * detrender;
                    Q1 = -Q1_Odd[hilbertIdx];
                    Q1_Odd[hilbertIdx] = hilbertTempReal;
                    Q1 += hilbertTempReal;
                    Q1 -= prev_Q1_Odd;
                    prev_Q1_Odd = b * prev_Q1_input_Odd;
                    Q1 += prev_Q1_Odd;
                    prev_Q1_input_Odd = detrender;
                    Q1 *= adjustedPrevPeriod;
                    if (today >= startIdx)
                    {
                        outQuadrature[outIdx] = Q1;
                        outInPhase[outIdx] = I1ForOddPrev3;
                        outIdx++;
                    }

                    hilbertTempReal = a * I1ForOddPrev3;
                    jI = -jI_Odd[hilbertIdx];
                    jI_Odd[hilbertIdx] = hilbertTempReal;
                    jI += hilbertTempReal;
                    jI -= prev_jI_Odd;
                    prev_jI_Odd = b * prev_jI_input_Odd;
                    jI += prev_jI_Odd;
                    prev_jI_input_Odd = I1ForOddPrev3;
                    jI *= adjustedPrevPeriod;
                    hilbertTempReal = a * Q1;
                    jQ = -jQ_Odd[hilbertIdx];
                    jQ_Odd[hilbertIdx] = hilbertTempReal;
                    jQ += hilbertTempReal;
                    jQ -= prev_jQ_Odd;
                    prev_jQ_Odd = b * prev_jQ_input_Odd;
                    jQ += prev_jQ_Odd;
                    prev_jQ_input_Odd = Q1;
                    jQ *= adjustedPrevPeriod;
                    Q2 = (0.2 * (Q1 + jI)) + (0.8 * prevQ2);
                    I2 = (0.2 * (I1ForOddPrev3 - jQ)) + (0.8 * prevI2);
                    I1ForEvenPrev3 = I1ForEvenPrev2;
                    I1ForEvenPrev2 = detrender;
                }

                Re = (0.2 * ((I2 * prevI2) + (Q2 * prevQ2))) + (0.8 * Re);
                Im = (0.2 * ((I2 * prevQ2) - (Q2 * prevI2))) + (0.8 * Im);
                prevQ2 = Q2;
                prevI2 = I2;
                tempReal = period;
                if ((Im != 0.0) && (Re != 0.0))
                {
                    period = 360.0 / (Math.Atan(Im / Re) * rad2Deg);
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

                period = (0.2 * period) + (0.8 * tempReal);
                today++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int HtPhasorLookback()
        {
            return (((int) Globals.unstablePeriod[8]) + 0x20);
        }
    }
}
