namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlEngulfing(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
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
                if (inClose[i] >= inOpen[i] && inClose[i - 1] < inOpen[i - 1] && inClose[i] > inOpen[i - 1] && inOpen[i] < inClose[i - 1] ||
                    inClose[i] < inOpen[i] && inClose[i - 1] >= inOpen[i - 1] && inOpen[i] > inClose[i - 1] && inClose[i] < inOpen[i - 1])
                {
                    int num;
                    if (inClose[i] >= inOpen[i])
                    {
                        num = 1;
                    }
                    else
                    {
                        num = -1;
                    }

                    outInteger[outIdx] = num * 100;
                    outIdx++;
                }
                else
                {
                    outInteger[outIdx] = 0;
                    outIdx++;
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
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
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
                if (inClose[i] >= inOpen[i] && inClose[i - 1] < inOpen[i - 1] && inClose[i] > inOpen[i - 1] && inOpen[i] < inClose[i - 1] ||
                    inClose[i] < inOpen[i] && inClose[i - 1] >= inOpen[i - 1] && inOpen[i] > inClose[i - 1] && inClose[i] < inOpen[i - 1])
                {
                    int num;
                    if (inClose[i] >= inOpen[i])
                    {
                        num = 1;
                    }
                    else
                    {
                        num = -1;
                    }

                    outInteger[outIdx] = num * 100;
                    outIdx++;
                }
                else
                {
                    outInteger[outIdx] = 0;
                    outIdx++;
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
