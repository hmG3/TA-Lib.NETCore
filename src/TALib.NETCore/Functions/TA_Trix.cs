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
    public static Core.RetCode Trix<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        TrixImpl(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);

    [PublicAPI]
    public static int TrixLookback(int optInTimePeriod = 30) =>
        optInTimePeriod < 1 ? -1 : EmaLookback(optInTimePeriod) * 3 + RocRLookback(1);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Trix<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        TrixImpl<T>(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);

    private static Core.RetCode TrixImpl<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        var emaLookback = EmaLookback(optInTimePeriod);
        var lookbackTotal = TrixLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = startIdx;
        var nbElementToOutput = endIdx - startIdx + 1 + lookbackTotal;
        Span<T> tempBuffer = new T[nbElementToOutput];

        var k = Two<T>() / (T.CreateChecked(optInTimePeriod) + T.One);
        var retCode =
            CalcExponentialMA(inReal, startIdx - lookbackTotal, endIdx, tempBuffer, out _, out var nbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || nbElement == 0)
        {
            return retCode;
        }

        nbElementToOutput--;

        nbElementToOutput -= emaLookback;
        retCode = CalcExponentialMA(tempBuffer, 0, nbElementToOutput, tempBuffer, out _, out nbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || nbElement == 0)
        {
            return retCode;
        }

        nbElementToOutput -= emaLookback;
        retCode = CalcExponentialMA(tempBuffer, 0, nbElementToOutput, tempBuffer, out _, out nbElement, optInTimePeriod, k);
        if (retCode != Core.RetCode.Success || nbElement == 0)
        {
            return retCode;
        }

        // Calculate the 1-day Rate-Of-Change
        nbElementToOutput -= emaLookback;
        retCode = Roc(tempBuffer, 0, nbElementToOutput, outReal, out _, out outNbElement, 1);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        return Core.RetCode.Success;
    }
}
