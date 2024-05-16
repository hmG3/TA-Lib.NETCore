namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode StochF(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outFastK,
        T[] outFastD, out int outBegIdx, out int outNbElement, int optInFastKPeriod = 5, int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outFastK == null || outFastD == null ||
            optInFastKPeriod is < 1 or > 100000 || optInFastDPeriod is < 1 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackK = optInFastKPeriod - 1;
        int lookbackFastD = MaLookback(optInFastDPeriod, optInFastDMAType);
        int lookbackTotal = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
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
        if (outFastK == inHigh || outFastK == inLow || outFastK == inClose)
        {
            tempBuffer = outFastK;
        }
        else if (outFastD == inHigh || outFastD == inLow || outFastD == inClose)
        {
            tempBuffer = outFastD;
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

        Core.RetCode retCode = Ma(tempBuffer, 0, outIdx - 1, outFastD, out _, out outNbElement, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        Array.Copy(tempBuffer, lookbackFastD, outFastK, 0, outNbElement);
        outBegIdx = startIdx;

        return Core.RetCode.Success;
    }

    public static int StochFLookback(int optInFastKPeriod = 5, int optInFastDPeriod = 3, Core.MAType optInFastDMAType = Core.MAType.Sma) =>
        optInFastKPeriod is < 1 or > 100000 || optInFastDPeriod is < 1 or > 100000
            ? -1
            : optInFastKPeriod - 1 + MaLookback(optInFastDPeriod, optInFastDMAType);
}
