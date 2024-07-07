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
    public static Core.RetCode SarExt<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        double optInStartValue = 0.0,
        double optInOffsetOnReverse = 0.0,
        double optInAccelerationInitLong = 0.02,
        double optInAccelerationLong = 0.02,
        double optInAccelerationMaxLong = 0.2,
        double optInAccelerationInitShort = 0.02,
        double optInAccelerationShort = 0.02,
        double optInAccelerationMaxShort = 0.2) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inHigh.Length || endIdx >= inLow.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInOffsetOnReverse < 0.0 || optInAccelerationInitLong < 0.0 || optInAccelerationLong < 0.0 ||
            optInAccelerationMaxLong < 0.0 || optInAccelerationInitShort < 0.0 || optInAccelerationShort < 0.0 ||
            optInAccelerationMaxShort < 0.0)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = SarExtLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        // Check if the acceleration factors are being defined by the caller.
        // Make sure the acceleration and maximum are coherent. If not, correct the acceleration.
        var afLong = AdjustAcceleration(ref optInAccelerationInitLong, ref optInAccelerationLong, optInAccelerationMaxLong);
        var afShort = AdjustAcceleration(ref optInAccelerationInitShort, ref optInAccelerationShort, optInAccelerationMaxShort);

        var (isLong, retCode) = DetermineInitialDirection(inHigh, inLow, optInStartValue, startIdx);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        outBegIdx = startIdx;
        int outIdx = default;

        var todayIdx = startIdx;

        var newHigh = inHigh[todayIdx - 1];
        var newLow = inLow[todayIdx - 1];
        var sar = InitializeSar(inHigh, inLow, optInStartValue, isLong, todayIdx, newLow, newHigh, out var ep);

        // Cheat on the newLow and newHigh for the first iteration.
        newLow = inLow[todayIdx];
        newHigh = inHigh[todayIdx];

        while (todayIdx <= endIdx)
        {
            var prevLow = newLow;
            var prevHigh = newHigh;
            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx++];
            if (isLong)
            {
                if (newLow <= sar)
                {
                    isLong = false;
                    sar = SwitchToShort(ref ep, prevHigh, newLow, newHigh, out afShort, optInAccelerationInitShort,
                        T.CreateChecked(optInOffsetOnReverse), ref outIdx, outReal);
                }
                else
                {
                    outReal[outIdx++] = sar;
                    sar = ProcessLongPosition(ref ep, prevLow, newLow, newHigh, ref afLong, optInAccelerationLong, optInAccelerationMaxLong,
                        sar);
                }
            }
            else if (newHigh >= sar)
            {
                isLong = true;
                sar = SwitchToLong(ref ep, prevLow, newLow, newHigh, out afLong, optInAccelerationInitLong,
                    T.CreateChecked(optInOffsetOnReverse), ref outIdx, outReal);
            }
            else
            {
                outReal[outIdx++] = -sar;

                sar = ProcessShortPosition(ref ep, prevHigh, newLow, newHigh, ref afShort, optInAccelerationShort,
                    optInAccelerationMaxShort, sar);
            }
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int SarExtLookback() => 1;

    private static double AdjustAcceleration(ref double optInAccelerationInit, ref double optInAcceleration, double optInAccelerationMax)
    {
        var af = optInAccelerationInit;
        if (af > optInAccelerationMax)
        {
            optInAccelerationInit = optInAccelerationMax;
            af = optInAccelerationInit;
        }

        optInAcceleration = Math.Min(optInAcceleration, optInAccelerationMax);

        return af;
    }

    private static (bool, Core.RetCode) DetermineInitialDirection<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        double optInStartValue,
        int startIdx) where T : IFloatingPointIeee754<T>
    {
        if (!optInStartValue.Equals(0.0))
        {
            return (optInStartValue > 0.0, Core.RetCode.Success);
        }

        Span<T> epTemp = new T[1];
        var retCode = MinusDM(inHigh, inLow, startIdx, startIdx, epTemp, out _, out _, 1);

        return retCode == Core.RetCode.Success ? (epTemp[0] <= T.Zero, Core.RetCode.Success) : (default, retCode);
    }

    private static T InitializeSar<T>(
        ReadOnlySpan<T> inHigh,
        ReadOnlySpan<T> inLow,
        double optInStartValue,
        bool isLong,
        int todayIdx,
        T newLow,
        T newHigh,
        out T ep) where T : IFloatingPointIeee754<T>
    {
        T sar;
        switch (optInStartValue)
        {
            case 0.0 when isLong:
                ep = inHigh[todayIdx];
                sar = newLow;
                break;
            case 0.0:
                ep = inLow[todayIdx];
                sar = newHigh;
                break;
            case > 0.0:
                ep = inHigh[todayIdx];
                sar = T.CreateChecked(optInStartValue);
                break;
            default:
                ep = inLow[todayIdx];
                sar = T.CreateChecked(Math.Abs(optInStartValue));
                break;
        }

        return sar;
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode SarExt<T>(
        T[] inHigh,
        T[] inLow,
        int startIdx,
        int endIdx,
        T[] outReal,
        double optInStartValue = 0.0,
        double optInOffsetOnReverse = 0.0,
        double optInAccelerationInitLong = 0.02,
        double optInAccelerationLong = 0.02,
        double optInAccelerationMaxLong = 0.2,
        double optInAccelerationInitShort = 0.02,
        double optInAccelerationShort = 0.02,
        double optInAccelerationMaxShort = 0.2) where T : IFloatingPointIeee754<T> =>
        SarExt<T>(inHigh, inLow, startIdx, endIdx, outReal, out _, out _, optInStartValue, optInOffsetOnReverse, optInAccelerationInitLong,
            optInAccelerationLong, optInAccelerationMaxLong, optInAccelerationInitShort, optInAccelerationShort, optInAccelerationMaxShort);
}
