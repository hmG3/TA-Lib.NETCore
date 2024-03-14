namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlStickSandwich(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlStickSandwichLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double equalPeriodTotal = default;
        int equalTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod;
        int i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (!Core.TA_CandleColor(inClose, inOpen, i - 2) && // first black
                Core.TA_CandleColor(inClose, inOpen, i - 1) && // second white
                !Core.TA_CandleColor(inClose, inOpen, i) && // third black
                inLow[i - 1] > inClose[i - 2] && // 2nd low > prior close
                inClose[i] <= inClose[i - 2] +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 2) && // 1st and 3rd same close
                inClose[i] >= inClose[i - 2] -
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 2)
               )
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2) -
                                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 2);
            i++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlStickSandwich(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = CdlStickSandwichLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal equalPeriodTotal = default;
        int equalTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod;
        int i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (!Core.TA_CandleColor(inClose, inOpen, i - 2) && // first black
                Core.TA_CandleColor(inClose, inOpen, i - 1) && // second white
                !Core.TA_CandleColor(inClose, inOpen, i) && // third black
                inLow[i - 1] > inClose[i - 2] && // 2nd low > prior close
                inClose[i] <= inClose[i - 2] +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 2) && // 1st and 3rd same close
                inClose[i] >= inClose[i - 2] -
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 2)
               )
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2) -
                                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 2);
            i++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlStickSandwichLookback() => Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod + 2;
}
