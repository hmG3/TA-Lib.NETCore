namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode MacdFix(T[] inReal, int startIdx, int endIdx, T[] outMACD, T[] outMACDSignal,
        T[] outMACDHist, out int outBegIdx, out int outNbElement, int optInSignalPeriod = 9)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInSignalPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        return TA_INT_MACD(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, 0, 0,
            optInSignalPeriod);
    }

    public static int MacdFixLookback(int optInSignalPeriod = 9) => EmaLookback(26) + EmaLookback(optInSignalPeriod);
}
