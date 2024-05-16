namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Wma(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
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

        int lookbackTotal = WmaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        int trailingIdx = startIdx - lookbackTotal;

        T periodSub = T.Zero;
        T periodSum = periodSub;
        int inIdx = trailingIdx;
        int i = 1;
        while (inIdx < startIdx)
        {
            T tempReal = inReal[inIdx++];
            periodSub += tempReal;
            periodSum += tempReal * T.CreateChecked(i);
            i++;
        }
        T trailingValue = T.Zero;

        T divider = T.CreateChecked((optInTimePeriod * (optInTimePeriod + 1)) >> 1);
        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);
        while (inIdx <= endIdx)
        {
            T tempReal = inReal[inIdx++];
            periodSub += tempReal;
            periodSub -= trailingValue;
            periodSum += tempReal * tOptInTimePeriod;
            trailingValue = inReal[trailingIdx++];
            outReal[outIdx++] = periodSum / divider;
            periodSum -= periodSub;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int WmaLookback(int optInTimePeriod = 30) => optInTimePeriod is < 2 or > 100000 ? -1 : optInTimePeriod - 1;
}
