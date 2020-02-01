using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlInvertedHammer(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
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

            int lookbackTotal = CdlInvertedHammerLookback();
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

            double bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            double shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowLong);
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int i = bodyTrailingIdx;
            while (i < startIdx)
            {
                bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = shadowLongTrailingIdx;
            while (i < startIdx)
            {
                shadowLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i);
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
                if (TA_RealBody(inClose, inOpen, i) <
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyPeriodTotal, i) && // small rb
                    TA_UpperShadow(inHigh, inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long upper shadow
                    TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) && // very short lower shadow
                    TA_RealBodyGapDown(inOpen, inClose, i, i - 1)) // gap down
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
                bodyPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyTrailingIdx);
                shadowLongPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, shadowLongTrailingIdx);
                shadowVeryShortPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
                i++;
                bodyTrailingIdx++;
                shadowLongTrailingIdx++;
                shadowVeryShortTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlInvertedHammer(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
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

            int lookbackTotal = CdlInvertedHammerLookback();
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

            decimal bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            decimal shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowLong);
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int i = bodyTrailingIdx;
            while (i < startIdx)
            {
                bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = shadowLongTrailingIdx;
            while (i < startIdx)
            {
                shadowLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i);
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
                if (TA_RealBody(inClose, inOpen, i) <
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyPeriodTotal, i) && // small rb
                    TA_UpperShadow(inHigh, inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long upper shadow
                    TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) && // very short lower shadow
                    TA_RealBodyGapDown(inOpen, inClose, i, i - 1)) // gap down
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
                bodyPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyTrailingIdx);
                shadowLongPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, shadowLongTrailingIdx);
                shadowVeryShortPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
                i++;
                bodyTrailingIdx++;
                shadowLongTrailingIdx++;
                shadowVeryShortTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlInvertedHammerLookback()
        {
            return Math.Max(Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyShort), TA_CandleAvgPeriod(CandleSettingType.ShadowLong)),
                       TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort)
                   ) + 1;
        }
    }
}
