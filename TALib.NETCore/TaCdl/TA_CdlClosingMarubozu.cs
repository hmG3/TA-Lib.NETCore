using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlClosingMarubozu(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlClosingMarubozuLookback();
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

            double bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i);
                i++;
            }

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) >
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongPeriodTotal, i) && // long body
                    // white body and very short lower shadow
                    (TA_CandleColor(inClose, inOpen, i) &&
                     TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                         CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)
                     ||
                     // black body and very short upper shadow
                     !TA_CandleColor(inClose, inOpen, i) &&
                     TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                         CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)))
                {
                    outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx);
                shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i)
                                              - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort,
                                                  shadowVeryShortTrailingIdx);
                i++;
                bodyLongTrailingIdx++;
                shadowVeryShortTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlClosingMarubozu(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh,
            decimal[] inLow, decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlClosingMarubozuLookback();
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

            decimal bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i);
                i++;
            }

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) >
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongPeriodTotal, i) && // long body
                                                                                                                            // white body and very short lower shadow
                    (TA_CandleColor(inClose, inOpen, i) &&
                     TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                         CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)
                     ||
                     // black body and very short upper shadow
                     !TA_CandleColor(inClose, inOpen, i) &&
                     TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                         CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)))
                {
                    outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx);
                shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i)
                                              - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort,
                                                  shadowVeryShortTrailingIdx);
                i++;
                bodyLongTrailingIdx++;
                shadowVeryShortTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlClosingMarubozuLookback()
        {
            return Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyLong), TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort));
        }
    }
}
