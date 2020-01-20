using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode T3(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 5, double optInVFactor = 0.7)
        {
            int i;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000 || optInVFactor < 0.0 || optInVFactor > 1.0)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = (optInTimePeriod - 1) * 6 + (int) Globals.UnstablePeriod[(int) FuncUnstId.T3];
            if (startIdx <= lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int today = startIdx - lookbackTotal;
            double k = 2.0 / (optInTimePeriod + 1.0);
            double oneMinusK = 1.0 - k;
            double tempReal = inReal[today];
            today++;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                tempReal += inReal[today];
                today++;
            }

            double e1 = tempReal / optInTimePeriod;
            tempReal = e1;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                tempReal += e1;
            }

            double e2 = tempReal / optInTimePeriod;
            tempReal = e2;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                tempReal += e2;
            }

            double e3 = tempReal / optInTimePeriod;
            tempReal = e3;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                tempReal += e3;
            }

            double e4 = tempReal / optInTimePeriod;
            tempReal = e4;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                e4 = k * e3 + oneMinusK * e4;
                tempReal += e4;
            }

            double e5 = tempReal / optInTimePeriod;
            tempReal = e5;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                e4 = k * e3 + oneMinusK * e4;
                e5 = k * e4 + oneMinusK * e5;
                tempReal += e5;
            }

            double e6 = tempReal / optInTimePeriod;
            while (true)
            {
                if (today > startIdx)
                {
                    break;
                }

                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                e4 = k * e3 + oneMinusK * e4;
                e5 = k * e4 + oneMinusK * e5;
                e6 = k * e5 + oneMinusK * e6;
            }

            tempReal = optInVFactor * optInVFactor;
            double c1 = -(tempReal * optInVFactor);
            double c2 = 3.0 * (tempReal - c1);
            double c3 = -6.0 * tempReal - 3.0 * (optInVFactor - c1);
            double c4 = 1.0 + 3.0 * optInVFactor - c1 + 3.0 * tempReal;
            int outIdx = default;
            outReal[outIdx] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;
            outIdx++;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                e4 = k * e3 + oneMinusK * e4;
                e5 = k * e4 + oneMinusK * e5;
                e6 = k * e5 + oneMinusK * e6;
                outReal[outIdx] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode T3(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 5, decimal optInVFactor = 0.7m)
        {
            int i;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000 || optInVFactor < Decimal.Zero || optInVFactor > Decimal.One)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = (optInTimePeriod - 1) * 6 + (int) Globals.UnstablePeriod[(int) FuncUnstId.T3];
            if (startIdx <= lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int today = startIdx - lookbackTotal;
            decimal k = 2m / (optInTimePeriod + Decimal.One);
            decimal oneMinusK = Decimal.One - k;
            decimal tempReal = inReal[today];
            today++;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                tempReal += inReal[today];
                today++;
            }

            decimal e1 = tempReal / optInTimePeriod;
            tempReal = e1;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                tempReal += e1;
            }

            decimal e2 = tempReal / optInTimePeriod;
            tempReal = e2;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                tempReal += e2;
            }

            decimal e3 = tempReal / optInTimePeriod;
            tempReal = e3;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                tempReal += e3;
            }

            decimal e4 = tempReal / optInTimePeriod;
            tempReal = e4;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                e4 = k * e3 + oneMinusK * e4;
                tempReal += e4;
            }

            decimal e5 = tempReal / optInTimePeriod;
            tempReal = e5;
            for (i = optInTimePeriod - 1; i > 0; i--)
            {
                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                e4 = k * e3 + oneMinusK * e4;
                e5 = k * e4 + oneMinusK * e5;
                tempReal += e5;
            }

            decimal e6 = tempReal / optInTimePeriod;
            while (true)
            {
                if (today > startIdx)
                {
                    break;
                }

                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                e4 = k * e3 + oneMinusK * e4;
                e5 = k * e4 + oneMinusK * e5;
                e6 = k * e5 + oneMinusK * e6;
            }

            tempReal = optInVFactor * optInVFactor;
            decimal c1 = -(tempReal * optInVFactor);
            decimal c2 = 3m * (tempReal - c1);
            decimal c3 = -6m * tempReal - 3m * (optInVFactor - c1);
            decimal c4 = Decimal.One + 3m * optInVFactor - c1 + 3m * tempReal;
            int outIdx = default;
            outReal[outIdx] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;
            outIdx++;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                e1 = k * inReal[today] + oneMinusK * e1;
                today++;
                e2 = k * e1 + oneMinusK * e2;
                e3 = k * e2 + oneMinusK * e3;
                e4 = k * e3 + oneMinusK * e4;
                e5 = k * e4 + oneMinusK * e5;
                e6 = k * e5 + oneMinusK * e6;
                outReal[outIdx] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int T3Lookback(int optInTimePeriod = 5)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }


            return (optInTimePeriod - 1) * 6 + (int) Globals.UnstablePeriod[(int) FuncUnstId.T3];
        }
    }
}
