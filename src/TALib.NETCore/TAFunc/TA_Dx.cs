namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Dx(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
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
        double prevMinusDM = default;
        double prevPlusDM = default;
        double prevTR = default;
        int today = startIdx - lookbackTotal;
        double prevHigh = inHigh[today];
        double prevLow = inLow[today];
        double prevClose = inClose[today];
        int i = optInTimePeriod - 1;
        while (i-- > 0)
        {
            today++;
            double tempReal = inHigh[today];
            double diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            double diffM = prevLow - tempReal;
            prevLow = tempReal;

            if (diffM > 0.0 && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > 0.0 && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            Core.TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR += tempReal;
            prevClose = inClose[today];
        }

        i = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Dx) + 1;
        while (i-- != 0)
        {
            today++;
            double tempReal = inHigh[today];
            double diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            double diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / optInTimePeriod;
            prevPlusDM -= prevPlusDM / optInTimePeriod;

            if (diffM > 0.0 && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > 0.0 && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            Core.TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
            prevClose = inClose[today];
        }

        if (!Core.TA_IsZero(prevTR))
        {
            double minusDI = 100.0 * (prevMinusDM / prevTR);
            double plusDI = 100.0 * (prevPlusDM / prevTR);
            double tempReal = minusDI + plusDI;
            outReal[0] = !Core.TA_IsZero(tempReal) ? 100.0 * (Math.Abs(minusDI - plusDI) / tempReal) : 0.0;
        }
        else
        {
            outReal[0] = 0.0;
        }

        var outIdx = 1;
        while (today < endIdx)
        {
            today++;
            double tempReal = inHigh[today];
            double diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            double diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / optInTimePeriod;
            prevPlusDM -= prevPlusDM / optInTimePeriod;

            if (diffM > 0.0 && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > 0.0 && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            Core.TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
            prevClose = inClose[today];

            if (!Core.TA_IsZero(prevTR))
            {
                double minusDI = 100.0 * (prevMinusDM / prevTR);
                double plusDI = 100.0 * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (!Core.TA_IsZero(tempReal))
                {
                    outReal[outIdx] = 100.0 * (Math.Abs(minusDI - plusDI) / tempReal);
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

    public static Core.RetCode Dx(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
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
        decimal prevMinusDM = default;
        decimal prevPlusDM = default;
        decimal prevTR = default;
        int today = startIdx - lookbackTotal;
        decimal prevHigh = inHigh[today];
        decimal prevLow = inLow[today];
        decimal prevClose = inClose[today];
        int i = optInTimePeriod - 1;
        while (i-- > 0)
        {
            today++;
            decimal tempReal = inHigh[today];
            decimal diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            decimal diffM = prevLow - tempReal;
            prevLow = tempReal;

            if (diffM > Decimal.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > Decimal.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            Core.TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR += tempReal;
            prevClose = inClose[today];
        }

        i = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Dx) + 1;
        while (i-- != 0)
        {
            today++;
            decimal tempReal = inHigh[today];
            decimal diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            decimal diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / optInTimePeriod;
            prevPlusDM -= prevPlusDM / optInTimePeriod;

            if (diffM > Decimal.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > Decimal.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            Core.TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
            prevClose = inClose[today];
        }

        if (!Core.TA_IsZero(prevTR))
        {
            decimal minusDI = 100m * (prevMinusDM / prevTR);
            decimal plusDI = 100m * (prevPlusDM / prevTR);
            decimal tempReal = minusDI + plusDI;
            outReal[0] = !Core.TA_IsZero(tempReal) ? 100m * (Math.Abs(minusDI - plusDI) / tempReal) : Decimal.Zero;
        }
        else
        {
            outReal[0] = Decimal.Zero;
        }

        var outIdx = 1;
        while (today < endIdx)
        {
            today++;
            decimal tempReal = inHigh[today];
            decimal diffP = tempReal - prevHigh;
            prevHigh = tempReal;

            tempReal = inLow[today];
            decimal diffM = prevLow - tempReal;
            prevLow = tempReal;

            prevMinusDM -= prevMinusDM / optInTimePeriod;
            prevPlusDM -= prevPlusDM / optInTimePeriod;

            if (diffM > Decimal.Zero && diffP < diffM)
            {
                prevMinusDM += diffM;
            }
            else if (diffP > Decimal.Zero && diffP > diffM)
            {
                prevPlusDM += diffP;
            }

            Core.TrueRange(prevHigh, prevLow, prevClose, out tempReal);
            prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
            prevClose = inClose[today];

            if (!Core.TA_IsZero(prevTR))
            {
                decimal minusDI = 100m * (prevMinusDM / prevTR);
                decimal plusDI = 100m * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (!Core.TA_IsZero(tempReal))
                {
                    outReal[outIdx] = 100m * (Math.Abs(minusDI - plusDI) / tempReal);
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

    public static int DxLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Dx);
    }
}
