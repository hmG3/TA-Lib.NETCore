using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode AdOsc(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, double[] inVolume,
            ref int outBegIdx, ref int outNBElement, double[] outReal, int optInFastPeriod = 3, int optInSlowPeriod = 10)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || inVolume == null)
            {
                return RetCode.BadParam;
            }

            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            var slowestPeriod = optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod;

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
            double ad = default;
            double fastk = 2.0 / (optInFastPeriod + 1);
            double oneMinusFastk = 1.0 - fastk;
            double slowk = 2.0 / (optInSlowPeriod + 1);
            double oneMinusSlowk = 1.0 - slowk;
            double high = inHigh[today];
            double low = inLow[today];
            double tmp = high - low;
            double close = inClose[today];
            if (tmp > 0.0)
            {
                ad += (close - low - (high - close)) / tmp * inVolume[today];
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
                    ad += (close - low - (high - close)) / tmp * inVolume[today];
                }

                today++;
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
            }

            int outIdx = default;
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
                    ad += (close - low - (high - close)) / tmp * inVolume[today];
                }

                today++;
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
                outReal[outIdx] = fastEMA - slowEMA;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static RetCode AdOsc(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, decimal[] inVolume,
            ref int outBegIdx, ref int outNBElement, decimal[] outReal, int optInFastPeriod = 3, int optInSlowPeriod = 10)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || inVolume == null)
            {
                return RetCode.BadParam;
            }

            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            var slowestPeriod = optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod;

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
            decimal ad = default;
            decimal fastk = 2m / (optInFastPeriod + 1);
            decimal oneMinusFastk = Decimal.One - fastk;
            decimal slowk = 2m / (optInSlowPeriod + 1);
            decimal oneMinusSlowk = Decimal.One - slowk;
            decimal high = inHigh[today];
            decimal low = inLow[today];
            decimal tmp = high - low;
            decimal close = inClose[today];
            if (tmp > Decimal.Zero)
            {
                ad += (close - low - (high - close)) / tmp * inVolume[today];
            }

            today++;
            decimal fastEMA = ad;
            decimal slowEMA = ad;
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
                if (tmp > Decimal.Zero)
                {
                    ad += (close - low - (high - close)) / tmp * inVolume[today];
                }

                today++;
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
            }

            int outIdx = default;
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
                if (tmp > Decimal.Zero)
                {
                    ad += (close - low - (high - close)) / tmp * inVolume[today];
                }

                today++;
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
                outReal[outIdx] = fastEMA - slowEMA;
                outIdx++;
            }

            outNBElement = outIdx;
            return RetCode.Success;
        }

        public static int AdOscLookback(int optInFastPeriod = 3, int optInSlowPeriod = 10)
        {
            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
            {
                return -1;
            }

            var slowestPeriod = optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod;

            return EmaLookback(slowestPeriod);
        }
    }
}
