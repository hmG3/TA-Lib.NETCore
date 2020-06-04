namespace TALib
{
    public partial class Core
    {
        public static RetCode Dema(int startIdx, int endIdx, double[] inReal, out int outBegIdx, out int outNbElement, double[] outReal,
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
            RetCode retCode = TA_INT_EMA(startIdx - lookbackEMA, endIdx, inReal, optInTimePeriod, k, out var firstEMABegIdx,
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

            int firstEMAIdx = secondEMABegIdx;
            int outIdx = default;
            while (outIdx < secondEMANbElement)
            {
                outReal[outIdx] = 2.0 * firstEMA[firstEMAIdx++] - secondEMA[outIdx];
                outIdx++;
            }

            outBegIdx = firstEMABegIdx + secondEMABegIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Dema(int startIdx, int endIdx, decimal[] inReal, out int outBegIdx, out int outNbElement, decimal[] outReal,
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

            outNbElement = 0;
            outBegIdx = 0;
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
            RetCode retCode = TA_INT_EMA(startIdx - lookbackEMA, endIdx, inReal, optInTimePeriod, k, out var firstEMABegIdx,
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

            int firstEMAIdx = secondEMABegIdx;
            int outIdx = default;
            while (outIdx < secondEMANbElement)
            {
                outReal[outIdx] = 2m * firstEMA[firstEMAIdx++] - secondEMA[outIdx];
                outIdx++;
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
