namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Tema(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
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

        int lookbackEMA = EmaLookback(optInTimePeriod);
        int lookbackTotal = TemaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int tempInt = lookbackTotal + (endIdx - startIdx) + 1;
        T k = TTwo / (T.CreateChecked(optInTimePeriod) + T.One);

        var firstEMA = new T[tempInt];
        Core.RetCode retCode = TA_INT_EMA(inReal, startIdx - lookbackEMA * 2, endIdx, firstEMA, out var firstEMABegIdx,
            out var firstEMANbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || firstEMANbElement == 0)
        {
            return retCode;
        }

        var secondEMA = new T[firstEMANbElement];
        retCode = TA_INT_EMA(firstEMA, 0, firstEMANbElement - 1, secondEMA, out var secondEMABegIdx, out var secondEMANbElement,
            optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || secondEMANbElement == 0)
        {
            return retCode;
        }

        retCode = TA_INT_EMA(secondEMA, 0, secondEMANbElement - 1, outReal, out var thirdEMABegIdx, out var thirdEMANbElement,
            optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || thirdEMANbElement == 0)
        {
            return retCode;
        }

        int firstEMAIdx = thirdEMABegIdx + secondEMABegIdx;
        int secondEMAIdx = thirdEMABegIdx;
        outBegIdx = firstEMAIdx + firstEMABegIdx;
        int outIdx = default;
        while (outIdx < thirdEMANbElement)
        {
            outReal[outIdx++] += TThree * firstEMA[firstEMAIdx++] - TThree * secondEMA[secondEMAIdx++];
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TemaLookback(int optInTimePeriod = 30) =>
        optInTimePeriod switch
        {
            < 2 or > 100000 => -1,
            _ => EmaLookback(optInTimePeriod) * 3
        };
}
