namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlTakuri(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlTakuriLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double bodyDojiPeriodTotal = default;
        int bodyDojiTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        double shadowVeryShortPeriodTotal = default;
        int shadowVeryShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        double shadowVeryLongPeriodTotal = default;
        int shadowVeryLongTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryLong);
        int i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = shadowVeryLongTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i) <=
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) &&
                TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryLongPeriodTotal, i)
               )
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            shadowVeryShortPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i)
                - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
            shadowVeryLongPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, i)
                - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, shadowVeryLongTrailingIdx);
            i++;
            bodyDojiTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            shadowVeryLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlTakuri(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlTakuriLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal bodyDojiPeriodTotal = default;
        int bodyDojiTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        decimal shadowVeryShortPeriodTotal = default;
        int shadowVeryShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        decimal shadowVeryLongPeriodTotal = default;
        int shadowVeryLongTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryLong);
        int i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = shadowVeryLongTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i) <=
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i) &&
                TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryLongPeriodTotal, i)
               )
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            shadowVeryShortPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i)
                - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx);
            shadowVeryLongPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, i)
                - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryLong, shadowVeryLongTrailingIdx);
            i++;
            bodyDojiTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            shadowVeryLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlTakuriLookback() =>
        Math.Max(
            Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji), TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort)),
            TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryLong));
}
