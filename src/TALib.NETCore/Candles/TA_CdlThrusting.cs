namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode CdlThrusting(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        T equalPeriodTotal = T.Zero;
        int equalTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Equal);
        T bodyLongPeriodTotal = T.Zero;
        int bodyLongTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int i = equalTrailingIdx;
        while (i < startIdx)
        {
            equalPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (TA_CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black && // 1st: black
                TA_RealBody(inClose, inOpen, i - 1) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 1) && //  long
                TA_CandleColor(inClose, inOpen, i) == Core.CandleColor.White && // 2nd: white
                inOpen[i] < inLow[i - 1] && //  open below prior low
                inClose[i] > inClose[i - 1] +
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalPeriodTotal,
                    i - 1) && //  close into prior body
                inClose[i] <= inClose[i - 1] + TA_RealBody(inClose, inOpen, i - 1) * T.CreateChecked(0.5)) //   under the midpoint
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
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, i - 1) -
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Equal, equalTrailingIdx - 1);
            bodyLongPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 1)
                - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 1);
            i++;
            equalTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlThrustingLookback() =>
        Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.Equal), TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong)) + 1;
}
