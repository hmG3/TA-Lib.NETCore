using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Sar(int startIdx, int endIdx, double[] inHigh, double[] inLow, ref int outBegIdx, ref int outNBElement,
            double[] outReal, double optInAcceleration = 0.02, double optInMaximum = 0.2)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInAcceleration < 0.0 || optInMaximum < 0.0)
            {
                return RetCode.BadParam;
            }

            if (startIdx < 1)
            {
                startIdx = 1;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double af = optInAcceleration;
            if (af > optInMaximum)
            {
                af = optInAcceleration = optInMaximum;
            }

            int _ = default;
            var epTemp = new double[1];
            RetCode retCode = MinusDM(startIdx, startIdx, inHigh, inLow, ref _, ref _, epTemp, 1);
            var isLong = epTemp[0] <= 0.0;

            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            outBegIdx = startIdx;
            double sar;
            double ep;
            int outIdx = default;

            int todayIdx = startIdx;

            double newHigh = inHigh[--todayIdx];
            double newLow = inLow[--todayIdx];
            if (isLong)
            {
                ep = inHigh[todayIdx];
                sar = newLow;
            }
            else
            {
                ep = inLow[todayIdx];
                sar = newHigh;
            }

            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];

            while (todayIdx <= endIdx)
            {
                double prevLow = newLow;
                double prevHigh = newHigh;
                newLow = inLow[todayIdx];
                newHigh = inHigh[todayIdx];
                todayIdx++;

                if (isLong)
                {
                    if (newLow <= sar)
                    {
                        isLong = false;
                        sar = ep;

                        if (sar < prevHigh)
                        {
                            sar = prevHigh;
                        }

                        if (sar < newHigh)
                        {
                            sar = newHigh;
                        }

                        outReal[outIdx++] = sar;

                        af = optInAcceleration;
                        ep = newLow;

                        sar += af * (ep - sar);
                        if (sar < prevHigh)
                        {
                            sar = prevHigh;
                        }

                        if (sar < newHigh)
                        {
                            sar = newHigh;
                        }
                    }
                    else
                    {
                        outReal[outIdx++] = sar;
                        if (newHigh > ep)
                        {
                            ep = newHigh;
                            af += optInAcceleration;
                            if (af > optInMaximum)
                            {
                                af = optInMaximum;
                            }
                        }

                        sar += af * (ep - sar);
                        if (sar > prevLow)
                        {
                            sar = prevLow;
                        }

                        if (sar > newLow)
                        {
                            sar = newLow;
                        }
                    }
                }
                else if (newHigh >= sar)
                {
                    isLong = true;
                    sar = ep;

                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }
                    if (sar > newLow)
                    {
                        sar = newLow;
                    }

                    outReal[outIdx++] = sar;

                    af = optInAcceleration;
                    ep = newHigh;

                    sar += af * (ep - sar);
                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }
                    if (sar > newLow)
                    {
                        sar = newLow;
                    }
                }
                else
                {
                    outReal[outIdx++] = sar;

                    if (newLow < ep)
                    {
                        ep = newLow;
                        af += optInAcceleration;
                        if (af > optInMaximum)
                        {
                            af = optInMaximum;
                        }
                    }

                    sar += af * (ep - sar);

                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }
                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }
                }
            }

            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Sar(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, decimal optInAcceleration = 0.02m, decimal optInMaximum = 0.2m)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInAcceleration < Decimal.Zero || optInMaximum < Decimal.Zero)
            {
                return RetCode.BadParam;
            }

            if (startIdx < 1)
            {
                startIdx = 1;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            decimal af = optInAcceleration;
            if (af > optInMaximum)
            {
                af = optInAcceleration = optInMaximum;
            }

            int _ = default;
            var epTemp = new decimal[1];
            RetCode retCode = MinusDM(startIdx, startIdx, inHigh, inLow, ref _, ref _, epTemp, 1);
            var isLong = epTemp[0] <= Decimal.Zero;

            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            outBegIdx = startIdx;
            decimal sar;
            decimal ep;
            int outIdx = default;

            int todayIdx = startIdx;

            decimal newHigh = inHigh[--todayIdx];
            decimal newLow = inLow[--todayIdx];
            if (isLong)
            {
                ep = inHigh[todayIdx];
                sar = newLow;
            }
            else
            {
                ep = inLow[todayIdx];
                sar = newHigh;
            }

            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];

            while (todayIdx <= endIdx)
            {
                decimal prevLow = newLow;
                decimal prevHigh = newHigh;
                newLow = inLow[todayIdx];
                newHigh = inHigh[todayIdx];
                todayIdx++;

                if (isLong)
                {
                    if (newLow <= sar)
                    {
                        isLong = false;
                        sar = ep;

                        if (sar < prevHigh)
                        {
                            sar = prevHigh;
                        }

                        if (sar < newHigh)
                        {
                            sar = newHigh;
                        }

                        outReal[outIdx++] = sar;

                        af = optInAcceleration;
                        ep = newLow;

                        sar += af * (ep - sar);
                        if (sar < prevHigh)
                        {
                            sar = prevHigh;
                        }

                        if (sar < newHigh)
                        {
                            sar = newHigh;
                        }
                    }
                    else
                    {
                        outReal[outIdx++] = sar;
                        if (newHigh > ep)
                        {
                            ep = newHigh;
                            af += optInAcceleration;
                            if (af > optInMaximum)
                            {
                                af = optInMaximum;
                            }
                        }

                        sar += af * (ep - sar);
                        if (sar > prevLow)
                        {
                            sar = prevLow;
                        }

                        if (sar > newLow)
                        {
                            sar = newLow;
                        }
                    }
                }
                else if (newHigh >= sar)
                {
                    isLong = true;
                    sar = ep;

                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }
                    if (sar > newLow)
                    {
                        sar = newLow;
                    }

                    outReal[outIdx++] = sar;

                    af = optInAcceleration;
                    ep = newHigh;

                    sar += af * (ep - sar);
                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }
                    if (sar > newLow)
                    {
                        sar = newLow;
                    }
                }
                else
                {
                    outReal[outIdx++] = sar;

                    if (newLow < ep)
                    {
                        ep = newLow;
                        af += optInAcceleration;
                        if (af > optInMaximum)
                        {
                            af = optInMaximum;
                        }
                    }

                    sar += af * (ep - sar);

                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }
                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }
                }
            }

            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static int SarLookback()
        {
            return 1;
        }
    }
}
