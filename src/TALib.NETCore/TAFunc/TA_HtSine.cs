namespace TALib
{
    public static partial class Core
    {
        public static RetCode HtSine(double[] inReal, int startIdx, int endIdx, double[] outSine, double[] outLeadSine, out int outBegIdx,
            out int outNbElement)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outSine == null || outLeadSine == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = HtSineLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            const int smoothPriceSize = 50;
            var smoothPrice = new double[smoothPriceSize];

            const double rad2Deg = 180.0 / Math.PI;
            const double deg2Rad = 1.0 / rad2Deg;
            const double constDeg2RadBy360 = 2.0 * Math.PI;

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
            var i = 34;
            do
            {
                tempReal = inReal[today++];
                DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
            } while (--i != 0);

            int hilbertIdx = default;
            int smoothPriceIdx = default;

            var hilbertVariables = InitHilbertVariables<double>();

            int outIdx = default;

            double prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod, dcPhase;
            double period = prevI2 = prevQ2 =
                re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = dcPhase = default;
            while (today <= endIdx)
            {
                double i2;
                double q2;

                double adjustedPrevPeriod = 0.075 * period + 0.54;

                DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                    out var smoothedValue);
                smoothPrice[smoothPriceIdx] = smoothedValue;
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

                double tempReal2 = 1.5 * tempReal;
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

                smoothPeriod = 0.33 * period + 0.67 * smoothPeriod;

                double dcPeriod = smoothPeriod + 0.5;
                var dcPeriodInt = (int) dcPeriod;
                double realPart = default;
                double imagPart = default;

                int idx = smoothPriceIdx;
                for (i = 0; i < dcPeriodInt; i++)
                {
                    tempReal = i * constDeg2RadBy360 / dcPeriodInt;
                    tempReal2 = smoothPrice[idx];
                    realPart += Math.Sin(tempReal) * tempReal2;
                    imagPart += Math.Cos(tempReal) * tempReal2;
                    if (idx == 0)
                    {
                        idx = smoothPriceSize - 1;
                    }
                    else
                    {
                        idx--;
                    }
                }

                tempReal = Math.Abs(imagPart);
                if (tempReal > 0.0)
                {
                    dcPhase = Math.Atan(realPart / imagPart) * rad2Deg;
                }
                else if (tempReal <= 0.01)
                {
                    if (realPart < 0.0)
                    {
                        dcPhase -= 90.0;
                    }
                    else if (realPart > 0.0)
                    {
                        dcPhase += 90.0;
                    }
                }

                dcPhase += 90.0;
                dcPhase += 360.0 / smoothPeriod;
                if (imagPart < 0.0)
                {
                    dcPhase += 180.0;
                }

                if (dcPhase > 315.0)
                {
                    dcPhase -= 360.0;
                }

                if (today >= startIdx)
                {
                    outSine[outIdx] = Math.Sin(dcPhase * deg2Rad);
                    outLeadSine[outIdx++] = Math.Sin((dcPhase + 45.0) * deg2Rad);
                }

                if (++smoothPriceIdx > smoothPriceSize - 1)
                {
                    smoothPriceIdx = 0;
                }

                today++;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode HtSine(decimal[] inReal, int startIdx, int endIdx, decimal[] outSine, decimal[] outLeadSine,
            out int outBegIdx, out int outNbElement)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outSine == null || outLeadSine == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = HtSineLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            const int smoothPriceSize = 50;
            var smoothPrice = new decimal[smoothPriceSize];

            const decimal rad2Deg = 180m / DecimalMath.PI;
            const decimal deg2Rad = Decimal.One / rad2Deg;
            const decimal constDeg2RadBy360 = 2m * DecimalMath.PI;

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
            var i = 34;
            do
            {
                tempReal = inReal[today++];
                DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
            } while (--i != 0);

            int hilbertIdx = default;
            int smoothPriceIdx = default;

            var hilbertVariables = InitHilbertVariables<decimal>();

            int outIdx = default;

            decimal prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod, dcPhase;
            decimal period = prevI2 = prevQ2 =
                re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = dcPhase = default;
            while (today <= endIdx)
            {
                decimal i2;
                decimal q2;

                decimal adjustedPrevPeriod = 0.075m * period + 0.54m;

                DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today], out var smoothedValue);
                smoothPrice[smoothPriceIdx] = smoothedValue;
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

                decimal tempReal2 = 1.5m * tempReal;
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

                smoothPeriod = 0.33m * period + 0.67m * smoothPeriod;

                decimal dcPeriod = smoothPeriod + 0.5m;
                var dcPeriodInt = (int) dcPeriod;
                decimal realPart = default;
                decimal imagPart = default;

                int idx = smoothPriceIdx;
                for (i = 0; i < dcPeriodInt; i++)
                {
                    tempReal = i * constDeg2RadBy360 / dcPeriodInt;
                    tempReal2 = smoothPrice[idx];
                    realPart += DecimalMath.Sin(tempReal) * tempReal2;
                    imagPart += DecimalMath.Cos(tempReal) * tempReal2;
                    if (idx == 0)
                    {
                        idx = smoothPriceSize - 1;
                    }
                    else
                    {
                        idx--;
                    }
                }

                tempReal = Math.Abs(imagPart);
                if (tempReal > Decimal.Zero)
                {
                    dcPhase = DecimalMath.Atan(realPart / imagPart) * rad2Deg;
                }
                else if (tempReal <= 0.01m)
                {
                    if (realPart < Decimal.Zero)
                    {
                        dcPhase -= 90m;
                    }
                    else if (realPart > Decimal.Zero)
                    {
                        dcPhase += 90m;
                    }
                }

                dcPhase += 90m;
                dcPhase += 360m / smoothPeriod;
                if (imagPart < Decimal.Zero)
                {
                    dcPhase += 180m;
                }

                if (dcPhase > 315m)
                {
                    dcPhase -= 360m;
                }

                if (today >= startIdx)
                {
                    outSine[outIdx] = DecimalMath.Sin(dcPhase * deg2Rad);
                    outLeadSine[outIdx++] = DecimalMath.Sin((dcPhase + 45m) * deg2Rad);
                }

                if (++smoothPriceIdx > smoothPriceSize - 1)
                {
                    smoothPriceIdx = 0;
                }

                today++;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int HtSineLookback() => (int) Globals.UnstablePeriod[(int) FuncUnstId.HtSine] + 63;
    }
}
