namespace TALib
{
    public static partial class Core
    {
        public static RetCode MinMax(double[] inReal, int startIdx, int endIdx, double[] outMin, double[] outMax, out int outBegIdx,
            out int outNbElement, int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMin == null || outMax == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
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
                else if (tmpHigh >= highest)
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

                outMax[outIdx] = highest;
                outMin[outIdx++] = lowest;
                trailingIdx++;
                today++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode MinMax(decimal[] inReal, int startIdx, int endIdx, decimal[] outMin, decimal[] outMax, out int outBegIdx,
            out int outNbElement, int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMin == null || outMax == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
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
                else if (tmpHigh >= highest)
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

                outMax[outIdx] = highest;
                outMin[outIdx++] = lowest;
                trailingIdx++;
                today++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int MinMaxLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
