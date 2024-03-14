namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Tema(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
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
        double k = 2.0 / (optInTimePeriod + 1);

        var firstEMA = new double[tempInt];
        Core.RetCode retCode = TA_INT_EMA(inReal, startIdx - lookbackEMA * 2, endIdx, firstEMA, out var firstEMABegIdx,
            out var firstEMANbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || firstEMANbElement == 0)
        {
            return retCode;
        }

        var secondEMA = new double[firstEMANbElement];
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
            outReal[outIdx++] += 3.0 * firstEMA[firstEMAIdx++] - 3.0 * secondEMA[secondEMAIdx++];
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Tema(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
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
        decimal k = 2m / (optInTimePeriod + 1);

        var firstEMA = new decimal[tempInt];
        Core.RetCode retCode = TA_INT_EMA(inReal, startIdx - lookbackEMA * 2, endIdx, firstEMA, out var firstEMABegIdx,
            out var firstEMANbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || firstEMANbElement == 0)
        {
            return retCode;
        }

        var secondEMA = new decimal[firstEMANbElement];
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
            outReal[outIdx++] += 3m * firstEMA[firstEMAIdx++] - 3m * secondEMA[secondEMAIdx++];
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TemaLookback(int optInTimePeriod = 30)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        return EmaLookback(optInTimePeriod) * 3;
    }
}
