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
    public static Core.RetCode Apo(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInFastPeriod = 12,
        int optInSlowPeriod = 26,
        Core.MAType optInMAType = Core.MAType.Sma)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInFastPeriod < 2 || optInSlowPeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        Span<T> tempBuffer = new T[endIdx - startIdx + 1];

        return CalcPriceOscillator(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInFastPeriod, optInSlowPeriod,
            optInMAType, tempBuffer, false);
    }

    public static int ApoLookback(int optInFastPeriod = 12, int optInSlowPeriod = 26, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInFastPeriod < 2 || optInSlowPeriod < 2 ? -1 : MaLookback(Math.Max(optInSlowPeriod, optInFastPeriod), optInMAType);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Apo(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInFastPeriod = 12,
        int optInSlowPeriod = 26,
        Core.MAType optInMAType = Core.MAType.Sma) =>
        Apo(inReal, startIdx, endIdx, outReal, out _, out _, optInFastPeriod, optInSlowPeriod, optInMAType);
}
