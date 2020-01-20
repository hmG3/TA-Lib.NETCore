namespace TALib
{
    public partial class Core
    {
        public static RetCode Mom(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 10)
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

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (startIdx < optInTimePeriod)
            {
                startIdx = optInTimePeriod;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = default;
            int inIdx = startIdx;
            int trailingIdx = startIdx - optInTimePeriod;
            while (true)
            {
                if (inIdx > endIdx)
                {
                    break;
                }

                outReal[outIdx] = inReal[inIdx] - inReal[trailingIdx];
                outIdx++;
                trailingIdx++;
                inIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Mom(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 10)
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

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (startIdx < optInTimePeriod)
            {
                startIdx = optInTimePeriod;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = default;
            int inIdx = startIdx;
            int trailingIdx = startIdx - optInTimePeriod;
            while (true)
            {
                if (inIdx > endIdx)
                {
                    break;
                }

                outReal[outIdx] = inReal[inIdx] - inReal[trailingIdx];
                outIdx++;
                trailingIdx++;
                inIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int MomLookback(int optInTimePeriod = 10)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod;
        }
    }
}
