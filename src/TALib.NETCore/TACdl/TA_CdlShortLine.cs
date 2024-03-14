namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlShortLine(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlShortLineLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double bodyPeriodTotal = default;
        int bodyTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
        double shadowPeriodTotal = default;
        int shadowTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowShort).AveragePeriod;
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = shadowTrailingIdx;
        while (i < startIdx)
        {
            shadowPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal, i) &&
                Core.TA_UpperShadow(inHigh, inClose, inOpen, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowShort, shadowPeriodTotal, i) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowShort, shadowPeriodTotal, i))
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                               Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyTrailingIdx);
            shadowPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i) -
                                 Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowTrailingIdx);
            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlShortLine(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlShortLineLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal bodyPeriodTotal = default;
        int bodyTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
        decimal shadowPeriodTotal = default;
        int shadowTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowShort).AveragePeriod;
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = shadowTrailingIdx;
        while (i < startIdx)
        {
            shadowPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal, i) &&
                Core.TA_UpperShadow(inHigh, inClose, inOpen, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowShort, shadowPeriodTotal, i) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowShort, shadowPeriodTotal, i))
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                               Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyTrailingIdx);
            shadowPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i) -
                                 Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowTrailingIdx);
            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlShortLineLookback() =>
        Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.ShadowShort).AveragePeriod);
}
