namespace TALib
{
    public static partial class Core
    {
        public static RetCode CdlDoji(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

            int lookbackTotal = CdlDojiLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            int i = bodyDojiTrailingIdx;
            while (i < startIdx)
            {
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) <=
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i))
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
                i++;
                bodyDojiTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode CdlDoji(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

            int lookbackTotal = CdlDojiLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            int i = bodyDojiTrailingIdx;
            while (i < startIdx)
            {
                bodyDojiPeriodTotal += TA_CandleRange(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, i);
                i++;
            }

            int outIdx = default;
            do
            {
                if (TA_RealBody(inClose, inOpen, i) <=
                    TA_CandleAverage(inOpen, inHigh, inLow, inClose, CandleSettingType.BodyDoji, bodyDojiPeriodTotal, i))
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
                i++;
                bodyDojiTrailingIdx++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int CdlDojiLookback() => TA_CandleAvgPeriod(CandleSettingType.BodyDoji);
    }
}
