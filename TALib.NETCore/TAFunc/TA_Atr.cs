namespace TALib
{
    public partial class Core
    {
        public static RetCode Atr(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal, int optInTimePeriod = 14)
        {
            int outNbElement1 = default;
            int outBegIdx1 = default;
            var prevATRTemp = new double[1];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
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
                return TrueRange(startIdx, endIdx, inHigh, inLow, inClose, ref outBegIdx, ref outNBElement, outReal);
            }

            var tempBuffer = new double[lookbackTotal + (endIdx - startIdx) + 1];
            RetCode retCode = TrueRange(startIdx - lookbackTotal + 1, endIdx, inHigh, inLow, inClose, ref outBegIdx1, ref outNbElement1,
                tempBuffer);
            if (retCode == RetCode.Success)
            {
                retCode = TA_INT_SMA(optInTimePeriod - 1, optInTimePeriod - 1, tempBuffer, optInTimePeriod, ref outBegIdx1,
                    ref outNbElement1, prevATRTemp);
                if (retCode != RetCode.Success)
                {
                    return retCode;
                }

                double prevATR = prevATRTemp[0];
                int today = optInTimePeriod;
                int outIdx = (int) Globals.UnstablePeriod[(int) FuncUnstId.Atr];
                while (true)
                {
                    if (outIdx == 0)
                    {
                        break;
                    }

                    prevATR *= optInTimePeriod - 1;
                    prevATR += tempBuffer[today];
                    today++;
                    prevATR /= optInTimePeriod;
                    outIdx--;
                }

                outIdx = 1;
                outReal[0] = prevATR;
                int nbATR = endIdx - startIdx + 1;
                while (true)
                {
                    nbATR--;
                    if (nbATR == 0)
                    {
                        break;
                    }

                    prevATR *= optInTimePeriod - 1;
                    prevATR += tempBuffer[today];
                    today++;
                    outReal[outIdx] = prevATR / optInTimePeriod;
                    outIdx++;
                }

                outBegIdx = startIdx;
                outNBElement = outIdx;
            }

            return retCode;
        }

        public static RetCode Atr(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, ref int outBegIdx,
            ref int outNBElement, decimal[] outReal, int optInTimePeriod = 14)
        {
            int outNbElement1 = default;
            int outBegIdx1 = default;
            var prevATRTemp = new decimal[1];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
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
                return TrueRange(startIdx, endIdx, inHigh, inLow, inClose, ref outBegIdx, ref outNBElement, outReal);
            }

            var tempBuffer = new decimal[lookbackTotal + (endIdx - startIdx) + 1];
            RetCode retCode = TrueRange(startIdx - lookbackTotal + 1, endIdx, inHigh, inLow, inClose, ref outBegIdx1,
                ref outNbElement1, tempBuffer);
            if (retCode == RetCode.Success)
            {
                retCode = TA_INT_SMA(optInTimePeriod - 1, optInTimePeriod - 1, tempBuffer, optInTimePeriod, ref outBegIdx1,
                    ref outNbElement1, prevATRTemp);
                if (retCode != RetCode.Success)
                {
                    return retCode;
                }

                decimal prevATR = prevATRTemp[0];
                int today = optInTimePeriod;
                int outIdx = (int) Globals.UnstablePeriod[(int) FuncUnstId.Atr];
                while (true)
                {
                    if (outIdx == 0)
                    {
                        break;
                    }

                    prevATR *= optInTimePeriod - 1;
                    prevATR += tempBuffer[today];
                    today++;
                    prevATR /= optInTimePeriod;
                    outIdx--;
                }

                outIdx = 1;
                outReal[0] = prevATR;
                int nbATR = endIdx - startIdx + 1;
                while (true)
                {
                    nbATR--;
                    if (nbATR == 0)
                    {
                        break;
                    }

                    prevATR *= optInTimePeriod - 1;
                    prevATR += tempBuffer[today];
                    today++;
                    outReal[outIdx] = prevATR / optInTimePeriod;
                    outIdx++;
                }

                outBegIdx = startIdx;
                outNBElement = outIdx;
            }

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
