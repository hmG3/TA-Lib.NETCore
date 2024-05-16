namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Accbands(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
        T[] outRealUpperBand, T[] outRealMiddleBand, T[] outRealLowerBand, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 20)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outRealUpperBand == null || outRealMiddleBand == null ||
            outRealLowerBand == null || optInTimePeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AccbandsLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outputSize = endIdx - startIdx + 1;
        int bufferSize = outputSize + lookbackTotal;
        var tempBuffer1 = new T[bufferSize];
        var tempBuffer2 = new T[bufferSize];

        for (int j = 0, i = startIdx - lookbackTotal; i <= endIdx; i++, j++)
        {
            T tempReal = inHigh[i] + inLow[i];
            if (!TA_IsZero(tempReal))
            {
                tempReal = TFour * (inHigh[i] - inLow[i]) / tempReal;
                tempBuffer1[j] = inHigh[i] * (T.One + tempReal);
                tempBuffer2[j] = inLow[i] * (T.One - tempReal);
            }
            else
            {
                tempBuffer1[j] = inHigh[i];
                tempBuffer2[j] = inLow[i];
            }
        }

        var retCode = Sma(inClose, startIdx, endIdx, outRealMiddleBand, out _, out var outNbElementDummy, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElementDummy != outputSize)
        {
            return retCode;
        }

        retCode = Sma(tempBuffer1, 0, bufferSize - 1, outRealUpperBand, out _, out outNbElementDummy, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElementDummy != outputSize)
        {
            return retCode;
        }

        retCode = Sma(tempBuffer2, 0, bufferSize - 1, outRealLowerBand, out _, out outNbElementDummy, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElementDummy != outputSize)
        {
            return retCode;
        }

        outBegIdx = startIdx;
        outNbElement = outputSize;

        return Core.RetCode.Success;
    }

    public static int AccbandsLookback(int optInTimePeriod = 20)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        return SmaLookback(optInTimePeriod);
    }
}
