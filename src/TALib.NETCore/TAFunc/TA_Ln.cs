using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Ln(int startIdx, int endIdx, double[] inReal, out int outBegIdx, out int outNbElement, double[] outReal)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (int i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx++] = Math.Log(inReal[i]);
            }

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode Ln(int startIdx, int endIdx, decimal[] inReal, out int outBegIdx, out int outNbElement, decimal[] outReal)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (int i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx++] = DecimalMath.Log(inReal[i]);
            }

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int LnLookback() => 0;
    }
}