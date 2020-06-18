namespace TALib
{
    public static partial class Core
    {
        public static RetCode Var(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 5)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            return TA_INT_VAR(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
        }

        public static RetCode Var(decimal[] inReal, int startIdx, int endIdx, out int outBegIdx, out int outNbElement,
            decimal[] outReal, int optInTimePeriod = 5)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            return TA_INT_VAR(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
        }

        public static int VarLookback(int optInTimePeriod = 5)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
