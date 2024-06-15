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
    /// Provides settings for handling the compatibility with other software.
    /// </summary>
    public static class CompatibilitySettings
    {
        /// <remarks>
        /// Initializes the default compatibility mode.
        /// </remarks>
        private static CompatibilityMode _compatibilityMode = CompatibilityMode.Default;

        /// <summary>
        /// Retrieves the current compatibility mode.
        /// </summary>
        /// <returns>A <see cref="CompatibilityMode"/> enum value representing the current compatibility mode.</returns>
        public static CompatibilityMode Get() => _compatibilityMode;

        /// <summary>
        /// Sets a new compatibility mode.
        /// </summary>
        /// <param name="mode">The <see cref="CompatibilityMode"/> to be set.</param>
        public static void Set(CompatibilityMode mode) => _compatibilityMode = mode;
    }
}
