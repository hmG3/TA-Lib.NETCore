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
    public static Core.RetCode Ma(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma)
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

        if (optInTimePeriod == 1)
        {
            var nbElement = endIdx - startIdx + 1;
            outNbElement = nbElement;
            for (int todayIdx = startIdx, outIdx = 0; outIdx < nbElement; outIdx++, todayIdx++)
            {
                outReal[outIdx] = inReal[todayIdx];
            }

            outBegIdx = startIdx;

            return Core.RetCode.Success;
        }

        switch (optInMAType)
        {
            case Core.MAType.Sma:
                return Sma(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Ema:
                return Ema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Wma:
                return Wma(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Dema:
                return Dema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Tema:
                return Tema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Trima:
                return Trima(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Kama:
                return Kama(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            case Core.MAType.Mama:
                Span<T> dummyBuffer = new T[endIdx - startIdx + 1];
                return Mama(inReal, startIdx, endIdx, outReal, dummyBuffer, out outBegIdx, out outNbElement);
            case Core.MAType.T3:
                return T3(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
            default:
                return Core.RetCode.BadParam;
        }
    }

    public static int MaLookback(int optInTimePeriod = 30, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInTimePeriod switch
        {
            < 1 => -1,
            1 => 0,
            _ => optInMAType switch
            {
                Core.MAType.Sma => SmaLookback(optInTimePeriod),
                Core.MAType.Ema => EmaLookback(optInTimePeriod),
                Core.MAType.Wma => WmaLookback(optInTimePeriod),
                Core.MAType.Dema => DemaLookback(optInTimePeriod),
                Core.MAType.Tema => TemaLookback(optInTimePeriod),
                Core.MAType.Trima => TrimaLookback(optInTimePeriod),
                Core.MAType.Kama => KamaLookback(optInTimePeriod),
                Core.MAType.Mama => MamaLookback(),
                Core.MAType.T3 => T3Lookback(optInTimePeriod),
                _ => 0
            }
        };

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Ma(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) => Ma(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod, optInMAType);
}
