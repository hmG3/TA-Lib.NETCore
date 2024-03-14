namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Obv(double[] inReal, double[] inVolume, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
        out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || inVolume == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        double prevOBV = inVolume[startIdx];
        double prevReal = inReal[startIdx];
        int outIdx = default;

        for (int i = startIdx; i <= endIdx; i++)
        {
            double tempReal = inReal[i];
            if (tempReal > prevReal)
            {
                prevOBV += inVolume[i];
            }
            else if (tempReal < prevReal)
            {
                prevOBV -= inVolume[i];
            }

            outReal[outIdx++] = prevOBV;
            prevReal = tempReal;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Obv(decimal[] inReal, decimal[] inVolume, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
        out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || inVolume == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        decimal prevOBV = inVolume[startIdx];
        decimal prevReal = inReal[startIdx];
        int outIdx = default;

        for (int i = startIdx; i <= endIdx; i++)
        {
            decimal tempReal = inReal[i];
            if (tempReal > prevReal)
            {
                prevOBV += inVolume[i];
            }
            else if (tempReal < prevReal)
            {
                prevOBV -= inVolume[i];
            }

            outReal[outIdx++] = prevOBV;
            prevReal = tempReal;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int ObvLookback() => 0;
}
