using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Dx(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal, int optInTimePeriod = 14)
        {
            double tempReal;
            double tempReal2;
            double diffM;
            double diffP;
            double plusDI;
            double minusDI;
            int lookbackTotal;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod > 1)
            {
                lookbackTotal = optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Dx];
            }
            else
            {
                lookbackTotal = 2;
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

            int today = startIdx;
            outBegIdx = today;
            double prevMinusDM = default;
            double prevPlusDM = default;
            double prevTR = default;
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
                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                tempReal = prevHigh - prevLow;
                tempReal2 = Math.Abs(prevHigh - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                tempReal2 = Math.Abs(prevLow - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                prevTR += tempReal;
                prevClose = inClose[today];
            }

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.Dx] + 1;
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
                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;
                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                tempReal = prevHigh - prevLow;
                tempReal2 = Math.Abs(prevHigh - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                tempReal2 = Math.Abs(prevLow - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
            }

            if (-1E-08 >= prevTR || prevTR >= 1E-08)
            {
                minusDI = 100.0 * (prevMinusDM / prevTR);
                plusDI = 100.0 * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (-1E-08 >= tempReal || tempReal >= 1E-08)
                {
                    outReal[0] = 100.0 * (Math.Abs(minusDI - plusDI) / tempReal);
                }
                else
                {
                    outReal[0] = 0.0;
                }
            }
            else
            {
                outReal[0] = 0.0;
            }

            var outIdx = 1;
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
                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;
                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                tempReal = prevHigh - prevLow;
                tempReal2 = Math.Abs(prevHigh - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                tempReal2 = Math.Abs(prevLow - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
                if (-1E-08 >= prevTR || prevTR >= 1E-08)
                {
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if (-1E-08 >= tempReal || tempReal >= 1E-08)
                    {
                        outReal[outIdx] = 100.0 * (Math.Abs(minusDI - plusDI) / tempReal);
                    }
                    else
                    {
                        outReal[outIdx] = outReal[outIdx - 1];
                    }
                }
                else
                {
                    outReal[outIdx] = outReal[outIdx - 1];
                }

                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode Dx(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, ref int outBegIdx,
            ref int outNBElement, decimal[] outReal, int optInTimePeriod = 14)
        {
            decimal tempReal;
            decimal tempReal2;
            decimal diffM;
            decimal diffP;
            decimal plusDI;
            decimal minusDI;
            int lookbackTotal;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod > 1)
            {
                lookbackTotal = optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Dx];
            }
            else
            {
                lookbackTotal = 2;
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

            int today = startIdx;
            outBegIdx = today;
            decimal prevMinusDM = default;
            decimal prevPlusDM = default;
            decimal prevTR = default;
            today = startIdx - lookbackTotal;
            decimal prevHigh = inHigh[today];
            decimal prevLow = inLow[today];
            decimal prevClose = inClose[today];
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
                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                tempReal = prevHigh - prevLow;
                tempReal2 = Math.Abs(prevHigh - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                tempReal2 = Math.Abs(prevLow - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                prevTR += tempReal;
                prevClose = inClose[today];
            }

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.Dx] + 1;
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
                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;
                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                tempReal = prevHigh - prevLow;
                tempReal2 = Math.Abs(prevHigh - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                tempReal2 = Math.Abs(prevLow - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
            }

            if (-1E-08m >= prevTR || prevTR >= 1E-08m)
            {
                minusDI = 100m * (prevMinusDM / prevTR);
                plusDI = 100m * (prevPlusDM / prevTR);
                tempReal = minusDI + plusDI;
                if (-1E-08m >= tempReal || tempReal >= 1E-08m)
                {
                    outReal[0] = 100m * (Math.Abs(minusDI - plusDI) / tempReal);
                }
                else
                {
                    outReal[0] = Decimal.Zero;
                }
            }
            else
            {
                outReal[0] = Decimal.Zero;
            }

            var outIdx = 1;
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
                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;
                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                tempReal = prevHigh - prevLow;
                tempReal2 = Math.Abs(prevHigh - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                tempReal2 = Math.Abs(prevLow - prevClose);
                if (tempReal2 > tempReal)
                {
                    tempReal = tempReal2;
                }

                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
                if (-1E-08m >= prevTR || prevTR >= 1E-08m)
                {
                    minusDI = 100m * (prevMinusDM / prevTR);
                    plusDI = 100m * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if (-1E-08m >= tempReal || tempReal >= 1E-08m)
                    {
                        outReal[outIdx] = 100 * (Math.Abs(minusDI - plusDI) / tempReal);
                    }
                    else
                    {
                        outReal[outIdx] = outReal[outIdx - 1];
                    }
                }
                else
                {
                    outReal[outIdx] = outReal[outIdx - 1];
                }

                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int DxLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod > 1 ? optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Dx] : 2;
        }
    }
}
