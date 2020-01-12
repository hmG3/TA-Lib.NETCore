using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Ema(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
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

            return TA_INT_EMA(startIdx, endIdx, inReal, optInTimePeriod, 2.0 / ((double) (optInTimePeriod + 1)), ref outBegIdx,
                ref outNBElement, outReal);
        }

        public static RetCode Ema(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
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

            return TA_INT_EMA(startIdx, endIdx, inReal, optInTimePeriod, 2.0 / ((double) (optInTimePeriod + 1)), ref outBegIdx,
                ref outNBElement, outReal);
        }

        public static int EmaLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return ((optInTimePeriod - 1) + ((int) Globals.unstablePeriod[5]));
        }
    }
}
