namespace TALib;

public static partial class Core
{
    public static RetCode MacdFix(double[] inReal, int startIdx, int endIdx, double[] outMACD, double[] outMACDSignal,
        double[] outMACDHist, out int outBegIdx, out int outNbElement, int optInSignalPeriod = 9)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInSignalPeriod < 1 ||
            optInSignalPeriod > 100000)
        {
            return RetCode.BadParam;
        }

        return TA_INT_MACD(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, 0, 0,
            optInSignalPeriod);
    }

    public static RetCode MacdFix(decimal[] inReal, int startIdx, int endIdx, decimal[] outMACD, decimal[] outMACDSignal,
        decimal[] outMACDHist, out int outBegIdx, out int outNbElement, int optInSignalPeriod = 9)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInSignalPeriod < 1 ||
            optInSignalPeriod > 100000)
        {
            return RetCode.BadParam;
        }

        return TA_INT_MACD(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, 0, 0,
            optInSignalPeriod);
    }

    public static int MacdFixLookback(int optInSignalPeriod = 9) => EmaLookback(26) + EmaLookback(optInSignalPeriod);
}
