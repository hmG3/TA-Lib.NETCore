/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2025 Anatolii Siryi
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
    /// <summary>
    /// Money Flow Index (Momentum Indicators)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inClose">A span of input close prices.</param>
    /// <param name="inVolume">A span of input volumes.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Money Flow Index is a momentum oscillator that measures the strength of money flowing in and out of a security over a given period.
    /// It combines price and volume data to indicate buying or selling pressure,
    /// and is often used to identify overbought or oversold conditions.
    /// <para>
    /// MFI is similar to the <see cref="Rsi{T}">RSI</see> but incorporates volume data.
    /// Combining it with trend indicators or <see cref="Obv{T}">OBV</see> may strengthen interpretive power.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Compute the typical price for each bar:
    ///       <code>
    ///         Typical Price = (High + Low + Close) / 3
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the raw money flow for each bar:
    ///       <code>
    ///         Money Flow = Typical Price * Volume
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Determine if the raw money flow is positive or negative by comparing the current typical price to the previous typical price:
    ///       - If the current typical price is greater than the previous typical price, it contributes to the positive money flow.
    ///       - If it is less, it contributes to the negative money flow.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Accumulate the positive and negative money flows over the specified time period (`optInTimePeriod`).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the Money Flow Index using the formula:
    ///       <code>
    ///         MFI = 100 * (Positive Money Flow / (Positive Money Flow + Negative Money Flow))
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value above 80 indicates overbought conditions, suggesting a potential trend reversal or pullback.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value below 20 indicates oversold conditions, suggesting a potential trend reversal or bounce.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Divergences between the MFI and price movement can signal potential trend reversals.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Mfi<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        MfiImpl(inHigh, inLow, inClose, inVolume, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Mfi{T}">Mfi</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MfiLookback(int optInTimePeriod = 14) =>
        optInTimePeriod < 2 ? -1 : optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Mfi);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Mfi<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        T[] inVolume,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        MfiImpl<T>(inHigh, inLow, inClose, inVolume, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode MfiImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        ReadOnlySpan<T> inVolume,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length, inClose.Length, inVolume.Length) is not
            { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MfiLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outIdx = 0;

        var moneyFlow = new (T negative, T positive)[optInTimePeriod];

        var mflowIdx = 0;
        var maxIdxMflow = optInTimePeriod - 1;

        // Accumulate the positive and negative money flow among the initial period.
        var today = startIdx - lookbackTotal;
        var prevValue = (inHigh[today] + inLow[today] + inClose[today]) / FunctionHelpers.Three<T>();

        var posSumMF = T.Zero;
        var negSumMF = T.Zero;
        today++;
        AccumulateInitialMoneyFlow(inHigh, inLow, inClose, inVolume, ref today, ref prevValue, ref posSumMF, ref negSumMF, moneyFlow,
            ref mflowIdx, maxIdxMflow, optInTimePeriod);

        /* The following two equations are equivalent:
         *   MFI = 100 - (100 / 1 + (posSumMF / negSumMF))
         *   MFI = 100 * (posSumMF / (posSumMF + negSumMF))
         * The second equation is used here for speed optimization.
         */
        if (today > startIdx)
        {
            var tempValue1 = posSumMF + negSumMF;
            outReal[outIdx++] = tempValue1 >= T.One ? FunctionHelpers.Hundred<T>() * (posSumMF / tempValue1) : T.Zero;
        }
        else
        {
            // Skip the unstable period. Do the processing but do not write it in the output.
            today = SkipMfiUnstablePeriod(inHigh, inLow, inClose, inVolume, today, startIdx, moneyFlow, maxIdxMflow, ref posSumMF,
                ref mflowIdx, ref negSumMF, ref prevValue);
        }

        while (today <= endIdx)
        {
            posSumMF -= moneyFlow[mflowIdx].positive;
            negSumMF -= moneyFlow[mflowIdx].negative;

            UpdateMoneyFlow(inHigh, inLow, inClose, inVolume, ref today, ref prevValue, ref posSumMF, ref negSumMF, moneyFlow,
                ref mflowIdx);

            var tempValue1 = posSumMF + negSumMF;
            outReal[outIdx++] = tempValue1 >= T.One ? FunctionHelpers.Hundred<T>() * (posSumMF / tempValue1) : T.Zero;

            if (++mflowIdx > maxIdxMflow)
            {
                mflowIdx = 0;
            }
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static void AccumulateInitialMoneyFlow<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        ReadOnlySpan<T> volume,
        ref int today,
        ref T prevValue,
        ref T posSumMF,
        ref T negSumMF,
        (T negative, T positive)[] moneyFlow,
        ref int mflowIdx,
        int maxIdxMflow,
        int timePeriod) where T : IFloatingPointIeee754<T>
    {
        for (var i = timePeriod; i > 0; i--)
        {
            UpdateMoneyFlow(high, low, close, volume, ref today, ref prevValue, ref posSumMF, ref negSumMF, moneyFlow, ref mflowIdx);

            if (++mflowIdx > maxIdxMflow)
            {
                mflowIdx = 0;
            }
        }
    }

    private static int SkipMfiUnstablePeriod<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        ReadOnlySpan<T> volume,
        int today,
        int startIdx,
        (T negative, T positive)[] moneyFlow,
        int maxIdxMflow,
        ref T posSumMF,
        ref int mFlowIdx,
        ref T negSumMF,
        ref T prevValue) where T : IFloatingPointIeee754<T>
    {
        while (today < startIdx)
        {
            posSumMF -= moneyFlow[mFlowIdx].positive;
            negSumMF -= moneyFlow[mFlowIdx].negative;

            UpdateMoneyFlow(high, low, close, volume, ref today, ref prevValue, ref posSumMF, ref negSumMF, moneyFlow, ref mFlowIdx);

            if (++mFlowIdx > maxIdxMflow)
            {
                mFlowIdx = 0;
            }
        }

        return today;
    }

    private static void UpdateMoneyFlow<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        ReadOnlySpan<T> volume,
        ref int today,
        ref T prevValue,
        ref T posSumMF,
        ref T negSumMF,
        (T negative, T positive)[] moneyFlow,
        ref int mflowIdx) where T : IFloatingPointIeee754<T>
    {
        var tempValue1 = (high[today] + low[today] + close[today]) / FunctionHelpers.Three<T>();
        var tempValue2 = tempValue1 - prevValue;
        prevValue = tempValue1;
        tempValue1 *= volume[today++];
        if (tempValue2 < T.Zero)
        {
            moneyFlow[mflowIdx].negative = tempValue1;
            negSumMF += tempValue1;
            moneyFlow[mflowIdx].positive = T.Zero;
        }
        else if (tempValue2 > T.Zero)
        {
            moneyFlow[mflowIdx].positive = tempValue1;
            posSumMF += tempValue1;
            moneyFlow[mflowIdx].negative = T.Zero;
        }
        else
        {
            moneyFlow[mflowIdx].positive = T.Zero;
            moneyFlow[mflowIdx].negative = T.Zero;
        }
    }
}
