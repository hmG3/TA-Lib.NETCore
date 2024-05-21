namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode ConcealingBabySwallow(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx,
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

        int lookbackTotal = ConcealingBabySwallowLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowVeryShortPeriodTotal = new T[4];
        int shadowVeryShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[3] += CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 3);
            shadowVeryShortPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 1);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (CandleColor(inClose, inOpen, i - 3) == Core.CandleColor.Black && // 1st black
                CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.Black && // 2nd black
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black && // 3rd black
                CandleColor(inClose, inOpen, i) == Core.CandleColor.Black && // 4th black
                // 1st: marubozu
                LowerShadow(inClose, inOpen, inLow, i - 3) < CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3], i - 3) &&
                UpperShadow(inHigh, inClose, inOpen, i - 3) < CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3], i - 3) &&
                // 2nd: marubozu
                LowerShadow(inClose, inOpen, inLow, i - 2) < CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                UpperShadow(inHigh, inClose, inOpen, i - 2) < CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // 3rd: opens gapping down
                //      and HAS an upper shadow
                UpperShadow(inHigh, inClose, inOpen, i - 1) > CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                inHigh[i - 1] > inClose[i - 2] && //      that extends into the prior body
                inHigh[i] > inHigh[i - 1] && inLow[i] < inLow[i - 1] // 4th: engulfs the 3rd including the shadows
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
            for (var totIdx = 3; totIdx >= 1; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int ConcealingBabySwallowLookback() => CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort) + 3;
}
