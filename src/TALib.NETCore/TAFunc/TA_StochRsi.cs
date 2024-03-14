namespace TALib;

public static partial class Functions
{
    public static Core.RetCode StochRsi(double[] inReal, int startIdx, int endIdx, double[] outFastK, double[] outFastD, out int outBegIdx,
        out int outNbElement, int optInTimePeriod = 14, int optInFastKPeriod = 5, int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outFastK == null || outFastD == null || optInTimePeriod < 2 || optInTimePeriod > 100000 ||
            optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackStochF = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        int lookbackTotal = StochRsiLookback(optInTimePeriod, optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        int tempArraySize = endIdx - startIdx + 1 + lookbackStochF;
        var tempRsiBuffer = new double[tempArraySize];
        Core.RetCode retCode = Rsi(inReal, startIdx - lookbackStochF, endIdx, tempRsiBuffer, out _, out var outNbElement1, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElement1 == 0)
        {
            return retCode;
        }

        retCode = StochF(tempRsiBuffer, tempRsiBuffer, tempRsiBuffer, 0, tempArraySize - 1, outFastK, outFastD, out _, out outNbElement,
            optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        return Core.RetCode.Success;
    }

    public static Core.RetCode StochRsi(decimal[] inReal, int startIdx, int endIdx, decimal[] outFastK, decimal[] outFastD,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14, int optInFastKPeriod = 5, int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outFastK == null || outFastD == null || optInTimePeriod < 2 || optInTimePeriod > 100000 ||
            optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackSTOCHF = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        int lookbackTotal = StochRsiLookback(optInTimePeriod, optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        int tempArraySize = endIdx - startIdx + 1 + lookbackSTOCHF;
        var tempRSIBuffer = new decimal[tempArraySize];
        Core.RetCode retCode = Rsi(inReal, startIdx - lookbackSTOCHF, endIdx, tempRSIBuffer, out _, out var outNbElement1, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElement1 == 0)
        {
            return retCode;
        }

        retCode = StochF(tempRSIBuffer, tempRSIBuffer, tempRSIBuffer, 0, tempArraySize - 1, outFastK, outFastD, out _, out outNbElement,
            optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        return Core.RetCode.Success;
    }

    public static int StochRsiLookback(int optInTimePeriod = 14, int optInFastKPeriod = 5, int optInFastDPeriod = 3,
        Core.MAType optInFastDMAType = Core.MAType.Sma)
    {
        if (optInTimePeriod < 2 || optInTimePeriod > 100000 || optInFastKPeriod < 1 || optInFastKPeriod > 100000 ||
            optInFastDPeriod < 1 || optInFastDPeriod > 100000)
        {
            return -1;
        }

        return RsiLookback(optInTimePeriod) + StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
    }
}
