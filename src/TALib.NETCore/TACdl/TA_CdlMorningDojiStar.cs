namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlMorningDojiStar(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx,
        int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement, double optInPenetration = 0.3)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null || optInPenetration < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = CdlMorningDojiStarLookback();
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
        T bodyShortPeriodTotal = T.Zero;
        int bodyLongTrailingIdx = startIdx - 2 - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int bodyDojiTrailingIdx = startIdx - 1 - TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        int bodyShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i - 2) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) && // 1st: long
                !TA_CandleColor(inClose, inOpen, i - 2) && //           black
                TA_RealBody(inClose, inOpen, i - 1) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyDojiPeriodTotal, i - 1) && // 2nd: doji
                TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && //           gapping down
                TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i) && // 3rd: longer than short
                TA_CandleColor(inClose, inOpen, i) && //          white real body
                inClose[i] > inClose[i - 2] +
                TA_RealBody(inClose, inOpen, i - 2) * T.CreateChecked(optInPenetration)) //               closing well within 1st rb
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
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i - 1) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                                    TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlMorningDojiStarLookback() =>
        Math.Max(
            Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji), TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong)),
            TA_CandleAveragePeriod(Core.CandleSettingType.BodyShort)
        ) + 2;
}
