using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode Cdl3WhiteSoldiers(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx,
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

            int lookbackTotal = Cdl3WhiteSoldiersLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var shadowVeryShortPeriodTotal = new double[3];
            var nearPeriodTotal = new double[3];
            var farPeriodTotal = new double[3];
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int farTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Far);
            double bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);

            int i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 2);
                shadowVeryShortPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 1);
                shadowVeryShortPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i);
                i++;
            }

            i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 2);
                nearPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1);
                i++;
            }

            i = farTrailingIdx;
            while (i < startIdx)
            {
                farPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Far, i - 2);
                farPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Far, i - 1);
                i++;
            }

            i = bodyShortTrailingIdx;
            while (i < startIdx)
            {
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 2) &&
                    TA_UpperShadow(inHigh, inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                    TA_CandleColor(inClose, inOpen, i - 1) &&
                    TA_UpperShadow(inHigh, inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                    TA_CandleColor(inClose, inOpen, i) &&
                    TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                    inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] &&
                    inOpen[i - 1] > inOpen[i - 2] &&
                    inOpen[i - 1] <= inClose[i - 2] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near,
                        nearPeriodTotal[2], i - 2) &&
                    inOpen[i] > inOpen[i - 1] &&
                    inOpen[i] <= inClose[i - 1] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal[1], i - 1) &&
                    TA_RealBody(inClose, inOpen, i - 1) > TA_RealBody(inClose, inOpen, i - 2) - TA_CandleAverage(inOpen, inHigh,
                        inLow, inClose, CandleSettingType.Far, farPeriodTotal[2], i - 2) &&
                    TA_RealBody(inClose, inOpen, i) > TA_RealBody(inClose, inOpen, i - 1) - TA_CandleAverage(inOpen, inHigh, inLow,
                        inClose, CandleSettingType.Far, farPeriodTotal[1], i - 1) &&
                    TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyShortPeriodTotal, i))
                {
                    outInteger[outIdx++] = 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                for (var totIdx = 2; totIdx >= 0; --totIdx)
                {
                    shadowVeryShortPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort,
                            shadowVeryShortTrailingIdx - totIdx);
                }

                for (var totIdx = 2; totIdx >= 1; --totIdx)
                {
                    farPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Far, i - totIdx)
                                              - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Far,
                                                  farTrailingIdx - totIdx);
                    nearPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - totIdx)
                                               - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near,
                                                   nearTrailingIdx - totIdx);
                }

                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i) -
                                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx);
                i++;
                shadowVeryShortTrailingIdx++;
                nearTrailingIdx++;
                farTrailingIdx++;
                bodyShortTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Cdl3WhiteSoldiers(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

            int lookbackTotal = Cdl3WhiteSoldiersLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var shadowVeryShortPeriodTotal = new decimal[3];
            var nearPeriodTotal = new decimal[3];
            var farPeriodTotal = new decimal[3];
            int shadowVeryShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort);
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int farTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Far);
            decimal bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyShort);

            int i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 2);
                shadowVeryShortPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - 1);
                shadowVeryShortPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i);
                i++;
            }

            i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 2);
                nearPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - 1);
                i++;
            }

            i = farTrailingIdx;
            while (i < startIdx)
            {
                farPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Far, i - 2);
                farPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Far, i - 1);
                i++;
            }

            i = bodyShortTrailingIdx;
            while (i < startIdx)
            {
                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i);
                i++;
            }

            i = startIdx;

            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i - 2) &&
                    TA_UpperShadow(inHigh, inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                    TA_CandleColor(inClose, inOpen, i - 1) &&
                    TA_UpperShadow(inHigh, inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                    TA_CandleColor(inClose, inOpen, i) &&
                    TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                    inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] &&
                    inOpen[i - 1] > inOpen[i - 2] &&
                    inOpen[i - 1] <= inClose[i - 2] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near,
                        nearPeriodTotal[2], i - 2) &&
                    inOpen[i] > inOpen[i - 1] &&
                    inOpen[i] <= inClose[i - 1] +
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal[1], i - 1) &&
                    TA_RealBody(inClose, inOpen, i - 1) > TA_RealBody(inClose, inOpen, i - 2) - TA_CandleAverage(inOpen, inHigh,
                        inLow, inClose, CandleSettingType.Far, farPeriodTotal[2], i - 2) &&
                    TA_RealBody(inClose, inOpen, i) > TA_RealBody(inClose, inOpen, i - 1) - TA_CandleAverage(inOpen, inHigh, inLow,
                        inClose, CandleSettingType.Far, farPeriodTotal[1], i - 1) &&
                    TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                        bodyShortPeriodTotal, i))
                {
                    outInteger[outIdx++] = 100;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                for (var totIdx = 2; totIdx >= 0; --totIdx)
                {
                    shadowVeryShortPeriodTotal[totIdx] +=
                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort, i - totIdx)
                        - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowVeryShort,
                            shadowVeryShortTrailingIdx - totIdx);
                }

                for (var totIdx = 2; totIdx >= 1; --totIdx)
                {
                    farPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Far, i - totIdx)
                                              - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Far,
                                                  farTrailingIdx - totIdx);
                    nearPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i - totIdx)
                                               - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near,
                                                   nearTrailingIdx - totIdx);
                }

                bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i) -
                                        TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx);
                i++;
                shadowVeryShortTrailingIdx++;
                nearTrailingIdx++;
                farTrailingIdx++;
                bodyShortTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int Cdl3WhiteSoldiersLookback() =>
            Math.Max(
                Math.Max(TA_CandleAvgPeriod(CandleSettingType.ShadowVeryShort), TA_CandleAvgPeriod(CandleSettingType.BodyShort)),
                Math.Max(TA_CandleAvgPeriod(CandleSettingType.Far), TA_CandleAvgPeriod(CandleSettingType.Near))
            ) + 2;
    }
}
