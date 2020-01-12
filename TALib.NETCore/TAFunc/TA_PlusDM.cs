using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode PlusDM(int startIdx, int endIdx, double[] inHigh, double[] inLow, int optInTimePeriod, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            double tempReal;
            int today;
            double diffP;
            double prevLow;
            double prevHigh;
            double diffM;
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
                lookbackTotal = (optInTimePeriod + ((int) Globals.unstablePeriod[0x13])) - 1;
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
                    if ((diffP > 0.0) && (diffP > diffM))
                    {
                        outReal[outIdx] = diffP;
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
            double prevPlusDM = 0.0;
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
                if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
                }

                goto Label_0138;
            }

            i = (int) Globals.unstablePeriod[0x13];
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
                if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM = (prevPlusDM - (prevPlusDM / ((double) optInTimePeriod))) + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                }

                goto Label_0186;
            }

            outReal[0] = prevPlusDM;
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
                if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM = (prevPlusDM - (prevPlusDM / ((double) optInTimePeriod))) + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                }

                outReal[outIdx] = prevPlusDM;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode PlusDM(int startIdx, int endIdx, float[] inHigh, float[] inLow, int optInTimePeriod, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            double tempReal;
            int today;
            double diffP;
            double prevLow;
            double prevHigh;
            double diffM;
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
                lookbackTotal = (optInTimePeriod + ((int) Globals.unstablePeriod[0x13])) - 1;
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
                    if ((diffP > 0.0) && (diffP > diffM))
                    {
                        outReal[outIdx] = diffP;
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
            double prevPlusDM = 0.0;
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
                if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
                }

                goto Label_0141;
            }

            i = (int) Globals.unstablePeriod[0x13];
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
                if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM = (prevPlusDM - (prevPlusDM / ((double) optInTimePeriod))) + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                }

                goto Label_0191;
            }

            outReal[0] = prevPlusDM;
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
                if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM = (prevPlusDM - (prevPlusDM / ((double) optInTimePeriod))) + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                }

                outReal[outIdx] = prevPlusDM;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int PlusDMLookback(int optInTimePeriod)
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
                return ((optInTimePeriod + ((int) Globals.unstablePeriod[0x13])) - 1);
            }

            return 1;
        }
    }
}
