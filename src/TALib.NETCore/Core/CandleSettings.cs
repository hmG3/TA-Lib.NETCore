using System.Collections.Generic;

namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Candle settings for all candlestick patterns
    /// </summary>
    public static class CandleSettings
    {
        /// <summary>
        /// Default settings for all candle setting types
        /// </summary>
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
        /// Returns the candle setting for the requested type
        /// </summary>
        /// <param name="type">The candle setting type</param>
        public static CandleSetting Get(CandleSettingType type) => Settings[type];

        /// <summary>
        /// Changes the default candle setting for the requested type
        /// </summary>
        /// <param name="type">The candle setting type</param>
        /// <param name="setting">The candle setting</param>
        public static void Set(CandleSettingType type, CandleSetting setting) => Settings[type] = setting;
    }
}
