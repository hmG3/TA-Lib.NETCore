namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlAbandonedBaby(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement, double optInPenetration = 0.3)
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

        int lookbackTotal = CdlAbandonedBabyLookback();
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
        double bodyShortPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - 2 - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        int bodyDojiTrailingIdx = startIdx - 1 - Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod;
        int bodyShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i - 2) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) &&
                Core.TA_RealBody(inClose, inOpen, i - 1) <= Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyDojiPeriodTotal, i - 1) &&
                Core.TA_RealBody(inClose, inOpen, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i) &&
                (Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                 !Core.TA_CandleColor(inClose, inOpen, i) &&
                 inClose[i] < inClose[i - 2] - Core.TA_RealBody(inClose, inOpen, i - 2) * optInPenetration &&
                 Core.TA_CandleGapUp(inLow, inHigh, i - 1, i - 2) &&
                 Core.TA_CandleGapDown(inLow, inHigh, i, i - 1)
                 ||
                 !Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                 Core.TA_CandleColor(inClose, inOpen, i) &&
                 inClose[i] > inClose[i - 2] +
                 Core.TA_RealBody(inClose, inOpen, i - 2) * optInPenetration &&
                 Core.TA_CandleGapDown(inLow, inHigh, i - 1, i - 2) &&
                 Core.TA_CandleGapUp(inLow, inHigh, i, i - 1)
                )
               )
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i - 1) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            bodyShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                                    Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlAbandonedBaby(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
        int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement, decimal optInPenetration = 0.3m)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null ||
            optInPenetration < Decimal.Zero)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = CdlAbandonedBabyLookback();
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
        decimal bodyShortPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - 2 - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        int bodyDojiTrailingIdx = startIdx - 1 - Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod;
        int bodyShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 2)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = bodyDojiTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i - 2) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) &&
                Core.TA_RealBody(inClose, inOpen, i - 1) <= Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji,
                    bodyDojiPeriodTotal, i - 1) &&
                Core.TA_RealBody(inClose, inOpen, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i) &&
                (Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                 !Core.TA_CandleColor(inClose, inOpen, i) &&
                 inClose[i] < inClose[i - 2] - Core.TA_RealBody(inClose, inOpen, i - 2) * optInPenetration &&
                 Core.TA_CandleGapUp(inLow, inHigh, i - 1, i - 2) &&
                 Core.TA_CandleGapDown(inLow, inHigh, i, i - 1)
                 ||
                 !Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                 Core.TA_CandleColor(inClose, inOpen, i) &&
                 inClose[i] > inClose[i - 2] +
                 Core.TA_RealBody(inClose, inOpen, i - 2) * optInPenetration &&
                 Core.TA_CandleGapDown(inLow, inHigh, i - 1, i - 2) &&
                 Core.TA_CandleGapUp(inLow, inHigh, i, i - 1))
               )
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i - 1) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            bodyShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                                    Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            bodyDojiTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlAbandonedBabyLookback() =>
        Math.Max(
            Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod),
            Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod
        ) + 2;
}
