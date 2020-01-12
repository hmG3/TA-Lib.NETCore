using System;

namespace TALib
{
    public partial class Core
    {
        private static GlobalsType Globals = new GlobalsType();

        static Core()
        {
            RestoreCandleDefaultSettings(CandleSettingType.AllCandleSettings);
        }

        public static Compatibility GetCompatibility()
        {
            return Globals.compatibility;
        }

        public static Int64 GetUnstablePeriod(FuncUnstId id)
        {
            if (id >= FuncUnstId.FuncUnstAll)
            {
                return 0;
            }

            return Globals.unstablePeriod[(int) id];
        }

        public static RetCode RestoreCandleDefaultSettings(CandleSettingType settingType)
        {
            switch (settingType)
            {
                case CandleSettingType.BodyLong:
                    SetCandleSettings(CandleSettingType.BodyLong, RangeType.RealBody, 10, 1.0);
                    break;

                case CandleSettingType.BodyVeryLong:
                    SetCandleSettings(CandleSettingType.BodyVeryLong, RangeType.RealBody, 10, 3.0);
                    break;

                case CandleSettingType.BodyShort:
                    SetCandleSettings(CandleSettingType.BodyShort, RangeType.RealBody, 10, 1.0);
                    break;

                case CandleSettingType.BodyDoji:
                    SetCandleSettings(CandleSettingType.BodyDoji, RangeType.HighLow, 10, 0.1);
                    break;

                case CandleSettingType.ShadowLong:
                    SetCandleSettings(CandleSettingType.ShadowLong, RangeType.RealBody, 0, 1.0);
                    break;

                case CandleSettingType.ShadowVeryLong:
                    SetCandleSettings(CandleSettingType.ShadowVeryLong, RangeType.RealBody, 0, 2.0);
                    break;

                case CandleSettingType.ShadowShort:
                    SetCandleSettings(CandleSettingType.ShadowShort, RangeType.Shadows, 10, 1.0);
                    break;

                case CandleSettingType.ShadowVeryShort:
                    SetCandleSettings(CandleSettingType.ShadowVeryShort, RangeType.HighLow, 10, 0.1);
                    break;

                case CandleSettingType.Near:
                    SetCandleSettings(CandleSettingType.Near, RangeType.HighLow, 5, 0.2);
                    break;

                case CandleSettingType.Far:
                    SetCandleSettings(CandleSettingType.Far, RangeType.HighLow, 5, 0.6);
                    break;

                case CandleSettingType.Equal:
                    SetCandleSettings(CandleSettingType.Equal, RangeType.HighLow, 5, 0.05);
                    break;

                case CandleSettingType.AllCandleSettings:
                    SetCandleSettings(CandleSettingType.BodyLong, RangeType.RealBody, 10, 1.0);
                    SetCandleSettings(CandleSettingType.BodyVeryLong, RangeType.RealBody, 10, 3.0);
                    SetCandleSettings(CandleSettingType.BodyShort, RangeType.RealBody, 10, 1.0);
                    SetCandleSettings(CandleSettingType.BodyDoji, RangeType.HighLow, 10, 0.1);
                    SetCandleSettings(CandleSettingType.ShadowLong, RangeType.RealBody, 0, 1.0);
                    SetCandleSettings(CandleSettingType.ShadowVeryLong, RangeType.RealBody, 0, 2.0);
                    SetCandleSettings(CandleSettingType.ShadowShort, RangeType.Shadows, 10, 1.0);
                    SetCandleSettings(CandleSettingType.ShadowVeryShort, RangeType.HighLow, 10, 0.1);
                    SetCandleSettings(CandleSettingType.Near, RangeType.HighLow, 5, 0.2);
                    SetCandleSettings(CandleSettingType.Far, RangeType.HighLow, 5, 0.6);
                    SetCandleSettings(CandleSettingType.Equal, RangeType.HighLow, 5, 0.05);
                    break;
            }

            return RetCode.Success;
        }

