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
    public static Core.RetCode Accbands<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outRealUpperBand,
        Span<T> outRealMiddleBand,
        Span<T> outRealLowerBand,
        out Range outRange,
        int optInTimePeriod = 20) where T : IFloatingPointIeee754<T> =>
        AccbandsImpl(inHigh, inLow, inClose, inRange, outRealUpperBand, outRealMiddleBand, outRealLowerBand, out outRange, optInTimePeriod);

    [PublicAPI]
    public static int AccbandsLookback(int optInTimePeriod = 20) => optInTimePeriod < 2 ? -1 : SmaLookback(optInTimePeriod);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Accbands<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        Range inRange,
        T[] outRealUpperBand,
        T[] outRealMiddleBand,
        T[] outRealLowerBand,
        out Range outRange,
        int optInTimePeriod = 20) where T : IFloatingPointIeee754<T> =>
        AccbandsImpl<T>(inHigh, inLow, inClose, inRange, outRealUpperBand, outRealMiddleBand, outRealLowerBand, out outRange,
            optInTimePeriod);

    private static Core.RetCode AccbandsImpl<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        Range inRange,
        Span<T> outRealUpperBand,
        Span<T> outRealMiddleBand,
        Span<T> outRealLowerBand,
        out Range outRange,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        outRange = Range.EndAt(0);

        var startIdx = inRange.Start.Value;
        var endIdx = inRange.End.Value;

        if (endIdx < startIdx || endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = AccbandsLookback(optInTimePeriod);
        startIdx = Math.Max(startIdx, lookbackTotal);

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Buffer will contain also the lookback required for SMA to satisfy the caller requested startIdx/endIdx.
        var outputSize = endIdx - startIdx + 1;
        var bufferSize = outputSize + lookbackTotal;
        Span<T> tempBuffer1 = new T[bufferSize];
        Span<T> tempBuffer2 = new T[bufferSize];

        // Calculate the upper/lower band at the same time (no SMA yet).
        // Must start calculation back enough to cover the lookback required later for the SMA.
        for (int j = 0, i = startIdx - lookbackTotal; i <= endIdx; i++, j++)
        {
            var tempReal = inHigh[i] + inLow[i];
            if (!T.IsZero(tempReal))
            {
                tempReal = Four<T>() * (inHigh[i] - inLow[i]) / tempReal;
                tempBuffer1[j] = inHigh[i] * (T.One + tempReal);
                tempBuffer2[j] = inLow[i] * (T.One - tempReal);
            }
            else
            {
                tempBuffer1[j] = inHigh[i];
                tempBuffer2[j] = inLow[i];
            }
        }

        // Calculate the middle band, which is a moving average of the close.
        var retCode = SmaImpl(inClose, new Range(startIdx, endIdx), outRealMiddleBand, out var dummyRange, optInTimePeriod);
        if (retCode != Core.RetCode.Success || dummyRange.End.Value - dummyRange.Start.Value != outputSize)
        {
            return retCode;
        }

        // Take the SMA for the upper band.
        retCode = SmaImpl(tempBuffer1, Range.EndAt(bufferSize - 1), outRealUpperBand, out dummyRange, optInTimePeriod);
        if (retCode != Core.RetCode.Success || dummyRange.End.Value - dummyRange.Start.Value != outputSize)
        {
            return retCode;
        }

        // Take the SMA for the lower band.
        retCode = SmaImpl(tempBuffer2, Range.EndAt(bufferSize - 1), outRealLowerBand, out dummyRange, optInTimePeriod);
        if (retCode != Core.RetCode.Success || dummyRange.End.Value - dummyRange.Start.Value != outputSize)
        {
            return retCode;
        }

        outRange = new Range(startIdx, startIdx + outputSize);

        return Core.RetCode.Success;
    }
}
