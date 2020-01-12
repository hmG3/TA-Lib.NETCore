using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Kama(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            double tempReal;
            const double constMax = 0.064516129032258063;
            const double constDiff = 0.66666666666666663 - constMax;
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

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
            int lookbackTotal = optInTimePeriod + ((int) Globals.unstablePeriod[12]);
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

            double sumROC1 = 0.0;
            int today = startIdx - lookbackTotal;
            int trailingIdx = today;
            int i = optInTimePeriod;
            while (true)
            {
                i--;
                if (i <= 0)
                {
                    break;
                }

                tempReal = inReal[today];
                today++;
                tempReal -= inReal[today];
                sumROC1 += Math.Abs(tempReal);
            }

            double prevKAMA = inReal[today - 1];
            tempReal = inReal[today];
            double tempReal2 = inReal[trailingIdx];
            trailingIdx++;
            double periodROC = tempReal - tempReal2;
            double trailingValue = tempReal2;
            if ((sumROC1 <= periodROC) || ((-1E-08 < sumROC1) && (sumROC1 < 1E-08)))
            {
                tempReal = 1.0;
            }
            else
            {
                tempReal = Math.Abs((double) (periodROC / sumROC1));
            }

            tempReal = (tempReal * constDiff) + constMax;
            tempReal *= tempReal;
            prevKAMA = ((inReal[today] - prevKAMA) * tempReal) + prevKAMA;
            today++;
            while (true)
            {
                if (today > startIdx)
                {
                    break;
                }

                tempReal = inReal[today];
                tempReal2 = inReal[trailingIdx];
                trailingIdx++;
                periodROC = tempReal - tempReal2;
                sumROC1 -= Math.Abs((double) (trailingValue - tempReal2));
                sumROC1 += Math.Abs((double) (tempReal - inReal[today - 1]));
                trailingValue = tempReal2;
                if ((sumROC1 <= periodROC) || ((-1E-08 < sumROC1) && (sumROC1 < 1E-08)))
                {
                    tempReal = 1.0;
                }
                else
                {
                    tempReal = Math.Abs((double) (periodROC / sumROC1));
                }

                tempReal = (tempReal * constDiff) + constMax;
                tempReal *= tempReal;
                prevKAMA = ((inReal[today] - prevKAMA) * tempReal) + prevKAMA;
                today++;
            }

            outReal[0] = prevKAMA;
            int outIdx = 1;
            outBegIdx = today - 1;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                tempReal = inReal[today];
                tempReal2 = inReal[trailingIdx];
                trailingIdx++;
                periodROC = tempReal - tempReal2;
                sumROC1 -= Math.Abs((double) (trailingValue - tempReal2));
                sumROC1 += Math.Abs((double) (tempReal - inReal[today - 1]));
                trailingValue = tempReal2;
                if ((sumROC1 <= periodROC) || ((-1E-08 < sumROC1) && (sumROC1 < 1E-08)))
                {
                    tempReal = 1.0;
                }
                else
                {
                    tempReal = Math.Abs((double) (periodROC / sumROC1));
                }

                tempReal = (tempReal * constDiff) + constMax;
                tempReal *= tempReal;
                prevKAMA = ((inReal[today] - prevKAMA) * tempReal) + prevKAMA;
                today++;
                outReal[outIdx] = prevKAMA;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode Kama(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            double tempReal;
            const double constMax = 0.064516129032258063;
            const double constDiff = 0.66666666666666663 - constMax;
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

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
            int lookbackTotal = optInTimePeriod + ((int) Globals.unstablePeriod[12]);
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

            double sumROC1 = 0.0;
            int today = startIdx - lookbackTotal;
            int trailingIdx = today;
            int i = optInTimePeriod;
            while (true)
            {
                i--;
                if (i <= 0)
                {
                    break;
                }

                tempReal = inReal[today];
                today++;
                tempReal -= inReal[today];
                sumROC1 += Math.Abs(tempReal);
            }

            double prevKAMA = inReal[today - 1];
            tempReal = inReal[today];
            double tempReal2 = inReal[trailingIdx];
            trailingIdx++;
            double periodROC = tempReal - tempReal2;
            double trailingValue = tempReal2;
            if ((sumROC1 <= periodROC) || ((-1E-08 < sumROC1) && (sumROC1 < 1E-08)))
            {
                tempReal = 1.0;
            }
            else
            {
                tempReal = Math.Abs((double) (periodROC / sumROC1));
            }

            tempReal = (tempReal * constDiff) + constMax;
            tempReal *= tempReal;
            prevKAMA = ((inReal[today] - prevKAMA) * tempReal) + prevKAMA;
            today++;
            while (true)
            {
                if (today > startIdx)
                {
                    break;
                }

                tempReal = inReal[today];
                tempReal2 = inReal[trailingIdx];
                trailingIdx++;
                periodROC = tempReal - tempReal2;
                sumROC1 -= Math.Abs((double) (trailingValue - tempReal2));
                sumROC1 += Math.Abs((double) (tempReal - inReal[today - 1]));
                trailingValue = tempReal2;
                if ((sumROC1 <= periodROC) || ((-1E-08 < sumROC1) && (sumROC1 < 1E-08)))
                {
                    tempReal = 1.0;
                }
                else
                {
                    tempReal = Math.Abs((double) (periodROC / sumROC1));
                }

                tempReal = (tempReal * constDiff) + constMax;
                tempReal *= tempReal;
                prevKAMA = ((inReal[today] - prevKAMA) * tempReal) + prevKAMA;
                today++;
            }

            outReal[0] = prevKAMA;
            int outIdx = 1;
            outBegIdx = today - 1;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                tempReal = inReal[today];
                tempReal2 = inReal[trailingIdx];
                trailingIdx++;
                periodROC = tempReal - tempReal2;
                sumROC1 -= Math.Abs((double) (trailingValue - tempReal2));
                sumROC1 += Math.Abs((double) (tempReal - inReal[today - 1]));
                trailingValue = tempReal2;
                if ((sumROC1 <= periodROC) || ((-1E-08 < sumROC1) && (sumROC1 < 1E-08)))
                {
                    tempReal = 1.0;
                }
                else
                {
                    tempReal = Math.Abs((double) (periodROC / sumROC1));
                }

                tempReal = (tempReal * constDiff) + constMax;
                tempReal *= tempReal;
                prevKAMA = ((inReal[today] - prevKAMA) * tempReal) + prevKAMA;
                today++;
                outReal[outIdx] = prevKAMA;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int KamaLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return (optInTimePeriod + ((int) Globals.unstablePeriod[12]));
        }
    }
}
