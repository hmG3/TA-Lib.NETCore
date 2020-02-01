using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3Inside(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
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

            int lookbackTotal = Cdl3InsideLookback();
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
            double bodyShortPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - 2 - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int bodyShortTrailingIdx = startIdx - 1 - TA_CandleAvgPeriod(CandleSettingType.BodyShort);

            int i = bodyLongTrailingIdx;
            while (i < startIdx - 2)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = bodyShortTrailingIdx;
            while (i < startIdx - 1)
            {
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i - 2) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal, i - 2) &&
                    TA_RealBody(inClose, inOpen, i - 1) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyShortPeriodTotal, i - 1) &&
                    Math.Max(inClose[i - 1], inOpen[i - 1]) < Math.Max(inClose[i - 2], inOpen[i - 2]) &&
                    Math.Min(inClose[i - 1], inOpen[i - 1]) > Math.Min(inClose[i - 2], inOpen[i - 2]) &&
                    (TA_CandleColor(inClose, inOpen, i - 2) && !TA_CandleColor(inClose, inOpen, i) && inClose[i] < inOpen[i - 2]
                     ||
                     !TA_CandleColor(inClose, inOpen, i - 2) && TA_CandleColor(inClose, inOpen, i) && inClose[i] > inOpen[i - 2]
                    ))
                {
                    outInteger[outIdx++] = Convert.ToInt32(!TA_CandleColor(inClose, inOpen, i - 2)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 2) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx);
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 1) -
                                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx);
                i++;
                bodyLongTrailingIdx++;
                bodyShortTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3Inside(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
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

            int lookbackTotal = Cdl3InsideLookback();
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
            decimal bodyShortPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - 2 - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
            int bodyShortTrailingIdx = startIdx - 1 - TA_CandleAvgPeriod(CandleSettingType.BodyShort);

            int i = bodyLongTrailingIdx;
            while (i < startIdx - 2)
            {
                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
                i++;
            }

            i = bodyShortTrailingIdx;
            while (i < startIdx - 1)
            {
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i - 2) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                        bodyLongPeriodTotal, i - 2) &&
                    TA_RealBody(inClose, inOpen, i - 1) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyShortPeriodTotal, i - 1) &&
                    Math.Max(inClose[i - 1], inOpen[i - 1]) < Math.Max(inClose[i - 2], inOpen[i - 2]) &&
                    Math.Min(inClose[i - 1], inOpen[i - 1]) > Math.Min(inClose[i - 2], inOpen[i - 2]) &&
                    (TA_CandleColor(inClose, inOpen, i - 2) && !TA_CandleColor(inClose, inOpen, i) && inClose[i] < inOpen[i - 2]
                     ||
                     !TA_CandleColor(inClose, inOpen, i - 2) && TA_CandleColor(inClose, inOpen, i) && inClose[i] > inOpen[i - 2]
                    ))
                {
                    outInteger[outIdx++] = Convert.ToInt32(!TA_CandleColor(inClose, inOpen, i - 2)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 2) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx);
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 1) -
                                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx);
                i++;
                bodyLongTrailingIdx++;
                bodyShortTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl3InsideLookback()
        {
            return Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyShort), TA_CandleAvgPeriod(CandleSettingType.BodyLong)) + 2;
        }
    }
}
