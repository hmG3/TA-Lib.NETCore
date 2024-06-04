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
    public static Core.RetCode LinearRegAngle<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T>
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

        var lookbackTotal = LinearRegAngleLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        var today = startIdx;

        T timePeriod = T.CreateChecked(optInTimePeriod);
        T sumX = T.CreateChecked(optInTimePeriod * (optInTimePeriod - 1) * 0.5);
        T sumXSqr = T.CreateChecked(optInTimePeriod * (optInTimePeriod - 1) * (optInTimePeriod * 2 - 1) / 6.0);
        T divisor = sumX * sumX - timePeriod * sumXSqr;
        while (today <= endIdx)
        {
            T sumXY = T.Zero;
            T sumY = T.Zero;
            for (var i = optInTimePeriod; i-- != 0;)
            {
                T tempValue1 = inReal[today - i];
                sumY += tempValue1;
                sumXY += T.CreateChecked(i) * tempValue1;
            }

            T m = (timePeriod * sumXY - sumX * sumY) / divisor;
            outReal[outIdx++] = T.RadiansToDegrees(T.Atan(m));
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int LinearRegAngleLookback(int optInTimePeriod = 14) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode LinearRegAngle<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 14) where T : IFloatingPointIeee754<T> =>
        LinearRegAngle<T>(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
