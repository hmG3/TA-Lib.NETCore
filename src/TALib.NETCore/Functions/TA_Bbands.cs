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
    public static Core.RetCode Bbands<T>(
        ReadOnlySpan<T> inReal,
        int startIdx,
        int endIdx,
        Span<T> outRealUpperBand,
        Span<T> outRealMiddleBand,
        Span<T> outRealLowerBand,
        out int outBegIdx,
        out int outNbElement,
        int optInTimePeriod = 5,
        double optInNbDevUp = 2.0,
        double optInNbDevDn = 2.0,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T>
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx || endIdx >= inReal.Length)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (optInTimePeriod < 2 || optInNbDevUp < 0 || optInNbDevDn < 0)
        {
            return Core.RetCode.BadParam;
        }

        /* Identify two temporary buffers among the outputs.
         *
         * These temporary buffers allows to perform the calculation without any memory allocation.
         *
         * Whenever possible, make the tempBuffer1 be the middle band output. This will save one copy operation.
         */

        Span<T> tempBuffer1 = outRealMiddleBand;
        Span<T> tempBuffer2;

        if (inReal == outRealUpperBand)
        {
            tempBuffer2 = outRealLowerBand;
        }
        else
        {
            tempBuffer2 = outRealUpperBand;

            if (inReal == outRealMiddleBand)
            {
                tempBuffer1 = outRealLowerBand;
            }
        }

        // Check that the caller is not doing tricky things (like using the input buffer in two output)
        if (tempBuffer1 == inReal || tempBuffer2 == inReal)
        {
            return Core.RetCode.BadParam;
        }

        /* Calculate the middle band, which is a moving average. The other two bands will simply
         * add/subtract the standard deviation from this middle band.
         */
        var retCode = Ma(inReal, startIdx, endIdx, tempBuffer1, out outBegIdx, out outNbElement, optInTimePeriod, optInMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        if (optInMAType == Core.MAType.Sma)
        {
            // A small speed optimization by re-using the already calculated SMA.
            CalcStandardDeviation(inReal, tempBuffer1, outBegIdx, outNbElement, tempBuffer2, optInTimePeriod);
        }
        else
        {
            // Calculate the Standard Deviation
            retCode = StdDev(inReal, outBegIdx, endIdx, tempBuffer2, out outBegIdx, out outNbElement, optInTimePeriod);
            if (retCode != Core.RetCode.Success)
            {
                outNbElement = 0;

                return retCode;
            }
        }

        // Copy the MA calculation into the middle band output, unless the calculation was done into it already
        if (tempBuffer1 != outRealMiddleBand)
        {
            tempBuffer1[..outNbElement].CopyTo(outRealMiddleBand);
        }

        var nbDevUp = T.CreateChecked(optInNbDevUp);
        var nbDevDn = T.CreateChecked(optInNbDevDn);

        T tempReal;
        T tempReal2;

        /* Do a tight loop to calculate the upper/lower band at the same time.
         *
         * All the following 5 loops are doing the same,
         * except there is an attempt to speed optimize by eliminating unneeded multiplication.
         */
        if (optInNbDevUp.Equals(optInNbDevDn))
        {
            if (nbDevUp.Equals(T.One))
            {
                // No standard deviation multiplier needed.
                for (var i = 0; i < outNbElement; i++)
                {
                    tempReal = tempBuffer2[i];
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal;
                    outRealLowerBand[i] = tempReal2 - tempReal;
                }
            }
            else
            {
                // Upper/lower band use the same standard deviation multiplier.
                for (var i = 0; i < outNbElement; i++)
                {
                    tempReal = tempBuffer2[i] * nbDevUp;
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal;
                    outRealLowerBand[i] = tempReal2 - tempReal;
                }
            }
        }
        else if (nbDevUp.Equals(T.One))
        {
            // Only lower band has a standard deviation multiplier.
            for (var i = 0; i < outNbElement; i++)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealUpperBand[i] = tempReal2 + tempReal;
                outRealLowerBand[i] = tempReal2 - tempReal * nbDevDn;
            }
        }
        else if (nbDevDn.Equals(T.One))
        {
            // Only upper band has a standard deviation multiplier.
            for (var i = 0; i < outNbElement; i++)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealLowerBand[i] = tempReal2 - tempReal;
                outRealUpperBand[i] = tempReal2 + tempReal * nbDevUp;
            }
        }
        else
        {
            // Upper/lower band have distinctive standard deviation multiplier.
            for (var i = 0; i < outNbElement; i++)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealUpperBand[i] = tempReal2 + tempReal * nbDevUp;
                outRealLowerBand[i] = tempReal2 - tempReal * nbDevDn;
            }
        }

        return Core.RetCode.Success;
    }

    public static int BbandsLookback(int optInTimePeriod = 5, Core.MAType optInMAType = Core.MAType.Sma) =>
        optInTimePeriod < 2 ? -1 : MaLookback(optInTimePeriod, optInMAType);

    private static void CalcStandardDeviation<T>(
        ReadOnlySpan<T> inReal,
        ReadOnlySpan<T> inMovAvg,
        int inMovAvgBegIdx,
        int inMovAvgNbElement,
        Span<T> outReal,
        int optInTimePeriod) where T : IFloatingPointIeee754<T>
    {
        var startSum = inMovAvgBegIdx + 1 - optInTimePeriod;
        var endSum = inMovAvgBegIdx;
        var periodTotal2 = T.Zero;
        for (var outIdx = startSum; outIdx < endSum; outIdx++)
        {
            var tempReal = inReal[outIdx];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
        }

        var timePeriod = T.CreateChecked(optInTimePeriod);
        for (var outIdx = 0; outIdx < inMovAvgNbElement; outIdx++, startSum++, endSum++)
        {
            var tempReal = inReal[endSum];
            tempReal *= tempReal;
            periodTotal2 += tempReal;
            var meanValue2 = periodTotal2 / timePeriod;

            tempReal = inReal[startSum];
            tempReal *= tempReal;
            periodTotal2 -= tempReal;

            tempReal = inMovAvg[outIdx];
            tempReal *= tempReal;
            meanValue2 -= tempReal;

            outReal[outIdx] = meanValue2 > T.Zero ? T.Sqrt(meanValue2) : T.Zero;
        }
    }

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    [UsedImplicitly]
    private static Core.RetCode Bbands<T>(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outRealUpperBand,
        T[] outRealMiddleBand,
        T[] outRealLowerBand,
        int optInTimePeriod = 5,
        double optInNbDevUp = 2.0,
        double optInNbDevDn = 2.0,
        Core.MAType optInMAType = Core.MAType.Sma) where T : IFloatingPointIeee754<T> =>
        Bbands<T>(inReal, startIdx, endIdx, outRealUpperBand, outRealMiddleBand, outRealLowerBand, out _, out _,
            optInTimePeriod, optInNbDevUp, optInNbDevDn, optInMAType);
}
