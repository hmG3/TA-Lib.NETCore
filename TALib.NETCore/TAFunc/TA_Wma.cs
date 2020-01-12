using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Wma(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            double tempReal;
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

            if (optInTimePeriod == 1)
            {
                outBegIdx = startIdx;
                outNBElement = (endIdx - startIdx) + 1;
                Array.Copy(inReal, startIdx, outReal, 0, outNBElement);
                return RetCode.Success;
            }

            int divider = (optInTimePeriod * (optInTimePeriod + 1)) >> 1;
            int outIdx = 0;
            int trailingIdx = startIdx - lookbackTotal;
            double periodSub = 0.0;
            double periodSum = periodSub;
            int inIdx = trailingIdx;
            int i = 1;
            while (true)
            {
                if (inIdx >= startIdx)
                {
                    break;
                }

                tempReal = inReal[inIdx];
                inIdx++;
                periodSub += tempReal;
                periodSum += tempReal * i;
                i++;
            }

            double trailingValue = 0.0;
            while (true)
            {
                if (inIdx > endIdx)
                {
                    break;
                }

                tempReal = inReal[inIdx];
                inIdx++;
                periodSub += tempReal;
                periodSub -= trailingValue;
                periodSum += tempReal * optInTimePeriod;
                trailingValue = inReal[trailingIdx];
                trailingIdx++;
                outReal[outIdx] = periodSum / ((double) divider);
                outIdx++;
                periodSum -= periodSub;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Wma(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            double tempReal;
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

            if (optInTimePeriod == 1)
            {
                outBegIdx = startIdx;
                outNBElement = (endIdx - startIdx) + 1;
                Array.Copy(inReal, startIdx, outReal, 0, outNBElement);
                return RetCode.Success;
            }

            int divider = (optInTimePeriod * (optInTimePeriod + 1)) >> 1;
            int outIdx = 0;
            int trailingIdx = startIdx - lookbackTotal;
            double periodSub = 0.0;
            double periodSum = periodSub;
            int inIdx = trailingIdx;
            int i = 1;
            while (true)
            {
                if (inIdx >= startIdx)
                {
                    break;
                }

                tempReal = inReal[inIdx];
                inIdx++;
                periodSub += tempReal;
                periodSum += tempReal * i;
                i++;
            }

            double trailingValue = 0.0;
            while (true)
            {
                if (inIdx > endIdx)
                {
                    break;
                }

                tempReal = inReal[inIdx];
                inIdx++;
                periodSub += tempReal;
                periodSub -= trailingValue;
                periodSum += tempReal * optInTimePeriod;
                trailingValue = inReal[trailingIdx];
                trailingIdx++;
                outReal[outIdx] = periodSum / ((double) divider);
                outIdx++;
                periodSum -= periodSub;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int WmaLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return (optInTimePeriod - 1);
        }
    }
}
