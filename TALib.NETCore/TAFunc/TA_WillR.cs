using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode WillR(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
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

            double diff = 0.0;
            int outIdx = 0;
            int today = startIdx;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            int highestIdx = -1;
            int lowestIdx = highestIdx;
            double lowest = 0.0;
            double highest = lowest;
            diff = highest;
            Label_00B1:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            double tmp = inLow[today];
            if (lowestIdx >= trailingIdx)
            {
                if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                    diff = (highest - lowest) / -100.0;
                }

                goto Label_0112;
            }

            lowestIdx = trailingIdx;
            lowest = inLow[lowestIdx];
            int i = lowestIdx;
            Label_00D0:
            i++;
            if (i <= today)
            {
                tmp = inLow[i];
                if (tmp < lowest)
                {
                    lowestIdx = i;
                    lowest = tmp;
                }

                goto Label_00D0;
            }

            diff = (highest - lowest) / -100.0;
            Label_0112:
            tmp = inHigh[today];
            if (highestIdx >= trailingIdx)
            {
                if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / -100.0;
                }

                goto Label_016B;
            }

            highestIdx = trailingIdx;
            highest = inHigh[highestIdx];
            i = highestIdx;
            Label_0129:
            i++;
            if (i <= today)
            {
                tmp = inHigh[i];
                if (tmp > highest)
                {
                    highestIdx = i;
                    highest = tmp;
                }

                goto Label_0129;
            }

            diff = (highest - lowest) / -100.0;
            Label_016B:
            if (diff != 0.0)
            {
                outReal[outIdx] = (highest - inClose[today]) / diff;
                outIdx++;
            }
            else
            {
                outReal[outIdx] = 0.0;
                outIdx++;
            }

            trailingIdx++;
            today++;
            goto Label_00B1;
        }

        public static RetCode WillR(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
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

            double diff = 0.0;
            int outIdx = 0;
            int today = startIdx;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            int highestIdx = -1;
            int lowestIdx = highestIdx;
            double lowest = 0.0;
            double highest = lowest;
            diff = highest;
            Label_00B1:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            double tmp = inLow[today];
            if (lowestIdx >= trailingIdx)
            {
                if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                    diff = (highest - lowest) / -100.0;
                }

                goto Label_0115;
            }

            lowestIdx = trailingIdx;
            lowest = inLow[lowestIdx];
            int i = lowestIdx;
            Label_00D2:
            i++;
            if (i <= today)
            {
                tmp = inLow[i];
                if (tmp < lowest)
                {
                    lowestIdx = i;
                    lowest = tmp;
                }

                goto Label_00D2;
            }

            diff = (highest - lowest) / -100.0;
            Label_0115:
            tmp = inHigh[today];
            if (highestIdx >= trailingIdx)
            {
                if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / -100.0;
                }

                goto Label_0171;
            }

            highestIdx = trailingIdx;
            highest = inHigh[highestIdx];
            i = highestIdx;
            Label_012E:
            i++;
            if (i <= today)
            {
                tmp = inHigh[i];
                if (tmp > highest)
                {
                    highestIdx = i;
                    highest = tmp;
                }

                goto Label_012E;
            }

            diff = (highest - lowest) / -100.0;
            Label_0171:
            if (diff != 0.0)
            {
                outReal[outIdx] = (highest - inClose[today]) / diff;
                outIdx++;
            }
            else
            {
                outReal[outIdx] = 0.0;
                outIdx++;
            }

            trailingIdx++;
            today++;
            goto Label_00B1;
        }

        public static int WillRLookback(int optInTimePeriod)
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
