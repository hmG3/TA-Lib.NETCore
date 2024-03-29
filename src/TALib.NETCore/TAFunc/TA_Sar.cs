namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Sar(double[] inHigh, double[] inLow, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
        out int outNbElement, double optInAcceleration = 0.02, double optInMaximum = 0.2)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || outReal == null || optInAcceleration < 0.0 || optInMaximum < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = SarLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double af = optInAcceleration;
        if (af > optInMaximum)
        {
            af = optInAcceleration = optInMaximum;
        }

        var epTemp = new double[1];
        Core.RetCode retCode = MinusDM(inHigh, inLow, startIdx, startIdx, epTemp, out _, out _, 1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        outBegIdx = startIdx;
        double sar;
        double ep;
        int outIdx = default;

        int todayIdx = startIdx;

        double newHigh = inHigh[todayIdx - 1];
        double newLow = inLow[todayIdx - 1];
        var isLong = epTemp[0] <= 0.0;
        if (isLong)
        {
            ep = inHigh[todayIdx];
            sar = newLow;
        }
        else
        {
            ep = inLow[todayIdx];
            sar = newHigh;
        }

        newLow = inLow[todayIdx];
        newHigh = inHigh[todayIdx];

        while (todayIdx <= endIdx)
        {
            double prevLow = newLow;
            double prevHigh = newHigh;
            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];
            todayIdx++;

            if (isLong)
            {
                if (newLow <= sar)
                {
                    isLong = false;
                    sar = ep;

                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }

                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }

                    outReal[outIdx++] = sar;

                    af = optInAcceleration;
                    ep = newLow;

                    sar += af * (ep - sar);
                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }

                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }
                }
                else
                {
                    outReal[outIdx++] = sar;
                    if (newHigh > ep)
                    {
                        ep = newHigh;
                        af += optInAcceleration;
                        if (af > optInMaximum)
                        {
                            af = optInMaximum;
                        }
                    }

                    sar += af * (ep - sar);
                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }

                    if (sar > newLow)
                    {
                        sar = newLow;
                    }
                }
            }
            else if (newHigh >= sar)
            {
                isLong = true;
                sar = ep;

                if (sar > prevLow)
                {
                    sar = prevLow;
                }

                if (sar > newLow)
                {
                    sar = newLow;
                }

                outReal[outIdx++] = sar;

                af = optInAcceleration;
                ep = newHigh;

                sar += af * (ep - sar);
                if (sar > prevLow)
                {
                    sar = prevLow;
                }

                if (sar > newLow)
                {
                    sar = newLow;
                }
            }
            else
            {
                outReal[outIdx++] = sar;

                if (newLow < ep)
                {
                    ep = newLow;
                    af += optInAcceleration;
                    if (af > optInMaximum)
                    {
                        af = optInMaximum;
                    }
                }

                sar += af * (ep - sar);

                if (sar < prevHigh)
                {
                    sar = prevHigh;
                }

                if (sar < newHigh)
                {
                    sar = newHigh;
                }
            }
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Sar(decimal[] inHigh, decimal[] inLow, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
        out int outNbElement, decimal optInAcceleration = 0.02m, decimal optInMaximum = 0.2m)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || outReal == null || optInAcceleration < Decimal.Zero || optInMaximum < Decimal.Zero)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = SarLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal af = optInAcceleration;
        if (af > optInMaximum)
        {
            af = optInAcceleration = optInMaximum;
        }

        var epTemp = new decimal[1];
        Core.RetCode retCode = MinusDM(inHigh, inLow, startIdx, startIdx, epTemp, out _, out _, 1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        outBegIdx = startIdx;
        decimal sar;
        decimal ep;
        int outIdx = default;

        int todayIdx = startIdx;

        decimal newHigh = inHigh[todayIdx - 1];
        decimal newLow = inLow[todayIdx - 1];
        var isLong = epTemp[0] <= Decimal.Zero;
        if (isLong)
        {
            ep = inHigh[todayIdx];
            sar = newLow;
        }
        else
        {
            ep = inLow[todayIdx];
            sar = newHigh;
        }

        newLow = inLow[todayIdx];
        newHigh = inHigh[todayIdx];

        while (todayIdx <= endIdx)
        {
            decimal prevLow = newLow;
            decimal prevHigh = newHigh;
            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];
            todayIdx++;

            if (isLong)
            {
                if (newLow <= sar)
                {
                    isLong = false;
                    sar = ep;

                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }

                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }

                    outReal[outIdx++] = sar;

                    af = optInAcceleration;
                    ep = newLow;

                    sar += af * (ep - sar);
                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }

                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }
                }
                else
                {
                    outReal[outIdx++] = sar;
                    if (newHigh > ep)
                    {
                        ep = newHigh;
                        af += optInAcceleration;
                        if (af > optInMaximum)
                        {
                            af = optInMaximum;
                        }
                    }

                    sar += af * (ep - sar);
                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }

                    if (sar > newLow)
                    {
                        sar = newLow;
                    }
                }
            }
            else if (newHigh >= sar)
            {
                isLong = true;
                sar = ep;

                if (sar > prevLow)
                {
                    sar = prevLow;
                }

                if (sar > newLow)
                {
                    sar = newLow;
                }

                outReal[outIdx++] = sar;

                af = optInAcceleration;
                ep = newHigh;

                sar += af * (ep - sar);
                if (sar > prevLow)
                {
                    sar = prevLow;
                }

                if (sar > newLow)
                {
                    sar = newLow;
                }
            }
            else
            {
                outReal[outIdx++] = sar;

                if (newLow < ep)
                {
                    ep = newLow;
                    af += optInAcceleration;
                    if (af > optInMaximum)
                    {
                        af = optInMaximum;
                    }
                }

                sar += af * (ep - sar);

                if (sar < prevHigh)
                {
                    sar = prevHigh;
                }

                if (sar < newHigh)
                {
                    sar = newHigh;
                }
            }
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int SarLookback() => 1;
}
