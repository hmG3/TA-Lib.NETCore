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
    public static Core.RetCode Accbands<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        ReadOnlySpan<T> inClose,
        int startIdx,
        int endIdx,
        Span<T> outRealUpperBand,
        Span<T> outRealMiddleBand,
        Span<T> outRealLowerBand,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 20) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx ||
            endIdx >= inHigh.Length || endIdx >= inLow.Length || endIdx >= inClose.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = AccbandsLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

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
            T tempReal = inHigh[i] + inLow[i];
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
        var retCode = Sma(inClose, startIdx, endIdx, outRealMiddleBand, out _, out var outNbElementDummy, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElementDummy != outputSize)
        {
            return retCode;
        }

        // Take the SMA for the upper band.
        retCode = Sma(tempBuffer1, 0, bufferSize - 1, outRealUpperBand, out _, out outNbElementDummy, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElementDummy != outputSize)
        {
            return retCode;
        }

        // Take the SMA for the lower band.
        retCode = Sma(tempBuffer2, 0, bufferSize - 1, outRealLowerBand, out _, out outNbElementDummy, optInTimePeriod);
        if (retCode != Core.RetCode.Success || outNbElementDummy != outputSize)
        {
            return retCode;
        }

        outBegIdx = startIdx;
        outNbElement = outputSize;

        return Core.RetCode.Success;
    }

    public static int AccbandsLookback(int optInTimePeriod = 20) => optInTimePeriod < 2 ? -1 : SmaLookback(optInTimePeriod);

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Accbands<T>(
        T[] inHigh,
        T[] inLow,
        T[] inClose,
        int startIdx,
        int endIdx,
        T[] outRealUpperBand,
        T[] outRealMiddleBand,
        T[] outRealLowerBand,
        int optInTimePeriod = 20) where T : IFloatingPointIeee754<T> =>
        Accbands<T>(inHigh, inLow, inClose, startIdx, endIdx, outRealUpperBand, outRealMiddleBand, outRealLowerBand,
            out _, out _, optInTimePeriod);
}
