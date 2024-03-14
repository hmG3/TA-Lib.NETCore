namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Sum(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
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

        int lookbackTotal = SumLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double periodTotal = default;
        int trailingIdx = startIdx - lookbackTotal;
        int i = trailingIdx;
        while (i < startIdx)
        {
            periodTotal += inReal[i++];
        }

        int outIdx = default;
        do
        {
            periodTotal += inReal[i++];
            double tempReal = periodTotal;
            periodTotal -= inReal[trailingIdx++];
            outReal[outIdx++] = tempReal;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Sum(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
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

        int lookbackTotal = SumLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal periodTotal = default;
        int trailingIdx = startIdx - lookbackTotal;
        int i = trailingIdx;
        while (i < startIdx)
        {
            periodTotal += inReal[i++];
        }

        int outIdx = default;
        do
        {
            periodTotal += inReal[i++];
            decimal tempReal = periodTotal;
            periodTotal -= inReal[trailingIdx++];
            outReal[outIdx++] = tempReal;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int SumLookback(int optInTimePeriod = 30)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod - 1;
    }
}
