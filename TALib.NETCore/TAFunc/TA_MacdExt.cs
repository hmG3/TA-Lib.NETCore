using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MacdExt(int startIdx, int endIdx, double[] inReal, MAType optInFastMAType, MAType optInSlowMAType,
            MAType optInSignalMAType, ref int outBegIdx, ref int outNBElement, double[] outMACD, double[] outMACDSignal,
            double[] outMACDHist, int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInFastPeriod < 2 ||
                optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 || optInSignalPeriod < 1 ||
                optInSignalPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int tempInteger;
            if (optInSlowPeriod < optInFastPeriod)
            {
                tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;

                MAType tempMAType = optInSlowMAType;
                optInSlowMAType = optInFastMAType;
                optInFastMAType = tempMAType;
            }

            int lookbackLargest = MaLookback(optInFastMAType, optInFastPeriod);
            tempInteger = MaLookback(optInSlowMAType, optInSlowPeriod);
            if (tempInteger > lookbackLargest)
            {
                lookbackLargest = tempInteger;
            }

            int lookbackSignal = MaLookback(optInSignalMAType, optInSignalPeriod);
            int lookbackTotal = lookbackSignal + lookbackLargest;
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

            int outNbElement1 = default;
            int outNbElement2 = default;
            int outBegIdx2 = default;
            int outBegIdx1 = default;
            tempInteger = endIdx - startIdx + 1 + lookbackSignal;
            var fastMABuffer = new double[tempInteger];
            var slowMABuffer = new double[tempInteger];

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = Ma(tempInteger, endIdx, inReal, optInSlowMAType, ref outBegIdx1, ref outNbElement1, slowMABuffer,
                optInSlowPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            retCode = Ma(tempInteger, endIdx, inReal, optInFastMAType, ref outBegIdx2, ref outNbElement2, fastMABuffer,
                optInFastPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;

            }

            if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
                outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.InternalError;
            }

            for (var i = 0; i < outNbElement1; i++)
            {
                fastMABuffer[i] -= slowMABuffer[i];
            }

            Array.Copy(fastMABuffer, lookbackSignal, outMACD, 0, endIdx - startIdx + 1);
            retCode = Ma(0, outNbElement1 - 1, fastMABuffer, optInSignalMAType, ref outBegIdx2, ref outNbElement2, outMACDSignal,
                optInSignalPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            for (var i = 0; i < outNbElement2; i++)
            {
                outMACDHist[i] = outMACD[i] - outMACDSignal[i];
            }

            outBegIdx = startIdx;
            outNBElement = outNbElement2;

            return RetCode.Success;
        }

        public static RetCode MacdExt(int startIdx, int endIdx, decimal[] inReal, MAType optInFastMAType, MAType optInSlowMAType,
            MAType optInSignalMAType, ref int outBegIdx, ref int outNBElement, decimal[] outMACD, decimal[] outMACDSignal,
            decimal[] outMACDHist, int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMACD == null || outMACDSignal == null || outMACDHist == null || optInFastPeriod < 2 ||
                optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 || optInSignalPeriod < 1 ||
                optInSignalPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int tempInteger;
            if (optInSlowPeriod < optInFastPeriod)
            {
                tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;

                MAType tempMAType = optInSlowMAType;
                optInSlowMAType = optInFastMAType;
                optInFastMAType = tempMAType;
            }

            int lookbackLargest = MaLookback(optInFastMAType, optInFastPeriod);
            tempInteger = MaLookback(optInSlowMAType, optInSlowPeriod);
            if (tempInteger > lookbackLargest)
            {
                lookbackLargest = tempInteger;
            }

            int lookbackSignal = MaLookback(optInSignalMAType, optInSignalPeriod);
            int lookbackTotal = lookbackSignal + lookbackLargest;
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

            int outNbElement1 = default;
            int outNbElement2 = default;
            int outBegIdx2 = default;
            int outBegIdx1 = default;
            tempInteger = endIdx - startIdx + 1 + lookbackSignal;
            var fastMABuffer = new decimal[tempInteger];
            var slowMABuffer = new decimal[tempInteger];

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = Ma(tempInteger, endIdx, inReal, optInSlowMAType, ref outBegIdx1, ref outNbElement1, slowMABuffer,
                optInSlowPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            retCode = Ma(tempInteger, endIdx, inReal, optInFastMAType, ref outBegIdx2, ref outNbElement2, fastMABuffer,
                optInFastPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;

            }

            if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
                outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.InternalError;
            }

            for (var i = 0; i < outNbElement1; i++)
            {
                fastMABuffer[i] -= slowMABuffer[i];
            }

            Array.Copy(fastMABuffer, lookbackSignal, outMACD, 0, endIdx - startIdx + 1);
            retCode = Ma(0, outNbElement1 - 1, fastMABuffer, optInSignalMAType, ref outBegIdx2, ref outNbElement2, outMACDSignal,
                optInSignalPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            for (var i = 0; i < outNbElement2; i++)
            {
                outMACDHist[i] = outMACD[i] - outMACDSignal[i];
            }

            outBegIdx = startIdx;
            outNBElement = outNbElement2;

            return RetCode.Success;
        }

        public static int MacdExtLookback(MAType optInFastMAType, MAType optInSlowMAType, MAType optInSignalMAType,
            int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 ||
                optInSignalPeriod < 1 || optInSignalPeriod > 100000)
            {
                return -1;
            }

            int lookbackLargest = MaLookback(optInFastMAType, optInFastPeriod);
            int tempInteger = MaLookback(optInSlowMAType, optInSlowPeriod);
            if (tempInteger > lookbackLargest)
            {
                lookbackLargest = tempInteger;
            }

            return lookbackLargest + MaLookback(optInSignalMAType, optInSignalPeriod);
        }
    }
}
