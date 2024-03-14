namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Ad(double[] inHigh, double[] inLow, double[] inClose, double[] inVolume, int startIdx, int endIdx,
        double[] outReal, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || inVolume == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int nbBar = endIdx - startIdx + 1;
        outBegIdx = startIdx;
        outNbElement = nbBar;
        int currentBar = startIdx;
        int outIdx = default;
        double ad = default;
        while (nbBar != 0)
        {
            double high = inHigh[currentBar];
            double low = inLow[currentBar];
            double tmp = high - low;
            double close = inClose[currentBar];

            if (tmp > 0.0)
            {
                ad += (close - low - (high - close)) / tmp * inVolume[currentBar];
            }

            outReal[outIdx++] = ad;

            currentBar++;
            nbBar--;
        }

        return Core.RetCode.Success;
    }

    public static Core.RetCode Ad(decimal[] inHigh, decimal[] inLow, decimal[] inClose, decimal[] inVolume, int startIdx, int endIdx,
        decimal[] outReal, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || inVolume == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int nbBar = endIdx - startIdx + 1;
        outBegIdx = startIdx;
        outNbElement = nbBar;
        int currentBar = startIdx;
        int outIdx = default;
        decimal ad = default;
        while (nbBar != 0)
        {
            decimal high = inHigh[currentBar];
            decimal low = inLow[currentBar];
            decimal tmp = high - low;
            decimal close = inClose[currentBar];

            if (tmp > Decimal.Zero)
            {
                ad += (close - low - (high - close)) / tmp * inVolume[currentBar];
            }

            outReal[outIdx++] = ad;

            currentBar++;
            nbBar--;
        }

        return Core.RetCode.Success;
    }

    public static int AdLookback() => 0;
}
