/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2025 Anatolii Siryi
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

internal static class CandleHelpers
{
    public static T RealBody<T>(ReadOnlySpan<T> close, ReadOnlySpan<T> open, int idx) where T : IFloatingPointIeee754<T> =>
        T.Abs(close[idx] - open[idx]);

    public static T UpperShadow<T>(
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> close,
        ReadOnlySpan<T> open,
        int idx) where T : IFloatingPointIeee754<T> => high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);

    public static T LowerShadow<T>(
        ReadOnlySpan<T> close,
        ReadOnlySpan<T> open,
        ReadOnlySpan<T> low,
        int idx) where T : IFloatingPointIeee754<T> => (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];

    public static T HighLowRange<T>(ReadOnlySpan<T> high, ReadOnlySpan<T> low, int idx) where T : IFloatingPointIeee754<T> =>
        high[idx] - low[idx];

    public static Core.CandleColor CandleColor<T>(ReadOnlySpan<T> close, ReadOnlySpan<T> open, int idx)
        where T : IFloatingPointIeee754<T> => close[idx] >= open[idx] ? Core.CandleColor.White : Core.CandleColor.Black;

    public static int CandleAveragePeriod(Core.CandleSettingType set) => Core.CandleSettings.Get(set).AveragePeriod;

    public static T CandleRange<T>(
        ReadOnlySpan<T> open,
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        Core.CandleSettingType set,
        int idx) where T : IFloatingPointIeee754<T> => Core.CandleSettings.Get(set).RangeType switch
    {
        Core.CandleRangeType.RealBody => RealBody(close, open, idx),
        Core.CandleRangeType.HighLow => HighLowRange(high, low, idx),
        Core.CandleRangeType.Shadows => UpperShadow(high, close, open, idx) + LowerShadow(close, open, low, idx),
        _ => T.Zero
    };

    public static T CandleAverage<T>(
        ReadOnlySpan<T> open,
        ReadOnlySpan<T> high,
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> close,
        Core.CandleSettingType set,
        T sum,
        int idx) where T : IFloatingPointIeee754<T>
    {
        var candleAveragePeriod = T.CreateChecked(CandleAveragePeriod(set));
        var candleFactor = T.CreateChecked(Core.CandleSettings.Get(set).Factor);
        var rangeTypeFactor = Core.CandleSettings.Get(set).RangeType == Core.CandleRangeType.Shadows ? T.CreateChecked(2) : T.One;

        return candleFactor * (!T.IsZero(candleAveragePeriod)
            ? sum / candleAveragePeriod
            : CandleRange(open, high, low, close, set, idx)) / rangeTypeFactor;
    }

    public static bool RealBodyGapUp<T>(
        ReadOnlySpan<T> open,
        ReadOnlySpan<T> close,
        int idx2,
        int idx1) where T : IFloatingPointIeee754<T> => T.Min(open[idx2], close[idx2]) > T.Max(open[idx1], close[idx1]);

    public static bool RealBodyGapDown<T>(
        ReadOnlySpan<T> open,
        ReadOnlySpan<T> close,
        int idx2,
        int idx1) where T : IFloatingPointIeee754<T> => T.Max(open[idx2], close[idx2]) < T.Min(open[idx1], close[idx1]);

    public static bool CandleGapUp<T>(
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> high,
        int idx2,
        int idx1) where T : IFloatingPointIeee754<T> => low[idx2] > high[idx1];

    public static bool CandleGapDown<T>(
        ReadOnlySpan<T> low,
        ReadOnlySpan<T> high,
        int idx2,
        int idx1) where T : IFloatingPointIeee754<T> => high[idx2] < low[idx1];
}

/// <summary>
/// Provides a Candles layer for accessing candlestick patterns functions.
/// </summary>
public static partial class Candles;
