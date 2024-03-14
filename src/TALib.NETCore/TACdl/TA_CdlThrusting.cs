namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlThrusting(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlThrustingLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double equalPeriodTotal = default;
        int equalTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod;
        double bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        int i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (!Core.TA_CandleColor(inClose, inOpen, i - 1) && // 1st: black
                Core.TA_RealBody(inClose, inOpen, i - 1) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 1) && //  long
                Core.TA_CandleColor(inClose, inOpen, i) && // 2nd: white
                inOpen[i] < inLow[i - 1] && //  open below prior low
                inClose[i] > inClose[i - 1] +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 1) && //  close into prior body
                inClose[i] <= inClose[i - 1] + Core.TA_RealBody(inClose, inOpen, i - 1) * 0.5) //   under the midpoint
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            equalPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            bodyLongPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 1);
            i++;
            equalTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlThrusting(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlThrustingLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal equalPeriodTotal = default;
        int equalTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod;
        decimal bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod;
        int i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (!Core.TA_CandleColor(inClose, inOpen, i - 1) && // 1st: black
                Core.TA_RealBody(inClose, inOpen, i - 1) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 1) && //  long
                Core.TA_CandleColor(inClose, inOpen, i) && // 2nd: white
                inOpen[i] < inLow[i - 1] && //  open below prior low
                inClose[i] > inClose[i - 1] +
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 1) && //  close into prior body
                inClose[i] <= inClose[i - 1] + Core.TA_RealBody(inClose, inOpen, i - 1) * 0.5m) //   under the midpoint
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            equalPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            bodyLongPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1)
                - Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 1);
            i++;
            equalTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlThrustingLookback() =>
        Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.Equal).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.BodyLong).AveragePeriod) + 1;
}
