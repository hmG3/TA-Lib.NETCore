using System.Collections.Generic;
using System.Linq;

namespace TALib;

public static partial class Core
{
    public static class UnstablePeriodSettings
    {
        private static readonly Dictionary<UnstableFunc, int> UnstablePeriods;

        static UnstablePeriodSettings()
        {
            UnstablePeriods = Enumerable.Range(0, (int) UnstableFunc.All).ToDictionary(i => (UnstableFunc) i, _ => default(int));
        }

        public static int Get(UnstableFunc id) => id >= UnstableFunc.All ? default : UnstablePeriods[id];

        public static void Set(UnstableFunc id, int unstablePeriod)
        {
            if (id != UnstableFunc.All)
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
