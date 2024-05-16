namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Dema(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
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
        int lookbackTotal = DemaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T[] firstEMA;
        if (inReal == outReal)
        {
            firstEMA = outReal;
        }
        else
        {
            int tempInt = lookbackTotal + (endIdx - startIdx) + 1;
            firstEMA = new T[tempInt];
        }

        T k = TTwo / (T.CreateChecked(optInTimePeriod) + T.One);
        Core.RetCode retCode = TA_INT_EMA(inReal, startIdx - lookbackEMA, endIdx, firstEMA, out var firstEMABegIdx,
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

        int firstEMAIdx = secondEMABegIdx;
        int outIdx = default;
        while (outIdx < secondEMANbElement)
        {
            outReal[outIdx] = TTwo * firstEMA[firstEMAIdx++] - secondEMA[outIdx];
            outIdx++;
        }

        outBegIdx = firstEMABegIdx + secondEMABegIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int DemaLookback(int optInTimePeriod = 30) =>
        optInTimePeriod switch
        {
            < 2 or > 100000 => -1,
            _ => EmaLookback(optInTimePeriod) * 2
        };
}
