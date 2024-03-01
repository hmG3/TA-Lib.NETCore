using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode StochF(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outFastK,
            double[] outFastD, out int outBegIdx, out int outNbElement, int optInFastKPeriod = 5, int optInFastDPeriod = 3,
            MAType optInFastDMAType = MAType.Sma)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outFastK == null || outFastD == null || optInFastKPeriod < 1 ||
                optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackK = optInFastKPeriod - 1;
            int lookbackFastD = MaLookback(optInFastDPeriod, optInFastDMAType);
            int lookbackTotal = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
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
            if (outFastK == inHigh || outFastK == inLow || outFastK == inClose)
            {
                tempBuffer = outFastK;
            }
            else if (outFastD == inHigh || outFastD == inLow || outFastD == inClose)
            {
                tempBuffer = outFastD;
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

                tempBuffer[outIdx++] = !diff.Equals(0.0) ? (inClose[today] - lowest) / diff : 0.0;

                trailingIdx++;
                today++;
            }

            RetCode retCode = Ma(tempBuffer, 0, outIdx - 1, outFastD, out _, out outNbElement, optInFastDPeriod, optInFastDMAType);
            if (retCode != RetCode.Success || outNbElement == 0)
            {
                return retCode;
            }

            Array.Copy(tempBuffer, lookbackFastD, outFastK, 0, outNbElement);
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode StochF(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outFastK,
            decimal[] outFastD, out int outBegIdx, out int outNbElement, int optInFastKPeriod = 5, int optInFastDPeriod = 3,
            MAType optInFastDMAType = MAType.Sma)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outFastK == null || outFastD == null || optInFastKPeriod < 1 ||
                optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackK = optInFastKPeriod - 1;
            int lookbackFastD = MaLookback(optInFastDPeriod, optInFastDMAType);
            int lookbackTotal = StochFLookback(optInFastKPeriod, optInFastDPeriod, optInFastDMAType);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
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
            if (outFastK == inHigh || outFastK == inLow || outFastK == inClose)
            {
                tempBuffer = outFastK;
            }
            else if (outFastD == inHigh || outFastD == inLow || outFastD == inClose)
            {
                tempBuffer = outFastD;
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

                tempBuffer[outIdx++] = diff != Decimal.Zero ? (inClose[today] - lowest) / diff : Decimal.Zero;

                trailingIdx++;
                today++;
            }

            RetCode retCode = Ma(tempBuffer, 0, outIdx - 1, outFastD, out _, out outNbElement, optInFastDPeriod, optInFastDMAType);
            if (retCode != RetCode.Success || outNbElement == 0)
            {
                return retCode;
            }

            Array.Copy(tempBuffer, lookbackFastD, outFastK, 0, outNbElement);
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int StochFLookback(int optInFastKPeriod = 5, int optInFastDPeriod = 3, MAType optInFastDMAType = MAType.Sma)
        {
            if (optInFastKPeriod < 1 || optInFastKPeriod > 100000 || optInFastDPeriod < 1 || optInFastDPeriod > 100000)
            {
                return -1;
            }

            int retValue = optInFastKPeriod - 1;

            return retValue + MaLookback(optInFastDPeriod, optInFastDMAType);
        }
    }
}
