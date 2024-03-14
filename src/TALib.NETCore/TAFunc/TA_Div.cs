namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Div(double[] inReal0, double[] inReal1, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
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
        for (int i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = inReal0[i] / inReal1[i];
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Div(decimal[] inReal0, decimal[] inReal1, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
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
        for (int i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx++] = inReal0[i] / inReal1[i];
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int DivLookback() => 0;
}
