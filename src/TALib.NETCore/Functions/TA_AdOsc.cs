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
    public static Core.RetCode AdOsc<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod = 3,
        int optInSlowPeriod = 10) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length || endIdx >= inVolume.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInFastPeriod < 2 || optInSlowPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = AdOscLookback(optInFastPeriod, optInSlowPeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        var today = startIdx - lookbackTotal;

        T ad = T.Zero;

        T fastk = Two<T>() / (T.CreateChecked(optInFastPeriod) + T.One);
        T oneMinusFastk = T.One - fastk;

        T slowk = Two<T>() / (T.CreateChecked(optInSlowPeriod) + T.One);
        T oneMinusSlowk = T.One - slowk;

        // Use the same range of initialization inputs for both EMA and simply seed with the first A/D value.
        ad = CalcAccumulationDistribution(inHigh, inLow, inClose, inVolume, ref today, ad);
        T fastEMA = ad;
        T slowEMA = ad;

        // Initialize the EMA and skip the unstable period.
        while (today < startIdx)
        {
            ad = CalcAccumulationDistribution(inHigh, inLow, inClose, inVolume, ref today, ad);
            fastEMA = fastk * ad + oneMinusFastk * fastEMA;
            slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
        }

        // Perform the calculation for the requested range
        int outIdx = default;
        while (today <= endIdx)
        {
            ad = CalcAccumulationDistribution(inHigh, inLow, inClose, inVolume, ref today, ad);
            fastEMA = fastk * ad + oneMinusFastk * fastEMA;
            slowEMA = slowk * ad + oneMinusSlowk * slowEMA;

            outReal[outIdx++] = fastEMA - slowEMA;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AdOscLookback(int optInFastPeriod = 3, int optInSlowPeriod = 10)
    {
        var slowestPeriod = optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod;

        return optInFastPeriod < 2 || optInSlowPeriod < 2 ? -1 : EmaLookback(slowestPeriod);
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode AdOsc<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        T[] inVolume,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInFastPeriod = 3,
        int optInSlowPeriod = 10) where T : IFloatingPointIeee754<T> =>
        AdOsc<T>(inHigh, inLow, inClose, inVolume, startIdx, endIdx, outReal, out _, out _, optInFastPeriod, optInSlowPeriod);
}
