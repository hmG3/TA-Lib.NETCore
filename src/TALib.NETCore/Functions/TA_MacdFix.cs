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

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode MacdFix(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out int outBegIdx,
        out int outNbElement,
        int optInSignalPeriod = 9)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInSignalPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        return CalcMACD(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, 0, 0,
            optInSignalPeriod);
    }

    public static int MacdFixLookback(int optInSignalPeriod = 9) => EmaLookback(26) + EmaLookback(optInSignalPeriod);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode MacdFix(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outMACD,
        T[] outMACDSignal,
        T[] outMACDHist,
        int optInSignalPeriod = 9) =>
        MacdFix(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out _, out _, optInSignalPeriod);
}
