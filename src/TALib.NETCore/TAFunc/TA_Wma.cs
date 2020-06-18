namespace TALib
{
    public static partial class Core
    {
        public static RetCode Wma(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = WmaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int divider = (optInTimePeriod * (optInTimePeriod + 1)) >> 1;

            int outIdx = default;
            int trailingIdx = startIdx - lookbackTotal;

            double periodSub = default;
            double periodSum = periodSub;
            int inIdx = trailingIdx;
            int i = 1;
            while (inIdx < startIdx)
            {
                double tempReal = inReal[inIdx++];
                periodSub += tempReal;
                periodSum += tempReal * i;
                i++;
            }
            double trailingValue = default;

            while (inIdx <= endIdx)
            {
                double tempReal = inReal[inIdx++];
                periodSub += tempReal;
                periodSub -= trailingValue;
                periodSum += tempReal * optInTimePeriod;
                trailingValue = inReal[trailingIdx++];
                outReal[outIdx++] = periodSum / divider;
                periodSum -= periodSub;
            }

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode Wma(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = WmaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int divider = (optInTimePeriod * (optInTimePeriod + 1)) >> 1;

            int outIdx = default;
            int trailingIdx = startIdx - lookbackTotal;

            decimal periodSub = default;
            decimal periodSum = periodSub;
            int inIdx = trailingIdx;
            int i = 1;
            while (inIdx < startIdx)
            {
                decimal tempReal = inReal[inIdx++];
                periodSub += tempReal;
                periodSum += tempReal * i;
                i++;
            }
            decimal trailingValue = default;

            while (inIdx <= endIdx)
            {
                decimal tempReal = inReal[inIdx++];
                periodSub += tempReal;
                periodSub -= trailingValue;
                periodSum += tempReal * optInTimePeriod;
                trailingValue = inReal[trailingIdx++];
                outReal[outIdx++] = periodSum / divider;
                periodSum -= periodSub;
            }

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int WmaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
