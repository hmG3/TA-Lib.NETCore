namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Var(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 5)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod is < 1 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        return TA_INT_VAR(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
    }

    public static int VarLookback(int optInTimePeriod = 5) => optInTimePeriod is < 1 or > 100000 ? -1 : optInTimePeriod - 1;
}
