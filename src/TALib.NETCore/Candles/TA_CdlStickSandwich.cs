namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlStickSandwich(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        T equalPeriodTotal = T.Zero;
        int equalTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Equal);
        int i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (TA_CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black && // first black
                TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White && // second white
                TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.Black && // third black
                inLow[i - 1] > inClose[i - 2] && // 2nd low > prior close
                inClose[i] <= inClose[i - 2] +
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 2) && // 1st and 3rd same close
                inClose[i] >= inClose[i - 2] -
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 2)
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
            equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 2) -
                                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 2);
            i++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlStickSandwichLookback() => TA_CandleAveragePeriod(Core.CandleSettingType.Equal) + 2;
}
