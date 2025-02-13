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
    /// Linear Regression Angle (Statistic Functions)
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
    /// Linear Regression Angle calculates the angle of the slope of the best-fit line for a series of data points
    /// over a specified period. It provides a perspective on the direction and magnitude of the trend in degrees,
    /// helping to identify trend strength and direction.
    /// <para>
    /// The function can suggest the strength of a trend. Confirming with ADX or volume indicators may reduce misinterpretation of
    /// minor price changes as significant moves.
    /// </para>
    ///
    /// <b>Calculation steps</b>:
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Calculate the sums of X values (index positions), squared X values, and the product of X and Y values (input values)
    ///       over the specified period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Compute the slope (m) of the regression line using the formula:
    ///       <code>
    ///         m = (n * Sum(XY) - Sum(X) * Sum(Y)) / (n * Sum(X^2) - (Sum(X))^2)
    ///       </code>
    ///       where n is the time period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Convert the slope (m) into an angle in degrees using the arctangent function:
    ///       <code>
    ///         Angle = RadiansToDegrees(Atan(m))
    ///       </code>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <b>Value interpretation</b>:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A positive angle indicates an uptrend, where prices are increasing.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A negative angle indicates a downtrend, where prices are decreasing.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       A near-zero angle suggests a lack of significant trend in the data over the period.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The greater the absolute value of the angle, the stronger the trend.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    [PublicAPI]
    public static Core.RetCode LinearRegAngle<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        LinearRegAngleImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    /// <summary>
    /// Returns the lookback period for <see cref="LinearRegAngle{T}">LinearRegAngle</see>.
    /// </summary>
    /// <param name="optInTimePeriod">The time period.</param>
    /// <returns>The number of periods required before the first output value can be calculated.</returns>
    [PublicAPI]
    public static int LinearRegAngleLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode LinearRegAngle<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        LinearRegAngleImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode LinearRegAngleImpl<T>(
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

        /* Linear Regression is a concept also known as the "least squares method" or "best fit."
         * Linear Regression attempts to fit a straight line between several data points in such a way that
         * distance between each data point and the line is minimized.
         *
         * For each point, a straight line over the specified previous bar period is determined in terms of y = b + m * x:
         *
         * Returns 'm' in degree.
         */

        var lookbackTotal = LinearRegAngleLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var outIdx = 0;
        var today = startIdx;

        var timePeriod = T.CreateChecked(optInTimePeriod);
        var sumX = T.CreateChecked(optInTimePeriod * (optInTimePeriod - 1) * 0.5);
        var sumXSqr = T.CreateChecked(optInTimePeriod * (optInTimePeriod - 1) * (optInTimePeriod * 2 - 1) / 6.0);
        var divisor = sumX * sumX - timePeriod * sumXSqr;
        while (today <= endIdx)
        {
            T sumXY = T.Zero, sumY = T.Zero;
            for (var i = optInTimePeriod; i-- != 0;)
            {
                var tempValue1 = inReal[today - i];
                sumY += tempValue1;
                sumXY += T.CreateChecked(i) * tempValue1;
            }

            var m = (timePeriod * sumXY - sumX * sumY) / divisor;
            outReal[outIdx++] = T.RadiansToDegrees(T.Atan(m));
            today++;
        }

        outRange = new Range(startIdx, startIdx + outIdx);

        return Core.RetCode.Success;
    }
}
