namespace TALib
{
    public partial class Core
    {
        public static RetCode MedPrice(int startIdx, int endIdx, double[] inHigh, double[] inLow, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null)
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
                outReal[outIdx] = (inHigh[i] + inLow[i]) / 2.0;
                outIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode MedPrice(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null)
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
                outReal[outIdx] = (inHigh[i] + inLow[i]) / 2m;
                outIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int MedPriceLookback()
        {
            return 0;
        }
    }
}
