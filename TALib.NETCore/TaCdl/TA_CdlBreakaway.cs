using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlBreakaway(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
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

            int lookbackTotal = CdlBreakawayLookback();
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
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i - 4) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal, i - 4) && // 1st long
                    TA_CandleColor(inClose, inOpen, i - 4) ==
                    TA_CandleColor(inClose, inOpen, i - 3) && // 1st, 2nd, 4th same color, 5th opposite
                    TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 1) &&
                    TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) &&
                    (!TA_CandleColor(inClose, inOpen, i - 4) && // when 1st is black:
                     TA_RealBodyGapDown(inOpen, inClose, i - 3, i - 4) && // 2nd gaps down
                     inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] && // 3rd has lower high and low than 2nd
                     inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] < inLow[i - 2] && // 4th has lower high and low than 3rd
                     inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4] // 5th closes inside the gap
                     ||
                     TA_CandleColor(inClose, inOpen, i - 4) && // when 1st is white:
                     TA_RealBodyGapUp(inClose, inOpen, i - 3, i - 4) && // 2nd gaps up
                     inHigh[i - 2] > inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 3rd has higher high and low than 2nd
                     inHigh[i - 1] > inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 4th has higher high and low than 3rd
                     inClose[i] < inOpen[i - 3] && inClose[i] > inClose[i - 4])) // 5th closes inside the gap
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
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4)
                                       - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                                           bodyLongTrailingIdx - 4);
                i++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlBreakaway(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
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

            int lookbackTotal = CdlBreakawayLookback();
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
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i - 4) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal, i - 4) && // 1st long
                    TA_CandleColor(inClose, inOpen, i - 4) ==
                    TA_CandleColor(inClose, inOpen, i - 3) && // 1st, 2nd, 4th same color, 5th opposite
                    TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 1) &&
                    TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) &&
                    (!TA_CandleColor(inClose, inOpen, i - 4) && // when 1st is black:
                     TA_RealBodyGapDown(inOpen, inClose, i - 3, i - 4) && // 2nd gaps down
                     inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] && // 3rd has lower high and low than 2nd
                     inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] < inLow[i - 2] && // 4th has lower high and low than 3rd
                     inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4] // 5th closes inside the gap
                     ||
                     TA_CandleColor(inClose, inOpen, i - 4) && // when 1st is white:
                     TA_RealBodyGapUp(inClose, inOpen, i - 3, i - 4) && // 2nd gaps up
                     inHigh[i - 2] > inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 3rd has higher high and low than 2nd
                     inHigh[i - 1] > inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 4th has higher high and low than 3rd
                     inClose[i] < inOpen[i - 3] && inClose[i] > inClose[i - 4])) // 5th closes inside the gap
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
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4)
                                       - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                                           bodyLongTrailingIdx - 4);
                i++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlBreakawayLookback()
        {
            return TA_CandleAvgPeriod(CandleSettingType.BodyLong) + 4;
        }
    }
}
