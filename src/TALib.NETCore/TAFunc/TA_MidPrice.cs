namespace TALib
{
    public static partial class Core
    {
        public static RetCode MidPrice(double[] inHigh, double[] inLow, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MidPriceLookback(optInTimePeriod);
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
            while (today <= endIdx)
            {
                double lowest = inLow[trailingIdx];
                double highest = inHigh[trailingIdx++];
                for (int i = trailingIdx; i <= today; i++)
                {
                    double tmp = inLow[i];
                    if (tmp < lowest)
                    {
                        lowest = tmp;
                    }

                    tmp = inHigh[i];
                    if (tmp > highest)
                    {
                        highest = tmp;
                    }
                }

                outReal[outIdx++] = (highest + lowest) / 2.0;
                today++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode MidPrice(decimal[] inHigh, decimal[] inLow, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MidPriceLookback(optInTimePeriod);
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
            while (today <= endIdx)
            {
                decimal lowest = inLow[trailingIdx];
                decimal highest = inHigh[trailingIdx++];
                for (int i = trailingIdx; i <= today; i++)
                {
                    decimal tmp = inLow[i];
                    if (tmp < lowest)
                    {
                        lowest = tmp;
                    }

                    tmp = inHigh[i];
                    if (tmp > highest)
                    {
                        highest = tmp;
                    }
                }

                outReal[outIdx++] = (highest + lowest) / 2m;
                today++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int MidPriceLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
