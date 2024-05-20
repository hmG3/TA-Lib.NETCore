namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Ln(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int outIdx = default;
        for (var i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = T.Log(inReal[i]);
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int LnLookback() => 0;
}
