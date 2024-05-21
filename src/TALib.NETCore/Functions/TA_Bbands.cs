namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Bbands(T[] inReal, int startIdx, int endIdx, T[] outRealUpperBand, T[] outRealMiddleBand,
        T[] outRealLowerBand, out int outBegIdx, out int outNbElement, int optInTimePeriod = 5, double optInNbDevUp = 2.0,
        double optInNbDevDn = 2.0, Core.MAType optInMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outRealUpperBand == null || outRealMiddleBand == null || outRealLowerBand == null || optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        T[] tempBuffer1;
        T[] tempBuffer2;
        if (inReal == outRealUpperBand)
        {
            tempBuffer1 = outRealMiddleBand;
            tempBuffer2 = outRealLowerBand;
        }
        else if (inReal == outRealLowerBand)
        {
            tempBuffer1 = outRealMiddleBand;
            tempBuffer2 = outRealUpperBand;
        }
        else if (inReal == outRealMiddleBand)
        {
            tempBuffer1 = outRealLowerBand;
            tempBuffer2 = outRealUpperBand;
        }
        else
        {
            tempBuffer1 = outRealMiddleBand;
            tempBuffer2 = outRealUpperBand;
        }

        if (tempBuffer1 == inReal || tempBuffer2 == inReal)
        {
            return Core.RetCode.BadParam;
        }

        Core.RetCode retCode = Ma(inReal, startIdx, endIdx, tempBuffer1, out outBegIdx, out outNbElement, optInTimePeriod, optInMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        if (optInMAType == Core.MAType.Sma)
        {
            CalcStandardDeviation(inReal, tempBuffer1, outBegIdx, outNbElement, tempBuffer2, optInTimePeriod);
        }
        else
        {
            retCode = StdDev(inReal, outBegIdx, endIdx, tempBuffer2, out outBegIdx, out outNbElement, optInTimePeriod);
            if (retCode != Core.RetCode.Success)
            {
                outNbElement = 0;

                return retCode;
            }
        }

        if (tempBuffer1 != outRealMiddleBand)
        {
            Array.Copy(tempBuffer1, 0, outRealMiddleBand, 0, outNbElement);
        }

        T tOptInNbDevUp = T.CreateChecked(optInNbDevUp);
        T tOptInNbDevDn = T.CreateChecked(optInNbDevDn);

        T tempReal;
        T tempReal2;
        if (optInNbDevUp.Equals(optInNbDevDn))
        {
            if (tOptInNbDevUp == T.One)
            {
                for (var i = 0; i < outNbElement; i++)
                {
                    tempReal = tempBuffer2[i];
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal;
                    outRealLowerBand[i] = tempReal2 - tempReal;
                }
            }
            else
            {
                for (var i = 0; i < outNbElement; i++)
                {
                    tempReal = tempBuffer2[i] * tOptInNbDevUp;
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal;
                    outRealLowerBand[i] = tempReal2 - tempReal;
                }
            }
        }
        else if (tOptInNbDevUp == T.One)
        {
            for (var i = 0; i < outNbElement; i++)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealUpperBand[i] = tempReal2 + tempReal;
                outRealLowerBand[i] = tempReal2 - tempReal * tOptInNbDevDn;
            }
        }
        else if (tOptInNbDevDn == T.One)
        {
            for (var i = 0; i < outNbElement; i++)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealLowerBand[i] = tempReal2 - tempReal;
                outRealUpperBand[i] = tempReal2 + tempReal * tOptInNbDevUp;
            }
        }
        else
        {
            for (var i = 0; i < outNbElement; i++)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealUpperBand[i] = tempReal2 + tempReal * tOptInNbDevUp;
                outRealLowerBand[i] = tempReal2 - tempReal * tOptInNbDevDn;
            }
        }

        return Core.RetCode.Success;
    }

    public static int BbandsLookback(int optInTimePeriod = 5, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInTimePeriod < 2 ? -1 : MaLookback(optInTimePeriod, optInMAType);
}
