using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Wma(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = WmaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            if (optInTimePeriod == 1)
            {
                outBegIdx = startIdx;
                outNBElement = endIdx - startIdx + 1;
                Array.Copy(inReal, startIdx, outReal, 0, outNBElement);
                return RetCode.Success;
            }

            int divider = (optInTimePeriod * (optInTimePeriod + 1)) >> 1;

            int outIdx = default;
            int trailingIdx = startIdx - lookbackTotal;

            double periodSub = default;
            double periodSum = periodSub;
            int inIdx = trailingIdx;
            int i = 1;
            while (inIdx < startIdx)
            {
                double tempReal = inReal[inIdx++];
                periodSub += tempReal;
                periodSum += tempReal * i;
                i++;
            }
            double trailingValue = default;

            while (inIdx <= endIdx)
            {
                double tempReal = inReal[inIdx++];
                periodSub += tempReal;
                periodSub -= trailingValue;
                periodSum += tempReal * optInTimePeriod;
                trailingValue = inReal[trailingIdx++];
                outReal[outIdx++] = periodSum / divider;
                periodSum -= periodSub;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode Wma(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = WmaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            if (optInTimePeriod == 1)
            {
                outBegIdx = startIdx;
                outNBElement = endIdx - startIdx + 1;
                Array.Copy(inReal, startIdx, outReal, 0, outNBElement);
                return RetCode.Success;
            }

            int divider = (optInTimePeriod * (optInTimePeriod + 1)) >> 1;

            int outIdx = default;
            int trailingIdx = startIdx - lookbackTotal;

            decimal periodSub = default;
            decimal periodSum = periodSub;
            int inIdx = trailingIdx;
            int i = 1;
            while (inIdx < startIdx)
            {
                decimal tempReal = inReal[inIdx++];
                periodSub += tempReal;
                periodSum += tempReal * i;
                i++;
            }
            decimal trailingValue = default;

            while (inIdx <= endIdx)
            {
                decimal tempReal = inReal[inIdx++];
                periodSub += tempReal;
                periodSub -= trailingValue;
                periodSum += tempReal * optInTimePeriod;
                trailingValue = inReal[trailingIdx++];
                outReal[outIdx++] = periodSum / divider;
                periodSum -= periodSub;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int WmaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
