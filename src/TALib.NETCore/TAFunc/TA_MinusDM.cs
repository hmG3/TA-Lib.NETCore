namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode MinusDM(T[] inHigh, T[] inLow, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || outReal == null || optInTimePeriod is < 1 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MinusDMLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int today;
        T diffM;
        T prevLow;
        T prevHigh;
        T diffP;
        int outIdx = default;
        if (optInTimePeriod == 1)
        {
            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            while (today < endIdx)
            {
                today++;
                T tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                outReal[outIdx++] = diffM > T.Zero && diffP < diffM ? diffM : T.Zero;
            }

            outNbElement = outIdx;

            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;

        T prevMinusDM = T.Zero;
        today = startIdx - lookbackTotal;
        prevHigh = inHigh[today];
        prevLow = inLow[today];
        int i = optInTimePeriod - 1;
        while (i-- > 0)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;
            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;
            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        i = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.MinusDM);
        while (i-- != 0)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;
            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;
            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM = prevMinusDM - prevMinusDM / tOptInTimePeriod + diffM;
            }
            else
            {
                prevMinusDM -= prevMinusDM / tOptInTimePeriod;
            }
        }

        outReal[0] = prevMinusDM;
        outIdx = 1;

        while (today < endIdx)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;
            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;

            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM = prevMinusDM - prevMinusDM / tOptInTimePeriod + diffM;
            }
            else
            {
                prevMinusDM -= prevMinusDM / tOptInTimePeriod;
            }

            outReal[outIdx++] = prevMinusDM;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MinusDMLookback(int optInTimePeriod = 14) =>
        optInTimePeriod switch
        {
            < 1 or > 100000 => -1,
            _ => optInTimePeriod > 1 ? optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.MinusDM) - 1 : 1
        };
}
