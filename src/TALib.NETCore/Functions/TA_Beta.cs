namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Beta(T[] inReal0, T[] inReal1, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 5)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal0 == null || inReal1 == null || outReal == null || optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = BetaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T x, y, tmpReal, sxy, sx, sy;
        T sxx = sxy = sx = sy = T.Zero;
        int trailingIdx = startIdx - lookbackTotal;
        var trailingLastPriceX = inReal0[trailingIdx];
        var lastPriceX = trailingLastPriceX;
        var trailingLastPriceY = inReal1[trailingIdx];
        var lastPriceY = trailingLastPriceY;

        int i = ++trailingIdx;
        while (i < startIdx)
        {
            tmpReal = inReal0[i];
            x = !TA_IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : T.Zero;
            lastPriceX = tmpReal;

            tmpReal = inReal1[i++];
            y = !TA_IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : T.Zero;
            lastPriceY = tmpReal;

            sxx += x * x;
            sxy += x * y;
            sx += x;
            sy += y;
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        int outIdx = default;
        do
        {
            tmpReal = inReal0[i];
            x = !TA_IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : T.Zero;
            lastPriceX = tmpReal;

            tmpReal = inReal1[i++];
            y = !TA_IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : T.Zero;
            lastPriceY = tmpReal;

            sxx += x * x;
            sxy += x * y;
            sx += x;
            sy += y;

            tmpReal = inReal0[trailingIdx];
            x = !TA_IsZero(trailingLastPriceX) ? (tmpReal - trailingLastPriceX) / trailingLastPriceX : T.Zero;
            trailingLastPriceX = tmpReal;

            tmpReal = inReal1[trailingIdx++];
            y = !TA_IsZero(trailingLastPriceY) ? (tmpReal - trailingLastPriceY) / trailingLastPriceY : T.Zero;
            trailingLastPriceY = tmpReal;

            tmpReal = tOptInTimePeriod * sxx - sx * sx;
            outReal[outIdx++] = !TA_IsZero(tmpReal) ? (tOptInTimePeriod * sxy - sx * sy) / tmpReal : T.Zero;

            sxx -= x * x;
            sxy -= x * y;
            sx -= x;
            sy -= y;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int BetaLookback(int optInTimePeriod = 5) => optInTimePeriod < 1 ? -1 : optInTimePeriod;
}
