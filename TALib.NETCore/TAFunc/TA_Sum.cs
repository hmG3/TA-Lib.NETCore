namespace TALib
{
    public partial class Core
    {
        public static RetCode Sum(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = optInTimePeriod - 1;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double periodTotal = default;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    periodTotal += inReal[i++];
                }
            }

            int outIdx = default;
            do
            {
                periodTotal += inReal[i++];
                double tempReal = periodTotal;
                periodTotal -= inReal[trailingIdx++];
                outReal[outIdx++] = tempReal;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode Sum(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = optInTimePeriod - 1;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            decimal periodTotal = default;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    periodTotal += inReal[i++];
                }
            }

            int outIdx = default;
            do
            {
                periodTotal += inReal[i++];
                decimal tempReal = periodTotal;
                periodTotal -= inReal[trailingIdx++];
                outReal[outIdx++] = tempReal;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int SumLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
