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

namespace TALib;

public static partial class Core
{
    /// <summary>
    /// Defines the different types of moving averages.
    /// </summary>
    public enum MAType
    {
        /// <summary>
        /// An unweighted, arithmetic mean.
        /// </summary>
        Sma,

        /// <summary>
        /// The standard exponential moving average, using a smoothing factor of 2/(n+1).
        /// </summary>
        Ema,

        /// <summary>
        /// An exponential moving average, using a smoothing factor of 1/n and simple moving average as seeding.
        /// </summary>
        Wma,

        /// <summary>
        /// The double exponential moving average.
        /// </summary>
        Dema,

        /// <summary>
        /// The triple exponential moving average.
        /// </summary>
        Tema,

        /// <summary>
        /// The triangular moving average.
        /// </summary>
        Trima,

        /// <summary>
        /// The Kaufman Adaptive Moving Average.
        /// </summary>
        Kama,

        /// <summary>
        /// The MESA Adaptive Moving Average.
        /// </summary>
        Mama,

        /// <summary>
        /// The triple generalized double exponential moving average.
        /// </summary>
        T3
    }
}
