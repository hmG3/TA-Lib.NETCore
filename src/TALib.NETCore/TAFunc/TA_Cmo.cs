using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode Cmo(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
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

            outBegIdx = 0;
            outNbElement = 0;

            int lookbackTotal = CmoLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double prevLoss;
            double prevGain;
            double tempValue1;
            double tempValue2;
            int outIdx = default;
            int today = startIdx - lookbackTotal;
            double prevValue = inReal[today];
            if (Globals.UnstablePeriod[(int) FuncUnstId.Cmo] == 0 && Globals.Compatibility == Compatibility.Metastock)
            {
                double savePrevValue = prevValue;
                prevGain = 0.0;
                prevLoss = 0.0;
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
                double tempValue3 = tempValue2 - tempValue1;
                double tempValue4 = tempValue1 + tempValue2;
                if (!TA_IsZero(tempValue4))
                {
                    outReal[outIdx++] = 100.0 * (tempValue3 / tempValue4);
                }
                else
                {
                    outReal[outIdx++] = 0.0;
                }

                if (today > endIdx)
                {
                    outBegIdx = startIdx;
                    outNbElement = outIdx;
                    return RetCode.Success;
                }

                today -= optInTimePeriod;
                prevValue = savePrevValue;
            }

            prevGain = 0.0;
            prevLoss = 0.0;
            today++;
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

            prevLoss /= optInTimePeriod;
            prevGain /= optInTimePeriod;
            if (today > startIdx)
            {
                tempValue1 = prevGain + prevLoss;
                if (!TA_IsZero(tempValue1))
                {
                    outReal[outIdx++] = 100.0 * ((prevGain - prevLoss) / tempValue1);
                }
                else
                {
                    outReal[outIdx++] = 0.0;
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
                tempValue1 = inReal[today++];
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
                if (!TA_IsZero(tempValue1))
                {
                    outReal[outIdx++] = 100.0 * ((prevGain - prevLoss) / tempValue1);
                }
                else
                {
                    outReal[outIdx++] = 0.0;
                }
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Cmo(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
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

            int lookbackTotal = CmoLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal prevLoss;
            decimal prevGain;
            decimal tempValue1;
            decimal tempValue2;
            int outIdx = default;
            int today = startIdx - lookbackTotal;
            decimal prevValue = inReal[today];
            if (Globals.UnstablePeriod[(int) FuncUnstId.Cmo] == 0 && Globals.Compatibility == Compatibility.Metastock)
            {
                decimal savePrevValue = prevValue;
                prevGain = Decimal.Zero;
                prevLoss = Decimal.Zero;
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
                decimal tempValue3 = tempValue2 - tempValue1;
                decimal tempValue4 = tempValue1 + tempValue2;
                if (!TA_IsZero(tempValue4))
                {
                    outReal[outIdx++] = 100m * (tempValue3 / tempValue4);
                }
                else
                {
                    outReal[outIdx++] = Decimal.Zero;
                }

                if (today > endIdx)
                {
                    outBegIdx = startIdx;
                    outNbElement = outIdx;
                    return RetCode.Success;
                }

                today -= optInTimePeriod;
                prevValue = savePrevValue;
            }

            prevGain = Decimal.Zero;
            prevLoss = Decimal.Zero;
            today++;
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

            prevLoss /= optInTimePeriod;
            prevGain /= optInTimePeriod;
            if (today > startIdx)
            {
                tempValue1 = prevGain + prevLoss;
                if (!TA_IsZero(tempValue1))
                {
                    outReal[outIdx++] = 100m * ((prevGain - prevLoss) / tempValue1);
                }
                else
                {
                    outReal[outIdx++] = Decimal.Zero;
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
                tempValue1 = inReal[today++];
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
                if (!TA_IsZero(tempValue1))
                {
                    outReal[outIdx++] = 100m * ((prevGain - prevLoss) / tempValue1);
                }
                else
                {
                    outReal[outIdx++] = Decimal.Zero;
                }
            }

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int CmoLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            int retValue = optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.Cmo];
            if (Globals.Compatibility == Compatibility.Metastock)
            {
                retValue--;
            }

            return retValue;
        }
    }
}
