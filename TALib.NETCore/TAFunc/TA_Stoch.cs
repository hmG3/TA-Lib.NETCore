using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Stoch(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, MAType optInSlowKMaType,
            MAType optInSlowDMaType, ref int outBegIdx, ref int outNBElement, double[] outSlowK, double[] outSlowD,
            int optInFastKPeriod = 5, int optInSlowKPeriod = 3, int optInSlowDPeriod = 3)
        {
            double[] tempBuffer;
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

            if (optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInSlowKPeriod < 1 || optInSlowKPeriod > 100000 ||
                optInSlowDPeriod < 1 || optInSlowDPeriod > 100000)
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

            int lookbackK = optInFastKPeriod - 1;
            int lookbackKSlow = MovingAverageLookback(optInSlowKMaType, optInSlowKPeriod);
            int lookbackDSlow = MovingAverageLookback(optInSlowDMaType, optInSlowDPeriod);
            int lookbackTotal = lookbackK + lookbackDSlow + lookbackKSlow;
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

            int outIdx = default;
            int trailingIdx = startIdx - lookbackTotal;
            int today = trailingIdx + lookbackK;
            int highestIdx = -1;
            int lowestIdx = highestIdx;
            double lowest = default;
            double highest = lowest;
            double diff = highest;
            if (outSlowK == inHigh || outSlowK == inLow || outSlowK == inClose)
            {
                tempBuffer = outSlowK;
            }
            else if (outSlowD == inHigh || outSlowD == inLow || outSlowD == inClose)
            {
                tempBuffer = outSlowD;
            }
            else
            {
                tempBuffer = new double[endIdx - today + 1];
            }

            Label_0156:
            if (today > endIdx)
            {
                RetCode retCode = MovingAverage(0, outIdx - 1, tempBuffer, optInSlowKMaType, ref outBegIdx, ref outNBElement, tempBuffer,
                    optInSlowKPeriod);
                if (retCode != RetCode.Success || outNBElement == 0)
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }

                retCode = MovingAverage(0, outNBElement - 1, tempBuffer, optInSlowDMaType, ref outBegIdx, ref outNBElement, outSlowD,
                    optInSlowDPeriod);
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
            if (!diff.Equals(0.0))
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

        public static RetCode Stoch(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            MAType optInSlowKMAType, MAType optInSlowDMAType, ref int outBegIdx, ref int outNBElement, decimal[] outSlowK,
            decimal[] outSlowD, int optInFastKPeriod = 5, int optInSlowKPeriod = 3, int optInSlowDPeriod = 3)
        {
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

            if (optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInSlowKPeriod < 1 || optInSlowKPeriod > 100000 ||
                optInSlowDPeriod < 1 || optInSlowDPeriod > 100000)
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

            int lookbackK = optInFastKPeriod - 1;
            int lookbackKSlow = MovingAverageLookback(optInSlowKMAType, optInSlowKPeriod);
            int lookbackDSlow = MovingAverageLookback(optInSlowDMAType, optInSlowDPeriod);
            int lookbackTotal = lookbackK + lookbackDSlow + lookbackKSlow;
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

            int outIdx = default;
            int trailingIdx = startIdx - lookbackTotal;
            int today = trailingIdx + lookbackK;
            int highestIdx = -1;
            int lowestIdx = highestIdx;
            decimal lowest = default;
            decimal highest = lowest;
            decimal diff = highest;
            var tempBuffer = new decimal[endIdx - today + 1];
            Label_012A:
            if (today > endIdx)
            {
                RetCode retCode = MovingAverage(0, outIdx - 1, tempBuffer, optInSlowKMAType, ref outBegIdx, ref outNBElement, tempBuffer,
                    optInSlowKPeriod);
                if (retCode != RetCode.Success || outNBElement == 0)
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }

                retCode = MovingAverage(0, outNBElement - 1, tempBuffer, optInSlowDMAType, ref outBegIdx, ref outNBElement, outSlowD,
                    optInSlowDPeriod);
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

            decimal tmp = inLow[today];
            if (lowestIdx >= trailingIdx)
            {
                if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                    diff = (highest - lowest) / 100m;
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

            diff = (highest - lowest) / 100m;
            Label_018C:
            tmp = inHigh[today];
            if (highestIdx >= trailingIdx)
            {
                if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / 100m;
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

            diff = (highest - lowest) / 100m;
            Label_01EC:
            if (diff != Decimal.Zero)
            {
                tempBuffer[outIdx] = (inClose[today] - lowest) / diff;
                outIdx++;
            }
            else
            {
                tempBuffer[outIdx] = Decimal.Zero;
                outIdx++;
            }

            trailingIdx++;
            today++;
            goto Label_012A;
        }

        public static int StochLookback(MAType optInSlowKMAType, MAType optInSlowDMAType, int optInFastKPeriod = 5,
            int optInSlowKPeriod = 3, int optInSlowDPeriod = 3)
        {
            if (optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInSlowKPeriod < 1 || optInSlowKPeriod > 100000 ||
                optInSlowDPeriod < 1 || optInSlowDPeriod > 100000)
            {
                return -1;
            }

            int retValue = optInFastKPeriod - 1;
            retValue += MovingAverageLookback(optInSlowKMAType, optInSlowKPeriod);

            return retValue + MovingAverageLookback(optInSlowDMAType, optInSlowDPeriod);
        }
    }
}
