namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Ad(T[] inHigh, T[] inLow, T[] inClose, T[] inVolume, int startIdx, int endIdx,
        T[] outReal, out int outBegIdx, out int outNbElement)
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
        T ad = T.Zero;
        while (nbBar != 0)
        {
            T high = inHigh[currentBar];
            T low = inLow[currentBar];
            T tmp = high - low;
            T close = inClose[currentBar];

            if (tmp > T.Zero)
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
