namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Tsf(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = TsfLookback(optInTimePeriod);
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

        var tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        T sumX = tOptInTimePeriod * (tOptInTimePeriod - T.One) * T.CreateChecked(0.5);
        T sumXSqr = tOptInTimePeriod * (tOptInTimePeriod - T.One) * (tOptInTimePeriod * TTwo - T.One) / T.CreateChecked(6);
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
            T b = (sumY - m * sumX) / tOptInTimePeriod;
            outReal[outIdx++] = b + m * tOptInTimePeriod;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TsfLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;
}
