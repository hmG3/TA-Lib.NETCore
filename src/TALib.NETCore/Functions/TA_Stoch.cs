namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Stoch(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outSlowK,
        T[] outSlowD, out int outBegIdx, out int outNbElement, int optInFastKPeriod = 5, int optInSlowKPeriod = 3,
        Core.MAType optInSlowKMAType = Core.MAType.Sma, int optInSlowDPeriod = 3, Core.MAType optInSlowDMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outSlowK == null || outSlowD == null ||
            optInFastKPeriod < 1 || optInSlowKPeriod < 1 || optInSlowDPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackK = optInFastKPeriod - 1;
        int lookbackDSlow = MaLookback(optInSlowDPeriod, optInSlowDMAType);
        int lookbackTotal = StochLookback(optInFastKPeriod, optInSlowKPeriod, optInSlowKMAType, optInSlowDPeriod, optInSlowDMAType);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        int trailingIdx = startIdx - lookbackTotal;
        int today = trailingIdx + lookbackK;
        int highestIdx = -1;
        int lowestIdx = highestIdx;
        T highest, lowest;
        T diff = highest = lowest = T.Zero;
        T[] tempBuffer;
        if (outSlowK == inHigh || outSlowK == inLow || outSlowK == inClose)
        {
            tempBuffer = outSlowK;
        }
        else if (outSlowD == inHigh || outSlowD == inLow || outSlowD == inClose)
        {
            tempBuffer = outSlowD;
        }
        else
        {
            tempBuffer = new T[endIdx - today + 1];
        }

        while (today <= endIdx)
        {
            T tmp = inLow[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inLow[lowestIdx];
                int i = lowestIdx;
                while (++i <= today)
                {
                    tmp = inLow[i];
                    if (tmp < lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }

                diff = (highest - lowest) / THundred;
            }
            else if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
                diff = (highest - lowest) / THundred;
            }

            tmp = inHigh[today];
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inHigh[highestIdx];
                int i = highestIdx;
                while (++i <= today)
                {
                    tmp = inHigh[i];
                    if (tmp > highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }

                diff = (highest - lowest) / THundred;
            }
            else if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
                diff = (highest - lowest) / THundred;
            }

            tempBuffer[outIdx++] = !T.IsZero(diff) ? (inClose[today] - lowest) / diff : T.Zero;

            trailingIdx++;
            today++;
        }

        Core.RetCode retCode = Ma(tempBuffer, 0, outIdx - 1, tempBuffer, out _, out outNbElement, optInSlowKPeriod, optInSlowKMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        retCode = Ma(tempBuffer, 0, outNbElement - 1, outSlowD, out _, out outNbElement, optInSlowDPeriod, optInSlowDMAType);
        Array.Copy(tempBuffer, lookbackDSlow, outSlowK, 0, outNbElement);
        if (retCode != Core.RetCode.Success)
        {
            outNbElement = 0;

            return retCode;
        }

        outBegIdx = startIdx;

        return Core.RetCode.Success;
    }

    public static int StochLookback(int optInFastKPeriod = 5, int optInSlowKPeriod = 3, Core.MAType optInSlowKMAType = Core.MAType.Sma,
        int optInSlowDPeriod = 3, Core.MAType optInSlowDMAType = Core.MAType.Sma)
    {
        if (optInFastKPeriod < 1 || optInSlowKPeriod < 1 || optInSlowDPeriod < 1)
        {
            return -1;
        }

        int retValue = optInFastKPeriod - 1;
        retValue += MaLookback(optInSlowKPeriod, optInSlowKMAType);
        retValue += MaLookback(optInSlowDPeriod, optInSlowDMAType);

        return retValue;
    }
}
