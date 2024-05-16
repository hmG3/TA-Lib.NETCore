namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlLongLine(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlLongLineLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyPeriodTotal = T.Zero;
        int bodyTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        T shadowPeriodTotal = T.Zero;
        int shadowTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowShort);
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = shadowTrailingIdx;
        while (i < startIdx)
        {
            shadowPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i) >
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyPeriodTotal, i) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowShort, shadowPeriodTotal, i) &&
                TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowShort, shadowPeriodTotal, i))
            {
                outInteger[outIdx++] = TA_CandleColor(inClose, inOpen, i) ? 100 : -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i) -
                               TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyTrailingIdx);
            shadowPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i) -
                                 TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowTrailingIdx);
            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlLongLineLookback() =>
        Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong), TA_CandleAveragePeriod(Core.CandleSettingType.ShadowShort));
}
