namespace TALib
{
    public static partial class Core
    {
        public static RetCode MedPrice(double[] inHigh, double[] inLow, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (int i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx++] = (inHigh[i] + inLow[i]) / 2.0;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode MedPrice(decimal[] inHigh, decimal[] inLow, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (int i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx++] = (inHigh[i] + inLow[i]) / 2m;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int MedPriceLookback() => 0;
    }
}
