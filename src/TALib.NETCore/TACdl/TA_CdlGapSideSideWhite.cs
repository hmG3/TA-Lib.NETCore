namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlGapSideSideWhite(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx,
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

        int lookbackTotal = CdlGapSideSideWhiteLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double nearPeriodTotal = default;
        double equalPeriodTotal = default;
        int nearTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod;
        int equalTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod;
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (( // upside or downside gap between the 1st candle and both the next 2 candles
                    Core.TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && Core.TA_RealBodyGapUp(inOpen, inClose, i, i - 2)
                    ||
                    Core.TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && Core.TA_RealBodyGapDown(inOpen, inClose, i, i - 2)
                ) &&
                Core.TA_CandleColor(inClose, inOpen, i - 1) && // 2nd: white
                Core.TA_CandleColor(inClose, inOpen, i) && // 3rd: white
                Core.TA_RealBody(inClose, inOpen, i) >= Core.TA_RealBody(inClose, inOpen, i - 1) -
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) && // same size 2 and 3
                Core.TA_RealBody(inClose, inOpen, i) <= Core.TA_RealBody(inClose, inOpen, i - 1) +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) &&
                inOpen[i] >= inOpen[i - 1] -
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 1) && // same open 2 and 3
                inOpen[i] <= inOpen[i - 1] +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1))
            {
                outInteger[outIdx++] = Core.TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) ? 100 : -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                               Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            i++;
            nearTrailingIdx++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlGapSideSideWhite(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = CdlGapSideSideWhiteLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal nearPeriodTotal = default;
        decimal equalPeriodTotal = default;
        int nearTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod;
        int equalTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod;
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (( // upside or downside gap between the 1st candle and both the next 2 candles
                    Core.TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && Core.TA_RealBodyGapUp(inOpen, inClose, i, i - 2)
                    ||
                    Core.TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && Core.TA_RealBodyGapDown(inOpen, inClose, i, i - 2)
                ) &&
                Core.TA_CandleColor(inClose, inOpen, i - 1) && // 2nd: white
                Core.TA_CandleColor(inClose, inOpen, i) && // 3rd: white
                Core.TA_RealBody(inClose, inOpen, i) >= Core.TA_RealBody(inClose, inOpen, i - 1) -
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) && // same size 2 and 3
                Core.TA_RealBody(inClose, inOpen, i) <= Core.TA_RealBody(inClose, inOpen, i - 1) +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1) &&
                inOpen[i] >= inOpen[i - 1] -
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 1) && // same open 2 and 3
                inOpen[i] <= inOpen[i - 1] +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1))
            {
                outInteger[outIdx++] = Core.TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) ? 100 : -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                               Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            i++;
            nearTrailingIdx++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlGapSideSideWhiteLookback() =>
        Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod) + 2;
}
