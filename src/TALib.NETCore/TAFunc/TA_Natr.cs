using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Natr(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, out int outBegIdx,
            out int outNbElement, double[] outReal, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

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

            int lookbackTotal = NatrLookback(optInTimePeriod);
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
                return TRange(startIdx, endIdx, inHigh, inLow, inClose, out outBegIdx, out outNbElement, outReal);
            }

            var tempBuffer = new double[lookbackTotal + (endIdx - startIdx) + 1];
            RetCode retCode = TRange(startIdx - lookbackTotal + 1, endIdx, inHigh, inLow, inClose, out _, out _, tempBuffer);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_SMA(optInTimePeriod - 1, optInTimePeriod - 1, tempBuffer, optInTimePeriod, out _, out _, prevATRTemp);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            double prevATR = prevATRTemp[0];
            int today = optInTimePeriod;
            int outIdx = (int) Globals.UnstablePeriod[(int) FuncUnstId.Natr];
            while (outIdx != 0)
            {
                prevATR *= optInTimePeriod - 1;
                prevATR += tempBuffer[today];
                today++;
                prevATR /= optInTimePeriod;
                outIdx--;
            }

            outIdx = 1;
            double tempValue = inClose[today];
            if (!TA_IsZero(tempValue))
            {
                outReal[0] = prevATR / tempValue * 100.0;
            }
            else
            {
                outReal[0] = 0.0;
            }

            int nbATR = endIdx - startIdx + 1;
            while (--nbATR != 0)
            {
                prevATR *= optInTimePeriod - 1;
                prevATR += tempBuffer[today];
                today++;
                prevATR /= optInTimePeriod;
                tempValue = inClose[today];
                if (!TA_IsZero(tempValue))
                {
                    outReal[outIdx] = prevATR / tempValue * 100.0;
                }
                else
                {
                    outReal[0] = 0.0;
                }

                outIdx++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return retCode;
        }

        public static RetCode Natr(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, out int outBegIdx,
            out int outNbElement, decimal[] outReal, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

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

            int lookbackTotal = NatrLookback(optInTimePeriod);
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
                return TRange(startIdx, endIdx, inHigh, inLow, inClose, out outBegIdx, out outNbElement, outReal);
            }

            var tempBuffer = new decimal[lookbackTotal + (endIdx - startIdx) + 1];
            RetCode retCode = TRange(startIdx - lookbackTotal + 1, endIdx, inHigh, inLow, inClose, out _, out _, tempBuffer);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_SMA(optInTimePeriod - 1, optInTimePeriod - 1, tempBuffer, optInTimePeriod, out _, out _, prevATRTemp);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            decimal prevATR = prevATRTemp[0];
            int today = optInTimePeriod;
            int outIdx = (int) Globals.UnstablePeriod[(int) FuncUnstId.Natr];
            while (outIdx != 0)
            {
                prevATR *= optInTimePeriod - 1;
                prevATR += tempBuffer[today];
                today++;
                prevATR /= optInTimePeriod;
                outIdx--;
            }

            outIdx = 1;
            decimal tempValue = inClose[today];
            if (!TA_IsZero(tempValue))
            {
                outReal[0] = prevATR / tempValue * 100m;
            }
            else
            {
                outReal[0] = Decimal.Zero;
            }

            int nbATR = endIdx - startIdx + 1;
            while (--nbATR != 0)
            {
                prevATR *= optInTimePeriod - 1;
                prevATR += tempBuffer[today];
                today++;
                prevATR /= optInTimePeriod;
                tempValue = inClose[today];
                if (!TA_IsZero(tempValue))
                {
                    outReal[outIdx] = prevATR / tempValue * 100m;
                }
                else
                {
                    outReal[0] = Decimal.Zero;
                }

                outIdx++;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return retCode;
        }

        public static int NatrLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Natr];
        }
    }
}
