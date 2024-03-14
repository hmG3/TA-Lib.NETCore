namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlSpinningTop(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlSpinningTopLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double bodyPeriodTotal = default;
        int bodyTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal, i) &&
                Core.TA_UpperShadow(inHigh, inClose, inOpen, i) > Core.TA_RealBody(inClose, inOpen, i) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) > Core.TA_RealBody(inClose, inOpen, i)
               )
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                               Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyTrailingIdx);
            i++;
            bodyTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlSpinningTop(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = CdlSpinningTopLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal bodyPeriodTotal = default;
        int bodyTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i) <
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyPeriodTotal, i) &&
                Core.TA_UpperShadow(inHigh, inClose, inOpen, i) > Core.TA_RealBody(inClose, inOpen, i) &&
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) > Core.TA_RealBody(inClose, inOpen, i)
               )
            {
                outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, i) -
                               Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyShort, bodyTrailingIdx);
            i++;
            bodyTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlSpinningTopLookback() => Core.CandleSettings.Get(Core.CandleSettingType.BodyShort).AveragePeriod;
}
