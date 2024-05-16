namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlThreeLineStrike(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlThreeLineStrikeLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var nearPeriodTotal = new T[4];
        int nearTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Near);
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[3] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 3);
            nearPeriodTotal[2] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 2) &&
                TA_CandleColor(inClose, inOpen, i - 2) == TA_CandleColor(inClose, inOpen, i - 1) &&
                TA_CandleColor(inClose, inOpen, i) == !TA_CandleColor(inClose, inOpen, i - 1) &&
                inOpen[i - 2] >= T.Min(inOpen[i - 3], inClose[i - 3]) - TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                inOpen[i - 2] <= T.Max(inOpen[i - 3], inClose[i - 3]) + TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                inOpen[i - 1] >= T.Min(inOpen[i - 2], inClose[i - 2]) - TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                inOpen[i - 1] <= T.Max(inOpen[i - 2], inClose[i - 2]) + TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                (
                    TA_CandleColor(inClose, inOpen, i - 1) &&
                    inClose[i - 1] > inClose[i - 2] && inClose[i - 2] > inClose[i - 3] &&
                    inOpen[i] > inClose[i - 1] &&
                    inClose[i] < inOpen[i - 3]
                    ||
                    !TA_CandleColor(inClose, inOpen, i - 1) &&
                    inClose[i - 1] < inClose[i - 2] && inClose[i - 2] < inClose[i - 3] &&
                    inOpen[i] < inClose[i - 1] &&
                    inClose[i] > inOpen[i - 3]
                )
               )
            {
                outInteger[outIdx++] = TA_CandleColor(inClose, inOpen, i - 1) ? 100 : -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            for (var totIdx = 3; totIdx >= 2; --totIdx)
            {
                nearPeriodTotal[totIdx] += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx)
                                           - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near,
                                               nearTrailingIdx - totIdx);
            }

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlThreeLineStrikeLookback() => TA_CandleAveragePeriod(Core.CandleSettingType.Near) + 3;
}
