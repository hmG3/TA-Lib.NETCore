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
