using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MacdExt(int startIdx, int endIdx, double[] inReal, MAType optInFastMAType, MAType optInSlowMaType,
            MAType optInSignalMaType, out int outBegIdx, out int outNbElement, double[] outMacd, double[] outMacdSignal,
            double[] outMacdHist, int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
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

                MAType tempMAType = optInSlowMaType;
                optInSlowMaType = optInFastMAType;
                optInFastMAType = tempMAType;
            }

            int lookbackSignal = MaLookback(optInSignalMaType, optInSignalPeriod);
            int lookbackTotal = MacdExtLookback(optInFastMAType, optInSlowMaType, optInSignalMaType, optInFastPeriod, optInSlowPeriod,
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
            RetCode retCode = Ma(tempInteger, endIdx, inReal, optInSlowMaType, out var outBegIdx1, out var outNbElement1, slowMABuffer,
                optInSlowPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            retCode = Ma(tempInteger, endIdx, inReal, optInFastMAType, out var outBegIdx2, out var outNbElement2, fastMABuffer,
                optInFastPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = outNbElement = 0;

                return retCode;

            }

            if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
                outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
            {
                outBegIdx = outNbElement = 0;

                return RetCode.InternalError;
            }

            for (var i = 0; i < outNbElement1; i++)
            {
                fastMABuffer[i] -= slowMABuffer[i];
            }

            Array.Copy(fastMABuffer, lookbackSignal, outMacd, 0, endIdx - startIdx + 1);
            retCode = Ma(0, outNbElement1 - 1, fastMABuffer, optInSignalMaType, out _, out outNbElement2, outMacdSignal, optInSignalPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = outNbElement = 0;

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

        public static RetCode MacdExt(int startIdx, int endIdx, decimal[] inReal, MAType optInFastMAType, MAType optInSlowMaType,
            MAType optInSignalMaType, out int outBegIdx, out int outNbElement, decimal[] outMacd, decimal[] outMacdSignal,
            decimal[] outMacdHist, int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
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

                MAType tempMAType = optInSlowMaType;
                optInSlowMaType = optInFastMAType;
                optInFastMAType = tempMAType;
            }

            int lookbackSignal = MaLookback(optInSignalMaType, optInSignalPeriod);
            int lookbackTotal = MacdExtLookback(optInFastMAType, optInSlowMaType, optInSignalMaType, optInFastPeriod, optInSlowPeriod);
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
            RetCode retCode = Ma(tempInteger, endIdx, inReal, optInSlowMaType, out var outBegIdx1, out var outNbElement1, slowMABuffer,
                optInSlowPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            retCode = Ma(tempInteger, endIdx, inReal, optInFastMAType, out var outBegIdx2, out var outNbElement2, fastMABuffer,
                optInFastPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = outNbElement = 0;

                return retCode;

            }

            if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
                outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
            {
                outBegIdx = outNbElement = 0;

                return RetCode.InternalError;
            }

            for (var i = 0; i < outNbElement1; i++)
            {
                fastMABuffer[i] -= slowMABuffer[i];
            }

            Array.Copy(fastMABuffer, lookbackSignal, outMacd, 0, endIdx - startIdx + 1);
            retCode = Ma(0, outNbElement1 - 1, fastMABuffer, optInSignalMaType, out _, out outNbElement2, outMacdSignal, optInSignalPeriod);
            if (retCode != RetCode.Success)
            {
                outBegIdx = outNbElement = 0;

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

        public static int MacdExtLookback(MAType optInFastMAType, MAType optInSlowMaType, MAType optInSignalMaType,
            int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
        {
            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000 ||
                optInSignalPeriod < 1 || optInSignalPeriod > 100000)
            {
                return -1;
            }

            int lookbackLargest = MaLookback(optInFastMAType, optInFastPeriod);
            int tempInteger = MaLookback(optInSlowMaType, optInSlowPeriod);
            if (tempInteger > lookbackLargest)
            {
                lookbackLargest = tempInteger;
            }

            return lookbackLargest + MaLookback(optInSignalMaType, optInSignalPeriod);
        }
    }
}
