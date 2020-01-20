namespace TALib
{
    public partial class Core
    {
        public static RetCode AvgPrice(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
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

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (var i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx] = (inHigh[i] + inLow[i] + inClose[i] + inOpen[i]) / 4.0;
                outIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode AvgPrice(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, decimal[] outReal)
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

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (var i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx] = (inHigh[i] + inLow[i] + inClose[i] + inOpen[i]) / 4m;
                outIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int AvgPriceLookback()
        {
            return 0;
        }
    }
}
