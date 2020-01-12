using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MacdExt(int startIdx, int endIdx, double[] inReal, int optInFastPeriod, MAType optInFastMAType,
            int optInSlowPeriod, MAType optInSlowMAType, int optInSignalPeriod, MAType optInSignalMAType, ref int outBegIdx,
            ref int outNBElement, double[] outMACD, double[] outMACDSignal, double[] outMACDHist)
        {
            int i;
            int tempInteger = 0;
            int outNbElement1 = 0;
            int outNbElement2 = 0;
            int outBegIdx2 = 0;
            int outBegIdx1 = 0;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 12;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 0x1a;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSignalPeriod == -2147483648)
            {
                optInSignalPeriod = 9;
            }
            else if ((optInSignalPeriod < 1) || (optInSignalPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outMACD == null)
            {
                return RetCode.BadParam;
            }

            if (outMACDSignal == null)
            {
                return RetCode.BadParam;
            }

            if (outMACDHist == null)
            {
                return RetCode.BadParam;
            }

            if (optInSlowPeriod < optInFastPeriod)
            {
                tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;
                MAType tempMAType = optInSlowMAType;
                optInSlowMAType = optInFastMAType;
                optInFastMAType = tempMAType;
            }

            int lookbackLargest = MovingAverageLookback(optInFastPeriod, optInFastMAType);
            tempInteger = MovingAverageLookback(optInSlowPeriod, optInSlowMAType);
            if (tempInteger > lookbackLargest)
            {
                lookbackLargest = tempInteger;
            }

            int lookbackSignal = MovingAverageLookback(optInSignalPeriod, optInSignalMAType);
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

            tempInteger = ((endIdx - startIdx) + 1) + lookbackSignal;
            double[] fastMABuffer = new double[tempInteger];
            if (fastMABuffer == null)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.AllocErr;
            }

            double[] slowMABuffer = new double[tempInteger];
            if (slowMABuffer == null)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.AllocErr;
            }

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = MovingAverage(tempInteger, endIdx, inReal, optInSlowPeriod, optInSlowMAType, ref outBegIdx1,
                ref outNbElement1, slowMABuffer);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            retCode = MovingAverage(tempInteger, endIdx, inReal, optInFastPeriod, optInFastMAType, ref outBegIdx2, ref outNbElement2,
                fastMABuffer);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            if (((outBegIdx1 != tempInteger) || (outBegIdx2 != tempInteger)) ||
                ((outNbElement1 != outNbElement2) || (outNbElement1 != (((endIdx - startIdx) + 1) + lookbackSignal))))
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.InternalError;
            }

            for (i = 0; i < outNbElement1; i++)
            {
                fastMABuffer[i] -= slowMABuffer[i];
            }

            Array.Copy(fastMABuffer, lookbackSignal, outMACD, 0, (endIdx - startIdx) + 1);
            retCode = MovingAverage(0, outNbElement1 - 1, fastMABuffer, optInSignalPeriod, optInSignalMAType, ref outBegIdx2,
                ref outNbElement2, outMACDSignal);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            for (i = 0; i < outNbElement2; i++)
            {
                outMACDHist[i] = outMACD[i] - outMACDSignal[i];
            }

            outBegIdx = startIdx;
            outNBElement = outNbElement2;
            return RetCode.Success;
        }

        public static RetCode MacdExt(int startIdx, int endIdx, float[] inReal, int optInFastPeriod, MAType optInFastMAType,
            int optInSlowPeriod, MAType optInSlowMAType, int optInSignalPeriod, MAType optInSignalMAType, ref int outBegIdx,
            ref int outNBElement, double[] outMACD, double[] outMACDSignal, double[] outMACDHist)
        {
            int i;
            int tempInteger = 0;
            int outNbElement1 = 0;
            int outNbElement2 = 0;
            int outBegIdx2 = 0;
            int outBegIdx1 = 0;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 12;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 0x1a;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSignalPeriod == -2147483648)
            {
                optInSignalPeriod = 9;
            }
            else if ((optInSignalPeriod < 1) || (optInSignalPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outMACD == null)
            {
                return RetCode.BadParam;
            }

            if (outMACDSignal == null)
            {
                return RetCode.BadParam;
            }

            if (outMACDHist == null)
            {
                return RetCode.BadParam;
            }

            if (optInSlowPeriod < optInFastPeriod)
            {
                tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;
                MAType tempMAType = optInSlowMAType;
                optInSlowMAType = optInFastMAType;
                optInFastMAType = tempMAType;
            }

            int lookbackLargest = MovingAverageLookback(optInFastPeriod, optInFastMAType);
            tempInteger = MovingAverageLookback(optInSlowPeriod, optInSlowMAType);
            if (tempInteger > lookbackLargest)
            {
                lookbackLargest = tempInteger;
            }

            int lookbackSignal = MovingAverageLookback(optInSignalPeriod, optInSignalMAType);
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

            tempInteger = ((endIdx - startIdx) + 1) + lookbackSignal;
            double[] fastMABuffer = new double[tempInteger];
            if (fastMABuffer == null)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.AllocErr;
            }

            double[] slowMABuffer = new double[tempInteger];
            if (slowMABuffer == null)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.AllocErr;
            }

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = MovingAverage(tempInteger, endIdx, inReal, optInSlowPeriod, optInSlowMAType, ref outBegIdx1,
                ref outNbElement1, slowMABuffer);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            retCode = MovingAverage(tempInteger, endIdx, inReal, optInFastPeriod, optInFastMAType, ref outBegIdx2, ref outNbElement2,
                fastMABuffer);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            if (((outBegIdx1 != tempInteger) || (outBegIdx2 != tempInteger)) ||
                ((outNbElement1 != outNbElement2) || (outNbElement1 != (((endIdx - startIdx) + 1) + lookbackSignal))))
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.InternalError;
            }

            for (i = 0; i < outNbElement1; i++)
            {
                fastMABuffer[i] -= slowMABuffer[i];
            }

            Array.Copy(fastMABuffer, lookbackSignal, outMACD, 0, (endIdx - startIdx) + 1);
            retCode = MovingAverage(0, outNbElement1 - 1, fastMABuffer, optInSignalPeriod, optInSignalMAType, ref outBegIdx2,
                ref outNbElement2, outMACDSignal);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            for (i = 0; i < outNbElement2; i++)
            {
                outMACDHist[i] = outMACD[i] - outMACDSignal[i];
            }

            outBegIdx = startIdx;
            outNBElement = outNbElement2;
            return RetCode.Success;
        }

        public static int MacdExtLookback(int optInFastPeriod, MAType optInFastMAType, int optInSlowPeriod, MAType optInSlowMAType,
            int optInSignalPeriod, MAType optInSignalMAType)
        {
            if (optInFastPeriod == -2147483648)
            {
                optInFastPeriod = 12;
            }
            else if ((optInFastPeriod < 2) || (optInFastPeriod > 0x186a0))
            {
                return -1;
            }

            if (optInSlowPeriod == -2147483648)
            {
                optInSlowPeriod = 0x1a;
            }
            else if ((optInSlowPeriod < 2) || (optInSlowPeriod > 0x186a0))
            {
                return -1;
            }

            if (optInSignalPeriod == -2147483648)
            {
                optInSignalPeriod = 9;
            }
            else if ((optInSignalPeriod < 1) || (optInSignalPeriod > 0x186a0))
            {
                return -1;
            }

            int lookbackLargest = MovingAverageLookback(optInFastPeriod, optInFastMAType);
            int tempInteger = MovingAverageLookback(optInSlowPeriod, optInSlowMAType);
            if (tempInteger > lookbackLargest)
            {
                lookbackLargest = tempInteger;
            }

            return (lookbackLargest + MovingAverageLookback(optInSignalPeriod, optInSignalMAType));
        }
    }
}
