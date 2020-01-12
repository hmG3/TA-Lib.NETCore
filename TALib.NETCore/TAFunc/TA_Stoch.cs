using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Stoch(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInFastK_Period,
            int optInSlowK_Period, MAType optInSlowK_MAType, int optInSlowD_Period, MAType optInSlowD_MAType, ref int outBegIdx,
            ref int outNBElement, double[] outSlowK, double[] outSlowD)
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

            if (optInSlowK_Period == -2147483648)
            {
                optInSlowK_Period = 3;
            }
            else if ((optInSlowK_Period < 1) || (optInSlowK_Period > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSlowD_Period == -2147483648)
            {
                optInSlowD_Period = 3;
            }
            else if ((optInSlowD_Period < 1) || (optInSlowD_Period > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outSlowK == null)
            {
                return RetCode.BadParam;
            }

            if (outSlowD == null)
            {
                return RetCode.BadParam;
            }

            int lookbackK = optInFastK_Period - 1;
            int lookbackKSlow = MovingAverageLookback(optInSlowK_Period, optInSlowK_MAType);
            int lookbackDSlow = MovingAverageLookback(optInSlowD_Period, optInSlowD_MAType);
            int lookbackTotal = (lookbackK + lookbackDSlow) + lookbackKSlow;
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
            if (((outSlowK == inHigh) || (outSlowK == inLow)) || (outSlowK == inClose))
            {
                tempBuffer = outSlowK;
            }
            else if (((outSlowD == inHigh) || (outSlowD == inLow)) || (outSlowD == inClose))
            {
                tempBuffer = outSlowD;
            }
            else
            {
                tempBuffer = new double[(endIdx - today) + 1];
            }

            Label_0156:
            if (today > endIdx)
            {
                RetCode retCode = MovingAverage(0, outIdx - 1, tempBuffer, optInSlowK_Period, optInSlowK_MAType, ref outBegIdx,
                    ref outNBElement, tempBuffer);
                if ((retCode != RetCode.Success) || (outNBElement == 0))
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }

                retCode = MovingAverage(0, outNBElement - 1, tempBuffer, optInSlowD_Period, optInSlowD_MAType, ref outBegIdx,
                    ref outNBElement, outSlowD);
                Array.Copy(tempBuffer, lookbackDSlow, outSlowK, 0, outNBElement);
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

                goto Label_01B5;
            }

            lowestIdx = trailingIdx;
            lowest = inLow[lowestIdx];
            int i = lowestIdx;
            Label_0173:
            i++;
            if (i <= today)
            {
                tmp = inLow[i];
                if (tmp < lowest)
                {
                    lowestIdx = i;
                    lowest = tmp;
                }

                goto Label_0173;
            }

            diff = (highest - lowest) / 100.0;
            Label_01B5:
            tmp = inHigh[today];
            if (highestIdx >= trailingIdx)
            {
                if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / 100.0;
                }

                goto Label_0212;
            }

            highestIdx = trailingIdx;
            highest = inHigh[highestIdx];
            i = highestIdx;
            Label_01CC:
            i++;
            if (i <= today)
            {
                tmp = inHigh[i];
                if (tmp > highest)
                {
                    highestIdx = i;
                    highest = tmp;
                }

                goto Label_01CC;
            }

            diff = (highest - lowest) / 100.0;
            Label_0212:
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
            goto Label_0156;
        }

        public static RetCode Stoch(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInFastK_Period,
            int optInSlowK_Period, MAType optInSlowK_MAType, int optInSlowD_Period, MAType optInSlowD_MAType, ref int outBegIdx,
            ref int outNBElement, double[] outSlowK, double[] outSlowD)
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

            if (optInSlowK_Period == -2147483648)
            {
                optInSlowK_Period = 3;
            }
            else if ((optInSlowK_Period < 1) || (optInSlowK_Period > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInSlowD_Period == -2147483648)
            {
                optInSlowD_Period = 3;
            }
            else if ((optInSlowD_Period < 1) || (optInSlowD_Period > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outSlowK == null)
            {
                return RetCode.BadParam;
            }

            if (outSlowD == null)
            {
                return RetCode.BadParam;
            }

            int lookbackK = optInFastK_Period - 1;
            int lookbackKSlow = MovingAverageLookback(optInSlowK_Period, optInSlowK_MAType);
            int lookbackDSlow = MovingAverageLookback(optInSlowD_Period, optInSlowD_MAType);
            int lookbackTotal = (lookbackK + lookbackDSlow) + lookbackKSlow;
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
            Label_012A:
            if (today > endIdx)
            {
                RetCode retCode = MovingAverage(0, outIdx - 1, tempBuffer, optInSlowK_Period, optInSlowK_MAType, ref outBegIdx,
                    ref outNBElement, tempBuffer);
                if ((retCode != RetCode.Success) || (outNBElement == 0))
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }

                retCode = MovingAverage(0, outNBElement - 1, tempBuffer, optInSlowD_Period, optInSlowD_MAType, ref outBegIdx,
                    ref outNBElement, outSlowD);
                Array.Copy(tempBuffer, lookbackDSlow, outSlowK, 0, outNBElement);
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

                goto Label_018C;
            }

            lowestIdx = trailingIdx;
            lowest = inLow[lowestIdx];
            int i = lowestIdx;
            Label_0149:
            i++;
            if (i <= today)
            {
                tmp = inLow[i];
                if (tmp < lowest)
                {
                    lowestIdx = i;
                    lowest = tmp;
                }

                goto Label_0149;
            }

            diff = (highest - lowest) / 100.0;
            Label_018C:
            tmp = inHigh[today];
            if (highestIdx >= trailingIdx)
            {
                if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / 100.0;
                }

                goto Label_01EC;
            }

            highestIdx = trailingIdx;
            highest = inHigh[highestIdx];
            i = highestIdx;
            Label_01A5:
            i++;
            if (i <= today)
            {
                tmp = inHigh[i];
                if (tmp > highest)
                {
                    highestIdx = i;
                    highest = tmp;
                }

                goto Label_01A5;
            }

            diff = (highest - lowest) / 100.0;
            Label_01EC:
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
            goto Label_012A;
        }

        public static int StochLookback(int optInFastK_Period, int optInSlowK_Period, MAType optInSlowK_MAType, int optInSlowD_Period,
            MAType optInSlowD_MAType)
        {
            if (optInFastK_Period == -2147483648)
            {
                optInFastK_Period = 5;
            }
            else if ((optInFastK_Period < 1) || (optInFastK_Period > 0x186a0))
            {
                return -1;
            }

            if (optInSlowK_Period == -2147483648)
            {
                optInSlowK_Period = 3;
            }
            else if ((optInSlowK_Period < 1) || (optInSlowK_Period > 0x186a0))
            {
                return -1;
            }

            if (optInSlowD_Period == -2147483648)
            {
                optInSlowD_Period = 3;
            }
            else if ((optInSlowD_Period < 1) || (optInSlowD_Period > 0x186a0))
            {
                return -1;
            }

            int retValue = optInFastK_Period - 1;
            retValue += MovingAverageLookback(optInSlowK_Period, optInSlowK_MAType);
            return (retValue + MovingAverageLookback(optInSlowD_Period, optInSlowD_MAType));
        }
    }
}
