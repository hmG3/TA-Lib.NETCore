/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2024 Anatolii Siryi
 *
 * This file is part of Technical Analysis Library for .NET.
 *
 * Technical Analysis Library for .NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Technical Analysis Library for .NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with Technical Analysis Library for .NET. If not, see <https://www.gnu.org/licenses/>.
 */

using System.Linq;

namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Provides settings for unstable periods for all functions.
    /// It allows configuring how many initial periods should be stripped off for functions with "memory",
    /// ensuring that their output stabilizes before being used.
    /// </summary>
    /// <remarks>
    /// Many TA functions, such as the Exponential Moving Average (EMA), utilize algorithms that retain memory of previous data points.
    /// This can lead to variations in the output during the initial periods of calculation, which may not be desirable for certain applications,
    /// especially in walk-forward testing.
    ///
    /// The <see cref="UnstablePeriodSettings"/> class allows setting and retrieving the unstable period for individual TA functions.
    /// The unstable period is the number of initial periods that are disregarded to ensure that the function's output is stable.
    ///
    /// By default, there is no unstable period set for any TA function. It is up to the consumer to determine the appropriate number of periods to strip off.
    /// The general guideline is to strip off as many periods as necessary to achieve consistent results.
    ///
    /// Setting an unstable period affects all instances where the function is used, including composite functions that use the specified function internally.
    /// For example, setting an unstable period for an EMA will also affect the EMA calculations within the MACD function.
    /// </remarks>
    public static class UnstablePeriodSettings
    {
        /// <remarks>
        /// Initializes the default unstable period settings for all functions.
        /// </remarks>
        private static readonly Dictionary<UnstableFunc, int> UnstablePeriods =
            Enumerable.Range(0, (int) UnstableFunc.All).ToDictionary(i => (UnstableFunc) i, _ => 0);

        /// <summary>
        /// Retrieves the unstable period for a specified function.
        /// </summary>
        /// <param name="id">The identifier of the function.</param>
        /// <returns>
        /// The number of periods to be stripped off.
        /// Returns zero if the identifier specified is <see cref="UnstableFunc.All"/>.
        /// </returns>
        public static int Get(UnstableFunc id) => id >= UnstableFunc.All ? 0 : UnstablePeriods[id];

        /// <summary>
        /// Sets the unstable period for a specified function.
        /// </summary>
        /// <remarks>
        /// To apply the change to all applicable functions, specify <see cref="UnstableFunc.All"/>.
        /// </remarks>
        /// <param name="id">The identifier of the function.</param>
        /// <param name="unstablePeriod">Number of periods to be stripped off.</param>
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
