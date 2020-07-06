using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode MinusDM(double[] inHigh, double[] inLow, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MinusDMLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int today;
            double diffM;
            double prevLow;
            double prevHigh;
            double diffP;
            int outIdx = default;
            if (optInTimePeriod == 1)
            {
                outBegIdx = startIdx;
                today = startIdx - 1;
                prevHigh = inHigh[today];
                prevLow = inLow[today];
                while (today < endIdx)
                {
                    today++;
                    double tempReal = inHigh[today];
                    diffP = tempReal - prevHigh;
                    prevHigh = tempReal;
                    tempReal = inLow[today];
                    diffM = prevLow - tempReal;
                    prevLow = tempReal;
                    outReal[outIdx++] = diffM > 0.0 && diffP < diffM ? diffM : 0.0;
                }

                outNbElement = outIdx;

                return RetCode.Success;
            }

            outBegIdx = startIdx;

            double prevMinusDM = default;
            today = startIdx - lookbackTotal;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            int i = optInTimePeriod - 1;
            while (i-- > 0)
            {
                today++;
                double tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
            }

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.MinusDM];
            while (i-- != 0)
            {
                today++;
                double tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM = prevMinusDM - prevMinusDM / optInTimePeriod + diffM;
                }
                else
                {
                    prevMinusDM -= prevMinusDM / optInTimePeriod;
                }
            }

            outReal[0] = prevMinusDM;
            outIdx = 1;

            while (today < endIdx)
            {
                today++;
                double tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM = prevMinusDM - prevMinusDM / optInTimePeriod + diffM;
                }
                else
                {
                    prevMinusDM -= prevMinusDM / optInTimePeriod;
                }

                outReal[outIdx++] = prevMinusDM;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode MinusDM(decimal[] inHigh, decimal[] inLow, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MinusDMLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int today;
            decimal diffM;
            decimal prevLow;
            decimal prevHigh;
            decimal diffP;
            int outIdx = default;
            if (optInTimePeriod == 1)
            {
                outBegIdx = startIdx;
                today = startIdx - 1;
                prevHigh = inHigh[today];
                prevLow = inLow[today];
                while (today < endIdx)
                {
                    today++;
                    decimal tempReal = inHigh[today];
                    diffP = tempReal - prevHigh;
                    prevHigh = tempReal;
                    tempReal = inLow[today];
                    diffM = prevLow - tempReal;
                    prevLow = tempReal;
                    outReal[outIdx++] = diffM > Decimal.Zero && diffP < diffM ? diffM : Decimal.Zero;
                }

                outNbElement = outIdx;

                return RetCode.Success;
            }

            outBegIdx = startIdx;

            decimal prevMinusDM = default;
            today = startIdx - lookbackTotal;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            int i = optInTimePeriod - 1;
            while (i-- > 0)
            {
                today++;
                decimal tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
            }

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.MinusDM];
            while (i-- != 0)
            {
                today++;
                decimal tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM = prevMinusDM - prevMinusDM / optInTimePeriod + diffM;
                }
                else
                {
                    prevMinusDM -= prevMinusDM / optInTimePeriod;
                }
            }

            outReal[0] = prevMinusDM;
            outIdx = 1;

            while (today < endIdx)
            {
                today++;
                decimal tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM = prevMinusDM - prevMinusDM / optInTimePeriod + diffM;
                }
                else
                {
                    prevMinusDM -= prevMinusDM / optInTimePeriod;
                }

                outReal[outIdx++] = prevMinusDM;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int MinusDMLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod > 1 ? optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.MinusDM] - 1 : 1;
        }
    }
}
