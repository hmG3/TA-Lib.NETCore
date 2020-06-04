using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cci(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, out int outBegIdx,
            out int outNbElement, double[] outReal, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CciLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var circBuffer = new double[optInTimePeriod];
            int circBufferIdx = default;
            var maxIdxCircBuffer = optInTimePeriod - 1;
            int i = startIdx - lookbackTotal;
            while (i < startIdx)
            {
                circBuffer[circBufferIdx++] = (inHigh[i] + inLow[i] + inClose[i]) / 3.0;
                i++;
                if (circBufferIdx > maxIdxCircBuffer)
                {
                    circBufferIdx = 0;
                }
            }

            int outIdx = default;
            do
            {
                double lastValue = (inHigh[i] + inLow[i] + inClose[i]) / 3.0;
                circBuffer[circBufferIdx++] = lastValue;

                int j;
                double theAverage = default;
                for (j = 0; j < optInTimePeriod; j++)
                {
                    theAverage += circBuffer[j];
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
                    outReal[outIdx++] = tempReal / (0.015 * (tempReal2 / optInTimePeriod));
                }
                else
                {
                    outReal[outIdx++] = 0.0;
                }

                if (circBufferIdx > maxIdxCircBuffer)
                {
                    circBufferIdx = 0;
                }

                i++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode Cci(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, out int outBegIdx,
            out int outNbElement, decimal[] outReal, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CciLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            var circBuffer = new decimal[optInTimePeriod];
            int circBufferIdx = default;
            var maxIdxCircBuffer = optInTimePeriod - 1;
            int i = startIdx - lookbackTotal;
            while (i < startIdx)
            {
                circBuffer[circBufferIdx++] = (inHigh[i] + inLow[i] + inClose[i]) / 3m;
                i++;
                if (circBufferIdx > maxIdxCircBuffer)
                {
                    circBufferIdx = 0;
                }
            }

            int outIdx = default;
            do
            {
                decimal lastValue = (inHigh[i] + inLow[i] + inClose[i]) / 3m;
                circBuffer[circBufferIdx++] = lastValue;

                int j;
                decimal theAverage = default;
                for (j = 0; j < optInTimePeriod; j++)
                {
                    theAverage += circBuffer[j];
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
                    outReal[outIdx++] = tempReal / (0.015m * (tempReal2 / optInTimePeriod));
                }
                else
                {
                    outReal[outIdx++] = Decimal.Zero;
                }

                if (circBufferIdx > maxIdxCircBuffer)
                {
                    circBufferIdx = 0;
                }

                i++;
            } while (i <= endIdx);

            outNbElement = outIdx;
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
