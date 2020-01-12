using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MovingAverageVariablePeriod(int startIdx, int endIdx, double[] inReal, double[] inPeriods, int optInMinPeriod,
            int optInMaxPeriod, MAType optInMAType, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int i;
            int tempInt = 0;
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

            if (inPeriods == null)
            {
                return RetCode.BadParam;
            }

            if (optInMinPeriod == -2147483648)
            {
                optInMinPeriod = 2;
            }
            else if ((optInMinPeriod < 2) || (optInMinPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInMaxPeriod == -2147483648)
            {
                optInMaxPeriod = 30;
            }
            else if ((optInMaxPeriod < 2) || (optInMaxPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MovingAverageLookback(optInMaxPeriod, optInMAType);
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

            if (lookbackTotal > startIdx)
            {
                tempInt = lookbackTotal;
            }
            else
            {
                tempInt = startIdx;
            }

            if (tempInt > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outputSize = (endIdx - tempInt) + 1;
            double[] localOutputArray = new double[outputSize];
            int[] localPeriodArray = new int[outputSize];
            for (i = 0; i < outputSize; i++)
            {
                tempInt = (int) inPeriods[startIdx + i];
                if (tempInt < optInMinPeriod)
                {
                    tempInt = optInMinPeriod;
                }
                else if (tempInt > optInMaxPeriod)
                {
                    tempInt = optInMaxPeriod;
                }

                localPeriodArray[i] = tempInt;
            }

            i = 0;
            while (true)
            {
                if (i >= outputSize)
                {
                    outBegIdx = startIdx;
                    outNBElement = outputSize;
                    return RetCode.Success;
                }

                int curPeriod = localPeriodArray[i];
                if (curPeriod != 0)
                {
                    int localNbElement = 0;
                    int localBegIdx = 0;
                    RetCode retCode = MovingAverage(startIdx, endIdx, inReal, curPeriod, optInMAType, ref localBegIdx, ref localNbElement,
                        localOutputArray);
                    if (retCode != RetCode.Success)
                    {
                        outBegIdx = 0;
                        outNBElement = 0;
                        return retCode;
                    }

                    outReal[i] = localOutputArray[i];
                    for (int j = i + 1; j < outputSize; j++)
                    {
                        if (localPeriodArray[j] == curPeriod)
                        {
                            localPeriodArray[j] = 0;
                            outReal[j] = localOutputArray[j];
                        }
                    }
                }

                i++;
            }
        }

        public static RetCode MovingAverageVariablePeriod(int startIdx, int endIdx, float[] inReal, float[] inPeriods, int optInMinPeriod,
            int optInMaxPeriod, MAType optInMAType, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int i;
            int tempInt = 0;
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

            if (inPeriods == null)
            {
                return RetCode.BadParam;
            }

            if (optInMinPeriod == -2147483648)
            {
                optInMinPeriod = 2;
            }
            else if ((optInMinPeriod < 2) || (optInMinPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInMaxPeriod == -2147483648)
            {
                optInMaxPeriod = 30;
            }
            else if ((optInMaxPeriod < 2) || (optInMaxPeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MovingAverageLookback(optInMaxPeriod, optInMAType);
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

            if (lookbackTotal > startIdx)
            {
                tempInt = lookbackTotal;
            }
            else
            {
                tempInt = startIdx;
            }

            if (tempInt > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outputSize = (endIdx - tempInt) + 1;
            double[] localOutputArray = new double[outputSize];
            int[] localPeriodArray = new int[outputSize];
            for (i = 0; i < outputSize; i++)
            {
                tempInt = (int) inPeriods[startIdx + i];
                if (tempInt < optInMinPeriod)
                {
                    tempInt = optInMinPeriod;
                }
                else if (tempInt > optInMaxPeriod)
                {
                    tempInt = optInMaxPeriod;
                }

                localPeriodArray[i] = tempInt;
            }

            i = 0;
            while (true)
            {
                if (i >= outputSize)
                {
                    outBegIdx = startIdx;
                    outNBElement = outputSize;
                    return RetCode.Success;
                }

                int curPeriod = localPeriodArray[i];
                if (curPeriod != 0)
                {
                    int localNbElement = 0;
                    int localBegIdx = 0;
                    RetCode retCode = MovingAverage(startIdx, endIdx, inReal, curPeriod, optInMAType, ref localBegIdx, ref localNbElement,
                        localOutputArray);
                    if (retCode != RetCode.Success)
                    {
                        outBegIdx = 0;
                        outNBElement = 0;
                        return retCode;
                    }

                    outReal[i] = localOutputArray[i];
                    for (int j = i + 1; j < outputSize; j++)
                    {
                        if (localPeriodArray[j] == curPeriod)
                        {
                            localPeriodArray[j] = 0;
                            outReal[j] = localOutputArray[j];
                        }
                    }
                }

                i++;
            }
        }

        public static int MovingAverageVariablePeriodLookback(int optInMinPeriod, int optInMaxPeriod, MAType optInMAType)
        {
            if (optInMinPeriod == -2147483648)
            {
                optInMinPeriod = 2;
            }
            else if ((optInMinPeriod < 2) || (optInMinPeriod > 0x186a0))
            {
                return -1;
            }

            if (optInMaxPeriod == -2147483648)
            {
                optInMaxPeriod = 30;
            }
            else if ((optInMaxPeriod < 2) || (optInMaxPeriod > 0x186a0))
            {
                return -1;
            }

            return MovingAverageLookback(optInMaxPeriod, optInMAType);
        }
    }
}
