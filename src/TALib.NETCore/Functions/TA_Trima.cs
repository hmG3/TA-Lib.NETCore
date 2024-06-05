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

public static partial class Functions
{
    public static Core.RetCode Trima<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outReal,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2)
        {
            return Core.RetCode.BadParam;
        }

        var lookbackTotal = TrimaLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int middleIdx;
        int trailingIdx;
        int todayIdx;
        int outIdx;
        if (optInTimePeriod % 2 == 1)
        {
            var i = optInTimePeriod >> 1;
            var ti = T.CreateChecked(i);
            T factor = (ti + T.One) * (ti + T.One);
            factor = T.One / factor;

            trailingIdx = startIdx - lookbackTotal;
            middleIdx = trailingIdx + i;
            todayIdx = middleIdx + i;
            T numerator = T.Zero;
            T numeratorSub = T.Zero;
            T tempReal;
            for (i = middleIdx; i >= trailingIdx; i--)
            {
                tempReal = inReal[i];
                numeratorSub += tempReal;
                numerator += numeratorSub;
            }

            T numeratorAdd = T.Zero;
            middleIdx++;
            for (i = middleIdx; i <= todayIdx; i++)
            {
                tempReal = inReal[i];
                numeratorAdd += tempReal;
                numerator += numeratorAdd;
            }

            outIdx = 0;
            tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = numerator * factor;
            todayIdx++;
            while (todayIdx <= endIdx)
            {
                numerator -= numeratorSub;
                numeratorSub -= tempReal;
                tempReal = inReal[middleIdx++];
                numeratorSub += tempReal;

                numerator += numeratorAdd;
                numeratorAdd -= tempReal;
                tempReal = inReal[todayIdx++];
                numeratorAdd += tempReal;

                numerator += tempReal;

                tempReal = inReal[trailingIdx++];
                outReal[outIdx++] = numerator * factor;
            }
        }
        else
        {
            var i = optInTimePeriod >> 1;
            var ti = T.CreateChecked(i);
            T factor = ti * (ti + T.One);
            factor = T.One / factor;

            trailingIdx = startIdx - lookbackTotal;
            middleIdx = trailingIdx + i - 1;
            todayIdx = middleIdx + i;
            T numerator = T.Zero;

            T numeratorSub = T.Zero;
            T tempReal;
            for (i = middleIdx; i >= trailingIdx; i--)
            {
                tempReal = inReal[i];
                numeratorSub += tempReal;
                numerator += numeratorSub;
            }

            T numeratorAdd = T.Zero;
            middleIdx++;
            for (i = middleIdx; i <= todayIdx; i++)
            {
                tempReal = inReal[i];
                numeratorAdd += tempReal;
                numerator += numeratorAdd;
            }

            outIdx = 0;
            tempReal = inReal[trailingIdx++];
            outReal[outIdx++] = numerator * factor;
            todayIdx++;

            while (todayIdx <= endIdx)
            {
                numerator -= numeratorSub;
                numeratorSub -= tempReal;
                tempReal = inReal[middleIdx++];
                numeratorSub += tempReal;

                numeratorAdd -= tempReal;
                numerator += numeratorAdd;
                tempReal = inReal[todayIdx++];
                numeratorAdd += tempReal;

                numerator += tempReal;

                tempReal = inReal[trailingIdx++];
                outReal[outIdx++] = numerator * factor;
            }
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TrimaLookback(int optInTimePeriod = 30) => optInTimePeriod < 2 ? -1 : optInTimePeriod - 1;

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Trima<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outReal,
        int optInTimePeriod = 30) where T : IFloatingPointIeee754<T> =>
        Trima<T>(inReal, startIdx, endIdx, outReal, out _, out _, optInTimePeriod);
}
