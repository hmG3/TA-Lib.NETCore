namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlRickshawMan(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlRickshawManLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        double bodyDojiPeriodTotal = default;
        int bodyDojiTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod;
        double shadowLongPeriodTotal = default;
        int shadowLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod;
        double nearPeriodTotal = default;
        int nearTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod;
        int i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i) <=
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) && // doji
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long shadow
                Core.TA_UpperShadow(inClose, inOpen, inLow, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && Math.Min(inOpen[i], inClose[i]) <= inLow[i] +
                Core.TA_HighLowRange(inHigh, inLow, i) / 2 + Core.TA_CandleAverage(inOpen,
                    inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i) && Math.Max(inOpen[i], inClose[i]) >= inLow[i] +
                Core.TA_HighLowRange(inHigh, inLow, i) / 2 - Core.TA_CandleAverage(inOpen,
                    inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i))
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            shadowLongPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i) -
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx);
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i) -
                               Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx);

            i++;
            bodyDojiTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
        } while (i <= endIdx);


        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlRickshawMan(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
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

        int lookbackTotal = CdlRickshawManLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        decimal bodyDojiPeriodTotal = default;
        int bodyDojiTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod;
        decimal shadowLongPeriodTotal = default;
        int shadowLongTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod;
        decimal nearPeriodTotal = default;
        int nearTrailingIdx = startIdx - Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod;
        int i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (Core.TA_RealBody(inClose, inOpen, i) <=
                Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) && // doji
                Core.TA_LowerShadow(inClose, inOpen, inLow, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long shadow
                Core.TA_UpperShadow(inClose, inOpen, inLow, i) > Core.TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && Math.Min(inOpen[i], inClose[i]) <= inLow[i] +
                Core.TA_HighLowRange(inHigh, inLow, i) / 2 + Core.TA_CandleAverage(inOpen,
                    inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i) && Math.Max(inOpen[i], inClose[i]) >= inLow[i] +
                Core.TA_HighLowRange(inHigh, inLow, i) / 2 - Core.TA_CandleAverage(inOpen,
                    inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i))
            {
                outInteger[outIdx++] = 100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            bodyDojiPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            shadowLongPeriodTotal +=
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i) -
                Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx);
            nearPeriodTotal += Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i) -
                               Core.TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx);

            i++;
            bodyDojiTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
        } while (i <= endIdx);


        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlRickshawManLookback() =>
        Math.Max(
            Math.Max(Core.CandleSettings.Get(Core.CandleSettingType.BodyDoji).AveragePeriod, Core.CandleSettings.Get(Core.CandleSettingType.ShadowLong).AveragePeriod),
            Core.CandleSettings.Get(Core.CandleSettingType.Near).AveragePeriod);
}
