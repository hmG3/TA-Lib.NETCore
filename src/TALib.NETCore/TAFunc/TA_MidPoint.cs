namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode MidPoint(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
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

        int lookbackTotal = MidPointLookback(optInTimePeriod);
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
        while (today <= endIdx)
        {
            T lowest = inReal[trailingIdx++];
            T highest = lowest;
            for (var i = trailingIdx; i <= today; i++)
            {
                T tmp = inReal[i];
                if (tmp < lowest)
                {
                    lowest = tmp;
                }
                else if (tmp > highest)
                {
                    highest = tmp;
                }
            }

            outReal[outIdx++] = (highest + lowest) / TTwo;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MidPointLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;
}
