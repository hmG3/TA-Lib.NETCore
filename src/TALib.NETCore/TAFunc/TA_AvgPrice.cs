namespace TALib;

public static partial class Core
{
    public static RetCode AvgPrice(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
        double[] outReal, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outReal == null)
        {
            return RetCode.BadParam;
        }

        int outIdx = default;
        for (var i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = (inHigh[i] + inLow[i] + inClose[i] + inOpen[i]) / 4.0;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return RetCode.Success;
    }

    public static RetCode AvgPrice(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
        decimal[] outReal, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outReal == null)
        {
            return RetCode.BadParam;
        }

        int outIdx = default;
        for (var i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = (inHigh[i] + inLow[i] + inClose[i] + inOpen[i]) / 4m;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return RetCode.Success;
    }

    public static int AvgPriceLookback() => 0;
}
