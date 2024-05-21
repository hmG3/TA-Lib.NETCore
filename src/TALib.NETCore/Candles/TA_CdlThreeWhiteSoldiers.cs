namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlThreeWhiteSoldiers(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx,
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

        int lookbackTotal = CdlThreeWhiteSoldiersLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowVeryShortPeriodTotal = new T[3];
        int shadowVeryShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort);
        var nearPeriodTotal = new T[3];
        int nearTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Near);
        var farPeriodTotal = new T[3];
        int farTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Far);
        T bodyShortPeriodTotal = T.Zero;
        int bodyShortTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyShort);

        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            shadowVeryShortPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 2);
            shadowVeryShortPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - 1);
            shadowVeryShortPeriodTotal[0] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            nearPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = farTrailingIdx;
        while (i < startIdx)
        {
            farPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - 2);
            farPeriodTotal[1] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - 1);
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
            if (TA_CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White &&
                TA_UpperShadow(inHigh, inClose, inOpen, i - 2) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[2], i - 2) &&
                TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White &&
                TA_UpperShadow(inHigh, inClose, inOpen, i - 1) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[1], i - 1) &&
                TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.White &&
                TA_UpperShadow(inHigh, inClose, inOpen, i) < TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowVeryShort, shadowVeryShortPeriodTotal[0], i) &&
                inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] &&
                inOpen[i - 1] > inOpen[i - 2] &&
                inOpen[i - 1] <= inClose[i - 2] + TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near,
                    nearPeriodTotal[2], i - 2) &&
                inOpen[i] > inOpen[i - 1] &&
                inOpen[i] <= inClose[i - 1] +
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[1], i - 1) &&
                TA_RealBody(inClose, inOpen, i - 1) > TA_RealBody(inClose, inOpen, i - 2) - TA_CandleAverage(inOpen, inHigh,
                    inLow, inClose, Core.CandleSettingType.Far, farPeriodTotal[2], i - 2) &&
                TA_RealBody(inClose, inOpen, i) > TA_RealBody(inClose, inOpen, i - 1) - TA_CandleAverage(inOpen, inHigh, inLow,
                    inClose, Core.CandleSettingType.Far, farPeriodTotal[1], i - 1) &&
                TA_RealBody(inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort,
                    bodyShortPeriodTotal, i))
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            for (var totIdx = 2; totIdx >= 0; --totIdx)
            {
                shadowVeryShortPeriodTotal[totIdx] +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort, i - totIdx)
                    - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowVeryShort,
                        shadowVeryShortTrailingIdx - totIdx);
            }

            for (var totIdx = 2; totIdx >= 1; --totIdx)
            {
                farPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - totIdx)
                                          - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far,
                                              farTrailingIdx - totIdx);
                nearPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx)
                                           - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near,
                                               nearTrailingIdx - totIdx);
            }

            bodyShortPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                                    TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyShortTrailingIdx);
            i++;
            shadowVeryShortTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlThreeWhiteSoldiersLookback() =>
        Math.Max(
            Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.ShadowVeryShort), TA_CandleAveragePeriod(Core.CandleSettingType.BodyShort)),
            Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.Far), TA_CandleAveragePeriod(Core.CandleSettingType.Near))
        ) + 2;
}
