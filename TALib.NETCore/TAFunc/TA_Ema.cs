namespace TALib
{
    public partial class Core
    {
        public static RetCode Ema(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 30)
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

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            return TA_INT_EMA(startIdx, endIdx, inReal, optInTimePeriod, 2.0 / (optInTimePeriod + 1), ref outBegIdx, ref outNBElement,
                outReal);
        }

        public static RetCode Ema(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 30)
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

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            return TA_INT_EMA(startIdx, endIdx, inReal, optInTimePeriod, 2m / (optInTimePeriod + 1), ref outBegIdx, ref outNBElement,
                outReal);
        }

        public static int EmaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1 + (int) Globals.UnstablePeriod[(int) FuncUnstId.Ema];
        }
    }
}
