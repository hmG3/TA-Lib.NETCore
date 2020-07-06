namespace TALib
{
    public static partial class Core
    {
        public static RetCode MinIndex(double[] inReal, int startIdx, int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outInteger == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MinIndexLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int outIdx = default;
            int today = startIdx;
            int trailingIdx = startIdx - lookbackTotal;
            int lowestIdx = -1;
            double lowest = default;

            while (today <= endIdx)
            {
                double tmp = inReal[today];
                if (lowestIdx < trailingIdx)
                {
                    lowestIdx = trailingIdx;
                    lowest = inReal[lowestIdx];
                    int i = lowestIdx;
                    while (++i <= today)
                    {
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

                outInteger[outIdx++] = lowestIdx;
                trailingIdx++;
                today++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode MinIndex(decimal[] inReal, int startIdx, int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outInteger == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MinIndexLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int outIdx = default;
            int today = startIdx;
            int trailingIdx = startIdx - lookbackTotal;
            int lowestIdx = -1;
            decimal lowest = default;

            while (today <= endIdx)
            {
                decimal tmp = inReal[today];
                if (lowestIdx < trailingIdx)
                {
                    lowestIdx = trailingIdx;
                    lowest = inReal[lowestIdx];
                    int i = lowestIdx;
                    while (++i <= today)
                    {
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

                outInteger[outIdx++] = lowestIdx;
                trailingIdx++;
                today++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
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
