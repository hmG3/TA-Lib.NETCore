namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode UpDownSideGapThreeMethods(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx,
        int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement)
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

        int lookbackTotal = UpDownSideGapThreeMethodsLookback();
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
            if (TA_CandleColor(inClose, inOpen, i - 2) == TA_CandleColor(inClose, inOpen, i - 1) && // 1st and 2nd of same color
                (int) TA_CandleColor(inClose, inOpen, i - 1) == -(int) TA_CandleColor(inClose, inOpen, i) && // 3rd opposite color
                inOpen[i] < T.Max(inClose[i - 1], inOpen[i - 1]) && // 3rd opens within 2nd rb
                inOpen[i] > T.Min(inClose[i - 1], inOpen[i - 1]) &&
                inClose[i] < T.Max(inClose[i - 2], inOpen[i - 2]) && // 3rd closes within 1st rb
                inClose[i] > T.Min(inClose[i - 2], inOpen[i - 2]) &&
                (TA_CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White && // when 1st is white
                 TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) // upside gap
                 ||
                 TA_CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black && // when 1st is black
                 TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2))) // downside gap
            {
                outInteger[outIdx++] = (int) TA_CandleColor(inClose, inOpen, i - 2) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int UpDownSideGapThreeMethodsLookback() => 2;
}
