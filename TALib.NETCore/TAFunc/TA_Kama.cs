using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Kama(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            double tempReal;
            const double constMax = 2.0 / (30.0 + 1.0);
            const double constDiff = 2.0 / (2.0 + 1.0) - constMax;

            int lookbackTotal = optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Kama];
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

            double sumROC1 = default;
            int today = startIdx - lookbackTotal;
            int trailingIdx = today;
            int i = optInTimePeriod;
            while (i-- > 0)
            {
                tempReal = inReal[today++];
                tempReal -= inReal[today];
                sumROC1 += Math.Abs(tempReal);
            }

            double prevKAMA = inReal[today - 1];

            tempReal = inReal[today];
            double tempReal2 = inReal[trailingIdx++];
            double periodROC = tempReal - tempReal2;

            double trailingValue = tempReal2;
            if (sumROC1 <= periodROC || TA_IsZero(sumROC1))
            {
                tempReal = 1.0;
            }
            else
            {
                tempReal = Math.Abs(periodROC / sumROC1);
            }

            tempReal = tempReal * constDiff + constMax;
            tempReal *= tempReal;

            prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
            while (today <= startIdx)
            {
                tempReal = inReal[today];
                tempReal2 = inReal[trailingIdx++];
                periodROC = tempReal - tempReal2;

                sumROC1 -= Math.Abs(trailingValue - tempReal2);
                sumROC1 += Math.Abs(tempReal - inReal[today - 1]);

                trailingValue = tempReal2;
                if (sumROC1 <= periodROC || TA_IsZero(sumROC1))
                {
                    tempReal = 1.0;
                }
                else
                {
                    tempReal = Math.Abs(periodROC / sumROC1);
                }

                tempReal = tempReal * constDiff + constMax;
                tempReal *= tempReal;

                prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
            }

            outReal[0] = prevKAMA;
            int outIdx = 1;
            outBegIdx = today - 1;
            while (today <= endIdx)
            {
                tempReal = inReal[today];
                tempReal2 = inReal[trailingIdx++];
                periodROC = tempReal - tempReal2;

                sumROC1 -= Math.Abs(trailingValue - tempReal2);
                sumROC1 += Math.Abs(tempReal - inReal[today - 1]);

                trailingValue = tempReal2;
                if (sumROC1 <= periodROC || TA_IsZero(sumROC1))
                {
                    tempReal = 1.0;
                }
                else
                {
                    tempReal = Math.Abs(periodROC / sumROC1);
                }

                tempReal = tempReal * constDiff + constMax;
                tempReal *= tempReal;

                prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
                outReal[outIdx++] = prevKAMA;
            }

            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Kama(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            decimal tempReal;
            const decimal constMax = 2m / (30m + Decimal.One);
            const decimal constDiff = 2m / (2m + Decimal.One) - constMax;

            int lookbackTotal = optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Kama];
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

            decimal sumROC1 = default;
            int today = startIdx - lookbackTotal;
            int trailingIdx = today;
            int i = optInTimePeriod;
            while (i-- > 0)
            {
                tempReal = inReal[today++];
                tempReal -= inReal[today];
                sumROC1 += Math.Abs(tempReal);
            }

            decimal prevKAMA = inReal[today - 1];

            tempReal = inReal[today];
            decimal tempReal2 = inReal[trailingIdx++];
            decimal periodROC = tempReal - tempReal2;

            decimal trailingValue = tempReal2;
            if (sumROC1 <= periodROC || TA_IsZero(sumROC1))
            {
                tempReal = Decimal.One;
            }
            else
            {
                tempReal = Math.Abs(periodROC / sumROC1);
            }

            tempReal = tempReal * constDiff + constMax;
            tempReal *= tempReal;

            prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
            while (today <= startIdx)
            {
                tempReal = inReal[today];
                tempReal2 = inReal[trailingIdx++];
                periodROC = tempReal - tempReal2;

                sumROC1 -= Math.Abs(trailingValue - tempReal2);
                sumROC1 += Math.Abs(tempReal - inReal[today - 1]);

                trailingValue = tempReal2;
                if (sumROC1 <= periodROC || TA_IsZero(sumROC1))
                {
                    tempReal = Decimal.One;
                }
                else
                {
                    tempReal = Math.Abs(periodROC / sumROC1);
                }

                tempReal = tempReal * constDiff + constMax;
                tempReal *= tempReal;

                prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
            }

            outReal[0] = prevKAMA;
            int outIdx = 1;
            outBegIdx = today - 1;
            while (today <= endIdx)
            {
                tempReal = inReal[today];
                tempReal2 = inReal[trailingIdx++];
                periodROC = tempReal - tempReal2;

                sumROC1 -= Math.Abs(trailingValue - tempReal2);
                sumROC1 += Math.Abs(tempReal - inReal[today - 1]);

                trailingValue = tempReal2;
                if (sumROC1 <= periodROC || TA_IsZero(sumROC1))
                {
                    tempReal = Decimal.One;
                }
                else
                {
                    tempReal = Math.Abs(periodROC / sumROC1);
                }

                tempReal = tempReal * constDiff + constMax;
                tempReal *= tempReal;

                prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
                outReal[outIdx++] = prevKAMA;
            }

            outNBElement = outIdx;

            return RetCode.Success;
        }

        private static int KamaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Kama];
        }
    }
}
