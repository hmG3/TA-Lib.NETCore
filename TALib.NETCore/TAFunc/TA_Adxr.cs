using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Adxr(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod,
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

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int adxrLookback = AdxrLookback(optInTimePeriod);
            if (startIdx < adxrLookback)
            {
                startIdx = adxrLookback;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double[] adx = new double[(endIdx - startIdx) + optInTimePeriod];
            if (adx == null)
            {
                return RetCode.AllocErr;
            }

            RetCode retCode = Adx(startIdx - (optInTimePeriod - 1), endIdx, inHigh, inLow, inClose, optInTimePeriod, ref outBegIdx,
                ref outNBElement, adx);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            int i = optInTimePeriod - 1;
            int j = 0;
            int outIdx = 0;
            int nbElement = (endIdx - startIdx) + 2;
            while (true)
            {
                nbElement--;
                if (nbElement == 0)
                {
                    break;
                }

                outReal[outIdx] = (adx[i] + adx[j]) / 2.0;
                outIdx++;
                j++;
                i++;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode Adxr(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInTimePeriod,
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

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int adxrLookback = AdxrLookback(optInTimePeriod);
            if (startIdx < adxrLookback)
            {
                startIdx = adxrLookback;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double[] adx = new double[(endIdx - startIdx) + optInTimePeriod];
            if (adx == null)
            {
                return RetCode.AllocErr;
            }

            RetCode retCode = Adx(startIdx - (optInTimePeriod - 1), endIdx, inHigh, inLow, inClose, optInTimePeriod, ref outBegIdx,
                ref outNBElement, adx);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            int i = optInTimePeriod - 1;
            int j = 0;
            int outIdx = 0;
            int nbElement = (endIdx - startIdx) + 2;
            while (true)
            {
                nbElement--;
                if (nbElement == 0)
                {
                    break;
                }

                outReal[outIdx] = (adx[i] + adx[j]) / 2.0;
                outIdx++;
                j++;
                i++;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int AdxrLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            if (optInTimePeriod > 1)
            {
                return ((optInTimePeriod + AdxLookback(optInTimePeriod)) - 1);
            }

            return 3;
        }
    }
}
