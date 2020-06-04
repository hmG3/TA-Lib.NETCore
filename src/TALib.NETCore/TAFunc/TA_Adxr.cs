namespace TALib
{
    public partial class Core
    {
        public static RetCode Adxr(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, out int outBegIdx,
            out int outNbElement, double[] outReal, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AdxrLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var adx = new double[endIdx - startIdx + optInTimePeriod];

            RetCode retCode = Adx(startIdx - (optInTimePeriod - 1), endIdx, inHigh, inLow, inClose, out outBegIdx, out outNbElement, adx,
                optInTimePeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            int i = optInTimePeriod - 1;
            int j = default;
            int outIdx = default;
            int nbElement = endIdx - startIdx + 2;
            while (--nbElement != 0)
            {
                outReal[outIdx++] = (adx[i++] + adx[j++]) / 2.0;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Adxr(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, out int outBegIdx,
            out int outNbElement, decimal[] outReal, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AdxrLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var adx = new decimal[endIdx - startIdx + optInTimePeriod];

            RetCode retCode = Adx(startIdx - (optInTimePeriod - 1), endIdx, inHigh, inLow, inClose, out outBegIdx, out outNbElement, adx,
                optInTimePeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            int i = optInTimePeriod - 1;
            int j = default;
            int outIdx = default;
            int nbElement = endIdx - startIdx + 2;
            while (--nbElement != 0)
            {
                outReal[outIdx++] = (adx[i++] + adx[j++]) / 2m;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int AdxrLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod > 1 ? optInTimePeriod + AdxLookback(optInTimePeriod) - 1 : 3;
        }
    }
}
