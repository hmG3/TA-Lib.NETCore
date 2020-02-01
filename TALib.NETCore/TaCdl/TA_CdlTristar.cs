using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlTristar(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
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

            int lookbackTotal = CdlTristarLookback();
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
            int bodyTrailingIdx = startIdx - 2 - TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
            int i = bodyTrailingIdx;
            while (i < startIdx - 2)
            {
                bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i - 2) <=
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyPeriodTotal, i - 2) && // 1st: doji
                    TA_RealBody(inClose, inOpen, i - 1) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji,
                        bodyPeriodTotal, i - 2) && // 2nd: doji
                    TA_RealBody(inClose, inOpen, i) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji,
                        bodyPeriodTotal, i - 2))
                {
                    // 3rd: doji
                    outInteger[outIdx] = 0;
                    if (TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) // 2nd gaps up
                        &&
                        Math.Max(inOpen[i], inClose[i]) < Math.Max(inOpen[i - 1], inClose[i - 1]) // 3rd is not higher than 2nd
                    )
                    {
                        outInteger[outIdx] = -100;
                    }

                    if (TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) // 2nd gaps down
                        &&
                        Math.Min(inOpen[i], inClose[i]) > Math.Min(inOpen[i - 1], inClose[i - 1]) // 3rd is not lower than 2nd
                    )
                    {
                        outInteger[outIdx] = +100;
                    }

                    outIdx++;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i - 2) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyTrailingIdx);
                i++;
                bodyTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlTristar(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
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

            int lookbackTotal = CdlTristarLookback();
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
            int bodyTrailingIdx = startIdx - 2 - TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
            int i = bodyTrailingIdx;
            while (i < startIdx - 2)
            {
                bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i - 2) <=
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyPeriodTotal, i - 2) && // 1st: doji
                    TA_RealBody(inClose, inOpen, i - 1) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji,
                        bodyPeriodTotal, i - 2) && // 2nd: doji
                    TA_RealBody(inClose, inOpen, i) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji,
                        bodyPeriodTotal, i - 2))
                {
                    // 3rd: doji
                    outInteger[outIdx] = 0;
                    if (TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) // 2nd gaps up
                        &&
                        Math.Max(inOpen[i], inClose[i]) < Math.Max(inOpen[i - 1], inClose[i - 1]) // 3rd is not higher than 2nd
                    )
                    {
                        outInteger[outIdx] = -100;
                    }

                    if (TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) // 2nd gaps down
                        &&
                        Math.Min(inOpen[i], inClose[i]) > Math.Min(inOpen[i - 1], inClose[i - 1]) // 3rd is not lower than 2nd
                    )
                    {
                        outInteger[outIdx] = +100;
                    }

                    outIdx++;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i - 2) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyTrailingIdx);
                i++;
                bodyTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlTristarLookback()
        {
            return TA_CandleAvgPeriod(CandleSettingType.BodyDoji) + 2;
        }
    }
}
