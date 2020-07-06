namespace TALib
{
    public static partial class Core
    {
        public static RetCode Dema(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackEMA = EmaLookback(optInTimePeriod);
            int lookbackTotal = DemaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double[] firstEMA;
            if (inReal == outReal)
            {
                firstEMA = outReal;
            }
            else
            {
                int tempInt = lookbackTotal + (endIdx - startIdx) + 1;
                firstEMA = new double[tempInt];
            }

            double k = 2.0 / (optInTimePeriod + 1);
            RetCode retCode = TA_INT_EMA(inReal, startIdx - lookbackEMA, endIdx, firstEMA, out var firstEMABegIdx,
                out var firstEMANbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || firstEMANbElement == 0)
            {
                return retCode;
            }

            var secondEMA = new double[firstEMANbElement];

            retCode = TA_INT_EMA(firstEMA, 0, firstEMANbElement - 1, secondEMA, out var secondEMABegIdx,
                out var secondEMANbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || secondEMANbElement == 0)
            {
                return retCode;
            }

            int firstEMAIdx = secondEMABegIdx;
            int outIdx = default;
            while (outIdx < secondEMANbElement)
            {
                outReal[outIdx++] = 2.0 * firstEMA[firstEMAIdx++] - secondEMA[outIdx];
            }

            outBegIdx = firstEMABegIdx + secondEMABegIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Dema(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackEMA = EmaLookback(optInTimePeriod);
            int lookbackTotal = DemaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal[] firstEMA;
            if (inReal == outReal)
            {
                firstEMA = outReal;
            }
            else
            {
                int tempInt = lookbackTotal + (endIdx - startIdx) + 1;
                firstEMA = new decimal[tempInt];
            }

            decimal k = 2m / (optInTimePeriod + 1);
            RetCode retCode = TA_INT_EMA(inReal, startIdx - lookbackEMA, endIdx, firstEMA, out var firstEMABegIdx,
                out var firstEMANbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || firstEMANbElement == 0)
            {
                return retCode;
            }

            var secondEMA = new decimal[firstEMANbElement];

            retCode = TA_INT_EMA(firstEMA, 0, firstEMANbElement - 1, secondEMA, out var secondEMABegIdx,
                out var secondEMANbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || secondEMANbElement == 0)
            {
                return retCode;
            }

            int firstEMAIdx = secondEMABegIdx;
            int outIdx = default;
            while (outIdx < secondEMANbElement)
            {
                outReal[outIdx++] = 2m * firstEMA[firstEMAIdx++] - secondEMA[outIdx];
            }

            outBegIdx = firstEMABegIdx + secondEMABegIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int DemaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return EmaLookback(optInTimePeriod) * 2;
        }
    }
}
