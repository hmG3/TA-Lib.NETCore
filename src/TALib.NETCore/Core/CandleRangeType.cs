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
    /// Specifies the types of ranges that can be considered when comparing parts of a candlestick to other candlesticks.
    /// </summary>
    public enum CandleRangeType
    {
        /// <summary>
        /// The part of the candlestick between the opening and closing prices.
        /// </summary>
        RealBody,

        /// <summary>
        /// The entire range of the candlestick, from the highest to the lowest price.
        /// </summary>
        HighLow,

        /// <summary>
        /// The shadows (or tails) of the candlestick, which are the wicks extending above and below the real body.
        /// </summary>
        Shadows
    }
}
