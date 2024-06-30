/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2024 Anatolii Siryi
 *
 * This file is part of Technical Analysis Library for .NET.
 *
 * Technical Analysis Library for .NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Technical Analysis Library for .NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with Technical Analysis Library for .NET. If not, see <https://www.gnu.org/licenses/>.
 */

namespace TALib;

public static partial class Functions
{
    public static Core.RetCode Kama<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = KamaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var sumROC1 = T.Zero;
        var today = startIdx - lookbackTotal;
        var trailingIdx = today;
        InitializeSumROC(inReal, ref sumROC1, ref today, optInTimePeriod);

        // At this point sumROC1 represent the summation of the 1-day price difference over the (optInTimePeriod - 1)

        // Calculate the first KAMA

        // The yesterday price is used here as the previous KAMA.
        var prevKAMA = inReal[today - 1];
        var tempReal = inReal[trailingIdx++];
        var periodROC = inReal[today] - tempReal;

        // Save the trailing value. Do this because inReal and outReal can be pointers to the same buffer.
        var trailingValue = tempReal;

        var efficiencyRatio = CalculateEfficiencyRatio(sumROC1, periodROC);
        var smoothingConstant = CalculateSmoothingConstant(efficiencyRatio);

        // Calculate the KAMA like an EMA, using the smoothing constant as the adaptive factor.
        prevKAMA = CalculateKAMA(inReal[today++], prevKAMA, smoothingConstant);

        // 'today' keep track of where the processing is within the input.
        while (today <= startIdx)
        {
            UpdateSumROC(inReal, ref sumROC1, ref today, ref trailingIdx, ref trailingValue);
            periodROC = inReal[today] - inReal[trailingIdx - 1];
            efficiencyRatio = CalculateEfficiencyRatio(sumROC1, periodROC);
            smoothingConstant = CalculateSmoothingConstant(efficiencyRatio);

            // Calculate the KAMA like an EMA, using the smoothing constant as the adaptive factor.
            prevKAMA = CalculateKAMA(inReal[today++], prevKAMA, smoothingConstant);
        }

        // Write the first value.
        outReal[0] = prevKAMA;
        var outIdx = 1;
        outBegIdx = today - 1;

        // Do the KAMA calculation for the requested range.
        while (today <= endIdx)
        {
            UpdateSumROC(inReal, ref sumROC1, ref today, ref trailingIdx, ref trailingValue);
            periodROC = inReal[today] - inReal[trailingIdx - 1];
            efficiencyRatio = CalculateEfficiencyRatio(sumROC1, periodROC);
            smoothingConstant = CalculateSmoothingConstant(efficiencyRatio);

            // Calculate the KAMA like an EMA, using the smoothing constant as the adaptive factor.
            prevKAMA = CalculateKAMA(inReal[today++], prevKAMA, smoothingConstant);

            outReal[outIdx++] = prevKAMA;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int KamaLookback(int optInTimePeriod = 30) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Kama);

    private static void InitializeSumROC<T>(
        ReadOnlySpan<T> inReal,
        ref T sumROC1,
        ref int today,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        for (var i = optInTimePeriod; i > 0; i--)
        {
            var tempReal = inReal[today++];
            tempReal -= inReal[today];
            sumROC1 += T.Abs(tempReal);
        }
    }

    private static T CalculateEfficiencyRatio<T>(T sumROC1, T periodROC) where T : IFloatingPointIeee754<T> =>
        sumROC1 <= periodROC || T.IsZero(sumROC1) ? T.One : T.Abs(periodROC / sumROC1);

    private static T CalculateSmoothingConstant<T>(T efficiencyRatio) where T : IFloatingPointIeee754<T>
    {
        var constMax = Two<T>() / (T.CreateChecked(30) + T.One);
        var constDiff = Two<T>() / (Two<T>() + T.One) - constMax;
        var tempReal = efficiencyRatio * constDiff + constMax;

        return tempReal * tempReal;
    }

    private static T CalculateKAMA<T>(T todayValue, T prevKAMA, T smoothingConstant) where T : IFloatingPointIeee754<T> =>
        (todayValue - prevKAMA) * smoothingConstant + prevKAMA;

    private static void UpdateSumROC<T>(
        ReadOnlySpan<T> inReal,
        ref T sumROC1,
        ref int today,
        ref int trailingIdx,
        ref T trailingValue) where T : IFloatingPointIeee754<T>
    {
        var tempReal = inReal[today];
        var tempReal2 = inReal[trailingIdx++];

        /* Adjust sumROC1:
         *  - Remove trailing ROC1
         *  - Add new ROC1
         */
        sumROC1 -= T.Abs(trailingValue - tempReal2);
        sumROC1 += T.Abs(tempReal - inReal[today - 1]);

        // Save the trailing value. Do this because inReal and outReal can be pointers to the same buffer.
        trailingValue = tempReal2;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Kama<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        Kama<T>(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
