namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode TasukiGap(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = TasukiGapLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T nearPeriodTotal = T.Zero;
        int nearTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Near);
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = startIdx;
        int outIdx = default;
        do
        {
            if (RealBodyGapUp(inOpen, inClose, i - 1, i - 2) && // upside gap
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White && // 1st: white
                CandleColor(inClose, inOpen, i) == Core.CandleColor.Black && // 2nd: black
                inOpen[i] < inClose[i - 1] && inOpen[i] > inOpen[i - 1] && //      that opens within the white rb
                inClose[i] < inOpen[i - 1] && //      and closes under the white rb
                inClose[i] > T.Max(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                // size of 2 rb near the same
                T.Abs(RealBody(inClose, inOpen, i - 1) - RealBody(inClose, inOpen, i)) <
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1)
                ||
                RealBodyGapDown(inOpen, inClose, i - 1, i - 2) && // downside gap
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.Black && // 1st: black
                CandleColor(inClose, inOpen, i) == Core.CandleColor.White && // 2nd: white
                inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] && //      that opens within the black rb
                inClose[i] > inOpen[i - 1] && //      and closes above the black rb
                inClose[i] < T.Min(inClose[i - 2], inOpen[i - 2]) && //      inside the gap
                // size of 2 rb near the same
                T.Abs(RealBody(inClose, inOpen, i - 1) - RealBody(inClose, inOpen, i)) <
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i - 1))
            {
                outInteger[outIdx++] = (int) CandleColor(inClose, inOpen, i - 1) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            nearPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1) -
                               CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx - 1);
            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TasukiGapLookback() => CandleAveragePeriod(Core.CandleSettingType.Near) + 2;
}
