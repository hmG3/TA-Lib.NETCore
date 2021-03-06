using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode Apo(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            MAType optInMAType = MAType.Sma, int optInFastPeriod = 12, int optInSlowPeriod = 26)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 ||
                optInSlowPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            var tempBuffer = new double[endIdx - startIdx + 1];

            return TA_INT_PO(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInFastPeriod, optInSlowPeriod,
                optInMAType, tempBuffer, false);
        }

        public static RetCode Apo(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
            MAType optInMAType = MAType.Sma, int optInFastPeriod = 12, int optInSlowPeriod = 26)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 ||
                optInSlowPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            var tempBuffer = new decimal[endIdx - startIdx + 1];

            return TA_INT_PO(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInFastPeriod, optInSlowPeriod,
                optInMAType, tempBuffer, false);
        }

        public static int ApoLookback(MAType optInMAType = MAType.Sma, int optInFastPeriod = 12, int optInSlowPeriod = 26)
        {
            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
            {
                return -1;
            }

            return MaLookback(optInMAType, Math.Max(optInSlowPeriod, optInFastPeriod));
        }
    }
}
