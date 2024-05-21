namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode GapSideBySideWhiteLines(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx,
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

        int lookbackTotal = GapSideBySideWhiteLinesLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T nearPeriodTotal = T.Zero;
        T equalPeriodTotal = T.Zero;
        int nearTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Near);
        int equalTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Equal);
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (( // upside or downside gap between the 1st candle and both the next 2 candles
                    TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && TA_RealBodyGapUp(inOpen, inClose, i, i - 2)
                    ||
                    TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && TA_RealBodyGapDown(inOpen, inClose, i, i - 2)
                ) &&
                TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White && // 2nd: white
                TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.White && // 3rd: white
                TA_RealBody(inClose, inOpen, i) >= TA_RealBody(inClose, inOpen, i - 1) -
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) && // same size 2 and 3
                TA_RealBody(inClose, inOpen, i) <= TA_RealBody(inClose, inOpen, i - 1) +
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) &&
                inOpen[i] >= inOpen[i - 1] -
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 1) && // same open 2 and 3
                inOpen[i] <= inOpen[i - 1] +
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1))
            {
                outInteger[outIdx++] = TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) ? 100 : -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                               TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);
            equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            i++;
            nearTrailingIdx++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int GapSideBySideWhiteLinesLookback() =>
        Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.Near), TA_CandleAveragePeriod(Core.CandleSettingType.Equal)) + 2;
}
