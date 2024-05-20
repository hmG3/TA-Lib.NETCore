namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode MinusDI(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MinusDILookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int today;
        T prevLow;
        T prevHigh;
        T diffM;
        T prevClose;
        T diffP;
        int outIdx = default;
        if (optInTimePeriod == 1)
        {
            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            prevClose = inClose[today];
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
                    TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                    outReal[outIdx++] = !TA_IsZero(tempReal) ? diffM / tempReal : T.Zero;
                }
                else
                {
                    outReal[outIdx++] = T.Zero;
                }

                prevClose = inClose[today];
            }

            outNbElement = outIdx;

            return Core.RetCode.Success;
        }

        today = startIdx;
        outBegIdx = today;
        T prevMinusDM = T.Zero;
        T prevTR = T.Zero;
        today = startIdx - lookbackTotal;
        prevHigh = inHigh[today];
        prevLow = inLow[today];
        prevClose = inClose[today];
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
            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR += tempReal;
            prevClose = inClose[today];
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        i = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.MinusDI) + 1;
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

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / tOptInTimePeriod + tempReal;
            prevClose = inClose[today];
        }

        outReal[0] = !TA_IsZero(prevTR) ? THundred * (prevMinusDM / prevTR) : T.Zero;
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

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / tOptInTimePeriod + tempReal;
            prevClose = inClose[today];
            outReal[outIdx++] = !TA_IsZero(prevTR) ? THundred * (prevMinusDM / prevTR) : T.Zero;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MinusDILookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 1 ? -1 :
        optInTimePeriod > 1 ? optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.MinusDI) : 1;
}
