using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Sar(int startIdx, int endIdx, double[] inHigh, double[] inLow, double optInAcceleration, double optInMaximum,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            double sar;
            double ep;
            int isLong;
            int tempInt = 0;
            double[] ep_temp = new double[1];
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

            if (optInAcceleration == -4E+37)
            {
                optInAcceleration = 0.02;
            }
            else if ((optInAcceleration < 0.0) || (optInAcceleration > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (optInMaximum == -4E+37)
            {
                optInMaximum = 0.2;
            }
            else if ((optInMaximum < 0.0) || (optInMaximum > 3E+37))
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

            RetCode retCode = MinusDM(startIdx, startIdx, inHigh, inLow, 1, ref tempInt, ref tempInt, ep_temp);
            if (ep_temp[0] > 0.0)
            {
                isLong = 0;
            }
            else
            {
                isLong = 1;
            }

            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            outBegIdx = startIdx;
            int outIdx = 0;
            int todayIdx = startIdx;
            double newHigh = inHigh[todayIdx - 1];
            double newLow = inLow[todayIdx - 1];
            if (isLong == 1)
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
                if (isLong == 1)
                {
                    if (newLow <= sar)
                    {
                        isLong = 0;
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
                    isLong = 1;
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

        public static RetCode Sar(int startIdx, int endIdx, float[] inHigh, float[] inLow, double optInAcceleration, double optInMaximum,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            double sar;
            double ep;
            int isLong;
            int tempInt = 0;
            double[] ep_temp = new double[1];
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

            if (optInAcceleration == -4E+37)
            {
                optInAcceleration = 0.02;
            }
            else if ((optInAcceleration < 0.0) || (optInAcceleration > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (optInMaximum == -4E+37)
            {
                optInMaximum = 0.2;
            }
            else if ((optInMaximum < 0.0) || (optInMaximum > 3E+37))
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

            RetCode retCode = MinusDM(startIdx, startIdx, inHigh, inLow, 1, ref tempInt, ref tempInt, ep_temp);
            if (ep_temp[0] > 0.0)
            {
                isLong = 0;
            }
            else
            {
                isLong = 1;
            }

            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            outBegIdx = startIdx;
            int outIdx = 0;
            int todayIdx = startIdx;
            double newHigh = inHigh[todayIdx - 1];
            double newLow = inLow[todayIdx - 1];
            if (isLong == 1)
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
                if (isLong == 1)
                {
                    if (newLow <= sar)
                    {
                        isLong = 0;
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
                    isLong = 1;
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

        public static int SarLookback(double optInAcceleration, double optInMaximum)
        {
            if (optInAcceleration == -4E+37)
            {
                optInAcceleration = 0.02;
            }
            else if ((optInAcceleration < 0.0) || (optInAcceleration > 3E+37))
            {
                return -1;
            }

            if (optInMaximum == -4E+37)
            {
                optInMaximum = 0.2;
            }
            else if ((optInMaximum < 0.0) || (optInMaximum > 3E+37))
            {
                return -1;
            }

            return 1;
        }
    }
}
