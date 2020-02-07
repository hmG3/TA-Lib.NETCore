namespace TALib
{
    public partial class Core
    {
        public static RetCode StochRsi(int startIdx, int endIdx, double[] inReal, MAType optInFastDMAType, ref int outBegIdx,
            ref int outNBElement, double[] outFastK, double[] outFastD, int optInTimePeriod = 14, int optInFastKPeriod = 5,
            int optInFastDPeriod = 3)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outFastK == null || outFastD == null || optInTimePeriod < 2 || optInTimePeriod > 100000 ||
                optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            outNBElement = 0;
            int lookbackSTOCHF = StochFLookback(optInFastDMAType);
            int lookbackTotal = RsiLookback(optInTimePeriod) + lookbackSTOCHF;
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

            int outBegIdx1, outBegIdx2;
            int outNbElement1 = outBegIdx1 = outBegIdx2 = default;
            outBegIdx = startIdx;
            int tempArraySize = endIdx - startIdx + 1 + lookbackSTOCHF;
            var tempRSIBuffer = new double[tempArraySize];
            RetCode retCode = Rsi(startIdx - lookbackSTOCHF, endIdx, inReal, ref outBegIdx1, ref outNbElement1, tempRSIBuffer,
                optInTimePeriod);
            if (retCode != RetCode.Success || outNbElement1 == 0)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            retCode = StochF(0, tempArraySize - 1, tempRSIBuffer, tempRSIBuffer, tempRSIBuffer, optInFastDMAType, ref outBegIdx2,
                ref outNBElement, outFastK, outFastD, optInFastKPeriod, optInFastDPeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            return RetCode.Success;
        }

        public static RetCode StochRsi(int startIdx, int endIdx, decimal[] inReal, MAType optInFastDMAType, ref int outBegIdx,
            ref int outNBElement, decimal[] outFastK, decimal[] outFastD, int optInTimePeriod = 14, int optInFastKPeriod = 5,
            int optInFastDPeriod = 3)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outFastK == null || outFastD == null || optInTimePeriod < 2 || optInTimePeriod > 100000 ||
                optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            outNBElement = 0;
            int lookbackSTOCHF = StochFLookback(optInFastDMAType);
            int lookbackTotal = RsiLookback(optInTimePeriod) + lookbackSTOCHF;
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

            int outBegIdx1, outBegIdx2;
            int outNbElement1 = outBegIdx1 = outBegIdx2 = default;
            outBegIdx = startIdx;
            int tempArraySize = endIdx - startIdx + 1 + lookbackSTOCHF;
            var tempRSIBuffer = new decimal[tempArraySize];
            RetCode retCode = Rsi(startIdx - lookbackSTOCHF, endIdx, inReal, ref outBegIdx1, ref outNbElement1, tempRSIBuffer,
                optInTimePeriod);
            if (retCode != RetCode.Success || outNbElement1 == 0)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            retCode = StochF(0, tempArraySize - 1, tempRSIBuffer, tempRSIBuffer, tempRSIBuffer, optInFastDMAType, ref outBegIdx2,
                ref outNBElement, outFastK, outFastD, optInFastKPeriod, optInFastDPeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outBegIdx = 0;
                outNBElement = 0;
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
