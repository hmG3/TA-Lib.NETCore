namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Defines the different types of moving averages.
    /// </summary>
    public enum MAType
    {
        /// <summary>
        /// An unweighted, arithmetic mean.
        /// </summary>
        Sma,

        /// <summary>
        /// The standard exponential moving average, using a smoothing factor of 2/(n+1).
        /// </summary>
        Ema,

        /// <summary>
        /// An exponential moving average, using a smoothing factor of 1/n and simple moving average as seeding.
        /// </summary>
        Wma,

        /// <summary>
        /// The double exponential moving average.
        /// </summary>
        Dema,

        /// <summary>
        /// The triple exponential moving average.
        /// </summary>
        Tema,

        /// <summary>
        /// The triangular moving average.
        /// </summary>
        Trima,

        /// <summary>
        /// The Kaufman Adaptive Moving Average.
        /// </summary>
        Kama,

        /// <summary>
        /// The MESA Adaptive Moving Average.
        /// </summary>
        Mama,

        /// <summary>
        /// The triple generalized double exponential moving average.
        /// </summary>
        T3
    }
}
