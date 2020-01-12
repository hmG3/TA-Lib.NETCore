using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Dema(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
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

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outNBElement = 0;
            outBegIdx = 0;
            int lookbackEMA = EmaLookback(optInTimePeriod);
            int lookbackTotal = lookbackEMA * 2;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx <= endIdx)
            {
                double[] firstEMA;
                int firstEMANbElement = 0;
                int secondEMANbElement = 0;
                int secondEMABegIdx = 0;
                int firstEMABegIdx = 0;
                if (inReal == outReal)
                {
                    firstEMA = outReal;
                }
                else
                {
                    int tempInt = (lookbackTotal + (endIdx - startIdx)) + 1;
                    firstEMA = new double[tempInt];
                    if (firstEMA == null)
                    {
                        return RetCode.AllocErr;
                    }
                }

                double k = 2.0 / ((double) (optInTimePeriod + 1));
                RetCode retCode = TA_INT_EMA(startIdx - lookbackEMA, endIdx, inReal, optInTimePeriod, k, ref firstEMABegIdx,
                    ref firstEMANbElement, firstEMA);
                if ((retCode != RetCode.Success) || (firstEMANbElement == 0))
                {
                    return retCode;
                }

                double[] secondEMA = new double[firstEMANbElement];
                if (secondEMA == null)
                {
                    return RetCode.AllocErr;
                }

                retCode = TA_INT_EMA(0, firstEMANbElement - 1, firstEMA, optInTimePeriod, k, ref secondEMABegIdx, ref secondEMANbElement,
                    secondEMA);
                if ((retCode != RetCode.Success) || (secondEMANbElement == 0))
                {
                    return retCode;
                }

                int firstEMAIdx = secondEMABegIdx;
                int outIdx = 0;
                while (true)
                {
                    if (outIdx >= secondEMANbElement)
                    {
                        break;
                    }

                    outReal[outIdx] = (2.0 * firstEMA[firstEMAIdx]) - secondEMA[outIdx];
                    firstEMAIdx++;
                    outIdx++;
                }

                outBegIdx = firstEMABegIdx + secondEMABegIdx;
                outNBElement = outIdx;
            }

            return RetCode.Success;
        }

        public static RetCode Dema(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
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

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outNBElement = 0;
            outBegIdx = 0;
            int lookbackEMA = EmaLookback(optInTimePeriod);
            int lookbackTotal = lookbackEMA * 2;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx <= endIdx)
            {
                int firstEMANbElement = 0;
                int secondEMANbElement = 0;
                int secondEMABegIdx = 0;
                int firstEMABegIdx = 0;
                int tempInt = (lookbackTotal + (endIdx - startIdx)) + 1;
                double[] firstEMA = new double[tempInt];
                if (firstEMA == null)
                {
                    return RetCode.AllocErr;
                }

                double k = 2.0 / ((double) (optInTimePeriod + 1));
                RetCode retCode = TA_INT_EMA(startIdx - lookbackEMA, endIdx, inReal, optInTimePeriod, k, ref firstEMABegIdx,
                    ref firstEMANbElement, firstEMA);
                if ((retCode != RetCode.Success) || (firstEMANbElement == 0))
                {
                    return retCode;
                }

                double[] secondEMA = new double[firstEMANbElement];
                if (secondEMA == null)
                {
                    return RetCode.AllocErr;
                }

                retCode = TA_INT_EMA(0, firstEMANbElement - 1, firstEMA, optInTimePeriod, k, ref secondEMABegIdx, ref secondEMANbElement,
                    secondEMA);
                if ((retCode != RetCode.Success) || (secondEMANbElement == 0))
                {
                    return retCode;
                }

                int firstEMAIdx = secondEMABegIdx;
                int outIdx = 0;
                while (true)
                {
                    if (outIdx >= secondEMANbElement)
                    {
                        break;
                    }

                    outReal[outIdx] = (2.0 * firstEMA[firstEMAIdx]) - secondEMA[outIdx];
                    firstEMAIdx++;
                    outIdx++;
                }

                outBegIdx = firstEMABegIdx + secondEMABegIdx;
                outNBElement = outIdx;
            }

            return RetCode.Success;
        }

        public static int DemaLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return (EmaLookback(optInTimePeriod) * 2);
        }
    }
}
