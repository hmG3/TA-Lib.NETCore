namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode RisingFallingThreeMethods(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx,
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

        int lookbackTotal = RisingFallingThreeMethodsLookback();
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
            bodyPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            var fifthCandleColor = T.CreateChecked((int) CandleColor(inClose, inOpen, i - 4) * 100);
            if ( // 1st long, then 3 small, 5th long
                RealBody(inClose, inOpen, i - 4) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyPeriodTotal[4], i - 4) &&
                RealBody(inClose, inOpen, i - 3) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[3], i - 3) &&
                RealBody(inClose, inOpen, i - 2) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[2], i - 2) &&
                RealBody(inClose, inOpen, i - 1) < CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyPeriodTotal[1], i - 1) &&
                RealBody(inClose, inOpen, i) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyPeriodTotal[0], i) &&
                // white, 3 black, white  ||  black, 3 white, black
                (int) CandleColor(inClose, inOpen, i - 4) == -(int) CandleColor(inClose, inOpen, i - 3) &&
                CandleColor(inClose, inOpen, i - 3) == CandleColor(inClose, inOpen, i - 2) &&
                CandleColor(inClose, inOpen, i - 2) == CandleColor(inClose, inOpen, i - 1) &&
                (int) CandleColor(inClose, inOpen, i - 1) == -(int) CandleColor(inClose, inOpen, i) &&
                // 2nd to 4th hold within 1st: a part of the real body must be within 1st range
                T.Min(inOpen[i - 3], inClose[i - 3]) < inHigh[i - 4] && T.Max(inOpen[i - 3], inClose[i - 3]) > inLow[i - 4] &&
                T.Min(inOpen[i - 2], inClose[i - 2]) < inHigh[i - 4] && T.Max(inOpen[i - 2], inClose[i - 2]) > inLow[i - 4] &&
                T.Min(inOpen[i - 1], inClose[i - 1]) < inHigh[i - 4] && T.Max(inOpen[i - 1], inClose[i - 1]) > inLow[i - 4] &&
                // 2nd to 4th are falling (rising)
                inClose[i - 2] * fifthCandleColor < inClose[i - 3] * fifthCandleColor &&
                inClose[i - 1] * fifthCandleColor < inClose[i - 2] * fifthCandleColor &&
                // 5th opens above (below) the prior close
                inOpen[i] * fifthCandleColor > inClose[i - 1] * fifthCandleColor &&
                // 5th closes above (below) the 1st close
                inClose[i] * fifthCandleColor > inClose[i - 4] * fifthCandleColor
               )
            {
                outInteger[outIdx++] = (int) CandleColor(inClose, inOpen, i - 4) * 100;
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

            bodyPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i) -
                                  CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int RisingFallingThreeMethodsLookback() =>
        Math.Max(CandleAveragePeriod(Core.CandleSettingType.BodyShort), CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 4;
}
