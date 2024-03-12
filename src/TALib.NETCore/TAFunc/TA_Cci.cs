namespace TALib
{
    public static partial class Core
    {
        public static RetCode Cci(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outReal,
            out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
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

                double theAverage = default;
                for (var j = 0; j < optInTimePeriod; j++)
                {
                    theAverage += circBuffer[j];
                }
                theAverage /= optInTimePeriod;

                double tempReal2 = default;
                for (var j = 0; j < optInTimePeriod; j++)
                {
                    tempReal2 += Math.Abs(circBuffer[j] - theAverage);
                }

                double tempReal = lastValue - theAverage;
                outReal[outIdx++] = !tempReal.Equals(0.0) && !tempReal2.Equals(0.0)
                    ? tempReal / (0.015 * (tempReal2 / optInTimePeriod))
                    : 0.0;

                if (circBufferIdx > maxIdxCircBuffer)
                {
                    circBufferIdx = 0;
                }

                i++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Cci(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outReal,
            out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
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

                decimal theAverage = default;
                for (var j = 0; j < optInTimePeriod; j++)
                {
                    theAverage += circBuffer[j];
                }

                theAverage /= optInTimePeriod;

                decimal tempReal2 = default;
                for (var j = 0; j < optInTimePeriod; j++)
                {
                    tempReal2 += Math.Abs(circBuffer[j] - theAverage);
                }

                decimal tempReal = lastValue - theAverage;
                outReal[outIdx++] = tempReal != Decimal.Zero && tempReal2 != Decimal.Zero
                    ? tempReal / (0.015m * (tempReal2 / optInTimePeriod))
                    : Decimal.Zero;

                if (circBufferIdx > maxIdxCircBuffer)
                {
                    circBufferIdx = 0;
                }

                i++;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

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
