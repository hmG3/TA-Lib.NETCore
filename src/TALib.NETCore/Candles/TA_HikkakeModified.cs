namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode HikkakeModified(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = HikkakeModifiedLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T nearPeriodTotal = T.Zero;
        int nearTrailingIdx = startIdx - 3 - CandleAveragePeriod(Core.CandleSettingType.Near);
        int i = nearTrailingIdx;
        while (i < startIdx - 3)
        {
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            i++;
        }

        int patternIdx = default;
        int patternResult = default;
        i = startIdx - 3;
        while (i < startIdx)
        {
            if (inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 2nd: lower high and higher low than 1st
                inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 3rd: lower high and higher low than 2nd
                (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] && // (bull) 4th: lower high and lower low
                 inClose[i - 2] <= inLow[i - 2] +
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)
                 ||
                 inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1] && // (bear) 4th: higher high and higher low
                 inClose[i - 2] >= inHigh[i - 2] -
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)))
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
            }
            else
                /* search for confirmation if modified hikkake was no more than 3 bars ago */
            if (i <= patternIdx + 3 &&
                (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] // close higher than the high of 3rd
                 ||
                 patternResult < 0 && inClose[i] < inLow[patternIdx - 1])) // close lower than the low of 3rd
            {
                patternIdx = 0;
            }

            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2) -
                               CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 2);
            nearTrailingIdx++;
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 2nd: lower high and higher low than 1st
                inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 3rd: lower high and higher low than 2nd
                (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] && // (bull) 4th: lower high and lower low
                 inClose[i - 2] <= inLow[i - 2] +
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)
                 ||
                 inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1] && // (bear) 4th: higher high and higher low
                 inClose[i - 2] >= inHigh[i - 2] -
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 2)))
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
                outInteger[outIdx++] = patternResult;
            }
            else
                /* search for confirmation if modified hikkake was no more than 3 bars ago */
            if (i <= patternIdx + 3 &&
                (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] // close higher than the high of 3rd
                 ||
                 patternResult < 0 && inClose[i] < inLow[patternIdx - 1])) // close lower than the low of 3rd
            {
                outInteger[outIdx++] = patternResult + 100 * (patternResult > 0 ? 1 : -1);
                patternIdx = 0;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2) -
                               CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 2);
            nearTrailingIdx++;
            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int HikkakeModifiedLookback() => Math.Max(1, CandleAveragePeriod(Core.CandleSettingType.Near)) + 5;
}
