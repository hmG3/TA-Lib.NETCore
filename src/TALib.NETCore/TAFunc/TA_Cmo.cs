namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Cmo(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        out int outBegIdx,
        out int outNbElement,
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

        var lookbackTotal = CmoLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        T prevLoss;
        T prevGain;
        T tempValue1;
        T tempValue2;
        int outIdx = default;
        var today = startIdx - lookbackTotal;
        T prevValue = inReal[today];
        if (Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Cmo) == 0 && Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            T savePrevValue = prevValue;
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
            T tempValue3 = tempValue2 - tempValue1;
            T tempValue4 = tempValue1 + tempValue2;
            outReal[outIdx++] = !TA_IsZero(tempValue4) ? THundred * (tempValue3 / tempValue4) : T.Zero;

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

        prevLoss /= tOptInTimePeriod;
        prevGain /= tOptInTimePeriod;

        if (today > startIdx)
        {
            tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !TA_IsZero(tempValue1) ? THundred * ((prevGain - prevLoss) / tempValue1) : T.Zero;
        }
        else
        {
            while (today < startIdx)
            {
                tempValue1 = inReal[today];
                tempValue2 = tempValue1 - prevValue;
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
            tempValue1 = inReal[today++];
            tempValue2 = tempValue1 - prevValue;
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
            outReal[outIdx++] = !TA_IsZero(tempValue1) ? THundred * ((prevGain - prevLoss) / tempValue1) : T.Zero;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CmoLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        int retValue = optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Cmo);
        if (Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            retValue--;
        }

        return retValue;
    }
}
