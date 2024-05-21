using System.Collections.Generic;

namespace TALib
{
    public static partial class Candles<T> where T : IFloatingPointIeee754<T>
    {
        private static T RealBody(IReadOnlyList<T> close, IReadOnlyList<T> open, int idx)
        {
            return T.Abs(close[idx] - open[idx]);
        }

        private static T UpperShadow(IReadOnlyList<T> high, IReadOnlyList<T> close, IReadOnlyList<T> open, int idx)
        {
            return high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);
        }

        private static T LowerShadow(IReadOnlyList<T> close, IReadOnlyList<T> open, IReadOnlyList<T> low, int idx)
        {
            return (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];
        }

        private static T HighLowRange(IReadOnlyList<T> high, IReadOnlyList<T> low, int idx)
        {
            return high[idx] - low[idx];
        }

        private static Core.CandleColor CandleColor(IReadOnlyList<T> close, IReadOnlyList<T> open, int idx)
        {
            return close[idx] >= open[idx] ? Core.CandleColor.White : Core.CandleColor.Black;
        }

        private static Core.CandleRangeType CandleRangeType(Core.CandleSettingType set)
        {
            return Core.CandleSettings.Get(set).RangeType;
        }

        private static int CandleAveragePeriod(Core.CandleSettingType set)
        {
            return Core.CandleSettings.Get(set).AveragePeriod;
        }

        private static double CandleFactor(Core.CandleSettingType set)
        {
            return Core.CandleSettings.Get(set).Factor;
        }

        private static T CandleRange(IReadOnlyList<T> open, IReadOnlyList<T> high, IReadOnlyList<T> low,
            IReadOnlyList<T> close, Core.CandleSettingType set, int idx) =>
            CandleRangeType(set) switch
            {
                Core.CandleRangeType.RealBody => RealBody(close, open, idx),
                Core.CandleRangeType.HighLow => HighLowRange(high, low, idx),
                Core.CandleRangeType.Shadows => UpperShadow(high, close, open, idx) + LowerShadow(close, open, low, idx),
                _ => T.Zero
            };

        private static T CandleAverage(IReadOnlyList<T> open, IReadOnlyList<T> high, IReadOnlyList<T> low,
            IReadOnlyList<T> close, Core.CandleSettingType set, T sum, int idx)
        {
            var candleAveragePeriod = T.CreateChecked(CandleAveragePeriod(set));
            var candleFactor = T.CreateChecked(CandleFactor(set));
            return candleFactor * (!T.IsZero(candleAveragePeriod)
                       ? sum / candleAveragePeriod
                       : CandleRange(open, high, low, close, set, idx)) /
                   (CandleRangeType(set) == Core.CandleRangeType.Shadows ? T.CreateChecked(2) : T.One);
        }

        private static bool RealBodyGapUp(IReadOnlyList<T> open, IReadOnlyList<T> close, int idx2, int idx1)
        {
            return T.Min(open[idx2], close[idx2]) > T.Max(open[idx1], close[idx1]);
        }

        private static bool RealBodyGapDown(IReadOnlyList<T> open, IReadOnlyList<T> close, int idx2, int idx1)
        {
            return T.Max(open[idx2], close[idx2]) < T.Min(open[idx1], close[idx1]);
        }

        private static bool CandleGapUp(IReadOnlyList<T> low, IReadOnlyList<T> high, int idx2, int idx1)
        {
            return low[idx2] > high[idx1];
        }

        private static bool CandleGapDown(IReadOnlyList<T> low, IReadOnlyList<T> high, int idx2, int idx1)
        {
            return high[idx2] < low[idx1];
        }
    }
}
