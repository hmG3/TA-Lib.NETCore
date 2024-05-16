namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode LinearRegIntercept(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod is < 2 or > 100000)
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

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);
        T sumX = T.CreateChecked(optInTimePeriod * (optInTimePeriod - 1) * 0.5);
        T sumXSqr = T.CreateChecked(optInTimePeriod * (optInTimePeriod - 1) * (optInTimePeriod * 2 - 1) / 6.0);
        T divisor = sumX * sumX - tOptInTimePeriod * sumXSqr;
        while (today <= endIdx)
        {
            T sumXY = T.Zero;
            T sumY = T.Zero;
            for (var i = optInTimePeriod; i-- != 0;)
            {
                T tempValue1 = inReal[today - i];
                sumY += tempValue1;
                sumXY += T.CreateChecked(i) * tempValue1;
            }

            T m = (tOptInTimePeriod * sumXY - sumX * sumY) / divisor;
            outReal[outIdx++] = (sumY - m * sumX) / tOptInTimePeriod;
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
