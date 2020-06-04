namespace TALib
{
    public partial class Core
    {
        public static RetCode Tema(int startIdx, int endIdx, double[] inReal, out int outBegIdx, out int outNbElement, double[] outReal,
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
            int lookbackTotal = TemaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int tempInt = lookbackTotal + (endIdx - startIdx) + 1;
            double k = 2.0 / (optInTimePeriod + 1);

            var firstEMA = new double[tempInt];
            RetCode retCode = TA_INT_EMA(startIdx - lookbackEMA * 2, endIdx, inReal, optInTimePeriod, k, out var firstEMABegIdx,
                out var firstEMANbElement, firstEMA);
            if (retCode != RetCode.Success || firstEMANbElement == 0)
            {
                return retCode;
            }

            var secondEMA = new double[firstEMANbElement];
            retCode = TA_INT_EMA(0, firstEMANbElement - 1, firstEMA, optInTimePeriod, k, out var secondEMABegIdx,
                out var secondEMANbElement, secondEMA);
            if (retCode != RetCode.Success || secondEMANbElement == 0)
            {
                return retCode;
            }

            retCode = TA_INT_EMA(0, secondEMANbElement - 1, secondEMA, optInTimePeriod, k, out var thirdEMABegIdx,
                out var thirdEMANbElement, outReal);
            if (retCode != RetCode.Success || thirdEMANbElement == 0)
            {
                return retCode;
            }

            int firstEMAIdx = thirdEMABegIdx + secondEMABegIdx;
            int secondEMAIdx = thirdEMABegIdx;
            outBegIdx = firstEMAIdx + firstEMABegIdx;
            int outIdx = default;
            while (outIdx < thirdEMANbElement)
            {
                outReal[outIdx++] += 3.0 * firstEMA[firstEMAIdx++] - 3.0 * secondEMA[secondEMAIdx++];
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Tema(int startIdx, int endIdx, decimal[] inReal, out int outBegIdx, out int outNbElement, decimal[] outReal,
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
            int lookbackTotal = TemaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int tempInt = lookbackTotal + (endIdx - startIdx) + 1;
            decimal k = 2m / (optInTimePeriod + 1);

            var firstEMA = new decimal[tempInt];
            RetCode retCode = TA_INT_EMA(startIdx - lookbackEMA * 2, endIdx, inReal, optInTimePeriod, k, out var firstEMABegIdx,
                out var firstEMANbElement, firstEMA);
            if (retCode != RetCode.Success || firstEMANbElement == 0)
            {
                return retCode;
            }

            var secondEMA = new decimal[firstEMANbElement];
            retCode = TA_INT_EMA(0, firstEMANbElement - 1, firstEMA, optInTimePeriod, k, out var secondEMABegIdx,
                out var secondEMANbElement, secondEMA);
            if (retCode != RetCode.Success || secondEMANbElement == 0)
            {
                return retCode;
            }

            retCode = TA_INT_EMA(0, secondEMANbElement - 1, secondEMA, optInTimePeriod, k, out var thirdEMABegIdx,
                out var thirdEMANbElement, outReal);
            if (retCode != RetCode.Success || thirdEMANbElement == 0)
            {
                return retCode;
            }

            int firstEMAIdx = thirdEMABegIdx + secondEMABegIdx;
            int secondEMAIdx = thirdEMABegIdx;
            outBegIdx = firstEMAIdx + firstEMABegIdx;
            int outIdx = default;
            while (outIdx < thirdEMANbElement)
            {
                outReal[outIdx++] += 3m * firstEMA[firstEMAIdx++] - 3m * secondEMA[secondEMAIdx++];
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int TemaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return EmaLookback(optInTimePeriod) * 3;
        }
    }
}
