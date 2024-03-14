namespace TALib;

public static partial class Candles
{
    public static Core.RetCode CdlEngulfing(double[] inOpen, double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlEngulfingLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int i = startIdx;
        int outIdx = default;
        do
        {
            if (Core.TA_CandleColor(inClose, inOpen, i) && !Core.TA_CandleColor(inClose, inOpen, i - 1) &&            // white engulfs black
                (inClose[i] >= inOpen[i - 1] && inOpen[i] < inClose[i - 1] ||
                 inClose[i] > inOpen[i - 1] && inOpen[i] <= inClose[i - 1]
                )
                ||
                !Core.TA_CandleColor(inClose, inOpen, i) && Core.TA_CandleColor(inClose, inOpen, i - 1) &&            // black engulfs white
                (inOpen[i] >= inClose[i - 1] && inClose[i] < inOpen[i - 1] ||
                 inOpen[i] > inClose[i - 1] && inClose[i] <= inOpen[i - 1]
                )
               )
            {
                if (!inOpen[i].Equals(inClose[i - 1]) && !inClose[i].Equals(inOpen[i - 1]))
                {
                    outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 80;
                }
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode CdlEngulfing(decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx,
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

        int lookbackTotal = CdlEngulfingLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int i = startIdx;
        int outIdx = default;
        do
        {
            if (Core.TA_CandleColor(inClose, inOpen, i) && !Core.TA_CandleColor(inClose, inOpen, i - 1) &&            // white engulfs black
                (inClose[i] >= inOpen[i - 1] && inOpen[i] < inClose[i - 1] ||
                 inClose[i] > inOpen[i - 1] && inOpen[i] <= inClose[i - 1]
                )
                ||
                !Core.TA_CandleColor(inClose, inOpen, i) && Core.TA_CandleColor(inClose, inOpen, i - 1) &&            // black engulfs white
                (inOpen[i] >= inClose[i - 1] && inClose[i] < inOpen[i - 1] ||
                 inOpen[i] > inClose[i - 1] && inClose[i] <= inOpen[i - 1]
                )
               )
            {
                if (inOpen[i] != inClose[i - 1] && inClose[i] != inOpen[i - 1])
                {
                    outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 100;
                }
                else
                {
                    outInteger[outIdx++] = Convert.ToInt32(Core.TA_CandleColor(inClose, inOpen, i)) * 80;
                }
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            i++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int CdlEngulfingLookback() => 2;
}
