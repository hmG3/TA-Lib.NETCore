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
    /// Moving average with variable period (Overlap Studies)
    /// </summary>
    /// <param name="inReal">A span of input values.</param>
    /// <param name="inPeriods">A span of period values that determine the moving average period for each data point.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInMinPeriod">The minimum allowable time period for calculating the moving average.</param>
    /// <param name="optInMaxPeriod">The maximum allowable time period for calculating the moving average.</param>
    /// <param name="optInMAType">The moving average type.</param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Moving Average Variable Period function calculates a moving average where the period can vary for each data point.
    /// This flexibility allows the moving average to adapt dynamically to changing conditions,
    /// such as volatility or custom-defined periods.
    /// <para>
    /// The function is particularly useful in scenarios where adaptability to market conditions or specific custom periods is required.
    /// The choice of <paramref name="optInMAType"/> and the range defined by <paramref name="optInMinPeriod"/> and
    /// <paramref name="optInMaxPeriod"/> significantly affects the behavior and responsiveness of the MAVP.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Truncate the input periods <paramref name="inPeriods"/> to fit within the specified MinPeriod and MaxPeriod.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the moving average for each data point using the corresponding period from the truncated series.
    ///       The moving average type <paramref name="optInMAType"/> determines the calculation method (e.g., SMA, EMA, etc.).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Populate the output with the calculated moving average values for each period.
    ///       Avoid redundant calculations by reusing results for identical periods in the input.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The MAVP output reflects a dynamically adjusted moving average based on the input period values.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Shorter periods result in more reactive and sensitive outputs, while longer periods provide smoother and less sensitive outputs.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Mavp<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inPeriods,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInMinPeriod = 2,
        int optInMaxPeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MavpImpl(inReal, inPeriods, inRange, outReal, out outRange, optInMinPeriod, optInMaxPeriod, optInMAType);

    /// <summary>
    /// Returns the lookback period for <see cref="Mavp{T}">Mavp</see>.
    /// </summary>
    /// <param name="optInMaxPeriod">The maximum allowable time period for calculating the moving average.</param>
    /// <param name="optInMAType">The moving average type.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int MavpLookback(int optInMaxPeriod = 30, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInMaxPeriod < 2 ? -1 : MaLookback(optInMaxPeriod, optInMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Mavp<T>(
        T[] inReal,
        T[] inPeriods,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInMinPeriod = 2,
        int optInMaxPeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MavpImpl<T>(inReal, inPeriods, inRange, outReal, out outRange, optInMinPeriod, optInMaxPeriod, optInMAType);

    private static Core.RetCode MavpImpl<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inPeriods,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInMinPeriod,
        int optInMaxPeriod,
        Core.MAType optInMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length, inPeriods.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInMinPeriod < 2 || optInMaxPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = MavpLookback(optInMaxPeriod, optInMAType);
        if (inPeriods.Length < lookbackTotal)
        {
            return Core.RetCode.BadParam;
        }

        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInt = lookbackTotal > startIdx ? lookbackTotal : startIdx;
        if (tempInt > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outputSize = endIdx - tempInt + 1;

        // Allocate intermediate local buffer.
        Span<T> localOutputArray = new T[outputSize];
        Span<int> localPeriodArray = new int[outputSize];

        // Copy caller array of period into local buffer. At the same time, truncate to min/max.
        for (var i = 0; i < outputSize; i++)
        {
            var period = Int32.CreateTruncating(inPeriods[startIdx + i]);
            localPeriodArray[i] = Math.Clamp(period, optInMinPeriod, optInMaxPeriod);
        }

        var intermediateOutput = outReal == inReal ? new T[outputSize] : outReal;

        /* Process each element of the input.
         * For each possible period value, the MA is calculated only once.
         * The outReal is then fill up for all element with the same period.
         * A local flag (value 0) is set in localPeriodArray to avoid doing a second time the same calculation.
         */
        var retCode = CalcMovingAverages(inReal, localPeriodArray, localOutputArray, new Range(startIdx, endIdx), outputSize, optInMAType,
            intermediateOutput);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        // Copy intermediate buffer to output buffer if necessary.
        if (intermediateOutput != outReal)
        {
            intermediateOutput[..outputSize].CopyTo(outReal);
        }

        outRange = new Range(startIdx, startIdx + outputSize);

        return Core.RetCode.Success;
    }

    private static Core.RetCode CalcMovingAverages<T>(
        ReadOnlySpan<T> real,
        Span<int> periodArray,
        Span<T> outputArray,
        Range range,
        int outputSize,
        Core.MAType maType,
        Span<T> intermediateOutput) where T : IFloatingPointIeee754<T>
    {
        for (var i = 0; i < outputSize; i++)
        {
            var curPeriod = periodArray[i];
            if (curPeriod == 0)
            {
                continue;
            }

            // Calculation of the MA required.
            var retCode = MaImpl(real, range, outputArray, out _, curPeriod, maType);
            if (retCode != Core.RetCode.Success)
            {
                return retCode;
            }

            intermediateOutput[i] = outputArray[i];
            for (var j = i + 1; j < outputSize; j++)
            {
                if (periodArray[j] == curPeriod)
                {
                    periodArray[j] = 0; // Flag to avoid recalculation
                    intermediateOutput[j] = outputArray[j];
                }
            }
        }

        return Core.RetCode.Success;
    }
}
