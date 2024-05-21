namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode MatHold(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = MatHoldLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var bodyPeriodTotal = new T[5];
        int bodyShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        int bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[3] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 3);
            bodyPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 2);
            bodyPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal[4] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if ( // 1st long, then 3 small
                RealBody(inClose, inOpen, i - 4) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyPeriodTotal[4], i - 4) &&
                RealBody(inClose, inOpen, i - 3) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[3], i - 3) &&
                RealBody(inClose, inOpen, i - 2) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[2], i - 2) &&
                RealBody(inClose, inOpen, i - 1) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[1], i - 1) &&
                // white, black, 2 black or white, white
                CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.White &&
                CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
                CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
                // upside gap 1st to 2nd
                RealBodyGapUp(inOpen, inClose, i - 3, i - 4) &&
                // 3rd to 4th hold within 1st: a part of the real body must be within 1st real body
                T.Min(inOpen[i - 2], inClose[i - 2]) < inClose[i - 4] &&
                T.Min(inOpen[i - 1], inClose[i - 1]) < inClose[i - 4] &&
                // reaction days penetrate first body less than optInPenetration percent
                T.Min(inOpen[i - 2], inClose[i - 2]) > inClose[i - 4] - RealBody(inClose, inOpen, i - 4) * T.CreateChecked(optInPenetration) &&
                T.Min(inOpen[i - 1], inClose[i - 1]) > inClose[i - 4] - RealBody(inClose, inOpen, i - 4) * T.CreateChecked(optInPenetration) &&
                // 2nd to 4th are falling
                T.Max(inClose[i - 2], inOpen[i - 2]) < inOpen[i - 3] &&
                T.Max(inClose[i - 1], inOpen[i - 1]) < T.Max(inClose[i - 2], inOpen[i - 2]) &&
                // 5th opens above the prior close
                inOpen[i] > inClose[i - 1] &&
                // 5th closes above the highest high of the reaction days
                inClose[i] > T.Max(T.Max(inHigh[i - 3], inHigh[i - 2]), inHigh[i - 1])
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
            bodyPeriodTotal[4] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4) -
                                  CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 4);
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                bodyPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx - totIdx);
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int MatHoldLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyShort), CandleAveragePeriod(Core.CandleSettingType.ShadowLong)) + 4;
}
