using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MidPrice(int startIdx, int endIdx, double[] inHigh, double[] inLow, int optInTimePeriod, ref int outBegIdx,
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

            if ((inHigh == null) || (inLow == null))
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

                double lowest = inLow[trailingIdx];
                double highest = inHigh[trailingIdx];
                trailingIdx++;
                for (int i = trailingIdx; i <= today; i++)
                {
                    double tmp = inLow[i];
                    if (tmp < lowest)
                    {
                        lowest = tmp;
                    }

                    tmp = inHigh[i];
                    if (tmp > highest)
                    {
                        highest = tmp;
                    }
                }

                outReal[outIdx] = (highest + lowest) / 2.0;
                outIdx++;
                today++;
            }
        }

        public static RetCode MidPrice(int startIdx, int endIdx, float[] inHigh, float[] inLow, int optInTimePeriod, ref int outBegIdx,
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

            if ((inHigh == null) || (inLow == null))
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

                double lowest = inLow[trailingIdx];
                double highest = inHigh[trailingIdx];
                trailingIdx++;
                for (int i = trailingIdx; i <= today; i++)
                {
                    double tmp = inLow[i];
                    if (tmp < lowest)
                    {
                        lowest = tmp;
                    }

                    tmp = inHigh[i];
                    if (tmp > highest)
                    {
                        highest = tmp;
                    }
                }

                outReal[outIdx] = (highest + lowest) / 2.0;
                outIdx++;
                today++;
            }
        }

        public static int MidPriceLookback(int optInTimePeriod)
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
