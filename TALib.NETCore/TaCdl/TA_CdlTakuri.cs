using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlTakuri(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
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

            int lookbackTotal = CdlTakuriLookback();
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

            double bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            double shadowVeryLongPeriodTotal = default;
            int shadowVeryLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryLong);
            int i = bodyDojiTrailingIdx;
            while (i < startIdx)
            {
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i);
                i++;
            }

            i = shadowVeryLongTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryLong, i);
                i++;
            }

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) <=
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) &&
                    TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) &&
                    TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryLongPeriodTotal, i)
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
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
                shadowVeryShortPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
                shadowVeryLongPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryLong, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryLong, shadowVeryLongTrailingIdx);
                i++;
                bodyDojiTrailingIdx++;
                shadowVeryShortTrailingIdx++;
                shadowVeryLongTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlTakuri(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
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

            int lookbackTotal = CdlTakuriLookback();
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

            decimal bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            decimal shadowVeryLongPeriodTotal = default;
            int shadowVeryLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryLong);
            int i = bodyDojiTrailingIdx;
            while (i < startIdx)
            {
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i);
                i++;
            }

            i = shadowVeryLongTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryLong, i);
                i++;
            }

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) <=
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) &&
                    TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) &&
                    TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryLongPeriodTotal, i)
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
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
                shadowVeryShortPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
                shadowVeryLongPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryLong, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryLong, shadowVeryLongTrailingIdx);
                i++;
                bodyDojiTrailingIdx++;
                shadowVeryShortTrailingIdx++;
                shadowVeryLongTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlTakuriLookback()
        {
            return Math.Max(
                Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyDoji), TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort)),
                TA_CandleAvgPeriod(CandleSettingType.ShadowVeryLong));
        }
    }
}
