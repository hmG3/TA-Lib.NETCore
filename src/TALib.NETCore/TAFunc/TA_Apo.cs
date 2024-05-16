namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Apo(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInFastPeriod = 12, int optInSlowPeriod = 26, Core.MAType optInMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInFastPeriod is < 2 or > 100000 || optInSlowPeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        var tempBuffer = new T[endIdx - startIdx + 1];

        return TA_INT_PO(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInFastPeriod, optInSlowPeriod,
            optInMAType, tempBuffer, false);
    }

    public static int ApoLookback(int optInFastPeriod = 12, int optInSlowPeriod = 26, Core.MAType optInMAType = Core.MAType.Sma)
    {
        if (optInFastPeriod is < 2 or > 100000 || optInSlowPeriod is < 2 or > 100000)
        {
            return -1;
        }

        return MaLookback(Math.Max(optInSlowPeriod, optInFastPeriod), optInMAType);
    }
}
