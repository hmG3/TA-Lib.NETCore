using System.Collections.Generic;

namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Provides settings for all candlestick patterns.
    /// </summary>
    public static class CandleSettings
    {
        private static readonly Dictionary<CandleSettingType, CandleSetting> Settings;

        /// <summary>
        /// Initializes the default settings for all candlestick setting types.
        /// </summary>
        static CandleSettings()
        {
            Settings = new Dictionary<CandleSettingType, CandleSetting>
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
        }

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
