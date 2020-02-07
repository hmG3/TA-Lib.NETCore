using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Beta(int startIdx, int endIdx, double[] inReal0, double[] inReal1, ref int outBegIdx, ref int outNBElement,
            double[] outReal, int optInTimePeriod = 5)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal0 == null || inReal1 == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
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
                outBegIdx = outNBElement = 0;
                return RetCode.Success;
            }

            double x, y, tmpReal, sxy, sx, sy;
            double sxx = sxy = sx = sy = default;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            var trailingLastPriceX = inReal0[trailingIdx];
            var lastPriceX = trailingLastPriceX;
            var trailingLastPriceY = inReal1[trailingIdx];
            var lastPriceY = trailingLastPriceY;
            int i = ++trailingIdx;
            while (i < startIdx)
            {
                tmpReal = inReal0[i];
                if (!TA_IsZero(lastPriceX))
                {
                    x = (tmpReal - lastPriceX) / lastPriceX;
                }
                else
                {
                    x = 0.0;
                }
                lastPriceX = tmpReal;

                tmpReal = inReal1[i++];
                if (!TA_IsZero(lastPriceY))
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
            do
            {
                tmpReal = inReal0[i];
                if (!TA_IsZero(lastPriceX))
                {
                    x = (tmpReal - lastPriceX) / lastPriceX;
                }
                else
                {
                    x = 0.0;
                }

                lastPriceX = tmpReal;

                tmpReal = inReal1[i++];
                if (!TA_IsZero(lastPriceY))
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
                if (!TA_IsZero(trailingLastPriceX))
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
                if (!TA_IsZero(trailingLastPriceY))
                {
                    y = (tmpReal - trailingLastPriceY) / trailingLastPriceY;
                }
                else
                {
                    y = 0.0;
                }
                trailingLastPriceY = tmpReal;

                tmpReal = optInTimePeriod * sxx - sx * sx;
                if (!TA_IsZero(tmpReal))
                {
                    outReal[outIdx++] = (optInTimePeriod * sxy - sx * sy) / tmpReal;
                }
                else
                {
                    outReal[outIdx++] = 0.0;
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
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal0 == null || inReal1 == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
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
                outBegIdx = outNBElement = 0;
                return RetCode.Success;
            }

            decimal x, y, tmpReal, sxy, sx, sy;
            decimal sxx = sxy = sx = sy = default;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            var trailingLastPriceX = inReal0[trailingIdx];
            var lastPriceX = trailingLastPriceX;
            var trailingLastPriceY = inReal1[trailingIdx];
            var lastPriceY = trailingLastPriceY;
            int i = ++trailingIdx;
            while (i < startIdx)
            {
                tmpReal = inReal0[i];
                if (!TA_IsZero(lastPriceX))
                {
                    x = (tmpReal - lastPriceX) / lastPriceX;
                }
                else
                {
                    x = Decimal.Zero;
                }
                lastPriceX = tmpReal;

                tmpReal = inReal1[i++];
                if (!TA_IsZero(lastPriceY))
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
            do
            {
                tmpReal = inReal0[i];
                if (!TA_IsZero(lastPriceX))
                {
                    x = (tmpReal - lastPriceX) / lastPriceX;
                }
                else
                {
                    x = Decimal.Zero;
                }

                lastPriceX = tmpReal;

                tmpReal = inReal1[i++];
                if (!TA_IsZero(lastPriceY))
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
                if (!TA_IsZero(trailingLastPriceX))
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
                if (!TA_IsZero(trailingLastPriceY))
                {
                    y = (tmpReal - trailingLastPriceY) / trailingLastPriceY;
                }
                else
                {
                    y = Decimal.Zero;
                }
                trailingLastPriceY = tmpReal;

                tmpReal = optInTimePeriod * sxx - sx * sx;
                if (!TA_IsZero(tmpReal))
                {
                    outReal[outIdx++] = (optInTimePeriod * sxy - sx * sy) / tmpReal;
                }
                else
                {
                    outReal[outIdx++] = Decimal.Zero;
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
