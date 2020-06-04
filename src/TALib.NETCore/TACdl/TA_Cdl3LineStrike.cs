using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3LineStrike(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            out int outBegIdx, out int outNbElement, int[] outInteger)
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

            int lookbackTotal = Cdl3LineStrikeLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var nearPeriodTotal = new double[4];
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 3);
                nearPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 2);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 2) &&
                    TA_CandleColor(inClose, inOpen, i - 2) == TA_CandleColor(inClose, inOpen, i - 1) &&
                    TA_CandleColor(inClose, inOpen, i) == !TA_CandleColor(inClose, inOpen, i - 1) &&
                    inOpen[i - 2] >= Math.Min(inOpen[i - 3], inClose[i - 3]) - TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                    inOpen[i - 2] <= Math.Max(inOpen[i - 3], inClose[i - 3]) + TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                    inOpen[i - 1] >= Math.Min(inOpen[i - 2], inClose[i - 2]) - TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                    inOpen[i - 1] <= Math.Max(inOpen[i - 2], inClose[i - 2]) + TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                    (
                        TA_CandleColor(inClose, inOpen, i - 1) &&
                        inClose[i - 1] > inClose[i - 2] && inClose[i - 2] > inClose[i - 3] &&
                        inOpen[i] > inClose[i - 1] &&
                        inClose[i] < inOpen[i - 3]
                        ||
                        !TA_CandleColor(inClose, inOpen, i - 1) &&
                        inClose[i - 1] < inClose[i - 2] && inClose[i - 2] < inClose[i - 3] &&
                        inOpen[i] < inClose[i - 1] &&
                        inClose[i] > inOpen[i - 3]
                    )
                )
                {
                    outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 1)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                for (var totIdx = 3; totIdx >= 2; --totIdx)
                {
                    nearPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - totIdx)
                                               - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near,
                                                   nearTrailingIdx - totIdx);
                }

                i++;
                nearTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode Cdl3LineStrike(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, out int outBegIdx, out int outNbElement, int[] outInteger)
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

            int lookbackTotal = Cdl3LineStrikeLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var nearPeriodTotal = new decimal[4];
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 3);
                nearPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 2);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 2) &&
                    TA_CandleColor(inClose, inOpen, i - 2) == TA_CandleColor(inClose, inOpen, i - 1) &&
                    TA_CandleColor(inClose, inOpen, i) == !TA_CandleColor(inClose, inOpen, i - 1) &&
                    inOpen[i - 2] >= Math.Min(inOpen[i - 3], inClose[i - 3]) - TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                    inOpen[i - 2] <= Math.Max(inOpen[i - 3], inClose[i - 3]) + TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                    inOpen[i - 1] >= Math.Min(inOpen[i - 2], inClose[i - 2]) - TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                    inOpen[i - 1] <= Math.Max(inOpen[i - 2], inClose[i - 2]) + TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                    (
                        TA_CandleColor(inClose, inOpen, i - 1) &&
                        inClose[i - 1] > inClose[i - 2] && inClose[i - 2] > inClose[i - 3] &&
                        inOpen[i] > inClose[i - 1] &&
                        inClose[i] < inOpen[i - 3]
                        ||
                        !TA_CandleColor(inClose, inOpen, i - 1) &&
                        inClose[i - 1] < inClose[i - 2] && inClose[i - 2] < inClose[i - 3] &&
                        inOpen[i] < inClose[i - 1] &&
                        inClose[i] > inOpen[i - 3]
                    )
                )
                {
                    outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i - 1)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                for (var totIdx = 3; totIdx >= 2; --totIdx)
                {
                    nearPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - totIdx)
                                               - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near,
                                                   nearTrailingIdx - totIdx);
                }

                i++;
                nearTrailingIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int Cdl3LineStrikeLookback() => TA_CandleAvgPeriod(CandleSettingType.Near) + 3;
    }
}
