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

            int outIdx = 0;
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
                double val2 = Math.Abs((double) (tempCY - tempHT));
                if (val2 > greatest)
                {
                    greatest = val2;
                }

                double val3 = Math.Abs((double) (tempCY - tempLT));
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

        public static RetCode TrueRange(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, ref int outBegIdx,
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

            int outIdx = 0;
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
                double val2 = Math.Abs((double) (tempCY - tempHT));
                if (val2 > greatest)
                {
                    greatest = val2;
                }

                double val3 = Math.Abs((double) (tempCY - tempLT));
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
