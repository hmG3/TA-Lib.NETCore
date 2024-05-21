namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Represents the color of a candle.
    /// </summary>
    internal enum CandleColor
    {
        /// <summary>
        /// Black candle (down, bearish).
        /// </summary>
        Black = -1,

        /// <summary>
        /// White candle (up, bullish).
        /// </summary>
        White = 1
    }
}
