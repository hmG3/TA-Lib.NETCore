using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode WclPrice(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = 0;
            for (int i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx] = ((inHigh[i] + inLow[i]) + (inClose[i] * 2.0)) / 4.0;
                outIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode WclPrice(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = 0;
            for (int i = startIdx; i <= endIdx; i++)
            {
                outReal[outIdx] = ((inHigh[i] + inLow[i]) + (inClose[i] * 2.0)) / 4.0;
                outIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int WclPriceLookback()
        {
            return 0;
        }
    }
}
