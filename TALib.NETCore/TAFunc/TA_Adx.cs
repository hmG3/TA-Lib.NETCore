using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Adx(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            double tempReal;
            double tempReal2;
            double diffM;
            double diffP;
            double plusDI;
            double minusDI;
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

            int lookbackTotal = ((optInTimePeriod * 2) + ((int) Globals.unstablePeriod[0])) - 1;
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
            int today = startIdx;
            outBegIdx = today;
            double prevMinusDM = 0.0;
            double prevPlusDM = 0.0;
            double prevTR = 0.0;
            today = startIdx - lookbackTotal;
            double prevHigh = inHigh[today];
            double prevLow = inLow[today];
            double prevClose = inClose[today];
            int i = optInTimePeriod - 1;
            while (true)
            {
                i--;
                if (i <= 0)
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
                    prevMinusDM += diffM;
                }
                else if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
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

            double sumDX = 0.0;
            i = optInTimePeriod;
            while (true)
            {
                i--;
                if (i <= 0)
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
                prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM += diffM;
                }
                else if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
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
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if ((-1E-08 >= tempReal) || (tempReal >= 1E-08))
                    {
                        sumDX += 100.0 * (Math.Abs((double) (minusDI - plusDI)) / tempReal);
                    }
                }
            }

            double prevADX = sumDX / ((double) optInTimePeriod);
            i = (int) Globals.unstablePeriod[0];
            while (true)
            {
                i--;
                if (i <= 0)
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
                prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM += diffM;
                }
                else if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
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
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if ((-1E-08 >= tempReal) || (tempReal >= 1E-08))
                    {
                        tempReal = 100.0 * (Math.Abs((double) (minusDI - plusDI)) / tempReal);
                        prevADX = ((prevADX * (optInTimePeriod - 1)) + tempReal) / ((double) optInTimePeriod);
                    }
                }
            }

            outReal[0] = prevADX;
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
                prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM += diffM;
                }
                else if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
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
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if ((-1E-08 >= tempReal) || (tempReal >= 1E-08))
                    {
                        tempReal = 100.0 * (Math.Abs((double) (minusDI - plusDI)) / tempReal);
                        prevADX = ((prevADX * (optInTimePeriod - 1)) + tempReal) / ((double) optInTimePeriod);
                    }
                }

                outReal[outIdx] = prevADX;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode Adx(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            double tempReal;
            double tempReal2;
            double diffM;
            double diffP;
            double plusDI;
            double minusDI;
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

            int lookbackTotal = ((optInTimePeriod * 2) + ((int) Globals.unstablePeriod[0])) - 1;
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
            int today = startIdx;
            outBegIdx = today;
            double prevMinusDM = 0.0;
            double prevPlusDM = 0.0;
            double prevTR = 0.0;
            today = startIdx - lookbackTotal;
            double prevHigh = inHigh[today];
            double prevLow = inLow[today];
            double prevClose = inClose[today];
            int i = optInTimePeriod - 1;
            while (true)
            {
                i--;
                if (i <= 0)
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
                    prevMinusDM += diffM;
                }
                else if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
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

            double sumDX = 0.0;
            i = optInTimePeriod;
            while (true)
            {
                i--;
                if (i <= 0)
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
                prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM += diffM;
                }
                else if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
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
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if ((-1E-08 >= tempReal) || (tempReal >= 1E-08))
                    {
                        sumDX += 100.0 * (Math.Abs((double) (minusDI - plusDI)) / tempReal);
                    }
                }
            }

            double prevADX = sumDX / ((double) optInTimePeriod);
            i = (int) Globals.unstablePeriod[0];
            while (true)
            {
                i--;
                if (i <= 0)
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
                prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM += diffM;
                }
                else if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
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
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if ((-1E-08 >= tempReal) || (tempReal >= 1E-08))
                    {
                        tempReal = 100.0 * (Math.Abs((double) (minusDI - plusDI)) / tempReal);
                        prevADX = ((prevADX * (optInTimePeriod - 1)) + tempReal) / ((double) optInTimePeriod);
                    }
                }
            }

            outReal[0] = prevADX;
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
                prevMinusDM -= prevMinusDM / ((double) optInTimePeriod);
                prevPlusDM -= prevPlusDM / ((double) optInTimePeriod);
                if ((diffM > 0.0) && (diffP < diffM))
                {
                    prevMinusDM += diffM;
                }
                else if ((diffP > 0.0) && (diffP > diffM))
                {
                    prevPlusDM += diffP;
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
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if ((-1E-08 >= tempReal) || (tempReal >= 1E-08))
                    {
                        tempReal = 100.0 * (Math.Abs((double) (minusDI - plusDI)) / tempReal);
                        prevADX = ((prevADX * (optInTimePeriod - 1)) + tempReal) / ((double) optInTimePeriod);
                    }
                }

                outReal[outIdx] = prevADX;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int AdxLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return (((optInTimePeriod * 2) + ((int) Globals.unstablePeriod[0])) - 1);
        }
    }
}
