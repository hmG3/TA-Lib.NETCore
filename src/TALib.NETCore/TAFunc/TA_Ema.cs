namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Ema(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        return TA_INT_EMA(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod,
            TTwo / (T.CreateChecked(optInTimePeriod) + T.One));
    }

    public static int EmaLookback(int optInTimePeriod = 30) =>
        optInTimePeriod switch
        {
            < 2 or > 100000 => -1,
            _ => optInTimePeriod - 1 + Core.UnstablePeriodSettings.Get(Core.FuncUnstId.Ema)
        };
}
