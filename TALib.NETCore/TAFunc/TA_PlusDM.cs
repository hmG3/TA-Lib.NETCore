using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode PlusDM(int startIdx, int endIdx, double[] inHigh, double[] inLow, ref int outBegIdx, ref int outNBElement,
            double[] outReal, int optInTimePeriod = 14)
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

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod > 1)
            {
                lookbackTotal = optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.PlusDM] - 1;
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

            int outIdx = default;
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
                    if (diffP > 0.0 && diffP > diffM)
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
            double prevPlusDM = default;
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
                if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                goto Label_0138;
            }

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.PlusDM];
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
                if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM = prevPlusDM - prevPlusDM / optInTimePeriod + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / optInTimePeriod;
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
                if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM = prevPlusDM - prevPlusDM / optInTimePeriod + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / optInTimePeriod;
                }

                outReal[outIdx] = prevPlusDM;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode PlusDM(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, int optInTimePeriod = 14)
        {
            decimal tempReal;
            int today;
            decimal diffP;
            decimal prevLow;
            decimal prevHigh;
            decimal diffM;
            int lookbackTotal;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod > 1)
            {
                lookbackTotal = optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.PlusDM] - 1;
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

            int outIdx = default;
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
                    if (diffP > Decimal.Zero && diffP > diffM)
                    {
                        outReal[outIdx] = diffP;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = Decimal.Zero;
                        outIdx++;
                    }
                }

                outNBElement = outIdx;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            decimal prevPlusDM = default;
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
                if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                goto Label_0141;
            }

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.PlusDM];
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
                if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM = prevPlusDM - prevPlusDM / optInTimePeriod + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / optInTimePeriod;
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
                if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM = prevPlusDM - prevPlusDM / optInTimePeriod + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / optInTimePeriod;
                }

                outReal[outIdx] = prevPlusDM;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int PlusDMLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod > 1 ? optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.PlusDM] - 1 : 1;
        }
    }
}
