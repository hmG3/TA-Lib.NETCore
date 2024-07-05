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
    /// Represents the type of candlestick pattern, indicating its trend direction.
    /// </summary>
    public enum CandlePatternType
    {
        /// <summary>
        /// A strong bearish candlestick pattern, indicating a strong downward trend.
        /// </summary>
        StrongBearish = -200,

        /// <summary>
        /// A bearish candlestick pattern, typically represented by a black candle, indicating a downward trend.
        /// </summary>
        Bearish = -100,

        /// <summary>
        /// No specific trend or a neutral candlestick pattern.
        /// </summary>
        None = 0,

        /// <summary>
        /// A bullish candlestick pattern, typically represented by a white candle, indicating an upward trend.
        /// </summary>
        Bullish = 100,

        /// <summary>
        /// A strong bullish candlestick pattern, indicating a strong upward trend.
        /// </summary>
        StrongBullish = 200
    }
}
