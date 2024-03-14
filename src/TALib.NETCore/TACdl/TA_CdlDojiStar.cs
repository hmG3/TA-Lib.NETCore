namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlDojiStar(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        double bodyLongPeriodTotal = default;
        double bodyDojiPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - 1 - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        int bodyDojiTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod;
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i - 1) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 1) && // 1st: long real body
                Core.TA_RealBody(inClose, inOpen, i) <= Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyDojiPeriodTotal, i) && // 2nd: doji
                (Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                 Core.TA_RealBodyGapUp(inOpen, inClose, i, i - 1) //      that gaps up if 1st is white
                 ||
                 !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                 Core.TA_RealBodyGapDown(inOpen, inClose, i, i - 1) //      or down if 1st is black
                ))
            {
                outInteger[outIdx++] = Convert.ToInt32(!Core.TA_CandleColor(inClose, inOpen, i - 1)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlDojiStar(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

        decimal bodyLongPeriodTotal = default;
        decimal bodyDojiPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - 1 - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        int bodyDojiTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod;
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i - 1) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 1) && // 1st: long real body
                Core.TA_RealBody(inClose, inOpen, i) <= Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyDojiPeriodTotal, i) && // 2nd: doji
                (Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                 Core.TA_RealBodyGapUp(inOpen, inClose, i, i - 1) //      that gaps up if 1st is white
                 ||
                 !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                 Core.TA_RealBodyGapDown(inOpen, inClose, i, i - 1) //      or down if 1st is black
                ))
            {
                outInteger[outIdx++] = Convert.ToInt32(!Core.TA_CandleColor(inClose, inOpen, i - 1)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlDojiStarLookback() =>
        Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod) + 1;
}
