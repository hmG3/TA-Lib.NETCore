namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Ma(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 30, Core.MAType optInMAType = Core.MAType.Sma)
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

        if (optInTimePeriod == 1)
        {
            var nbElement = endIdx - startIdx + 1;
            outNbElement = nbElement;
            for (int todayIdx = startIdx, outIdx = 0; outIdx < nbElement; outIdx++, todayIdx++)
            {
                outReal[outIdx] = inReal[todayIdx];
            }

            outBegIdx = startIdx;

            return Core.RetCode.Success;
        }

        switch (optInMAType)
        {
            case Core.MAType.Sma:
                return Sma(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Ema:
                return Ema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Wma:
                return Wma(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Dema:
                return Dema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Tema:
                return Tema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Trima:
                return Trima(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Kama:
                return Kama(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Mama:
                var dummyBuffer = new T[endIdx - startIdx + 1];
                return Mama(inReal, startIdx, endIdx, outReal, dummyBuffer, out outBegIdx, out outNbElement);
            case Core.MAType.T3:
                return T3(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            default:
                return Core.RetCode.BadParam;
        }
    }

    public static int MaLookback(int optInTimePeriod = 30, Core.MAType optInMAType = Core.MAType.Sma)
    {
        return optInTimePeriod switch
        {
            < 1 or > 100000 => -1,
            1 => 0,
            _ => optInMAType switch
            {
                Core.MAType.Sma => SmaLookback(optInTimePeriod),
                Core.MAType.Ema => EmaLookback(optInTimePeriod),
                Core.MAType.Wma => WmaLookback(optInTimePeriod),
                Core.MAType.Dema => DemaLookback(optInTimePeriod),
                Core.MAType.Tema => TemaLookback(optInTimePeriod),
                Core.MAType.Trima => TrimaLookback(optInTimePeriod),
                Core.MAType.Kama => KamaLookback(optInTimePeriod),
                Core.MAType.Mama => MamaLookback(),
                Core.MAType.T3 => T3Lookback(optInTimePeriod),
                _ => 0
            }
        };
    }
}