        public static RetCode SetCandleSettings(CandleSettingType settingType, RangeType rangeType, int avgPeriod, double factor)
        {
            if (settingType >= CandleSettingType.AllCandleSettings)
            {
                return RetCode.BadParam;
            }

            Globals.candleSettings[(int) settingType].settingType = settingType;
            Globals.candleSettings[(int) settingType].rangeType = rangeType;
            Globals.candleSettings[(int) settingType].avgPeriod = avgPeriod;
            Globals.candleSettings[(int) settingType].factor = factor;
            return RetCode.Success;
        }

        public static RetCode SetCompatibility(Compatibility value)
        {
            Globals.compatibility = value;
            return RetCode.Success;
        }

        public static RetCode SetUnstablePeriod(FuncUnstId id, Int64 unstablePeriod)
        {
            if (id > FuncUnstId.FuncUnstAll)
            {
                return RetCode.BadParam;
            }

            if (id != FuncUnstId.FuncUnstAll)
            {
                Globals.unstablePeriod[(int) id] = unstablePeriod;
            }
            else
            {
                for (int i = 0; i < 0x17; i++)
                {
                    Globals.unstablePeriod[i] = unstablePeriod;
                }
            }

            return RetCode.Success;
        }

        private static RetCode TA_INT_EMA(int startIdx, int endIdx, double[] inReal_0, int optInTimePeriod_0, double optInK_1,
            ref int outBegIdx, ref int outNbElement, double[] outReal_0)
        {
            int today;
            double prevMA;
            int lookbackTotal = EmaLookback(optInTimePeriod_0);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            if (Globals.compatibility != Compatibility.Default)
            {
                prevMA = inReal_0[0];
                today = 1;
            }
            else
            {
                today = startIdx - lookbackTotal;
                int i = optInTimePeriod_0;
                double tempReal = 0.0;
                while (true)
                {
                    i--;
                    if (i <= 0)
                    {
                        break;
                    }

                    tempReal += inReal_0[today];
                    today++;
                }

                prevMA = tempReal / ((double) optInTimePeriod_0);
            }

            while (today <= startIdx)
            {
                prevMA = ((inReal_0[today] - prevMA) * optInK_1) + prevMA;
                today++;
            }

            outReal_0[0] = prevMA;
            int outIdx = 1;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                prevMA = ((inReal_0[today] - prevMA) * optInK_1) + prevMA;
                today++;
                outReal_0[outIdx] = prevMA;
                outIdx++;
            }

            outNbElement = outIdx;
            return RetCode.Success;
        }

        private static RetCode TA_INT_EMA(int startIdx, int endIdx, float[] inReal_0, int optInTimePeriod_0, double optInK_1,
            ref int outBegIdx, ref int outNbElement, double[] outReal_0)
        {
            int today;
            double prevMA;
            int lookbackTotal = EmaLookback(optInTimePeriod_0);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            if (Globals.compatibility != Compatibility.Default)
            {
                prevMA = inReal_0[0];
                today = 1;
            }
            else
            {
                today = startIdx - lookbackTotal;
                int i = optInTimePeriod_0;
                double tempReal = 0.0;
                while (true)
                {
                    i--;
                    if (i <= 0)
                    {
                        break;
                    }

                    tempReal += inReal_0[today];
                    today++;
                }

                prevMA = tempReal / ((double) optInTimePeriod_0);
            }

            while (today <= startIdx)
            {
                prevMA = ((inReal_0[today] - prevMA) * optInK_1) + prevMA;
                today++;
            }

            outReal_0[0] = prevMA;
            int outIdx = 1;
            while (true)
            {
                if (today > endIdx)
                {
                    break;
                }

                prevMA = ((inReal_0[today] - prevMA) * optInK_1) + prevMA;
                today++;
                outReal_0[outIdx] = prevMA;
                outIdx++;
            }

            outNbElement = outIdx;
            return RetCode.Success;
        }

