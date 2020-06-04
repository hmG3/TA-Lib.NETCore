namespace TALib
{
    public partial class Core
    {
        public static RetCode MacdFix(int startIdx, int endIdx, double[] inReal, out int outBegIdx, out int outNbElement, double[] outMacd,
            double[] outMacdSignal, double[] outMacdHist, int optInSignalPeriod = 9)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMacd == null || outMacdSignal == null || outMacdHist == null || optInSignalPeriod < 1 ||
                optInSignalPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            return TA_INT_MACD(startIdx, endIdx, inReal, 0, 0, optInSignalPeriod, out outBegIdx, out outNbElement, outMacd, outMacdSignal,
                outMacdHist);
        }

        public static RetCode MacdFix(int startIdx, int endIdx, decimal[] inReal, out int outBegIdx, out int outNbElement,
            decimal[] outMacd, decimal[] outMacdSignal, decimal[] outMacdHist, int optInSignalPeriod = 9)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMacd == null || outMacdSignal == null || outMacdHist == null || optInSignalPeriod < 1 ||
                optInSignalPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            return TA_INT_MACD(startIdx, endIdx, inReal, 0, 0, optInSignalPeriod, out outBegIdx, out outNbElement, outMacd, outMacdSignal,
                outMacdHist);
        }

        public static int MacdFixLookback(int optInSignalPeriod) => EmaLookback(26) + EmaLookback(optInSignalPeriod);
    }
}
