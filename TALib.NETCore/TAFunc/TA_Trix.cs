namespace TALib
{
    public partial class Core
    {
        public static RetCode Trix(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 30)
        {
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
                outNBElement = 0;
                outBegIdx = 0;
                return RetCode.Success;
            }

            int nbElement = default;
            int begIdx = default;
            outBegIdx = startIdx;
            int nbElementToOutput = endIdx - startIdx + 1 + totalLookback;
            var tempBuffer = new double[nbElementToOutput];

            double k = 2.0 / (optInTimePeriod + 1);
            RetCode retCode = TA_INT_EMA(startIdx - totalLookback, endIdx, inReal, optInTimePeriod, k, ref begIdx, ref nbElement,
                tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput--;

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, ref begIdx, ref nbElement, tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, ref begIdx, ref nbElement, tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = Roc(0, nbElementToOutput, tempBuffer, ref begIdx, ref outNBElement, outReal, 1);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            return RetCode.Success;
        }

        public static RetCode Trix(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 30)
        {
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
                outNBElement = 0;
                outBegIdx = 0;
                return RetCode.Success;
            }

            int nbElement = default;
            int begIdx = default;
            outBegIdx = startIdx;
            int nbElementToOutput = endIdx - startIdx + 1 + totalLookback;
            var tempBuffer = new decimal[nbElementToOutput];

            decimal k = 2m / (optInTimePeriod + 1);
            RetCode retCode = TA_INT_EMA(startIdx - totalLookback, endIdx, inReal, optInTimePeriod, k, ref begIdx, ref nbElement,
                tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput--;

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, ref begIdx, ref nbElement, tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = TA_INT_EMA(0, nbElementToOutput, tempBuffer, optInTimePeriod, k, ref begIdx, ref nbElement, tempBuffer);
            if (retCode != RetCode.Success || nbElement == 0)
            {
                outNBElement = 0;
                outBegIdx = 0;
                return retCode;
            }

            nbElementToOutput -= emaLookback;
            retCode = Roc(0, nbElementToOutput, tempBuffer, ref begIdx, ref outNBElement, outReal, 1);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outNBElement = 0;
                outBegIdx = 0;
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
