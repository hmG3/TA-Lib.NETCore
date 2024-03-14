namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlSeparatingLines(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx,
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

        int lookbackTotal = CdlSeparatingLinesLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double shadowVeryShortPeriodTotal = default;
        int shadowVeryShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod;
        double bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        double equalPeriodTotal = default;
        int equalTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod;
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (Core.TA_CandleColor(inClose, inOpen, i - 1) == !Core.TA_CandleColor(inClose, inOpen, i) && // opposite candles
                inOpen[i] <= inOpen[i - 1] +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) && // same open
                inOpen[i] >= inOpen[i - 1] -
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) &&
                Core.TA_RealBody(inClose, inOpen, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i) && // belt hold: long body
                (
                    Core.TA_CandleColor(inClose, inOpen, i) && // with no lower shadow if bullish
                    Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)
                    ||
                    !Core.TA_CandleColor(inClose, inOpen, i) && // with no upper shadow if bearish
                    Core.TA_UpperShadow(inHigh, inClose, inOpen, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)
                )
               )
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
            shadowVeryShortPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            i++;
            shadowVeryShortTrailingIdx++;
            bodyLongTrailingIdx++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlSeparatingLines(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = CdlSeparatingLinesLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal shadowVeryShortPeriodTotal = default;
        int shadowVeryShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod;
        decimal bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        decimal equalPeriodTotal = default;
        int equalTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod;
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (Core.TA_CandleColor(inClose, inOpen, i - 1) == !Core.TA_CandleColor(inClose, inOpen, i) && // opposite candles
                inOpen[i] <= inOpen[i - 1] +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) && // same open
                inOpen[i] >= inOpen[i - 1] -
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) &&
                Core.TA_RealBody(inClose, inOpen, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i) && // belt hold: long body
                (
                    Core.TA_CandleColor(inClose, inOpen, i) && // with no lower shadow if bullish
                    Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)
                    ||
                    !Core.TA_CandleColor(inClose, inOpen, i) && // with no upper shadow if bearish
                    Core.TA_UpperShadow(inHigh, inClose, inOpen, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)
                )
               )
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
            shadowVeryShortPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            i++;
            shadowVeryShortTrailingIdx++;
            bodyLongTrailingIdx++;
            equalTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlSeparatingLinesLookback() =>
        Math.Max(
            Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod),
            Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod
        ) + 1;
}
