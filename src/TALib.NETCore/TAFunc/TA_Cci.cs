namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Cci(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = CciLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var circBuffer = new T[optInTimePeriod];
        int circBufferIdx = default;
        var maxIdxCircBuffer = optInTimePeriod - 1;
        int i = startIdx - lookbackTotal;
        while (i < startIdx)
        {
            circBuffer[circBufferIdx++] = (inHigh[i] + inLow[i] + inClose[i]) / TThree;
            i++;
            if (circBufferIdx > maxIdxCircBuffer)
            {
                circBufferIdx = 0;
            }
        }

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);
        T tPointZeroOneFive = T.CreateChecked(0.015);

        int outIdx = default;
        do
        {
            T lastValue = (inHigh[i] + inLow[i] + inClose[i]) / TThree;
            circBuffer[circBufferIdx++] = lastValue;

            T theAverage = T.Zero;
            for (var j = 0; j < optInTimePeriod; j++)
            {
                theAverage += circBuffer[j];
            }
            theAverage /= tOptInTimePeriod;

            T tempReal2 = T.Zero;
            for (var j = 0; j < optInTimePeriod; j++)
            {
                tempReal2 += T.Abs(circBuffer[j] - theAverage);
            }

            T tempReal = lastValue - theAverage;
            outReal[outIdx++] = !T.IsZero(tempReal) && !T.IsZero(tempReal2)
                ? tempReal / (tPointZeroOneFive * (tempReal2 / tOptInTimePeriod))
                : T.Zero;

            if (circBufferIdx > maxIdxCircBuffer)
            {
                circBufferIdx = 0;
            }

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CciLookback(int optInTimePeriod = 14) =>
        optInTimePeriod switch
        {
            < 2 or > 100000 => -1,
            _ => optInTimePeriod - 1
        };
}
