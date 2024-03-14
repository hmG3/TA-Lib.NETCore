namespace TALib;

public static partial class Candles
{
    public static Core.RetCode Cdl3LineStrike(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = Cdl3LineStrikeLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var nearPeriodTotal = new double[4];
        int nearTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod;
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[3] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 3);
            nearPeriodTotal[2] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (Core.TA_CandleColor(inClose, inOpen, i - 3) == Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                Core.TA_CandleColor(inClose, inOpen, i - 2) == Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                Core.TA_CandleColor(inClose, inOpen, i) == !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                inOpen[i - 2] >= Math.Min(inOpen[i - 3], inClose[i - 3]) - Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                inOpen[i - 2] <= Math.Max(inOpen[i - 3], inClose[i - 3]) + Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                inOpen[i - 1] >= Math.Min(inOpen[i - 2], inClose[i - 2]) - Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                inOpen[i - 1] <= Math.Max(inOpen[i - 2], inClose[i - 2]) + Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                (
                    Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                    inClose[i - 1] > inClose[i - 2] && inClose[i - 2] > inClose[i - 3] &&
                    inOpen[i] > inClose[i - 1] &&
                    inClose[i] < inOpen[i - 3]
                    ||
                    !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                    inClose[i - 1] < inClose[i - 2] && inClose[i - 2] < inClose[i - 3] &&
                    inOpen[i] < inClose[i - 1] &&
                    inClose[i] > inOpen[i - 3]
                )
               )
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i - 1)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            for (var totIdx = 3; totIdx >= 2; --totIdx)
            {
                nearPeriodTotal[totIdx] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx)
                                           - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near,
                                               nearTrailingIdx - totIdx);
            }

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode Cdl3LineStrike(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = Cdl3LineStrikeLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var nearPeriodTotal = new decimal[4];
        int nearTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod;
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[3] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 3);
            nearPeriodTotal[2] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (Core.TA_CandleColor(inClose, inOpen, i - 3) == Core.TA_CandleColor(inClose, inOpen, i - 2) &&
                Core.TA_CandleColor(inClose, inOpen, i - 2) == Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                Core.TA_CandleColor(inClose, inOpen, i) == !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                inOpen[i - 2] >= Math.Min(inOpen[i - 3], inClose[i - 3]) - Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                inOpen[i - 2] <= Math.Max(inOpen[i - 3], inClose[i - 3]) + Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[3], i - 3) &&
                inOpen[i - 1] >= Math.Min(inOpen[i - 2], inClose[i - 2]) - Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                inOpen[i - 1] <= Math.Max(inOpen[i - 2], inClose[i - 2]) + Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.Near, nearPeriodTotal[2], i - 2) &&
                (
                    Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                    inClose[i - 1] > inClose[i - 2] && inClose[i - 2] > inClose[i - 3] &&
                    inOpen[i] > inClose[i - 1] &&
                    inClose[i] < inOpen[i - 3]
                    ||
                    !Core.TA_CandleColor(inClose, inOpen, i - 1) &&
                    inClose[i - 1] < inClose[i - 2] && inClose[i - 2] < inClose[i - 3] &&
                    inOpen[i] < inClose[i - 1] &&
                    inClose[i] > inOpen[i - 3]
                )
               )
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i - 1)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            for (var totIdx = 3; totIdx >= 2; --totIdx)
            {
                nearPeriodTotal[totIdx] += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx)
                                           - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near,
                                               nearTrailingIdx - totIdx);
            }

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int Cdl3LineStrikeLookback() => Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod + 3;
}
