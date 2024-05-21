namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Tristar(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = TristarLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyPeriodTotal = T.Zero;
        int bodyTrailingIdx = startIdx - 2 - TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        int i = bodyTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i - 2) <=
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyPeriodTotal, i - 2) && // 1st: doji
                TA_RealBody(inClose, inOpen, i - 1) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyPeriodTotal, i - 2) && // 2nd: doji
                TA_RealBody(inClose, inOpen, i) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyPeriodTotal, i - 2))
            {
                // 3rd: doji
                outInteger[outIdx] = 0;
                if (TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) // 2nd gaps up
                    &&
                    T.Max(inOpen[i], inClose[i]) < T.Max(inOpen[i - 1], inClose[i - 1]) // 3rd is not higher than 2nd
                   )
                {
                    outInteger[outIdx] = -100;
                }

                if (TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) // 2nd gaps down
                    &&
                    T.Min(inOpen[i], inClose[i]) > T.Min(inOpen[i - 1], inClose[i - 1]) // 3rd is not lower than 2nd
                   )
                {
                    outInteger[outIdx] = 100;
                }

                outIdx++;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i - 2) -
                               TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyTrailingIdx);
            i++;
            bodyTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TristarLookback() => TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji) + 2;
}
