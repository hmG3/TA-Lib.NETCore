namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Mfi(T[] inHigh, T[] inLow, T[] inClose, T[] inVolume, int startIdx, int endIdx,
        T[] outReal, out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || inVolume == null || outReal == null || optInTimePeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = MfiLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;

        var moneyFlow = new (T negative, T positive)[optInTimePeriod];

        int mflowIdx = default;
        var maxIdxMflow = optInTimePeriod - 1;

        int today = startIdx - lookbackTotal;
        T prevValue = (inHigh[today] + inLow[today] + inClose[today]) / TThree;

        T posSumMF = T.Zero;
        T negSumMF = T.Zero;
        today++;
        for (var i = optInTimePeriod; i > 0; i--)
        {
            T tempValue1 = (inHigh[today] + inLow[today] + inClose[today]) / TThree;
            T tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;
            tempValue1 *= inVolume[today++];
            if (tempValue2 < T.Zero)
            {
                moneyFlow[mflowIdx].negative = tempValue1;
                negSumMF += tempValue1;
                moneyFlow[mflowIdx].positive =T.Zero;
            }
            else if (tempValue2 > T.Zero)
            {
                moneyFlow[mflowIdx].positive = tempValue1;
                posSumMF += tempValue1;
                moneyFlow[mflowIdx].negative = T.Zero;
            }
            else
            {
                moneyFlow[mflowIdx].positive = T.Zero;
                moneyFlow[mflowIdx].negative = T.Zero;
            }

            if (++mflowIdx > maxIdxMflow)
            {
                mflowIdx = 0;
            }
        }

        if (today > startIdx)
        {
            T tempValue1 = posSumMF + negSumMF;
            outReal[outIdx++] = tempValue1 >= T.One ? THundred * (posSumMF / tempValue1) : T.Zero;
        }
        else
        {
            while (today < startIdx)
            {
                posSumMF -= moneyFlow[mflowIdx].positive;
                negSumMF -= moneyFlow[mflowIdx].negative;

                T tempValue1 = (inHigh[today] + inLow[today] + inClose[today]) / TThree;
                T tempValue2 = tempValue1 - prevValue;
                prevValue = tempValue1;
                tempValue1 *= inVolume[today++];
                if (tempValue2 < T.Zero)
                {
                    moneyFlow[mflowIdx].negative = tempValue1;
                    negSumMF += tempValue1;
                    moneyFlow[mflowIdx].positive = T.Zero;
                }
                else if (tempValue2 > T.Zero)
                {
                    moneyFlow[mflowIdx].positive = tempValue1;
                    posSumMF += tempValue1;
                    moneyFlow[mflowIdx].negative = T.Zero;
                }
                else
                {
                    moneyFlow[mflowIdx].positive = T.Zero;
                    moneyFlow[mflowIdx].negative = T.Zero;
                }

                if (++mflowIdx > maxIdxMflow)
                {
                    mflowIdx = 0;
                }
            }
        }

        while (today <= endIdx)
        {
            posSumMF -= moneyFlow[mflowIdx].positive;
            negSumMF -= moneyFlow[mflowIdx].negative;

            T tempValue1 = (inHigh[today] + inLow[today] + inClose[today]) / TThree;
            T tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;
            tempValue1 *= inVolume[today++];
            if (tempValue2 < T.Zero)
            {
                moneyFlow[mflowIdx].negative = tempValue1;
                negSumMF += tempValue1;
                moneyFlow[mflowIdx].positive = T.Zero;
            }
            else if (tempValue2 > T.Zero)
            {
                moneyFlow[mflowIdx].positive = tempValue1;
                posSumMF += tempValue1;
                moneyFlow[mflowIdx].negative = T.Zero;
            }
            else
            {
                moneyFlow[mflowIdx].positive = T.Zero;
                moneyFlow[mflowIdx].negative = T.Zero;
            }

            tempValue1 = posSumMF + negSumMF;
            outReal[outIdx++] = tempValue1 >= T.One ? THundred * (posSumMF / tempValue1) : T.Zero;

            if (++mflowIdx > maxIdxMflow)
            {
                mflowIdx = 0;
            }
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MfiLookback(int optInTimePeriod = 14) =>
        optInTimePeriod is < 2 or > 100000 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Mfi);
}
