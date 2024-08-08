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
    public static Core.RetCode Ma<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MaImpl(inReal, inRange, outReal, out outRange, optInTimePeriod, optInMAType);

    [PublicAPI]
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
    [UsedImplicitly]
    private static Core.RetCode Ma<T>(
        T[] inReal,
        Range inRange,
        T[] outReal,
        out Range outRange,
        int optInTimePeriod = 30,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        MaImpl<T>(inReal, inRange, outReal, out outRange, optInTimePeriod, optInMAType);

    private static Core.RetCode MaImpl<T>(
        ReadOnlySpan<T> inReal,
        Range inRange,
        Span<T> outReal,
        out Range outRange,
        int optInTimePeriod,
        Core.MAType optInMAType) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        if (FunctionHelpers.ValidateInputRange(inRange, inReal.Length) is not { } rangeIndices)
        {
            return Core.RetCode.OutOfRangeParam;
        }

        var (startIdx, endIdx) = rangeIndices;

        if (optInTimePeriod < 1)
        {
            return Core.RetCode.BadParam;
        }

        if (optInTimePeriod == 1)
        {
            var nbElement = endIdx - startIdx + 1;
            for (int todayIdx = startIdx, outIdx = 0; outIdx < nbElement; outIdx++, todayIdx++)
            {
                outReal[outIdx] = inReal[todayIdx];
            }

            outRange = new Range(startIdx, startIdx + nbElement);

            return Core.RetCode.Success;
        }

        // Simply forward the job to the corresponding function.
        switch (optInMAType)
        {
            case Core.MAType.Sma:
                return Sma(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Ema:
                return Ema(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Wma:
                return Wma(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Dema:
                return Dema(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Tema:
                return Tema(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Trima:
                return Trima(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Kama:
                return Kama(inReal, inRange, outReal, out outRange, optInTimePeriod);
            case Core.MAType.Mama:
                Span<T> dummyBuffer = new T[endIdx - startIdx + 1];
                return Mama(inReal, inRange, outReal, dummyBuffer, out outRange);
            case Core.MAType.T3:
                return T3(inReal, inRange, outReal, out outRange, optInTimePeriod);
            default:
                return Core.RetCode.BadParam;
        }
    }
}
