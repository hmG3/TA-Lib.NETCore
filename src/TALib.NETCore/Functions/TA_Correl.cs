namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Correl(T[] inReal0, T[] inReal1, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal0 == null || inReal1 == null || outReal == null || optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = CorrelLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        int trailingIdx = startIdx - lookbackTotal;

        T sumX, sumY, sumX2, sumY2;
        T sumXY = sumX = sumY = sumX2 = sumY2 = T.Zero;
        int today;
        for (today = trailingIdx; today <= startIdx; today++)
        {
            T x = inReal0[today];
            sumX += x;
            sumX2 += x * x;

            T y = inReal1[today];
            sumXY += x * y;
            sumY += y;
            sumY2 += y * y;
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        T trailingX = inReal0[trailingIdx];
        T trailingY = inReal1[trailingIdx++];
        T tempReal = (sumX2 - sumX * sumX / tOptInTimePeriod) * (sumY2 - sumY * sumY / tOptInTimePeriod);
        outReal[0] = !TA_IsZeroOrNeg(tempReal) ? (sumXY - sumX * sumY / tOptInTimePeriod) / T.Sqrt(tempReal) : T.Zero;

        int outIdx = 1;
        while (today <= endIdx)
        {
            sumX -= trailingX;
            sumX2 -= trailingX * trailingX;

            sumXY -= trailingX * trailingY;
            sumY -= trailingY;
            sumY2 -= trailingY * trailingY;

            T x = inReal0[today];
            sumX += x;
            sumX2 += x * x;

            T y = inReal1[today++];
            sumXY += x * y;
            sumY += y;
            sumY2 += y * y;

            trailingX = inReal0[trailingIdx];
            trailingY = inReal1[trailingIdx++];
            tempReal = (sumX2 - sumX * sumX / tOptInTimePeriod) * (sumY2 - sumY * sumY / tOptInTimePeriod);
            outReal[outIdx++] = !TA_IsZeroOrNeg(tempReal) ? (sumXY - sumX * sumY / tOptInTimePeriod) / T.Sqrt(tempReal) : T.Zero;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CorrelLookback(int optInTimePeriod = 30) => optInTimePeriod < 1 ? -1 : optInTimePeriod - 1;
}
