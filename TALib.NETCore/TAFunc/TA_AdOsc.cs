using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode AdOsc(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, double[] inVolume,
            int optInFastPeriod, int optInSlowPeriod, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int slowestPeriod;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || ((inClose == null) || (inVolume == null)))
            {
                return RetCode.BadParam;
            }

            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 3;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 10;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInFastPeriod < optInSlowPeriod)
            {
                slowestPeriod = optInSlowPeriod;
            }
            else
            {
                slowestPeriod = optInFastPeriod;
            }

            int lookbackTotal = EmaLookback(slowestPeriod);
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

            outBegIdx = startIdx;
            int today = startIdx - lookbackTotal;
            double ad = 0.0;
            double fastk = 2.0 / ((double) (optInFastPeriod + 1));
            double one_minus_fastk = 1.0 - fastk;
            double slowk = 2.0 / ((double) (optInSlowPeriod + 1));
            double one_minus_slowk = 1.0 - slowk;
            double high = inHigh[today];
            double low = inLow[today];
            double tmp = high - low;
            double close = inClose[today];
            if (tmp > 0.0)
            {
                ad += (((close - low) - (high - close)) / tmp) * inVolume[today];
            }

            today++;
            double fastEMA = ad;
            double slowEMA = ad;
            while (true)
            {
                if (today >= startIdx)
                {
                    break;
                }

                high = inHigh[today];
                low = inLow[today];
                tmp = high - low;
                close = inClose[today];
                if (tmp > 0.0)
                {
                    ad += (((close - low) - (high - close)) / tmp) * inVolume[today];
                }

                today++;
                fastEMA = (fastk * ad) + (one_minus_fastk * fastEMA);
                slowEMA = (slowk * ad) + (one_minus_slowk * slowEMA);
            }

            int outIdx = 0;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                high = inHigh[today];
                low = inLow[today];
                tmp = high - low;
                close = inClose[today];
                if (tmp > 0.0)
                {
                    ad += (((close - low) - (high - close)) / tmp) * inVolume[today];
                }

                today++;
                fastEMA = (fastk * ad) + (one_minus_fastk * fastEMA);
                slowEMA = (slowk * ad) + (one_minus_slowk * slowEMA);
                outReal[outIdx] = fastEMA - slowEMA;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode AdOsc(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, float[] inVolume,
            int optInFastPeriod, int optInSlowPeriod, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int slowestPeriod;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || ((inClose == null) || (inVolume == null)))
            {
                return RetCode.BadParam;
            }

            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 3;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 10;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInFastPeriod < optInSlowPeriod)
            {
                slowestPeriod = optInSlowPeriod;
            }
            else
            {
                slowestPeriod = optInFastPeriod;
            }

            int lookbackTotal = EmaLookback(slowestPeriod);
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

            outBegIdx = startIdx;
            int today = startIdx - lookbackTotal;
            double ad = 0.0;
            double fastk = 2.0 / ((double) (optInFastPeriod + 1));
            double one_minus_fastk = 1.0 - fastk;
            double slowk = 2.0 / ((double) (optInSlowPeriod + 1));
            double one_minus_slowk = 1.0 - slowk;
            double high = inHigh[today];
            double low = inLow[today];
            double tmp = high - low;
            double close = inClose[today];
            if (tmp > 0.0)
            {
                ad += (((close - low) - (high - close)) / tmp) * inVolume[today];
            }

            today++;
            double fastEMA = ad;
            double slowEMA = ad;
            while (true)
            {
                if (today >= startIdx)
                {
                    break;
                }

                high = inHigh[today];
                low = inLow[today];
                tmp = high - low;
                close = inClose[today];
                if (tmp > 0.0)
                {
                    ad += (((close - low) - (high - close)) / tmp) * inVolume[today];
                }

                today++;
                fastEMA = (fastk * ad) + (one_minus_fastk * fastEMA);
                slowEMA = (slowk * ad) + (one_minus_slowk * slowEMA);
            }

            int outIdx = 0;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                high = inHigh[today];
                low = inLow[today];
                tmp = high - low;
                close = inClose[today];
                if (tmp > 0.0)
                {
                    ad += (((close - low) - (high - close)) / tmp) * inVolume[today];
                }

                today++;
                fastEMA = (fastk * ad) + (one_minus_fastk * fastEMA);
                slowEMA = (slowk * ad) + (one_minus_slowk * slowEMA);
                outReal[outIdx] = fastEMA - slowEMA;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int AdOscLookback(int optInFastPeriod, int optInSlowPeriod)
        {
            int slowestPeriod;
            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 3;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return -1;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 10;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return -1;
            }

            if (optInFastPeriod < optInSlowPeriod)
            {
                slowestPeriod = optInSlowPeriod;
            }
            else
            {
                slowestPeriod = optInFastPeriod;
            }

            return EmaLookback(slowestPeriod);
        }
    }
}
