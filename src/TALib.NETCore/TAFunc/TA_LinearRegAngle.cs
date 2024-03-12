namespace TALib
{
    public static partial class Core
    {
        public static RetCode LinearRegAngle(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod = 14)
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

            int lookbackTotal = LinearRegAngleLookback(optInTimePeriod);
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

            double sumX = optInTimePeriod * (optInTimePeriod - 1) * 0.5;
            double sumXSqr = optInTimePeriod * (optInTimePeriod - 1) * (optInTimePeriod * 2 - 1) / 6.0;
            double divisor = sumX * sumX - optInTimePeriod * sumXSqr;
            while (today <= endIdx)
            {
                double sumXY = default;
                double sumY = default;
                for (int i = optInTimePeriod; i-- != 0;)
                {
                    double tempValue1 = inReal[today - i];
                    sumY += tempValue1;
                    sumXY += i * tempValue1;
                }

                double m = (optInTimePeriod * sumXY - sumX * sumY) / divisor;
                outReal[outIdx++] = Math.Atan(m) * 180.0 / Math.PI;
                today++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode LinearRegAngle(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod = 14)
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

            int lookbackTotal = LinearRegAngleLookback(optInTimePeriod);
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

            decimal sumX = optInTimePeriod * (optInTimePeriod - 1) * 0.5m;
            decimal sumXSqr = optInTimePeriod * (optInTimePeriod - 1) * (optInTimePeriod * 2 - 1) / 6m;
            decimal divisor = sumX * sumX - optInTimePeriod * sumXSqr;
            while (today <= endIdx)
            {
                decimal sumXY = default;
                decimal sumY = default;
                for (int i = optInTimePeriod; i-- != 0;)
                {
                    decimal tempValue1 = inReal[today - i];
                    sumY += tempValue1;
                    sumXY += i * tempValue1;
                }

                decimal m = (optInTimePeriod * sumXY - sumX * sumY) / divisor;
                outReal[outIdx++] = DecimalMath.Atan(m) * 180m / DecimalMath.PI;
                today++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int LinearRegAngleLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
