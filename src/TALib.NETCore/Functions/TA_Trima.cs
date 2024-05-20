namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Trima(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
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

        int lookbackTotal = TrimaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int middleIdx;
        int trailingIdx;
        int todayIdx;
        int outIdx;
        if (optInTimePeriod % 2 == 1)
        {
            int i = optInTimePeriod >> 1;
            var ti = T.CreateChecked(i);
            T factor = (ti + T.One) * (ti + T.One);
            factor = T.One / factor;

            trailingIdx = startIdx - lookbackTotal;
            middleIdx = trailingIdx + i;
            todayIdx = middleIdx + i;
            T numerator = T.Zero;
            T numeratorSub = T.Zero;
            T tempReal;
            for (i = middleIdx; i >= trailingIdx; i--)
            {
                tempReal = inReal[i];
                numeratorSub += tempReal;
                numerator += numeratorSub;
            }

            T numeratorAdd = T.Zero;
            middleIdx++;
            for (i = middleIdx; i <= todayIdx; i++)
            {
                tempReal = inReal[i];
                numeratorAdd += tempReal;
                numerator += numeratorAdd;
            }

            outIdx = 0;
            tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = numerator * factor;
            todayIdx++;
            while (todayIdx <= endIdx)
            {
                numerator -= numeratorSub;
                numeratorSub -= tempReal;
                tempReal = inReal[middleIdx++];
                numeratorSub += tempReal;

                numerator += numeratorAdd;
                numeratorAdd -= tempReal;
                tempReal = inReal[todayIdx++];
                numeratorAdd += tempReal;

                numerator += tempReal;

                tempReal = inReal[trailingIdx++];
                outReal[outIdx++] = numerator * factor;
            }
        }
        else
        {
            int i = optInTimePeriod >> 1;
            var ti = T.CreateChecked(i);
            T factor = ti * (ti + T.One);
            factor = T.One / factor;

            trailingIdx = startIdx - lookbackTotal;
            middleIdx = trailingIdx + i - 1;
            todayIdx = middleIdx + i;
            T numerator = T.Zero;

            T numeratorSub = T.Zero;
            T tempReal;
            for (i = middleIdx; i >= trailingIdx; i--)
            {
                tempReal = inReal[i];
                numeratorSub += tempReal;
                numerator += numeratorSub;
            }
            T numeratorAdd = T.Zero;
            middleIdx++;
            for (i = middleIdx; i <= todayIdx; i++)
            {
                tempReal = inReal[i];
                numeratorAdd += tempReal;
                numerator += numeratorAdd;
            }

            outIdx = 0;
            tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = numerator * factor;
            todayIdx++;

            while (todayIdx <= endIdx)
            {
                numerator -= numeratorSub;
                numeratorSub -= tempReal;
                tempReal = inReal[middleIdx++];
                numeratorSub += tempReal;

                numeratorAdd -= tempReal;
                numerator += numeratorAdd;
                tempReal = inReal[todayIdx++];
                numeratorAdd += tempReal;

                numerator += tempReal;

                tempReal = inReal[trailingIdx++];
                outReal[outIdx++] = numerator * factor;
            }
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TrimaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;
}
