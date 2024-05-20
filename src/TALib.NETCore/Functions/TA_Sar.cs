namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Sar(T[] inHigh, T[] inLow, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
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

        var epTemp = new T[1];
        Core.RetCode retCode = MinusDM(inHigh, inLow, startIdx, startIdx, epTemp, out _, out _, 1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        outBegIdx = startIdx;
        T sar;
        T ep;
        int outIdx = default;

        int todayIdx = startIdx;

        T newHigh = inHigh[todayIdx - 1];
        T newLow = inLow[todayIdx - 1];
        var isLong = epTemp[0] <= T.Zero;
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
            T prevLow = newLow;
            T prevHigh = newHigh;
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

                    sar += T.CreateChecked(af) * (ep - sar);
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

                    sar += T.CreateChecked(af) * (ep - sar);
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

                sar += T.CreateChecked(af) * (ep - sar);
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

                sar += T.CreateChecked(af) * (ep - sar);

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
