namespace TALib
{
    public partial class Core
    {
        public static RetCode Dema(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 30)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
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
                int firstEMANbElement = default;
                int secondEMANbElement = default;
                int secondEMABegIdx = default;
                int firstEMABegIdx = default;
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
                RetCode retCode = TA_INT_EMA(startIdx - lookbackEMA, endIdx, inReal, optInTimePeriod, k, ref firstEMABegIdx,
                    ref firstEMANbElement, firstEMA);
                if (retCode != RetCode.Success || firstEMANbElement == 0)
                {
                    return retCode;
                }

                var secondEMA = new double[firstEMANbElement];

                retCode = TA_INT_EMA(0, firstEMANbElement - 1, firstEMA, optInTimePeriod, k, ref secondEMABegIdx,
                    ref secondEMANbElement, secondEMA);
                if (retCode != RetCode.Success || secondEMANbElement == 0)
                {
                    return retCode;
                }

                int firstEMAIdx = secondEMABegIdx;
                int outIdx = default;
                while (true)
                {
                    if (outIdx >= secondEMANbElement)
                    {
                        break;
                    }

                    outReal[outIdx] = 2.0 * firstEMA[firstEMAIdx] - secondEMA[outIdx];
                    firstEMAIdx++;
                    outIdx++;
                }

                outBegIdx = firstEMABegIdx + secondEMABegIdx;
                outNBElement = outIdx;
            }

            return RetCode.Success;
        }

        public static RetCode Dema(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 30)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
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
                int firstEMANbElement = default;
                int secondEMANbElement = default;
                int secondEMABegIdx = default;
                int firstEMABegIdx = default;
                int tempInt = lookbackTotal + (endIdx - startIdx) + 1;
                var firstEMA = new decimal[tempInt];

                decimal k = 2m / (optInTimePeriod + 1);
                RetCode retCode = TA_INT_EMA(startIdx - lookbackEMA, endIdx, inReal, optInTimePeriod, k, ref firstEMABegIdx,
                    ref firstEMANbElement, firstEMA);
                if (retCode != RetCode.Success || firstEMANbElement == 0)
                {
                    return retCode;
                }

                var secondEMA = new decimal[firstEMANbElement];

                retCode = TA_INT_EMA(0, firstEMANbElement - 1, firstEMA, optInTimePeriod, k, ref secondEMABegIdx,
                    ref secondEMANbElement, secondEMA);
                if (retCode != RetCode.Success || secondEMANbElement == 0)
                {
                    return retCode;
                }

                int firstEMAIdx = secondEMABegIdx;
                int outIdx = default;
                while (true)
                {
                    if (outIdx >= secondEMANbElement)
                    {
                        break;
                    }

                    outReal[outIdx] = 2m * firstEMA[firstEMAIdx] - secondEMA[outIdx];
                    firstEMAIdx++;
                    outIdx++;
                }

                outBegIdx = firstEMABegIdx + secondEMABegIdx;
                outNBElement = outIdx;
            }

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
