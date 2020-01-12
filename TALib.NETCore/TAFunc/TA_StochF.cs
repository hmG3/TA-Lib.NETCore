using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode StochF(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInFastK_Period,
            int optInFastD_Period, MAType optInFastD_MAType, ref int outBegIdx, ref int outNBElement, double[] outFastK, double[] outFastD)
        {
            double[] tempBuffer;
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

            if (optInFastK_Period == -2147483648)
            {
                optInFastK_Period = 5;
            }
            else if ((optInFastK_Period < 1) || (optInFastK_Period > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInFastD_Period == -2147483648)
            {
                optInFastD_Period = 3;
            }
            else if ((optInFastD_Period < 1) || (optInFastD_Period > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outFastK == null)
            {
                return RetCode.BadParam;
            }

            if (outFastD == null)
            {
                return RetCode.BadParam;
            }

            int lookbackK = optInFastK_Period - 1;
            int lookbackFastD = MovingAverageLookback(optInFastD_Period, optInFastD_MAType);
            int lookbackTotal = lookbackK + lookbackFastD;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = 0;
            int trailingIdx = startIdx - lookbackTotal;
            int today = trailingIdx + lookbackK;
            int highestIdx = -1;
            int lowestIdx = highestIdx;
            double lowest = 0.0;
            double highest = lowest;
            double diff = highest;
            if (((outFastK == inHigh) || (outFastK == inLow)) || (outFastK == inClose))
            {
                tempBuffer = outFastK;
            }
            else if (((outFastD == inHigh) || (outFastD == inLow)) || (outFastD == inClose))
            {
                tempBuffer = outFastD;
            }
            else
            {
                tempBuffer = new double[(endIdx - today) + 1];
            }

            Label_0124:
            if (today > endIdx)
            {
                RetCode retCode = MovingAverage(0, outIdx - 1, tempBuffer, optInFastD_Period, optInFastD_MAType, ref outBegIdx,
                    ref outNBElement, outFastD);
                if ((retCode != RetCode.Success) || (outNBElement == 0))
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }

                Array.Copy(tempBuffer, lookbackFastD, outFastK, 0, outNBElement);
                if (retCode != RetCode.Success)
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }

                outBegIdx = startIdx;
                return RetCode.Success;
            }

            double tmp = inLow[today];
            if (lowestIdx >= trailingIdx)
            {
                if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                    diff = (highest - lowest) / 100.0;
                }

                goto Label_0183;
            }

            lowestIdx = trailingIdx;
            lowest = inLow[lowestIdx];
            int i = lowestIdx;
            Label_0141:
            i++;
            if (i <= today)
            {
                tmp = inLow[i];
                if (tmp < lowest)
                {
                    lowestIdx = i;
                    lowest = tmp;
                }

                goto Label_0141;
            }

            diff = (highest - lowest) / 100.0;
            Label_0183:
            tmp = inHigh[today];
            if (highestIdx >= trailingIdx)
            {
                if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / 100.0;
                }

                goto Label_01E0;
            }

            highestIdx = trailingIdx;
            highest = inHigh[highestIdx];
            i = highestIdx;
            Label_019A:
            i++;
            if (i <= today)
            {
                tmp = inHigh[i];
                if (tmp > highest)
                {
                    highestIdx = i;
                    highest = tmp;
                }

                goto Label_019A;
            }

            diff = (highest - lowest) / 100.0;
            Label_01E0:
            if (diff != 0.0)
            {
                tempBuffer[outIdx] = (inClose[today] - lowest) / diff;
                outIdx++;
            }
            else
            {
                tempBuffer[outIdx] = 0.0;
                outIdx++;
            }

            trailingIdx++;
            today++;
            goto Label_0124;
        }

        public static RetCode StochF(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInFastK_Period,
            int optInFastD_Period, MAType optInFastD_MAType, ref int outBegIdx, ref int outNBElement, double[] outFastK, double[] outFastD)
        {
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

            if (optInFastK_Period == -2147483648)
            {
                optInFastK_Period = 5;
            }
            else if ((optInFastK_Period < 1) || (optInFastK_Period > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInFastD_Period == -2147483648)
            {
                optInFastD_Period = 3;
            }
            else if ((optInFastD_Period < 1) || (optInFastD_Period > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outFastK == null)
            {
                return RetCode.BadParam;
            }

            if (outFastD == null)
            {
                return RetCode.BadParam;
            }

            int lookbackK = optInFastK_Period - 1;
            int lookbackFastD = MovingAverageLookback(optInFastD_Period, optInFastD_MAType);
            int lookbackTotal = lookbackK + lookbackFastD;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outIdx = 0;
            int trailingIdx = startIdx - lookbackTotal;
            int today = trailingIdx + lookbackK;
            int highestIdx = -1;
            int lowestIdx = highestIdx;
            double lowest = 0.0;
            double highest = lowest;
            double diff = highest;
            double[] tempBuffer = new double[(endIdx - today) + 1];
            Label_00F8:
            if (today > endIdx)
            {
                RetCode retCode = MovingAverage(0, outIdx - 1, tempBuffer, optInFastD_Period, optInFastD_MAType, ref outBegIdx,
                    ref outNBElement, outFastD);
                if ((retCode != RetCode.Success) || (outNBElement == 0))
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }

                Array.Copy(tempBuffer, lookbackFastD, outFastK, 0, outNBElement);
                if (retCode != RetCode.Success)
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }

                outBegIdx = startIdx;
                return RetCode.Success;
            }

            double tmp = inLow[today];
            if (lowestIdx >= trailingIdx)
            {
                if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                    diff = (highest - lowest) / 100.0;
                }

                goto Label_015A;
            }

            lowestIdx = trailingIdx;
            lowest = inLow[lowestIdx];
            int i = lowestIdx;
            Label_0117:
            i++;
            if (i <= today)
            {
                tmp = inLow[i];
                if (tmp < lowest)
                {
                    lowestIdx = i;
                    lowest = tmp;
                }

                goto Label_0117;
            }

            diff = (highest - lowest) / 100.0;
            Label_015A:
            tmp = inHigh[today];
            if (highestIdx >= trailingIdx)
            {
                if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / 100.0;
                }

                goto Label_01BA;
            }

            highestIdx = trailingIdx;
            highest = inHigh[highestIdx];
            i = highestIdx;
            Label_0173:
            i++;
            if (i <= today)
            {
                tmp = inHigh[i];
                if (tmp > highest)
                {
                    highestIdx = i;
                    highest = tmp;
                }

                goto Label_0173;
            }

            diff = (highest - lowest) / 100.0;
            Label_01BA:
            if (diff != 0.0)
            {
                tempBuffer[outIdx] = (inClose[today] - lowest) / diff;
                outIdx++;
            }
            else
            {
                tempBuffer[outIdx] = 0.0;
                outIdx++;
            }

            trailingIdx++;
            today++;
            goto Label_00F8;
        }

        public static int StochFLookback(int optInFastK_Period, int optInFastD_Period, MAType optInFastD_MAType)
        {
            if (optInFastK_Period == -2147483648)
            {
                optInFastK_Period = 5;
            }
            else if ((optInFastK_Period < 1) || (optInFastK_Period > 0x186a0))
            {
                return -1;
            }

            if (optInFastD_Period == -2147483648)
            {
                optInFastD_Period = 3;
            }
            else if ((optInFastD_Period < 1) || (optInFastD_Period > 0x186a0))
            {
                return -1;
            }

            int retValue = optInFastK_Period - 1;
            return (retValue + MovingAverageLookback(optInFastD_Period, optInFastD_MAType));
        }
    }
}
