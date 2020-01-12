using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Tanh(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int i = startIdx;
            int outIdx = 0;
            while (i <= endIdx)
            {
                outReal[outIdx] = Math.Tanh(inReal[i]);
                i++;
                outIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Tanh(int startIdx, int endIdx, float[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int i = startIdx;
            int outIdx = 0;
            while (i <= endIdx)
            {
                outReal[outIdx] = Math.Tanh((double) inReal[i]);
                i++;
                outIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int TanhLookback()
        {
            return 0;
        }
    }
}