        private static RetCode TA_INT_MACD(int startIdx, int endIdx, double[] inReal_0, int optInFastPeriod_0, int optInSlowPeriod_1,
            int optInSignalPeriod_2, ref int outBegIdx, ref int outNbElement, double[] outMACD_0, double[] outMACDSignal_1,
            double[] outMACDHist_2)
        {
            int i;
            int tempInteger = 0;
            int outNbElement1 = 0;
            int outNbElement2 = 0;
            double k2;
            double k1;
            int outBegIdx2 = 0;
            int outBegIdx1 = 0;
            if (optInSlowPeriod_1 < optInFastPeriod_0)
            {
                tempInteger = optInSlowPeriod_1;
                optInSlowPeriod_1 = optInFastPeriod_0;
                optInFastPeriod_0 = tempInteger;
            }

            if (optInSlowPeriod_1 != 0)
            {
                k1 = 2.0 / ((double) (optInSlowPeriod_1 + 1));
            }
            else
            {
                optInSlowPeriod_1 = 0x1a;
                k1 = 0.075;
            }

            if (optInFastPeriod_0 != 0)
            {
                k2 = 2.0 / ((double) (optInFastPeriod_0 + 1));
            }
            else
            {
                optInFastPeriod_0 = 12;
                k2 = 0.15;
            }

            int lookbackSignal = EmaLookback(optInSignalPeriod_2);
            int lookbackTotal = lookbackSignal;
            lookbackTotal += EmaLookback(optInSlowPeriod_1);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            tempInteger = ((endIdx - startIdx) + 1) + lookbackSignal;
            double[] fastEMABuffer = new double[tempInteger];
            if (fastEMABuffer == null)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.AllocErr;
            }

