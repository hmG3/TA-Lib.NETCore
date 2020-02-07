namespace TALib
{
    public partial class Core
    {
        public static RetCode Aroon(int startIdx, int endIdx, double[] inHigh, double[] inLow, ref int outBegIdx, ref int outNBElement,
            double[] outAroonDown, double[] outAroonUp, int optInTimePeriod = 14)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outAroonDown == null || outAroonUp == null || optInTimePeriod < 2 ||
                optInTimePeriod > 100000)
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
            int today = startIdx;
            int trailingIdx = startIdx - optInTimePeriod;
            int lowestIdx = -1;
            int highestIdx = -1;
            double lowest = default;
            double highest = default;
            double factor = 100.0 / optInTimePeriod;

            while (today <= endIdx)
            {
                int i;
                double tmp = inLow[today];
                if (lowestIdx < trailingIdx)
                {
                    lowestIdx = trailingIdx;
                    lowest = inLow[lowestIdx];
                    i = lowestIdx;
                    while (++i <= today)
                    {
                        tmp = inLow[i];
                        if (tmp <= lowest)
                        {
                            lowestIdx = i;
                            lowest = tmp;
                        }
                    }
                }
                else if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                }

                tmp = inHigh[today];
                if (highestIdx < trailingIdx)
                {
                    highestIdx = trailingIdx;
                    highest = inHigh[highestIdx];
                    i = highestIdx;
                    while (++i <= today)
                    {
                        tmp = inHigh[i];
                        if (tmp >= highest)
                        {
                            highestIdx = i;
                            highest = tmp;
                        }
                    }
                }
                else if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                }

                outAroonUp[outIdx] = factor * (optInTimePeriod - (today - highestIdx));
                outAroonDown[outIdx] = factor * (optInTimePeriod - (today - lowestIdx));

                outIdx++;
                trailingIdx++;
                today++;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Aroon(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, ref int outBegIdx, ref int outNBElement,
            decimal[] outAroonDown, decimal[] outAroonUp, int optInTimePeriod = 14)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outAroonDown == null || outAroonUp == null || optInTimePeriod < 2 ||
                optInTimePeriod > 100000)
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
            int today = startIdx;
            int trailingIdx = startIdx - optInTimePeriod;
            int lowestIdx = -1;
            int highestIdx = -1;
            decimal lowest = default;
            decimal highest = default;
            decimal factor = 100m / optInTimePeriod;

            while (today <= endIdx)
            {
                int i;
                decimal tmp = inLow[today];
                if (lowestIdx < trailingIdx)
                {
                    lowestIdx = trailingIdx;
                    lowest = inLow[lowestIdx];
                    i = lowestIdx;
                    while (++i <= today)
                    {
                        tmp = inLow[i];
                        if (tmp <= lowest)
                        {
                            lowestIdx = i;
                            lowest = tmp;
                        }
                    }
                }
                else if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                }

                tmp = inHigh[today];
                if (highestIdx < trailingIdx)
                {
                    highestIdx = trailingIdx;
                    highest = inHigh[highestIdx];
                    i = highestIdx;
                    while (++i <= today)
                    {
                        tmp = inHigh[i];
                        if (tmp >= highest)
                        {
                            highestIdx = i;
                            highest = tmp;
                        }
                    }
                }
                else if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                }

                outAroonUp[outIdx] = factor * (optInTimePeriod - (today - highestIdx));
                outAroonDown[outIdx] = factor * (optInTimePeriod - (today - lowestIdx));

                outIdx++;
                trailingIdx++;
                today++;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static int AroonLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod;
        }
    }
}
