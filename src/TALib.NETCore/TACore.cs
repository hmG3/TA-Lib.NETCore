using System;

namespace TALib
{
    public partial class Core
    {
        private static readonly GlobalsType Globals = new GlobalsType();

        static Core()
        {
            RestoreCandleDefaultSettings(CandleSettingType.AllCandleSettings);
        }

        public static Compatibility GetCompatibility() => Globals.Compatibility;

        public static long GetUnstablePeriod(FuncUnstId id) => id >= FuncUnstId.FuncUnstAll ? 0 : Globals.UnstablePeriod[(int) id];

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

            Globals.CandleSettings[(int) settingType].SettingType = settingType;
            Globals.CandleSettings[(int) settingType].RangeType = rangeType;
            Globals.CandleSettings[(int) settingType].AvgPeriod = avgPeriod;
            Globals.CandleSettings[(int) settingType].Factor = factor;

            return RetCode.Success;
        }

        public static RetCode SetCompatibility(Compatibility value)
        {
            Globals.Compatibility = value;
            return RetCode.Success;
        }

        public static RetCode SetUnstablePeriod(FuncUnstId id, long unstablePeriod)
        {
            if (id > FuncUnstId.FuncUnstAll)
            {
                return RetCode.BadParam;
            }

            if (id != FuncUnstId.FuncUnstAll)
            {
                Globals.UnstablePeriod[(int) id] = unstablePeriod;
            }
            else
            {
                Array.Fill(Globals.UnstablePeriod, unstablePeriod);
            }

            return RetCode.Success;
        }

