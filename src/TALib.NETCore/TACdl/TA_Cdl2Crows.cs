namespace TALib;

public static partial class Candles
{
    public static Core.RetCode Cdl2Crows(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = Cdl2CrowsLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - 2 - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (TA_CandleColor(inClose, inOpen, i - 2) &&
                TA_RealBody(inClose, inOpen, i - 2) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i) &&
                !TA_CandleColor(inClose, inOpen, i - 1) &&
                TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) &&
                !TA_CandleColor(inClose, inOpen, i) &&
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
                inClose[i] > inOpen[i - 2] && inClose[i] < inClose[i - 2])
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            i++;
            bodyLongPeriodTotal++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Cdl2Crows(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = Cdl2CrowsLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - 2 - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (TA_CandleColor(inClose, inOpen, i - 2) &&
                TA_RealBody(inClose, inOpen, i - 2) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i) &&
                !TA_CandleColor(inClose, inOpen, i - 1) &&
                TA_RealBodyGapUp(inOpen, inClose, i - 1, i - 2) &&
                !TA_CandleColor(inClose, inOpen, i) &&
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
                inClose[i] > inOpen[i - 2] && inClose[i] < inClose[i - 2])
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            i++;
            bodyLongPeriodTotal++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int Cdl2CrowsLookback() => TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong) + 2;
}
