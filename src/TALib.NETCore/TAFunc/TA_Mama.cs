namespace TALib
{
    public static partial class Core
    {
        public static RetCode Mama(double[] inReal, int startIdx, int endIdx, double[] outMAMA, double[] outFAMA, out int outBegIdx,
            out int outNbElement, double optInFastLimit = 0.5, double optInSlowLimit = 0.05)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMAMA == null || outFAMA == null || optInFastLimit < 0.01 || optInFastLimit > 0.99 ||
                optInSlowLimit < 0.01 || optInSlowLimit > 0.99)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MamaLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            const double rad2Deg = 180.0 / Math.PI;

            outBegIdx = startIdx;

            int trailingWMAIdx = startIdx - lookbackTotal;
            int today = trailingWMAIdx;

            double tempReal = inReal[today++];
            double periodWMASub = tempReal;
            double periodWMASum = tempReal;
            tempReal = inReal[today++];
            periodWMASub += tempReal;
            periodWMASum += tempReal * 2.0;
            tempReal = inReal[today++];
            periodWMASub += tempReal;
            periodWMASum += tempReal * 3.0;

            double trailingWMAValue = default;
            int i = 9;
            do
            {
                tempReal = inReal[today++];
                DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
            } while (--i != 0);

            int hilbertIdx = default;
            var hilbertVariables = InitHilbertVariables<double>();

            int outIdx = default;

            double prevI2, prevQ2, re, im, mama, fama, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, prevPhase;
            double period = prevI2 = prevQ2
                = re = im = mama = fama = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = prevPhase = default;
            while (today <= endIdx)
            {
                double tempReal2;
                double i2;
                double q2;

                double adjustedPrevPeriod = 0.075 * period + 0.54;

                double todayValue = inReal[today];
                DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue,
                    todayValue, out var smoothedValue);
                if (today % 2 == 0)
                {
                    DoHilbertEven(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                    DoHilbertEven(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                    DoHilbertEven(hilbertVariables, "jI", i1ForEvenPrev3, hilbertIdx, adjustedPrevPeriod);
                    DoHilbertEven(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

                    if (++hilbertIdx == 3)
                    {
                        hilbertIdx = 0;
                    }

                    q2 = 0.2 * (hilbertVariables["q1"] + hilbertVariables["jI"]) + 0.8 * prevQ2;
                    i2 = 0.2 * (i1ForEvenPrev3 - hilbertVariables["jQ"]) + 0.8 * prevI2;

                    i1ForOddPrev3 = i1ForOddPrev2;
                    i1ForOddPrev2 = hilbertVariables["detrender"];

                    tempReal2 = !i1ForEvenPrev3.Equals(0.0) ? Math.Atan(hilbertVariables["q1"] / i1ForEvenPrev3) * rad2Deg : 0.0;
                }
                else
                {
                    DoHilbertOdd(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                    DoHilbertOdd(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                    DoHilbertOdd(hilbertVariables, "jI", i1ForOddPrev3, hilbertIdx, adjustedPrevPeriod);
                    DoHilbertOdd(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

                    q2 = 0.2 * (hilbertVariables["q1"] + hilbertVariables["jI"]) + 0.8 * prevQ2;
                    i2 = 0.2 * (i1ForOddPrev3 - hilbertVariables["jQ"]) + 0.8 * prevI2;

                    i1ForEvenPrev3 = i1ForEvenPrev2;
                    i1ForEvenPrev2 = hilbertVariables["detrender"];
                    tempReal2 = !i1ForOddPrev3.Equals(0.0) ? Math.Atan(hilbertVariables["q1"] / i1ForOddPrev3) * rad2Deg : 0.0;
                }

                tempReal = prevPhase - tempReal2;
                prevPhase = tempReal2;
                if (tempReal < 1.0)
                {
                    tempReal = 1.0;
                }

                if (tempReal > 1.0)
                {
                    tempReal = optInFastLimit / tempReal;
                    if (tempReal < optInSlowLimit)
                    {
                        tempReal = optInSlowLimit;
                    }
                }
                else
                {
                    tempReal = optInFastLimit;
                }

                mama = tempReal * todayValue + (1.0 - tempReal) * mama;
                tempReal *= 0.5;
                fama = tempReal * mama + (1.0 - tempReal) * fama;
                if (today >= startIdx)
                {
                    outMAMA[outIdx] = mama;
                    outFAMA[outIdx++] = fama;
                }

                re = 0.2 * (i2 * prevI2 + q2 * prevQ2) + 0.8 * re;
                im = 0.2 * (i2 * prevQ2 - q2 * prevI2) + 0.8 * im;
                prevQ2 = q2;
                prevI2 = i2;
                tempReal = period;
                if (!im.Equals(0.0) && !re.Equals(0.0))
                {
                    period = 360.0 / (Math.Atan(im / re) * rad2Deg);
                }

                tempReal2 = 1.5 * tempReal;
                if (period > tempReal2)
                {
                    period = tempReal2;
                }

                tempReal2 = 0.67 * tempReal;
                if (period < tempReal2)
                {
                    period = tempReal2;
                }

                if (period < 6.0)
                {
                    period = 6.0;
                }
                else if (period > 50.0)
                {
                    period = 50.0;
                }

                period = 0.2 * period + 0.8 * tempReal;
                today++;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Mama(decimal[] inReal, int startIdx, int endIdx, decimal[] outMAMA, decimal[] outFAMA, out int outBegIdx,
            out int outNbElement, decimal optInFastLimit = 0.5m, decimal optInSlowLimit = 0.05m)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outMAMA == null || outFAMA == null || optInFastLimit < 0.01m || optInFastLimit > 0.99m ||
                optInSlowLimit < 0.01m || optInSlowLimit > 0.99m)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MamaLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            const decimal rad2Deg = 180m / DecimalMath.PI;

            outBegIdx = startIdx;

            int trailingWMAIdx = startIdx - lookbackTotal;
            int today = trailingWMAIdx;

            decimal tempReal = inReal[today++];
            decimal periodWMASub = tempReal;
            decimal periodWMASum = tempReal;
            tempReal = inReal[today++];
            periodWMASub += tempReal;
            periodWMASum += tempReal * 2m;
            tempReal = inReal[today++];
            periodWMASub += tempReal;
            periodWMASum += tempReal * 3m;

            decimal trailingWMAValue = default;
            int i = 9;
            do
            {
                tempReal = inReal[today++];
                DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
            } while (--i != 0);

            int hilbertIdx = default;
            var hilbertVariables = InitHilbertVariables<decimal>();

            int outIdx = default;

            decimal prevI2, prevQ2, re, im, mama, fama, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, prevPhase;
            decimal period = prevI2 = prevQ2
                = re = im = mama = fama = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = prevPhase = default;
            while (today <= endIdx)
            {
                decimal tempReal2;
                decimal i2;
                decimal q2;

                decimal adjustedPrevPeriod = 0.075m * period + 0.54m;

                decimal todayValue = inReal[today];
                DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue,
                    todayValue, out var smoothedValue);
                if (today % 2 == 0)
                {
                    DoHilbertEven(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                    DoHilbertEven(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                    DoHilbertEven(hilbertVariables, "jI", i1ForEvenPrev3, hilbertIdx, adjustedPrevPeriod);
                    DoHilbertEven(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

                    if (++hilbertIdx == 3)
                    {
                        hilbertIdx = 0;
                    }

                    q2 = 0.2m * (hilbertVariables["q1"] + hilbertVariables["jI"]) + 0.8m * prevQ2;
                    i2 = 0.2m * (i1ForEvenPrev3 - hilbertVariables["jQ"]) + 0.8m * prevI2;

                    i1ForOddPrev3 = i1ForOddPrev2;
                    i1ForOddPrev2 = hilbertVariables["detrender"];
                    tempReal2 = i1ForEvenPrev3 != Decimal.Zero
                        ? DecimalMath.Atan(hilbertVariables["q1"] / i1ForEvenPrev3) * rad2Deg
                        : Decimal.Zero;
                }
                else
                {
                    DoHilbertOdd(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                    DoHilbertOdd(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                    DoHilbertOdd(hilbertVariables, "jI", i1ForOddPrev3, hilbertIdx, adjustedPrevPeriod);
                    DoHilbertOdd(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

                    q2 = 0.2m * (hilbertVariables["q1"] + hilbertVariables["jI"]) + 0.8m * prevQ2;
                    i2 = 0.2m * (i1ForOddPrev3 - hilbertVariables["jQ"]) + 0.8m * prevI2;

                    i1ForEvenPrev3 = i1ForEvenPrev2;
                    i1ForEvenPrev2 = hilbertVariables["detrender"];
                    tempReal2 = i1ForOddPrev3 != Decimal.Zero
                        ? DecimalMath.Atan(hilbertVariables["q1"] / i1ForOddPrev3) * rad2Deg
                        : Decimal.Zero;
                }

                tempReal = prevPhase - tempReal2;
                prevPhase = tempReal2;
                if (tempReal < Decimal.One)
                {
                    tempReal = Decimal.One;
                }

                if (tempReal > Decimal.One)
                {
                    tempReal = optInFastLimit / tempReal;
                    if (tempReal < optInSlowLimit)
                    {
                        tempReal = optInSlowLimit;
                    }
                }
                else
                {
                    tempReal = optInFastLimit;
                }

                mama = tempReal * todayValue + (Decimal.One - tempReal) * mama;
                tempReal *= 0.5m;
                fama = tempReal * mama + (Decimal.One - tempReal) * fama;
                if (today >= startIdx)
                {
                    outMAMA[outIdx] = mama;
                    outFAMA[outIdx++] = fama;
                }

                re = 0.2m * (i2 * prevI2 + q2 * prevQ2) + 0.8m * re;
                im = 0.2m * (i2 * prevQ2 - q2 * prevI2) + 0.8m * im;
                prevQ2 = q2;
                prevI2 = i2;
                tempReal = period;
                if (im != Decimal.Zero && re != Decimal.Zero)
                {
                    period = 360m / (DecimalMath.Atan(im / re) * rad2Deg);
                }

                tempReal2 = 1.5m * tempReal;
                if (period > tempReal2)
                {
                    period = tempReal2;
                }

                tempReal2 = 0.67m * tempReal;
                if (period < tempReal2)
                {
                    period = tempReal2;
                }

                if (period < 6m)
                {
                    period = 6m;
                }
                else if (period > 50m)
                {
                    period = 50m;
                }

                period = 0.2m * period + 0.8m * tempReal;
                today++;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int MamaLookback() => (int) Globals.UnstablePeriod[(int) FuncUnstId.Mama] + 32;
    }
}
