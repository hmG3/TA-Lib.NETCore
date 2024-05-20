namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Adx(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AdxLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        T tempReal;
        T diffM;
        T diffP;
        T plusDI;
        T minusDI;
        int today = startIdx;
        outBegIdx = today;
        T prevMinusDM = T.Zero;
        T prevPlusDM = T.Zero;
        T prevTR = T.Zero;
        today = startIdx - lookbackTotal;
        T prevHigh = inHigh[today];
        T prevLow = inLow[today];
        T prevClose = inClose[today];
        int i = optInTimePeriod - 1;
        while (i-- > 0)
        {
            today++;
            tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;

            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR += tempReal;
            prevClose = inClose[today];
        }

        T sumDX = T.Zero;
        i = optInTimePeriod;
        while (i-- > 0)
        {
            today++;
            tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / tOptInTimePeriod;
            prevPlusDM -= prevPlusDM / tOptInTimePeriod;
            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / tOptInTimePeriod + tempReal;
            prevClose = inClose[today];
            if (!TA_IsZero(prevTR))
            {
                minusDI = THundred * (prevMinusDM / prevTR);
                plusDI = THundred * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (!TA_IsZero(tempReal))
                {
                    sumDX += THundred * (T.Abs(minusDI - plusDI) / tempReal);
                }
            }
        }

        T prevADX = sumDX / tOptInTimePeriod;

        i = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Adx);
        while (i-- > 0)
        {
            today++;
            tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / tOptInTimePeriod;
            prevPlusDM -= prevPlusDM / tOptInTimePeriod;

            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / tOptInTimePeriod + tempReal;
            prevClose = inClose[today];
            if (!TA_IsZero(prevTR))
            {
                minusDI = THundred * (prevMinusDM / prevTR);
                plusDI = THundred * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (!TA_IsZero(tempReal))
                {
                    tempReal = THundred * (T.Abs(minusDI - plusDI) / tempReal);
                    prevADX = (prevADX * (tOptInTimePeriod - T.One) + tempReal) / tOptInTimePeriod;
                }
            }
        }

        outReal[0] = prevADX;
        var outIdx = 1;

        while (today < endIdx)
        {
            today++;
            tempReal = inHigh[today];
            diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / tOptInTimePeriod;
            prevPlusDM -= prevPlusDM / tOptInTimePeriod;

            if (diffM > T.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > T.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / tOptInTimePeriod + tempReal;
            prevClose = inClose[today];
            if (!TA_IsZero(prevTR))
            {
                minusDI = THundred * (prevMinusDM / prevTR);
                plusDI = THundred * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (!TA_IsZero(tempReal))
                {
                    tempReal = THundred * (T.Abs(minusDI - plusDI) / tempReal);
                    prevADX = (prevADX * (tOptInTimePeriod - T.One) + tempReal) / tOptInTimePeriod;
                }
            }

            outReal[outIdx++] = prevADX;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AdxLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod * 2 + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Adx) - 1;
}
