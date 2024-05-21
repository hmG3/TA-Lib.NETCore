using System.Collections.Generic;

namespace TALib
{
    public static partial class Candles<T> where T : IFloatingPointIeee754<T>
    {
        private static T TA_RealBody(IReadOnlyList<T> close, IReadOnlyList<T> open, int idx)
        {
            return T.Abs(close[idx] - open[idx]);
        }

        private static T TA_UpperShadow(IReadOnlyList<T> high, IReadOnlyList<T> close, IReadOnlyList<T> open, int idx)
        {
            return high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);
        }

        private static T TA_LowerShadow(IReadOnlyList<T> close, IReadOnlyList<T> open, IReadOnlyList<T> low, int idx)
        {
            return (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];
        }

        private static T TA_HighLowRange(IReadOnlyList<T> high, IReadOnlyList<T> low, int idx)
        {
            return high[idx] - low[idx];
        }

        private static Core.CandleColor TA_CandleColor(IReadOnlyList<T> close, IReadOnlyList<T> open, int idx)
        {
            return close[idx] >= open[idx] ? Core.CandleColor.White : Core.CandleColor.Black;
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

        private static T TA_CandleRange(IReadOnlyList<T> open, IReadOnlyList<T> high, IReadOnlyList<T> low,
            IReadOnlyList<T> close, Core.CandleSettingType set, int idx) =>
            TA_CandleRangeType(set) switch
            {
                Core.CandleRangeType.RealBody => TA_RealBody(close, open, idx),
                Core.CandleRangeType.HighLow => TA_HighLowRange(high, low, idx),
                Core.CandleRangeType.Shadows => TA_UpperShadow(high, close, open, idx) + TA_LowerShadow(close, open, low, idx),
                _ => T.Zero
            };

        private static T TA_CandleAverage(IReadOnlyList<T> open, IReadOnlyList<T> high, IReadOnlyList<T> low,
            IReadOnlyList<T> close, Core.CandleSettingType set, T sum, int idx)
        {
            var candleAveragePeriod = T.CreateChecked(TA_CandleAveragePeriod(set));
            var candleFactor = T.CreateChecked(TA_CandleFactor(set));
            return candleFactor * (!T.IsZero(candleAveragePeriod)
                       ? sum / candleAveragePeriod
                       : TA_CandleRange(open, high, low, close, set, idx)) /
                   (TA_CandleRangeType(set) == Core.CandleRangeType.Shadows ? T.CreateChecked(2) : T.One);
        }

        private static bool TA_RealBodyGapUp(IReadOnlyList<T> open, IReadOnlyList<T> close, int idx2, int idx1)
        {
            return T.Min(open[idx2], close[idx2]) > T.Max(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapDown(IReadOnlyList<T> open, IReadOnlyList<T> close, int idx2, int idx1)
        {
            return T.Max(open[idx2], close[idx2]) < T.Min(open[idx1], close[idx1]);
        }

        private static bool TA_CandleGapUp(IReadOnlyList<T> low, IReadOnlyList<T> high, int idx2, int idx1)
        {
            return low[idx2] > high[idx1];
        }

        private static bool TA_CandleGapDown(IReadOnlyList<T> low, IReadOnlyList<T> high, int idx2, int idx1)
        {
            return high[idx2] < low[idx1];
        }
    }
}
