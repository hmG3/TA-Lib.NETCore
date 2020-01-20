using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Beta(int startIdx, int endIdx, double[] inReal0, double[] inReal1, ref int outBegIdx, ref int outNBElement,
            double[] outReal, int optInTimePeriod = 5)
        {
            double x;
            double y;
            double sxx = default;
            double sxy = default;
            double sx = default;
            double sy = default;
            double tmpReal;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal0 == null)
            {
                return RetCode.BadParam;
            }

            if (inReal1 == null)
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

            int nbInitialElementNeeded = optInTimePeriod;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int trailingIdx = startIdx - nbInitialElementNeeded;
            var trailingLastPriceX = inReal0[trailingIdx];
            var lastPriceX = trailingLastPriceX;
            var trailingLastPriceY = inReal1[trailingIdx];
            var lastPriceY = trailingLastPriceY;
            trailingIdx++;
            int i = trailingIdx;
            while (true)
            {
                if (i >= startIdx)
                {
                    break;
                }

                tmpReal = inReal0[i];
                if (-1E-08 >= lastPriceX || lastPriceX >= 1E-08)
                {
                    x = (tmpReal - lastPriceX) / lastPriceX;
                }
                else
                {
                    x = 0.0;
                }

                lastPriceX = tmpReal;
                tmpReal = inReal1[i];
                i++;
                if (-1E-08 >= lastPriceY || lastPriceY >= 1E-08)
                {
                    y = (tmpReal - lastPriceY) / lastPriceY;
                }
                else
                {
                    y = 0.0;
                }

                lastPriceY = tmpReal;
                sxx += x * x;
                sxy += x * y;
                sx += x;
                sy += y;
            }

            int outIdx = default;
            double n = optInTimePeriod;
            do
            {
                tmpReal = inReal0[i];
                if (-1E-08 >= lastPriceX || lastPriceX >= 1E-08)
                {
                    x = (tmpReal - lastPriceX) / lastPriceX;
                }
                else
                {
                    x = 0.0;
                }

                lastPriceX = tmpReal;
                tmpReal = inReal1[i];
                i++;
                if (-1E-08 >= lastPriceY || lastPriceY >= 1E-08)
                {
                    y = (tmpReal - lastPriceY) / lastPriceY;
                }
                else
                {
                    y = 0.0;
                }

                lastPriceY = tmpReal;
                sxx += x * x;
                sxy += x * y;
                sx += x;
                sy += y;
                tmpReal = inReal0[trailingIdx];
                if (-1E-08 >= trailingLastPriceX || trailingLastPriceX >= 1E-08)
                {
                    x = (tmpReal - trailingLastPriceX) / trailingLastPriceX;
                }
                else
                {
                    x = 0.0;
                }

                trailingLastPriceX = tmpReal;
                tmpReal = inReal1[trailingIdx];
                trailingIdx++;
                if (-1E-08 >= trailingLastPriceY || trailingLastPriceY >= 1E-08)
                {
                    y = (tmpReal - trailingLastPriceY) / trailingLastPriceY;
                }
                else
                {
                    y = 0.0;
                }

                trailingLastPriceY = tmpReal;
                tmpReal = n * sxx - sx * sx;
                if (-1E-08 >= tmpReal || tmpReal >= 1E-08)
                {
                    outReal[outIdx] = (n * sxy - sx * sy) / tmpReal;
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                sxx -= x * x;
                sxy -= x * y;
                sx -= x;
                sy -= y;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Beta(int startIdx, int endIdx, decimal[] inReal0, decimal[] inReal1, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, int optInTimePeriod = 5)
        {
            decimal x;
            decimal y;
            decimal sxx = default;
            decimal sxy = default;
            decimal sx = default;
            decimal sy = default;
            decimal tmpReal;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal0 == null)
            {
                return RetCode.BadParam;
            }

            if (inReal1 == null)
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

            int nbInitialElementNeeded = optInTimePeriod;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int trailingIdx = startIdx - nbInitialElementNeeded;
            decimal trailingLastPriceX = inReal0[trailingIdx];
            var lastPriceX = trailingLastPriceX;
            decimal trailingLastPriceY = inReal1[trailingIdx];
            var lastPriceY = trailingLastPriceY;
            trailingIdx++;
            int i = trailingIdx;
            while (true)
            {
                if (i >= startIdx)
                {
                    break;
                }

                tmpReal = inReal0[i];
                if (-1E-08m >= lastPriceX || lastPriceX >= 1E-08m)
                {
                    x = (tmpReal - lastPriceX) / lastPriceX;
                }
                else
                {
                    x = Decimal.Zero;
                }

                lastPriceX = tmpReal;
                tmpReal = inReal1[i];
                i++;
                if (-1E-08m >= lastPriceY || lastPriceY >= 1E-08m)
                {
                    y = (tmpReal - lastPriceY) / lastPriceY;
                }
                else
                {
                    y = Decimal.Zero;
                }

                lastPriceY = tmpReal;
                sxx += x * x;
                sxy += x * y;
                sx += x;
                sy += y;
            }

            int outIdx = default;
            decimal n = optInTimePeriod;
            do
            {
                tmpReal = inReal0[i];
                if (-1E-08m >= lastPriceX || lastPriceX >= 1E-08m)
                {
                    x = (tmpReal - lastPriceX) / lastPriceX;
                }
                else
                {
                    x = Decimal.Zero;
                }

                lastPriceX = tmpReal;
                tmpReal = inReal1[i];
                i++;
                if (-1E-08m >= lastPriceY || lastPriceY >= 1E-08m)
                {
                    y = (tmpReal - lastPriceY) / lastPriceY;
                }
                else
                {
                    y = Decimal.Zero;
                }

                lastPriceY = tmpReal;
                sxx += x * x;
                sxy += x * y;
                sx += x;
                sy += y;
                tmpReal = inReal0[trailingIdx];
                if (-1E-08m >= trailingLastPriceX || trailingLastPriceX >= 1E-08m)
                {
                    x = (tmpReal - trailingLastPriceX) / trailingLastPriceX;
                }
                else
                {
                    x = Decimal.Zero;
                }

                trailingLastPriceX = tmpReal;
                tmpReal = inReal1[trailingIdx];
                trailingIdx++;
                if (-1E-08m >= trailingLastPriceY || trailingLastPriceY >= 1E-08m)
                {
                    y = (tmpReal - trailingLastPriceY) / trailingLastPriceY;
                }
                else
                {
                    y = Decimal.Zero;
                }

                trailingLastPriceY = tmpReal;
                tmpReal = n * sxx - sx * sx;
                if (-1E-08m >= tmpReal || tmpReal >= 1E-08m)
                {
                    outReal[outIdx] = (n * sxy - sx * sy) / tmpReal;
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = Decimal.Zero;
                    outIdx++;
                }

                sxx -= x * x;
                sxy -= x * y;
                sx -= x;
                sy -= y;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int BetaLookback(int optInTimePeriod = 5)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod;
        }
    }
}
