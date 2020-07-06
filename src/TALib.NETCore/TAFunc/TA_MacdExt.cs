using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode MacdExt(double[] inReal, int startIdx, int endIdx, double[] outMacd, double[] outMacdSignal,
            double[] outMacdHist, out int outBegIdx, out int outNbElement, MAType optInFastMAType, MAType optInSlowMAType,
            MAType optInSignalMAType, int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMacd == null || outMacdSignal == null || outMacdHist == null || optInFastPeriod < 2 ||
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

            int lookbackSignal = MaLookback(optInSignalMAType, optInSignalPeriod);
            int lookbackTotal = MacdExtLookback(optInFastMAType, optInSlowMAType, optInSignalMAType, optInFastPeriod, optInSlowPeriod,
                optInSignalPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            tempInteger = endIdx - startIdx + 1 + lookbackSignal;
            var fastMABuffer = new double[tempInteger];
            var slowMABuffer = new double[tempInteger];

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = Ma(inReal, tempInteger, endIdx, slowMABuffer, out var outBegIdx1, out var outNbElement1, optInSlowMAType,
                optInSlowPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = Ma(inReal, tempInteger, endIdx, fastMABuffer, out var outBegIdx2, out var outNbElement2, optInFastMAType,
                optInFastPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
                outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
            {
                return RetCode.InternalError;
            }

            for (var i = 0; i < outNbElement1; i++)
            {
                fastMABuffer[i] -= slowMABuffer[i];
            }

            Array.Copy(fastMABuffer, lookbackSignal, outMacd, 0, endIdx - startIdx + 1);
            retCode = Ma(fastMABuffer, 0, outNbElement1 - 1, outMacdSignal, out _, out outNbElement2, optInSignalMAType, optInSignalPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            for (var i = 0; i < outNbElement2; i++)
            {
                outMacdHist[i] = outMacd[i] - outMacdSignal[i];
            }

            outBegIdx = startIdx;
            outNbElement = outNbElement2;

            return RetCode.Success;
        }

        public static RetCode MacdExt(decimal[] inReal, int startIdx, int endIdx, decimal[] outMacd, decimal[] outMacdSignal,
            decimal[] outMacdHist, out int outBegIdx, out int outNbElement, MAType optInFastMAType, MAType optInSlowMAType,
            MAType optInSignalMAType, int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMacd == null || outMacdSignal == null || outMacdHist == null || optInFastPeriod < 2 ||
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

            int lookbackSignal = MaLookback(optInSignalMAType, optInSignalPeriod);
            int lookbackTotal = MacdExtLookback(optInFastMAType, optInSlowMAType, optInSignalMAType, optInFastPeriod, optInSlowPeriod,
                optInSignalPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            tempInteger = endIdx - startIdx + 1 + lookbackSignal;
            var fastMABuffer = new decimal[tempInteger];
            var slowMABuffer = new decimal[tempInteger];

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = Ma(inReal, tempInteger, endIdx, slowMABuffer, out var outBegIdx1, out var outNbElement1, optInSlowMAType,
                optInSlowPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = Ma(inReal, tempInteger, endIdx, fastMABuffer, out var outBegIdx2, out var outNbElement2, optInFastMAType,
                optInFastPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
                outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
            {
                return RetCode.InternalError;
            }

            for (var i = 0; i < outNbElement1; i++)
            {
                fastMABuffer[i] -= slowMABuffer[i];
            }

            Array.Copy(fastMABuffer, lookbackSignal, outMacd, 0, endIdx - startIdx + 1);
            retCode = Ma(fastMABuffer, 0, outNbElement1 - 1, outMacdSignal, out _, out outNbElement2, optInSignalMAType, optInSignalPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            for (var i = 0; i < outNbElement2; i++)
            {
                outMacdHist[i] = outMacd[i] - outMacdSignal[i];
            }

            outBegIdx = startIdx;
            outNbElement = outNbElement2;

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
