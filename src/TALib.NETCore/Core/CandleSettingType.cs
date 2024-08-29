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
    /// Types of candlestick pattern settings.
    /// </summary>
    /// <remarks>
    /// The settings are based on the parts of the candle and the common words indicating the length (short, long, very long).
    /// </remarks>
    public enum CandleSettingType
    {
        /// <summary>
        /// Real body is long when it's longer than the average of the 10 previous candles' real body.
        /// </summary>
        BodyLong,

        /// <summary>
        /// Real body is very long when it's longer than 3 times the average of the 10 previous candles' real body.
        /// </summary>
        BodyVeryLong,

        /// <summary>
        /// Real body is short when it's shorter than the average of the 10 previous candles' real bodies.
        /// </summary>
        BodyShort,

        /// <summary>
        /// Real body is like doji's body when it's shorter than 10% the average of the 10 previous candles' high-low range.
        /// </summary>
        BodyDoji,

        /// <summary>
        /// Shadow is long when it's longer than the real body.
        /// </summary>
        ShadowLong,

        /// <summary>
        /// Shadow is very long when it's longer than 2 times the real body.
        /// </summary>
        ShadowVeryLong,

        /// <summary>
        /// Shadow is short when it's shorter than half the average of the 10 previous candles' sum of shadows.
        /// </summary>
        ShadowShort,

        /// <summary>
        /// Shadow is very short when it's shorter than 10% the average of the 10 previous candles' high-low range.
        /// </summary>
        ShadowVeryShort,

        /// <summary>
        /// When measuring distance between parts of candles or width of gaps.
        /// "near" means >=20% of the average of the 5 previous candles' high-low range"
        /// </summary>
        Near,

        /// <summary>
        /// When measuring distance between parts of candles or width of gaps.
        /// "far" means ">=60% of the average of the 5 previous candles' high-low range"
        /// </summary>
        Far,

        /// <summary>
        /// When measuring distance between parts of candles or width of gaps.
        /// "equal" means "≤5% of the average of the 5 previous candles' high-low range"
        /// </summary>
        Equal
    }
}
