namespace TALib
{
    public partial class Core
    {
        public static RetCode Sma(int startIdx, int endIdx, double[] inReal, out int outBegIdx, out int outNbElement, double[] outReal,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            return TA_INT_SMA(startIdx, endIdx, inReal, optInTimePeriod, out outBegIdx, out outNbElement, outReal);
        }

        public static RetCode Sma(int startIdx, int endIdx, decimal[] inReal, out int outBegIdx, out int outNbElement, decimal[] outReal,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            return TA_INT_SMA(startIdx, endIdx, inReal, optInTimePeriod, out outBegIdx, out outNbElement, outReal);
        }

        public static int SmaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
