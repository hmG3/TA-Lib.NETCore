namespace TALib;

public static partial class Functions
{
    public static Core.RetCode HtTrendline(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx,
        out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = HtTrendlineLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        const int smoothPriceSize = 50;
        var smoothPrice = new double[smoothPriceSize];

        double iTrend2, iTrend1;
        double iTrend3 = iTrend2 = iTrend1 = default;
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

        var i = 34;
        do
        {
            tempReal = inReal[today++];
            Core.DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
        } while (--i != 0);

        int hilbertIdx = default;
        int smoothPriceIdx = default;

        var hilbertVariables = Core.InitHilbertVariables<double>();

        int outIdx = default;

        double prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod;
        double period = prevI2 = prevQ2 =
            re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = default;
        while (today <= endIdx)
        {
            double i2;
            double q2;

            double adjustedPrevPeriod = 0.075 * period + 0.54;

            Core.DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);

            smoothPrice[smoothPriceIdx] = smoothedValue;
            if (today % 2 == 0)
            {
                Core.DoHilbertEven(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertEven(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertEven(hilbertVariables, "jI", i1ForEvenPrev3, hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertEven(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

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
                Core.DoHilbertOdd(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertOdd(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertOdd(hilbertVariables, "jI", i1ForOddPrev3, hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertOdd(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

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

            int idx = today;
            tempReal = default;
            for (i = 0; i < dcPeriodInt; i++)
            {
                tempReal += inReal[idx--];
            }

            if (dcPeriodInt > 0)
            {
                tempReal /= dcPeriodInt;
            }

            tempReal2 = (4.0 * tempReal + 3.0 * iTrend1 + 2.0 * iTrend2 + iTrend3) / 10.0;
            iTrend3 = iTrend2;
            iTrend2 = iTrend1;
            iTrend1 = tempReal;

            if (today >= startIdx)
            {
                outReal[outIdx++] = tempReal2;
            }

            if (++smoothPriceIdx > smoothPriceSize - 1)
            {
                smoothPriceIdx = 0;
            }

            today++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode HtTrendline(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx,
        out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = HtTrendlineLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        const int smoothPriceSize = 50;
        var smoothPrice = new decimal[smoothPriceSize];

        decimal iTrend2, iTrend1;
        decimal iTrend3 = iTrend2 = iTrend1 = default;
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

        var i = 34;
        do
        {
            tempReal = inReal[today++];
            Core.DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, tempReal, out _);
        } while (--i != 0);

        int hilbertIdx = default;
        int smoothPriceIdx = default;

        var hilbertVariables = Core.InitHilbertVariables<decimal>();

        int outIdx = default;

        decimal prevI2, prevQ2, re, im, i1ForOddPrev3, i1ForEvenPrev3, i1ForOddPrev2, i1ForEvenPrev2, smoothPeriod;
        decimal period = prevI2 = prevQ2 =
            re = im = i1ForOddPrev3 = i1ForEvenPrev3 = i1ForOddPrev2 = i1ForEvenPrev2 = smoothPeriod = default;
        while (today <= endIdx)
        {
            decimal i2;
            decimal q2;

            decimal adjustedPrevPeriod = 0.075m * period + 0.54m;

            Core.DoPriceWma(inReal, ref trailingWMAIdx, ref periodWMASub, ref periodWMASum, ref trailingWMAValue, inReal[today],
                out var smoothedValue);

            smoothPrice[smoothPriceIdx] = smoothedValue;
            if (today % 2 == 0)
            {
                Core.DoHilbertEven(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertEven(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertEven(hilbertVariables, "jI", i1ForEvenPrev3, hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertEven(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

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
                Core.DoHilbertOdd(hilbertVariables, "detrender", smoothedValue, hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertOdd(hilbertVariables, "q1", hilbertVariables["detrender"], hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertOdd(hilbertVariables, "jI", i1ForOddPrev3, hilbertIdx, adjustedPrevPeriod);
                Core.DoHilbertOdd(hilbertVariables, "jQ", hilbertVariables["q1"], hilbertIdx, adjustedPrevPeriod);

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

            int idx = today;
            tempReal = default;
            for (i = 0; i < dcPeriodInt; i++)
            {
                tempReal += inReal[idx--];
            }

            if (dcPeriodInt > 0)
            {
                tempReal /= dcPeriodInt;
            }

            tempReal2 = (4m * tempReal + 3m * iTrend1 + 2m * iTrend2 + iTrend3) / 10m;
            iTrend3 = iTrend2;
            iTrend2 = iTrend1;
            iTrend1 = tempReal;

            if (today >= startIdx)
            {
                outReal[outIdx++] = tempReal2;
            }

            if (++smoothPriceIdx > smoothPriceSize - 1)
            {
                smoothPriceIdx = 0;
            }

            today++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int HtTrendlineLookback() => Core.UnstablePeriodSettings.Get(Core.FuncUnstId.HtTrendline) + 63;
}
