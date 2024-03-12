namespace TALib
{
    public static partial class Core
    {
        public static RetCode AdOsc(double[] inHigh, double[] inLow, double[] inClose, double[] inVolume, int startIdx, int endIdx,
            double[] outReal, out int outBegIdx, out int outNbElement, int optInFastPeriod = 3, int optInSlowPeriod = 10)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || inVolume == null || optInFastPeriod < 2 || optInFastPeriod > 100000 ||
                optInSlowPeriod < 2 || optInSlowPeriod > 100000 || outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AdOscLookback(optInFastPeriod, optInSlowPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int today = startIdx - lookbackTotal;

            double ad = default;

            double fastk = 2.0 / (optInFastPeriod + 1);
            double oneMinusFastk = 1.0 - fastk;

            double slowk = 2.0 / (optInSlowPeriod + 1);
            double oneMinusSlowk = 1.0 - slowk;

            CalculateAd();
            double fastEMA = ad;
            double slowEMA = ad;

            while (today < startIdx)
            {
                CalculateAd();
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
            }

            int outIdx = default;
            while (today <= endIdx)
            {
                CalculateAd();
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;

                outReal[outIdx++] = fastEMA - slowEMA;
            }

            outNbElement = outIdx;

            return RetCode.Success;

            void CalculateAd()
            {
                double h = inHigh[today];
                double l = inLow[today];
                double tmp = h - l;
                double c = inClose[today];
                if (tmp > 0.0)
                {
                    ad += (c - l - (h - c)) / tmp * inVolume[today];
                }

                today++;
            }
        }

        public static RetCode AdOsc(decimal[] inHigh, decimal[] inLow, decimal[] inClose, decimal[] inVolume, int startIdx, int endIdx,
            decimal[] outReal, out int outBegIdx, out int outNbElement, int optInFastPeriod = 3, int optInSlowPeriod = 10)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || inVolume == null || optInFastPeriod < 2 || optInFastPeriod > 100000 ||
                optInSlowPeriod < 2 || optInSlowPeriod > 100000 || outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AdOscLookback(optInFastPeriod, optInSlowPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int today = startIdx - lookbackTotal;

            decimal ad = default;

            decimal fastk = 2m / (optInFastPeriod + 1);
            decimal oneMinusFastk = Decimal.One - fastk;

            decimal slowk = 2m / (optInSlowPeriod + 1);
            decimal oneMinusSlowk = Decimal.One - slowk;

            CalculateAd();
            decimal fastEMA = ad;
            decimal slowEMA = ad;

            while (today < startIdx)
            {
                CalculateAd();
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
            }

            int outIdx = default;
            while (today <= endIdx)
            {
                CalculateAd();
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;

                outReal[outIdx++] = fastEMA - slowEMA;
            }

            outNbElement = outIdx;

            return RetCode.Success;

            void CalculateAd()
            {
                decimal h = inHigh[today];
                decimal l = inLow[today];
                decimal tmp = h - l;
                decimal c = inClose[today];
                if (tmp > Decimal.Zero)
                {
                    ad += (c - l - (h - c)) / tmp * inVolume[today];
                }

                today++;
            }
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
