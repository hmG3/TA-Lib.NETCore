namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Atr(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AtrLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        if (optInTimePeriod == 1)
        {
            return TRange(inHigh, inLow, inClose, startIdx, endIdx, outReal, out outBegIdx, out outNbElement);
        }

        var prevATRTemp = new double[1];

        var tempBuffer = new double[lookbackTotal + (endIdx - startIdx) + 1];
        Core.RetCode retCode = TRange(inHigh, inLow, inClose, startIdx - lookbackTotal + 1, endIdx, tempBuffer, out _, out _);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        retCode = Core.TA_INT_SMA(tempBuffer, optInTimePeriod - 1, optInTimePeriod - 1, prevATRTemp, out _, out _, optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        double prevATR = prevATRTemp[0];
        int today = optInTimePeriod;
        int outIdx = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Atr);
        while (outIdx != 0)
        {
            prevATR *= optInTimePeriod - 1;
            prevATR += tempBuffer[today++];
            prevATR /= optInTimePeriod;
            outIdx--;
        }

        outIdx = 1;
        outReal[0] = prevATR;

        int nbATR = endIdx - startIdx + 1;

        while (--nbATR != 0)
        {
            prevATR *= optInTimePeriod - 1;
            prevATR += tempBuffer[today++];
            prevATR /= optInTimePeriod;
            outReal[outIdx++] = prevATR;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return retCode;
    }

    public static Core.RetCode Atr(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AtrLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        if (optInTimePeriod == 1)
        {
            return TRange(inHigh, inLow, inClose, startIdx, endIdx, outReal, out outBegIdx, out outNbElement);
        }

        var prevATRTemp = new decimal[1];

        var tempBuffer = new decimal[lookbackTotal + (endIdx - startIdx) + 1];
        Core.RetCode retCode = TRange(inHigh, inLow, inClose, startIdx - lookbackTotal + 1, endIdx, tempBuffer, out _, out _);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        retCode = Core.TA_INT_SMA(tempBuffer, optInTimePeriod - 1, optInTimePeriod - 1, prevATRTemp, out _, out _, optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        decimal prevATR = prevATRTemp[0];
        int today = optInTimePeriod;
        int outIdx = Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Atr);
        while (outIdx != 0)
        {
            prevATR *= optInTimePeriod - 1;
            prevATR += tempBuffer[today++];
            prevATR /= optInTimePeriod;
            outIdx--;
        }

        outIdx = 1;
        outReal[0] = prevATR;

        int nbATR = endIdx - startIdx + 1;

        while (--nbATR != 0)
        {
            prevATR *= optInTimePeriod - 1;
            prevATR += tempBuffer[today++];
            prevATR /= optInTimePeriod;
            outReal[outIdx++] = prevATR;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return retCode;
    }

    public static int AtrLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod is < 1 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Atr);
    }
}
