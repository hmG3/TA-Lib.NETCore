using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MinusDI(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            double tempReal;
            int today;
            double tempReal2;
            double prevLow;
            double prevHigh;
            double diffM;
            double prevClose;
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

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
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
                lookbackTotal = optInTimePeriod + ((int) Globals.unstablePeriod[15]);
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
            if (optInTimePeriod > 1)
            {
                today = startIdx;
                outBegIdx = today;
                double prevMinusDM = 0.0;
                double prevTR = 0.0;
                today = startIdx - lookbackTotal;
                prevHigh = inHigh[today];
                prevLow = inLow[today];
                prevClose = inClose[today];
                int i = optInTimePeriod - 1;
                while (true)
                {
                    i--;
                    if (i <= 0)
                    {
                        i = ((int) Globals.unstablePeriod[15]) + 1;
                        while (true)
                        {
                            i--;
                            if (i == 0)
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

                            tempReal = prevHigh - prevLow;
                            tempReal2 = Math.Abs((double) (prevHigh - prevClose));
                            if (tempReal2 > tempReal)
                            {
                                tempReal = tempReal2;
                            }

                            tempReal2 = Math.Abs((double) (prevLow - prevClose));
                            if (tempReal2 > tempReal)
                            {
                                tempReal = tempReal2;
                            }

                            prevTR = (prevTR - (prevTR / ((double) optInTimePeriod))) + tempReal;
                            prevClose = inClose[today];
                        }

                        if ((-1E-08 >= prevTR) || (prevTR >= 1E-08))
                        {
                            outReal[0] = 100.0 * (prevMinusDM / prevTR);
                        }
                        else
                        {
                            outReal[0] = 0.0;
                        }

                        outIdx = 1;
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
                                prevMinusDM = (prevMinusDM - (prevMinusDM / ((double) optInTimePeriod))) + diffM;
                            }
                            else
                            {
                                prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                            }

                            tempReal = prevHigh - prevLow;
                            tempReal2 = Math.Abs((double) (prevHigh - prevClose));
                            if (tempReal2 > tempReal)
                            {
                                tempReal = tempReal2;
                            }

                            tempReal2 = Math.Abs((double) (prevLow - prevClose));
                            if (tempReal2 > tempReal)
                            {
                                tempReal = tempReal2;
                            }

                            prevTR = (prevTR - (prevTR / ((double) optInTimePeriod))) + tempReal;
                            prevClose = inClose[today];
                            if ((-1E-08 >= prevTR) || (prevTR >= 1E-08))
                            {
                                outReal[outIdx] = 100.0 * (prevMinusDM / prevTR);
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

                    tempReal = prevHigh - prevLow;
                    tempReal2 = Math.Abs((double) (prevHigh - prevClose));
                    if (tempReal2 > tempReal)
                    {
                        tempReal = tempReal2;
                    }

                    tempReal2 = Math.Abs((double) (prevLow - prevClose));
                    if (tempReal2 > tempReal)
                    {
                        tempReal = tempReal2;
                    }

                    prevTR += tempReal;
                    prevClose = inClose[today];
                }
            }

            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            prevClose = inClose[today];
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
                    tempReal = prevHigh - prevLow;
                    tempReal2 = Math.Abs((double) (prevHigh - prevClose));
                    if (tempReal2 > tempReal)
                    {
                        tempReal = tempReal2;
                    }

                    tempReal2 = Math.Abs((double) (prevLow - prevClose));
                    if (tempReal2 > tempReal)
                    {
                        tempReal = tempReal2;
                    }

                    if ((-1E-08 < tempReal) && (tempReal < 1E-08))
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = diffM / tempReal;
                        outIdx++;
                    }
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                prevClose = inClose[today];
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode MinusDI(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            double tempReal;
            int today;
            double tempReal2;
            double prevLow;
            double prevHigh;
            double diffM;
            double prevClose;
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

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
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
                lookbackTotal = optInTimePeriod + ((int) Globals.unstablePeriod[15]);
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
            if (optInTimePeriod > 1)
            {
                today = startIdx;
                outBegIdx = today;
                double prevMinusDM = 0.0;
                double prevTR = 0.0;
                today = startIdx - lookbackTotal;
                prevHigh = inHigh[today];
                prevLow = inLow[today];
                prevClose = inClose[today];
                int i = optInTimePeriod - 1;
                while (true)
                {
                    i--;
                    if (i <= 0)
                    {
                        i = ((int) Globals.unstablePeriod[15]) + 1;
                        while (true)
                        {
                            i--;
                            if (i == 0)
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

                            tempReal = prevHigh - prevLow;
                            tempReal2 = Math.Abs((double) (prevHigh - prevClose));
                            if (tempReal2 > tempReal)
                            {
                                tempReal = tempReal2;
                            }

                            tempReal2 = Math.Abs((double) (prevLow - prevClose));
                            if (tempReal2 > tempReal)
                            {
                                tempReal = tempReal2;
                            }

                            prevTR = (prevTR - (prevTR / ((double) optInTimePeriod))) + tempReal;
                            prevClose = inClose[today];
                        }

                        if ((-1E-08 >= prevTR) || (prevTR >= 1E-08))
                        {
                            outReal[0] = 100.0 * (prevMinusDM / prevTR);
                        }
                        else
                        {
                            outReal[0] = 0.0;
                        }

                        outIdx = 1;
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
                                prevMinusDM = (prevMinusDM - (prevMinusDM / ((double) optInTimePeriod))) + diffM;
                            }
                            else
                            {
                                prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                            }

                            tempReal = prevHigh - prevLow;
                            tempReal2 = Math.Abs((double) (prevHigh - prevClose));
                            if (tempReal2 > tempReal)
                            {
                                tempReal = tempReal2;
                            }

                            tempReal2 = Math.Abs((double) (prevLow - prevClose));
                            if (tempReal2 > tempReal)
                            {
                                tempReal = tempReal2;
                            }

                            prevTR = (prevTR - (prevTR / ((double) optInTimePeriod))) + tempReal;
                            prevClose = inClose[today];
                            if ((-1E-08 >= prevTR) || (prevTR >= 1E-08))
                            {
                                outReal[outIdx] = 100.0 * (prevMinusDM / prevTR);
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

                    tempReal = prevHigh - prevLow;
                    tempReal2 = Math.Abs((double) (prevHigh - prevClose));
                    if (tempReal2 > tempReal)
                    {
                        tempReal = tempReal2;
                    }

                    tempReal2 = Math.Abs((double) (prevLow - prevClose));
                    if (tempReal2 > tempReal)
                    {
                        tempReal = tempReal2;
                    }

                    prevTR += tempReal;
                    prevClose = inClose[today];
                }
            }

            outBegIdx = startIdx;
            today = startIdx - 1;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            prevClose = inClose[today];
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
                    tempReal = prevHigh - prevLow;
                    tempReal2 = Math.Abs((double) (prevHigh - prevClose));
                    if (tempReal2 > tempReal)
                    {
                        tempReal = tempReal2;
                    }

                    tempReal2 = Math.Abs((double) (prevLow - prevClose));
                    if (tempReal2 > tempReal)
                    {
                        tempReal = tempReal2;
                    }

                    if ((-1E-08 < tempReal) && (tempReal < 1E-08))
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = diffM / tempReal;
                        outIdx++;
                    }
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                prevClose = inClose[today];
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int MinusDILookback(int optInTimePeriod)
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
                return (optInTimePeriod + ((int) Globals.unstablePeriod[15]));
            }

            return 1;
        }
    }
}
