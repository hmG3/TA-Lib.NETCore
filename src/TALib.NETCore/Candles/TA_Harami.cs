namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Harami(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = HaramiLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyLongPeriodTotal = T.Zero;
        T bodyShortPeriodTotal = T.Zero;
        int bodyLongTrailingIdx = startIdx - 1 - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int bodyShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyShort);
        int i = bodyLongTrailingIdx;
        while (i < startIdx - 1)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i);
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
            if (TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 1) && // 1st: long
                TA_RealBody(inClose, inOpen, i) <= TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i) // 2nd: short
               )
            {
                if (T.Max(inClose[i], inOpen[i]) < T.Max(inClose[i - 1], inOpen[i - 1]) && // 2nd is engulfed by 1st
                    T.Min(inClose[i], inOpen[i]) > T.Min(inClose[i - 1], inOpen[i - 1])
                   )
                {
                    outInteger[outIdx++] = -(int) TA_CandleColor(inClose, inOpen, i - 1) * 100;
                }
                else if (T.Max(inClose[i], inOpen[i]) <= T.Max(inClose[i - 1], inOpen[i - 1]) && // 2nd is engulfed by 1st
                         T.Min(inClose[i], inOpen[i]) >= T.Min(inClose[i - 1], inOpen[i - 1]) // (one end of real body can match;
                        ) // engulfing guaranteed by "long" and "short")
                {
                    outInteger[outIdx++] = -(int) TA_CandleColor(inClose, inOpen, i - 1) * 80;
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyLongPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1) -
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx);
            bodyShortPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int HaramiLookback() =>
        Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.BodyShort), TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 1;
}