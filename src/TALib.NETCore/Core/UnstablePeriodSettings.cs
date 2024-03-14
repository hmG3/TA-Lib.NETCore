using System.Collections.Generic;
using System.Linq;

namespace TALib;

public static partial class Core
{
    public static class UnstablePeriodSettings
    {
        private static readonly Dictionary<FuncUnstId, int> UnstablePeriods;

        static UnstablePeriodSettings()
        {
            UnstablePeriods = Enumerable.Range(0, (int) FuncUnstId.All).ToDictionary(i => (FuncUnstId) i, _ => default(int));
        }

        public static int Get(FuncUnstId id) => id >= FuncUnstId.All ? default : UnstablePeriods[id];

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
}
