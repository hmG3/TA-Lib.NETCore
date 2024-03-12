namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlHammer(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

            int lookbackTotal = CdlHammerLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            double shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowLong);
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            double nearPeriodTotal = default;
            int nearTrailingIdx = startIdx - 1 - TA_CandleAvgPeriod(CandleSettingType.Near);
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

            i = nearTrailingIdx;
            while (i < startIdx - 1)
            {
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) <
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyPeriodTotal, i) && // small rb
                    TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long lower shadow
                    TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) && // very short upper shadow
                    Math.Min(inClose[i], inOpen[i]) <= inLow[i - 1] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal,
                        i - 1) // rb near the prior candle's lows
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
                bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i)
                                   - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyTrailingIdx);
                shadowLongPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, shadowLongTrailingIdx);
                shadowVeryShortPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1)
                                   - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearTrailingIdx);
                i++;
                bodyTrailingIdx++;
                shadowLongTrailingIdx++;
                shadowVeryShortTrailingIdx++;
                nearTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode CdlHammer(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

            int lookbackTotal = CdlHammerLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            decimal shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowLong);
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            decimal nearPeriodTotal = default;
            int nearTrailingIdx = startIdx - 1 - TA_CandleAvgPeriod(CandleSettingType.Near);
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

            i = nearTrailingIdx;
            while (i < startIdx - 1)
            {
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) <
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyPeriodTotal, i) && // small rb
                    TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long lower shadow
                    TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) && // very short upper shadow
                    Math.Min(inClose[i], inOpen[i]) <= inLow[i - 1] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal,
                        i - 1) // rb near the prior candle's lows
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
                bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i)
                                   - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyTrailingIdx);
                shadowLongPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, shadowLongTrailingIdx);
                shadowVeryShortPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1)
                                   - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearTrailingIdx);
                i++;
                bodyTrailingIdx++;
                shadowLongTrailingIdx++;
                shadowVeryShortTrailingIdx++;
                nearTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int CdlHammerLookback() =>
            Math.Max(
                Math.Max(
                    Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyShort), TA_CandleAvgPeriod(CandleSettingType.ShadowLong)),
                    TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort)),
                TA_CandleAvgPeriod(CandleSettingType.Near)
            ) + 1;
    }
}
