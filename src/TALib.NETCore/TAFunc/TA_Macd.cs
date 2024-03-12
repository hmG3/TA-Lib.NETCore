namespace TALib;

public static partial class Core
{
    public static RetCode Macd(double[] inReal, int startIdx, int endIdx, double[] outMACD, double[] outMACDSignal,
        double[] outMACDHist, out int outBegIdx, out int outNbElement, int optInFastPeriod = 12, int optInSlowPeriod = 26,
        int optInSignalPeriod = 9)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInFastPeriod < 2 ||
            optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 || optInSignalPeriod < 1 ||
            optInSignalPeriod > 100000)
        {
            return RetCode.BadParam;
        }

        return TA_INT_MACD(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement,
            optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
    }

    public static RetCode Macd(decimal[] inReal, int startIdx, int endIdx, decimal[] outMACD, decimal[] outMACDSignal,
        decimal[] outMACDHist, out int outBegIdx, out int outNbElement, int optInFastPeriod = 12, int optInSlowPeriod = 26,
        int optInSignalPeriod = 9)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInFastPeriod < 2 ||
            optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 || optInSignalPeriod < 1 ||
            optInSignalPeriod > 100000)
        {
            return RetCode.BadParam;
        }

        return TA_INT_MACD(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement,
            optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
    }

    public static int MacdLookback(int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
    {
        if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 ||
            optInSignalPeriod < 1 || optInSignalPeriod > 100000)
        {
            return -1;
        }

        if (optInSlowPeriod < optInFastPeriod)
        {
            optInSlowPeriod = optInFastPeriod;
        }

        return EmaLookback(optInSlowPeriod) + EmaLookback(optInSignalPeriod);
    }
}
