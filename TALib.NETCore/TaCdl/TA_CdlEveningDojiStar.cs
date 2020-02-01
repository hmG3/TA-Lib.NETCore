using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlEveningDojiStar(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger, double optInPenetration = 0.3)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null || optInPenetration < 0.0)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlEveningDojiStarLookback();
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
            double bodyDojiPeriodTotal = default;
            double bodyShortPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - 2 - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int bodyDojiTrailingIdx = startIdx - 1 - TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
            int bodyShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            int i = bodyLongTrailingIdx;
            while (i < startIdx - 2)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = bodyDojiTrailingIdx;
            while (i < startIdx - 1)
            {
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            i = bodyShortTrailingIdx;
            while (i < startIdx)
            {
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i - 2) >
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 2) && // 1st: long
                    TA_CandleColor(inClose, inOpen, i - 2) && //           white
                    TA_RealBody(inClose, inOpen, i - 1) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji,
                        bodyDojiPeriodTotal, i - 1) && // 2nd: doji
                    TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && //           gapping up
                    TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyShortPeriodTotal, i) && // 3rd: longer than short
                    !TA_CandleColor(inClose, inOpen, i) && //          black real body
                    inClose[i] < inClose[i - 2] -
                    TA_RealBody(inClose, inOpen, i - 2) * optInPenetration //               closing well within 1st rb
                )
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
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 2) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx);
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i - 1) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i) -
                                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx);
                i++;
                bodyLongTrailingIdx++;
                bodyDojiTrailingIdx++;
                bodyShortTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlEveningDojiStar(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger, decimal optInPenetration = 0.3m)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null ||
                optInPenetration < Decimal.Zero)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlEveningDojiStarLookback();
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
            decimal bodyDojiPeriodTotal = default;
            decimal bodyShortPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - 2 - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int bodyDojiTrailingIdx = startIdx - 1 - TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
            int bodyShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            int i = bodyLongTrailingIdx;
            while (i < startIdx - 2)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = bodyDojiTrailingIdx;
            while (i < startIdx - 1)
            {
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            i = bodyShortTrailingIdx;
            while (i < startIdx)
            {
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i - 2) >
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongPeriodTotal, i - 2) && // 1st: long
                    TA_CandleColor(inClose, inOpen, i - 2) && //           white
                    TA_RealBody(inClose, inOpen, i - 1) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji,
                        bodyDojiPeriodTotal, i - 1) && // 2nd: doji
                    TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && //           gapping up
                    TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyShortPeriodTotal, i) && // 3rd: longer than short
                    !TA_CandleColor(inClose, inOpen, i) && //          black real body
                    inClose[i] < inClose[i - 2] -
                    TA_RealBody(inClose, inOpen, i - 2) * optInPenetration //               closing well within 1st rb
                )
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
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 2) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx);
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i - 1) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i) -
                                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx);
                i++;
                bodyLongTrailingIdx++;
                bodyDojiTrailingIdx++;
                bodyShortTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlEveningDojiStarLookback()
        {
            return Math.Max(Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyDoji), TA_CandleAvgPeriod(CandleSettingType.BodyLong)),
                       TA_CandleAvgPeriod(CandleSettingType.BodyShort)
                   ) + 2;
        }
    }
}
