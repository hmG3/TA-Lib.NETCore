using System.Collections.Generic;
using System.Linq;

namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Candle settings for all candlestick patterns
    /// </summary>
    public static class UnstablePeriodSettings
    {
        private static readonly Dictionary<FuncUnstId, int> UnstablePeriods;

        static UnstablePeriodSettings()
        {
            UnstablePeriods = Enumerable.Range(0, (int) FuncUnstId.All).ToDictionary(i => (FuncUnstId) i, _ => 0);
        }

        public static int Get(FuncUnstId id) => id >= FuncUnstId.All ? 0 : UnstablePeriods[id];

        public static void Set(FuncUnstId id, int unstablePeriod)
        {
            if (id != FuncUnstId.All)
            {
                UnstablePeriods[id] = unstablePeriod;
            }
            else
            {
                foreach (var key in UnstablePeriods.Keys)
                {
                    UnstablePeriods[key] = unstablePeriod;
                }
            }
        }
    }

    public enum FuncUnstId
    {
        Adx,
        Adxr,
        Atr,
        Cmo,
        Dx,
        Ema,
        HtDcPeriod,
        HtDcPhase,
        HtPhasor,
        HtSine,
        HtTrendline,
        HtTrendMode,
        Kama,
        Mama,
        Mfi,
        MinusDI,
        MinusDM,
        Natr,
        PlusDI,
        PlusDM,
        Rsi,
        StochRsi,
        T3,
        All
    }
}
