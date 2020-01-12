using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MidPoint(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, ref int outBegIdx,
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

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int nbInitialElementNeeded = optInTimePeriod - 1;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = 0;
            int today = startIdx;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            while (true)
            {
                if (today > endIdx)
                {
                    outBegIdx = startIdx;
                    outNBElement = outIdx;
                    return RetCode.Success;
                }

                double lowest = inReal[trailingIdx];
                trailingIdx++;
                double highest = lowest;
                for (int i = trailingIdx; i <= today; i++)
                {
                    double tmp = inReal[i];
                    if (tmp < lowest)
                    {
                        lowest = tmp;
                    }
                    else if (tmp > highest)
                    {
                        highest = tmp;
                    }
                }

                outReal[outIdx] = (highest + lowest) / 2.0;
                outIdx++;
                today++;
            }
        }

        public static RetCode MidPoint(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, ref int outBegIdx,
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

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int nbInitialElementNeeded = optInTimePeriod - 1;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = 0;
            int today = startIdx;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            while (true)
            {
                if (today > endIdx)
                {
                    outBegIdx = startIdx;
                    outNBElement = outIdx;
                    return RetCode.Success;
                }

                double lowest = inReal[trailingIdx];
                trailingIdx++;
                double highest = lowest;
                for (int i = trailingIdx; i <= today; i++)
                {
                    double tmp = inReal[i];
                    if (tmp < lowest)
                    {
                        lowest = tmp;
                    }
                    else if (tmp > highest)
                    {
                        highest = tmp;
                    }
                }

                outReal[outIdx] = (highest + lowest) / 2.0;
                outIdx++;
                today++;
            }
        }

        public static int MidPointLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return (optInTimePeriod - 1);
        }
    }
}
