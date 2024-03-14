namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Cmo(
        double[] inReal,
        int startIdx,
        int endIdx,
        double[] outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = CmoLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double prevLoss;
        double prevGain;
        double tempValue1;
        double tempValue2;
        int outIdx = default;
        int today = startIdx - lookbackTotal;
        double prevValue = inReal[today];
        if (Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Cmo) == 0 && Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            double savePrevValue = prevValue;
            prevGain = 0.0;
            prevLoss = 0.0;
            for (int i = optInTimePeriod; i > 0; i--)
            {
                tempValue1 = inReal[today++];
                tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;
                if (tempValue2 < 0.0)
                {
                    prevLoss -= tempValue2;
                }
                else
                {
                    prevGain += tempValue2;
                }
            }

            tempValue1 = prevLoss / optInTimePeriod;
            tempValue2 = prevGain / optInTimePeriod;
            double tempValue3 = tempValue2 - tempValue1;
            double tempValue4 = tempValue1 + tempValue2;
            outReal[outIdx++] = !Core.TA_IsZero(tempValue4) ? 100.0 * (tempValue3 / tempValue4) : 0.0;

            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNbElement = outIdx;

                return Core.RetCode.Success;
            }

            today -= optInTimePeriod;
            prevValue = savePrevValue;
        }

        prevGain = 0.0;
        prevLoss = 0.0;
        today++;
        for (int i = optInTimePeriod; i > 0; i--)
        {
            tempValue1 = inReal[today++];
            tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;
            if (tempValue2 < 0.0)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }
        }

        prevLoss /= optInTimePeriod;
        prevGain /= optInTimePeriod;

        if (today > startIdx)
        {
            tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !Core.TA_IsZero(tempValue1) ? 100.0 * ((prevGain - prevLoss) / tempValue1) : 0.0;
        }
        else
        {
            while (today < startIdx)
            {
                tempValue1 = inReal[today];
                tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;

                prevLoss *= optInTimePeriod - 1;
                prevGain *= optInTimePeriod - 1;
                if (tempValue2 < 0.0)
                {
                    prevLoss -= tempValue2;
                }
                else
                {
                    prevGain += tempValue2;
                }

                prevLoss /= optInTimePeriod;
                prevGain /= optInTimePeriod;

                today++;
            }
        }

        while (today <= endIdx)
        {
            tempValue1 = inReal[today++];
            tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;

            prevLoss *= optInTimePeriod - 1;
            prevGain *= optInTimePeriod - 1;
            if (tempValue2 < 0.0)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }

            prevLoss /= optInTimePeriod;
            prevGain /= optInTimePeriod;
            tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !Core.TA_IsZero(tempValue1) ? 100.0 * ((prevGain - prevLoss) / tempValue1) : 0.0;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Cmo(
        decimal[] inReal,
        int startIdx,
        int endIdx,
        decimal[] outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = CmoLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal prevLoss;
        decimal prevGain;
        decimal tempValue1;
        decimal tempValue2;
        int outIdx = default;
        int today = startIdx - lookbackTotal;
        decimal prevValue = inReal[today];
        if (Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Cmo) == 0 && Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            decimal savePrevValue = prevValue;
            prevGain = Decimal.Zero;
            prevLoss = Decimal.Zero;
            for (int i = optInTimePeriod; i > 0; i--)
            {
                tempValue1 = inReal[today++];
                tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;
                if (tempValue2 < Decimal.Zero)
                {
                    prevLoss -= tempValue2;
                }
                else
                {
                    prevGain += tempValue2;
                }
            }

            tempValue1 = prevLoss / optInTimePeriod;
            tempValue2 = prevGain / optInTimePeriod;
            decimal tempValue3 = tempValue2 - tempValue1;
            decimal tempValue4 = tempValue1 + tempValue2;
            outReal[outIdx++] = !Core.TA_IsZero(tempValue4) ? 100m * (tempValue3 / tempValue4) : Decimal.Zero;

            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNbElement = outIdx;

                return Core.RetCode.Success;
            }

            today -= optInTimePeriod;
            prevValue = savePrevValue;
        }

        prevGain = Decimal.Zero;
        prevLoss = Decimal.Zero;
        today++;
        for (int i = optInTimePeriod; i > 0; i--)
        {
            tempValue1 = inReal[today++];
            tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;
            if (tempValue2 < Decimal.Zero)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }
        }

        prevLoss /= optInTimePeriod;
        prevGain /= optInTimePeriod;

        if (today > startIdx)
        {
            tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !Core.TA_IsZero(tempValue1) ? 100m * ((prevGain - prevLoss) / tempValue1) : Decimal.Zero;
        }
        else
        {
            while (today < startIdx)
            {
                tempValue1 = inReal[today];
                tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;
                prevLoss *= optInTimePeriod - 1;
                prevGain *= optInTimePeriod - 1;
                if (tempValue2 < Decimal.Zero)
                {
                    prevLoss -= tempValue2;
                }
                else
                {
                    prevGain += tempValue2;
                }

                prevLoss /= optInTimePeriod;
                prevGain /= optInTimePeriod;

                today++;
            }
        }

        while (today <= endIdx)
        {
            tempValue1 = inReal[today++];
            tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;

            prevLoss *= optInTimePeriod - 1;
            prevGain *= optInTimePeriod - 1;
            if (tempValue2 < Decimal.Zero)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }

            prevLoss /= optInTimePeriod;
            prevGain /= optInTimePeriod;
            tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !Core.TA_IsZero(tempValue1) ? 100m * ((prevGain - prevLoss) / tempValue1) : Decimal.Zero;
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
