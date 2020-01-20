using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode TrueRange(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (startIdx < 1)
            {
                startIdx = 1;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = default;
            int today = startIdx;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                double tempLT = inLow[today];
                double tempHT = inHigh[today];
                double tempCY = inClose[today - 1];
                double greatest = tempHT - tempLT;
                double val2 = Math.Abs(tempCY - tempHT);
                if (val2 > greatest)
                {
                    greatest = val2;
                }

                double val3 = Math.Abs(tempCY - tempLT);
                if (val3 > greatest)
                {
                    greatest = val3;
                }

                outReal[outIdx] = greatest;
                outIdx++;
                today++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode TrueRange(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, ref int outBegIdx,
            ref int outNBElement, decimal[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (startIdx < 1)
            {
                startIdx = 1;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = default;
            int today = startIdx;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                decimal tempLT = inLow[today];
                decimal tempHT = inHigh[today];
                decimal tempCY = inClose[today - 1];
                decimal greatest = tempHT - tempLT;
                decimal val2 = Math.Abs(tempCY - tempHT);
                if (val2 > greatest)
                {
                    greatest = val2;
                }

                decimal val3 = Math.Abs(tempCY - tempLT);
                if (val3 > greatest)
                {
                    greatest = val3;
                }

                outReal[outIdx] = greatest;
                outIdx++;
                today++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int TrueRangeLookback()
        {
            return 1;
        }
    }
}
