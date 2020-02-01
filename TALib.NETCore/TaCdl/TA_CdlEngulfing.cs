using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlEngulfing(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlEngulfingLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i) && !TA_CandleColor(inClose, inOpen, i - 1) &&            // white engulfs black
                    (inClose[i] >= inOpen[i - 1] && inOpen[i] < inClose[i - 1] ||
                     inClose[i] > inOpen[i - 1] && inOpen[i] <= inClose[i - 1]
                    )
                    ||
                    !TA_CandleColor(inClose, inOpen, i) && TA_CandleColor(inClose, inOpen, i - 1) &&            // black engulfs white
                    (inOpen[i] >= inClose[i - 1] && inClose[i] < inOpen[i - 1] ||
                     inOpen[i] > inClose[i - 1] && inClose[i] <= inOpen[i - 1]
                    )
                )
                {
                    if (!inOpen[i].Equals(inClose[i - 1]) && !inClose[i].Equals(inOpen[i - 1]))
                    {
                        outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i)) * 100;
                    }
                    else
                    {
                        outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i)) * 80;
                    }
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                i++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode CdlEngulfing(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlEngulfingLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int i = startIdx;
            int outIdx = default;
            do
            {
                if (TA_CandleColor(inClose, inOpen, i) && !TA_CandleColor(inClose, inOpen, i - 1) &&            // white engulfs black
                    (inClose[i] >= inOpen[i - 1] && inOpen[i] < inClose[i - 1] ||
                     inClose[i] > inOpen[i - 1] && inOpen[i] <= inClose[i - 1]
                    )
                    ||
                    !TA_CandleColor(inClose, inOpen, i) && TA_CandleColor(inClose, inOpen, i - 1) &&            // black engulfs white
                    (inOpen[i] >= inClose[i - 1] && inClose[i] < inOpen[i - 1] ||
                     inOpen[i] > inClose[i - 1] && inClose[i] <= inOpen[i - 1]
                    )
                )
                {
                    if (inOpen[i] != inClose[i - 1] && inClose[i] != inOpen[i - 1])
                    {
                        outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i)) * 100;
                    }
                    else
                    {
                        outInteger[outIdx++] = Convert.ToInt32(TA_CandleColor(inClose, inOpen, i)) * 80;
                    }
                }
                else
                {
                    outInteger[outIdx++] = 0;
                }

                i++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int CdlEngulfingLookback()
        {
            return 2;
        }
    }
}
