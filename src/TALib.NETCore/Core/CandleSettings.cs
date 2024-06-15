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
    /// Provides settings for all candlestick patterns.
    /// </summary>
    public static class CandleSettings
    {
        /// <remarks>
        /// Initializes the default settings for all candlestick setting types.
        /// </remarks>
        private static readonly Dictionary<CandleSettingType, CandleSetting> Settings = new()
        {
            { CandleSettingType.BodyLong, new CandleSetting(CandleRangeType.RealBody, 10, 1.0) },
            { CandleSettingType.BodyVeryLong, new CandleSetting(CandleRangeType.RealBody, 10, 3.0) },
            { CandleSettingType.BodyShort, new CandleSetting(CandleRangeType.RealBody, 10, 1.0) },
            { CandleSettingType.BodyDoji, new CandleSetting(CandleRangeType.HighLow, 10, 0.1) },
            { CandleSettingType.ShadowLong, new CandleSetting(CandleRangeType.RealBody, 0, 1.0) },
            { CandleSettingType.ShadowVeryLong, new CandleSetting(CandleRangeType.RealBody, 0, 2.0) },
            { CandleSettingType.ShadowShort, new CandleSetting(CandleRangeType.Shadows, 10, 1.0) },
            { CandleSettingType.ShadowVeryShort, new CandleSetting(CandleRangeType.HighLow, 10, 0.1) },
            { CandleSettingType.Near, new CandleSetting(CandleRangeType.HighLow, 5, 0.2) },
            { CandleSettingType.Far, new CandleSetting(CandleRangeType.HighLow, 5, 0.6) },
            { CandleSettingType.Equal, new CandleSetting(CandleRangeType.HighLow, 5, 0.05) }
        };

        /// <summary>
        /// Retrieves the candle setting for a specified type.
        /// </summary>
        /// <param name="type">The type of the candle setting.</param>
        /// <returns>An instance of <see cref="CandleSetting"/> containing the settings for the specified type.</returns>
        public static CandleSetting Get(CandleSettingType type) => Settings[type];

        /// <summary>
        /// Updates the default candle setting for a specified type.
        /// </summary>
        /// <param name="type">The type of the candle setting.</param>
        /// <param name="setting">An instance of <see cref="CandleSetting"/> containing the new settings.</param>
        public static void Set(CandleSettingType type, CandleSetting setting) => Settings[type] = setting;
    }
}
