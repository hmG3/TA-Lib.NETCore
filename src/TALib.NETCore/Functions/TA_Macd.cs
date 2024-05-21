namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Macd(T[] inReal, int startIdx, int endIdx, T[] outMACD, T[] outMACDSignal,
        T[] outMACDHist, out int outBegIdx, out int outNbElement, int optInFastPeriod = 12, int optInSlowPeriod = 26,
        int optInSignalPeriod = 9)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null ||
            optInFastPeriod < 2 || optInSlowPeriod < 2 || optInSignalPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        return CalcMACD(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement,
            optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
    }

    public static int MacdLookback(int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
    {
        if (optInFastPeriod < 2 || optInSlowPeriod < 2 || optInSignalPeriod < 1)
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
