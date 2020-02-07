using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Stoch(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, MAType optInSlowKMaType,
            MAType optInSlowDMaType, ref int outBegIdx, ref int outNBElement, double[] outSlowK, double[] outSlowD,
            int optInFastKPeriod = 5, int optInSlowKPeriod = 3, int optInSlowDPeriod = 3)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outSlowK == null || outSlowD == null || optInFastKPeriod < 1 ||
                optInFastKPeriod > 100000 || optInSlowKPeriod < 1 || optInSlowKPeriod > 100000 || optInSlowDPeriod < 1 ||
                optInSlowDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackK = optInFastKPeriod - 1;
            int lookbackKSlow = MaLookback(optInSlowKMaType, optInSlowKPeriod);
            int lookbackDSlow = MaLookback(optInSlowDMaType, optInSlowDPeriod);
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
            double highest, lowest;
            double diff = highest = lowest = default;
            double[] tempBuffer;
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

            while (today <= endIdx)
            {
                double tmp = inLow[today];
                if (lowestIdx < trailingIdx)
                {
                    lowestIdx = trailingIdx;
                    lowest = inLow[lowestIdx];
                    int i = lowestIdx;
                    while (++i <= today)
                    {
                        tmp = inLow[i];
                        if (tmp < lowest)
                        {
                            lowestIdx = i;
                            lowest = tmp;
                        }
                    }

                    diff = (highest - lowest) / 100.0;
                }
                else if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                    diff = (highest - lowest) / 100.0;
                }

                tmp = inHigh[today];
                if (highestIdx < trailingIdx)
                {
                    highestIdx = trailingIdx;
                    highest = inHigh[highestIdx];
                    int i = highestIdx;
                    while (++i <= today)
                    {
                        tmp = inHigh[i];
                        if (tmp > highest)
                        {
                            highestIdx = i;
                            highest = tmp;
                        }
                    }

                    diff = (highest - lowest) / 100.0;
                }
                else if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / 100.0;
                }

                if (!diff.Equals(0.0))
                {
                    tempBuffer[outIdx++] = (inClose[today] - lowest) / diff;
                }
                else
                {
                    tempBuffer[outIdx++] = 0.0;
                }

                trailingIdx++;
                today++;
            }

            RetCode retCode = Ma(0, outIdx - 1, tempBuffer, optInSlowKMaType, ref outBegIdx, ref outNBElement, tempBuffer,
                optInSlowKPeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            retCode = Ma(0, outNBElement - 1, tempBuffer, optInSlowDMaType, ref outBegIdx, ref outNBElement, outSlowD,
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

        public static RetCode Stoch(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            MAType optInSlowKMaType, MAType optInSlowDMaType, ref int outBegIdx, ref int outNBElement, decimal[] outSlowK,
            decimal[] outSlowD, int optInFastKPeriod = 5, int optInSlowKPeriod = 3, int optInSlowDPeriod = 3)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outSlowK == null || outSlowD == null || optInFastKPeriod < 1 ||
                optInFastKPeriod > 100000 || optInSlowKPeriod < 1 || optInSlowKPeriod > 100000 || optInSlowDPeriod < 1 ||
                optInSlowDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackK = optInFastKPeriod - 1;
            int lookbackKSlow = MaLookback(optInSlowKMaType, optInSlowKPeriod);
            int lookbackDSlow = MaLookback(optInSlowDMaType, optInSlowDPeriod);
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
            decimal highest, lowest;
            decimal diff = highest = lowest = default;
            decimal[] tempBuffer;
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
                tempBuffer = new decimal[endIdx - today + 1];
            }

            while (today <= endIdx)
            {
                decimal tmp = inLow[today];
                if (lowestIdx < trailingIdx)
                {
                    lowestIdx = trailingIdx;
                    lowest = inLow[lowestIdx];
                    int i = lowestIdx;
                    while (++i <= today)
                    {
                        tmp = inLow[i];
                        if (tmp < lowest)
                        {
                            lowestIdx = i;
                            lowest = tmp;
                        }
                    }

                    diff = (highest - lowest) / 100m;
                }
                else if (tmp <= lowest)
                {
                    lowestIdx = today;
                    lowest = tmp;
                    diff = (highest - lowest) / 100m;
                }

                tmp = inHigh[today];
                if (highestIdx < trailingIdx)
                {
                    highestIdx = trailingIdx;
                    highest = inHigh[highestIdx];
                    int i = highestIdx;
                    while (++i <= today)
                    {
                        tmp = inHigh[i];
                        if (tmp > highest)
                        {
                            highestIdx = i;
                            highest = tmp;
                        }
                    }

                    diff = (highest - lowest) / 100m;
                }
                else if (tmp >= highest)
                {
                    highestIdx = today;
                    highest = tmp;
                    diff = (highest - lowest) / 100m;
                }

                if (diff != Decimal.Zero)
                {
                    tempBuffer[outIdx++] = (inClose[today] - lowest) / diff;
                }
                else
                {
                    tempBuffer[outIdx++] = Decimal.Zero;
                }

                trailingIdx++;
                today++;
            }

            RetCode retCode = Ma(0, outIdx - 1, tempBuffer, optInSlowKMaType, ref outBegIdx, ref outNBElement, tempBuffer,
                optInSlowKPeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return retCode;
            }

            retCode = Ma(0, outNBElement - 1, tempBuffer, optInSlowDMaType, ref outBegIdx, ref outNBElement, outSlowD,
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

        public static int StochLookback(MAType optInSlowKMAType, MAType optInSlowDMAType, int optInFastKPeriod = 5,
            int optInSlowKPeriod = 3, int optInSlowDPeriod = 3)
        {
            if (optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInSlowKPeriod < 1 || optInSlowKPeriod > 100000 ||
                optInSlowDPeriod < 1 || optInSlowDPeriod > 100000)
            {
                return -1;
            }

            int retValue = optInFastKPeriod - 1;
            retValue += MaLookback(optInSlowKMAType, optInSlowKPeriod);
            retValue += MaLookback(optInSlowDMAType, optInSlowDPeriod);

            return retValue;
        }
    }
}
