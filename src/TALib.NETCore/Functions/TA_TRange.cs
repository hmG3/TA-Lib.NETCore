namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode TRange(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
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

        int lookbackTotal = TRangeLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        int today = startIdx;
        while (today <= endIdx)
        {
            T tempLT = inLow[today];
            T tempHT = inHigh[today];
            T tempCY = inClose[today - 1];
            T greatest = tempHT - tempLT;

            T val2 = T.Abs(tempCY - tempHT);
            if (val2 > greatest)
            {
                greatest = val2;
            }

            T val3 = T.Abs(tempCY - tempLT);
            if (val3 > greatest)
            {
                greatest = val3;
            }

            outReal[outIdx++] = greatest;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TRangeLookback() => 1;
}
