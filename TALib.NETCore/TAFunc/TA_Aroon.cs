namespace TALib
{
    public partial class Core
    {
        public static RetCode Aroon(int startIdx, int endIdx, double[] inHigh, double[] inLow, ref int outBegIdx, ref int outNBElement,
            double[] outAroonDown, double[] outAroonUp, int optInTimePeriod = 14)
        {
            int i;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outAroonDown == null)
            {
                return RetCode.BadParam;
            }

            if (outAroonUp == null)
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
            Label_00BB:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            double tmp = inLow[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inLow[lowestIdx];
                i = lowestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_00FF;
                    }

                    tmp = inLow[i];
                    if (tmp <= lowest)
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

            Label_00FF:
            tmp = inHigh[today];
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inHigh[highestIdx];
                i = highestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_0136;
                    }

                    tmp = inHigh[i];
                    if (tmp >= highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }
            }

            if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
            }

            Label_0136:
            outAroonUp[outIdx] = factor * (optInTimePeriod - (today - highestIdx));
            outAroonDown[outIdx] = factor * (optInTimePeriod - (today - lowestIdx));
            outIdx++;
            trailingIdx++;
            today++;
            goto Label_00BB;
        }

        public static RetCode Aroon(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, ref int outBegIdx, ref int outNBElement,
            decimal[] outAroonDown, decimal[] outAroonUp, int optInTimePeriod = 14)
        {
            int i;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outAroonDown == null)
            {
                return RetCode.BadParam;
            }

            if (outAroonUp == null)
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
            Label_00BB:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            decimal tmp = inLow[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inLow[lowestIdx];
                i = lowestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_0102;
                    }

                    tmp = inLow[i];
                    if (tmp <= lowest)
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

            Label_0102:
            tmp = inHigh[today];
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inHigh[highestIdx];
                i = highestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_013C;
                    }

                    tmp = inHigh[i];
                    if (tmp >= highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }
            }

            if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
            }

            Label_013C:
            outAroonUp[outIdx] = factor * (optInTimePeriod - (today - highestIdx));
            outAroonDown[outIdx] = factor * (optInTimePeriod - (today - lowestIdx));
            outIdx++;
            trailingIdx++;
            today++;
            goto Label_00BB;
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
