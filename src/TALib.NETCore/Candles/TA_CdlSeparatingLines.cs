namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlSeparatingLines(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx,
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

        T shadowVeryShortPeriodTotal = T.Zero;
        int shadowVeryShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        T bodyLongPeriodTotal = T.Zero;
        int bodyLongTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        T equalPeriodTotal = T.Zero;
        int equalTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Equal);
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if ((int) TA_CandleColor(inClose, inOpen, i - 1) == -(int) TA_CandleColor(inClose, inOpen, i) && // opposite candles
                inOpen[i] <= inOpen[i - 1] +
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) && // same open
                inOpen[i] >= inOpen[i - 1] -
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal, i - 1) &&
                TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i) && // belt hold: long body
                (
                    TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.White && // with no lower shadow if bullish
                    TA_LowerShadow(inClose, inOpen, inLow, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)
                    ||
                    TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.Black && // with no upper shadow if bearish
                    TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i)
                )
               )
            {
                outInteger[outIdx++] = (int) TA_CandleColor(inClose, inOpen, i) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            shadowVeryShortPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i)
                - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
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
            Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort), TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong)),
            TA_CandleAveragePeriod(Core.CandleSettingType.Equal)
        ) + 1;
}
