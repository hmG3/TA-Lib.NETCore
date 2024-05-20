namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode WclPrice(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
        out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int outIdx = default;
        for (var i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = (inHigh[i] + inLow[i] + inClose[i] * TTwo) / TFour;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int WclPriceLookback() => 0;
}
