namespace TALib;

public static partial class Core
{
    public static RetCode Ema(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
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

        return TA_INT_EMA(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod,
            2.0 / (optInTimePeriod + 1));
    }

    public static RetCode Ema(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
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

        return TA_INT_EMA(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod,
            2m / (optInTimePeriod + 1));
    }

    public static int EmaLookback(int optInTimePeriod = 30)
    {
        if (optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return -1;
        }

        return optInTimePeriod - 1 + (int) Globals.UnstablePeriod[(int) FuncUnstId.Ema];
    }
}
