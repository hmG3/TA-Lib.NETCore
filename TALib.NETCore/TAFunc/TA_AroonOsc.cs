using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode AroonOsc(int startIdx, int endIdx, double[] inHigh, double[] inLow, int optInTimePeriod, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            int i;
            double aroon;
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

            int outIdx = 0;
            int today = startIdx;
            int trailingIdx = startIdx - optInTimePeriod;
            int lowestIdx = -1;
            int highestIdx = -1;
            double lowest = 0.0;
            double highest = 0.0;
            double factor = 100.0 / ((double) optInTimePeriod);
            Label_00AF:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            double tmp = inLow[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inLow[lowestIdx];
                i = lowestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_00F3;
                    }

                    tmp = inLow[i];
                    if (tmp <= lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }
            }

            if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
            }

            Label_00F3:
            tmp = inHigh[today];
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inHigh[highestIdx];
                i = highestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_012A;
                    }

                    tmp = inHigh[i];
                    if (tmp >= highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }
            }

            if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
            }

            Label_012A:
            aroon = factor * (highestIdx - lowestIdx);
            outReal[outIdx] = aroon;
            outIdx++;
            trailingIdx++;
            today++;
            goto Label_00AF;
        }

        public static RetCode AroonOsc(int startIdx, int endIdx, float[] inHigh, float[] inLow, int optInTimePeriod, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            int i;
            double aroon;
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

            int outIdx = 0;
            int today = startIdx;
            int trailingIdx = startIdx - optInTimePeriod;
            int lowestIdx = -1;
            int highestIdx = -1;
            double lowest = 0.0;
            double highest = 0.0;
            double factor = 100.0 / ((double) optInTimePeriod);
            Label_00AF:
            if (today > endIdx)
            {
                outBegIdx = startIdx;
                outNBElement = outIdx;
                return RetCode.Success;
            }

            double tmp = inLow[today];
            if (lowestIdx < trailingIdx)
            {
                lowestIdx = trailingIdx;
                lowest = inLow[lowestIdx];
                i = lowestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_00F6;
                    }

                    tmp = inLow[i];
                    if (tmp <= lowest)
                    {
                        lowestIdx = i;
                        lowest = tmp;
                    }
                }
            }

            if (tmp <= lowest)
            {
                lowestIdx = today;
                lowest = tmp;
            }

            Label_00F6:
            tmp = inHigh[today];
            if (highestIdx < trailingIdx)
            {
                highestIdx = trailingIdx;
                highest = inHigh[highestIdx];
                i = highestIdx;
                while (true)
                {
                    i++;
                    if (i > today)
                    {
                        goto Label_0130;
                    }

                    tmp = inHigh[i];
                    if (tmp >= highest)
                    {
                        highestIdx = i;
                        highest = tmp;
                    }
                }
            }

            if (tmp >= highest)
            {
                highestIdx = today;
                highest = tmp;
            }

            Label_0130:
            aroon = factor * (highestIdx - lowestIdx);
            outReal[outIdx] = aroon;
            outIdx++;
            trailingIdx++;
            today++;
            goto Label_00AF;
        }

        public static int AroonOscLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return optInTimePeriod;
        }
    }
}
