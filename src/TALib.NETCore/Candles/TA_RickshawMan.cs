namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode RickshawMan(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = RickshawManLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        T bodyDojiPeriodTotal = T.Zero;
        int bodyDojiTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji);
        T shadowLongPeriodTotal = T.Zero;
        int shadowLongTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.ShadowLong);
        T nearPeriodTotal = T.Zero;
        int nearTrailingIdx = startIdx - TA_CandleAveragePeriod(Core.CandleSettingType.Near);
        int i = bodyDojiTrailingIdx;
        while (i < startIdx)
        {
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i);
            i++;
        }

        int outIdx = default;
        do
        {
            if (TA_RealBody(inClose, inOpen, i) <=
                TA_CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) && // doji
                TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long shadow
                TA_UpperShadow(inHigh, inClose, inOpen, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && T.Min(inOpen[i], inClose[i]) <= inLow[i] +
                TA_HighLowRange(inHigh, inLow, i) / T.CreateChecked(2) + TA_CandleAverage(inOpen,
                    inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal, i) && T.Max(inOpen[i], inClose[i]) >= inLow[i] +
                TA_HighLowRange(inHigh, inLow, i) / T.CreateChecked(2) - TA_CandleAverage(inOpen,
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
            bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, i) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
            shadowLongPeriodTotal +=
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i) -
                TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx);
            nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i) -
                               TA_CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearTrailingIdx);

            i++;
            bodyDojiTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
        } while (i <= endIdx);


        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int RickshawManLookback() =>
        Math.Max(
            Math.Max(TA_CandleAveragePeriod(Core.CandleSettingType.BodyDoji), TA_CandleAveragePeriod(Core.CandleSettingType.ShadowLong)),
            TA_CandleAveragePeriod(Core.CandleSettingType.Near));
}