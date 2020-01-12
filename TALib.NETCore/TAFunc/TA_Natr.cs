using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Natr(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int outNbElement1 = 0;
            int outBegIdx1 = 0;
            double[] prevATRTemp = new double[1];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
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
                return TrueRange(startIdx, endIdx, inHigh, inLow, inClose, ref outBegIdx, ref outNBElement, outReal);
            }

            double[] tempBuffer = new double[(lookbackTotal + (endIdx - startIdx)) + 1];
            RetCode retCode = TrueRange((startIdx - lookbackTotal) + 1, endIdx, inHigh, inLow, inClose, ref outBegIdx1, ref outNbElement1,
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
                int outIdx = (int) Globals.unstablePeriod[0x11];
                while (true)
                {
                    if (outIdx == 0)
                    {
                        break;
                    }

                    prevATR *= optInTimePeriod - 1;
                    prevATR += tempBuffer[today];
                    today++;
                    prevATR /= (double) optInTimePeriod;
                    outIdx--;
                }

                outIdx = 1;
                double tempValue = inClose[today];
                if ((-1E-08 >= tempValue) || (tempValue >= 1E-08))
                {
                    outReal[0] = (prevATR / tempValue) * 100.0;
                }
                else
                {
                    outReal[0] = 0.0;
                }

                int nbATR = (endIdx - startIdx) + 1;
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
                    prevATR /= (double) optInTimePeriod;
                    tempValue = inClose[today];
                    if ((-1E-08 >= tempValue) || (tempValue >= 1E-08))
                    {
                        outReal[outIdx] = (prevATR / tempValue) * 100.0;
                    }
                    else
                    {
                        outReal[0] = 0.0;
                    }

                    outIdx++;
                }

                outBegIdx = startIdx;
                outNBElement = outIdx;
            }

            return retCode;
        }

        public static RetCode Natr(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInTimePeriod,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int outNbElement1 = 0;
            int outBegIdx1 = 0;
            double[] prevATRTemp = new double[1];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || (inClose == null))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
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
                return TrueRange(startIdx, endIdx, inHigh, inLow, inClose, ref outBegIdx, ref outNBElement, outReal);
            }

            double[] tempBuffer = new double[(lookbackTotal + (endIdx - startIdx)) + 1];
            RetCode retCode = TrueRange((startIdx - lookbackTotal) + 1, endIdx, inHigh, inLow, inClose, ref outBegIdx1, ref outNbElement1,
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
                int outIdx = (int) Globals.unstablePeriod[0x11];
                while (true)
                {
                    if (outIdx == 0)
                    {
                        break;
                    }

                    prevATR *= optInTimePeriod - 1;
                    prevATR += tempBuffer[today];
                    today++;
                    prevATR /= (double) optInTimePeriod;
                    outIdx--;
                }

                outIdx = 1;
                double tempValue = inClose[today];
                if ((-1E-08 >= tempValue) || (tempValue >= 1E-08))
                {
                    outReal[0] = (prevATR / tempValue) * 100.0;
                }
                else
                {
                    outReal[0] = 0.0;
                }

                int nbATR = (endIdx - startIdx) + 1;
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
                    prevATR /= (double) optInTimePeriod;
                    tempValue = inClose[today];
                    if ((-1E-08 >= tempValue) || (tempValue >= 1E-08))
                    {
                        outReal[outIdx] = (prevATR / tempValue) * 100.0;
                    }
                    else
                    {
                        outReal[0] = 0.0;
                    }

                    outIdx++;
                }

                outBegIdx = startIdx;
                outNBElement = outIdx;
            }

            return retCode;
        }

        public static int NatrLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return (optInTimePeriod + ((int) Globals.unstablePeriod[0x11]));
        }
    }
}
