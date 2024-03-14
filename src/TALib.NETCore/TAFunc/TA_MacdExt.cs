namespace TALib;

public static partial class Functions
{
    public static Core.RetCode MacdExt(double[] inReal, int startIdx, int endIdx, double[] outMACD, double[] outMACDSignal,
        double[] outMACDHist, out int outBegIdx, out int outNbElement, int optInFastPeriod = 12, Core.MAType optInFastMAType = Core.MAType.Sma,
        int optInSlowPeriod = 26, Core.MAType optInSlowMAType = Core.MAType.Sma, int optInSignalPeriod = 9, Core.MAType optInSignalMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInFastPeriod < 2 ||
            optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 || optInSignalPeriod < 1 ||
            optInSignalPeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            (optInSlowMAType, optInFastMAType) = (optInFastMAType, optInSlowMAType);
        }

        int lookbackSignal = MaLookback(optInSignalPeriod, optInSignalMAType);
        int lookbackTotal = MacdExtLookback(optInFastPeriod, optInFastMAType, optInSlowPeriod, optInSlowMAType, optInSignalPeriod,
            optInSignalMAType);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
        var fastMABuffer = new double[tempInteger];
        var slowMABuffer = new double[tempInteger];

        tempInteger = startIdx - lookbackSignal;
        Core.RetCode retCode = Ma(inReal, tempInteger, endIdx, slowMABuffer, out var outBegIdx1, out var outNbElement1, optInSlowPeriod,
            optInSlowMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        retCode = Ma(inReal, tempInteger, endIdx, fastMABuffer, out var outBegIdx2, out var outNbElement2, optInFastPeriod,
            optInFastMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
            outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
        {
            return Core.RetCode.InternalError;
        }

        for (var i = 0; i < outNbElement1; i++)
        {
            fastMABuffer[i] -= slowMABuffer[i];
        }

        Array.Copy(fastMABuffer, lookbackSignal, outMACD, 0, endIdx - startIdx + 1);
        retCode = Ma(fastMABuffer, 0, outNbElement1 - 1, outMACDSignal, out _, out outNbElement2, optInSignalPeriod, optInSignalMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        for (var i = 0; i < outNbElement2; i++)
        {
            outMACDHist[i] = outMACD[i] - outMACDSignal[i];
        }

        outBegIdx = startIdx;
        outNbElement = outNbElement2;

        return Core.RetCode.Success;
    }

    public static Core.RetCode MacdExt(decimal[] inReal, int startIdx, int endIdx, decimal[] outMACD, decimal[] outMACDSignal,
        decimal[] outMACDHist, out int outBegIdx, out int outNbElement, int optInFastPeriod = 12, Core.MAType optInFastMAType = Core.MAType.Sma,
        int optInSlowPeriod = 26, Core.MAType optInSlowMAType = Core.MAType.Sma, int optInSignalPeriod = 9, Core.MAType optInSignalMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInFastPeriod < 2 ||
            optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 || optInSignalPeriod < 1 ||
            optInSignalPeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        if (optInSlowPeriod < optInFastPeriod)
        {
            (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            (optInSlowMAType, optInFastMAType) = (optInFastMAType, optInSlowMAType);
        }

        int lookbackSignal = MaLookback(optInSignalPeriod, optInSignalMAType);
        int lookbackTotal = MacdExtLookback(optInFastPeriod, optInFastMAType, optInSlowPeriod, optInSlowMAType, optInSignalPeriod,
            optInSignalMAType);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
        var fastMABuffer = new decimal[tempInteger];
        var slowMABuffer = new decimal[tempInteger];

        tempInteger = startIdx - lookbackSignal;
        Core.RetCode retCode = Ma(inReal, tempInteger, endIdx, slowMABuffer, out var outBegIdx1, out var outNbElement1, optInSlowPeriod,
            optInSlowMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        retCode = Ma(inReal, tempInteger, endIdx, fastMABuffer, out var outBegIdx2, out var outNbElement2, optInFastPeriod,
            optInFastMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
            outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
        {
            return Core.RetCode.InternalError;
        }

        for (var i = 0; i < outNbElement1; i++)
        {
            fastMABuffer[i] -= slowMABuffer[i];
        }

        Array.Copy(fastMABuffer, lookbackSignal, outMACD, 0, endIdx - startIdx + 1);
        retCode = Ma(fastMABuffer, 0, outNbElement1 - 1, outMACDSignal, out _, out outNbElement2, optInSignalPeriod, optInSignalMAType);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        for (var i = 0; i < outNbElement2; i++)
        {
            outMACDHist[i] = outMACD[i] - outMACDSignal[i];
        }

        outBegIdx = startIdx;
        outNbElement = outNbElement2;

        return Core.RetCode.Success;
    }

    public static int MacdExtLookback(int optInFastPeriod = 12, Core.MAType optInFastMAType = Core.MAType.Sma, int optInSlowPeriod = 26,
        Core.MAType optInSlowMAType = Core.MAType.Sma, int optInSignalPeriod = 9, Core.MAType optInSignalMAType = Core.MAType.Sma)
    {
        if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 ||
            optInSignalPeriod < 1 || optInSignalPeriod > 100000)
        {
            return -1;
        }

        int lookbackLargest = MaLookback(optInFastPeriod, optInFastMAType);
        int tempInteger = MaLookback(optInSlowPeriod, optInSlowMAType);
        if (tempInteger > lookbackLargest)
        {
            lookbackLargest = tempInteger;
        }

        return lookbackLargest + MaLookback(optInSignalPeriod, optInSignalMAType);
    }
}
