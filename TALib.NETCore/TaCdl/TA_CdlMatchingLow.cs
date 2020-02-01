using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlMatchingLow(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlMatchingLowLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Equal);
            int i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (!TA_CandleColor(inClose, inOpen, i - 1) && // first black
                    !TA_CandleColor(inClose, inOpen, i) && // second black
                    inClose[i] <= inClose[i - 1] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal,
                        i - 1) && // 1st and 2nd same close
                    inClose[i] >= inClose[i - 1] -
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal, i - 1)
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
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1) -
                                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalTrailingIdx - 1);
                i++;
                equalTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlMatchingLow(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlMatchingLowLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            decimal equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Equal);
            int i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (!TA_CandleColor(inClose, inOpen, i - 1) && // first black
                    !TA_CandleColor(inClose, inOpen, i) && // second black
                    inClose[i] <= inClose[i - 1] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal,
                        i - 1) && // 1st and 2nd same close
                    inClose[i] >= inClose[i - 1] -
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal, i - 1)
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
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1) -
                                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalTrailingIdx - 1);
                i++;
                equalTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlMatchingLowLookback()
        {
            return TA_CandleAvgPeriod(CandleSettingType.Equal) + 1;
        }
    }
}
