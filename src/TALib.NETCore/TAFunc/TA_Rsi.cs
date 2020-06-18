using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode Rsi(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = RsiLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int outIdx = default;
            int today = startIdx - lookbackTotal;
            double prevValue = inReal[today];
            double prevGain;
            double prevLoss;
            if (Globals.UnstablePeriod[(int) FuncUnstId.Rsi] == 0 && Globals.Compatibility == Compatibility.Metastock)
            {
                double savePrevValue = prevValue;
                double tempValue1;
                double tempValue2;
                prevGain = default;
                prevLoss = default;
                for (int i = optInTimePeriod; i > 0; i--)
                {
                    tempValue1 = inReal[today++];
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
                outReal[outIdx++] = !TA_IsZero(tempValue1) ? 100.0 * (tempValue2 / tempValue1) : 0.0;

                if (today > endIdx)
                {
                    outBegIdx = startIdx;
                    outNbElement = outIdx;
                    return RetCode.Success;
                }

                today -= optInTimePeriod;
                prevValue = savePrevValue;
            }

            prevGain = default;
            prevLoss = default;
            today++;
            for (int i = optInTimePeriod; i > 0; i--)
            {
                double tempValue1 = inReal[today++];
                double tempValue2 = tempValue1 - prevValue;
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
                double tempValue1 = prevGain + prevLoss;
                outReal[outIdx++] = !TA_IsZero(tempValue1) ? 100.0 * (prevGain / tempValue1) : 0.0;
            }
            else
            {
                while (today < startIdx)
                {
                    double tempValue1 = inReal[today];
                    double tempValue2 = tempValue1 - prevValue;
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
                double tempValue1 = inReal[today++];
                double tempValue2 = tempValue1 - prevValue;
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
                outReal[outIdx++] = !TA_IsZero(tempValue1) ? 100.0 * (prevGain / tempValue1) : 0.0;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Rsi(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = RsiLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int outIdx = default;
            int today = startIdx - lookbackTotal;
            decimal prevValue = inReal[today];
            decimal prevGain;
            decimal prevLoss;
            if (Globals.UnstablePeriod[(int) FuncUnstId.Rsi] == 0 && Globals.Compatibility == Compatibility.Metastock)
            {
                decimal savePrevValue = prevValue;
                decimal tempValue1;
                decimal tempValue2;
                prevGain = default;
                prevLoss = default;
                for (int i = optInTimePeriod; i > 0; i--)
                {
                    tempValue1 = inReal[today++];
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
                outReal[outIdx++] = !TA_IsZero(tempValue1) ? 100m * (tempValue2 / tempValue1) : Decimal.Zero;

                if (today > endIdx)
                {
                    outBegIdx = startIdx;
                    outNbElement = outIdx;
                    return RetCode.Success;
                }

                today -= optInTimePeriod;
                prevValue = savePrevValue;
            }

            prevGain = default;
            prevLoss = default;
            today++;
            for (int i = optInTimePeriod; i > 0; i--)
            {
                decimal tempValue1 = inReal[today++];
                decimal tempValue2 = tempValue1 - prevValue;
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
                decimal tempValue1 = prevGain + prevLoss;
                outReal[outIdx++] = !TA_IsZero(tempValue1) ? 100m * (prevGain / tempValue1) : Decimal.Zero;
            }
            else
            {
                while (today < startIdx)
                {
                    decimal tempValue1 = inReal[today];
                    decimal tempValue2 = tempValue1 - prevValue;
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
                decimal tempValue1 = inReal[today++];
                decimal tempValue2 = tempValue1 - prevValue;
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
                outReal[outIdx++] = !TA_IsZero(tempValue1) ? 100m * (prevGain / tempValue1) : Decimal.Zero;
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

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
