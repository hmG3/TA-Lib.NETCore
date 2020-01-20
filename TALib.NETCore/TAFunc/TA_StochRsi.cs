namespace TALib
{
    public partial class Core
    {
        public static RetCode StochRsi(int startIdx, int endIdx, double[] inReal, MAType optInFastDMaType, ref int outBegIdx,
            ref int outNBElement, double[] outFastK, double[] outFastD, int optInTimePeriod = 14, int optInFastKPeriod = 5,
            int optInFastDPeriod = 3)
        {
            int outNbElement1 = default;
            int outBegIdx2 = default;
            int outBegIdx1 = default;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000 || optInFastKPeriod < 1 || optInFastKPeriod > 100000 ||
                optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outFastK == null)
            {
                return RetCode.BadParam;
            }

            if (outFastD == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
            int lookbackSTOCHF = StochFLookback(optInFastDMaType);
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

            retCode = StochF(0, tempArraySize - 1, tempRSIBuffer, tempRSIBuffer, tempRSIBuffer, optInFastDMaType, ref outBegIdx2,
                ref outNBElement, outFastK, outFastD, optInFastKPeriod, optInFastDPeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            return RetCode.Success;
        }

        public static RetCode StochRsi(int startIdx, int endIdx, decimal[] inReal, MAType optInFastDMaType, ref int outBegIdx,
            ref int outNBElement, decimal[] outFastK, decimal[] outFastD, int optInTimePeriod = 14, int optInFastKPeriod = 5,
            int optInFastDPeriod = 3)
        {
            int outNbElement1 = default;
            int outBegIdx2 = default;
            int outBegIdx1 = default;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000 || optInFastKPeriod < 1 || optInFastKPeriod > 100000 ||
                optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outFastK == null)
            {
                return RetCode.BadParam;
            }

            if (outFastD == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
            int lookbackSTOCHF = StochFLookback(optInFastDMaType);
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

            retCode = StochF(0, tempArraySize - 1, tempRSIBuffer, tempRSIBuffer, tempRSIBuffer, optInFastDMaType, ref outBegIdx2,
                ref outNBElement, outFastK, outFastD, optInFastKPeriod, optInFastDPeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            return RetCode.Success;
        }

        public static int StochRsiLookback(MAType optInFastDMaType, int optInTimePeriod = 14, int optInFastKPeriod = 5,
            int optInFastDPeriod = 3)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000 || optInFastKPeriod < 1 || optInFastKPeriod > 100000 ||
                optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return -1;
            }

            return RsiLookback(optInTimePeriod) + StochFLookback(optInFastDMaType, optInFastKPeriod, optInFastDPeriod);
        }
    }
}
