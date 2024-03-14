namespace TALib;

public static partial class Candles
{
    public static Core.RetCode Cdl3BlackCrows(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = Cdl3BlackCrowsLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowVeryShortPeriodTotal = new double[3];
        int shadowVeryShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod;
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[2] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (Core.TA_CandleColor(inClose, inOpen, i - 3) &&
                !Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i - 2) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i - 1) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                !Core.TA_CandleColor(inClose, inOpen, i) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                inOpen[i - 1] < inOpen[i - 2] && inOpen[i - 1] > inClose[i - 2] &&
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
                inHigh[i - 3] > inClose[i - 2] &&
                inClose[i - 2] > inClose[i - 1] &&
                inClose[i - 1] > inClose[i])
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            for (var totIdx = 2; totIdx >= 0; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Cdl3BlackCrows(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = Cdl3BlackCrowsLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowVeryShortPeriodTotal = new decimal[3];
        int shadowVeryShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod;
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[2] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (Core.TA_CandleColor(inClose, inOpen, i - 3) &&
                !Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i - 2) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i - 1) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                !Core.TA_CandleColor(inClose, inOpen, i) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                inOpen[i - 1] < inOpen[i - 2] && inOpen[i - 1] > inClose[i - 2] &&
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] &&
                inHigh[i - 3] > inClose[i - 2] &&
                inClose[i - 2] > inClose[i - 1] &&
                inClose[i - 1] > inClose[i])
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            for (var totIdx = 2; totIdx >= 0; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            i++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int Cdl3BlackCrowsLookback() => Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod + 3;
}
