namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Represents a candle setting.
    /// </summary>
    /// <param name="RangeType">The type of range to consider for the candle.</param>
    /// <param name="AveragePeriod">The number of previous candles to average when calculating the range.</param>
    /// <param name="Factor">A multiplier used to calculate the range of the candle.</param>
    public readonly record struct CandleSetting(CandleRangeType RangeType, int AveragePeriod, double Factor);
}
