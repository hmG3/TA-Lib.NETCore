namespace TALib;

public static partial class Core
{
    public static RetCode Sma(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return RetCode.BadParam;
        }

        return TA_INT_SMA(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
    }

    public static RetCode Sma(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return RetCode.BadParam;
        }

        return TA_INT_SMA(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
    }

    public static int SmaLookback(int optInTimePeriod = 30)
    {
        if (optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return -1;
        }

        return optInTimePeriod - 1;
    }
}
