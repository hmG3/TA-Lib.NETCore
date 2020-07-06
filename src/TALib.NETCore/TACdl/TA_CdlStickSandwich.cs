namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlStickSandwich(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
            int[] outInteger, out int outBegIdx, out int outNbElement)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlStickSandwichLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Equal);
            int i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 2);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (!TA_CandleColor(inClose, inOpen, i - 2) && // first black
                    TA_CandleColor(inClose, inOpen, i - 1) && // second white
                    !TA_CandleColor(inClose, inOpen, i) && // third black
                    inLow[i - 1] > inClose[i - 2] && // 2nd low > prior close
                    inClose[i] <= inClose[i - 2] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal,
                        i - 2) && // 1st and 3rd same close
                    inClose[i] >= inClose[i - 2] -
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal, i - 2)
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
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 2) -
                                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalTrailingIdx - 2);
                i++;
                equalTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode CdlStickSandwich(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
            int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlStickSandwichLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Equal);
            int i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 2);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (!TA_CandleColor(inClose, inOpen, i - 2) && // first black
                    TA_CandleColor(inClose, inOpen, i - 1) && // second white
                    !TA_CandleColor(inClose, inOpen, i) && // third black
                    inLow[i - 1] > inClose[i - 2] && // 2nd low > prior close
                    inClose[i] <= inClose[i - 2] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal,
                        i - 2) && // 1st and 3rd same close
                    inClose[i] >= inClose[i - 2] -
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal, i - 2)
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
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 2) -
                                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalTrailingIdx - 2);
                i++;
                equalTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int CdlStickSandwichLookback() => TA_CandleAvgPeriod(CandleSettingType.Equal) + 2;
    }
}
