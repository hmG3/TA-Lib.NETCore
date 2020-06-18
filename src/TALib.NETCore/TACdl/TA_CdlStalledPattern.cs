using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlStalledPattern(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, int[] outInteger, out int outBegIdx, out int outNbElement)
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

            int lookbackTotal = CdlStalledPatternLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var bodyLongPeriodTotal = new double[3];
            var nearPeriodTotal = new double[3];
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            double bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 2);
                bodyLongPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 1);
                i++;
            }

            i = bodyShortTrailingIdx;
            while (i < startIdx)
            {
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 1);
                i++;
            }

            i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 2);
                nearPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 2) && // 1st white
                    TA_CandleColor(inClose, inOpen, i - 1) && // 2nd white
                    TA_CandleColor(inClose, inOpen, i) && // 3rd white
                    inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] && // consecutive higher closes
                    TA_RealBody(inClose, inOpen, i - 2) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal[2], i - 2) && // 1st: long real body
                    TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal[1], i - 1) && // 2nd: long real body
                    // very short upper shadow
                    TA_UpperShadow(inHigh, inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i - 1) &&
                    // opens within/near 1st real body
                    inOpen[i - 1] > inOpen[i - 2] &&
                    inOpen[i - 1] <= inClose[i - 2] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near,
                        nearPeriodTotal[2], i - 2) &&
                    TA_RealBody(inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyShortPeriodTotal, i) && // 3rd: small real body
                    // rides on the shoulder of 2nd real body
                    inOpen[i] >= inClose[i - 1] - TA_RealBody(inClose, inOpen, i) - TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[1], i - 1)
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
                for (var totIdx = 2; totIdx >= 1; --totIdx)
                {
                    bodyLongPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - totIdx);
                    nearPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearTrailingIdx - totIdx);
                }

                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i) -
                                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx);
                shadowVeryShortPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 1)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx - 1);
                i++;
                bodyLongTrailingIdx++;
                bodyShortTrailingIdx++;
                shadowVeryShortTrailingIdx++;
                nearTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlStalledPattern(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, int[] outInteger, out int outBegIdx, out int outNbElement)
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

            int lookbackTotal = CdlStalledPatternLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var bodyLongPeriodTotal = new decimal[3];
            var nearPeriodTotal = new decimal[3];
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            decimal bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 2);
                bodyLongPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 1);
                i++;
            }

            i = bodyShortTrailingIdx;
            while (i < startIdx)
            {
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 1);
                i++;
            }

            i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 2);
                nearPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 2) && // 1st white
                    TA_CandleColor(inClose, inOpen, i - 1) && // 2nd white
                    TA_CandleColor(inClose, inOpen, i) && // 3rd white
                    inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] && // consecutive higher closes
                    TA_RealBody(inClose, inOpen, i - 2) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal[2], i - 2) && // 1st: long real body
                    TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal[1], i - 1) && // 2nd: long real body
                                                          // very short upper shadow
                    TA_UpperShadow(inHigh, inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i - 1) &&
                    // opens within/near 1st real body
                    inOpen[i - 1] > inOpen[i - 2] &&
                    inOpen[i - 1] <= inClose[i - 2] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near,
                        nearPeriodTotal[2], i - 2) &&
                    TA_RealBody(inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyShortPeriodTotal, i) && // 3rd: small real body
                                                    // rides on the shoulder of 2nd real body
                    inOpen[i] >= inClose[i - 1] - TA_RealBody(inClose, inOpen, i) - TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[1], i - 1)
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
                for (var totIdx = 2; totIdx >= 1; --totIdx)
                {
                    bodyLongPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - totIdx);
                    nearPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearTrailingIdx - totIdx);
                }

                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i) -
                                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx);
                shadowVeryShortPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 1)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx - 1);
                i++;
                bodyLongTrailingIdx++;
                bodyShortTrailingIdx++;
                shadowVeryShortTrailingIdx++;
                nearTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlStalledPatternLookback() =>
            Math.Max(
                Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyLong), TA_CandleAvgPeriod(CandleSettingType.BodyShort)),
                Math.Max(TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort), TA_CandleAvgPeriod(CandleSettingType.Near))
            ) + 2;
    }
}
