namespace TALib
{
    public partial class Core
    {
        public static RetCode MinMaxIndex(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement,
            int[] outMinIdx, int[] outMaxIdx, int optInTimePeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMinIdx == null || outMaxIdx == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
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
            int highestIdx = -1;
            double highest = default;
            int lowestIdx = -1;
            double lowest = default;

            while (today <= endIdx)
            {
                double tmpHigh = inReal[today];
                double tmpLow = tmpHigh;
                if (highestIdx < trailingIdx)
                {
                    highestIdx = trailingIdx;
                    highest = inReal[highestIdx];
                    int i = highestIdx;
                    while (++i <= today)
                    {
                        tmpHigh = inReal[i];
                        if (tmpHigh > highest)
                        {
                            highestIdx = i;
                            highest = tmpHigh;
                        }
                    }
                }

                if (tmpHigh >= highest)
                {
                    highestIdx = today;
                    highest = tmpHigh;
                }

                if (lowestIdx < trailingIdx)
                {
                    lowestIdx = trailingIdx;
                    lowest = inReal[lowestIdx];
                    int i = lowestIdx;
                    while (++i <= today)
                    {
                        tmpLow = inReal[i];
                        if (tmpLow < lowest)
                        {
                            lowestIdx = i;
                            lowest = tmpLow;
                        }
                    }
                }

                if (tmpLow <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmpLow;
                }

                outMaxIdx[outIdx] = highestIdx;
                outMinIdx[outIdx++] = lowestIdx;
                trailingIdx++;
                today++;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode MinMaxIndex(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement,
            int[] outMinIdx, int[] outMaxIdx, int optInTimePeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMinIdx == null || outMaxIdx == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
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
            int highestIdx = -1;
            decimal highest = default;
            int lowestIdx = -1;
            decimal lowest = default;

            while (today <= endIdx)
            {
                decimal tmpHigh = inReal[today];
                decimal tmpLow = tmpHigh;
                if (highestIdx < trailingIdx)
                {
                    highestIdx = trailingIdx;
                    highest = inReal[highestIdx];
                    int i = highestIdx;
                    while (++i <= today)
                    {
                        tmpHigh = inReal[i];
                        if (tmpHigh > highest)
                        {
                            highestIdx = i;
                            highest = tmpHigh;
                        }
                    }
                }

                if (tmpHigh >= highest)
                {
                    highestIdx = today;
                    highest = tmpHigh;
                }

                if (lowestIdx < trailingIdx)
                {
                    lowestIdx = trailingIdx;
                    lowest = inReal[lowestIdx];
                    int i = lowestIdx;
                    while (++i <= today)
                    {
                        tmpLow = inReal[i];
                        if (tmpLow < lowest)
                        {
                            lowestIdx = i;
                            lowest = tmpLow;
                        }
                    }
                }

                if (tmpLow <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmpLow;
                }

                outMaxIdx[outIdx] = highestIdx;
                outMinIdx[outIdx++] = lowestIdx;
                trailingIdx++;
                today++;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static int MinMaxIndexLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
