namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Mavp(T[] inReal, T[] inPeriods, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
        out int outNbElement, int optInMinPeriod = 2, int optInMaxPeriod = 30, Core.MAType optInMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || inPeriods == null || outReal == null || optInMinPeriod < 2 || optInMaxPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MavpLookback(optInMaxPeriod, optInMAType);
        if (inPeriods.Length < lookbackTotal)
        {
            return Core.RetCode.BadParam;
        }

        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInt = lookbackTotal > startIdx ? lookbackTotal : startIdx;
        if (tempInt > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outputSize = endIdx - tempInt + 1;
        var localOutputArray = new T[outputSize];
        int[] localPeriodArray = new int[outputSize];
        for (var i = 0; i < outputSize; i++)
        {
            tempInt = Int32.CreateTruncating(inPeriods[startIdx + i]);
            if (tempInt < optInMinPeriod)
            {
                tempInt = optInMinPeriod;
            }
            else if (tempInt > optInMaxPeriod)
            {
                tempInt = optInMaxPeriod;
            }

            localPeriodArray[i] = tempInt;
        }

        for (var i = 0; i < outputSize; i++)
        {
            int curPeriod = localPeriodArray[i];
            if (curPeriod != 0)
            {
                Core.RetCode retCode = Ma(inReal, startIdx, endIdx, localOutputArray, out _, out _, curPeriod, optInMAType);
                if (retCode != Core.RetCode.Success)
                {
                    return retCode;
                }

                outReal[i] = localOutputArray[i];
                for (var j = i + 1; j < outputSize; j++)
                {
                    if (localPeriodArray[j] == curPeriod)
                    {
                        localPeriodArray[j] = 0;
                        outReal[j] = localOutputArray[j];
                    }
                }
            }
        }

        outBegIdx = startIdx;
        outNbElement = outputSize;

        return Core.RetCode.Success;
    }

    public static int MavpLookback(int optInMaxPeriod = 30, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInMaxPeriod < 2 ? -1 : MaLookback(optInMaxPeriod, optInMAType);
}
