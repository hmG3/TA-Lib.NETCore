namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Dx(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
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

        int lookbackTotal = DxLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        T prevMinusDM = T.Zero;
        T prevPlusDM = T.Zero;
        T prevTR = T.Zero;
        int today = startIdx - lookbackTotal;
        T prevHigh = inHigh[today];
        T prevLow = inLow[today];
        T prevClose = inClose[today];
        int i = optInTimePeriod - 1;
        while (i-- > 0)
        {
            today++;
            T tempReal = inHigh[today];
            T diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            T diffM = prevLow - tempReal;
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

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        i = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Dx) + 1;
        while (i-- != 0)
        {
            today++;
            T tempReal = inHigh[today];
            T diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            T diffM = prevLow - tempReal;
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
        }

        if (!T.IsZero(prevTR))
        {
            T minusDI = THundred * (prevMinusDM / prevTR);
            T plusDI = THundred * (prevPlusDM / prevTR);
            T tempReal = minusDI + plusDI;
            outReal[0] = !T.IsZero(tempReal) ? THundred * (T.Abs(minusDI - plusDI) / tempReal) : T.Zero;
        }
        else
        {
            outReal[0] = T.Zero;
        }

        var outIdx = 1;
        while (today < endIdx)
        {
            today++;
            T tempReal = inHigh[today];
            T diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            T diffM = prevLow - tempReal;
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

            if (!T.IsZero(prevTR))
            {
                T minusDI = THundred * (prevMinusDM / prevTR);
                T plusDI = THundred * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (!T.IsZero(tempReal))
                {
                    outReal[outIdx] = THundred * (T.Abs(minusDI - plusDI) / tempReal);
                }
                else
                {
                    outReal[outIdx] = outReal[outIdx - 1];
                }
            }
            else
            {
                outReal[outIdx] = outReal[outIdx - 1];
            }

            outIdx++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int DxLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Dx);
}
