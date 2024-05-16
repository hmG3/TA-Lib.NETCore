namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode WillR(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = WillRLookback(optInTimePeriod);
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
        int lowestIdx = highestIdx;
        T highest, lowest;
        T diff = highest = lowest = T.Zero;

        while (today <= endIdx)
        {
            T tmp = inLow[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inLow[lowestIdx];
                int i = lowestIdx;
                while (++i <= today)
                {
                    tmp = inLow[i];
                    if (tmp < lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }

                diff = (highest - lowest) / (T.NegativeOne * THundred);
            }
            else if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
                diff = (highest - lowest) / (T.NegativeOne * THundred);
            }

            tmp = inHigh[today];
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inHigh[highestIdx];
                int i = highestIdx;
                while (++i <= today)
                {
                    tmp = inHigh[i];
                    if (tmp > highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }

                diff = (highest - lowest) / (T.NegativeOne * THundred);
            }
            else if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
                diff = (highest - lowest) / (T.NegativeOne * THundred);
            }

            outReal[outIdx++] = !T.IsZero(diff) ? (highest - inClose[today]) / diff : T.Zero;

            trailingIdx++;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int WillRLookback(int optInTimePeriod = 14) => optInTimePeriod is < 2 or > 100000 ? -1 : optInTimePeriod - 1;
}
