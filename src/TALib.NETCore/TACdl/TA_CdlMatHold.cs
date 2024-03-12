namespace TALib;

public static partial class Core
{
    public static RetCode CdlMatHold(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement, double optInPenetration = 0.5)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null || optInPenetration < 0.0)
        {
            return RetCode.BadParam;
        }

        int lookbackTotal = CdlMatHoldLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return RetCode.Success;
        }

        var bodyPeriodTotal = new double[5];
        int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
        int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
        int i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 3);
            bodyPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 2);
            bodyPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[4] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if ( // 1st long, then 3 small
                TA_RealBody(inClose, inOpen, i - 4) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                    bodyPeriodTotal[4], i - 4) &&
                TA_RealBody(inClose, inOpen, i - 3) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                    bodyPeriodTotal[3], i - 3) &&
                TA_RealBody(inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                    bodyPeriodTotal[2], i - 2) &&
                TA_RealBody(inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                    bodyPeriodTotal[1], i - 1) &&
                // white, black, 2 black or white, white
                TA_CandleColor(inClose, inOpen, i - 4) &&
                !TA_CandleColor(inClose, inOpen, i - 3) &&
                TA_CandleColor(inClose, inOpen, i) &&
                // upside gap 1st to 2nd
                TA_RealBodyGapUp(inOpen, inClose, i - 3, i - 4) &&
                // 3rd to 4th hold within 1st: a part of the real body must be within 1st real body
                Math.Min(inOpen[i - 2], inClose[i - 2]) < inClose[i - 4] &&
                Math.Min(inOpen[i - 1], inClose[i - 1]) < inClose[i - 4] &&
                // reaction days penetrate first body less than optInPenetration percent
                Math.Min(inOpen[i - 2], inClose[i - 2]) > inClose[i - 4] - TA_RealBody(inClose, inOpen, i - 4) * optInPenetration &&
                Math.Min(inOpen[i - 1], inClose[i - 1]) > inClose[i - 4] - TA_RealBody(inClose, inOpen, i - 4) * optInPenetration &&
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
            bodyPeriodTotal[4] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4) -
                                  TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - totIdx)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return RetCode.Success;
    }

    public static RetCode CdlMatHold(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement, decimal optInPenetration = 0.5m)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null ||
            optInPenetration < Decimal.Zero)
        {
            return RetCode.BadParam;
        }

        int lookbackTotal = CdlMatHoldLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return RetCode.Success;
        }

        var bodyPeriodTotal = new decimal[5];
        int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
        int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
        int i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 3);
            bodyPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 2);
            bodyPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[4] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if ( // 1st long, then 3 small
                TA_RealBody(inClose, inOpen, i - 4) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong,
                    bodyPeriodTotal[4], i - 4) &&
                TA_RealBody(inClose, inOpen, i - 3) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                    bodyPeriodTotal[3], i - 3) &&
                TA_RealBody(inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                    bodyPeriodTotal[2], i - 2) &&
                TA_RealBody(inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort,
                    bodyPeriodTotal[1], i - 1) &&
                // white, black, 2 black or white, white
                TA_CandleColor(inClose, inOpen, i - 4) &&
                !TA_CandleColor(inClose, inOpen, i - 3) &&
                TA_CandleColor(inClose, inOpen, i) &&
                // upside gap 1st to 2nd
                TA_RealBodyGapUp(inOpen, inClose, i - 3, i - 4) &&
                // 3rd to 4th hold within 1st: a part of the real body must be within 1st real body
                Math.Min(inOpen[i - 2], inClose[i - 2]) < inClose[i - 4] &&
                Math.Min(inOpen[i - 1], inClose[i - 1]) < inClose[i - 4] &&
                // reaction days penetrate first body less than optInPenetration percent
                Math.Min(inOpen[i - 2], inClose[i - 2]) > inClose[i - 4] - TA_RealBody(inClose, inOpen, i - 4) * optInPenetration &&
                Math.Min(inOpen[i - 1], inClose[i - 1]) > inClose[i - 4] - TA_RealBody(inClose, inOpen, i - 4) * optInPenetration &&
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
            bodyPeriodTotal[4] += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, i - 4) -
                                  TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, i - totIdx)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return RetCode.Success;
    }

    public static int CdlMatHoldLookback() =>
        Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyShort), TA_CandleAvgPeriod(CandleSettingType.ShadowLong)) + 4;
}
