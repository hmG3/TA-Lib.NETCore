namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode T3(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 5, double optInVFactor = 0.7)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null ||
            optInTimePeriod < 2 || optInVFactor < 0.0 || optInVFactor > 1.0)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = T3Lookback(optInTimePeriod);
        if (startIdx <= lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        int today = startIdx - lookbackTotal;

        T tOptInTimePeriod = T.CreateChecked(optInTimePeriod);

        T k = TTwo / (tOptInTimePeriod + T.One);
        T oneMinusK = T.One - k;

        T tempReal = inReal[today++];
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            tempReal += inReal[today++];
        }
        T e1 = tempReal / tOptInTimePeriod;

        tempReal = e1;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            tempReal += e1;
        }
        T e2 = tempReal / tOptInTimePeriod;

        tempReal = e2;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            tempReal += e2;
        }
        T e3 = tempReal / tOptInTimePeriod;

        tempReal = e3;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            tempReal += e3;
        }
        T e4 = tempReal / tOptInTimePeriod;

        tempReal = e4;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            tempReal += e4;
        }
        T e5 = tempReal / tOptInTimePeriod;

        tempReal = e5;
        for (var i = optInTimePeriod - 1; i > 0; i--)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            tempReal += e5;
        }
        T e6 = tempReal / tOptInTimePeriod;

        while (today <= startIdx)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            e6 = k * e5 + oneMinusK * e6;
        }

        T tOptInVFactorT = T.CreateChecked(optInVFactor);
        tempReal = tOptInVFactorT * tOptInVFactorT;
        T c1 = T.NegativeOne * tempReal * tOptInVFactorT;
        T c2 = TThree * (tempReal - c1);
        T c3 = T.NegativeOne * TTwo * TThree * tempReal - TThree * (tOptInVFactorT - c1);
        T c4 = T.One + TThree * tOptInVFactorT - c1 + TThree * tempReal;

        int outIdx = default;
        outReal[outIdx++] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;

        while (today <= endIdx)
        {
            e1 = k * inReal[today++] + oneMinusK * e1;
            e2 = k * e1 + oneMinusK * e2;
            e3 = k * e2 + oneMinusK * e3;
            e4 = k * e3 + oneMinusK * e4;
            e5 = k * e4 + oneMinusK * e5;
            e6 = k * e5 + oneMinusK * e6;
            outReal[outIdx++] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int T3Lookback(int optInTimePeriod = 5) =>
        optInTimePeriod < 2 ? -1 : (optInTimePeriod - 1) * 6 + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.T3);
}
