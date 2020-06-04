using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode AvgDev(int startIdx, int endIdx, double[] inReal, out int outBegIdx, out int outNbElement, double[] outReal,
            int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            int lookback = AvgDevLookback(optInTimePeriod);
            if (startIdx < lookback)
            {
                startIdx = lookback;
            }

            int today = startIdx;
            if (today > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = today;

            int outIdx = default;
            while (today <= endIdx)
            {
                double todaySum = default;
                for (var i = 0; i < optInTimePeriod; i++)
                {
                    todaySum += inReal[today - i];
                }

                double todayDev = default;
                for (var i = 0; i < optInTimePeriod; i++)
                {
                    todayDev += Math.Abs(inReal[today - i] - todaySum / optInTimePeriod);
                }
                outReal[outIdx++] = todayDev / optInTimePeriod;
                today++;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode AvgDev(int startIdx, int endIdx, decimal[] inReal, out int outBegIdx, out int outNbElement, decimal[] outReal,
            int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            int lookback = AvgDevLookback(optInTimePeriod);
            if (startIdx < lookback)
            {
                startIdx = lookback;
            }

            int today = startIdx;
            if (today > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = today;

            int outIdx = default;
            while (today <= endIdx)
            {
                decimal todaySum = default;
                for (var i = 0; i < optInTimePeriod; i++)
                {
                    todaySum += inReal[today - i];
                }

                decimal todayDev = default;
                for (var i = 0; i < optInTimePeriod; i++)
                {
                    todayDev += Math.Abs(inReal[today - i] - todaySum / optInTimePeriod);
                }
                outReal[outIdx++] = todayDev / optInTimePeriod;
                today++;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int AvgDevLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
