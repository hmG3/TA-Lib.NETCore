namespace TALib
{
    public static partial class Core
    {
        public static RetCode Macd(double[] inReal, int startIdx, int endIdx, double[] outMacd, double[] outMacdSignal,
            double[] outMacdHist, out int outBegIdx, out int outNbElement, int optInFastPeriod = 12, int optInSlowPeriod = 26,
            int optInSignalPeriod = 9)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMacd == null || outMacdSignal == null || outMacdHist == null || optInFastPeriod < 2 ||
                optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 || optInSignalPeriod < 1 ||
                optInSignalPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            return TA_INT_MACD(inReal, startIdx, endIdx, outMacd, outMacdSignal, outMacdHist, out outBegIdx, out outNbElement,
                optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
        }

        public static RetCode Macd(decimal[] inReal, int startIdx, int endIdx, decimal[] outMacd, decimal[] outMacdSignal,
            decimal[] outMacdHist, out int outBegIdx, out int outNbElement, int optInFastPeriod = 12, int optInSlowPeriod = 26,
            int optInSignalPeriod = 9)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMacd == null || outMacdSignal == null || outMacdHist == null || optInFastPeriod < 2 ||
                optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 || optInSignalPeriod < 1 ||
                optInSignalPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            return TA_INT_MACD(inReal, startIdx, endIdx, outMacd, outMacdSignal, outMacdHist, out outBegIdx, out outNbElement,
                optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
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
                //TODO check
                optInFastPeriod = tempInteger;
            }

            return EmaLookback(optInSlowPeriod) + EmaLookback(optInSignalPeriod);
        }
    }
}
