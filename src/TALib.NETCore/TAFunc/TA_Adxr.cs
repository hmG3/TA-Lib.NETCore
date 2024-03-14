namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Adxr(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AdxrLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var adx = new double[endIdx - startIdx + optInTimePeriod];

        Core.RetCode retCode = Adx(inHigh, inLow, inClose, startIdx - (optInTimePeriod - 1), endIdx, adx, out outBegIdx, out outNbElement,
            optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        int i = optInTimePeriod - 1;
        int j = default;
        int outIdx = default;
        int nbElement = endIdx - startIdx + 2;
        while (--nbElement != 0)
        {
            outReal[outIdx++] = (adx[i++] + adx[j++]) / 2.0;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Adxr(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AdxrLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var adx = new decimal[endIdx - startIdx + optInTimePeriod];

        Core.RetCode retCode = Adx(inHigh, inLow, inClose, startIdx - (optInTimePeriod - 1), endIdx, adx, out outBegIdx, out outNbElement,
            optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        int i = optInTimePeriod - 1;
        int j = default;
        int outIdx = default;
        int nbElement = endIdx - startIdx + 2;
        while (--nbElement != 0)
        {
            outReal[outIdx++] = (adx[i++] + adx[j++]) / 2m;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AdxrLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod + AdxLookback(optInTimePeriod) - 1;
    }
}
