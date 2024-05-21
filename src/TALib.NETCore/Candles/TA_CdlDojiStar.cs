namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlDojiStar(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlDojiStarLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyLongPeriodTotal = T.Zero;
        T bodyDojiPeriodTotal = T.Zero;
        int bodyLongTrailingIdx = startIdx - 1 - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int bodyDojiTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 1) && // 1st: long real body
                TA_RealBody(inClose, inOpen, i) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyDojiPeriodTotal, i) && // 2nd: doji
                (TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
                 TA_RealBodyGapUp(inOpen, inClose, i, i - 1) //      that gaps up if 1st is white
                 ||
                 TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black &&
                 TA_RealBodyGapDown(inOpen, inClose, i, i - 1) //      or down if 1st is black
                ))
            {
                outInteger[outIdx++] = -(int) TA_CandleColor(inClose, inOpen, i - 1) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlDojiStarLookback() =>
        Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji), TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 1;
}
