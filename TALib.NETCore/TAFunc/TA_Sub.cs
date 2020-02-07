namespace TALib
{
    public partial class Core
    {
        public static RetCode Sub(int startIdx, int endIdx, double[] inReal0, double[] inReal1, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal0 == null || inReal1 == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (int i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx++] = inReal0[i] - inReal1[i];
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode Sub(int startIdx, int endIdx, decimal[] inReal0, decimal[] inReal1, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal0 == null || inReal1 == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (int i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx++] = inReal0[i] * inReal1[i];
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int SubLookback()
        {
            return 0;
        }
    }
}
