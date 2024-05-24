namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Atr(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 1)
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

        var prevATRTemp = new T[1];

        var tempBuffer = new T[lookbackTotal + (endIdx - startIdx) + 1];
        Core.RetCode retCode = TRange(inHigh, inLow, inClose, startIdx - lookbackTotal + 1, endIdx, tempBuffer, out _, out _);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        retCode = CalcSimpleMA(tempBuffer, optInTimePeriod - 1, optInTimePeriod - 1, prevATRTemp, out _, out _, optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        T prevATR = prevATRTemp[0];
        int today = optInTimePeriod;
        int outIdx = Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Atr);
        while (outIdx != 0)
        {
            prevATR *= tOptInTimePeriod - T.One;
            prevATR += tempBuffer[today++];
            prevATR /= tOptInTimePeriod;
            outIdx--;
        }

        outIdx = 1;
        outReal[0] = prevATR;

        int nbATR = endIdx - startIdx + 1;

        while (--nbATR != 0)
        {
            prevATR *= tOptInTimePeriod - T.One;
            prevATR += tempBuffer[today++];
            prevATR /= tOptInTimePeriod;
            outReal[outIdx++] = prevATR;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return retCode;
    }

    public static int AtrLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 1 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Atr);
}
