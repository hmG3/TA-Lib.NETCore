namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Ppo(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
        int optInFastPeriod = 12, int optInSlowPeriod = 26, Core.MAType optInMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 ||
            optInSlowPeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        var tempBuffer = new double[endIdx - startIdx + 1];

        return Core.TA_INT_PO(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInFastPeriod, optInSlowPeriod,
            optInMAType, tempBuffer, true);
    }

    public static Core.RetCode Ppo(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
        int optInFastPeriod = 12, int optInSlowPeriod = 26, Core.MAType optInMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 ||
            optInSlowPeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        var tempBuffer = new decimal[endIdx - startIdx + 1];

        return Core.TA_INT_PO(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInFastPeriod, optInSlowPeriod,
            optInMAType, tempBuffer, true);
    }

    public static int PpoLookback(int optInFastPeriod = 12, int optInSlowPeriod = 26, Core.MAType optInMAType = Core.MAType.Sma)
    {
        if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
        {
            return -1;
        }

        return MaLookback(Math.Max(optInSlowPeriod, optInFastPeriod), optInMAType);
    }
}
