namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode AvgDev(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AvgDevLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        int today = startIdx;
        if (today > endIdx)
        {
            return Core.RetCode.Success;
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        outBegIdx = today;

        int outIdx = default;
        while (today <= endIdx)
        {
            T todaySum = T.Zero;
            for (var i = 0; i < optInTimePeriod; i++)
            {
                todaySum += inReal[today - i];
            }

            T todayDev = T.Zero;
            for (var i = 0; i < optInTimePeriod; i++)
            {
                todayDev += T.Abs(inReal[today - i] - todaySum / tOptInTimePeriod);
            }

            outReal[outIdx++] = todayDev / tOptInTimePeriod;
            today++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AvgDevLookback(int optInTimePeriod = 14) => optInTimePeriod is < 2 or > 100000 ? -1 : optInTimePeriod - 1;
}
