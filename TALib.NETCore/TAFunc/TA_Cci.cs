using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cci(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal, int optInTimePeriod = 14)
        {
            int circBufferIdx = default;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
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

            var circBuffer = new double[optInTimePeriod];

            var maxIdxCircBuffer = optInTimePeriod - 1;
            int i = startIdx - lookbackTotal;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    circBuffer[circBufferIdx] = (inHigh[i] + inLow[i] + inClose[i]) / 3.0;
                    i++;
                    circBufferIdx++;
                    if (circBufferIdx > maxIdxCircBuffer)
                    {
                        circBufferIdx = 0;
                    }
                }
            }

            int outIdx = default;
            do
            {
                double lastValue = (inHigh[i] + inLow[i] + inClose[i]) / 3.0;
                circBuffer[circBufferIdx] = lastValue;
                double theAverage = default;
                int j = default;
                while (j < optInTimePeriod)
                {
                    theAverage += circBuffer[j];
                    j++;
                }

                theAverage /= optInTimePeriod;
                double tempReal2 = default;
                for (j = 0; j < optInTimePeriod; j++)
                {
                    tempReal2 += Math.Abs(circBuffer[j] - theAverage);
                }

                double tempReal = lastValue - theAverage;
                if (!tempReal.Equals(0.0) && !tempReal2.Equals(0.0))
                {
                    outReal[outIdx] = tempReal / (0.015 * (tempReal2 / optInTimePeriod));
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                circBufferIdx++;
                if (circBufferIdx > maxIdxCircBuffer)
                {
                    circBufferIdx = 0;
                }

                i++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cci(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, ref int outBegIdx,
            ref int outNBElement, decimal[] outReal, int optInTimePeriod = 14)
        {
            int circBufferIdx = default;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
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

            var circBuffer = new decimal[optInTimePeriod];

            var maxIdxCircBuffer = optInTimePeriod - 1;
            int i = startIdx - lookbackTotal;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    circBuffer[circBufferIdx] = (inHigh[i] + inLow[i] + inClose[i]) / 3m;
                    i++;
                    circBufferIdx++;
                    if (circBufferIdx > maxIdxCircBuffer)
                    {
                        circBufferIdx = 0;
                    }
                }
            }

            int outIdx = default;
            do
            {
                decimal lastValue = (inHigh[i] + inLow[i] + inClose[i]) / 3m;
                circBuffer[circBufferIdx] = lastValue;
                decimal theAverage = default;
                int j = default;
                while (j < optInTimePeriod)
                {
                    theAverage += circBuffer[j];
                    j++;
                }

                theAverage /= optInTimePeriod;
                decimal tempReal2 = default;
                for (j = 0; j < optInTimePeriod; j++)
                {
                    tempReal2 += Math.Abs(circBuffer[j] - theAverage);
                }

                decimal tempReal = lastValue - theAverage;
                if (tempReal != Decimal.Zero && tempReal2 != Decimal.Zero)
                {
                    outReal[outIdx] = tempReal / (0.015m * (tempReal2 / optInTimePeriod));
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = Decimal.Zero;
                    outIdx++;
                }

                circBufferIdx++;
                if (circBufferIdx > maxIdxCircBuffer)
                {
                    circBufferIdx = 0;
                }

                i++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CciLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