            double[] slowEMABuffer = new double[tempInteger];
            if (slowEMABuffer == null)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.AllocErr;
            }

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = TA_INT_EMA(tempInteger, endIdx, inReal_0, optInSlowPeriod_1, k1, ref outBegIdx1, ref outNbElement1,
                slowEMABuffer);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return retCode;
            }

            retCode = TA_INT_EMA(tempInteger, endIdx, inReal_0, optInFastPeriod_0, k2, ref outBegIdx2, ref outNbElement2, fastEMABuffer);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return retCode;
            }

            if (((outBegIdx1 != tempInteger) || (outBegIdx2 != tempInteger)) ||
                ((outNbElement1 != outNbElement2) || (outNbElement1 != (((endIdx - startIdx) + 1) + lookbackSignal))))
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.InternalError;
            }

            for (i = 0; i < outNbElement1; i++)
            {
                fastEMABuffer[i] -= slowEMABuffer[i];
            }

            Array.Copy(fastEMABuffer, lookbackSignal, outMACD_0, 0, (endIdx - startIdx) + 1);
            retCode = TA_INT_EMA(0, outNbElement1 - 1, fastEMABuffer, optInSignalPeriod_2, 2.0 / ((double) (optInSignalPeriod_2 + 1)),
                ref outBegIdx2, ref outNbElement2, outMACDSignal_1);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return retCode;
            }

            for (i = 0; i < outNbElement2; i++)
            {
                outMACDHist_2[i] = outMACD_0[i] - outMACDSignal_1[i];
            }

            outBegIdx = startIdx;
            outNbElement = outNbElement2;
            return RetCode.Success;
        }

        private static RetCode TA_INT_MACD(int startIdx, int endIdx, float[] inReal_0, int optInFastPeriod_0, int optInSlowPeriod_1,
            int optInSignalPeriod_2, ref int outBegIdx, ref int outNbElement, double[] outMACD_0, double[] outMACDSignal_1,
            double[] outMACDHist_2)
        {
            int i;
            int tempInteger = 0;
            int outNbElement1 = 0;
            int outNbElement2 = 0;
            double k2;
            double k1;
            int outBegIdx2 = 0;
            int outBegIdx1 = 0;
            if (optInSlowPeriod_1 < optInFastPeriod_0)
            {
                tempInteger = optInSlowPeriod_1;
                optInSlowPeriod_1 = optInFastPeriod_0;
                optInFastPeriod_0 = tempInteger;
            }

            if (optInSlowPeriod_1 != 0)
            {
                k1 = 2.0 / ((double) (optInSlowPeriod_1 + 1));
            }
            else
            {
                optInSlowPeriod_1 = 0x1a;
                k1 = 0.075;
            }

            if (optInFastPeriod_0 != 0)
            {
                k2 = 2.0 / ((double) (optInFastPeriod_0 + 1));
            }
            else
            {
                optInFastPeriod_0 = 12;
                k2 = 0.15;
            }

            int lookbackSignal = EmaLookback(optInSignalPeriod_2);
            int lookbackTotal = lookbackSignal;
            lookbackTotal += EmaLookback(optInSlowPeriod_1);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            tempInteger = ((endIdx - startIdx) + 1) + lookbackSignal;
            double[] fastEMABuffer = new double[tempInteger];
            if (fastEMABuffer == null)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.AllocErr;
            }

            double[] slowEMABuffer = new double[tempInteger];
            if (slowEMABuffer == null)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.AllocErr;
            }

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = TA_INT_EMA(tempInteger, endIdx, inReal_0, optInSlowPeriod_1, k1, ref outBegIdx1, ref outNbElement1,
                slowEMABuffer);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return retCode;
            }

            retCode = TA_INT_EMA(tempInteger, endIdx, inReal_0, optInFastPeriod_0, k2, ref outBegIdx2, ref outNbElement2, fastEMABuffer);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return retCode;
            }

            if (((outBegIdx1 != tempInteger) || (outBegIdx2 != tempInteger)) ||
                ((outNbElement1 != outNbElement2) || (outNbElement1 != (((endIdx - startIdx) + 1) + lookbackSignal))))
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.InternalError;
            }

            for (i = 0; i < outNbElement1; i++)
            {
                fastEMABuffer[i] -= slowEMABuffer[i];
            }

            Array.Copy(fastEMABuffer, lookbackSignal, outMACD_0, 0, (endIdx - startIdx) + 1);
            retCode = TA_INT_EMA(0, outNbElement1 - 1, fastEMABuffer, optInSignalPeriod_2, 2.0 / ((double) (optInSignalPeriod_2 + 1)),
                ref outBegIdx2, ref outNbElement2, outMACDSignal_1);
            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return retCode;
            }

            for (i = 0; i < outNbElement2; i++)
            {
                outMACDHist_2[i] = outMACD_0[i] - outMACDSignal_1[i];
            }

            outBegIdx = startIdx;
            outNbElement = outNbElement2;
            return RetCode.Success;
        }

        private static RetCode TA_INT_PO(int startIdx, int endIdx, double[] inReal_0, int optInFastPeriod_0, int optInSlowPeriod_1,
            MAType optInMethod_2, ref int outBegIdx, ref int outNbElement, double[] outReal_0, double[] tempBuffer, int doPercentageOutput)
        {
            int tempInteger = 0;
            int outBegIdx2 = 0;
            int outNbElement2 = 0;
            if (optInSlowPeriod_1 < optInFastPeriod_0)
            {
                tempInteger = optInSlowPeriod_1;
                optInSlowPeriod_1 = optInFastPeriod_0;
                optInFastPeriod_0 = tempInteger;
            }

            RetCode retCode = MovingAverage(startIdx, endIdx, inReal_0, optInFastPeriod_0, optInMethod_2, ref outBegIdx2, ref outNbElement2,
                tempBuffer);
            if (retCode == RetCode.Success)
            {
                int outNbElement1 = 0;
                int outBegIdx1 = 0;
                retCode = MovingAverage(startIdx, endIdx, inReal_0, optInSlowPeriod_1, optInMethod_2, ref outBegIdx1, ref outNbElement1,
                    outReal_0);
                if (retCode == RetCode.Success)
                {
                    int i;
                    int j;
                    tempInteger = outBegIdx1 - outBegIdx2;
                    if (doPercentageOutput == 0)
                    {
                        i = 0;
                        j = tempInteger;
                        while (i < outNbElement1)
                        {
                            outReal_0[i] = tempBuffer[j] - outReal_0[i];
                            i++;
                            j++;
                        }
                    }
                    else
                    {
                        i = 0;
                        for (j = tempInteger; i < outNbElement1; j++)
                        {
                            double tempReal = outReal_0[i];
                            if ((-1E-08 >= tempReal) || (tempReal >= 1E-08))
                            {
                                outReal_0[i] = ((tempBuffer[j] - tempReal) / tempReal) * 100.0;
                            }
                            else
                            {
                                outReal_0[i] = 0.0;
                            }

                            i++;
                        }
                    }

                    outBegIdx = outBegIdx1;
                    outNbElement = outNbElement1;
                }
            }

            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNbElement = 0;
            }

            return retCode;
        }

        private static RetCode TA_INT_PO(int startIdx, int endIdx, float[] inReal_0, int optInFastPeriod_0, int optInSlowPeriod_1,
            MAType optInMethod_2, ref int outBegIdx, ref int outNbElement, double[] outReal_0, double[] tempBuffer, int doPercentageOutput)
        {
            int tempInteger = 0;
            int outBegIdx2 = 0;
            int outNbElement2 = 0;
            if (optInSlowPeriod_1 < optInFastPeriod_0)
            {
                tempInteger = optInSlowPeriod_1;
                optInSlowPeriod_1 = optInFastPeriod_0;
                optInFastPeriod_0 = tempInteger;
            }

            RetCode retCode = MovingAverage(startIdx, endIdx, inReal_0, optInFastPeriod_0, optInMethod_2, ref outBegIdx2, ref outNbElement2,
                tempBuffer);
            if (retCode == RetCode.Success)
            {
                int outNbElement1 = 0;
                int outBegIdx1 = 0;
                retCode = MovingAverage(startIdx, endIdx, inReal_0, optInSlowPeriod_1, optInMethod_2, ref outBegIdx1, ref outNbElement1,
                    outReal_0);
                if (retCode == RetCode.Success)
                {
                    int i;
                    int j;
                    tempInteger = outBegIdx1 - outBegIdx2;
                    if (doPercentageOutput == 0)
                    {
                        i = 0;
                        j = tempInteger;
                        while (i < outNbElement1)
                        {
                            outReal_0[i] = tempBuffer[j] - outReal_0[i];
                            i++;
                            j++;
                        }
                    }
                    else
                    {
                        i = 0;
                        for (j = tempInteger; i < outNbElement1; j++)
                        {
                            double tempReal = outReal_0[i];
                            if ((-1E-08 >= tempReal) || (tempReal >= 1E-08))
                            {
                                outReal_0[i] = ((tempBuffer[j] - tempReal) / tempReal) * 100.0;
                            }
                            else
                            {
                                outReal_0[i] = 0.0;
                            }

                            i++;
                        }
                    }

                    outBegIdx = outBegIdx1;
                    outNbElement = outNbElement1;
                }
            }

            if (retCode != RetCode.Success)
            {
                outBegIdx = 0;
                outNbElement = 0;
            }

            return retCode;
        }

        private static RetCode TA_INT_SMA(int startIdx, int endIdx, double[] inReal_0, int optInTimePeriod_0, ref int outBegIdx,
            ref int outNbElement, double[] outReal_0)
        {
            int lookbackTotal = optInTimePeriod_0 - 1;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            double periodTotal = 0.0;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod_0 > 1)
            {
                while (i < startIdx)
                {
                    periodTotal += inReal_0[i];
                    i++;
                }
            }

            int outIdx = 0;
            do
            {
                periodTotal += inReal_0[i];
                i++;
                double tempReal = periodTotal;
                periodTotal -= inReal_0[trailingIdx];
                trailingIdx++;
                outReal_0[outIdx] = tempReal / ((double) optInTimePeriod_0);
                outIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        private static RetCode TA_INT_SMA(int startIdx, int endIdx, float[] inReal_0, int optInTimePeriod_0, ref int outBegIdx,
            ref int outNbElement, double[] outReal_0)
        {
            int lookbackTotal = optInTimePeriod_0 - 1;
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            double periodTotal = 0.0;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod_0 > 1)
            {
                while (i < startIdx)
                {
                    periodTotal += inReal_0[i];
                    i++;
                }
            }

            int outIdx = 0;
            do
            {
                periodTotal += inReal_0[i];
                i++;
                double tempReal = periodTotal;
                periodTotal -= inReal_0[trailingIdx];
                trailingIdx++;
                outReal_0[outIdx] = tempReal / ((double) optInTimePeriod_0);
                outIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        private static void TA_INT_stddev_using_precalc_ma(double[] inReal, double[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            int timePeriod, double[] output)
        {
            double tempReal;
            int outIdx;
            int startSum = (inMovAvgBegIdx + 1) - timePeriod;
            int endSum = inMovAvgBegIdx;
            double periodTotal2 = 0.0;
            for (outIdx = startSum; outIdx < endSum; outIdx++)
            {
                tempReal = inReal[outIdx];
                tempReal *= tempReal;
                periodTotal2 += tempReal;
            }

            outIdx = 0;
            while (outIdx < inMovAvgNbElement)
            {
                tempReal = inReal[endSum];
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                double meanValue2 = periodTotal2 / ((double) timePeriod);
                tempReal = inReal[startSum];
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                tempReal = inMovAvg[outIdx];
                tempReal *= tempReal;
                meanValue2 -= tempReal;
                if (meanValue2 >= 1E-08)
                {
                    output[outIdx] = Math.Sqrt(meanValue2);
                }
                else
                {
                    output[outIdx] = 0.0;
                }

                outIdx++;
                startSum++;
                endSum++;
            }
        }

        private static void TA_INT_stddev_using_precalc_ma(float[] inReal, double[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            int timePeriod, double[] output)
        {
            double tempReal;
            int outIdx;
            int startSum = (inMovAvgBegIdx + 1) - timePeriod;
            int endSum = inMovAvgBegIdx;
            double periodTotal2 = 0.0;
            for (outIdx = startSum; outIdx < endSum; outIdx++)
            {
                tempReal = inReal[outIdx];
                tempReal *= tempReal;
                periodTotal2 += tempReal;
            }

            outIdx = 0;
            while (outIdx < inMovAvgNbElement)
            {
                tempReal = inReal[endSum];
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                double meanValue2 = periodTotal2 / ((double) timePeriod);
                tempReal = inReal[startSum];
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                tempReal = inMovAvg[outIdx];
                tempReal *= tempReal;
                meanValue2 -= tempReal;
                if (meanValue2 >= 1E-08)
                {
                    output[outIdx] = Math.Sqrt(meanValue2);
                }
                else
                {
                    output[outIdx] = 0.0;
                }

                outIdx++;
                startSum++;
                endSum++;
            }
        }

        private static RetCode TA_INT_VAR(int startIdx, int endIdx, double[] inReal_0, int optInTimePeriod_0, ref int outBegIdx,
            ref int outNbElement, double[] outReal_0)
        {
            double tempReal;
            int nbInitialElementNeeded = optInTimePeriod_0 - 1;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            double periodTotal1 = 0.0;
            double periodTotal2 = 0.0;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            int i = trailingIdx;
            if (optInTimePeriod_0 > 1)
            {
                while (i < startIdx)
                {
                    tempReal = inReal_0[i];
                    i++;
                    periodTotal1 += tempReal;
                    tempReal *= tempReal;
                    periodTotal2 += tempReal;
                }
            }

            int outIdx = 0;
            do
            {
                tempReal = inReal_0[i];
                i++;
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                double meanValue1 = periodTotal1 / ((double) optInTimePeriod_0);
                double meanValue2 = periodTotal2 / ((double) optInTimePeriod_0);
                tempReal = inReal_0[trailingIdx];
                trailingIdx++;
                periodTotal1 -= tempReal;
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                outReal_0[outIdx] = meanValue2 - (meanValue1 * meanValue1);
                outIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        private static RetCode TA_INT_VAR(int startIdx, int endIdx, float[] inReal_0, int optInTimePeriod_0, ref int outBegIdx,
            ref int outNbElement, double[] outReal_0)
        {
            double tempReal;
            int nbInitialElementNeeded = optInTimePeriod_0 - 1;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNbElement = 0;
                return RetCode.Success;
            }

            double periodTotal1 = 0.0;
            double periodTotal2 = 0.0;
            int trailingIdx = startIdx - nbInitialElementNeeded;
            int i = trailingIdx;
            if (optInTimePeriod_0 > 1)
            {
                while (i < startIdx)
                {
                    tempReal = inReal_0[i];
                    i++;
                    periodTotal1 += tempReal;
                    tempReal *= tempReal;
                    periodTotal2 += tempReal;
                }
            }

            int outIdx = 0;
            do
            {
                tempReal = inReal_0[i];
                i++;
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                double meanValue1 = periodTotal1 / ((double) optInTimePeriod_0);
                double meanValue2 = periodTotal2 / ((double) optInTimePeriod_0);
                tempReal = inReal_0[trailingIdx];
                trailingIdx++;
                periodTotal1 -= tempReal;
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                outReal_0[outIdx] = meanValue2 - (meanValue1 * meanValue1);
                outIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        private sealed class CandleSetting
        {
            public int avgPeriod;
            public double factor;
            public Core.RangeType rangeType;
            public Core.CandleSettingType settingType;
        }

        public enum CandleSettingType
        {
            BodyLong,
            BodyVeryLong,
            BodyShort,
            BodyDoji,
            ShadowLong,
            ShadowVeryLong,
            ShadowShort,
            ShadowVeryShort,
            Near,
            Far,
            Equal,
            AllCandleSettings
        }

        public enum Compatibility
        {
            Default,
            Metastock
        }

        public enum FuncUnstId
        {
            Adx = 0,
            Adxr = 1,
            Atr = 2,
            Cmo = 3,
            Dx = 4,
            Ema = 5,
            FuncUnstAll = 0x17,
            FuncUnstNone = -1,
            HtDcPeriod = 6,
            HtDcPhase = 7,
            HtPhasor = 8,
            HtSine = 9,
            HtTrendline = 10,
            HtTrendMode = 11,
            Kama = 12,
            Mama = 13,
            Mfi = 14,
            MinusDI = 15,
            MinusDM = 0x10,
            Natr = 0x11,
            PlusDI = 0x12,
            PlusDM = 0x13,
            Rsi = 20,
            StochRsi = 0x15,
            T3 = 0x16
        }

        private sealed class GlobalsType
        {
            public Core.CandleSetting[] candleSettings;
            public Core.Compatibility compatibility = Core.Compatibility.Default;
            public Int64[] unstablePeriod = new Int64[0x17];

            public GlobalsType()
            {
                for (int i = 0; i < 0x17; i++)
                {
                    this.unstablePeriod[i] = 0;
                }

                this.candleSettings = new Core.CandleSetting[11];
                for (int j = 0; j < this.candleSettings.Length; j++)
                {
                    this.candleSettings[j] = new Core.CandleSetting();
                }
            }
        }

        public enum MAType
        {
            Sma,
            Ema,
            Wma,
            Dema,
            Tema,
            Trima,
            Kama,
            Mama,
            T3
        }

        public enum RangeType
        {
            RealBody,
            HighLow,
            Shadows
        }

        public enum RetCode
        {
            AllocErr = 3,
            BadObject = 15,
            BadParam = 2,
            FuncNotFound = 5,
            GroupNotFound = 4,
            InputNotAllInitialize = 10,
            InternalError = 0x1388,
            InvalidHandle = 6,
            InvalidListType = 14,
            InvalidParamFunction = 9,
            InvalidParamHolder = 7,
            InvalidParamHolderType = 8,
            LibNotInitialize = 1,
            NotSupported = 0x10,
            OutOfRangeEndIndex = 13,
            OutOfRangeStartIndex = 12,
            OutputNotAllInitialize = 11,
            Success = 0,
            UnknownErr = 0xffff
        }

        internal class MoneyFlow
        {
            public double negative;
            public double positive;
        }
    }
}
