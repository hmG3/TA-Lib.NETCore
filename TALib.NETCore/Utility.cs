using System;
using System.Collections.Generic;

namespace TALib
{
    public partial class Core
    {
        private static double TA_RealBody(in IReadOnlyList<double> close, in IReadOnlyList<double> open, int idx)
        {
            return Math.Abs(close[idx] - open[idx]);
        }

        private static decimal TA_RealBody(in IReadOnlyList<decimal> close, in IReadOnlyList<decimal> open, int idx)
        {
            return Math.Abs(close[idx] - open[idx]);
        }

        private static double TA_UpperShadow(in IReadOnlyList<double> high, in IReadOnlyList<double> close, in IReadOnlyList<double> open,
            int idx)
        {
            return high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);
        }

        private static decimal TA_UpperShadow(in IReadOnlyList<decimal> high, in IReadOnlyList<decimal> close,
            in IReadOnlyList<decimal> open, int idx)
        {
            return high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);
        }

        private static double TA_LowerShadow(in IReadOnlyList<double> close, in IReadOnlyList<double> open, in IReadOnlyList<double> low,
            int idx)
        {
            return (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];
        }

        private static decimal TA_LowerShadow(in IReadOnlyList<decimal> close, in IReadOnlyList<decimal> open,
            in IReadOnlyList<decimal> low, int idx)
        {
            return (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];
        }

        private static double TA_HighLowRange(in IReadOnlyList<double> high, in IReadOnlyList<double> low, int idx)
        {
            return high[idx] - low[idx];
        }

        private static decimal TA_HighLowRange(in IReadOnlyList<decimal> high, in IReadOnlyList<decimal> low, int idx)
        {
            return high[idx] - low[idx];
        }

        private static bool TA_CandleColor(in IReadOnlyList<double> close, in IReadOnlyList<double> open, int idx)
        {
            return close[idx] >= open[idx];
        }

        private static bool TA_CandleColor(in IReadOnlyList<decimal> close, in IReadOnlyList<decimal> open, int idx)
        {
            return close[idx] >= open[idx];
        }

        private static RangeType TA_CandleRangeType(CandleSettingType set)
        {
            return Globals.CandleSettings[(int) set].RangeType;
        }

        private static int TA_CandleAvgPeriod(CandleSettingType set)
        {
            return Globals.CandleSettings[(int) set].AvgPeriod;
        }

        private static double TA_CandleFactor(CandleSettingType set)
        {
            return Globals.CandleSettings[(int) set].Factor;
        }

        private static double TA_CandleRange(in IReadOnlyList<double> open, in IReadOnlyList<double> high, in IReadOnlyList<double> low,
            in IReadOnlyList<double> close, CandleSettingType set, int idx)
        {
            return TA_CandleRangeType(set) switch
            {
                RangeType.RealBody => TA_RealBody(close, open, idx),
                RangeType.HighLow => TA_HighLowRange(high, low, idx),
                RangeType.Shadows => (TA_UpperShadow(high, close, open, idx) + TA_LowerShadow(close, open, low, idx)),
                _ => 0
            };
        }

        private static decimal TA_CandleRange(in IReadOnlyList<decimal> open, in IReadOnlyList<decimal> high, in IReadOnlyList<decimal> low,
            in IReadOnlyList<decimal> close, CandleSettingType set, int idx)
        {
            return TA_CandleRangeType(set) switch
            {
                RangeType.RealBody => TA_RealBody(close, open, idx),
                RangeType.HighLow => TA_HighLowRange(high, low, idx),
                RangeType.Shadows => (TA_UpperShadow(high, close, open, idx) + TA_LowerShadow(close, open, low, idx)),
                _ => 0
            };
        }

        private static double TA_CandleAverage(in IReadOnlyList<double> open, in IReadOnlyList<double> high, in IReadOnlyList<double> low,
            in IReadOnlyList<double> close, CandleSettingType set, double sum, int idx)
        {
            return TA_CandleFactor(set) * (TA_CandleAvgPeriod(set) != 0
                       ? sum / TA_CandleAvgPeriod(set)
                       : TA_CandleRange(open, high, low, close, set, idx)) / (TA_CandleRangeType(set) == RangeType.Shadows ? 2.0 : 1.0);
        }

        private static decimal TA_CandleAverage(in IReadOnlyList<decimal> open, in IReadOnlyList<decimal> high,
            in IReadOnlyList<decimal> low, in IReadOnlyList<decimal> close, CandleSettingType set, decimal sum, int idx)
        {
            return (decimal) TA_CandleFactor(set) * (TA_CandleAvgPeriod(set) != 0
                       ? sum / TA_CandleAvgPeriod(set)
                       : TA_CandleRange(open, high, low, close, set, idx)) /
                   (TA_CandleRangeType(set) == RangeType.Shadows ? 2m : Decimal.One);
        }

        private static bool TA_RealBodyGapUp(in IReadOnlyList<double> open, in IReadOnlyList<double> close, int idx2, int idx1)
        {
            return Math.Min(open[idx2], close[idx2]) > Math.Max(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapUp(in IReadOnlyList<decimal> open, in IReadOnlyList<decimal> close, int idx2, int idx1)
        {
            return Math.Min(open[idx2], close[idx2]) > Math.Max(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapDown(in IReadOnlyList<double> open, in IReadOnlyList<double> close, int idx2, int idx1)
        {
            return Math.Max(open[idx2], close[idx2]) < Math.Min(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapDown(in IReadOnlyList<decimal> open, in IReadOnlyList<decimal> close, int idx2, int idx1)
        {
            return Math.Max(open[idx2], close[idx2]) < Math.Min(open[idx1], close[idx1]);
        }

        private static bool TA_CandleGapUp(in IReadOnlyList<double> low, in IReadOnlyList<double> high, int idx2, int idx1)
        {
            return low[idx2] > high[idx1];
        }

        private static bool TA_CandleGapUp(in IReadOnlyList<decimal> low, in IReadOnlyList<decimal> high, int idx2, int idx1)
        {
            return low[idx2] > high[idx1];
        }

        private static bool TA_CandleGapDown(in IReadOnlyList<double> low, in IReadOnlyList<double> high, int idx2, int idx1)
        {
            return high[idx2] < low[idx1];
        }

        private static bool TA_CandleGapDown(in IReadOnlyList<decimal> low, in IReadOnlyList<decimal> high, int idx2, int idx1)
        {
            return high[idx2] < low[idx1];
        }
    }
}