        private static RetCode TA_INT_EMA(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, double optInK1,
            out int outBegIdx, out int outNbElement, double[] outReal)
        {
            int today;
            double prevMA;
            int lookbackTotal = EmaLookback(optInTimePeriod);
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
            if (Globals.Compatibility == Compatibility.Default)
            {
                today = startIdx - lookbackTotal;
                int i = optInTimePeriod;
                double tempReal = default;
                while (i-- > 0)
                {
                    tempReal += inReal[today];
                    today++;
                }

                prevMA = tempReal / optInTimePeriod;
            }
            else
            {
                prevMA = inReal[0];
                today = 1;
            }

            while (today <= startIdx)
            {
                prevMA = (inReal[today] - prevMA) * optInK1 + prevMA;
                today++;
            }

            outReal[0] = prevMA;
            int outIdx = 1;
            while (today <= startIdx)
            {
                prevMA = (inReal[today] - prevMA) * optInK1 + prevMA;
                today++;
                outReal[outIdx] = prevMA;
                outIdx++;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        private static RetCode TA_INT_EMA(int startIdx, int endIdx, decimal[] inReal, int optInTimePeriod, decimal optInK1,
            out int outBegIdx, out int outNbElement, decimal[] outReal)
        {
            int today;
            decimal prevMA;
            int lookbackTotal = EmaLookback(optInTimePeriod);
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
            if (Globals.Compatibility == Compatibility.Default)
            {
                today = startIdx - lookbackTotal;
                int i = optInTimePeriod;
                decimal tempReal = default;
                while (i-- > 0)
                {
                    tempReal += inReal[today];
                    today++;
                }

                prevMA = tempReal / optInTimePeriod;
            }
            else
            {
                prevMA = inReal[0];
                today = 1;
            }

            while (today <= startIdx)
            {
                prevMA = (inReal[today] - prevMA) * optInK1 + prevMA;
                today++;
            }

            outReal[0] = prevMA;
            int outIdx = 1;
            while (today <= startIdx)
            {
                prevMA = (inReal[today] - prevMA) * optInK1 + prevMA;
                today++;
                outReal[outIdx] = prevMA;
                outIdx++;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        private static RetCode TA_INT_MACD(int startIdx, int endIdx, double[] inReal, int optInFastPeriod, int optInSlowPeriod,
            int optInSignalPeriod, out int outBegIdx, out int outNbElement, double[] outMacd, double[] outMacdSignal,
            double[] outMacdHist)
        {
            outBegIdx = outNbElement = 0;

            int tempInteger;
            double k2;
            double k1;
            if (optInSlowPeriod < optInFastPeriod)
            {
                tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;
            }

            if (optInSlowPeriod != 0)
            {
                k1 = 2.0 / (optInSlowPeriod + 1);
            }
            else
            {
                optInSlowPeriod = 26;
                k1 = 0.075;
            }

            if (optInFastPeriod != 0)
            {
                k2 = 2.0 / (optInFastPeriod + 1);
            }
            else
            {
                optInFastPeriod = 12;
                k2 = 0.15;
            }

            int lookbackSignal = EmaLookback(optInSignalPeriod);
            int lookbackTotal = MacdLookback(optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            tempInteger = endIdx - startIdx + 1 + lookbackSignal;
            var fastEMABuffer = new double[tempInteger];

            var slowEMABuffer = new double[tempInteger];

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = TA_INT_EMA(tempInteger, endIdx, inReal, optInSlowPeriod, k1, out var outBegIdx1, out var outNbElement1,
                slowEMABuffer);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_EMA(tempInteger, endIdx, inReal, optInFastPeriod, k2, out var outBegIdx2, out var outNbElement2,
                fastEMABuffer);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
                outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
            {
                return RetCode.InternalError;
            }

            for (var i = 0; i < outNbElement1; i++)
            {
                fastEMABuffer[i] -= slowEMABuffer[i];
            }

            Array.Copy(fastEMABuffer, lookbackSignal, outMacd, 0, endIdx - startIdx + 1);
            retCode = TA_INT_EMA(0, outNbElement1 - 1, fastEMABuffer, optInSignalPeriod, 2.0 / (optInSignalPeriod + 1), out _,
                out outNbElement2, outMacdSignal);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            for (var i = 0; i < outNbElement2; i++)
            {
                outMacdHist[i] = outMacd[i] - outMacdSignal[i];
            }

            outBegIdx = startIdx;
            outNbElement = outNbElement2;

            return RetCode.Success;
        }

        private static RetCode TA_INT_MACD(int startIdx, int endIdx, decimal[] inReal, int optInFastPeriod, int optInSlowPeriod,
            int optInSignalPeriod, out int outBegIdx, out int outNbElement, decimal[] outMacd, decimal[] outMacdSignal,
            decimal[] outMacdHist)
        {
            outBegIdx = outNbElement = 0;

            int tempInteger;
            decimal k2;
            decimal k1;
            if (optInSlowPeriod < optInFastPeriod)
            {
                tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;
            }

            if (optInSlowPeriod != 0)
            {
                k1 = 2m / (optInSlowPeriod + 1);
            }
            else
            {
                optInSlowPeriod = 26;
                k1 = 0.075m;
            }

            if (optInFastPeriod != 0)
            {
                k2 = 2m / (optInFastPeriod + 1);
            }
            else
            {
                optInFastPeriod = 12;
                k2 = 0.15m;
            }

            int lookbackSignal = EmaLookback(optInSignalPeriod);
            int lookbackTotal = MacdLookback(optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            tempInteger = endIdx - startIdx + 1 + lookbackSignal;
            var fastEMABuffer = new decimal[tempInteger];

            var slowEMABuffer = new decimal[tempInteger];

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = TA_INT_EMA(tempInteger, endIdx, inReal, optInSlowPeriod, k1, out int outBegIdx1, out var outNbElement1,
                slowEMABuffer);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_EMA(tempInteger, endIdx, inReal, optInFastPeriod, k2, out var outBegIdx2, out var outNbElement2,
                fastEMABuffer);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            if (outBegIdx1 != tempInteger || outBegIdx2 != tempInteger || outNbElement1 != outNbElement2 ||
                outNbElement1 != endIdx - startIdx + 1 + lookbackSignal)
            {
                return RetCode.InternalError;
            }

            for (var i = 0; i < outNbElement1; i++)
            {
                fastEMABuffer[i] -= slowEMABuffer[i];
            }

            Array.Copy(fastEMABuffer, lookbackSignal, outMacd, 0, endIdx - startIdx + 1);
            retCode = TA_INT_EMA(0, outNbElement1 - 1, fastEMABuffer, optInSignalPeriod, 2m / (optInSignalPeriod + 1), out _,
                out outNbElement2, outMacdSignal);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            for (var i = 0; i < outNbElement2; i++)
            {
                outMacdHist[i] = outMacd[i] - outMacdSignal[i];
            }

            outBegIdx = startIdx;
            outNbElement = outNbElement2;

            return RetCode.Success;
        }

        private static RetCode TA_INT_PO(int startIdx, int endIdx, double[] inReal, int optInFastPeriod, int optInSlowPeriod,
            MAType optInMethod, out int outBegIdx, out int outNbElement, double[] outReal, double[] tempBuffer, int doPercentageOutput)
        {
            outBegIdx = outNbElement = 0;

            int tempInteger;
            if (optInSlowPeriod < optInFastPeriod)
            {
                tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;
            }

            RetCode retCode = Ma(startIdx, endIdx, inReal, optInMethod, out var outBegIdx2, out _, tempBuffer,
                optInFastPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = Ma(startIdx, endIdx, inReal, optInMethod, out var outBegIdx1, out var outNbElement1, outReal,
                optInSlowPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            int i;
            int j;
            tempInteger = outBegIdx1 - outBegIdx2;
            if (doPercentageOutput == 0)
            {
                i = 0;
                j = tempInteger;
                while (i < outNbElement1)
                {
                    outReal[i] = tempBuffer[j] - outReal[i];
                    i++;
                    j++;
                }
            }
            else
            {
                i = 0;
                for (j = tempInteger; i < outNbElement1; j++)
                {
                    double tempReal = outReal[i];
                    if (!TA_IsZero(tempReal))
                    {
                        outReal[i] = (tempBuffer[j] - tempReal) / tempReal * 100.0;
                    }
                    else
                    {
                        outReal[i] = 0.0;
                    }

                    i++;
                }
            }

            outBegIdx = outBegIdx1;
            outNbElement = outNbElement1;

            return retCode;
        }

        private static RetCode TA_INT_PO(int startIdx, int endIdx, decimal[] inReal, int optInFastPeriod, int optInSlowPeriod,
            MAType optInMethod, out int outBegIdx, out int outNbElement, decimal[] outReal, decimal[] tempBuffer, int doPercentageOutput)
        {
            outBegIdx = outNbElement = 0;

            int tempInteger;
            if (optInSlowPeriod < optInFastPeriod)
            {
                tempInteger = optInSlowPeriod;
                optInSlowPeriod = optInFastPeriod;
                optInFastPeriod = tempInteger;
            }

            RetCode retCode = Ma(startIdx, endIdx, inReal, optInMethod, out var outBegIdx2, out _, tempBuffer,
                optInFastPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = Ma(startIdx, endIdx, inReal, optInMethod, out var outBegIdx1, out var outNbElement1, outReal,
                optInSlowPeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            int i;
            int j;
            tempInteger = outBegIdx1 - outBegIdx2;
            if (doPercentageOutput == 0)
            {
                i = 0;
                j = tempInteger;
                while (i < outNbElement1)
                {
                    outReal[i] = tempBuffer[j] - outReal[i];
                    i++;
                    j++;
                }
            }
            else
            {
                i = 0;
                for (j = tempInteger; i < outNbElement1; j++)
                {
                    decimal tempReal = outReal[i];
                    if (!TA_IsZero(tempReal))
                    {
                        outReal[i] = (tempBuffer[j] - tempReal) / tempReal * 100m;
                    }
                    else
                    {
                        outReal[i] = Decimal.Zero;
                    }

                    i++;
                }
            }

            outBegIdx = outBegIdx1;
            outNbElement = outNbElement1;

            return retCode;
        }

        private static RetCode TA_INT_SMA(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, out int outBegIdx,
            out int outNbElement, double[] outReal)
        {
            int lookbackTotal = SmaLookback(optInTimePeriod);
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

            double periodTotal = default;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    periodTotal += inReal[i];
                    i++;
                }
            }

            int outIdx = default;
            do
            {
                periodTotal += inReal[i];
                i++;
                double tempReal = periodTotal;
                periodTotal -= inReal[trailingIdx];
                trailingIdx++;
                outReal[outIdx] = tempReal / optInTimePeriod;
                outIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        private static RetCode TA_INT_SMA(int startIdx, int endIdx, decimal[] inReal, int optInTimePeriod, out int outBegIdx,
            out int outNbElement, decimal[] outReal)
        {
            int lookbackTotal = SmaLookback(optInTimePeriod);
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

            decimal periodTotal = default;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    periodTotal += inReal[i];
                    i++;
                }
            }

            int outIdx = default;
            do
            {
                periodTotal += inReal[i];
                i++;
                decimal tempReal = periodTotal;
                periodTotal -= inReal[trailingIdx];
                trailingIdx++;
                outReal[outIdx] = tempReal / optInTimePeriod;
                outIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        private static void TA_INT_StdDevUsingPrecalcMA(double[] inReal, double[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            int timePeriod, double[] output)
        {
            double tempReal;
            int outIdx;
            int startSum = inMovAvgBegIdx + 1 - timePeriod;
            int endSum = inMovAvgBegIdx;
            double periodTotal2 = default;
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
                double meanValue2 = periodTotal2 / timePeriod;
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

        private static void TA_INT_StdDevUsingPrecalcMA(decimal[] inReal, decimal[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            int timePeriod, decimal[] output)
        {
            decimal tempReal;
            int outIdx;
            int startSum = inMovAvgBegIdx + 1 - timePeriod;
            int endSum = inMovAvgBegIdx;
            decimal periodTotal2 = default;
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
                decimal meanValue2 = periodTotal2 / timePeriod;
                tempReal = inReal[startSum];
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                tempReal = inMovAvg[outIdx];
                tempReal *= tempReal;
                meanValue2 -= tempReal;
                if (meanValue2 >= 1E-08m)
                {
                    output[outIdx] = DecimalMath.Sqrt(meanValue2);
                }
                else
                {
                    output[outIdx] = Decimal.Zero;
                }

                outIdx++;
                startSum++;
                endSum++;
            }
        }

        private static RetCode TA_INT_VAR(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, out int outBegIdx,
            out int outNbElement, double[] outReal)
        {
            double tempReal;
            int lookbackTotal = VarLookback(optInTimePeriod);
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

            double periodTotal1 = default;
            double periodTotal2 = default;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    tempReal = inReal[i];
                    i++;
                    periodTotal1 += tempReal;
                    tempReal *= tempReal;
                    periodTotal2 += tempReal;
                }
            }

            int outIdx = default;
            do
            {
                tempReal = inReal[i];
                i++;
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                double meanValue1 = periodTotal1 / optInTimePeriod;
                double meanValue2 = periodTotal2 / optInTimePeriod;
                tempReal = inReal[trailingIdx];
                trailingIdx++;
                periodTotal1 -= tempReal;
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                outReal[outIdx] = meanValue2 - meanValue1 * meanValue1;
                outIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        private static RetCode TA_INT_VAR(int startIdx, int endIdx, decimal[] inReal, int optInTimePeriod, out int outBegIdx,
            out int outNbElement, decimal[] outReal)
        {
            decimal tempReal;
            int lookbackTotal = VarLookback(optInTimePeriod);
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

            decimal periodTotal1 = default;
            decimal periodTotal2 = default;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    tempReal = inReal[i];
                    i++;
                    periodTotal1 += tempReal;
                    tempReal *= tempReal;
                    periodTotal2 += tempReal;
                }
            }

            int outIdx = default;
            do
            {
                tempReal = inReal[i];
                i++;
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                decimal meanValue1 = periodTotal1 / optInTimePeriod;
                decimal meanValue2 = periodTotal2 / optInTimePeriod;
                tempReal = inReal[trailingIdx];
                trailingIdx++;
                periodTotal1 -= tempReal;
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                outReal[outIdx] = meanValue2 - meanValue1 * meanValue1;
                outIdx++;
            } while (i <= endIdx);

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        private sealed class CandleSetting
        {
            internal int AvgPeriod;
            internal double Factor;
            internal RangeType RangeType;
            internal CandleSettingType SettingType;
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
            FuncUnstNone = -1,
            Adx,
            Adxr,
            Atr,
            Cmo,
            Dx,
            Ema,
            HtDcPeriod,
            HtDcPhase,
            HtPhasor,
            HtSine,
            HtTrendline,
            HtTrendMode,
            Kama,
            Mama,
            Mfi,
            MinusDI,
            MinusDM,
            Natr,
            PlusDI,
            PlusDM,
            Rsi,
            StochRsi,
            T3,
            FuncUnstAll
        }

        private sealed class GlobalsType
        {
            internal Compatibility Compatibility = Compatibility.Default;
            internal readonly CandleSetting[] CandleSettings;
            internal readonly long[] UnstablePeriod;

            internal GlobalsType()
            {
                CandleSettings = new CandleSetting[(int) CandleSettingType.AllCandleSettings];
                UnstablePeriod = new long[(int) FuncUnstId.FuncUnstAll];

                Array.Fill(CandleSettings, new CandleSetting());
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

        public enum RetCode : ushort
        {
            Success,
            LibNotInitialize,
            BadParam,
            GroupNotFound,
            FuncNotFound,
            InvalidHandle,
            InvalidParamHolder,
            InvalidParamHolderType,
            InvalidParamFunction,
            InputNotAllInitialize,
            OutputNotAllInitialize,
            OutOfRangeStartIndex,
            OutOfRangeEndIndex,
            InvalidListType,
            BadObject,
            NotSupported,
            InternalError = 5000,
            UnknownErr = UInt16.MaxValue
        }
    }
}
