using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Roc(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 10)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (startIdx < optInTimePeriod)
            {
                startIdx = optInTimePeriod;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = default;
            int inIdx = startIdx;
            int trailingIdx = startIdx - optInTimePeriod;
            while (true)
            {
                if (inIdx > endIdx)
                {
                    break;
                }

                double tempReal = inReal[trailingIdx];
                trailingIdx++;
                if (!tempReal.Equals(0.0))
                {
                    outReal[outIdx] = (inReal[inIdx] / tempReal - 1.0) * 100.0;
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                inIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Roc(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 10)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (startIdx < optInTimePeriod)
            {
                startIdx = optInTimePeriod;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = default;
            int inIdx = startIdx;
            int trailingIdx = startIdx - optInTimePeriod;
            while (true)
            {
                if (inIdx > endIdx)
                {
                    break;
                }

                decimal tempReal = inReal[trailingIdx];
                trailingIdx++;
                if (tempReal != Decimal.Zero)
                {
                    outReal[outIdx] = (inReal[inIdx] / tempReal - Decimal.One) * 100m;
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = Decimal.Zero;
                    outIdx++;
                }

                inIdx++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int RocLookback(int optInTimePeriod = 10)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod;
        }
    }
}
