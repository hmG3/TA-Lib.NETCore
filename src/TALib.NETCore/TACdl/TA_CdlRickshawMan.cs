namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlRickshawMan(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
            int[] outInteger, out int outBegIdx, out int outNbElement)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlRickshawManLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
            double shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowLong);
            double nearPeriodTotal = default;
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int i = bodyDojiTrailingIdx;
            while (i < startIdx)
            {
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            i = shadowLongTrailingIdx;
            while (i < startIdx)
            {
                shadowLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i);
                i++;
            }

            i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i);
                i++;
            }

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) <=
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) && // doji
                    TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long shadow
                    TA_UpperShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && Math.Min(inOpen[i], inClose[i]) <= inLow[i] +
                    TA_HighLowRange(inHigh, inLow, i) / 2 + TA_CandleAverage(inOpen,
                        inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal, i) && Math.Max(inOpen[i], inClose[i]) >= inLow[i] +
                    TA_HighLowRange(inHigh, inLow, i) / 2 - TA_CandleAverage(inOpen,
                        inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal, i))
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
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
                shadowLongPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i) -
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, shadowLongTrailingIdx);
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearTrailingIdx);

                i++;
                bodyDojiTrailingIdx++;
                shadowLongTrailingIdx++;
                nearTrailingIdx++;
            } while (i <= endIdx);


            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode CdlRickshawMan(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx,
            int endIdx, int[] outInteger, out int outBegIdx, out int outNbElement)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlRickshawManLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
            decimal shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.ShadowLong);
            decimal nearPeriodTotal = default;
            int nearTrailingIdx = startIdx - TA_CandleAvgPeriod(CandleSettingType.Near);
            int i = bodyDojiTrailingIdx;
            while (i < startIdx)
            {
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            i = shadowLongTrailingIdx;
            while (i < startIdx)
            {
                shadowLongPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i);
                i++;
            }

            i = nearTrailingIdx;
            while (i < startIdx)
            {
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i);
                i++;
            }

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) <=
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i) && // doji
                    TA_LowerShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && // long shadow
                    TA_UpperShadow(inClose, inOpen, inLow, i) > TA_CandleAverage(inOpen, inHigh, inLow, inClose,
                        CandleSettingType.ShadowLong, shadowLongPeriodTotal, i) && Math.Min(inOpen[i], inClose[i]) <= inLow[i] +
                    TA_HighLowRange(inHigh, inLow, i) / 2 + TA_CandleAverage(inOpen,
                        inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal, i) && Math.Max(inOpen[i], inClose[i]) >= inLow[i] +
                    TA_HighLowRange(inHigh, inLow, i) / 2 - TA_CandleAverage(inOpen,
                        inHigh, inLow, inClose, CandleSettingType.Near, nearPeriodTotal, i))
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
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i) -
                                       TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiTrailingIdx);
                shadowLongPeriodTotal +=
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, i) -
                    TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.ShadowLong, shadowLongTrailingIdx);
                nearPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, i) -
                                   TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.Near, nearTrailingIdx);

                i++;
                bodyDojiTrailingIdx++;
                shadowLongTrailingIdx++;
                nearTrailingIdx++;
            } while (i <= endIdx);


            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int CdlRickshawManLookback() =>
            Math.Max(
                Math.Max(TA_CandleAvgPeriod(CandleSettingType.BodyDoji), TA_CandleAvgPeriod(CandleSettingType.ShadowLong)),
                TA_CandleAvgPeriod(CandleSettingType.Near));
    }
}
