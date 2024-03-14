namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlMatHold(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement, double optInPenetration = 0.5)
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

        int lookbackTotal = CdlMatHoldLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var bodyPeriodTotal = new double[5];
        int bodyShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
        int bodyLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        int i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[3] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 3);
            bodyPeriodTotal[2] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 2);
            bodyPeriodTotal[1] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[4] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if ( // 1st long, then 3 small
                Core.TA_RealBody(inClose, inOpen, i - 4) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyPeriodTotal[4], i - 4) &&
                Core.TA_RealBody(inClose, inOpen, i - 3) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[3], i - 3) &&
                Core.TA_RealBody(inClose, inOpen, i - 2) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[2], i - 2) &&
                Core.TA_RealBody(inClose, inOpen, i - 1) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[1], i - 1) &&
                // white, black, 2 black or white, white
                Core.TA_CandleColor(inClose, inOpen, i - 4) &&
                !Core.TA_CandleColor(inClose, inOpen, i - 3) &&
                Core.TA_CandleColor(inClose, inOpen, i) &&
                // upside gap 1st to 2nd
                Core.TA_RealBodyGapUp(inOpen, inClose, i - 3, i - 4) &&
                // 3rd to 4th hold within 1st: a part of the real body must be within 1st real body
                Math.Min(inOpen[i - 2], inClose[i - 2]) < inClose[i - 4] &&
                Math.Min(inOpen[i - 1], inClose[i - 1]) < inClose[i - 4] &&
                // reaction days penetrate first body less than optInPenetration percent
                Math.Min(inOpen[i - 2], inClose[i - 2]) > inClose[i - 4] - Core.TA_RealBody(inClose, inOpen, i - 4) * optInPenetration &&
                Math.Min(inOpen[i - 1], inClose[i - 1]) > inClose[i - 4] - Core.TA_RealBody(inClose, inOpen, i - 4) * optInPenetration &&
                // 2nd to 4th are falling
                Math.Max(inClose[i - 2], inOpen[i - 2]) < inOpen[i - 3] &&
                Math.Max(inClose[i - 1], inOpen[i - 1]) < Math.Max(inClose[i - 2], inOpen[i - 2]) &&
                // 5th opens above the prior close
                inOpen[i] > inClose[i - 1] &&
                // 5th closes above the highest high of the reaction days
                inClose[i] > Math.Max(Math.Max(inHigh[i - 3], inHigh[i - 2]), inHigh[i - 1])
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
            bodyPeriodTotal[4] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                                  Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - totIdx)
                    - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlMatHold(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement, decimal optInPenetration = 0.5m)
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

        int lookbackTotal = CdlMatHoldLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var bodyPeriodTotal = new decimal[5];
        int bodyShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
        int bodyLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        int i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[3] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 3);
            bodyPeriodTotal[2] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 2);
            bodyPeriodTotal[1] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[4] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if ( // 1st long, then 3 small
                Core.TA_RealBody(inClose, inOpen, i - 4) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyPeriodTotal[4], i - 4) &&
                Core.TA_RealBody(inClose, inOpen, i - 3) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[3], i - 3) &&
                Core.TA_RealBody(inClose, inOpen, i - 2) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[2], i - 2) &&
                Core.TA_RealBody(inClose, inOpen, i - 1) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[1], i - 1) &&
                // white, black, 2 black or white, white
                Core.TA_CandleColor(inClose, inOpen, i - 4) &&
                !Core.TA_CandleColor(inClose, inOpen, i - 3) &&
                Core.TA_CandleColor(inClose, inOpen, i) &&
                // upside gap 1st to 2nd
                Core.TA_RealBodyGapUp(inOpen, inClose, i - 3, i - 4) &&
                // 3rd to 4th hold within 1st: a part of the real body must be within 1st real body
                Math.Min(inOpen[i - 2], inClose[i - 2]) < inClose[i - 4] &&
                Math.Min(inOpen[i - 1], inClose[i - 1]) < inClose[i - 4] &&
                // reaction days penetrate first body less than optInPenetration percent
                Math.Min(inOpen[i - 2], inClose[i - 2]) > inClose[i - 4] - Core.TA_RealBody(inClose, inOpen, i - 4) * optInPenetration &&
                Math.Min(inOpen[i - 1], inClose[i - 1]) > inClose[i - 4] - Core.TA_RealBody(inClose, inOpen, i - 4) * optInPenetration &&
                // 2nd to 4th are falling
                Math.Max(inClose[i - 2], inOpen[i - 2]) < inOpen[i - 3] &&
                Math.Max(inClose[i - 1], inOpen[i - 1]) < Math.Max(inClose[i - 2], inOpen[i - 2]) &&
                // 5th opens above the prior close
                inOpen[i] > inClose[i - 1] &&
                // 5th closes above the highest high of the reaction days
                inClose[i] > Math.Max(Math.Max(inHigh[i - 3], inHigh[i - 2]), inHigh[i - 1])
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
            bodyPeriodTotal[4] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                                  Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - totIdx)
                    - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlMatHoldLookback() =>
        Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod) + 4;
}
