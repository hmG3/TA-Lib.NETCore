using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Trima(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 30)
        {
            int i;
            double tempReal;
            double numerator;
            double numeratorAdd;
            double numeratorSub;
            int middleIdx;
            int trailingIdx;
            int todayIdx;
            double factor;
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

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = optInTimePeriod - 1;
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

            int outIdx;
            if (optInTimePeriod % 2 != 1)
            {
                i = optInTimePeriod >> 1;
                factor = i * (i + 1);
                factor = 1.0 / factor;
                trailingIdx = startIdx - lookbackTotal;
                middleIdx = trailingIdx + i - 1;
                todayIdx = middleIdx + i;
                numerator = 0.0;
                numeratorSub = 0.0;
                i = middleIdx;
                while (i >= trailingIdx)
                {
                    tempReal = inReal[i];
                    numeratorSub += tempReal;
                    numerator += numeratorSub;
                    i--;
                }

                numeratorAdd = 0.0;
                middleIdx++;
                for (i = middleIdx; i <= todayIdx; i++)
                {
                    tempReal = inReal[i];
                    numeratorAdd += tempReal;
                    numerator += numeratorAdd;
                }

                outIdx = 0;
                tempReal = inReal[trailingIdx];
                trailingIdx++;
                outReal[outIdx] = numerator * factor;
                outIdx++;
                todayIdx++;
                while (todayIdx <= endIdx)
                {
                    numerator -= numeratorSub;
                    numeratorSub -= tempReal;
                    tempReal = inReal[middleIdx];
                    middleIdx++;
                    numeratorSub += tempReal;
                    numeratorAdd -= tempReal;
                    numerator += numeratorAdd;
                    tempReal = inReal[todayIdx];
                    todayIdx++;
                    numeratorAdd += tempReal;
                    numerator += tempReal;
                    tempReal = inReal[trailingIdx];
                    trailingIdx++;
                    outReal[outIdx] = numerator * factor;
                    outIdx++;
                }
            }
            else
            {
                i = optInTimePeriod >> 1;
                factor = (i + 1) * (i + 1);
                factor = 1.0 / factor;
                trailingIdx = startIdx - lookbackTotal;
                middleIdx = trailingIdx + i;
                todayIdx = middleIdx + i;
                numerator = 0.0;
                numeratorSub = 0.0;
                for (i = middleIdx; i >= trailingIdx; i--)
                {
                    tempReal = inReal[i];
                    numeratorSub += tempReal;
                    numerator += numeratorSub;
                }

                numeratorAdd = 0.0;
                middleIdx++;
                for (i = middleIdx; i <= todayIdx; i++)
                {
                    tempReal = inReal[i];
                    numeratorAdd += tempReal;
                    numerator += numeratorAdd;
                }

                outIdx = 0;
                tempReal = inReal[trailingIdx];
                trailingIdx++;
                outReal[outIdx] = numerator * factor;
                outIdx++;
                todayIdx++;
                while (todayIdx <= endIdx)
                {
                    numerator -= numeratorSub;
                    numeratorSub -= tempReal;
                    tempReal = inReal[middleIdx];
                    middleIdx++;
                    numeratorSub += tempReal;
                    numerator += numeratorAdd;
                    numeratorAdd -= tempReal;
                    tempReal = inReal[todayIdx];
                    todayIdx++;
                    numeratorAdd += tempReal;
                    numerator += tempReal;
                    tempReal = inReal[trailingIdx];
                    trailingIdx++;
                    outReal[outIdx] = numerator * factor;
                    outIdx++;
                }
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Trima(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 30)
        {
            int i;
            decimal tempReal;
            decimal numerator;
            decimal numeratorAdd;
            decimal numeratorSub;
            int middleIdx;
            int trailingIdx;
            int todayIdx;
            decimal factor;
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

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = optInTimePeriod - 1;
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

            int outIdx;
            if (optInTimePeriod % 2 != 1)
            {
                i = optInTimePeriod >> 1;
                factor = i * (i + 1);
                factor = Decimal.One / factor;
                trailingIdx = startIdx - lookbackTotal;
                middleIdx = trailingIdx + i - 1;
                todayIdx = middleIdx + i;
                numerator = Decimal.Zero;
                numeratorSub = Decimal.Zero;
                i = middleIdx;
                while (i >= trailingIdx)
                {
                    tempReal = inReal[i];
                    numeratorSub += tempReal;
                    numerator += numeratorSub;
                    i--;
                }

                numeratorAdd = Decimal.Zero;
                middleIdx++;
                for (i = middleIdx; i <= todayIdx; i++)
                {
                    tempReal = inReal[i];
                    numeratorAdd += tempReal;
                    numerator += numeratorAdd;
                }

                outIdx = 0;
                tempReal = inReal[trailingIdx];
                trailingIdx++;
                outReal[outIdx] = numerator * factor;
                outIdx++;
                todayIdx++;
                while (todayIdx <= endIdx)
                {
                    numerator -= numeratorSub;
                    numeratorSub -= tempReal;
                    tempReal = inReal[middleIdx];
                    middleIdx++;
                    numeratorSub += tempReal;
                    numeratorAdd -= tempReal;
                    numerator += numeratorAdd;
                    tempReal = inReal[todayIdx];
                    todayIdx++;
                    numeratorAdd += tempReal;
                    numerator += tempReal;
                    tempReal = inReal[trailingIdx];
                    trailingIdx++;
                    outReal[outIdx] = numerator * factor;
                    outIdx++;
                }
            }
            else
            {
                i = optInTimePeriod >> 1;
                factor = (i + 1) * (i + 1);
                factor = Decimal.One / factor;
                trailingIdx = startIdx - lookbackTotal;
                middleIdx = trailingIdx + i;
                todayIdx = middleIdx + i;
                numerator = Decimal.Zero;
                numeratorSub = Decimal.Zero;
                for (i = middleIdx; i >= trailingIdx; i--)
                {
                    tempReal = inReal[i];
                    numeratorSub += tempReal;
                    numerator += numeratorSub;
                }

                numeratorAdd = Decimal.Zero;
                middleIdx++;
                for (i = middleIdx; i <= todayIdx; i++)
                {
                    tempReal = inReal[i];
                    numeratorAdd += tempReal;
                    numerator += numeratorAdd;
                }

                outIdx = 0;
                tempReal = inReal[trailingIdx];
                trailingIdx++;
                outReal[outIdx] = numerator * factor;
                outIdx++;
                todayIdx++;
                while (todayIdx <= endIdx)
                {
                    numerator -= numeratorSub;
                    numeratorSub -= tempReal;
                    tempReal = inReal[middleIdx];
                    middleIdx++;
                    numeratorSub += tempReal;
                    numerator += numeratorAdd;
                    numeratorAdd -= tempReal;
                    tempReal = inReal[todayIdx];
                    todayIdx++;
                    numeratorAdd += tempReal;
                    numerator += tempReal;
                    tempReal = inReal[trailingIdx];
                    trailingIdx++;
                    outReal[outIdx] = numerator * factor;
                    outIdx++;
                }
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int TrimaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
