namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode ThreeOutside(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = ThreeOutsideLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int i = startIdx;
        int outIdx = default;
        do
        {
            if (TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
                TA_CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black &&
                inClose[i - 1] > inOpen[i - 2] && inOpen[i - 1] < inClose[i - 2] &&
                inClose[i] > inClose[i - 1]
                ||
                TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
                TA_CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
                inOpen[i - 1] > inClose[i - 2] && inClose[i - 1] < inOpen[i - 2] &&
                inClose[i] < inClose[i - 1])
            {
                outInteger[outIdx++] = (int) TA_CandleColor(inClose, inOpen, i - 1) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int ThreeOutsideLookback() => 3;
}
