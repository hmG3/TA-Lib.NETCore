namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Flags providing hints on how to display the output.
    /// </summary>
    [Flags]
    public enum OutputDisplayHints
    {
        /// <summary>
        /// Suggests displaying the output as a connected line.
        /// </summary>
        Line = 0x00000001,

        /// <summary>
        /// Suggests displaying the output as a dotted line.
        /// </summary>
        DotLine = 0x00000002,

        /// <summary>
        /// Suggests displaying the output as a dashed line.
        /// </summary>
        DashLine = 0x00000004,

        /// <summary>
        /// Suggests displaying the output using dots only.
        /// </summary>
        Dot = 0x00000008,

        /// <summary>
        /// Suggests displaying the output as a histogram.
        /// </summary>
        Histo = 0x00000010,

        /// <summary>
        /// Indicates whether a pattern exists (non-zero) or not (zero).
        /// </summary>
        PatternBool = 0x00000020,

        /// <summary>
        /// Indicates pattern type: zero means no pattern, greater than zero means bullish, and less than zero means bearish.
        /// </summary>
        PatternBullBear = 0x00000040,

        /// <summary>
        /// Indicates pattern strength: zero means neutral, (0..100] means getting bullish, (100..200] means bullish, [-100..0) means getting bearish, and [-200..-100) means bearish.
        /// </summary>
        PatternStrength = 0x00000080,

        /// <summary>
        /// Indicates that the output can be positive.
        /// </summary>
        Positive = 0x00000100,

        /// <summary>
        /// Indicates that the output can be negative.
        /// </summary>
        Negative = 0x00000200,

        /// <summary>
        /// Indicates that the output can be zero.
        /// </summary>
        Zero = 0x00000400,

        /// <summary>
        /// Indicates that the values represent an upper limit.
        /// </summary>
        UpperLimit = 0x00000800,

        /// <summary>
        /// Indicates that the values represent a lower limit.
        /// </summary>
        LowerLimit = 0x00001000
    }
}
