namespace TALib
{
    public partial class Core
    {
        public static RetCode MidPoint(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 14)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
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
            while (today <= endIdx)
            {
                double lowest = inReal[trailingIdx++];
                double highest = lowest;
                for (int i = trailingIdx; i <= today; i++)
                {
                    double tmp = inReal[i];
                    if (tmp < lowest)
                    {
                        lowest = tmp;
                    }
                    else if (tmp > highest)
                    {
                        highest = tmp;
                    }
                }

                outReal[outIdx++] = (highest + lowest) / 2.0;
                today++;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode MidPoint(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, int optInTimePeriod = 14)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
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
            while (today <= endIdx)
            {
                decimal lowest = inReal[trailingIdx++];
                decimal highest = lowest;
                for (int i = trailingIdx; i <= today; i++)
                {
                    decimal tmp = inReal[i];
                    if (tmp < lowest)
                    {
                        lowest = tmp;
                    }
                    else if (tmp > highest)
                    {
                        highest = tmp;
                    }
                }

                outReal[outIdx++] = (highest + lowest) / 2m;
                today++;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static int MidPointLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
