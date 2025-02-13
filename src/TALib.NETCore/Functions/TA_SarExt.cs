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
    /// Parabolic SAR - Extended (Overlap Studies)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInStartValue">
    /// The starting SAR value used to initialize the calculation:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Positive values indicate a starting long position.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Negative values indicate a starting short position.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Typical range: <c>-100.0..100.0</c>, which determines the direction based on the first two price bars.
    /// </para>
    /// </param>
    /// <param name="optInOffsetOnReverse">
    /// An offset applied to the SAR value upon position reversal:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Positive values increase the distance between the SAR and the current price after reversal.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Negative values decrease the distance.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Typical range: <c>-0.5..0.5</c>.
    /// </para>
    /// </param>
    /// <param name="optInAccelerationInitLong">
    /// Initial acceleration factor for long positions:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values make the SAR more responsive at the start of a trend.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values smooth the initial SAR.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Typical range: <c>0.01..0.1</c>.
    /// </para>
    /// </param>
    /// <param name="optInAccelerationLong">
    /// Incremental acceleration factor for long positions:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values increase SAR sensitivity with each new extreme point.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values limit the rate of acceleration.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Typical range: <c>0.01..0.05</c>.
    /// </para>
    /// </param>
    /// <param name="optInAccelerationMaxLong">
    /// Maximum acceleration factor for long positions:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values allow faster SAR movement during trends.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values limit SAR movement, increasing stability.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Typical range: <c>0.1..0.5</c>.
    /// </para>
    /// </param>
    /// <param name="optInAccelerationInitShort">
    /// Initial acceleration factor for short positions, with behavior similar to <paramref name="optInAccelerationInitLong"/>.
    /// Typical range: <c>0.01..0.1</c>.
    /// </param>
    /// <param name="optInAccelerationShort">
    /// Incremental acceleration factor for short positions, with behavior similar to <paramref name="optInAccelerationLong"/>.
    /// Typical range: <c>0.01..0.05</c>.
    /// </param>
    /// <param name="optInAccelerationMaxShort">
    /// Maximum acceleration factor for short positions, with behavior similar to <paramref name="optInAccelerationMaxLong"/>.
    /// Typical range: <c>0.1..0.5</c>.
    /// </param>
    /// <typeparam name="T">
    /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
    /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
    /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
    /// </returns>
    /// <remarks>
    /// Extended Parabolic Stop and Reverse indicator is an enhanced version of the traditional <see cref="Sar{T}">SAR</see>.
    /// It allows for more granular control over the SAR calculation by providing separate parameters for long and short positions,
    /// as well as an optional offset applied during position reversals. SAR-Ext is particularly useful for adaptive trading strategies
    /// in volatile or trending markets.
    /// <para>
    /// By customizing acceleration factors, offsets, and initial values, the indicator's responsiveness to price changes can be fine-tuned.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///        Determine the initial trend direction based on the directional movement (DM) of the first two bars,
    ///       or explicitly set it via the <paramref name="optInStartValue"/> parameter:
    ///       <code>
    ///         Direction = Long if +DM > -DM; otherwise, Short.
    ///       </code>
    ///       If <paramref name="optInStartValue"/> is positive, the direction is Long. If negative, the direction is Short.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Set the initial SAR and Extreme Point (EP) based on the starting trend:
    /// <code>
    /// SAR = Lowest Low (for Long) or Highest High (for Short) of the first price bar.
    /// EP = Highest High (for Long) or Lowest Low (for Short) of the second price bar.
    /// </code>
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       For each subsequent price bar:
    ///       <list type="bullet">
    ///         <item>
    ///           <description>
    ///             Calculate the SAR using separate acceleration factors for Long and Short positions:
    ///             <code>
    ///               SAR = Previous SAR + Acceleration Factor * (EP - Previous SAR)
    ///             </code>
    ///             The Acceleration Factor (AF) starts at the initial value and increases incrementally with new highs/lows up to the maximum limit.
    ///             The EP is updated to the new highest high (for Long) or lowest low (for Short).
    ///             </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             Ensure the SAR does not penetrate the range of the previous two price bars.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             If the SAR crosses the current price, reverse the position, reset the SAR to the EP,
    ///             and apply the specified <paramref name="optInOffsetOnReverse"/> offset if provided.
    ///           </description>
    ///         </item>
    ///       </list>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       SAR dots below the price indicate an uptrend, providing potential stop-loss levels for long positions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       SAR dots above the price indicate a downtrend, providing potential stop-loss levels for short positions.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       When the SAR switches from below to above (or vice versa), it signals a potential trend reversal.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode SarExt<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        double optInStartValue = 0.0,
        double optInOffsetOnReverse = 0.0,
        double optInAccelerationInitLong = 0.02,
        double optInAccelerationLong = 0.02,
        double optInAccelerationMaxLong = 0.2,
        double optInAccelerationInitShort = 0.02,
        double optInAccelerationShort = 0.02,
        double optInAccelerationMaxShort = 0.2) where T : IFloatingPointIeee754<T> =>
        SarExtImpl(inHigh, inLow, inRange, outReal, out outRange, optInStartValue, optInOffsetOnReverse, optInAccelerationInitLong,
            optInAccelerationLong, optInAccelerationMaxLong, optInAccelerationInitShort, optInAccelerationShort, optInAccelerationMaxShort);

    /// <summary>
    /// Returns the lookback period for <see cref="SarExt{T}">SarExt</see>.
    /// </summary>
    /// <returns>Always 1 since there is only one price bar required for this calculation.</returns>
    [PublicAPI]
    public static int SarExtLookback() => 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode SarExt<T>(
        T[] inHigh,
        T[] inLow,
        Range inRange,
        T[] outReal,
        out Range outRange,
        double optInStartValue = 0.0,
        double optInOffsetOnReverse = 0.0,
        double optInAccelerationInitLong = 0.02,
        double optInAccelerationLong = 0.02,
        double optInAccelerationMaxLong = 0.2,
        double optInAccelerationInitShort = 0.02,
        double optInAccelerationShort = 0.02,
        double optInAccelerationMaxShort = 0.2) where T : IFloatingPointIeee754<T> =>
        SarExtImpl<T>(inHigh, inLow, inRange, outReal, out outRange, optInStartValue, optInOffsetOnReverse, optInAccelerationInitLong,
            optInAccelerationLong, optInAccelerationMaxLong, optInAccelerationInitShort, optInAccelerationShort, optInAccelerationMaxShort);

    private static Core.RetCode SarExtImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        double optInStartValue,
        double optInOffsetOnReverse,
        double optInAccelerationInitLong,
        double optInAccelerationLong,
        double optInAccelerationMaxLong,
        double optInAccelerationInitShort,
        double optInAccelerationShort,
        double optInAccelerationMaxShort) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInOffsetOnReverse < 0.0 || optInAccelerationInitLong < 0.0 || optInAccelerationLong < 0.0 ||
            optInAccelerationMaxLong < 0.0 || optInAccelerationInitShort < 0.0 || optInAccelerationShort < 0.0 ||
            optInAccelerationMaxShort < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = SarExtLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Check if the acceleration factors are being defined by the caller.
        // Make sure the acceleration and maximum are coherent. If not, correct the acceleration.
        var afLong = AdjustAcceleration(ref optInAccelerationInitLong, ref optInAccelerationLong, optInAccelerationMaxLong);
        var afShort = AdjustAcceleration(ref optInAccelerationInitShort, ref optInAccelerationShort, optInAccelerationMaxShort);

        var (isLong, retCode) = DetermineInitialDirection(inHigh, inLow, optInStartValue, startIdx);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var outBegIdx = startIdx;
        var outIdx = 0;

        var todayIdx = startIdx;

        var newHigh = inHigh[todayIdx - 1];
        var newLow = inLow[todayIdx - 1];
        var sar = InitializeSar(inHigh, inLow, optInStartValue, isLong, todayIdx, newLow, newHigh, out var ep);

        // Cheat on the newLow and newHigh for the first iteration.
        newLow = inLow[todayIdx];
        newHigh = inHigh[todayIdx];

        while (todayIdx <= endIdx)
        {
            var prevLow = newLow;
            var prevHigh = newHigh;
            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx++];
            if (isLong)
            {
                if (newLow <= sar)
                {
                    isLong = false;
                    sar = SwitchToShort(ref ep, prevHigh, newLow, newHigh, out afShort, optInAccelerationInitShort,
                        T.CreateChecked(optInOffsetOnReverse), ref outIdx, outReal);
                }
                else
                {
                    outReal[outIdx++] = sar;
                    sar = ProcessLongPosition(ref ep, prevLow, newLow, newHigh, ref afLong, optInAccelerationLong, optInAccelerationMaxLong,
                        sar);
                }
            }
            else if (newHigh >= sar)
            {
                isLong = true;
                sar = SwitchToLong(ref ep, prevLow, newLow, newHigh, out afLong, optInAccelerationInitLong,
                    T.CreateChecked(optInOffsetOnReverse), ref outIdx, outReal);
            }
            else
            {
                outReal[outIdx++] = -sar;

                sar = ProcessShortPosition(ref ep, prevHigh, newLow, newHigh, ref afShort, optInAccelerationShort,
                    optInAccelerationMaxShort, sar);
            }
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static double AdjustAcceleration(ref double optInAccelerationInit, ref double optInAcceleration, double optInAccelerationMax)
    {
        var af = optInAccelerationInit;
        if (af > optInAccelerationMax)
        {
            optInAccelerationInit = optInAccelerationMax;
            af = optInAccelerationInit;
        }

        optInAcceleration = Math.Min(optInAcceleration, optInAccelerationMax);

        return af;
    }

    private static (bool, Core.RetCode) DetermineInitialDirection<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        double optInStartValue,
        int startIdx) where T : IFloatingPointIeee754<T>
    {
        if (!optInStartValue.Equals(0.0))
        {
            return (optInStartValue > 0.0, Core.RetCode.Success);
        }

        Span<T> epTemp = new T[1];
        var retCode = MinusDMImpl(inHigh, inLow, new Range(startIdx, startIdx), epTemp, out _, 1);

        return retCode == Core.RetCode.Success ? (epTemp[0] <= T.Zero, Core.RetCode.Success) : (default, retCode);
    }

    private static T InitializeSar<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        double optInStartValue,
        bool isLong,
        int todayIdx,
        T newLow,
        T newHigh,
        out T ep) where T : IFloatingPointIeee754<T>
    {
        T sar;
        switch (optInStartValue)
        {
            case 0.0 when isLong:
                ep = inHigh[todayIdx];
                sar = newLow;
                break;
            case 0.0:
                ep = inLow[todayIdx];
                sar = newHigh;
                break;
            case > 0.0:
                ep = inHigh[todayIdx];
                sar = T.CreateChecked(optInStartValue);
                break;
            default:
                ep = inLow[todayIdx];
                sar = T.CreateChecked(Math.Abs(optInStartValue));
                break;
        }

        return sar;
    }
}
