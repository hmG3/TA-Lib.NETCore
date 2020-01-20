namespace TALib
{
    public partial class Core
    {
        public static RetCode MinIndex(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, int[] outInteger,
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

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int nbInitialElementNeeded = optInTimePeriod - 1;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = default;
            int today = startIdx;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            int lowestIdx = -1;
            double lowest = default;
            Label_008B:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            double tmp = inReal[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inReal[lowestIdx];
                int i = lowestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_00CC;
                    }

                    tmp = inReal[i];
                    if (tmp < lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }
            }

            if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
            }

            Label_00CC:
            outInteger[outIdx] = lowestIdx;
            outIdx++;
            trailingIdx++;
            today++;
            goto Label_008B;
        }

        public static RetCode MinIndex(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement,
            int[] outInteger, int optInTimePeriod = 30)
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

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int nbInitialElementNeeded = optInTimePeriod - 1;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = default;
            int today = startIdx;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            int lowestIdx = -1;
            decimal lowest = default;
            Label_008B:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            decimal tmp = inReal[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inReal[lowestIdx];
                int i = lowestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_00CF;
                    }

                    tmp = inReal[i];
                    if (tmp < lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }
            }

            if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
            }

            Label_00CF:
            outInteger[outIdx] = lowestIdx;
            outIdx++;
            trailingIdx++;
            today++;
            goto Label_008B;
        }

        public static int MinIndexLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
