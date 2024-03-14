namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlTasukiGap(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlTasukiGapLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double nearPeriodTotal = default;
        int nearTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod;
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (Core.TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && // upside gap
                Core.TA_CandleColor(inClose, inOpen, i - 1) && // 1st: white
                !Core.TA_CandleColor(inClose, inOpen, i) && // 2nd: black
                inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] && //      that opens within the white rb
                inClose[i] < inOpen[i - 1] && //      and closes under the white rb
                inClose[i] > Math.Max(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                // size of 2 rb near the same
                Math.Abs(Core.TA_RealBody(inClose, inOpen, i - 1) - Core.TA_RealBody(inClose, inOpen, i)) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
                ||
                Core.TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // downside gap
                !Core.TA_CandleColor(inClose, inOpen, i - 1) && // 1st: black
                Core.TA_CandleColor(inClose, inOpen, i) && // 2nd: white
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] && //      that opens within the black rb
                inClose[i] > inOpen[i - 1] && //      and closes above the black rb
                inClose[i] < Math.Min(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                // size of 2 rb near the same
                Math.Abs(Core.TA_RealBody(inClose, inOpen, i - 1) - Core.TA_RealBody(inClose, inOpen, i)) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1))
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i - 1)) * 100;
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
            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlTasukiGap(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlTasukiGapLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal nearPeriodTotal = default;
        int nearTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod;
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (Core.TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && // upside gap
                Core.TA_CandleColor(inClose, inOpen, i - 1) && // 1st: white
                !Core.TA_CandleColor(inClose, inOpen, i) && // 2nd: black
                inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] && //      that opens within the white rb
                inClose[i] < inOpen[i - 1] && //      and closes under the white rb
                inClose[i] > Math.Max(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                // size of 2 rb near the same
                Math.Abs(Core.TA_RealBody(inClose, inOpen, i - 1) - Core.TA_RealBody(inClose, inOpen, i)) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
                ||
                Core.TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // downside gap
                !Core.TA_CandleColor(inClose, inOpen, i - 1) && // 1st: black
                Core.TA_CandleColor(inClose, inOpen, i) && // 2nd: white
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] && //      that opens within the black rb
                inClose[i] > inOpen[i - 1] && //      and closes above the black rb
                inClose[i] < Math.Min(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                // size of 2 rb near the same
                Math.Abs(Core.TA_RealBody(inClose, inOpen, i - 1) - Core.TA_RealBody(inClose, inOpen, i)) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1))
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i - 1)) * 100;
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
            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlTasukiGapLookback() => Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod + 2;
}
