using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode RocR100(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 10)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = RocR100Lookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int outIdx = default;
            int inIdx = startIdx;
            int trailingIdx = startIdx - lookbackTotal;
            while (inIdx <= endIdx)
            {
                double tempReal = inReal[trailingIdx++];
                outReal[outIdx++] = !tempReal.Equals(0.0) ? inReal[inIdx] / tempReal * 100.0 : 0.0;
                inIdx++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode RocR100(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 10)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = RocR100Lookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int outIdx = default;
            int inIdx = startIdx;
            int trailingIdx = startIdx - lookbackTotal;
            while (inIdx <= endIdx)
            {
                decimal tempReal = inReal[trailingIdx++];
                outReal[outIdx++] = tempReal != Decimal.Zero ? inReal[inIdx] / tempReal * 100m : Decimal.Zero;
                inIdx++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int RocR100Lookback(int optInTimePeriod = 10)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod;
        }
    }
}
