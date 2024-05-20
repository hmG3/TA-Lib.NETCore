namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Sub(T[] inReal0, T[] inReal1, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
        out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal0 == null || inReal1 == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int outIdx = default;
        for (var i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = inReal0[i] - inReal1[i];
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int SubLookback() => 0;
}
