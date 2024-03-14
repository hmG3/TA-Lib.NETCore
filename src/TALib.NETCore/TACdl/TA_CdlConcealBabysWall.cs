namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlConcealBabysWall(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx,
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

        int lookbackTotal = CdlConcealBabysWallLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowVeryShortPeriodTotal = new double[4];
        int shadowVeryShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 3);
            shadowVeryShortPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 1);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (!TA_CandleColor(inClose, inOpen, i - 3) && // 1st black
                !TA_CandleColor(inClose, inOpen, i - 2) && // 2nd black
                !TA_CandleColor(inClose, inOpen, i - 1) && // 3rd black
                !TA_CandleColor(inClose, inOpen, i) && // 4th black
                // 1st: marubozu
                TA_LowerShadow(inClose, inOpen, inLow, i - 3) < TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3], i - 3) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i - 3) < TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3], i - 3) &&
                // 2nd: marubozu
                TA_LowerShadow(inClose, inOpen, inLow, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // 3rd: opens gapping down
                //      and HAS an upper shadow
                TA_UpperShadow(inHigh, inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
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
                    TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlConcealBabysWall(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = CdlConcealBabysWallLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowVeryShortPeriodTotal = new decimal[4];
        int shadowVeryShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 3);
            shadowVeryShortPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - 1);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (!TA_CandleColor(inClose, inOpen, i - 3) && // 1st black
                !TA_CandleColor(inClose, inOpen, i - 2) && // 2nd black
                !TA_CandleColor(inClose, inOpen, i - 1) && // 3rd black
                !TA_CandleColor(inClose, inOpen, i) && // 4th black
                // 1st: marubozu
                TA_LowerShadow(inClose, inOpen, inLow, i - 3) < TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3], i - 3) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i - 3) < TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[3], i - 3) &&
                // 2nd: marubozu
                TA_LowerShadow(inClose, inOpen, inLow, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                TA_UpperShadow(inHigh, inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                TA_RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // 3rd: opens gapping down
                //      and HAS an upper shadow
                TA_UpperShadow(inHigh, inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inOpen,
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
                    TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - TA_CandleRange(inOpen, inHigh, inLow, inOpen, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlConcealBabysWallLookback() => TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort) + 3;
}
