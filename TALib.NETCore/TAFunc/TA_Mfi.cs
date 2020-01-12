using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Mfi(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, double[] inVolume,
            int optInTimePeriod, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int mflow_Idx = 0;
            int maxIdx_mflow = 0x31;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || ((inClose == null) || (inVolume == null)))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod <= 0)
            {
                return RetCode.AllocErr;
            }

            MoneyFlow[] mflow = new MoneyFlow[optInTimePeriod];
            for (int _mflow_index = 0; _mflow_index < mflow.Length; _mflow_index++)
            {
                mflow[_mflow_index] = new MoneyFlow();
            }

            if (mflow == null)
            {
                return RetCode.AllocErr;
            }

            maxIdx_mflow = optInTimePeriod - 1;
            outBegIdx = 0;
            outNBElement = 0;
            int lookbackTotal = optInTimePeriod + ((int) Globals.unstablePeriod[14]);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx <= endIdx)
            {
                double tempValue1;
                double tempValue2;
                int outIdx = 0;
                int today = startIdx - lookbackTotal;
                double prevValue = ((inHigh[today] + inLow[today]) + inClose[today]) / 3.0;
                double posSumMF = 0.0;
                double negSumMF = 0.0;
                today++;
                for (int i = optInTimePeriod; i > 0; i--)
                {
                    tempValue1 = ((inHigh[today] + inLow[today]) + inClose[today]) / 3.0;
                    tempValue2 = tempValue1 - prevValue;
                    prevValue = tempValue1;
                    tempValue1 *= inVolume[today];
                    today++;
                    if (tempValue2 < 0.0)
                    {
                        mflow[mflow_Idx].negative = tempValue1;
                        negSumMF += tempValue1;
                        mflow[mflow_Idx].positive = 0.0;
                    }
                    else if (tempValue2 > 0.0)
                    {
                        mflow[mflow_Idx].positive = tempValue1;
                        posSumMF += tempValue1;
                        mflow[mflow_Idx].negative = 0.0;
                    }
                    else
                    {
                        mflow[mflow_Idx].positive = 0.0;
                        mflow[mflow_Idx].negative = 0.0;
                    }

                    mflow_Idx++;
                    if (mflow_Idx > maxIdx_mflow)
                    {
                        mflow_Idx = 0;
                    }
                }

                if (today > startIdx)
                {
                    tempValue1 = posSumMF + negSumMF;
                    if (tempValue1 < 1.0)
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 100.0 * (posSumMF / tempValue1);
                        outIdx++;
                    }
                }
                else
                {
                    while (today < startIdx)
                    {
                        posSumMF -= mflow[mflow_Idx].positive;
                        negSumMF -= mflow[mflow_Idx].negative;
                        tempValue1 = ((inHigh[today] + inLow[today]) + inClose[today]) / 3.0;
                        tempValue2 = tempValue1 - prevValue;
                        prevValue = tempValue1;
                        tempValue1 *= inVolume[today];
                        today++;
                        if (tempValue2 < 0.0)
                        {
                            mflow[mflow_Idx].negative = tempValue1;
                            negSumMF += tempValue1;
                            mflow[mflow_Idx].positive = 0.0;
                        }
                        else if (tempValue2 > 0.0)
                        {
                            mflow[mflow_Idx].positive = tempValue1;
                            posSumMF += tempValue1;
                            mflow[mflow_Idx].negative = 0.0;
                        }
                        else
                        {
                            mflow[mflow_Idx].positive = 0.0;
                            mflow[mflow_Idx].negative = 0.0;
                        }

                        mflow_Idx++;
                        if (mflow_Idx > maxIdx_mflow)
                        {
                            mflow_Idx = 0;
                        }
                    }
                }

                while (today <= endIdx)
                {
                    posSumMF -= mflow[mflow_Idx].positive;
                    negSumMF -= mflow[mflow_Idx].negative;
                    tempValue1 = ((inHigh[today] + inLow[today]) + inClose[today]) / 3.0;
                    tempValue2 = tempValue1 - prevValue;
                    prevValue = tempValue1;
                    tempValue1 *= inVolume[today];
                    today++;
                    if (tempValue2 < 0.0)
                    {
                        mflow[mflow_Idx].negative = tempValue1;
                        negSumMF += tempValue1;
                        mflow[mflow_Idx].positive = 0.0;
                    }
                    else if (tempValue2 > 0.0)
                    {
                        mflow[mflow_Idx].positive = tempValue1;
                        posSumMF += tempValue1;
                        mflow[mflow_Idx].negative = 0.0;
                    }
                    else
                    {
                        mflow[mflow_Idx].positive = 0.0;
                        mflow[mflow_Idx].negative = 0.0;
                    }

                    tempValue1 = posSumMF + negSumMF;
                    if (tempValue1 < 1.0)
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 100.0 * (posSumMF / tempValue1);
                        outIdx++;
                    }

                    mflow_Idx++;
                    if (mflow_Idx > maxIdx_mflow)
                    {
                        mflow_Idx = 0;
                    }
                }

                outBegIdx = startIdx;
                outNBElement = outIdx;
            }

            return RetCode.Success;
        }

        public static RetCode Mfi(int startIdx, int endIdx, float[] inHigh, float[] inLow, float[] inClose, float[] inVolume,
            int optInTimePeriod, ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
            int mflow_Idx = 0;
            int maxIdx_mflow = 0x31;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inHigh == null) || (inLow == null)) || ((inClose == null) || (inVolume == null)))
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod <= 0)
            {
                return RetCode.AllocErr;
            }

            MoneyFlow[] mflow = new MoneyFlow[optInTimePeriod];
            for (int _mflow_index = 0; _mflow_index < mflow.Length; _mflow_index++)
            {
                mflow[_mflow_index] = new MoneyFlow();
            }

            if (mflow == null)
            {
                return RetCode.AllocErr;
            }

            maxIdx_mflow = optInTimePeriod - 1;
            outBegIdx = 0;
            outNBElement = 0;
            int lookbackTotal = optInTimePeriod + ((int) Globals.unstablePeriod[14]);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx <= endIdx)
            {
                double tempValue1;
                double tempValue2;
                int outIdx = 0;
                int today = startIdx - lookbackTotal;
                double prevValue = ((inHigh[today] + inLow[today]) + inClose[today]) / 3.0;
                double posSumMF = 0.0;
                double negSumMF = 0.0;
                today++;
                for (int i = optInTimePeriod; i > 0; i--)
                {
                    tempValue1 = ((inHigh[today] + inLow[today]) + inClose[today]) / 3.0;
                    tempValue2 = tempValue1 - prevValue;
                    prevValue = tempValue1;
                    tempValue1 *= inVolume[today];
                    today++;
                    if (tempValue2 < 0.0)
                    {
                        mflow[mflow_Idx].negative = tempValue1;
                        negSumMF += tempValue1;
                        mflow[mflow_Idx].positive = 0.0;
                    }
                    else if (tempValue2 > 0.0)
                    {
                        mflow[mflow_Idx].positive = tempValue1;
                        posSumMF += tempValue1;
                        mflow[mflow_Idx].negative = 0.0;
                    }
                    else
                    {
                        mflow[mflow_Idx].positive = 0.0;
                        mflow[mflow_Idx].negative = 0.0;
                    }

                    mflow_Idx++;
                    if (mflow_Idx > maxIdx_mflow)
                    {
                        mflow_Idx = 0;
                    }
                }

                if (today > startIdx)
                {
                    tempValue1 = posSumMF + negSumMF;
                    if (tempValue1 < 1.0)
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 100.0 * (posSumMF / tempValue1);
                        outIdx++;
                    }
                }
                else
                {
                    while (today < startIdx)
                    {
                        posSumMF -= mflow[mflow_Idx].positive;
                        negSumMF -= mflow[mflow_Idx].negative;
                        tempValue1 = ((inHigh[today] + inLow[today]) + inClose[today]) / 3.0;
                        tempValue2 = tempValue1 - prevValue;
                        prevValue = tempValue1;
                        tempValue1 *= inVolume[today];
                        today++;
                        if (tempValue2 < 0.0)
                        {
                            mflow[mflow_Idx].negative = tempValue1;
                            negSumMF += tempValue1;
                            mflow[mflow_Idx].positive = 0.0;
                        }
                        else if (tempValue2 > 0.0)
                        {
                            mflow[mflow_Idx].positive = tempValue1;
                            posSumMF += tempValue1;
                            mflow[mflow_Idx].negative = 0.0;
                        }
                        else
                        {
                            mflow[mflow_Idx].positive = 0.0;
                            mflow[mflow_Idx].negative = 0.0;
                        }

                        mflow_Idx++;
                        if (mflow_Idx > maxIdx_mflow)
                        {
                            mflow_Idx = 0;
                        }
                    }
                }

                while (today <= endIdx)
                {
                    posSumMF -= mflow[mflow_Idx].positive;
                    negSumMF -= mflow[mflow_Idx].negative;
                    tempValue1 = ((inHigh[today] + inLow[today]) + inClose[today]) / 3.0;
                    tempValue2 = tempValue1 - prevValue;
                    prevValue = tempValue1;
                    tempValue1 *= inVolume[today];
                    today++;
                    if (tempValue2 < 0.0)
                    {
                        mflow[mflow_Idx].negative = tempValue1;
                        negSumMF += tempValue1;
                        mflow[mflow_Idx].positive = 0.0;
                    }
                    else if (tempValue2 > 0.0)
                    {
                        mflow[mflow_Idx].positive = tempValue1;
                        posSumMF += tempValue1;
                        mflow[mflow_Idx].negative = 0.0;
                    }
                    else
                    {
                        mflow[mflow_Idx].positive = 0.0;
                        mflow[mflow_Idx].negative = 0.0;
                    }

                    tempValue1 = posSumMF + negSumMF;
                    if (tempValue1 < 1.0)
                    {
                        outReal[outIdx] = 0.0;
                        outIdx++;
                    }
                    else
                    {
                        outReal[outIdx] = 100.0 * (posSumMF / tempValue1);
                        outIdx++;
                    }

                    mflow_Idx++;
                    if (mflow_Idx > maxIdx_mflow)
                    {
                        mflow_Idx = 0;
                    }
                }

                outBegIdx = startIdx;
                outNBElement = outIdx;
            }

            return RetCode.Success;
        }

        public static int MfiLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 14;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return (optInTimePeriod + ((int) Globals.unstablePeriod[14]));
        }
    }
}
