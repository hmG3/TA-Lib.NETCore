using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlXSideGap3Methods(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
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

            int lookbackTotal = CdlXSideGap3MethodsLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 2) == TA_CandleColor(inClose, inOpen, i - 1) && // 1st and 2nd of same color
                    TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) && // 3rd opposite color
                    inOpen[i] < Math.Max(inClose[i - 1], inOpen[i - 1]) && // 3rd opens within 2nd rb
                    inOpen[i] > Math.Min(inClose[i - 1], inOpen[i - 1]) &&
                    inClose[i] < Math.Max(inClose[i - 2], inOpen[i - 2]) && // 3rd closes within 1st rb
                    inClose[i] > Math.Min(inClose[i - 2], inOpen[i - 2]) &&
                    (TA_CandleColor(inClose, inOpen, i - 2) && // when 1st is white
                     TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) // upside gap
                     ||
                     !TA_CandleColor(inClose, inOpen, i - 2) && // when 1st is black
                     TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2))) // downside gap
                {
                    outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 2)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                i++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlXSideGap3Methods(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
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

            int lookbackTotal = CdlXSideGap3MethodsLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 2) == TA_CandleColor(inClose, inOpen, i - 1) && // 1st and 2nd of same color
                    TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) && // 3rd opposite color
                    inOpen[i] < Math.Max(inClose[i - 1], inOpen[i - 1]) && // 3rd opens within 2nd rb
                    inOpen[i] > Math.Min(inClose[i - 1], inOpen[i - 1]) &&
                    inClose[i] < Math.Max(inClose[i - 2], inOpen[i - 2]) && // 3rd closes within 1st rb
                    inClose[i] > Math.Min(inClose[i - 2], inOpen[i - 2]) &&
                    (TA_CandleColor(inClose, inOpen, i - 2) && // when 1st is white
                     TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) // upside gap
                     ||
                     !TA_CandleColor(inClose, inOpen, i - 2) && // when 1st is black
                     TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2))) // downside gap
                {
                    outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 2)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                i++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlXSideGap3MethodsLookback() => 2;
    }
}
