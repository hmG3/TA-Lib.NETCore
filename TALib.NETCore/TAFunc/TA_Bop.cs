using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Bop(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
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
                double tempReal = inHigh[i] - inLow[i];
                if (tempReal < 1E-08)
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = (inClose[i] - inOpen[i]) / tempReal;
                    outIdx++;
                }
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Bop(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
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
                decimal tempReal = inHigh[i] - inLow[i];
                if (tempReal < 1E-08m)
                {
                    outReal[outIdx] = Decimal.Zero;
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = (inClose[i] - inOpen[i]) / tempReal;
                    outIdx++;
                }
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int BopLookback()
        {
            return 0;
        }
    }
}
