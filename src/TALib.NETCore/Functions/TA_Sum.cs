namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Sum(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
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

        int lookbackTotal = SumLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T periodTotal = T.Zero;
        int trailingIdx = startIdx - lookbackTotal;
        int i = trailingIdx;
        while (i < startIdx)
        {
            periodTotal += inReal[i++];
        }

        int outIdx = default;
        do
        {
            periodTotal += inReal[i++];
            T tempReal = periodTotal;
            periodTotal -= inReal[trailingIdx++];
            outReal[outIdx++] = tempReal;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int SumLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;
}
