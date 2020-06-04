namespace TALib
{
    public partial class Core
    {
        public static RetCode StochRsi(int startIdx, int endIdx, double[] inReal, MAType optInFastDMAType, out int outBegIdx,
            out int outNbElement, double[] outFastK, double[] outFastD, int optInTimePeriod = 14, int optInFastKPeriod = 5,
            int optInFastDPeriod = 3)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outFastK == null || outFastD == null || optInTimePeriod < 2 || optInTimePeriod > 100000 ||
                optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackSTOCHF = StochFLookback(optInFastDMAType);
            int lookbackTotal = StochRsiLookback(optInFastDMAType, optInTimePeriod, optInFastKPeriod, optInFastDPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int tempArraySize = endIdx - startIdx + 1 + lookbackSTOCHF;
            var tempRSIBuffer = new double[tempArraySize];
            RetCode retCode = Rsi(startIdx - lookbackSTOCHF, endIdx, inReal, out _, out var outNbElement1, tempRSIBuffer,
                optInTimePeriod);
            if (retCode != RetCode.Success || outNbElement1 == 0)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            retCode = StochF(0, tempArraySize - 1, tempRSIBuffer, tempRSIBuffer, tempRSIBuffer, optInFastDMAType, out _,
                out outNbElement, outFastK, outFastD, optInFastKPeriod, optInFastDPeriod);
            if (retCode != RetCode.Success || outNbElement == 0)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            return RetCode.Success;
        }

        public static RetCode StochRsi(int startIdx, int endIdx, decimal[] inReal, MAType optInFastDMAType, out int outBegIdx,
            out int outNbElement, decimal[] outFastK, decimal[] outFastD, int optInTimePeriod = 14, int optInFastKPeriod = 5,
            int optInFastDPeriod = 3)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outFastK == null || outFastD == null || optInTimePeriod < 2 || optInTimePeriod > 100000 ||
                optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackSTOCHF = StochFLookback(optInFastDMAType);
            int lookbackTotal = StochRsiLookback(optInFastDMAType, optInTimePeriod, optInFastKPeriod, optInFastDPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int tempArraySize = endIdx - startIdx + 1 + lookbackSTOCHF;
            var tempRSIBuffer = new decimal[tempArraySize];
            RetCode retCode = Rsi(startIdx - lookbackSTOCHF, endIdx, inReal, out _, out var outNbElement1, tempRSIBuffer,
                optInTimePeriod);
            if (retCode != RetCode.Success || outNbElement1 == 0)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            retCode = StochF(0, tempArraySize - 1, tempRSIBuffer, tempRSIBuffer, tempRSIBuffer, optInFastDMAType, out _,
                out outNbElement, outFastK, outFastD, optInFastKPeriod, optInFastDPeriod);
            if (retCode != RetCode.Success || outNbElement == 0)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            return RetCode.Success;
        }

        public static int StochRsiLookback(MAType optInFastDMAType, int optInTimePeriod = 14, int optInFastKPeriod = 5,
            int optInFastDPeriod = 3)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000 || optInFastKPeriod < 1 || optInFastKPeriod > 100000 ||
                optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return -1;
            }

            return RsiLookback(optInTimePeriod) + StochFLookback(optInFastDMAType, optInFastKPeriod, optInFastDPeriod);
        }
    }
}
