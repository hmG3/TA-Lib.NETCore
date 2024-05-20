namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode MinMaxIndex(T[] inReal, int startIdx, int endIdx, T[] outMinIdx, T[] outMaxIdx, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMinIdx == null || outMaxIdx == null || optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MinMaxIndexLookback(optInTimePeriod);
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
        var highestIdx = -1;
        T highest = T.Zero;
        var lowestIdx = -1;
        T lowest = T.Zero;

        while (today <= endIdx)
        {
            T tmpHigh = inReal[today];
            T tmpLow = tmpHigh;
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inReal[highestIdx];
                int i = highestIdx;
                while (++i <= today)
                {
                    tmpHigh = inReal[i];
                    if (tmpHigh > highest)
                    {
                        highestIdx = i;
                        highest = tmpHigh;
                    }
                }
            }

            if (tmpHigh >= highest)
            {
                highestIdx = today;
                highest = tmpHigh;
            }

            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inReal[lowestIdx];
                int i = lowestIdx;
                while (++i <= today)
                {
                    tmpLow = inReal[i];
                    if (tmpLow < lowest)
                    {
                        lowestIdx = i;
                        lowest = tmpLow;
                    }
                }
            }

            if (tmpLow <= lowest)
            {
                lowestIdx = today;
                lowest = tmpLow;
            }

            outMaxIdx[outIdx] = T.CreateChecked(highestIdx);
            outMinIdx[outIdx++] = T.CreateChecked(lowestIdx);
            trailingIdx++;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MinMaxIndexLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;
}
