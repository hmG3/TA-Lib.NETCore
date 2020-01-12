using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Trix(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            int nbElement = 0;
            int begIdx = 0;
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
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int emaLookback = EmaLookback(optInTimePeriod);
            int rocLookback = RocRLookback(1);
            int totalLookback = (emaLookback * 3) + rocLookback;
            if (startIdx < totalLookback)
            {
                startIdx = totalLookback;
            }

            if (startIdx > endIdx)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int nbElementToOutput = ((endIdx - startIdx) + 1) + totalLookback;
            double[] tempBuffer = new double[nbElementToOutput];
            if (tempBuffer == null)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return RetCode.AllocErr;
            }

            double k = 2.0 / ((double) (optInTimePeriod + 1));
            RetCode retCode = TA_INT_EMA(startIdx - totalLookback, endIdx, inReal, optInTimePeriod, k, ref begIdx, ref nbElement,
                tempBuffer);
            if ((retCode != RetCode.Success) || (nbElement == 0))
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput--;
            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, ref begIdx, ref nbElement, tempBuffer);
            if ((retCode != RetCode.Success) || (nbElement == 0))
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, ref begIdx, ref nbElement, tempBuffer);
            if ((retCode != RetCode.Success) || (nbElement == 0))
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = Roc(0, nbElementToOutput, tempBuffer, 1, ref begIdx, ref outNBElement, outReal);
            if ((retCode != RetCode.Success) || (outNBElement == 0))
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            return RetCode.Success;
        }

        public static RetCode Trix(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            int nbElement = 0;
            int begIdx = 0;
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
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int emaLookback = EmaLookback(optInTimePeriod);
            int rocLookback = RocRLookback(1);
            int totalLookback = (emaLookback * 3) + rocLookback;
            if (startIdx < totalLookback)
            {
                startIdx = totalLookback;
            }

            if (startIdx > endIdx)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int nbElementToOutput = ((endIdx - startIdx) + 1) + totalLookback;
            double[] tempBuffer = new double[nbElementToOutput];
            if (tempBuffer == null)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return RetCode.AllocErr;
            }

            double k = 2.0 / ((double) (optInTimePeriod + 1));
            RetCode retCode = TA_INT_EMA(startIdx - totalLookback, endIdx, inReal, optInTimePeriod, k, ref begIdx, ref nbElement,
                tempBuffer);
            if ((retCode != RetCode.Success) || (nbElement == 0))
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput--;
            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, ref begIdx, ref nbElement, tempBuffer);
            if ((retCode != RetCode.Success) || (nbElement == 0))
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, ref begIdx, ref nbElement, tempBuffer);
            if ((retCode != RetCode.Success) || (nbElement == 0))
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = Roc(0, nbElementToOutput, tempBuffer, 1, ref begIdx, ref outNBElement, outReal);
            if ((retCode != RetCode.Success) || (outNBElement == 0))
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            return RetCode.Success;
        }

        public static int TrixLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return ((EmaLookback(optInTimePeriod) * 3) + RocRLookback(1));
        }
    }
}
