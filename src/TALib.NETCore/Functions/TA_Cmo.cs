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
    /// Chande Momentum Oscillator (Momentum Indicators)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
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
    /// Chande Momentum Oscillator is a momentum indicator that measures the difference between the sum of gains
    /// and losses over a specified time period, normalized by their total. It is similar to <see cref="Rsi{T}">RSI</see>,
    /// but CMO considers both the upward and downward momentum simultaneously in its calculation.
    /// <para>
    /// The function can detect momentum shifts. Incorporating it with trend confirmation tools may improve accuracy in timing decisions.
    /// The function is particularly useful in identifying overbought and oversold conditions and
    /// in confirming trends when used with other indicators.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the gains and losses between consecutive price values over the specified time period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the sum of gains and the sum of losses.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the CMO:
    ///       <code>
    ///         CMO = 100 * (Sum of Gains - Sum of Losses) / (Sum of Gains + Sum of Losses)
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A positive value indicates upward momentum, suggesting bullish conditions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A negative value indicates downward momentum, suggesting bearish conditions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Values near zero suggest a lack of significant momentum in either direction.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Cmo<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        CmoImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Cmo{T}">Cmo</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int CmoLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod < 2)
        {
            return -1;
        }

        var retValue = optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Cmo);
        if (Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock)
        {
            retValue--;
        }

        return retValue;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Cmo<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        CmoImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode CmoImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        /* CMO calculation is mostly identical to RSI.
         *
         * The only difference is in the last step of calculation:
         *
         *   RSI = gain / (gain+loss)
         *   CMO = (gain-loss) / (gain+loss)
         *
         * See the Rsi function for potentially some more info on this algo.
         */

        var lookbackTotal = CmoLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        var outIdx = 0;

        // Accumulate Wilder's "Average Gain" and "Average Loss" among the initial period.
        var today = startIdx - lookbackTotal;
        var prevValue = inReal[today];

        // If there is an unstable period, no need to calculate since this first value will be surely skip.
        if (Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Cmo) == 0 &&
            Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock &&
            ProcessCmoMetastockCompatibility(inReal, outReal, ref outRange, optInTimePeriod, endIdx, startIdx, ref prevValue, ref today,
                ref outIdx, out var retCode))
        {
            return retCode;
        }

        InitGainsAndLosses(inReal, ref today, ref prevValue, optInTimePeriod, out T prevGain, out T prevLoss);

        /* Subsequent prevLoss and prevGain are smoothed using the previous values (Wilder's approach).
         * 1) Multiply the previous by 'period - 1'.
         * 2) Add today value.
         * 3) Divide by 'period'.
         */
        prevLoss /= timePeriod;
        prevGain /= timePeriod;

        /* Often documentation present the RSI calculation as follows:
         *    RSI = 100 - (100 / 1 + (prevGain / prevLoss))
         *
         * The following is equivalent:
         *    RSI = 100 * (prevGain / (prevGain + prevLoss))
         *
         * The second equation is used here for speed optimization.
         */
        if (today > startIdx)
        {
            var tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !T.IsZero(tempValue1) ? FunctionHelpers.Hundred<T>() * ((prevGain - prevLoss) / tempValue1) : T.Zero;
        }
        else
        {
            // Skip the unstable period. Do the processing but do not write it in the output.
            while (today < startIdx)
            {
                ProcessToday(inReal, ref today, ref prevValue, ref prevGain, ref prevLoss, timePeriod);
            }
        }

        // Unstable period skipped... now continue processing if needed.
        while (today <= endIdx)
        {
            ProcessToday(inReal, ref today, ref prevValue, ref prevGain, ref prevLoss, timePeriod);
            var tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !T.IsZero(tempValue1) ? FunctionHelpers.Hundred<T>() * ((prevGain - prevLoss) / tempValue1) : T.Zero;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool ProcessCmoMetastockCompatibility<T>(
        ReadOnlySpan<T> inReal,
        Span<T> outReal,
        ref Range outRange,
        int optInTimePeriod,
        int endIdx,
        int startIdx,
        ref T prevValue,
        ref int today,
        ref int outIdx,
        out Core.RetCode retCode)
        where T : IFloatingPointIeee754<T>
    {
        // Preserve prevValue because it may get overwritten by the output.
        // (because output ptr could be the same as input ptr).
        var savePrevValue = prevValue;

        InitGainsAndLosses(inReal, ref today, ref prevValue, optInTimePeriod, out T prevGain, out T prevLoss);
        WriteInitialCmoValue(prevGain, prevLoss, optInTimePeriod, outReal, ref outIdx);

        if (today > endIdx)
        {
            outRange = new Range(startIdx, startIdx + outIdx);
            retCode = Core.RetCode.Success;

            return true;
        }

        // Start over for the next price bar.
        today -= optInTimePeriod;
        prevValue = savePrevValue;
        retCode = Core.RetCode.Success;

        return false;
    }

    private static void WriteInitialCmoValue<T>(
        T prevGain,
        T prevLoss,
        int optInTimePeriod,
        Span<T> outReal,
        ref int outIdx) where T : IFloatingPointIeee754<T>
    {
        var timePeriod = T.CreateChecked(optInTimePeriod);

        var tempValue1 = prevLoss / timePeriod;
        var tempValue2 = prevGain / timePeriod;
        var tempValue3 = tempValue2 - tempValue1;
        var tempValue4 = tempValue1 + tempValue2;

        outReal[outIdx++] = !T.IsZero(tempValue4) ? FunctionHelpers.Hundred<T>() * (tempValue3 / tempValue4) : T.Zero;
    }
}
