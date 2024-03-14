namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Var(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 5)
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

        return TA_INT_VAR(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
    }

    public static Core.RetCode Var(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 5)
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

        return TA_INT_VAR(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
    }

    public static int VarLookback(int optInTimePeriod = 5)
    {
        if (optInTimePeriod is < 1 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod - 1;
    }
}
