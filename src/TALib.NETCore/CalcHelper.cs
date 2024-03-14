namespace TALib;

public static partial class Core
{
    internal static RetCode TA_INT_EMA(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod, double optInK1)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = Functions.EmaLookback(optInTimePeriod);
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
            if (CompatibilitySettings.Get() == CompatibilityMode.Default)
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

        internal static RetCode TA_INT_EMA(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod, decimal optInK1)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = Functions.EmaLookback(optInTimePeriod);
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
            if (CompatibilitySettings.Get() == CompatibilityMode.Default)
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

        internal static RetCode TA_INT_MACD(double[] inReal, int startIdx, int endIdx, double[] outMacd, double[] outMacdSignal,
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

            int lookbackSignal = Functions.EmaLookback(optInSignalPeriod);
            int lookbackTotal = Functions.MacdLookback(optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
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

        internal static RetCode TA_INT_MACD(decimal[] inReal, int startIdx, int endIdx, decimal[] outMacd, decimal[] outMacdSignal,
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

            int lookbackSignal = Functions.EmaLookback(optInSignalPeriod);
            int lookbackTotal = Functions.MacdLookback(optInFastPeriod, optInSlowPeriod, optInSignalPeriod);
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

        internal static RetCode TA_INT_PO(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInFastPeriod, int optInSlowPeriod, MAType optInMethod, double[] tempBuffer,
            bool doPercentageOutput)
        {
            outBegIdx = outNbElement = 0;

            if (optInSlowPeriod < optInFastPeriod)
            {
                (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            }

            RetCode retCode = Functions.Ma(inReal, startIdx, endIdx, tempBuffer, out var outBegIdx2, out _, optInFastPeriod, optInMethod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = Functions.Ma(inReal, startIdx, endIdx, outReal, out var outBegIdx1, out var outNbElement1, optInSlowPeriod, optInMethod);
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

        internal static RetCode TA_INT_PO(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInFastPeriod, int optInSlowPeriod, MAType optInMethod, decimal[] tempBuffer,
            bool doPercentageOutput)
        {
            outBegIdx = outNbElement = 0;

            if (optInSlowPeriod < optInFastPeriod)
            {
                (optInSlowPeriod, optInFastPeriod) = (optInFastPeriod, optInSlowPeriod);
            }

            RetCode retCode = Functions.Ma(inReal, startIdx, endIdx, tempBuffer, out var outBegIdx2, out _, optInFastPeriod, optInMethod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            retCode = Functions.Ma(inReal, startIdx, endIdx, outReal, out var outBegIdx1, out var outNbElement1, optInSlowPeriod, optInMethod);
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

        internal static RetCode TA_INT_SMA(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = Functions.SmaLookback(optInTimePeriod);
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

        internal static RetCode TA_INT_SMA(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = Functions.SmaLookback(optInTimePeriod);
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

        internal static void TA_INT_StdDevUsingPrecalcMA(double[] inReal, double[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
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

        internal static void TA_INT_StdDevUsingPrecalcMA(decimal[] inReal, decimal[] inMovAvg, int inMovAvgBegIdx, int inMovAvgNbElement,
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

        internal static RetCode TA_INT_VAR(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = Functions.VarLookback(optInTimePeriod);
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

        internal static RetCode TA_INT_VAR(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
            out int outNbElement, int optInTimePeriod)
        {
            outBegIdx = outNbElement = 0;

            int lookbackTotal = Functions.VarLookback(optInTimePeriod);
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
}
