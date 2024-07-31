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
    [PublicAPI]
    public static Core.RetCode Var<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 5) where T : IFloatingPointIeee754<T> =>
        VarImpl(inReal, inRange, outReal, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int VarLookback(int optInTimePeriod = 5) => optInTimePeriod < 1 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Var<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 5) where T : IFloatingPointIeee754<T> =>
        VarImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod);

    private static Core.RetCode VarImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        return CalcVariance(inReal, inRange, outReal, out outRange, optInTimePeriod);
    }
}
