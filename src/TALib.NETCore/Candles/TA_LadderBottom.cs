namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode LadderBottom(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = LadderBottomLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T shadowVeryShortPeriodTotal = T.Zero;
        int shadowVeryShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (TA_CandleColor(inClose, inOpen, i - 4) == Core.CandleColor.Black &&
                TA_CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black &&
                TA_CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black && // 3 black candlesticks
                inOpen[i - 4] > inOpen[i - 3] && inOpen[i - 3] > inOpen[i - 2] && // with consecutively lower opens
                inClose[i - 4] > inClose[i - 3] && inClose[i - 3] > inClose[i - 2] && // and closes
                TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black && // 4th: black with an upper shadow
                TA_UpperShadow(inHigh, inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal, i - 1) &&
                TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.White && // 5th: white
                inOpen[i] > inOpen[i - 1] && // that opens above prior candle's body
                inClose[i] > inHigh[i - 1]) // and closes above prior candle's high
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
            shadowVeryShortPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1)
                - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, shadowVeryShortTrailingIdx - 1);
            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int LadderBottomLookback() => TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort) + 4;
}