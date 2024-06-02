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
    public static Core.RetCode Tema(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30)
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

        var lookbackEMA = EmaLookback(optInTimePeriod);
        var lookbackTotal = TemaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var tempInt = lookbackTotal + (endIdx - startIdx) + 1;
        T k = TTwo / (T.CreateChecked(optInTimePeriod) + T.One);

        Span<T> firstEMA = new T[tempInt];
        var retCode = CalcExponentialMA(inReal, startIdx - lookbackEMA * 2, endIdx, firstEMA, out var firstEMABegIdx,
            out var firstEMANbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || firstEMANbElement == 0)
        {
            return retCode;
        }

        Span<T> secondEMA = new T[firstEMANbElement];
        retCode = CalcExponentialMA(firstEMA, 0, firstEMANbElement - 1, secondEMA, out var secondEMABegIdx, out var secondEMANbElement,
            optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || secondEMANbElement == 0)
        {
            return retCode;
        }

        retCode = CalcExponentialMA(secondEMA, 0, secondEMANbElement - 1, outReal, out var thirdEMABegIdx, out var thirdEMANbElement,
            optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || thirdEMANbElement == 0)
        {
            return retCode;
        }

        var firstEMAIdx = thirdEMABegIdx + secondEMABegIdx;
        var secondEMAIdx = thirdEMABegIdx;
        outBegIdx = firstEMAIdx + firstEMABegIdx;
        int outIdx = default;
        while (outIdx < thirdEMANbElement)
        {
            outReal[outIdx++] += TThree * firstEMA[firstEMAIdx++] - TThree * secondEMA[secondEMAIdx++];
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TemaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : EmaLookback(optInTimePeriod) * 3;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Tema(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 30) => Tema(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
