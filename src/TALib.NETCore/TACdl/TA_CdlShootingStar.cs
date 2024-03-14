namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlShootingStar(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlShootingStarLookback();
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
        double shadowLongPeriodTotal = default;
        int shadowLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod;
        double shadowVeryShortPeriodTotal = default;
        int shadowVeryShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod;
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal, i) && // small rb
                Core.TA_UpperShadow(inHigh, inClose, inOpen, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long upper shadow
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) && // very short lower shadow
                Core.TA_RealBodyGapUp(inOpen, inClose, i, i - 1)) // gap up
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
            bodyPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyTrailingIdx);
            shadowLongPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx);
            shadowVeryShortPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
            i++;
            bodyTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlShootingStar(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
        int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement)
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

        int lookbackTotal = CdlShootingStarLookback();
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
        decimal shadowLongPeriodTotal = default;
        int shadowLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod;
        decimal shadowVeryShortPeriodTotal = default;
        int shadowVeryShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod;
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal, i) && // small rb
                Core.TA_UpperShadow(inHigh, inClose, inOpen, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long upper shadow
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) && // very short lower shadow
                Core.TA_RealBodyGapUp(inOpen, inClose, i, i - 1)) // gap up
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
            bodyPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyTrailingIdx);
            shadowLongPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx);
            shadowVeryShortPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
            i++;
            bodyTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlShootingStarLookback() =>
        Math.Max(
            Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod),
            Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod
        ) + 1;
}
