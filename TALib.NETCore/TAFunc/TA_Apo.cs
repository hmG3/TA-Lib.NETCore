using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Apo(int startIdx, int endIdx, double[] inReal, int optInFastPeriod, int optInSlowPeriod, MAType optInMAType,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
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

            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 12;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 0x1a;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            double[] tempBuffer = new double[(endIdx - startIdx) + 1];
            if (tempBuffer == null)
            {
                return RetCode.AllocErr;
            }

            return TA_INT_PO(startIdx, endIdx, inReal, optInFastPeriod, optInSlowPeriod, optInMAType, ref outBegIdx, ref outNBElement,
                outReal, tempBuffer, 0);
        }

        public static RetCode Apo(int startIdx, int endIdx, float[] inReal, int optInFastPeriod, int optInSlowPeriod, MAType optInMAType,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
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

            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 12;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 0x1a;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            double[] tempBuffer = new double[(endIdx - startIdx) + 1];
            if (tempBuffer == null)
            {
                return RetCode.AllocErr;
            }

            return TA_INT_PO(startIdx, endIdx, inReal, optInFastPeriod, optInSlowPeriod, optInMAType, ref outBegIdx, ref outNBElement,
                outReal, tempBuffer, 0);
        }

        public static int ApoLookback(int optInFastPeriod, int optInSlowPeriod, MAType optInMAType)
        {
            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 12;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return -1;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 0x1a;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return -1;
            }

            return MovingAverageLookback((optInSlowPeriod <= optInFastPeriod) ? optInFastPeriod : optInSlowPeriod, optInMAType);
        }
    }
}
