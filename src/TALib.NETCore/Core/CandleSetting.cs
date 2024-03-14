namespace TALib;

public static partial class Core
{
    public readonly record struct CandleSetting(CandleRangeType RangeType, int AveragePeriod, double Factor);
}
