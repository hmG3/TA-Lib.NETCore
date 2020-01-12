using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode UltOsc(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod1,
            int optInTimePeriod2, int optInTimePeriod3, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int outIdx;
            int[] usedFlag = new int[3];
            int[] periods = new int[3];
            int[] sortedPeriods = new int[3];
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

            if (optInTimePeriod1 == -2147483648)
            {
                optInTimePeriod1 = 7;
            }
            else if ((optInTimePeriod1 < 1) || (optInTimePeriod1 > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod2 == -2147483648)
            {
                optInTimePeriod2 = 14;
            }
            else if ((optInTimePeriod2 < 1) || (optInTimePeriod2 > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod3 == -2147483648)
            {
                optInTimePeriod3 = 0x1c;
            }
            else if ((optInTimePeriod3 < 1) || (optInTimePeriod3 > 0x186a0))
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
            int i = 0;
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

                    double a1Total = 0.0;
                    double b1Total = 0.0;
                    i = (startIdx - optInTimePeriod1) + 1;
                    while (i < startIdx)
                    {
                        double num7;
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        if (tempLT < tempCY)
                        {
                            num7 = tempLT;
                        }
                        else
                        {
                            num7 = tempCY;
                        }

                        trueLow = num7;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total += closeMinusTrueLow;
                        b1Total += trueRange;
                        i++;
                    }

                    double a2Total = 0.0;
                    double b2Total = 0.0;
                    i = (startIdx - optInTimePeriod2) + 1;
                    while (i < startIdx)
                    {
                        double num6;
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        if (tempLT < tempCY)
                        {
                            num6 = tempLT;
                        }
                        else
                        {
                            num6 = tempCY;
                        }

                        trueLow = num6;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a2Total += closeMinusTrueLow;
                        b2Total += trueRange;
                        i++;
                    }

                    double a3Total = 0.0;
                    double b3Total = 0.0;
                    i = (startIdx - optInTimePeriod3) + 1;
                    while (i < startIdx)
                    {
                        double num5;
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        if (tempLT < tempCY)
                        {
                            num5 = tempLT;
                        }
                        else
                        {
                            num5 = tempCY;
                        }

                        trueLow = num5;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
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
                    int trailingIdx1 = (today - optInTimePeriod1) + 1;
                    int trailingIdx2 = (today - optInTimePeriod2) + 1;
                    for (int trailingIdx3 = (today - optInTimePeriod3) + 1; today <= endIdx; trailingIdx3++)
                    {
                        double num;
                        double num2;
                        double num3;
                        double num4;
                        tempLT = inLow[today];
                        tempHT = inHigh[today];
                        tempCY = inClose[today - 1];
                        if (tempLT < tempCY)
                        {
                            num4 = tempLT;
                        }
                        else
                        {
                            num4 = tempCY;
                        }

                        trueLow = num4;
                        closeMinusTrueLow = inClose[today] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
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
                        double output = 0.0;
                        if ((-1E-08 >= b1Total) || (b1Total >= 1E-08))
                        {
                            output += 4.0 * (a1Total / b1Total);
                        }

                        if ((-1E-08 >= b2Total) || (b2Total >= 1E-08))
                        {
                            output += 2.0 * (a2Total / b2Total);
                        }

                        if ((-1E-08 >= b3Total) || (b3Total >= 1E-08))
                        {
                            output += a3Total / b3Total;
                        }

                        tempLT = inLow[trailingIdx1];
                        tempHT = inHigh[trailingIdx1];
                        tempCY = inClose[trailingIdx1 - 1];
                        if (tempLT < tempCY)
                        {
                            num3 = tempLT;
                        }
                        else
                        {
                            num3 = tempCY;
                        }

                        trueLow = num3;
                        closeMinusTrueLow = inClose[trailingIdx1] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total -= closeMinusTrueLow;
                        b1Total -= trueRange;
                        tempLT = inLow[trailingIdx2];
                        tempHT = inHigh[trailingIdx2];
                        tempCY = inClose[trailingIdx2 - 1];
                        if (tempLT < tempCY)
                        {
                            num2 = tempLT;
                        }
                        else
                        {
                            num2 = tempCY;
                        }

                        trueLow = num2;
                        closeMinusTrueLow = inClose[trailingIdx2] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a2Total -= closeMinusTrueLow;
                        b2Total -= trueRange;
                        tempLT = inLow[trailingIdx3];
                        tempHT = inHigh[trailingIdx3];
                        tempCY = inClose[trailingIdx3 - 1];
                        if (tempLT < tempCY)
                        {
                            num = tempLT;
                        }
                        else
                        {
                            num = tempCY;
                        }

                        trueLow = num;
                        closeMinusTrueLow = inClose[trailingIdx3] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
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

                int longestPeriod = 0;
                int longestIndex = 0;
                for (int j = 0; j < 3; j++)
                {
                    if ((usedFlag[j] == 0) && (periods[j] > longestPeriod))
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

        public static RetCode UltOsc(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, int optInTimePeriod1,
            int optInTimePeriod2, int optInTimePeriod3, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int outIdx;
            int[] usedFlag = new int[3];
            int[] periods = new int[3];
            int[] sortedPeriods = new int[3];
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

            if (optInTimePeriod1 == -2147483648)
            {
                optInTimePeriod1 = 7;
            }
            else if ((optInTimePeriod1 < 1) || (optInTimePeriod1 > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod2 == -2147483648)
            {
                optInTimePeriod2 = 14;
            }
            else if ((optInTimePeriod2 < 1) || (optInTimePeriod2 > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod3 == -2147483648)
            {
                optInTimePeriod3 = 0x1c;
            }
            else if ((optInTimePeriod3 < 1) || (optInTimePeriod3 > 0x186a0))
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
            int i = 0;
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

                    double a1Total = 0.0;
                    double b1Total = 0.0;
                    i = (startIdx - optInTimePeriod1) + 1;
                    while (i < startIdx)
                    {
                        double num7;
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        if (tempLT < tempCY)
                        {
                            num7 = tempLT;
                        }
                        else
                        {
                            num7 = tempCY;
                        }

                        trueLow = num7;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total += closeMinusTrueLow;
                        b1Total += trueRange;
                        i++;
                    }

                    double a2Total = 0.0;
                    double b2Total = 0.0;
                    i = (startIdx - optInTimePeriod2) + 1;
                    while (i < startIdx)
                    {
                        double num6;
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        if (tempLT < tempCY)
                        {
                            num6 = tempLT;
                        }
                        else
                        {
                            num6 = tempCY;
                        }

                        trueLow = num6;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a2Total += closeMinusTrueLow;
                        b2Total += trueRange;
                        i++;
                    }

                    double a3Total = 0.0;
                    double b3Total = 0.0;
                    i = (startIdx - optInTimePeriod3) + 1;
                    while (i < startIdx)
                    {
                        double num5;
                        tempLT = inLow[i];
                        tempHT = inHigh[i];
                        tempCY = inClose[i - 1];
                        if (tempLT < tempCY)
                        {
                            num5 = tempLT;
                        }
                        else
                        {
                            num5 = tempCY;
                        }

                        trueLow = num5;
                        closeMinusTrueLow = inClose[i] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
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
                    int trailingIdx1 = (today - optInTimePeriod1) + 1;
                    int trailingIdx2 = (today - optInTimePeriod2) + 1;
                    for (int trailingIdx3 = (today - optInTimePeriod3) + 1; today <= endIdx; trailingIdx3++)
                    {
                        double num;
                        double num2;
                        double num3;
                        double num4;
                        tempLT = inLow[today];
                        tempHT = inHigh[today];
                        tempCY = inClose[today - 1];
                        if (tempLT < tempCY)
                        {
                            num4 = tempLT;
                        }
                        else
                        {
                            num4 = tempCY;
                        }

                        trueLow = num4;
                        closeMinusTrueLow = inClose[today] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
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
                        double output = 0.0;
                        if ((-1E-08 >= b1Total) || (b1Total >= 1E-08))
                        {
                            output += 4.0 * (a1Total / b1Total);
                        }

                        if ((-1E-08 >= b2Total) || (b2Total >= 1E-08))
                        {
                            output += 2.0 * (a2Total / b2Total);
                        }

                        if ((-1E-08 >= b3Total) || (b3Total >= 1E-08))
                        {
                            output += a3Total / b3Total;
                        }

                        tempLT = inLow[trailingIdx1];
                        tempHT = inHigh[trailingIdx1];
                        tempCY = inClose[trailingIdx1 - 1];
                        if (tempLT < tempCY)
                        {
                            num3 = tempLT;
                        }
                        else
                        {
                            num3 = tempCY;
                        }

                        trueLow = num3;
                        closeMinusTrueLow = inClose[trailingIdx1] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a1Total -= closeMinusTrueLow;
                        b1Total -= trueRange;
                        tempLT = inLow[trailingIdx2];
                        tempHT = inHigh[trailingIdx2];
                        tempCY = inClose[trailingIdx2 - 1];
                        if (tempLT < tempCY)
                        {
                            num2 = tempLT;
                        }
                        else
                        {
                            num2 = tempCY;
                        }

                        trueLow = num2;
                        closeMinusTrueLow = inClose[trailingIdx2] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        a2Total -= closeMinusTrueLow;
                        b2Total -= trueRange;
                        tempLT = inLow[trailingIdx3];
                        tempHT = inHigh[trailingIdx3];
                        tempCY = inClose[trailingIdx3 - 1];
                        if (tempLT < tempCY)
                        {
                            num = tempLT;
                        }
                        else
                        {
                            num = tempCY;
                        }

                        trueLow = num;
                        closeMinusTrueLow = inClose[trailingIdx3] - trueLow;
                        trueRange = tempHT - tempLT;
                        tempDouble = Math.Abs((double) (tempCY - tempHT));
                        if (tempDouble > trueRange)
                        {
                            trueRange = tempDouble;
                        }

                        tempDouble = Math.Abs((double) (tempCY - tempLT));
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

                int longestPeriod = 0;
                int longestIndex = 0;
                for (int j = 0; j < 3; j++)
                {
                    if ((usedFlag[j] == 0) && (periods[j] > longestPeriod))
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

        public static int UltOscLookback(int optInTimePeriod1, int optInTimePeriod2, int optInTimePeriod3)
        {
            int num2;
            if (optInTimePeriod1 == -2147483648)
            {
                optInTimePeriod1 = 7;
            }
            else if ((optInTimePeriod1 < 1) || (optInTimePeriod1 > 0x186a0))
            {
                return -1;
            }

            if (optInTimePeriod2 == -2147483648)
            {
                optInTimePeriod2 = 14;
            }
            else if ((optInTimePeriod2 < 1) || (optInTimePeriod2 > 0x186a0))
            {
                return -1;
            }

            if (optInTimePeriod3 == -2147483648)
            {
                optInTimePeriod3 = 0x1c;
            }
            else if ((optInTimePeriod3 < 1) || (optInTimePeriod3 > 0x186a0))
            {
                return -1;
            }

            if (((optInTimePeriod1 <= optInTimePeriod2) ? optInTimePeriod2 : optInTimePeriod1) > optInTimePeriod3)
            {
                num2 = (optInTimePeriod1 <= optInTimePeriod2) ? optInTimePeriod2 : optInTimePeriod1;
            }
            else
            {
                num2 = optInTimePeriod3;
            }

            int maxPeriod = num2;
            return (SmaLookback(maxPeriod) + 1);
        }
    }
}
