namespace TALib
{
    public static partial class Core
    {
        public static RetCode Atr(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outReal,
            out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AtrLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            if (optInTimePeriod <= 1)
            {
                return TRange(inHigh, inLow, inClose, startIdx, endIdx, outReal, out outBegIdx, out outNbElement);
            }

            var prevATRTemp = new double[1];

            var tempBuffer = new double[lookbackTotal + (endIdx - startIdx) + 1];
            RetCode retCode = TRange(inHigh, inLow, inClose, startIdx - lookbackTotal + 1, endIdx, tempBuffer, out _, out _);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_SMA(tempBuffer, optInTimePeriod - 1, optInTimePeriod - 1, prevATRTemp, out _, out _, optInTimePeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            double prevATR = prevATRTemp[0];
            int today = optInTimePeriod;
            int outIdx = (int) Globals.UnstablePeriod[(int) FuncUnstId.Atr];
            while (outIdx != 0)
            {
                prevATR *= optInTimePeriod - 1;
                prevATR += tempBuffer[today++];
                prevATR /= optInTimePeriod;
                outIdx--;
            }

            outIdx = 1;
            outReal[0] = prevATR;

            int nbATR = endIdx - startIdx + 1;

            while (--nbATR != 0)
            {
                prevATR *= optInTimePeriod - 1;
                prevATR += tempBuffer[today++];
                outReal[outIdx++] = prevATR / optInTimePeriod;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return retCode;
        }

        public static RetCode Atr(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outReal,
            out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AtrLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            if (optInTimePeriod <= 1)
            {
                return TRange(inHigh, inLow, inClose, startIdx, endIdx, outReal, out outBegIdx, out outNbElement);
            }

            var prevATRTemp = new decimal[1];

            var tempBuffer = new decimal[lookbackTotal + (endIdx - startIdx) + 1];
            RetCode retCode = TRange(inHigh, inLow, inClose, startIdx - lookbackTotal + 1, endIdx, tempBuffer, out _, out _);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_SMA(tempBuffer, optInTimePeriod - 1, optInTimePeriod - 1, prevATRTemp, out _, out _, optInTimePeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            decimal prevATR = prevATRTemp[0];
            int today = optInTimePeriod;
            int outIdx = (int) Globals.UnstablePeriod[(int) FuncUnstId.Atr];
            while (outIdx != 0)
            {
                prevATR *= optInTimePeriod - 1;
                prevATR += tempBuffer[today++];
                prevATR /= optInTimePeriod;
                outIdx--;
            }

            outIdx = 1;
            outReal[0] = prevATR;

            int nbATR = endIdx - startIdx + 1;

            while (--nbATR != 0)
            {
                prevATR *= optInTimePeriod - 1;
                prevATR += tempBuffer[today++];
                outReal[outIdx++] = prevATR / optInTimePeriod;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return retCode;
        }

        public static int AtrLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Atr];
        }
    }
}
