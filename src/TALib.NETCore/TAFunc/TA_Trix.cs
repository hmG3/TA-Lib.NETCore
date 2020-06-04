namespace TALib
{
    public partial class Core
    {
        public static RetCode Trix(int startIdx, int endIdx, double[] inReal, out int outBegIdx, out int outNbElement, double[] outReal,
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
            RetCode retCode = TA_INT_EMA(startIdx - totalLookback, endIdx, inReal, optInTimePeriod, k, out _, out var nbElement,
                tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput--;

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, out _, out nbElement, tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, out _, out nbElement, tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = Roc(0, nbElementToOutput, tempBuffer, out _, out outNbElement, outReal, 1);
            if (retCode != RetCode.Success || outNbElement == 0)
            {
                outBegIdx = outNbElement = 0;

                return retCode;
            }

            return RetCode.Success;
        }

        public static RetCode Trix(int startIdx, int endIdx, decimal[] inReal, out int outBegIdx, out int outNbElement, decimal[] outReal,
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
            RetCode retCode = TA_INT_EMA(startIdx - totalLookback, endIdx, inReal, optInTimePeriod, k, out _, out var nbElement,
                tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput--;

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, out _, out nbElement, tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, out _, out nbElement, tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = Roc(0, nbElementToOutput, tempBuffer, out _, out outNbElement, outReal, 1);
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
