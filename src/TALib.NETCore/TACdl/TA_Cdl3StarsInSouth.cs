namespace TALib;

public static partial class Candles
{
    public static Core.RetCode Cdl3StarsInSouth(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = Cdl3StarsInSouthLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowVeryShortPeriodTotal = new double[2];
        double bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        double shadowLongPeriodTotal = default;
        int shadowLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod;
        int shadowVeryShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod;
        double bodyShortPeriodTotal = default;
        int bodyShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;

        int i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 2);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[1] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (!Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                !Core.TA_CandleColor(inClose, inOpen, i) &&
                Core.TA_RealBody(inClose, inOpen, i - 2) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i - 2) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i - 2) &&
                Core.TA_RealBody(inClose, inOpen, i - 1) < Core.TA_RealBody(inClose, inOpen, i - 2) &&
                inOpen[i - 1] > inClose[i - 2] && inOpen[i - 1] <= inHigh[i - 2] &&
                inLow[i - 1] < inClose[i - 2] &&
                inLow[i - 1] >= inLow[i - 2] &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i - 1) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                Core.TA_RealBody(inClose, inOpen, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                Core.TA_UpperShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                inLow[i] > inLow[i - 1] && inHigh[i] < inHigh[i - 1])
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2)
                                   - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                                       bodyLongTrailingIdx - 2);
            shadowLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 2)
                                     - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong,
                                         shadowLongTrailingIdx - 2);
            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            bodyShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i)
                                    - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Cdl3StarsInSouth(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = Cdl3StarsInSouthLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowVeryShortPeriodTotal = new decimal[2];
        decimal bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        decimal shadowLongPeriodTotal = default;
        int shadowLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod;
        int shadowVeryShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod;
        decimal bodyShortPeriodTotal = default;
        int bodyShortTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;

        int i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 2);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[1] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            bodyShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (!Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                !Core.TA_CandleColor(inClose, inOpen, i) &&
                Core.TA_RealBody(inClose, inOpen, i - 2) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i - 2) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i - 2) &&
                Core.TA_RealBody(inClose, inOpen, i - 1) < Core.TA_RealBody(inClose, inOpen, i - 2) &&
                inOpen[i - 1] > inClose[i - 2] && inOpen[i - 1] <= inHigh[i - 2] &&
                inLow[i - 1] < inClose[i - 2] &&
                inLow[i - 1] >= inLow[i - 2] &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i - 1) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                Core.TA_RealBody(inClose, inOpen, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                Core.TA_UpperShadow(inClose, inOpen, inLow, i) < Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                inLow[i] > inLow[i - 1] && inHigh[i] < inHigh[i - 1])
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2)
                                   - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                                       bodyLongTrailingIdx - 2);
            shadowLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 2)
                                     - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong,
                                         shadowLongTrailingIdx - 2);
            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            bodyShortPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i)
                                    - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            bodyLongTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int Cdl3StarsInSouthLookback() =>
        Math.Max(
            Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.ShadowVeryShort).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod),
            Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod)
        ) + 2;
}
