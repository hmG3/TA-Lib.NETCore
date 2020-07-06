using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlRiseFall3Methods(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx,
            int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement)
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

            int lookbackTotal = CdlRiseFall3MethodsLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var bodyPeriodTotal = new double[5];
            int bodyShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int i = bodyShortTrailingIdx;
            while (i < startIdx)
            {
                bodyPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 3);
                bodyPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 2);
                bodyPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 1);
                i++;
            }

            i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyPeriodTotal[4] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4);
                bodyPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if ( // 1st long, then 3 small, 5th long
                    TA_RealBody(inClose, inOpen, i - 4) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyPeriodTotal[4], i - 4) &&
                    TA_RealBody(inClose, inOpen, i - 3) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyPeriodTotal[3], i - 3) &&
                    TA_RealBody(inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyPeriodTotal[2], i - 2) &&
                    TA_RealBody(inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyPeriodTotal[1], i - 1) &&
                    TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyPeriodTotal[0], i) &&
                    // white, 3 black, white  ||  black, 3 white, black
                    TA_CandleColor(inClose, inOpen, i - 4) == !TA_CandleColor(inClose, inOpen, i - 3) &&
                    TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 2) &&
                    TA_CandleColor(inClose, inOpen, i - 2) == TA_CandleColor(inClose, inOpen, i - 1) &&
                    TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) &&
                    // 2nd to 4th hold within 1st: a part of the real body must be within 1st range
                    Math.Min(inOpen[i - 3], inClose[i - 3]) < inHigh[i - 4] && Math.Max(inOpen[i - 3], inClose[i - 3]) > inLow[i - 4] &&
                    Math.Min(inOpen[i - 2], inClose[i - 2]) < inHigh[i - 4] && Math.Max(inOpen[i - 2], inClose[i - 2]) > inLow[i - 4] &&
                    Math.Min(inOpen[i - 1], inClose[i - 1]) < inHigh[i - 4] && Math.Max(inOpen[i - 1], inClose[i - 1]) > inLow[i - 4] &&
                    // 2nd to 4th are falling (rising)
                    inClose[i - 2] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) <
                    inClose[i - 3] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) &&
                    inClose[i - 1] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) <
                    inClose[i - 2] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) &&
                    // 5th opens above (below) the prior close
                    inOpen[i] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) >
                    inClose[i - 1] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) &&
                    // 5th closes above (below) the 1st close
                    inClose[i] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) >
                    inClose[i - 4] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4))
                )
                {
                    outInteger[outIdx++] = 100 * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4));
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                bodyPeriodTotal[4] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4) -
                                      TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);
                for (var totIdx = 3; totIdx >= 1; --totIdx)
                {
                    bodyPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
                }

                bodyPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i) -
                                      TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx);

                i++;
                bodyShortTrailingIdx++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode CdlRiseFall3Methods(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
            int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement)
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

            int lookbackTotal = CdlRiseFall3MethodsLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var bodyPeriodTotal = new decimal[5];
            int bodyShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);
            int bodyLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int i = bodyShortTrailingIdx;
            while (i < startIdx)
            {
                bodyPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 3);
                bodyPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 2);
                bodyPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 1);
                i++;
            }

            i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyPeriodTotal[4] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4);
                bodyPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = startIdx;
            int outIdx = default;
            do
            {
                if ( // 1st long, then 3 small, 5th long
                    TA_RealBody(inClose, inOpen, i - 4) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyPeriodTotal[4], i - 4) &&
                    TA_RealBody(inClose, inOpen, i - 3) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyPeriodTotal[3], i - 3) &&
                    TA_RealBody(inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyPeriodTotal[2], i - 2) &&
                    TA_RealBody(inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyPeriodTotal[1], i - 1) &&
                    TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyPeriodTotal[0], i) &&
                    // white, 3 black, white  ||  black, 3 white, black
                    TA_CandleColor(inClose, inOpen, i - 4) == !TA_CandleColor(inClose, inOpen, i - 3) &&
                    TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 2) &&
                    TA_CandleColor(inClose, inOpen, i - 2) == TA_CandleColor(inClose, inOpen, i - 1) &&
                    TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) &&
                    // 2nd to 4th hold within 1st: a part of the real body must be within 1st range
                    Math.Min(inOpen[i - 3], inClose[i - 3]) < inHigh[i - 4] && Math.Max(inOpen[i - 3], inClose[i - 3]) > inLow[i - 4] &&
                    Math.Min(inOpen[i - 2], inClose[i - 2]) < inHigh[i - 4] && Math.Max(inOpen[i - 2], inClose[i - 2]) > inLow[i - 4] &&
                    Math.Min(inOpen[i - 1], inClose[i - 1]) < inHigh[i - 4] && Math.Max(inOpen[i - 1], inClose[i - 1]) > inLow[i - 4] &&
                    // 2nd to 4th are falling (rising)
                    inClose[i - 2] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) <
                    inClose[i - 3] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) &&
                    inClose[i - 1] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) <
                    inClose[i - 2] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) &&
                    // 5th opens above (below) the prior close
                    inOpen[i] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) >
                    inClose[i - 1] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) &&
                    // 5th closes above (below) the 1st close
                    inClose[i] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4)) >
                    inClose[i - 4] * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4))
                )
                {
                    outInteger[outIdx++] = 100 * Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 4));
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                /* add the current range and subtract the first range: this is done after the pattern recognition
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                bodyPeriodTotal[4] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4) -
                                      TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);
                for (var totIdx = 3; totIdx >= 1; --totIdx)
                {
                    bodyPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
                }

                bodyPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i) -
                                      TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx);

                i++;
                bodyShortTrailingIdx++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int CdlRiseFall3MethodsLookback() =>
            Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyShort), TA_CandleAvgPeriod(CandleSettingType.BodyLong)) + 4;
    }
}
