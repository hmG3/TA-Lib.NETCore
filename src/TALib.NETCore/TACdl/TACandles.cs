using System.Collections.Generic;

namespace TALib
{
    public static partial class Candles
    {
        private static double TA_RealBody(IReadOnlyList<double> close, IReadOnlyList<double> open, int idx)
        {
            return Math.Abs(close[idx] - open[idx]);
        }

        private static decimal TA_RealBody(IReadOnlyList<decimal> close, IReadOnlyList<decimal> open, int idx)
        {
            return Math.Abs(close[idx] - open[idx]);
        }

        private static double TA_UpperShadow(IReadOnlyList<double> high, IReadOnlyList<double> close, IReadOnlyList<double> open, int idx)
        {
            return high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);
        }

        private static decimal TA_UpperShadow(IReadOnlyList<decimal> high, IReadOnlyList<decimal> close, IReadOnlyList<decimal> open,
            int idx)
        {
            return high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);
        }

        private static double TA_LowerShadow(IReadOnlyList<double> close, IReadOnlyList<double> open, IReadOnlyList<double> low, int idx)
        {
            return (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];
        }

        private static decimal TA_LowerShadow(IReadOnlyList<decimal> close, IReadOnlyList<decimal> open, IReadOnlyList<decimal> low,
            int idx)
        {
            return (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];
        }

        private static double TA_HighLowRange(IReadOnlyList<double> high, IReadOnlyList<double> low, int idx)
        {
            return high[idx] - low[idx];
        }

        private static decimal TA_HighLowRange(IReadOnlyList<decimal> high, IReadOnlyList<decimal> low, int idx)
        {
            return high[idx] - low[idx];
        }

        private static bool TA_CandleColor(IReadOnlyList<double> close, IReadOnlyList<double> open, int idx)
        {
            return close[idx] >= open[idx];
        }

        private static bool TA_CandleColor(IReadOnlyList<decimal> close, IReadOnlyList<decimal> open, int idx)
        {
            return close[idx] >= open[idx];
        }

        private static Core.CandleRangeType TA_CandleRangeType(Core.CandleSettingType set)
        {
            return Core.CandleSettings.Get(set).RangeType;
        }

        private static int TA_CandleAveragePeriod(Core.CandleSettingType set)
        {
            return Core.CandleSettings.Get(set).AveragePeriod;
        }

        private static double TA_CandleFactor(Core.CandleSettingType set)
        {
            return Core.CandleSettings.Get(set).Factor;
        }

        private static double TA_CandleRange(IReadOnlyList<double> open, IReadOnlyList<double> high, IReadOnlyList<double> low,
            IReadOnlyList<double> close, Core.CandleSettingType set, int idx)
        {
            return TA_CandleRangeType(set) switch
            {
                Core.CandleRangeType.RealBody => TA_RealBody(close, open, idx),
                Core.CandleRangeType.HighLow => TA_HighLowRange(high, low, idx),
                Core.CandleRangeType.Shadows => TA_UpperShadow(high, close, open, idx) + TA_LowerShadow(close, open, low, idx),
                _ => 0
            };
        }

        private static decimal TA_CandleRange(IReadOnlyList<decimal> open, IReadOnlyList<decimal> high, IReadOnlyList<decimal> low,
            IReadOnlyList<decimal> close, Core.CandleSettingType set, int idx)
        {
            return TA_CandleRangeType(set) switch
            {
                Core.CandleRangeType.RealBody => TA_RealBody(close, open, idx),
                Core.CandleRangeType.HighLow => TA_HighLowRange(high, low, idx),
                Core.CandleRangeType.Shadows => TA_UpperShadow(high, close, open, idx) + TA_LowerShadow(close, open, low, idx),
                _ => 0
            };
        }

        private static double TA_CandleAverage(IReadOnlyList<double> open, IReadOnlyList<double> high, IReadOnlyList<double> low,
            IReadOnlyList<double> close, Core.CandleSettingType set, double sum, int idx)
        {
            return TA_CandleFactor(set) * (TA_CandleAveragePeriod(set) != 0
                ? sum / TA_CandleAveragePeriod(set)
                : TA_CandleRange(open, high, low, close, set, idx)) / (TA_CandleRangeType(set) == Core.CandleRangeType.Shadows ? 2.0 : 1.0);
        }

        private static decimal TA_CandleAverage(IReadOnlyList<decimal> open, IReadOnlyList<decimal> high, IReadOnlyList<decimal> low,
            IReadOnlyList<decimal> close, Core.CandleSettingType set, decimal sum, int idx)
        {
            return (decimal) TA_CandleFactor(set) * (TA_CandleAveragePeriod(set) != 0
                       ? sum / TA_CandleAveragePeriod(set)
                       : TA_CandleRange(open, high, low, close, set, idx)) /
                   (TA_CandleRangeType(set) == Core.CandleRangeType.Shadows ? 2m : Decimal.One);
        }

        private static bool TA_RealBodyGapUp(IReadOnlyList<double> open, IReadOnlyList<double> close, int idx2, int idx1)
        {
            return Math.Min(open[idx2], close[idx2]) > Math.Max(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapUp(IReadOnlyList<decimal> open, IReadOnlyList<decimal> close, int idx2, int idx1)
        {
            return Math.Min(open[idx2], close[idx2]) > Math.Max(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapDown(IReadOnlyList<double> open, IReadOnlyList<double> close, int idx2, int idx1)
        {
            return Math.Max(open[idx2], close[idx2]) < Math.Min(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapDown(IReadOnlyList<decimal> open, IReadOnlyList<decimal> close, int idx2, int idx1)
        {
            return Math.Max(open[idx2], close[idx2]) < Math.Min(open[idx1], close[idx1]);
        }

        private static bool TA_CandleGapUp(IReadOnlyList<double> low, IReadOnlyList<double> high, int idx2, int idx1)
        {
            return low[idx2] > high[idx1];
        }

        private static bool TA_CandleGapUp(IReadOnlyList<decimal> low, IReadOnlyList<decimal> high, int idx2, int idx1)
        {
            return low[idx2] > high[idx1];
        }

        private static bool TA_CandleGapDown(IReadOnlyList<double> low, IReadOnlyList<double> high, int idx2, int idx1)
        {
            return high[idx2] < low[idx1];
        }

        private static bool TA_CandleGapDown(IReadOnlyList<decimal> low, IReadOnlyList<decimal> high, int idx2, int idx1)
        {
            return high[idx2] < low[idx1];
        }
    }
}
