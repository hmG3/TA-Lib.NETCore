namespace TALib
{
    public partial class Core
    {
        public static RetCode LinearReg(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement,
            double[] outReal, int optInTimePeriod = 14)
        {
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

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = LinearRegLookback(optInTimePeriod);
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

            int outIdx = default;
            int today = startIdx;
            double sumX = optInTimePeriod * (optInTimePeriod - 1) * 0.5;
            double sumXSqr = optInTimePeriod * (optInTimePeriod - 1) * (optInTimePeriod * 2 - 1) / 6.0;
            double divisor = sumX * sumX - optInTimePeriod * sumXSqr;
            while (true)
            {
                if (today > endIdx)
                {
                    outBegIdx = startIdx;
                    outNBElement = outIdx;
                    return RetCode.Success;
                }

                double sumXY = default;
                double sumY = default;
                int i = optInTimePeriod;
                while (true)
                {
                    i--;
                    if (i == 0)
                    {
                        break;
                    }

                    double tempValue1 = inReal[today - i];
                    sumY += tempValue1;
                    sumXY += i * tempValue1;
                }

                double m = (optInTimePeriod * sumXY - sumX * sumY) / divisor;
                double b = (sumY - m * sumX) / optInTimePeriod;
                outReal[outIdx] = b + m * (optInTimePeriod - 1);
                outIdx++;
                today++;
            }
        }

        public static RetCode LinearReg(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, int optInTimePeriod = 14)
        {
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

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = LinearRegLookback(optInTimePeriod);
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

            int outIdx = default;
            int today = startIdx;
            decimal sumX = optInTimePeriod * (optInTimePeriod - 1) * 0.5m;
            decimal sumXSqr = optInTimePeriod * (optInTimePeriod - 1) * (optInTimePeriod * 2 - 1) / 6m;
            decimal divisor = sumX * sumX - optInTimePeriod * sumXSqr;
            while (true)
            {
                if (today > endIdx)
                {
                    outBegIdx = startIdx;
                    outNBElement = outIdx;
                    return RetCode.Success;
                }

                decimal sumXY = default;
                decimal sumY = default;
                int i = optInTimePeriod;
                while (true)
                {
                    i--;
                    if (i == 0)
                    {
                        break;
                    }

                    decimal tempValue1 = inReal[today - i];
                    sumY += tempValue1;
                    sumXY += i * tempValue1;
                }

                decimal m = (optInTimePeriod * sumXY - sumX * sumY) / divisor;
                decimal b = (sumY - m * sumX) / optInTimePeriod;
                outReal[outIdx] = b + m * (optInTimePeriod - 1);
                outIdx++;
                today++;
            }
        }

        public static int LinearRegLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
