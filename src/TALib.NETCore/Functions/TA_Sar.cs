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
    /// Parabolic SAR (Overlap Studies)
    /// </summary>
    /// <param name="inHigh">A span of input high prices.</param>
    /// <param name="inLow">A span of input low prices.</param>
    /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input spans.</param>
    /// <param name="outReal">A span to store the calculated values.</param>
    /// <param name="outRange">The range of indices representing the valid data within the output spans.</param>
    /// <param name="optInAcceleration">
    /// The acceleration factor that controls the sensitivity of the SAR:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values make the SAR more responsive to price changes but may increase the risk of false signals.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values reduce responsiveness, providing smoother outputs.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// A typical range is <c>0.01..0.2</c>.
    /// </para>
    /// </param>
    /// <param name="optInMaximum">
    /// The maximum value to which the acceleration factor can increase:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Higher values allow the SAR to accelerate more quickly during strong trends.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Lower values limit acceleration, maintaining smoother trends.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// A typical range is <c>0.01..0.5</c>.
    /// </para>
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
    /// Parabolic Stop and Reverse is a trend-following indicator designed to identify potential reversal points in the market.
    /// It plots a series of dots above or below price bars to signify the direction of the trend.
    /// As the trend progresses, the dots move closer to the price, providing dynamic stop-loss levels that adapt to changing market conditions.
    /// <para>
    /// The function is particularly useful for identifying trend direction, setting trailing stops, and detecting potential reversals.
    /// Pairing it with trend or volatility indicators such as <see cref="Adx{T}">ADX</see> or
    /// <see cref="Atr{T}">ATR</see> enhances its application.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Determine the initial trend direction based on the directional movement (DM) of the first two bars:
    ///       <code>
    ///         Direction = Long if +DM > -DM; otherwise, Short.
    ///       </code>
    ///       In the case of a tie, the trend defaults to Long.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Set the initial SAR (Stop and Reverse) and Extreme Point (EP):
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
    ///             Calculate the SAR using the formula:
    ///             <code>
    ///               SAR = Previous SAR + Acceleration Factor * (EP - Previous SAR)
    ///             </code>
    ///             Where the Acceleration Factor (AF) starts at the initial value and increases incrementally with new highs/lows up to the maximum limit.
    ///             The EP is updated to the new highest high (for Long) or lowest low (for Short).
    ///             </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             Ensure that the SAR does not penetrate the range of the previous two price bars.
    ///           </description>
    ///         </item>
    ///         <item>
    ///           <description>
    ///             If the SAR crosses the current price, the trend direction reverses, and the SAR is reset to the EP.
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
    public static Core.RetCode Sar<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        double optInAcceleration = 0.02,
        double optInMaximum = 0.2) where T : IFloatingPointIeee754<T> =>
        SarImpl(inHigh, inLow, inRange, outReal, out outRange, optInAcceleration, optInMaximum);

    /// <summary>
    /// Returns the lookback period for <see cref="Sar{T}">Sar</see>.
    /// </summary>
    /// <returns>Always 1 since there is only one price bar required for this calculation.</returns>
    [PublicAPI]
    public static int SarLookback() => 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Sar<T>(
        T[] inHigh,
        T[] inLow,
        Range inRange,
        T[] outReal,
        out Range outRange,
        double optInAcceleration = 0.02,
        double optInMaximum = 0.2) where T : IFloatingPointIeee754<T> =>
        SarImpl<T>(inHigh, inLow, inRange, outReal, out outRange, optInAcceleration, optInMaximum);

    private static Core.RetCode SarImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        double optInAcceleration,
        double optInMaximum) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inHigh.Length, inLow.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInAcceleration < 0.0 || optInMaximum < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        /* Implementation of the SAR has been a little bit open to interpretation since Wilder (the original author)
         * did not define a precise algorithm on how to bootstrap the algorithm.
         * Take any existing software application, and you will see slight variation on how the algorithm was adapted.
         *
         * What is the initial trade direction? Long or short?
         * ───────────────────────────────────────────────────
         * The interpretation of what should be the initial SAR values is open to interpretation,
         * particularly since the caller to the function does not specify the initial direction of the trade.
         *
         * The following logic is used:
         *   - Calculate +DM and -DM between the first and second bar.
         *     The highest directional indication will indicate the assumed direction of the trade for the second price bar.
         *   - In the case of a tie between +DM and -DM, the direction is LONG by default.
         *
         * What is the initial "extreme point" and thus SAR?
         * ─────────────────────────────────────────────────
         * The following shows how different people took different approach:
         *   - Metastock use the first price bar high/low depending on the direction.
         *     No SAR is calculated for the first price bar.
         *   - Tradestation use the closing price of the second bar.
         *     No SAR are calculated for the first price bar.
         *   - Wilder (the original author) use the SIP from the previous trade
         *     (cannot be implemented here since the direction and length of the previous trade is unknown).
         *   - The Magazine TASC seems to follow Wilder approach which is not practical here.
         *
         * The library "consume" the first price bar and use its high/low as the initial SAR of the second price bar.
         * It has found that approach to be the closest to Wilder's idea of having
         * the first entry day use the previous extreme point, except that here the extreme point is
         * derived solely from the first price bar. I found the same approach to be used by Metastock.
         */

        var lookbackTotal = SarLookback();
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Make sure the acceleration and maximum are coherent. If not, correct the acceleration.
        optInAcceleration = Math.Min(optInAcceleration, optInMaximum);
        var af = optInAcceleration;

        // Identify if the initial direction is long or short.
        // (ep is just used as a temp buffer here, the name of the parameter is not significant).
        Span<T> epTemp = new T[1];
        var retCode = MinusDMImpl(inHigh, inLow, new Range(startIdx, startIdx), epTemp, out _, 1);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        var outBegIdx = startIdx;
        var outIdx = 0;

        var todayIdx = startIdx;

        var newHigh = inHigh[todayIdx - 1];
        var newLow = inLow[todayIdx - 1];

        var isLong = epTemp[0] <= T.Zero;

        var sar = InitializeSar(inHigh, inLow, isLong, todayIdx, newLow, newHigh, out var ep);

        // Cheat on the newLow and newHigh for the first iteration.
        newLow = inLow[todayIdx];
        newHigh = inHigh[todayIdx];

        while (todayIdx <= endIdx)
        {
            var prevLow = newLow;
            var prevHigh = newHigh;
            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];
            todayIdx++;

            if (isLong)
            {
                // Switch to short if the low penetrates the SAR value.
                if (newLow <= sar)
                {
                    // Switch and override the SAR with the ep
                    isLong = false;
                    sar = SwitchToShort(ref ep, prevHigh, newLow, newHigh, out af, optInAcceleration, T.NegativeOne, ref outIdx, outReal);
                }
                else
                {
                    // No switch
                    // Output the SAR (was calculated in the previous iteration)
                    outReal[outIdx++] = sar;

                    sar = ProcessLongPosition(ref ep, prevLow, newLow, newHigh, ref af, optInAcceleration, optInMaximum, sar);
                }
            }
            /* Switch to long if the high penetrates the SAR value. */
            else if (newHigh >= sar)
            {
                /* Switch and override the SAR with the ep */
                isLong = true;
                sar = SwitchToLong(ref ep, prevLow, newLow, newHigh, out af, optInAcceleration, T.NegativeOne, ref outIdx, outReal);
            }
            else
            {
                // No switch
                // Output the SAR (was calculated in the previous iteration)
                outReal[outIdx++] = sar;

                sar = ProcessShortPosition(ref ep, prevHigh, newLow, newHigh, ref af, optInAcceleration, optInMaximum, sar);
            }
        }

        outRange = new Range(outBegIdx, outBegIdx + outIdx);

        return Core.RetCode.Success;
    }

    private static T InitializeSar<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        bool isLong,
        int todayIdx,
        T newLow,
        T newHigh,
        out T ep) where T : IFloatingPointIeee754<T>
    {
        T sar;
        if (isLong)
        {
            ep = inHigh[todayIdx];
            sar = newLow;
        }
        else
        {
            ep = inLow[todayIdx];
            sar = newHigh;
        }

        return sar;
    }

    private static T SwitchToShort<T>(
        ref T ep,
        T prevHigh,
        T newLow,
        T newHigh,
        out double af,
        double optInAcceleration,
        T optInOffsetOnReverse,
        ref int outIdx,
        Span<T> outReal) where T : IFloatingPointIeee754<T>
    {
        var sar = ep;

        // Make sure the overridden SAR is within yesterday's and today's range.
        sar = T.Max(sar, prevHigh);
        sar = T.Max(sar, newHigh);

        if (optInOffsetOnReverse > T.Zero)
        {
            sar += sar * optInOffsetOnReverse;
        }

        // Output the overridden SAR
        outReal[outIdx++] = sar * (optInOffsetOnReverse < T.Zero ? T.One : T.NegativeOne);

        // Adjust af and ep
        af = optInAcceleration;
        ep = newLow;

        sar += T.CreateChecked(af) * (ep - sar);

        // Make sure the new SAR is within yesterday's and today's range.
        sar = T.Max(sar, prevHigh);
        sar = T.Max(sar, newHigh);

        return sar;
    }

    private static T ProcessLongPosition<T>(
        ref T ep,
        T prevLow,
        T newLow,
        T newHigh,
        ref double af,
        double optInAcceleration,
        double optInMaximum,
        T sar) where T : IFloatingPointIeee754<T>
    {
        // Adjust af and ep.
        if (newHigh > ep)
        {
            ep = newHigh;
            af += optInAcceleration;
            af = Math.Min(af, optInMaximum);
        }

        // Calculate the new SAR
        sar += T.CreateChecked(af) * (ep - sar);

        // Make sure the new SAR is within yesterday's and today's range.
        sar = T.Min(sar, prevLow);
        sar = T.Min(sar, newLow);

        return sar;
    }

    private static T SwitchToLong<T>(
        ref T ep,
        T prevLow,
        T newLow,
        T newHigh,
        out double af,
        double optInAcceleration,
        T optInOffsetOnReverse,
        ref int outIdx,
        Span<T> outReal) where T : IFloatingPointIeee754<T>
    {
        var sar = ep;

        // Make sure the overridden SAR is within yesterday's and today's range.
        sar = T.Min(sar, prevLow);
        sar = T.Min(sar, newLow);

        if (optInOffsetOnReverse > T.Zero)
        {
            sar -= sar * optInOffsetOnReverse;
        }

        // Output the overridden SAR
        outReal[outIdx++] = sar;

        /* Adjust af and ep */
        af = optInAcceleration;
        ep = newHigh;

        sar += T.CreateChecked(af) * (ep - sar);

        // Make sure the new SAR is within yesterday's and today's range.
        sar = T.Min(sar, prevLow);
        sar = T.Min(sar, newLow);

        return sar;
    }

    private static T ProcessShortPosition<T>(
        ref T ep,
        T prevHigh,
        T newLow,
        T newHigh,
        ref double af,
        double optInAcceleration,
        double optInMaximum,
        T sar) where T : IFloatingPointIeee754<T>
    {
        // Adjust af and ep.
        if (newLow < ep)
        {
            ep = newLow;
            af += optInAcceleration;
            af = Math.Min(af, optInMaximum);
        }

        // Calculate the new SAR
        sar += T.CreateChecked(af) * (ep - sar);

        // Make sure the new SAR is within yesterday's and today's range.
        sar = T.Max(sar, prevHigh);
        sar = T.Max(sar, newHigh);

        return sar;
    }
}
