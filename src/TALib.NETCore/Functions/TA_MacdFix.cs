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
    public static Core.RetCode MacdFix<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out Range outRange,
        int optInSignalPeriod = 9) where T : IFloatingPointIeee754<T> =>
        MacdFixImpl(inReal, inRange, outMACD, outMACDSignal, outMACDHist, out outRange, optInSignalPeriod);

    [PublicAPI]
    public static int MacdFixLookback(int optInSignalPeriod = 9) => EmaLookback(26) + EmaLookback(optInSignalPeriod);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode MacdFix<T>(
        T[] inReal,
        Range inRange,
        T[] outMACD,
        T[] outMACDSignal,
        T[] outMACDHist,
        out Range outRange,
        int optInSignalPeriod = 9) where T : IFloatingPointIeee754<T> =>
        MacdFixImpl<T>(inReal, inRange, outMACD, outMACDSignal, outMACDHist, out outRange, optInSignalPeriod);

    private static Core.RetCode MacdFixImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out Range outRange,
        int optInSignalPeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (ValidateInputRange(inRange, inReal.Length) is null)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        if (optInSignalPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        return CalcMACD(
            inReal,
            inRange,
            outMACD,
            outMACDSignal,
            outMACDHist,
            out outRange,
            0, /* 0 indicate fix 12 == 0.15  for optInFastPeriod */
            0, /* 0 indicate fix 26 == 0.075 for optInSlowPeriod */
            optInSignalPeriod);
    }
}
