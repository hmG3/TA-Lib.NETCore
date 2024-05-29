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

public static partial class Core
{
    /// <summary>
    /// Represents a candle setting.
    /// </summary>
    /// <param name="RangeType">The type of range to consider for the candle.</param>
    /// <param name="AveragePeriod">The number of previous candles to average when calculating the range.</param>
    /// <param name="Factor">A multiplier used to calculate the range of the candle.</param>
    public readonly record struct CandleSetting(CandleRangeType RangeType, int AveragePeriod, double Factor);
}
