namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode PlusDM(T[] inHigh, T[] inLow, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
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

        int lookbackTotal = PlusDMLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int today;
        T diffP;
        T prevLow;
        T prevHigh;
        T diffM;
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
                outReal[outIdx++] = diffP > T.Zero && diffP > diffM ? diffP : T.Zero;
            }

            outNbElement = outIdx;

            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        T prevPlusDM = T.Zero;
        today = startIdx - lookbackTotal;
        prevHigh = inHigh[today];
        prevLow = inLow[today];
        var i = optInTimePeriod - 1;
        while (i-- > 0)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;
            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;
            if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        i = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.PlusDM);
        while (i-- != 0)
        {
            today++;
            T tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;
            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;
            if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM = prevPlusDM - prevPlusDM / tOptInTimePeriod + diffP;
            }
            else
            {
                prevPlusDM -= prevPlusDM / tOptInTimePeriod;
            }
        }

        outReal[0] = prevPlusDM;
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
            if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM = prevPlusDM - prevPlusDM / tOptInTimePeriod + diffP;
            }
            else
            {
                prevPlusDM -= prevPlusDM / tOptInTimePeriod;
            }

            outReal[outIdx++] = prevPlusDM;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int PlusDMLookback(int optInTimePeriod = 14) =>
        optInTimePeriod switch
        {
            < 1 or > 100000 => -1,
            _ => optInTimePeriod > 1 ? optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.PlusDM) - 1 : 1
        };
}
