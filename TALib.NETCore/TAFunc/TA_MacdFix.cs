using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MacdFix(int startIdx, int endIdx, double[] inReal, int optInSignalPeriod, ref int outBegIdx,
            ref int outNBElement, double[] outMACD, double[] outMACDSignal, double[] outMACDHist)
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

            if (optInSignalPeriod == -2147483648)
            {
                optInSignalPeriod = 9;
            }
            else if ((optInSignalPeriod < 1) || (optInSignalPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outMACD == null)
            {
                return RetCode.BadParam;
            }

            if (outMACDSignal == null)
            {
                return RetCode.BadParam;
            }

            if (outMACDHist == null)
            {
                return RetCode.BadParam;
            }

            return TA_INT_MACD(startIdx, endIdx, inReal, 0, 0, optInSignalPeriod, ref outBegIdx, ref outNBElement, outMACD, outMACDSignal,
                outMACDHist);
        }

        public static RetCode MacdFix(int startIdx, int endIdx, float[] inReal, int optInSignalPeriod, ref int outBegIdx,
            ref int outNBElement, double[] outMACD, double[] outMACDSignal, double[] outMACDHist)
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

            if (optInSignalPeriod == -2147483648)
            {
                optInSignalPeriod = 9;
            }
            else if ((optInSignalPeriod < 1) || (optInSignalPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outMACD == null)
            {
                return RetCode.BadParam;
            }

            if (outMACDSignal == null)
            {
                return RetCode.BadParam;
            }

            if (outMACDHist == null)
            {
                return RetCode.BadParam;
            }

            return TA_INT_MACD(startIdx, endIdx, inReal, 0, 0, optInSignalPeriod, ref outBegIdx, ref outNBElement, outMACD, outMACDSignal,
                outMACDHist);
        }

        public static int MacdFixLookback(int optInSignalPeriod)
        {
            if (optInSignalPeriod == -2147483648)
            {
                optInSignalPeriod = 9;
            }
            else if ((optInSignalPeriod < 1) || (optInSignalPeriod > 0x186a0))
            {
                return -1;
            }

            return (EmaLookback(0x1a) + EmaLookback(optInSignalPeriod));
        }
    }
}
