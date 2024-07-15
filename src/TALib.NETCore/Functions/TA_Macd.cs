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
    public static Core.RetCode Macd<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod = 12,
        int optInSlowPeriod = 26,
        int optInSignalPeriod = 9) where T : IFloatingPointIeee754<T> =>
        MacdImpl(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, optInFastPeriod,
            optInSlowPeriod, optInSignalPeriod);

    [PublicAPI]
    public static int MacdLookback(int optInFastPeriod = 12, int optInSlowPeriod = 26, int optInSignalPeriod = 9)
    {
        if (optInFastPeriod < 2 || optInSlowPeriod < 2 || optInSignalPeriod < 1)
        {
            return -1;
        }

        if (optInSlowPeriod < optInFastPeriod)
        {
            optInSlowPeriod = optInFastPeriod;
        }

        return EmaLookback(optInSlowPeriod) + EmaLookback(optInSignalPeriod);
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Macd<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outMACD,
        T[] outMACDSignal,
        T[] outMACDHist,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod = 12,
        int optInSlowPeriod = 26,
        int optInSignalPeriod = 9) where T : IFloatingPointIeee754<T> =>
        MacdImpl<T>(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, optInFastPeriod,
            optInSlowPeriod, optInSignalPeriod);

    private static Core.RetCode MacdImpl<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outMACD,
        Span<T> outMACDSignal,
        Span<T> outMACDHist,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod,
        int optInSlowPeriod,
        int optInSignalPeriod) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInFastPeriod < 2 || optInSlowPeriod < 2 || optInSignalPeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        return CalcMACD(inReal, startIdx, endIdx, outMACD, outMACDSignal, outMACDHist, out outBegIdx, out outNbElement, optInFastPeriod,
            optInSlowPeriod, optInSignalPeriod);
    }
}
