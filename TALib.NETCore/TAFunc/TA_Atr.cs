namespace TALib
{
    public partial class Core
    {
        public static RetCode Atr(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal, int optInTimePeriod = 14)
        {
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

            outBegIdx = 0;
            outNBElement = 0;
            if (optInTimePeriod <= 1)
            {
                return TRange(startIdx, endIdx, inHigh, inLow, inClose, ref outBegIdx, ref outNBElement, outReal);
            }

            int outNbElement1 = default;
            int outBegIdx1 = default;
            var prevATRTemp = new double[1];

            var tempBuffer = new double[lookbackTotal + (endIdx - startIdx) + 1];
            RetCode retCode = TRange(startIdx - lookbackTotal + 1, endIdx, inHigh, inLow, inClose, ref outBegIdx1, ref outNbElement1,
                tempBuffer);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_SMA(optInTimePeriod - 1, optInTimePeriod - 1, tempBuffer, optInTimePeriod, ref outBegIdx1,
                ref outNbElement1, prevATRTemp);
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
            outNBElement = outIdx;

            return retCode;
        }

        public static RetCode Atr(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, ref int outBegIdx,
            ref int outNBElement, decimal[] outReal, int optInTimePeriod = 14)
        {
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

            outBegIdx = 0;
            outNBElement = 0;
            if (optInTimePeriod <= 1)
            {
                return TRange(startIdx, endIdx, inHigh, inLow, inClose, ref outBegIdx, ref outNBElement, outReal);
            }

            int outNbElement1 = default;
            int outBegIdx1 = default;
            var prevATRTemp = new decimal[1];

            var tempBuffer = new decimal[lookbackTotal + (endIdx - startIdx) + 1];
            RetCode retCode = TRange(startIdx - lookbackTotal + 1, endIdx, inHigh, inLow, inClose, ref outBegIdx1, ref outNbElement1,
                tempBuffer);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_SMA(optInTimePeriod - 1, optInTimePeriod - 1, tempBuffer, optInTimePeriod, ref outBegIdx1,
                ref outNbElement1, prevATRTemp);
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
            outNBElement = outIdx;

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
