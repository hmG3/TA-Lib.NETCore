namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Beta(double[] inReal0, double[] inReal1, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 5)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal0 == null || inReal1 == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
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

        double x, y, tmpReal, sxy, sx, sy;
        double sxx = sxy = sx = sy = default;
        int trailingIdx = startIdx - lookbackTotal;
        var trailingLastPriceX = inReal0[trailingIdx];
        var lastPriceX = trailingLastPriceX;
        var trailingLastPriceY = inReal1[trailingIdx];
        var lastPriceY = trailingLastPriceY;

        int i = ++trailingIdx;
        while (i < startIdx)
        {
            tmpReal = inReal0[i];
            x = !Core.TA_IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : 0.0;
            lastPriceX = tmpReal;

            tmpReal = inReal1[i++];
            y = !Core.TA_IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : 0.0;
            lastPriceY = tmpReal;

            sxx += x * x;
            sxy += x * y;
            sx += x;
            sy += y;
        }

        int outIdx = default;
        do
        {
            tmpReal = inReal0[i];
            x = !Core.TA_IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : 0.0;
            lastPriceX = tmpReal;

            tmpReal = inReal1[i++];
            y = !Core.TA_IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : 0.0;
            lastPriceY = tmpReal;

            sxx += x * x;
            sxy += x * y;
            sx += x;
            sy += y;

            tmpReal = inReal0[trailingIdx];
            x = !Core.TA_IsZero(trailingLastPriceX) ? (tmpReal - trailingLastPriceX) / trailingLastPriceX : 0.0;
            trailingLastPriceX = tmpReal;

            tmpReal = inReal1[trailingIdx++];
            y = !Core.TA_IsZero(trailingLastPriceY) ? (tmpReal - trailingLastPriceY) / trailingLastPriceY : 0.0;
            trailingLastPriceY = tmpReal;

            tmpReal = optInTimePeriod * sxx - sx * sx;
            outReal[outIdx++] = !Core.TA_IsZero(tmpReal) ? (optInTimePeriod * sxy - sx * sy) / tmpReal : 0.0;

            sxx -= x * x;
            sxy -= x * y;
            sx -= x;
            sy -= y;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Beta(decimal[] inReal0, decimal[] inReal1, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 5)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal0 == null || inReal1 == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
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

        decimal x, y, tmpReal, sxy, sx, sy;
        decimal sxx = sxy = sx = sy = default;
        int trailingIdx = startIdx - lookbackTotal;
        var trailingLastPriceX = inReal0[trailingIdx];
        var lastPriceX = trailingLastPriceX;
        var trailingLastPriceY = inReal1[trailingIdx];
        var lastPriceY = trailingLastPriceY;

        int i = ++trailingIdx;
        while (i < startIdx)
        {
            tmpReal = inReal0[i];
            x = !Core.TA_IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : Decimal.Zero;
            lastPriceX = tmpReal;

            tmpReal = inReal1[i++];
            y = !Core.TA_IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : Decimal.Zero;
            lastPriceY = tmpReal;

            sxx += x * x;
            sxy += x * y;
            sx += x;
            sy += y;
        }

        int outIdx = default;
        do
        {
            tmpReal = inReal0[i];
            x = !Core.TA_IsZero(lastPriceX) ? (tmpReal - lastPriceX) / lastPriceX : Decimal.Zero;
            lastPriceX = tmpReal;

            tmpReal = inReal1[i++];
            y = !Core.TA_IsZero(lastPriceY) ? (tmpReal - lastPriceY) / lastPriceY : Decimal.Zero;
            lastPriceY = tmpReal;

            sxx += x * x;
            sxy += x * y;
            sx += x;
            sy += y;

            tmpReal = inReal0[trailingIdx];
            x = !Core.TA_IsZero(trailingLastPriceX) ? (tmpReal - trailingLastPriceX) / trailingLastPriceX : Decimal.Zero;
            trailingLastPriceX = tmpReal;

            tmpReal = inReal1[trailingIdx++];
            y = !Core.TA_IsZero(trailingLastPriceY) ? (tmpReal - trailingLastPriceY) / trailingLastPriceY : Decimal.Zero;
            trailingLastPriceY = tmpReal;

            tmpReal = optInTimePeriod * sxx - sx * sx;
            outReal[outIdx++] = !Core.TA_IsZero(tmpReal) ? (optInTimePeriod * sxy - sx * sy) / tmpReal : Decimal.Zero;

            sxx -= x * x;
            sxy -= x * y;
            sx -= x;
            sy -= y;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int BetaLookback(int optInTimePeriod = 5)
    {
        if (optInTimePeriod is < 1 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod;
    }
}
