using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Sar(int startIdx, int endIdx, double[] inHigh, double[] inLow, ref int outBegIdx, ref int outNBElement,
            double[] outReal, double optInAcceleration = 0.02, double optInMaximum = 0.2)
        {
            double sar;
            double ep;
            int tempInt = default;
            var epTemp = new double[1];
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

            if (optInAcceleration < 0.0 || optInMaximum < 0.0)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
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
                optInAcceleration = optInMaximum;
                af = optInAcceleration;
            }

            RetCode retCode = MinusDM(startIdx, startIdx, inHigh, inLow, ref tempInt, ref tempInt, epTemp, 1);
            var isLong = epTemp[0] <= 0.0;

            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            outBegIdx = startIdx;
            int outIdx = default;
            int todayIdx = startIdx;
            double newHigh = inHigh[todayIdx - 1];
            double newLow = inLow[todayIdx - 1];
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

                        outReal[outIdx] = sar;
                        outIdx++;
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
                        outReal[outIdx] = sar;
                        outIdx++;
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

                    outReal[outIdx] = sar;
                    outIdx++;
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
                    outReal[outIdx] = sar;
                    outIdx++;
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
            decimal sar;
            decimal ep;
            int tempInt = default;
            var epTemp = new decimal[1];
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

            if (optInAcceleration < Decimal.Zero || optInMaximum < Decimal.Zero)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
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
                optInAcceleration = optInMaximum;
                af = optInAcceleration;
            }

            RetCode retCode = MinusDM(startIdx, startIdx, inHigh, inLow, ref tempInt, ref tempInt, epTemp, 1);
            var isLong = epTemp[0] <= Decimal.Zero;

            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            outBegIdx = startIdx;
            int outIdx = default;
            int todayIdx = startIdx;
            decimal newHigh = inHigh[todayIdx - 1];
            decimal newLow = inLow[todayIdx - 1];
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

                        outReal[outIdx] = sar;
                        outIdx++;
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
                        outReal[outIdx] = sar;
                        outIdx++;
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

                    outReal[outIdx] = sar;
                    outIdx++;
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
                    outReal[outIdx] = sar;
                    outIdx++;
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
