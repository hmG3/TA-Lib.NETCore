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
    /// <summary>
    /// Relative Strength Index (Momentum Indicators)
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
    /// Relative Strength Index is a momentum oscillator that measures the speed and change of price movements.
    /// It ranges from 0 to 100 and is used to identify overbought or oversold conditions, potential reversal points,
    /// and the strength of a trend.
    /// <para>
    /// The function is widely used in technical analysis as an indicator to assess market conditions and timing entry/exit points.
    /// It is calculated using Wilder's smoothing method for average gains and losses over the specified time period.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the difference between consecutive values to determine daily gains and losses:
    /// <code>
    /// Gain = Max(CurrentValue - PreviousValue, 0)
    /// Loss = Max(PreviousValue - CurrentValue, 0)
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Smooth the gains and losses using Wilder's smoothing method:
    /// <code>
    /// AvgGain = ((PreviousAvgGain * (TimePeriod - 1)) + CurrentGain) / TimePeriod
    /// AvgLoss = ((PreviousAvgLoss * (TimePeriod - 1)) + CurrentLoss) / TimePeriod
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Calculate the Relative Strength (RS) as the ratio of average gain to average loss:
    ///       <code>
    ///         RS = AvgGain / AvgLoss
    ///       </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the RSI using the RS value:
    ///       <code>
    ///         RSI = 100 - (100 / (1 + RS))
    ///       </code>
    ///       Alternatively:
    ///       <code>
    ///         RSI = 100 * (AvgGain / (AvgGain + AvgLoss))
    ///       </code>
    ///       The latter formula is optimized for computational efficiency.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A value above 70 suggests overbought conditions, potentially signaling a price reversal or pullback.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A value below 30 indicates oversold conditions, potentially signaling a price rebound or recovery.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Values near 50 suggest neutral momentum with no strong trend direction.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Rsi<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        RsiImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Rsi{T}">Rsi</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int RsiLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod < 2)
        {
            return -1;
        }

        var retValue = optInTimePeriod + Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Rsi);
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
    private static Core.RetCode Rsi<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        RsiImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode RsiImpl<T>(
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

        /* The following algorithm is base on the original work from Wilder's and shall represent
         * the original idea behind the classic RSI.
         *
         * Metastock is starting the calculation one price bar earlier.
         * To make this possible, they assume that the very first bar will be identical to the previous one (no gain or loss).
         */

        var lookbackTotal = RsiLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);

        int outIdx = default;

        // Accumulate Wilder's "Average Gain" and "Average Loss" among the initial period.
        var today = startIdx - lookbackTotal;
        var prevValue = inReal[today];

        // If there is an unstable period, no need to calculate since this first value will be surely skip.
        if (Core.UnstablePeriodSettings.Get(Core.UnstableFunc.Rsi) == 0 &&
            Core.CompatibilitySettings.Get() == Core.CompatibilityMode.Metastock &&
            ProcessRsiMetastockCompatibility(inReal, outReal, ref outRange, optInTimePeriod, endIdx, startIdx, ref prevValue, ref today,
                ref outIdx, out var retCode))
        {
            return retCode;
        }

        // Remaining of the processing is identical.
        InitGainsAndLosses(inReal, ref today, ref prevValue, optInTimePeriod, out T prevGain, out T prevLoss);

        /* Subsequent prevLoss and prevGain are smoothed using the previous values (Wilder's approach).
         *   1) Multiply the previous by 'period - 1'.
         *   2) Add today value.
         *   3) Divide by 'period'.
         */
        prevLoss /= timePeriod;
        prevGain /= timePeriod;

        /* Often documentation present the RSI calculation as follows:
         *   RSI = 100 - (100 / 1 + (prevGain / prevLoss))
         *
         * The following is equivalent:
         *   RSI = 100 * (prevGain / (prevGain + prevLoss))
         *
         * The second equation is used here for speed optimization.
         */
        if (today > startIdx)
        {
            var tempValue1 = prevGain + prevLoss;
            outReal[outIdx++] = !T.IsZero(tempValue1) ? FunctionHelpers.Hundred<T>() * (prevGain / tempValue1) : T.Zero;
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
            outReal[outIdx++] = !T.IsZero(tempValue1) ? FunctionHelpers.Hundred<T>() * (prevGain / tempValue1) : T.Zero;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static bool ProcessRsiMetastockCompatibility<T>(
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
        WriteInitialRsiValue(prevGain, prevLoss, optInTimePeriod, outReal, ref outIdx);

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

    private static void InitGainsAndLosses<T>(
        ReadOnlySpan<T> real,
        ref int today,
        ref T prevValue,
        int optInTimePeriod,
        out T prevGain,
        out T prevLoss) where T : IFloatingPointIeee754<T>
    {
        prevGain = T.Zero;
        prevLoss = T.Zero;
        today++;
        for (var i = optInTimePeriod; i > 0; i--)
        {
            var tempValue1 = real[today++];
            var tempValue2 = tempValue1 - prevValue;
            prevValue = tempValue1;

            if (tempValue2 < T.Zero)
            {
                prevLoss -= tempValue2;
            }
            else
            {
                prevGain += tempValue2;
            }
        }
    }

    private static void WriteInitialRsiValue<T>(
        T prevGain,
        T prevLoss,
        int optInTimePeriod,
        Span<T> outReal,
        ref int outIdx) where T : IFloatingPointIeee754<T>
    {
        var timePeriod = T.CreateChecked(optInTimePeriod);

        var tempValue1 = prevLoss / timePeriod;
        var tempValue2 = prevGain / timePeriod;

        tempValue1 = tempValue2 + tempValue1;
        outReal[outIdx++] = !T.IsZero(tempValue1) ? FunctionHelpers.Hundred<T>() * (tempValue2 / tempValue1) : T.Zero;
    }

    private static void ProcessToday<T>(
        ReadOnlySpan<T> real,
        ref int today,
        ref T prevValue,
        ref T prevGain,
        ref T prevLoss,
        T timePeriod) where T : IFloatingPointIeee754<T>
    {
        var tempValue1 = real[today++];
        var tempValue2 = tempValue1 - prevValue;
        prevValue = tempValue1;

        prevLoss *= timePeriod - T.One;
        prevGain *= timePeriod - T.One;

        if (tempValue2 < T.Zero)
        {
            prevLoss -= tempValue2;
        }
        else
        {
            prevGain += tempValue2;
        }

        prevLoss /= timePeriod;
        prevGain /= timePeriod;
    }
}
