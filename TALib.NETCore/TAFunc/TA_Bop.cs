using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Bop(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (var i = startIdx; i <= endIdx; i++)
            {
                double tempReal = inHigh[i] - inLow[i];
                if (TA_IsZeroOrNeg(tempReal))
                {
                    outReal[outIdx++] = 0.0;
                }
                else
                {
                    outReal[outIdx++] = (inClose[i] - inOpen[i]) / tempReal;
                }
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode Bop(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, decimal[] outReal)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            int outIdx = default;
            for (var i = startIdx; i <= endIdx; i++)
            {
                decimal tempReal = inHigh[i] - inLow[i];
                if (TA_IsZeroOrNeg(tempReal))
                {
                    outReal[outIdx++] = Decimal.Zero;
                }
                else
                {
                    outReal[outIdx++] = (inClose[i] - inOpen[i]) / tempReal;
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
