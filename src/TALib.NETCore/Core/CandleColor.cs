namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Represents the color of a candlestick, indicating its direction.
    /// </summary>
    internal enum CandleColor
    {
        /// <summary>
        /// Black candle representing a downward (bearish) trend.
        /// </summary>
        Black = -1,

        /// <summary>
        /// White candle representing an upward (bullish) trend.
        /// </summary>
        White = 1
    }
}
