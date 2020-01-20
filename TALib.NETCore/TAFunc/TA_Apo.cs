namespace TALib
{
    public partial class Core
    {
        public static RetCode Apo(int startIdx, int endIdx, double[] inReal, MAType optInMAType, ref int outBegIdx, ref int outNBElement,
            double[] outReal, int optInFastPeriod = 12, int optInSlowPeriod = 26)
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

            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            var tempBuffer = new double[endIdx - startIdx + 1];

            return TA_INT_PO(startIdx, endIdx, inReal, optInFastPeriod, optInSlowPeriod, optInMAType, ref outBegIdx, ref outNBElement,
                outReal, tempBuffer, 0);
        }

        public static RetCode Apo(int startIdx, int endIdx, decimal[] inReal, MAType optInMAType, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, int optInFastPeriod = 12, int optInSlowPeriod = 26)
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

            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            var tempBuffer = new decimal[endIdx - startIdx + 1];

            return TA_INT_PO(startIdx, endIdx, inReal, optInFastPeriod, optInSlowPeriod, optInMAType, ref outBegIdx, ref outNBElement,
                outReal, tempBuffer, 0);
        }

        public static int ApoLookback(MAType optInMAType, int optInFastPeriod = 12, int optInSlowPeriod = 26)
        {
            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
            {
                return -1;
            }

            return MovingAverageLookback(optInMAType, optInSlowPeriod <= optInFastPeriod ? optInFastPeriod : optInSlowPeriod);
        }
    }
}
