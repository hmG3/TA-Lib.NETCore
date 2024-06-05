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
    public static Core.RetCode StdDev<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 5,
        double optInNbDev = 1.0) where T : IFloatingPointIeee754<T>
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

        var retCode = CalcVariance(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        if (!optInNbDev.Equals(1.0))
        {
            for (var i = 0; i < outNbElement; i++)
            {
                T tempReal = outReal[i];
                outReal[i] = tempReal > T.Zero ? T.Sqrt(tempReal) * T.CreateChecked(optInNbDev) : T.Zero;
            }
        }
        else
        {
            for (var i = 0; i < outNbElement; i++)
            {
                T tempReal = outReal[i];
                outReal[i] = tempReal > T.Zero ? T.Sqrt(tempReal) : T.Zero;
            }
        }

        return Core.RetCode.Success;
    }

    public static int StdDevLookback(int optInTimePeriod = 5) => optInTimePeriod < 2 ? -1 : VarLookback(optInTimePeriod);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode StdDev<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 5,
        double optInNbDev = 1.0) where T : IFloatingPointIeee754<T> =>
        StdDev<T>(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod, optInNbDev);
}
