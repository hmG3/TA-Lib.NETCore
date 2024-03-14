namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Roc(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 10)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = RocLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        int inIdx = startIdx;
        int trailingIdx = startIdx - lookbackTotal;
        while (inIdx <= endIdx)
        {
            double tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = !tempReal.Equals(0.0) ? (inReal[inIdx] / tempReal - 1.0) * 100.0 : 0.0;
            inIdx++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Roc(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 10)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = RocLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        int inIdx = startIdx;
        int trailingIdx = startIdx - lookbackTotal;
        while (inIdx <= endIdx)
        {
            decimal tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = tempReal != Decimal.Zero ? (inReal[inIdx] / tempReal - Decimal.One) * 100m : Decimal.Zero;
            inIdx++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int RocLookback(int optInTimePeriod = 10)
    {
        if (optInTimePeriod is < 1 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod;
    }
}
