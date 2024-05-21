namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Trix(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        int emaLookback = EmaLookback(optInTimePeriod);
        int lookbackTotal = TrixLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        int nbElementToOutput = endIdx - startIdx + 1 + lookbackTotal;
        var tempBuffer = new T[nbElementToOutput];

        T k = TTwo / (T.CreateChecked(optInTimePeriod) + T.One);
        Core.RetCode retCode =
            CalcExponentialMA(inReal, startIdx - lookbackTotal, endIdx, tempBuffer, out _, out var nbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || nbElement == 0)
        {
            return retCode;
        }

        nbElementToOutput--;

        nbElementToOutput -= emaLookback;
        retCode = CalcExponentialMA(tempBuffer, 0, nbElementToOutput, tempBuffer, out _, out nbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || nbElement == 0)
        {
            return retCode;
        }

        nbElementToOutput -= emaLookback;
        retCode = CalcExponentialMA(tempBuffer, 0, nbElementToOutput, tempBuffer, out _, out nbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || nbElement == 0)
        {
            return retCode;
        }

        nbElementToOutput -= emaLookback;
        retCode = Roc(tempBuffer, 0, nbElementToOutput, outReal, out _, out outNbElement, 1);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        return Core.RetCode.Success;
    }

    public static int TrixLookback(int optInTimePeriod = 30) =>
        optInTimePeriod < 1 ? -1 : EmaLookback(optInTimePeriod) * 3 + RocRLookback(1);
}
