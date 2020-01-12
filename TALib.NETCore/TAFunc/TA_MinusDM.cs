using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MinusDM(int startIdx, int endIdx, double[] inHigh, double[] inLow, int optInTimePeriod, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            double tempReal;
            int today;
            double diffM;
            double prevLow;
            double prevHigh;
            double diffP;
            int lookbackTotal;
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
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod > 1)
            {
                lookbackTotal = (optInTimePeriod + ((int) Globals.unstablePeriod[0x10])) - 1;
            }
            else
            {
                lookbackTotal = 1;
            }

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

            int outIdx = 0;
            if (optInTimePeriod <= 1)
            {
                outBegIdx = startIdx;
                today = startIdx - 1;
                prevHigh = inHigh[today];
                prevLow = inLow[today];
                while (today < endIdx)
                {
                    today++;
                    tempReal = inHigh[today];
                    diffP = tempReal - prevHigh;
                    prevHigh = tempReal;
                    tempReal = inLow[today];
                    diffM = prevLow - tempReal;
                    prevLow = tempReal;
                    if ((diffM > 0.0) && (diffP < diffM))
                    {
                        outReal[outIdx] = diffM;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                }

                outNBElement = outIdx;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            double prevMinusDM = 0.0;
            today = startIdx - lookbackTotal;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            int i = optInTimePeriod - 1;
            Label_0138:
            i--;
            if (i > 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM += diffM;
                }

                goto Label_0138;
            }

            i = (int) Globals.unstablePeriod[0x10];
            Label_0186:
            i--;
            if (i != 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM = (prevMinusDM - (prevMinusDM / ((double) optInTimePeriod))) + diffM;
                }
                else
                {
                    prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                }

                goto Label_0186;
            }

            outReal[0] = prevMinusDM;
            outIdx = 1;
            while (true)
            {
                if (today >= endIdx)
                {
                    break;
                }

                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM = (prevMinusDM - (prevMinusDM / ((double) optInTimePeriod))) + diffM;
                }
                else
                {
                    prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                }

                outReal[outIdx] = prevMinusDM;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode MinusDM(int startIdx, int endIdx, float[] inHigh, float[] inLow, int optInTimePeriod, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            double tempReal;
            int today;
            double diffM;
            double prevLow;
            double prevHigh;
            double diffP;
            int lookbackTotal;
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
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod > 1)
            {
                lookbackTotal = (optInTimePeriod + ((int) Globals.unstablePeriod[0x10])) - 1;
            }
            else
            {
                lookbackTotal = 1;
            }

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

            int outIdx = 0;
            if (optInTimePeriod <= 1)
            {
                outBegIdx = startIdx;
                today = startIdx - 1;
                prevHigh = inHigh[today];
                prevLow = inLow[today];
                while (today < endIdx)
                {
                    today++;
                    tempReal = inHigh[today];
                    diffP = tempReal - prevHigh;
                    prevHigh = tempReal;
                    tempReal = inLow[today];
                    diffM = prevLow - tempReal;
                    prevLow = tempReal;
                    if ((diffM > 0.0) && (diffP < diffM))
                    {
                        outReal[outIdx] = diffM;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                }

                outNBElement = outIdx;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            double prevMinusDM = 0.0;
            today = startIdx - lookbackTotal;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            int i = optInTimePeriod - 1;
            Label_0141:
            i--;
            if (i > 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM += diffM;
                }

                goto Label_0141;
            }

            i = (int) Globals.unstablePeriod[0x10];
            Label_0191:
            i--;
            if (i != 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM = (prevMinusDM - (prevMinusDM / ((double) optInTimePeriod))) + diffM;
                }
                else
                {
                    prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                }

                goto Label_0191;
            }

            outReal[0] = prevMinusDM;
            outIdx = 1;
            while (true)
            {
                if (today >= endIdx)
                {
                    break;
                }

                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM = (prevMinusDM - (prevMinusDM / ((double) optInTimePeriod))) + diffM;
                }
                else
                {
                    prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                }

                outReal[outIdx] = prevMinusDM;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int MinusDMLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            if (optInTimePeriod > 1)
            {
                return ((optInTimePeriod + ((int) Globals.unstablePeriod[0x10])) - 1);
            }

            return 1;
        }
    }
}
