using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Variance(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, double optInNbDev, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
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
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInNbDev == -4E+37)
            {
                optInNbDev = 1.0;
            }
            else if ((optInNbDev < -3E+37) || (optInNbDev > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            return TA_INT_VAR(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);
        }

        public static RetCode Variance(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, double optInNbDev, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
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
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInNbDev == -4E+37)
            {
                optInNbDev = 1.0;
            }
            else if ((optInNbDev < -3E+37) || (optInNbDev > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            return TA_INT_VAR(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);
        }

        public static int VarianceLookback(int optInTimePeriod, double optInNbDev)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            if (optInNbDev == -4E+37)
            {
                optInNbDev = 1.0;
            }
            else if ((optInNbDev < -3E+37) || (optInNbDev > 3E+37))
            {
                return -1;
            }

            return (optInTimePeriod - 1);
        }
    }
}
