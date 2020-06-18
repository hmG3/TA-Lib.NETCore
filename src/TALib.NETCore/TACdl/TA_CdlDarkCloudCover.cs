using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlDarkCloudCover(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, int[] outInteger, out int outBegIdx, out int outNbElement, double optInPenetration = 0.5)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null || optInPenetration < 0.0)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlDarkCloudCoverLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 1);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 1) && // 1st: white
                    TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal, i - 1) && //      long
                    !TA_CandleColor(inClose, inOpen, i) && // 2nd: black
                    inOpen[i] > inHigh[i - 1] && //      open above prior high
                    inClose[i] > inOpen[i - 1] && //      close within prior body
                    inClose[i] < inClose[i - 1] - TA_RealBody(inClose, inOpen, i - 1) * optInPenetration
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
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 1) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - 1);
                i++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlDarkCloudCover(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, int[] outInteger, out int outBegIdx, out int outNbElement, decimal optInPenetration = 0.5m)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null ||
                optInPenetration < Decimal.Zero)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlDarkCloudCoverLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 1);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 1) && // 1st: white
                    TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal, i - 1) && //      long
                    !TA_CandleColor(inClose, inOpen, i) && // 2nd: black
                    inOpen[i] > inHigh[i - 1] && //      open above prior high
                    inClose[i] > inOpen[i - 1] && //      close within prior body
                    inClose[i] < inClose[i - 1] - TA_RealBody(inClose, inOpen, i - 1) * optInPenetration
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
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 1) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - 1);
                i++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlDarkCloudCoverLookback() => TA_CandleAvgPeriod(CandleSettingType.BodyLong) + 1;
    }
}
