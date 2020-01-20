namespace TALib
{
    public partial class Core
    {
        public static RetCode Macd(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outMACD,
            double[] outMACDSignal, double[] outMACDHist, int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
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

            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 ||
                optInSignalPeriod < 1 || optInSignalPeriod > 100000)
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

            return TA_INT_MACD(startIdx, endIdx, inReal, optInFastPeriod, optInSlowPeriod, optInSignalPeriod, ref outBegIdx,
                ref outNBElement, outMACD, outMACDSignal, outMACDHist);
        }

        public static RetCode Macd(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outMACD,
            decimal[] outMACDSignal, decimal[] outMACDHist, int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
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

            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 ||
                optInSignalPeriod < 1 || optInSignalPeriod > 100000)
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

            return TA_INT_MACD(startIdx, endIdx, inReal, optInFastPeriod, optInSlowPeriod, optInSignalPeriod, ref outBegIdx,
                ref outNBElement, outMACD, outMACDSignal, outMACDHist);
        }

        public static int MacdLookback(int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 ||
                optInSignalPeriod < 1 || optInSignalPeriod > 100000)
            {
                return -1;
            }
            if (optInSlowPeriod < optInFastPeriod)
            {
                int tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;
            }

            return EmaLookback(optInSlowPeriod) + EmaLookback(optInSignalPeriod);
        }
    }
}
