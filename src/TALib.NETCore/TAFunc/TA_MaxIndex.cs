namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode MaxIndex(T[] inReal, int startIdx, int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outInteger == null || optInTimePeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MaxIndexLookback(optInTimePeriod);
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
        int trailingIdx = startIdx - lookbackTotal;
        int highestIdx = -1;
        T highest = T.Zero;

        while (today <= endIdx)
        {
            T tmp = inReal[today];

            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inReal[highestIdx];
                int i = highestIdx;
                while (++i <= today)
                {
                    tmp = inReal[i];
                    if (tmp > highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }
            }
            else if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
            }

            outInteger[outIdx++] = highestIdx;
            trailingIdx++;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MaxIndexLookback(int optInTimePeriod = 30) => optInTimePeriod is < 2 or > 100000 ? -1 : optInTimePeriod - 1;
}
