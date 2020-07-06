using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode Atan(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement)
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
                outReal[outIdx++] = Math.Atan(inReal[i]);
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Atan(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement)
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
                outReal[outIdx++] = DecimalMath.Atan(inReal[i]);
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int AtanLookback() => 0;
    }
}
