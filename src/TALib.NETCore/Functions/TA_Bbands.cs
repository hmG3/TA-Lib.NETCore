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

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode Bbands(
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
        Core.MAType optInMAType = Core.MAType.Sma)
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

        Span<T> tempBuffer1;
        Span<T> tempBuffer2;
        if (inReal == outRealUpperBand)
        {
            tempBuffer1 = outRealMiddleBand.ToArray();
            tempBuffer2 = outRealLowerBand.ToArray();
        }
        else if (inReal == outRealLowerBand)
        {
            tempBuffer1 = outRealMiddleBand.ToArray();
            tempBuffer2 = outRealUpperBand.ToArray();
        }
        else if (inReal == outRealMiddleBand)
        {
            tempBuffer1 = outRealLowerBand.ToArray();
            tempBuffer2 = outRealUpperBand.ToArray();
        }
        else
        {
            tempBuffer1 = outRealMiddleBand.ToArray();
            tempBuffer2 = outRealUpperBand.ToArray();
        }

        if (tempBuffer1 == inReal || tempBuffer2 == inReal)
        {
            return Core.RetCode.BadParam;
        }

        var retCode = Ma(inReal, startIdx, endIdx, tempBuffer1, out outBegIdx, out outNbElement, optInTimePeriod, optInMAType);
        if (retCode != Core.RetCode.Success || outNbElement == 0)
        {
            return retCode;
        }

        if (optInMAType == Core.MAType.Sma)
        {
            CalcStandardDeviation(inReal, tempBuffer1, outBegIdx, outNbElement, tempBuffer2, optInTimePeriod);
        }
        else
        {
            retCode = StdDev(inReal, outBegIdx, endIdx, tempBuffer2, out outBegIdx, out outNbElement, optInTimePeriod);
            if (retCode != Core.RetCode.Success)
            {
                outNbElement = 0;

                return retCode;
            }
        }

        if (tempBuffer1 != outRealMiddleBand)
        {
            tempBuffer1.Slice(0, outNbElement).CopyTo(outRealMiddleBand);
        }

        T nbDevUp = T.CreateChecked(optInNbDevUp);
        T nbDevDn = T.CreateChecked(optInNbDevDn);

        T tempReal;
        T tempReal2;
        if (optInNbDevUp.Equals(optInNbDevDn))
        {
            if (nbDevUp.Equals(T.One))
            {
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

    /// <remarks>
    /// For compatibility with abstract API
    /// </remarks>
    private static Core.RetCode Bbands(
        T[] inReal,
        int startIdx,
        int endIdx,
        T[] outRealUpperBand,
        T[] outRealMiddleBand,
        T[] outRealLowerBand,
        int optInTimePeriod = 5,
        double optInNbDevUp = 2.0,
        double optInNbDevDn = 2.0,
        Core.MAType optInMAType = Core.MAType.Sma) =>
        Bbands(inReal, startIdx, endIdx, outRealUpperBand, outRealMiddleBand, outRealLowerBand, out _, out _,
            optInTimePeriod, optInNbDevUp, optInNbDevDn, optInMAType);
}
