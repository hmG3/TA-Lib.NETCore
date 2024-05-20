namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Roc(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 10)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = RocLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        int inIdx = startIdx;
        int trailingIdx = startIdx - lookbackTotal;
        while (inIdx <= endIdx)
        {
            T tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = !T.IsZero(tempReal) ? (inReal[inIdx] / tempReal - T.One) * THundred : T.Zero;
            inIdx++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int RocLookback(int optInTimePeriod = 10) => optInTimePeriod < 1 ? -1 : optInTimePeriod;
}
