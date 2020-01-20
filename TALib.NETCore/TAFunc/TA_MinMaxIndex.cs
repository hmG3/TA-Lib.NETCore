namespace TALib
{
    public partial class Core
    {
        public static RetCode MinMaxIndex(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement,
            int[] outMinIdx, int[] outMaxIdx, int optInTimePeriod = 30)
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

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outMinIdx == null)
            {
                return RetCode.BadParam;
            }

            if (outMaxIdx == null)
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
            Label_00AA:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            double tmpHigh = inReal[today];
            double tmpLow = tmpHigh;
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inReal[highestIdx];
                i = highestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_00F2;
                    }

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

            Label_00F2:
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inReal[lowestIdx];
                i = lowestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_012A;
                    }

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

            Label_012A:
            outMaxIdx[outIdx] = highestIdx;
            outMinIdx[outIdx] = lowestIdx;
            outIdx++;
            trailingIdx++;
            today++;
            goto Label_00AA;
        }

        public static RetCode MinMaxIndex(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement,
            int[] outMinIdx, int[] outMaxIdx, int optInTimePeriod = 30)
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

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outMinIdx == null)
            {
                return RetCode.BadParam;
            }

            if (outMaxIdx == null)
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
            Label_00AA:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            decimal tmpHigh = inReal[today];
            decimal tmpLow = tmpHigh;
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inReal[highestIdx];
                i = highestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_00F5;
                    }

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

            Label_00F5:
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inReal[lowestIdx];
                i = lowestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_012F;
                    }

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

            Label_012F:
            outMaxIdx[outIdx] = highestIdx;
            outMinIdx[outIdx] = lowestIdx;
            outIdx++;
            trailingIdx++;
            today++;
            goto Label_00AA;
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
