namespace TALib;

public static partial class Functions
{
    public static Core.RetCode LinearRegIntercept(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = LinearRegInterceptLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
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
            outReal[outIdx++] = (sumY - m * sumX) / optInTimePeriod;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode LinearRegIntercept(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = LinearRegInterceptLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
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
            outReal[outIdx++] = (sumY - m * sumX) / optInTimePeriod;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int LinearRegInterceptLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod - 1;
    }
}
