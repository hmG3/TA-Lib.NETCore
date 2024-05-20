namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode AdOsc(T[] inHigh, T[] inLow, T[] inClose, T[] inVolume, int startIdx, int endIdx,
        T[] outReal, out int outBegIdx, out int outNbElement, int optInFastPeriod = 3, int optInSlowPeriod = 10)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || inVolume == null || outReal == null ||
            optInFastPeriod < 2 || optInSlowPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AdOscLookback(optInFastPeriod, optInSlowPeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        int today = startIdx - lookbackTotal;

        T ad = T.Zero;

        T fastk = TTwo / (T.CreateChecked(optInFastPeriod) + T.One);
        T oneMinusFastk = T.One - fastk;

        T slowk = TTwo / (T.CreateChecked(optInSlowPeriod) + T.One);
        T oneMinusSlowk = T.One - slowk;

        CalculateAd();
        T fastEMA = ad;
        T slowEMA = ad;

        while (today < startIdx)
        {
            CalculateAd();
            fastEMA = fastk * ad + oneMinusFastk * fastEMA;
            slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
        }

        int outIdx = default;
        while (today <= endIdx)
        {
            CalculateAd();
            fastEMA = fastk * ad + oneMinusFastk * fastEMA;
            slowEMA = slowk * ad + oneMinusSlowk * slowEMA;

            outReal[outIdx++] = fastEMA - slowEMA;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;

        void CalculateAd()
        {
            T h = inHigh[today];
            T l = inLow[today];
            T tmp = h - l;
            T c = inClose[today];
            if (tmp > T.Zero)
            {
                ad += (c - l - (h - c)) / tmp * inVolume[today];
            }

            today++;
        }
    }

    public static int AdOscLookback(int optInFastPeriod = 3, int optInSlowPeriod = 10) =>
        optInFastPeriod < 2 || optInSlowPeriod < 2
            ? -1
            : EmaLookback(optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod);
}
