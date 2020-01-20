using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Rsi(int startIdx, in int endIdx, in double[] inReal, ref int outBegIdx, ref int outNBElement,
            double[] outReal, int optInTimePeriod = 14)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
            int lookbackTotal = RsiLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx <= endIdx)
            {
                double tempValue1;
                double prevGain;
                double prevLoss;
                double tempValue2;
                int i;
                int outIdx = default;
                if (optInTimePeriod == 1)
                {
                    outBegIdx = startIdx;
                    i = endIdx - startIdx + 1;
                    outNBElement = i;
                    Array.Copy(inReal, startIdx, outReal, 0, i);
                    return RetCode.Success;
                }

                int today = startIdx - lookbackTotal;
                double prevValue = inReal[today];
                if (Globals.UnstablePeriod[(int) FuncUnstId.Rsi] == 0 && Globals.Compatibility == Compatibility.Metastock)
                {
                    double savePrevValue = prevValue;
                    prevGain = 0.0;
                    prevLoss = 0.0;
                    for (i = optInTimePeriod; i > 0; i--)
                    {
                        tempValue1 = inReal[today];
                        today++;
                        tempValue2 = tempValue1 - prevValue;
                        prevValue = tempValue1;
                        if (tempValue2 < 0.0)
                        {
                            prevLoss -= tempValue2;
                        }
                        else
                        {
                            prevGain += tempValue2;
                        }
                    }

                    tempValue1 = prevLoss / optInTimePeriod;
                    tempValue2 = prevGain / optInTimePeriod;
                    tempValue1 = tempValue2 + tempValue1;
                    if (-1E-08 >= tempValue1 || tempValue1 >= 1E-08)
                    {
                        outReal[outIdx] = 100.0 * (tempValue2 / tempValue1);
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }

                    if (today > endIdx)
                    {
                        outBegIdx = startIdx;
                        outNBElement = outIdx;
                        return RetCode.Success;
                    }

                    today -= optInTimePeriod;
                    prevValue = savePrevValue;
                }

                prevGain = 0.0;
                prevLoss = 0.0;
                today++;
                for (i = optInTimePeriod; i > 0; i--)
                {
                    tempValue1 = inReal[today];
                    today++;
                    tempValue2 = tempValue1 - prevValue;
                    prevValue = tempValue1;
                    if (tempValue2 < 0.0)
                    {
                        prevLoss -= tempValue2;
                    }
                    else
                    {
                        prevGain += tempValue2;
                    }
                }

                prevLoss /= optInTimePeriod;
                prevGain /= optInTimePeriod;
                if (today > startIdx)
                {
                    tempValue1 = prevGain + prevLoss;
                    if (-1E-08 >= tempValue1 || tempValue1 >= 1E-08)
                    {
                        outReal[outIdx] = 100.0 * (prevGain / tempValue1);
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                }
                else
                {
                    while (today < startIdx)
                    {
                        tempValue1 = inReal[today];
                        tempValue2 = tempValue1 - prevValue;
                        prevValue = tempValue1;
                        prevLoss *= optInTimePeriod - 1;
                        prevGain *= optInTimePeriod - 1;
                        if (tempValue2 < 0.0)
                        {
                            prevLoss -= tempValue2;
                        }
                        else
                        {
                            prevGain += tempValue2;
                        }

                        prevLoss /= optInTimePeriod;
                        prevGain /= optInTimePeriod;
                        today++;
                    }
                }

                while (today <= endIdx)
                {
                    tempValue1 = inReal[today];
                    today++;
                    tempValue2 = tempValue1 - prevValue;
                    prevValue = tempValue1;
                    prevLoss *= optInTimePeriod - 1;
                    prevGain *= optInTimePeriod - 1;
                    if (tempValue2 < 0.0)
                    {
                        prevLoss -= tempValue2;
                    }
                    else
                    {
                        prevGain += tempValue2;
                    }

                    prevLoss /= optInTimePeriod;
                    prevGain /= optInTimePeriod;
                    tempValue1 = prevGain + prevLoss;
                    if (-1E-08 >= tempValue1 || tempValue1 >= 1E-08)
                    {
                        outReal[outIdx] = 100.0 * (prevGain / tempValue1);
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                }

                outBegIdx = startIdx;
                outNBElement = outIdx;
            }

            return RetCode.Success;
        }

        public static RetCode Rsi(int startIdx, in int endIdx, in decimal[] inReal, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, int optInTimePeriod = 14)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            outBegIdx = 0;
            outNBElement = 0;
            int lookbackTotal = RsiLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx <= endIdx)
            {
                decimal tempValue1;
                decimal prevGain;
                decimal prevLoss;
                decimal tempValue2;
                int i;
                int outIdx = default;
                if (optInTimePeriod == 1)
                {
                    outBegIdx = startIdx;
                    i = endIdx - startIdx + 1;
                    outNBElement = i;
                    Array.Copy(inReal, startIdx, outReal, 0, i);
                    return RetCode.Success;
                }

                int today = startIdx - lookbackTotal;
                decimal prevValue = inReal[today];
                if (Globals.UnstablePeriod[(int) FuncUnstId.Rsi] == 0 && Globals.Compatibility == Compatibility.Metastock)
                {
                    decimal savePrevValue = prevValue;
                    prevGain = Decimal.Zero;
                    prevLoss = Decimal.Zero;
                    for (i = optInTimePeriod; i > 0; i--)
                    {
                        tempValue1 = inReal[today];
                        today++;
                        tempValue2 = tempValue1 - prevValue;
                        prevValue = tempValue1;
                        if (tempValue2 < Decimal.Zero)
                        {
                            prevLoss -= tempValue2;
                        }
                        else
                        {
                            prevGain += tempValue2;
                        }
                    }

                    tempValue1 = prevLoss / optInTimePeriod;
                    tempValue2 = prevGain / optInTimePeriod;
                    tempValue1 = tempValue2 + tempValue1;
                    if (-1E-08m >= tempValue1 || tempValue1 >= 1E-08m)
                    {
                        outReal[outIdx] = 100m * (tempValue2 / tempValue1);
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = Decimal.Zero;
                        outIdx++;
                    }

                    if (today > endIdx)
                    {
                        outBegIdx = startIdx;
                        outNBElement = outIdx;
                        return RetCode.Success;
                    }

                    today -= optInTimePeriod;
                    prevValue = savePrevValue;
                }

                prevGain = Decimal.Zero;
                prevLoss = Decimal.Zero;
                today++;
                for (i = optInTimePeriod; i > 0; i--)
                {
                    tempValue1 = inReal[today];
                    today++;
                    tempValue2 = tempValue1 - prevValue;
                    prevValue = tempValue1;
                    if (tempValue2 < Decimal.Zero)
                    {
                        prevLoss -= tempValue2;
                    }
                    else
                    {
                        prevGain += tempValue2;
                    }
                }

                prevLoss /= optInTimePeriod;
                prevGain /= optInTimePeriod;
                if (today > startIdx)
                {
                    tempValue1 = prevGain + prevLoss;
                    if (-1E-08m >= tempValue1 || tempValue1 >= 1E-08m)
                    {
                        outReal[outIdx] = 100m * (prevGain / tempValue1);
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = Decimal.Zero;
                        outIdx++;
                    }
                }
                else
                {
                    while (today < startIdx)
                    {
                        tempValue1 = inReal[today];
                        tempValue2 = tempValue1 - prevValue;
                        prevValue = tempValue1;
                        prevLoss *= optInTimePeriod - 1;
                        prevGain *= optInTimePeriod - 1;
                        if (tempValue2 < Decimal.Zero)
                        {
                            prevLoss -= tempValue2;
                        }
                        else
                        {
                            prevGain += tempValue2;
                        }

                        prevLoss /= optInTimePeriod;
                        prevGain /= optInTimePeriod;
                        today++;
                    }
                }

                while (today <= endIdx)
                {
                    tempValue1 = inReal[today];
                    today++;
                    tempValue2 = tempValue1 - prevValue;
                    prevValue = tempValue1;
                    prevLoss *= optInTimePeriod - 1;
                    prevGain *= optInTimePeriod - 1;
                    if (tempValue2 < Decimal.Zero)
                    {
                        prevLoss -= tempValue2;
                    }
                    else
                    {
                        prevGain += tempValue2;
                    }

                    prevLoss /= optInTimePeriod;
                    prevGain /= optInTimePeriod;
                    tempValue1 = prevGain + prevLoss;
                    if (-1E-08m >= tempValue1 || tempValue1 >= 1E-08m)
                    {
                        outReal[outIdx] = 100m * (prevGain / tempValue1);
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = Decimal.Zero;
                        outIdx++;
                    }
                }

                outBegIdx = startIdx;
                outNBElement = outIdx;
            }

            return RetCode.Success;
        }

        public static int RsiLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            int retValue = optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Rsi];
            if (Globals.Compatibility == Compatibility.Metastock)
            {
                retValue--;
            }

            return retValue;
        }
    }
}
