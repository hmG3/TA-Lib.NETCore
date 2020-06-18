namespace TALib
{
    public static partial class Core
    {
        public static RetCode Trix(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int emaLookback = EmaLookback(optInTimePeriod);
            int rocLookback = RocRLookback(1);
            int totalLookback = emaLookback * 3 + rocLookback;
            if (startIdx < totalLookback)
            {
                startIdx = totalLookback;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int nbElementToOutput = endIdx - startIdx + 1 + totalLookback;
            var tempBuffer = new double[nbElementToOutput];

            double k = 2.0 / (optInTimePeriod + 1);
            RetCode retCode = TA_INT_EMA(inReal, startIdx - totalLookback, endIdx, tempBuffer, out _,
                out var nbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput--;

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(tempBuffer, 0, nbElementToOutput, tempBuffer, out _, out nbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(tempBuffer, 0, nbElementToOutput, tempBuffer, out _, out nbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = Roc(tempBuffer, 0, nbElementToOutput, outReal, out _, out outNbElement, 1);
            if (retCode != RetCode.Success || outNbElement == 0)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            return RetCode.Success;
        }

        public static RetCode Trix(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int emaLookback = EmaLookback(optInTimePeriod);
            int rocLookback = RocRLookback(1);
            int totalLookback = emaLookback * 3 + rocLookback;
            if (startIdx < totalLookback)
            {
                startIdx = totalLookback;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int nbElementToOutput = endIdx - startIdx + 1 + totalLookback;
            var tempBuffer = new decimal[nbElementToOutput];

            decimal k = 2m / (optInTimePeriod + 1);
            RetCode retCode = TA_INT_EMA(inReal, startIdx - totalLookback, endIdx, tempBuffer, out _, out var nbElement, optInTimePeriod,
                k);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput--;

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(tempBuffer, 0, nbElementToOutput, tempBuffer, out _, out nbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(tempBuffer, 0, nbElementToOutput, tempBuffer, out _, out nbElement, optInTimePeriod, k);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = Roc(tempBuffer, 0, nbElementToOutput, outReal, out _, out outNbElement, 1);
            if (retCode != RetCode.Success || outNbElement == 0)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            return RetCode.Success;
        }

        public static int TrixLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return EmaLookback(optInTimePeriod) * 3 + RocRLookback(1);
        }
    }
}
