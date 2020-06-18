using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlCounterAttack(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

            int lookbackTotal = CdlCounterAttackLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var bodyLongPeriodTotal = new double[2];
            double equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Equal);
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1);
                i++;
            }

            i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 1);
                bodyLongPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) && // opposite candles
                    TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal[1], i - 1) && // 1st long
                    TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal[0], i) && // 2nd long
                    inClose[i] <= inClose[i - 1] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal, i - 1) && // equal closes
                    inClose[i] >= inClose[i - 1] -
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal, i - 1)
                )
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
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1) -
                                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalTrailingIdx - 1);
                for (var totIdx = 1; totIdx >= 0; --totIdx)
                {
                    bodyLongPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - totIdx);
                }

                i++;
                equalTrailingIdx++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlCounterAttack(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
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

            int lookbackTotal = CdlCounterAttackLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var bodyLongPeriodTotal = new decimal[2];
            decimal equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Equal);
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1);
                i++;
            }

            i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 1);
                bodyLongPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) && // opposite candles
                    TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal[1], i - 1) && // 1st long
                    TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal[0], i) && // 2nd long
                    inClose[i] <= inClose[i - 1] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal, i - 1) && // equal closes
                    inClose[i] >= inClose[i - 1] -
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalPeriodTotal, i - 1)
                )
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
                equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, i - 1) -
                                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Equal, equalTrailingIdx - 1);
                for (var totIdx = 1; totIdx >= 0; --totIdx)
                {
                    bodyLongPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - totIdx);
                }

                i++;
                equalTrailingIdx++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlCounterAttackLookback() =>
            Math.Max(TA_CandleAvgPeriod(CandleSettingType.Equal), TA_CandleAvgPeriod(CandleSettingType.BodyLong)) + 1;
    }
}
