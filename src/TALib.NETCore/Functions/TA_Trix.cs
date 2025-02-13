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
    /// 1-day Rate-Of-Change (ROC) of a Triple Smooth EMA (Momentum Indicators)
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
    /// TRIX (1-day Rate-Of-Change of a Triple Smooth Exponential Moving Average) indicator measures
    /// the percentage rate of change of a triple exponentially smoothed moving average. It is commonly used
    /// to identify trends and potential reversals in time series data while filtering out short-term noise.
    /// <para>
    /// The function can identify trend reversals and underlying momentum changes.
    /// Confirming TRIX signals with volume data or oscillators can strengthen their quality.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate a single Exponential Moving Average (EMA) of the input data over the specified period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Apply two additional EMA calculations on the previously calculated EMA to achieve triple smoothing.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the 1-day Rate-Of-Change (ROC) of the triple smoothed EMA to obtain the TRIX values.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Positive values indicate an upward trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Negative values indicate a downward trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Changes in the direction of the TRIX can signal potential reversals or trend shifts.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode Trix<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        TrixImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="Trix{T}">Trix</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int TrixLookback(int optInTimePeriod = 30) =>
        optInTimePeriod < 1 ? -1 : EmaLookback(optInTimePeriod) * 3 + RocRLookback(1);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Trix<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        TrixImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode TrixImpl<T>(
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

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var emaLookback = EmaLookback(optInTimePeriod);
        var lookbackTotal = TrixLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outBegIdx = startIdx;
        var nbElementToOutput = endIdx - startIdx + 1 + lookbackTotal;
        Span<T> tempBuffer = new T[nbElementToOutput];

        var k = FunctionHelpers.Two<T>() / (T.CreateChecked(optInTimePeriod) + T.One);
        var retCode = FunctionHelpers.CalcExponentialMA(inReal, new Range(startIdx - lookbackTotal, endIdx), tempBuffer, out var range,
            optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || range.End.Value == 0)
        {
            return retCode;
        }

        nbElementToOutput--;

        nbElementToOutput -= emaLookback;
        retCode = FunctionHelpers.CalcExponentialMA(tempBuffer, Range.EndAt(nbElementToOutput), tempBuffer, out range, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || range.End.Value == 0)
        {
            return retCode;
        }

        nbElementToOutput -= emaLookback;
        retCode = FunctionHelpers.CalcExponentialMA(tempBuffer, Range.EndAt(nbElementToOutput), tempBuffer, out range, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || range.End.Value == 0)
        {
            return retCode;
        }

        // Calculate the 1-day Rate-Of-Change
        nbElementToOutput -= emaLookback;
        retCode = RocImpl(tempBuffer, Range.EndAt(nbElementToOutput), outReal, out range, 1);
        if (retCode != Core.RetCode.Success || range.End.Value == 0)
        {
            return retCode;
        }

        outRange = new Range(outBegIdx, outBegIdx + range.End.Value - range.Start.Value);

        return Core.RetCode.Success;
    }
}
