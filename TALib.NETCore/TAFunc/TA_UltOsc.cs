using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode UltOsc(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal, int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28)
        {
            int outIdx;
            var usedFlag = new int[3];
            var periods = new int[3];
            var sortedPeriods = new int[3];
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

            if (optInTimePeriod1 < 1 || optInTimePeriod1 > 100000 || optInTimePeriod2 < 1 || optInTimePeriod2 > 100000 ||
                optInTimePeriod3 < 1 || optInTimePeriod3 > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
            periods[0] = optInTimePeriod1;
            periods[1] = optInTimePeriod2;
            periods[2] = optInTimePeriod3;
            usedFlag[0] = 0;
            usedFlag[1] = 0;
            usedFlag[2] = 0;
            int i = default;
            while (true)
            {
                if (i >= 3)
                {
                    double trueRange;
                    double tempDouble;
                    double tempCY;
                    double tempLT;
                    double tempHT;
                    double closeMinusTrueLow;
                    double trueLow;
                    optInTimePeriod1 = sortedPeriods[2];
                    optInTimePeriod2 = sortedPeriods[1];
                    optInTimePeriod3 = sortedPeriods[0];
                    int lookbackTotal = UltOscLookback(optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);
                    if (startIdx < lookbackTotal)
                    {
                        startIdx = lookbackTotal;
                    }

                    if (startIdx > endIdx)
                    {
                        return RetCode.Success;
                    }

                    double a1Total = default;
                    double b1Total = default;
                    i = startIdx - optInTimePeriod1 + 1;
                    while (i < startIdx)
                    {
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        var num7 = tempLT < tempCY ? tempLT : tempCY;

                        trueLow = num7;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total += closeMinusTrueLow;
                        b1Total += trueRange;
                        i++;
                    }

                    double a2Total = default;
                    double b2Total = default;
                    i = startIdx - optInTimePeriod2 + 1;
                    while (i < startIdx)
                    {
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        var num6 = tempLT < tempCY ? tempLT : tempCY;

                        trueLow = num6;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a2Total += closeMinusTrueLow;
                        b2Total += trueRange;
                        i++;
                    }

                    double a3Total = default;
                    double b3Total = default;
                    i = startIdx - optInTimePeriod3 + 1;
                    while (i < startIdx)
                    {
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        var num5 = tempLT < tempCY ? tempLT : tempCY;

                        trueLow = num5;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a3Total += closeMinusTrueLow;
                        b3Total += trueRange;
                        i++;
                    }

                    int today = startIdx;
                    outIdx = 0;
                    int trailingIdx1 = today - optInTimePeriod1 + 1;
                    int trailingIdx2 = today - optInTimePeriod2 + 1;
                    for (var trailingIdx3 = today - optInTimePeriod3 + 1; today <= endIdx; trailingIdx3++)
                    {
                        tempLT = inLow[today];
                        tempHT = inHigh[today];
                        tempCY = inClose[today - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[today] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total += closeMinusTrueLow;
                        a2Total += closeMinusTrueLow;
                        a3Total += closeMinusTrueLow;
                        b1Total += trueRange;
                        b2Total += trueRange;
                        b3Total += trueRange;
                        double output = default;
                        if (-1E-08 >= b1Total || b1Total >= 1E-08)
                        {
                            output += 4.0 * (a1Total / b1Total);
                        }

                        if (-1E-08 >= b2Total || b2Total >= 1E-08)
                        {
                            output += 2.0 * (a2Total / b2Total);
                        }

                        if (-1E-08 >= b3Total || b3Total >= 1E-08)
                        {
                            output += a3Total / b3Total;
                        }

                        tempLT = inLow[trailingIdx1];
                        tempHT = inHigh[trailingIdx1];
                        tempCY = inClose[trailingIdx1 - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[trailingIdx1] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total -= closeMinusTrueLow;
                        b1Total -= trueRange;
                        tempLT = inLow[trailingIdx2];
                        tempHT = inHigh[trailingIdx2];
                        tempCY = inClose[trailingIdx2 - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[trailingIdx2] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a2Total -= closeMinusTrueLow;
                        b2Total -= trueRange;
                        tempLT = inLow[trailingIdx3];
                        tempHT = inHigh[trailingIdx3];
                        tempCY = inClose[trailingIdx3 - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[trailingIdx3] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a3Total -= closeMinusTrueLow;
                        b3Total -= trueRange;
                        outReal[outIdx] = 100.0 * (output / 7.0);
                        outIdx++;
                        today++;
                        trailingIdx1++;
                        trailingIdx2++;
                    }

                    break;
                }

                int longestPeriod = default;
                int longestIndex = default;
                for (var j = 0; j < 3; j++)
                {
                    if (usedFlag[j] == 0 && periods[j] > longestPeriod)
                    {
                        longestPeriod = periods[j];
                        longestIndex = j;
                    }
                }

                usedFlag[longestIndex] = 1;
                sortedPeriods[i] = longestPeriod;
                i++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode UltOsc(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, ref int outBegIdx,
            ref int outNBElement, decimal[] outReal, int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28)
        {
            int outIdx;
            var usedFlag = new int[3];
            var periods = new int[3];
            var sortedPeriods = new int[3];
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

            if (optInTimePeriod1 < 1 || optInTimePeriod1 > 100000 || optInTimePeriod2 < 1 || optInTimePeriod2 > 100000 ||
                optInTimePeriod3 < 1 || optInTimePeriod3 > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
            periods[0] = optInTimePeriod1;
            periods[1] = optInTimePeriod2;
            periods[2] = optInTimePeriod3;
            usedFlag[0] = 0;
            usedFlag[1] = 0;
            usedFlag[2] = 0;
            int i = default;
            while (true)
            {
                if (i >= 3)
                {
                    decimal trueRange;
                    decimal tempDouble;
                    decimal tempCY;
                    decimal tempLT;
                    decimal tempHT;
                    decimal closeMinusTrueLow;
                    decimal trueLow;
                    optInTimePeriod1 = sortedPeriods[2];
                    optInTimePeriod2 = sortedPeriods[1];
                    optInTimePeriod3 = sortedPeriods[0];
                    int lookbackTotal = UltOscLookback(optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);
                    if (startIdx < lookbackTotal)
                    {
                        startIdx = lookbackTotal;
                    }

                    if (startIdx > endIdx)
                    {
                        return RetCode.Success;
                    }

                    decimal a1Total = default;
                    decimal b1Total = default;
                    i = startIdx - optInTimePeriod1 + 1;
                    while (i < startIdx)
                    {
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total += closeMinusTrueLow;
                        b1Total += trueRange;
                        i++;
                    }

                    decimal a2Total = default;
                    decimal b2Total = default;
                    i = startIdx - optInTimePeriod2 + 1;
                    while (i < startIdx)
                    {
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a2Total += closeMinusTrueLow;
                        b2Total += trueRange;
                        i++;
                    }

                    decimal a3Total = default;
                    decimal b3Total = default;
                    i = startIdx - optInTimePeriod3 + 1;
                    while (i < startIdx)
                    {
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a3Total += closeMinusTrueLow;
                        b3Total += trueRange;
                        i++;
                    }

                    int today = startIdx;
                    outIdx = 0;
                    int trailingIdx1 = today - optInTimePeriod1 + 1;
                    int trailingIdx2 = today - optInTimePeriod2 + 1;
                    for (var trailingIdx3 = today - optInTimePeriod3 + 1; today <= endIdx; trailingIdx3++)
                    {
                        tempLT = inLow[today];
                        tempHT = inHigh[today];
                        tempCY = inClose[today - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[today] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total += closeMinusTrueLow;
                        a2Total += closeMinusTrueLow;
                        a3Total += closeMinusTrueLow;
                        b1Total += trueRange;
                        b2Total += trueRange;
                        b3Total += trueRange;
                        decimal output = default;
                        if (-1E-08m >= b1Total || b1Total >= 1E-08m)
                        {
                            output += 4m * (a1Total / b1Total);
                        }

                        if (-1E-08m >= b2Total || b2Total >= 1E-08m)
                        {
                            output += 2m * (a2Total / b2Total);
                        }

                        if (-1E-08m >= b3Total || b3Total >= 1E-08m)
                        {
                            output += a3Total / b3Total;
                        }

                        tempLT = inLow[trailingIdx1];
                        tempHT = inHigh[trailingIdx1];
                        tempCY = inClose[trailingIdx1 - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[trailingIdx1] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total -= closeMinusTrueLow;
                        b1Total -= trueRange;
                        tempLT = inLow[trailingIdx2];
                        tempHT = inHigh[trailingIdx2];
                        tempCY = inClose[trailingIdx2 - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[trailingIdx2] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a2Total -= closeMinusTrueLow;
                        b2Total -= trueRange;
                        tempLT = inLow[trailingIdx3];
                        tempHT = inHigh[trailingIdx3];
                        tempCY = inClose[trailingIdx3 - 1];

                        trueLow = tempLT < tempCY ? tempLT : tempCY;
                        closeMinusTrueLow = inClose[trailingIdx3] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs(tempCY - tempHT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs(tempCY - tempLT);
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a3Total -= closeMinusTrueLow;
                        b3Total -= trueRange;
                        outReal[outIdx] = 100m * (output / 7m);
                        outIdx++;
                        today++;
                        trailingIdx1++;
                        trailingIdx2++;
                    }

                    break;
                }

                int longestPeriod = default;
                int longestIndex = default;
                for (var j = 0; j < 3; j++)
                {
                    if (usedFlag[j] == 0 && periods[j] > longestPeriod)
                    {
                        longestPeriod = periods[j];
                        longestIndex = j;
                    }
                }

                usedFlag[longestIndex] = 1;
                sortedPeriods[i] = longestPeriod;
                i++;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int UltOscLookback(int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28)
        {
            if (optInTimePeriod1 < 1 || optInTimePeriod1 > 100000 || optInTimePeriod2 < 1 || optInTimePeriod2 > 100000 ||
                optInTimePeriod3 < 1 || optInTimePeriod3 > 100000)
            {
                return -1;
            }

            int maxPeriod = Math.Max(Math.Max(optInTimePeriod1, optInTimePeriod2), optInTimePeriod3);

            return SmaLookback(maxPeriod) + 1;
        }
    }
}
