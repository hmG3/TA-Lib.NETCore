using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlIdentical3Crows(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx,
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

            int lookbackTotal = CdlIdentical3CrowsLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var shadowVeryShortPeriodTotal = new double[3];
            var equalPeriodTotal = new double[3];
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int equalTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Equal);
            int i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 2);
                shadowVeryShortPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 1);
                shadowVeryShortPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i);
                i++;
            }

            i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 2);
                equalPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (!TA_CandleColor(inClose, inOpen, i - 2) && // 1st black
                    // very short lower shadow
                    TA_LowerShadow(inClose, inOpen, inLow, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                    !TA_CandleColor(inClose, inOpen, i - 1) && // 2nd black
                    // very short lower shadow
                    TA_LowerShadow(inClose, inOpen, inLow, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                    !TA_CandleColor(inClose, inOpen, i) && // 3rd black
                    // very short lower shadow
                    TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                    inClose[i - 2] > inClose[i - 1] && // three declining
                    inClose[i - 1] > inClose[i] &&
                    // 2nd black opens very close to 1st close
                    inOpen[i - 1] <= inClose[i - 2] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal,
                        equalPeriodTotal[2], i - 2) &&
                    inOpen[i - 1] >= inClose[i - 2] - TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal,
                        equalPeriodTotal[2], i - 2) &&
                    // 3rd black opens very close to 2nd close
                    inOpen[i] <= inClose[i - 1] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal,
                        equalPeriodTotal[1], i - 1) &&
                    inOpen[i] >= inClose[i - 1] - TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal,
                        equalPeriodTotal[1], i - 1))
                {
                    outInteger[outIdx++] = -100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                for (var totIdx = 2; totIdx >= 0; --totIdx)
                {
                    shadowVeryShortPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort,
                            shadowVeryShortTrailingIdx - totIdx);
                }

                for (var totIdx = 2; totIdx >= 1; --totIdx)
                {
                    equalPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalTrailingIdx - totIdx);
                }

                i++;
                shadowVeryShortTrailingIdx++;
                equalTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode CdlIdentical3Crows(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

            int lookbackTotal = CdlIdentical3CrowsLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var shadowVeryShortPeriodTotal = new decimal[3];
            var equalPeriodTotal = new decimal[3];
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int equalTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Equal);
            int i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 2);
                shadowVeryShortPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 1);
                shadowVeryShortPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i);
                i++;
            }

            i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 2);
                equalPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (!TA_CandleColor(inClose, inOpen, i - 2) && // 1st black
                    // very short lower shadow
                    TA_LowerShadow(inClose, inOpen, inLow, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                    !TA_CandleColor(inClose, inOpen, i - 1) && // 2nd black
                    // very short lower shadow
                    TA_LowerShadow(inClose, inOpen, inLow, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                    !TA_CandleColor(inClose, inOpen, i) && // 3rd black
                    // very short lower shadow
                    TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                    inClose[i - 2] > inClose[i - 1] && // three declining
                    inClose[i - 1] > inClose[i] &&
                    // 2nd black opens very close to 1st close
                    inOpen[i - 1] <= inClose[i - 2] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal,
                        equalPeriodTotal[2], i - 2) &&
                    inOpen[i - 1] >= inClose[i - 2] - TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal,
                        equalPeriodTotal[2], i - 2) &&
                    // 3rd black opens very close to 2nd close
                    inOpen[i] <= inClose[i - 1] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal,
                        equalPeriodTotal[1], i - 1) &&
                    inOpen[i] >= inClose[i - 1] - TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal,
                        equalPeriodTotal[1], i - 1))
                {
                    outInteger[outIdx++] = -100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                for (var totIdx = 2; totIdx >= 0; --totIdx)
                {
                    shadowVeryShortPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort,
                            shadowVeryShortTrailingIdx - totIdx);
                }

                for (var totIdx = 2; totIdx >= 1; --totIdx)
                {
                    equalPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalTrailingIdx - totIdx);
                }

                i++;
                shadowVeryShortTrailingIdx++;
                equalTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int CdlIdentical3CrowsLookback() =>
            Math.Max(TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort), TA_CandleAvgPeriod(CandleSettingType.Equal)) + 2;
    }
}
