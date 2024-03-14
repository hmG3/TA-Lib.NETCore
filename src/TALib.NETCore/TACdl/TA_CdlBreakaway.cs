namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlBreakaway(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlBreakawayLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i - 4) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 4) && // 1st long
                TA_CandleColor(inClose, inOpen, i - 4) ==
                TA_CandleColor(inClose, inOpen, i - 3) && // 1st, 2nd, 4th same color, 5th opposite
                TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 1) &&
                TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) &&
                (!TA_CandleColor(inClose, inOpen, i - 4) && // when 1st is black:
                 TA_RealBodyGapDown(inOpen, inClose, i - 3, i - 4) && // 2nd gaps down
                 inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] && // 3rd has lower high and low than 2nd
                 inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] < inLow[i - 2] && // 4th has lower high and low than 3rd
                 inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4] // 5th closes inside the gap
                 ||
                 TA_CandleColor(inClose, inOpen, i - 4) && // when 1st is white:
                 TA_RealBodyGapUp(inClose, inOpen, i - 3, i - 4) && // 2nd gaps up
                 inHigh[i - 2] > inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 3rd has higher high and low than 2nd
                 inHigh[i - 1] > inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 4th has higher high and low than 3rd
                 inClose[i] < inOpen[i - 3] && inClose[i] > inClose[i - 4])) // 5th closes inside the gap
            {
                outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4)
                                   - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                                       bodyLongTrailingIdx - 4);
            i++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlBreakaway(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlBreakawayLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal bodyLongPeriodTotal = default;
        int bodyLongTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i - 4) > TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 4) && // 1st long
                TA_CandleColor(inClose, inOpen, i - 4) ==
                TA_CandleColor(inClose, inOpen, i - 3) && // 1st, 2nd, 4th same color, 5th opposite
                TA_CandleColor(inClose, inOpen, i - 3) == TA_CandleColor(inClose, inOpen, i - 1) &&
                TA_CandleColor(inClose, inOpen, i - 1) == !TA_CandleColor(inClose, inOpen, i) &&
                (!TA_CandleColor(inClose, inOpen, i - 4) && // when 1st is black:
                 TA_RealBodyGapDown(inOpen, inClose, i - 3, i - 4) && // 2nd gaps down
                 inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] && // 3rd has lower high and low than 2nd
                 inHigh[i - 1] < inHigh[i - 2] && inLow[i - 1] < inLow[i - 2] && // 4th has lower high and low than 3rd
                 inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4] // 5th closes inside the gap
                 ||
                 TA_CandleColor(inClose, inOpen, i - 4) && // when 1st is white:
                 TA_RealBodyGapUp(inClose, inOpen, i - 3, i - 4) && // 2nd gaps up
                 inHigh[i - 2] > inHigh[i - 3] && inLow[i - 2] > inLow[i - 3] && // 3rd has higher high and low than 2nd
                 inHigh[i - 1] > inHigh[i - 2] && inLow[i - 1] > inLow[i - 2] && // 4th has higher high and low than 3rd
                 inClose[i] < inOpen[i - 3] && inClose[i] > inClose[i - 4])) // 5th closes inside the gap
            {
                outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 4)
                                   - TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                                       bodyLongTrailingIdx - 4);
            i++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlBreakawayLookback() => TA_CandleAveragePeriod(Core.CandleSettingType.BodyLong) + 4;
}
