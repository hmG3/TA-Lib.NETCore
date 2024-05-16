namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Rsi(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = RsiLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        int outIdx = default;
        int today = startIdx - lookbackTotal;
        T prevValue = inReal[today];
        T prevGain;
        T prevLoss;
        if (Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Rsi) == 0 && Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            T savePrevValue = prevValue;
            T tempValue1;
            T tempValue2;
            prevGain = T.Zero;
            prevLoss = T.Zero;
            for (var i = optInTimePeriod; i > 0; i--)
            {
                tempValue1 = inReal[today++];
                tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;
                if (tempValue2 < T.Zero)
                {
                    prevLoss -= tempValue2;
                }
                else
                {
                    prevGain += tempValue2;
                }
            }

            tempValue1 = prevLoss / tOptInTimePeriod;
            tempValue2 = prevGain / tOptInTimePeriod;

            tempValue1 = tempValue2 + tempValue1;
            outReal[outIdx++] = !TA_IsZero(tempValue1) ? THundred * (tempValue2 / tempValue1) : T.Zero;

            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNbElement = outIdx;
                return Core.RetCode.Success;
            }

            today -= optInTimePeriod;
            prevValue = savePrevValue;
        }

        prevGain = T.Zero;
        prevLoss = T.Zero;
        today++;
        for (var i = optInTimePeriod; i > 0; i--)
        {
            T tempValue1 = inReal[today++];
            T tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;
            if (tempValue2 < T.Zero)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }
        }

        prevLoss /= tOptInTimePeriod;
        prevGain /= tOptInTimePeriod;

        if (today > startIdx)
        {
            T tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !TA_IsZero(tempValue1) ? THundred * (prevGain / tempValue1) : T.Zero;
        }
        else
        {
            while (today < startIdx)
            {
                T tempValue1 = inReal[today];
                T tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;

                prevLoss *= tOptInTimePeriod - T.One;
                prevGain *= tOptInTimePeriod - T.One;
                if (tempValue2 < T.Zero)
                {
                    prevLoss -= tempValue2;
                }
                else
                {
                    prevGain += tempValue2;
                }

                prevLoss /= tOptInTimePeriod;
                prevGain /= tOptInTimePeriod;

                today++;
            }
        }

        while (today <= endIdx)
        {
            T tempValue1 = inReal[today++];
            T tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;

            prevLoss *= tOptInTimePeriod - T.One;
            prevGain *= tOptInTimePeriod - T.One;
            if (tempValue2 < T.Zero)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }

            prevLoss /= tOptInTimePeriod;
            prevGain /= tOptInTimePeriod;
            tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !TA_IsZero(tempValue1) ? THundred * (prevGain / tempValue1) : T.Zero;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int RsiLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        int retValue = optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Rsi);
        if (Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            retValue--;
        }

        return retValue;
    }
}
