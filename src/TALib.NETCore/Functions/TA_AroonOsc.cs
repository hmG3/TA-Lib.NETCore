namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode AroonOsc(T[] inHigh, T[] inLow, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || outReal == null || optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AroonOscLookback(optInTimePeriod);
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
        int lowestIdx = -1;
        int highestIdx = -1;
        T lowest = T.Zero;
        T highest = T.Zero;
        T factor = THundred / T.CreateChecked(optInTimePeriod);

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
                    if (tmp <= lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }
            }
            else if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
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
                    if (tmp >= highest)
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

            outReal[outIdx++] = factor * T.CreateChecked(highestIdx - lowestIdx);

            trailingIdx++;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AroonOscLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod;
}
