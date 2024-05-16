namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlHikkake(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlHikkakeLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int patternIdx = default;
        int patternResult = default;
        int i = startIdx - 3;
        while (i < startIdx)
        {
            if (inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 1st + 2nd: lower high and higher low
                (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] // (bull) 3rd: lower high and lower low
                 ||
                 inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1])) // (bear) 3rd: higher high and higher low
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
            }
            /* search for confirmation if hikkake was no more than 3 bars ago */
            else if (i <= patternIdx + 3 &&
                     (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] // close higher than the high of 2nd
                      ||
                      patternResult < 0 && inClose[i] < inLow[patternIdx - 1])) // close lower than the low of 2nd
            {
                patternIdx = 0;
            }

            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 1st + 2nd: lower high and higher low
                (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1] // (bull) 3rd: lower high and lower low
                 ||
                 inHigh[i] > inHigh[i - 1] && inLow[i] > inLow[i - 1] // (bear) 3rd: higher high and higher low
                )
               )
            {
                patternResult = 100 * (inHigh[i] < inHigh[i - 1] ? 1 : -1);
                patternIdx = i;
                outInteger[outIdx++] = patternResult;
            }
            /* search for confirmation if hikkake was no more than 3 bars ago */
            else if (i <= patternIdx + 3 &&
                     (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] // close higher than the high of 2nd
                      ||
                      patternResult < 0 && inClose[i] < inLow[patternIdx - 1])) // close lower than the low of 2nd
            {
                outInteger[outIdx++] = patternResult + 100 * (patternResult > 0 ? 1 : -1);
                patternIdx = 0;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlHikkakeLookback() => 5;
}
