using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cci(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int circBuffer_Idx = 0;
            int maxIdx_circBuffer = 0x1d;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = optInTimePeriod - 1;
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

            if (optInTimePeriod <= 0)
            {
                return RetCode.AllocErr;
            }

            double[] circBuffer = new double[optInTimePeriod];
            if (circBuffer == null)
            {
                return RetCode.AllocErr;
            }

            maxIdx_circBuffer = optInTimePeriod - 1;
            int i = startIdx - lookbackTotal;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    circBuffer[circBuffer_Idx] = ((inHigh[i] + inLow[i]) + inClose[i]) / 3.0;
                    i++;
                    circBuffer_Idx++;
                    if (circBuffer_Idx > maxIdx_circBuffer)
                    {
                        circBuffer_Idx = 0;
                    }
                }
            }

            int outIdx = 0;
            do
            {
                double lastValue = ((inHigh[i] + inLow[i]) + inClose[i]) / 3.0;
                circBuffer[circBuffer_Idx] = lastValue;
                double theAverage = 0.0;
                int j = 0;
                while (j < optInTimePeriod)
                {
                    theAverage += circBuffer[j];
                    j++;
                }

                theAverage /= (double) optInTimePeriod;
                double tempReal2 = 0.0;
                for (j = 0; j < optInTimePeriod; j++)
                {
                    tempReal2 += Math.Abs((double) (circBuffer[j] - theAverage));
                }

                double tempReal = lastValue - theAverage;
                if ((tempReal != 0.0) && (tempReal2 != 0.0))
                {
                    outReal[outIdx] = tempReal / (0.015 * (tempReal2 / ((double) optInTimePeriod)));
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                circBuffer_Idx++;
                if (circBuffer_Idx > maxIdx_circBuffer)
                {
                    circBuffer_Idx = 0;
                }

                i++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cci(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int circBuffer_Idx = 0;
            int maxIdx_circBuffer = 0x1d;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = optInTimePeriod - 1;
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

            if (optInTimePeriod <= 0)
            {
                return RetCode.AllocErr;
            }

            double[] circBuffer = new double[optInTimePeriod];
            if (circBuffer == null)
            {
                return RetCode.AllocErr;
            }

            maxIdx_circBuffer = optInTimePeriod - 1;
            int i = startIdx - lookbackTotal;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    circBuffer[circBuffer_Idx] = ((inHigh[i] + inLow[i]) + inClose[i]) / 3.0;
                    i++;
                    circBuffer_Idx++;
                    if (circBuffer_Idx > maxIdx_circBuffer)
                    {
                        circBuffer_Idx = 0;
                    }
                }
            }

            int outIdx = 0;
            do
            {
                double lastValue = ((inHigh[i] + inLow[i]) + inClose[i]) / 3.0;
                circBuffer[circBuffer_Idx] = lastValue;
                double theAverage = 0.0;
                int j = 0;
                while (j < optInTimePeriod)
                {
                    theAverage += circBuffer[j];
                    j++;
                }

                theAverage /= (double) optInTimePeriod;
                double tempReal2 = 0.0;
                for (j = 0; j < optInTimePeriod; j++)
                {
                    tempReal2 += Math.Abs((double) (circBuffer[j] - theAverage));
                }

                double tempReal = lastValue - theAverage;
                if ((tempReal != 0.0) && (tempReal2 != 0.0))
                {
                    outReal[outIdx] = tempReal / (0.015 * (tempReal2 / ((double) optInTimePeriod)));
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                circBuffer_Idx++;
                if (circBuffer_Idx > maxIdx_circBuffer)
                {
                    circBuffer_Idx = 0;
                }

                i++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CciLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return (optInTimePeriod - 1);
        }
    }
}
