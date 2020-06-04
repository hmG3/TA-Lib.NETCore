namespace TALib
{
    public partial class Core
    {
        public static RetCode Accbands(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, out int outBegIdx,
            out int outNbElement, double[] outRealUpperBand, double[] outRealMiddleBand, double[] outRealLowerBand,
            int optInTimePeriod = 20)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outRealUpperBand == null || outRealMiddleBand == null ||
                outRealLowerBand == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AccbandsLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int outputSize = endIdx - startIdx + 1;
            int bufferSize = outputSize + lookbackTotal;
            var tempBuffer1 = new double[bufferSize];
            var tempBuffer2 = new double[bufferSize];

            for (int j = 0, i = startIdx - lookbackTotal; i <= endIdx; i++, j++)
            {
                double tempReal = inHigh[i] + inLow[i];
                if (TA_IsZero(tempReal))
                {
                    tempReal = 4 * (inHigh[i] - inLow[i]) / tempReal;
                    tempBuffer1[j] = inHigh[i] * (1 + tempReal);
                    tempBuffer2[j] = inLow[i] * (1 - tempReal);
                }
                else
                {
                    tempBuffer1[j] = inHigh[i];
                    tempBuffer2[j] = inLow[i];
                }
            }

            var retCode = Sma(startIdx, endIdx, inClose, out _, out var outNbElementDummy, outRealMiddleBand, optInTimePeriod);
            if (retCode != RetCode.Success || outNbElementDummy != outputSize)
            {
                return retCode;
            }

            retCode = Sma(0, bufferSize - 1, tempBuffer1, out _, out outNbElementDummy, outRealUpperBand, optInTimePeriod);
            if (retCode != RetCode.Success || outNbElementDummy != outputSize)
            {
                return retCode;
            }

            retCode = Sma(0, bufferSize - 1, tempBuffer2, out _, out outNbElementDummy, outRealLowerBand, optInTimePeriod);
            if (retCode != RetCode.Success || outNbElementDummy != outputSize)
            {
                return retCode;
            }

            return RetCode.Success;
        }

        public static RetCode Accbands(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, int optInTimePeriod,
            out int outBegIdx, out int outNbElement, decimal[] outRealUpperBand, decimal[] outRealMiddleBand, decimal[] outRealLowerBand)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outRealUpperBand == null || outRealMiddleBand == null ||
                outRealLowerBand == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AccbandsLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int outputSize = endIdx - startIdx + 1;
            int bufferSize = outputSize + lookbackTotal;
            var tempBuffer1 = new decimal[bufferSize];
            var tempBuffer2 = new decimal[bufferSize];

            for (int j = 0, i = startIdx - lookbackTotal; i <= endIdx; i++, j++)
            {
                decimal tempReal = inHigh[i] + inLow[i];
                if (TA_IsZero(tempReal))
                {
                    tempReal = 4 * (inHigh[i] - inLow[i]) / tempReal;
                    tempBuffer1[j] = inHigh[i] * (1 + tempReal);
                    tempBuffer2[j] = inLow[i] * (1 - tempReal);
                }
                else
                {
                    tempBuffer1[j] = inHigh[i];
                    tempBuffer2[j] = inLow[i];
                }
            }

            var retCode = Sma(startIdx, endIdx, inClose, out _, out var outNbElementDummy, outRealMiddleBand, optInTimePeriod);
            if (retCode != RetCode.Success || outNbElementDummy != outputSize)
            {
                return retCode;
            }

            retCode = Sma(0, bufferSize - 1, tempBuffer1, out _, out outNbElementDummy, outRealUpperBand, optInTimePeriod);
            if (retCode != RetCode.Success || outNbElementDummy != outputSize)
            {
                return retCode;
            }

            retCode = Sma(0, bufferSize - 1, tempBuffer2, out _, out outNbElementDummy, outRealLowerBand, optInTimePeriod);
            if (retCode != RetCode.Success || outNbElementDummy != outputSize)
            {
                return retCode;
            }

            return RetCode.Success;
        }

        public static int AccbandsLookback(int optInTimePeriod = 20)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return SmaLookback(optInTimePeriod);
        }
    }
}
