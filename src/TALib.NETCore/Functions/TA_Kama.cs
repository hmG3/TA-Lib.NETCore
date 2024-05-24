namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Kama(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = KamaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T constMax = TTwo / (T.CreateChecked(30) + T.One);
        T constDiff = TTwo / (TTwo + T.One) - constMax;

        T sumROC1 = T.Zero;
        T tempReal;
        int today = startIdx - lookbackTotal;
        int trailingIdx = today;
        int i = optInTimePeriod;
        while (i-- > 0)
        {
            tempReal = inReal[today++];
            tempReal -= inReal[today];
            sumROC1 += T.Abs(tempReal);
        }

        T prevKAMA = inReal[today - 1];

        tempReal = inReal[today];
        T tempReal2 = inReal[trailingIdx++];
        T periodROC = tempReal - tempReal2;

        T trailingValue = tempReal2;
        if (sumROC1 <= periodROC || T.IsZero(sumROC1))
        {
            tempReal = T.One;
        }
        else
        {
            tempReal = T.Abs(periodROC / sumROC1);
        }

        tempReal = tempReal * constDiff + constMax;
        tempReal *= tempReal;

        prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
        while (today <= startIdx)
        {
            tempReal = inReal[today];
            tempReal2 = inReal[trailingIdx++];
            periodROC = tempReal - tempReal2;

            sumROC1 -= T.Abs(trailingValue - tempReal2);
            sumROC1 += T.Abs(tempReal - inReal[today - 1]);

            trailingValue = tempReal2;
            if (sumROC1 <= periodROC || T.IsZero(sumROC1))
            {
                tempReal = T.One;
            }
            else
            {
                tempReal = T.Abs(periodROC / sumROC1);
            }

            tempReal = tempReal * constDiff + constMax;
            tempReal *= tempReal;

            prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
        }

        outReal[0] = prevKAMA;
        int outIdx = 1;
        outBegIdx = today - 1;
        while (today <= endIdx)
        {
            tempReal = inReal[today];
            tempReal2 = inReal[trailingIdx++];
            periodROC = tempReal - tempReal2;

            sumROC1 -= T.Abs(trailingValue - tempReal2);
            sumROC1 += T.Abs(tempReal - inReal[today - 1]);

            trailingValue = tempReal2;
            if (sumROC1 <= periodROC || T.IsZero(sumROC1))
            {
                tempReal = T.One;
            }
            else
            {
                tempReal = T.Abs(periodROC / sumROC1);
            }

            tempReal = tempReal * constDiff + constMax;
            tempReal *= tempReal;

            prevKAMA = (inReal[today++] - prevKAMA) * tempReal + prevKAMA;
            outReal[outIdx++] = prevKAMA;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int KamaLookback(int optInTimePeriod = 30) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Kama);
}
