namespace TALib;

public static partial class Core
{
    public static RetCode CdlLongLine(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlLongLineLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return RetCode.Success;
        }

        double bodyPeriodTotal = default;
        int bodyTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
        double shadowPeriodTotal = default;
        int shadowTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowShort);
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
            i++;
        }

        i = shadowTrailingIdx;
        while (i < startIdx)
        {
            shadowPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i) >
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyPeriodTotal, i) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    CandleSettingType.ShadowShort, shadowPeriodTotal, i) &&
                TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    CandleSettingType.ShadowShort, shadowPeriodTotal, i))
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
            bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i) -
                               TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyTrailingIdx);
            shadowPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowShort, i) -
                                 TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowShort, shadowTrailingIdx);
            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return RetCode.Success;
    }

    public static RetCode CdlLongLine(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlLongLineLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return RetCode.Success;
        }

        decimal bodyPeriodTotal = default;
        int bodyTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyLong);
        decimal shadowPeriodTotal = default;
        int shadowTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowShort);
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i);
            i++;
        }

        i = shadowTrailingIdx;
        while (i < startIdx)
        {
            shadowPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i) >
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyPeriodTotal, i) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    CandleSettingType.ShadowShort, shadowPeriodTotal, i) &&
                TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    CandleSettingType.ShadowShort, shadowPeriodTotal, i))
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
            bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i) -
                               TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyTrailingIdx);
            shadowPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowShort, i) -
                                 TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowShort, shadowTrailingIdx);
            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return RetCode.Success;
    }

    public static int CdlLongLineLookback() =>
        Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyLong), TA_CandleAvgPeriod(CandleSettingType.ShadowShort));
}
