using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlTasukiGap(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            out int outBegIdx, out int outNbElement, int[] outInteger)
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

            int lookbackTotal = CdlTasukiGapLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double nearPeriodTotal = default;
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && // upside gap
                    TA_CandleColor(inClose, inOpen, i - 1) && // 1st: white
                    !TA_CandleColor(inClose, inOpen, i) && // 2nd: black
                    inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] && //      that opens within the white rb
                    inClose[i] < inOpen[i - 1] && //      and closes under the white rb
                    inClose[i] > Math.Max(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                    // size of 2 rb near the same
                    Math.Abs(TA_RealBody(inClose, inOpen, i - 1) - TA_RealBody(inClose, inOpen, i)) <
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal, i - 1)
                    ||
                    TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // downside gap
                    !TA_CandleColor(inClose, inOpen, i - 1) && // 1st: black
                    TA_CandleColor(inClose, inOpen, i) && // 2nd: white
                    inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] && //      that opens within the black rb
                    inClose[i] > inOpen[i - 1] && //      and closes above the black rb
                    inClose[i] < Math.Min(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                    // size of 2 rb near the same
                    Math.Abs(TA_RealBody(inClose, inOpen, i - 1) - TA_RealBody(inClose, inOpen, i)) <
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal, i - 1))
                {
                    outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 1)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearTrailingIdx - 1);
                i++;
                nearTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlTasukiGap(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            out int outBegIdx, out int outNbElement, int[] outInteger)
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

            int lookbackTotal = CdlTasukiGapLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal nearPeriodTotal = default;
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && // upside gap
                    TA_CandleColor(inClose, inOpen, i - 1) && // 1st: white
                    !TA_CandleColor(inClose, inOpen, i) && // 2nd: black
                    inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] && //      that opens within the white rb
                    inClose[i] < inOpen[i - 1] && //      and closes under the white rb
                    inClose[i] > Math.Max(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                                                                            // size of 2 rb near the same
                    Math.Abs(TA_RealBody(inClose, inOpen, i - 1) - TA_RealBody(inClose, inOpen, i)) <
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal, i - 1)
                    ||
                    TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // downside gap
                    !TA_CandleColor(inClose, inOpen, i - 1) && // 1st: black
                    TA_CandleColor(inClose, inOpen, i) && // 2nd: white
                    inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] && //      that opens within the black rb
                    inClose[i] > inOpen[i - 1] && //      and closes above the black rb
                    inClose[i] < Math.Min(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                                                                            // size of 2 rb near the same
                    Math.Abs(TA_RealBody(inClose, inOpen, i - 1) - TA_RealBody(inClose, inOpen, i)) <
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal, i - 1))
                {
                    outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 1)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearTrailingIdx - 1);
                i++;
                nearTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlTasukiGapLookback() => TA_CandleAvgPeriod(CandleSettingType.Near) + 2;
    }
}
