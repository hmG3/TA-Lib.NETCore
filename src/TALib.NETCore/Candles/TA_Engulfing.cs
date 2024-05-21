namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Engulfing(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = EngulfingLookback();
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
            if (TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
                TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&            // white engulfs black
                (inClose[i] >= inOpen[i - 1] && inOpen[i] < inClose[i - 1] ||
                 inClose[i] > inOpen[i - 1] && inOpen[i] <= inClose[i - 1]
                )
                ||
                TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.Black &&
                TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&            // black engulfs white
                (inOpen[i] >= inClose[i - 1] && inClose[i] < inOpen[i - 1] ||
                 inOpen[i] > inClose[i - 1] && inClose[i] <= inOpen[i - 1]
                )
               )
            {
                if (!inOpen[i].Equals(inClose[i - 1]) && !inClose[i].Equals(inOpen[i - 1]))
                {
                    outInteger[outIdx++] = (int) TA_CandleColor(inClose, inOpen, i) * 100;
                }
                else
                {
                    outInteger[outIdx++] = (int) TA_CandleColor(inClose, inOpen, i) * 80;
                }
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

    public static int EngulfingLookback() => 2;
}
