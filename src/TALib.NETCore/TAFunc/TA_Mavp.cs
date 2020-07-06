namespace TALib
{
    public static partial class Core
    {
        public static RetCode Mavp(double[] inReal, double[] inPeriods, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, MAType optInMAType, int optInMinPeriod = 2, int optInMaxPeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || inPeriods == null || outReal == null || optInMinPeriod < 2 || optInMinPeriod > 100000 ||
                optInMaxPeriod < 2 || optInMaxPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MavpLookback(optInMAType, optInMaxPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var tempInt = lookbackTotal > startIdx ? lookbackTotal : startIdx;
            if (tempInt > endIdx)
            {
                return RetCode.Success;
            }

            int outputSize = endIdx - tempInt + 1;
            var localOutputArray = new double[outputSize];
            int[] localPeriodArray = new int[outputSize];
            for (var i = 0; i < outputSize; i++)
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

            for (var i = 0; i < outputSize; i++)
            {
                int curPeriod = localPeriodArray[i];
                if (curPeriod != 0)
                {
                    RetCode retCode = Ma(inReal, startIdx, endIdx, localOutputArray, out _, out _, optInMAType, curPeriod);
                    if (retCode != RetCode.Success)
                    {
                        return retCode;
                    }

                    outReal[i] = localOutputArray[i];
                    for (var j = i + 1; j < outputSize; j++)
                    {
                        if (localPeriodArray[j] == curPeriod)
                        {
                            localPeriodArray[j] = 0;
                            outReal[j] = localOutputArray[j];
                        }
                    }
                }
            }

            outBegIdx = startIdx;
            outNbElement = outputSize;

            return RetCode.Success;
        }

        public static RetCode Mavp(decimal[] inReal, decimal[] inPeriods, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, MAType optInMAType, int optInMinPeriod = 2, int optInMaxPeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || inPeriods == null || outReal == null || optInMinPeriod < 2 || optInMinPeriod > 100000 ||
                optInMaxPeriod < 2 || optInMaxPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MavpLookback(optInMAType, optInMaxPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            var tempInt = lookbackTotal > startIdx ? lookbackTotal : startIdx;
            if (tempInt > endIdx)
            {
                return RetCode.Success;
            }

            int outputSize = endIdx - tempInt + 1;
            var localOutputArray = new decimal[outputSize];
            int[] localPeriodArray = new int[outputSize];
            for (var i = 0; i < outputSize; i++)
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

            for (var i = 0; i < outputSize; i++)
            {
                int curPeriod = localPeriodArray[i];
                if (curPeriod != 0)
                {
                    RetCode retCode = Ma(inReal, startIdx, endIdx, localOutputArray, out _, out _, optInMAType, curPeriod);
                    if (retCode != RetCode.Success)
                    {
                        return retCode;
                    }

                    outReal[i] = localOutputArray[i];
                    for (var j = i + 1; j < outputSize; j++)
                    {
                        if (localPeriodArray[j] == curPeriod)
                        {
                            localPeriodArray[j] = 0;
                            outReal[j] = localOutputArray[j];
                        }
                    }
                }
            }

            outBegIdx = startIdx;
            outNbElement = outputSize;

            return RetCode.Success;
        }

        public static int MavpLookback(MAType optInMAType, int optInMaxPeriod = 30)
        {
            if (optInMaxPeriod < 2 || optInMaxPeriod > 100000)
            {
                return -1;
            }

            return MaLookback(optInMAType, optInMaxPeriod);
        }
    }
}
