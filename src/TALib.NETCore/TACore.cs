using System;

namespace TALib
{
    public static partial class Core
    {
        private static readonly GlobalsType Globals = new GlobalsType();

        static Core()
        {
            RestoreCandleDefaultSettings(CandleSettingType.AllCandleSettings);
        }

        public static Compatibility GetCompatibility() => Globals.Compatibility;

        public static RetCode SetCompatibility(Compatibility value)
        {
            Globals.Compatibility = value;

            return RetCode.Success;
        }

        public static long GetUnstablePeriod(FuncUnstId id) => id >= FuncUnstId.FuncUnstAll ? 0 : Globals.UnstablePeriod[(int) id];

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

        private static RetCode TA_INT_EMA(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod, double optInK1)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = EmaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = startIdx;

            int today;
            double prevMA;
            if (Globals.Compatibility == Compatibility.Default)
            {
                today = startIdx - lookbackTotal;
                int i = optInTimePeriod;
                double tempReal = default;
                while (i-- > 0)
                {
                    tempReal += inReal[today++];
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
                prevMA = (inReal[today++] - prevMA) * optInK1 + prevMA;
            }

            outReal[0] = prevMA;
            int outIdx = 1;
            while (today <= endIdx)
            {
                prevMA = (inReal[today++] - prevMA) * optInK1 + prevMA;
                outReal[outIdx++] = prevMA;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        private static RetCode TA_INT_EMA(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod, decimal optInK1)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = EmaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            outBegIdx = startIdx;

            int today;
            decimal prevMA;
            if (Globals.Compatibility == Compatibility.Default)
            {
                today = startIdx - lookbackTotal;
                int i = optInTimePeriod;
                decimal tempReal = default;
                while (i-- > 0)
                {
                    tempReal += inReal[today++];
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
                prevMA = (inReal[today++] - prevMA) * optInK1 + prevMA;
            }

            outReal[0] = prevMA;
            int outIdx = 1;
            while (today <= endIdx)
            {
                prevMA = (inReal[today++] - prevMA) * optInK1 + prevMA;
                outReal[outIdx++] = prevMA;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        private static RetCode TA_INT_MACD(double[] inReal, int startIdx, int endIdx, double[] outMacd, double[] outMacdSignal,
            double[] outMacdHist, out int outBegIdx, out int outNbElement, int optInFastPeriod, int optInSlowPeriod, int optInSignalPeriod)
        {
            outBegIdx = outNbElement = 0;

            if (optInSlowPeriod < optInFastPeriod)
            {
                (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            }

            double k1;
            double k2;
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

            var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
            var fastEMABuffer = new double[tempInteger];
            var slowEMABuffer = new double[tempInteger];

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = TA_INT_EMA(inReal, tempInteger, endIdx, slowEMABuffer, out var outBegIdx1, out var outNbElement1,
                optInSlowPeriod, k1);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_EMA(inReal, tempInteger, endIdx, fastEMABuffer, out var outBegIdx2, out var outNbElement2, optInFastPeriod,
                k2);
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
            retCode = TA_INT_EMA(fastEMABuffer, 0, outNbElement1 - 1, outMacdSignal, out _, out outNbElement2, optInSignalPeriod,
                2.0 / (optInSignalPeriod + 1));
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

        private static RetCode TA_INT_MACD(decimal[] inReal, int startIdx, int endIdx, decimal[] outMacd, decimal[] outMacdSignal,
            decimal[] outMacdHist, out int outBegIdx, out int outNbElement, int optInFastPeriod, int optInSlowPeriod, int optInSignalPeriod)
        {
            outBegIdx = outNbElement = 0;

            if (optInSlowPeriod < optInFastPeriod)
            {
                (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            }

            decimal k1;
            decimal k2;
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

            var tempInteger = endIdx - startIdx + 1 + lookbackSignal;
            var fastEMABuffer = new decimal[tempInteger];
            var slowEMABuffer = new decimal[tempInteger];

            tempInteger = startIdx - lookbackSignal;
            RetCode retCode = TA_INT_EMA(inReal, tempInteger, endIdx, slowEMABuffer, out var outBegIdx1, out var outNbElement1,
                optInSlowPeriod, k1);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = TA_INT_EMA(inReal, tempInteger, endIdx, fastEMABuffer, out var outBegIdx2, out var outNbElement2, optInFastPeriod,
                k2);
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
            retCode = TA_INT_EMA(fastEMABuffer, 0, outNbElement1 - 1, outMacdSignal, out _, out outNbElement2, optInSignalPeriod,
                2m / (optInSignalPeriod + 1));
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

        private static RetCode TA_INT_PO(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInFastPeriod, int optInSlowPeriod, MAType optInMethod, double[] tempBuffer,
            bool doPercentageOutput)
        {
            outBegIdx = outNbElement = 0;

            if (optInSlowPeriod < optInFastPeriod)
            {
                (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            }

            RetCode retCode = Ma(inReal, startIdx, endIdx, tempBuffer, out var outBegIdx2, out _, optInFastPeriod, optInMethod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = Ma(inReal, startIdx, endIdx, outReal, out var outBegIdx1, out var outNbElement1, optInSlowPeriod, optInMethod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            for (int i = 0, j = outBegIdx1 - outBegIdx2; i < outNbElement1; i++, j++)
            {
                if (doPercentageOutput)
                {
                    double tempReal = outReal[i];
                    outReal[i] = !TA_IsZero(tempReal) ? (tempBuffer[j] - tempReal) / tempReal * 100.0 : 0.0;
                }
                else
                {
                    outReal[i] = tempBuffer[j] - outReal[i];
                }
            }

            outBegIdx = outBegIdx1;
            outNbElement = outNbElement1;

            return retCode;
        }

        private static RetCode TA_INT_PO(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInFastPeriod, int optInSlowPeriod, MAType optInMethod, decimal[] tempBuffer,
            bool doPercentageOutput)
        {
            outBegIdx = outNbElement = 0;

            if (optInSlowPeriod < optInFastPeriod)
            {
                (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            }

            RetCode retCode = Ma(inReal, startIdx, endIdx, tempBuffer, out var outBegIdx2, out _, optInFastPeriod, optInMethod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = Ma(inReal, startIdx, endIdx, outReal, out var outBegIdx1, out var outNbElement1, optInSlowPeriod, optInMethod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            for (int i = 0, j = outBegIdx1 - outBegIdx2; i < outNbElement1; i++, j++)
            {
                if (doPercentageOutput)
                {
                    decimal tempReal = outReal[i];
                    outReal[i] = !TA_IsZero(tempReal) ? (tempBuffer[j] - tempReal) / tempReal * 100m : Decimal.Zero;
                }
                else
                {
                    outReal[i] = tempBuffer[j] - outReal[i];
                }
            }

            outBegIdx = outBegIdx1;
            outNbElement = outNbElement1;

            return retCode;
        }

        private static RetCode TA_INT_SMA(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = SmaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double periodTotal = default;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    periodTotal += inReal[i++];
                }
            }

            int outIdx = default;
            do
            {
                periodTotal += inReal[i++];
                double tempReal = periodTotal;
                periodTotal -= inReal[trailingIdx++];
                outReal[outIdx++] = tempReal / optInTimePeriod;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        private static RetCode TA_INT_SMA(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = SmaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal periodTotal = default;
            int trailingIdx = startIdx - lookbackTotal;
            int i = trailingIdx;
            if (optInTimePeriod > 1)
            {
                while (i < startIdx)
                {
                    periodTotal += inReal[i++];
                }
            }

            int outIdx = default;
            do
            {
                periodTotal += inReal[i++];
                decimal tempReal = periodTotal;
                periodTotal -= inReal[trailingIdx++];
                outReal[outIdx++] = tempReal / optInTimePeriod;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        private static void TA_INT_StdDevUsingPrecalcMA(double[] inReal, double[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            double[] outReal, int optInTimePeriod)
        {
            int startSum = inMovAvgBegIdx + 1 - optInTimePeriod;
            int endSum = inMovAvgBegIdx;
            double periodTotal2 = default;
            for (int outIdx = startSum; outIdx < endSum; outIdx++)
            {
                double tempReal = inReal[outIdx];
                tempReal *= tempReal;
                periodTotal2 += tempReal;
            }

            for (var outIdx = 0; outIdx < inMovAvgNbElement; outIdx++, startSum++, endSum++)
            {
                double tempReal = inReal[endSum];
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                double meanValue2 = periodTotal2 / optInTimePeriod;

                tempReal = inReal[startSum];
                tempReal *= tempReal;
                periodTotal2 -= tempReal;

                tempReal = inMovAvg[outIdx];
                tempReal *= tempReal;
                meanValue2 -= tempReal;

                outReal[outIdx] = !TA_IsZeroOrNeg(meanValue2) ? Math.Sqrt(meanValue2) : 0.0;
            }
        }

        private static void TA_INT_StdDevUsingPrecalcMA(decimal[] inReal, decimal[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
            decimal[] outReal, int optInTimePeriod)
        {
            int startSum = inMovAvgBegIdx + 1 - optInTimePeriod;
            int endSum = inMovAvgBegIdx;
            decimal periodTotal2 = default;
            for (int outIdx = startSum; outIdx < endSum; outIdx++)
            {
                decimal tempReal = inReal[outIdx];
                tempReal *= tempReal;
                periodTotal2 += tempReal;
            }

            for (var outIdx = 0; outIdx < inMovAvgNbElement; outIdx++, startSum++, endSum++)
            {
                decimal tempReal = inReal[endSum];
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                decimal meanValue2 = periodTotal2 / optInTimePeriod;

                tempReal = inReal[startSum];
                tempReal *= tempReal;
                periodTotal2 -= tempReal;

                tempReal = inMovAvg[outIdx];
                tempReal *= tempReal;
                meanValue2 -= tempReal;

                outReal[outIdx] = !TA_IsZeroOrNeg(meanValue2) ? DecimalMath.Sqrt(meanValue2) : Decimal.Zero;
            }
        }

        private static RetCode TA_INT_VAR(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = VarLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
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
                    double tempReal = inReal[i++];
                    periodTotal1 += tempReal;
                    tempReal *= tempReal;
                    periodTotal2 += tempReal;
                }
            }

            int outIdx = default;
            do
            {
                double tempReal = inReal[i++];
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                double meanValue1 = periodTotal1 / optInTimePeriod;
                double meanValue2 = periodTotal2 / optInTimePeriod;
                tempReal = inReal[trailingIdx++];
                periodTotal1 -= tempReal;
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                outReal[outIdx++] = meanValue2 - meanValue1 * meanValue1;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

            return RetCode.Success;
        }

        private static RetCode TA_INT_VAR(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = VarLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
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
                    decimal tempReal = inReal[i++];
                    periodTotal1 += tempReal;
                    tempReal *= tempReal;
                    periodTotal2 += tempReal;
                }
            }

            int outIdx = default;
            do
            {
                decimal tempReal = inReal[i++];
                periodTotal1 += tempReal;
                tempReal *= tempReal;
                periodTotal2 += tempReal;
                decimal meanValue1 = periodTotal1 / optInTimePeriod;
                decimal meanValue2 = periodTotal2 / optInTimePeriod;
                tempReal = inReal[trailingIdx++];
                periodTotal1 -= tempReal;
                tempReal *= tempReal;
                periodTotal2 -= tempReal;
                outReal[outIdx++] = meanValue2 - meanValue1 * meanValue1;
            } while (i <= endIdx);

            outBegIdx = startIdx;
            outNbElement = outIdx;

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
