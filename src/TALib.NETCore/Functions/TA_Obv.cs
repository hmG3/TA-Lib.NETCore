namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Obv(T[] inReal, T[] inVolume, int startIdx, int endIdx, T[] outReal, out int outBegIdx,
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

        T prevOBV = inVolume[startIdx];
        T prevReal = inReal[startIdx];
        int outIdx = default;

        for (var i = startIdx; i <= endIdx; i++)
        {
            T tempReal = inReal[i];
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
