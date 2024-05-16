namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode StdDev(T[] inReal, int startIdx, int endIdx, T[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 5, double optInNbDev = 1.0)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null || outReal == null || optInTimePeriod is < 2 or > 100000)
        {
            return Core.RetCode.BadParam;
        }

        Core.RetCode retCode = TA_INT_VAR(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
        if (retCode != Core.RetCode.Success)
        {
            return retCode;
        }

        if (!optInNbDev.Equals(1.0))
        {
            for (var i = 0; i < outNbElement; i++)
            {
                T tempReal = outReal[i];
                outReal[i] = !TA_IsZeroOrNeg(tempReal) ? T.Sqrt(tempReal) * T.CreateChecked(optInNbDev) : T.Zero;
            }
        }
        else
        {
            for (var i = 0; i < outNbElement; i++)
            {
                T tempReal = outReal[i];
                outReal[i] = !TA_IsZeroOrNeg(tempReal) ? T.Sqrt(tempReal) : T.Zero;
            }
        }

        return Core.RetCode.Success;
    }

    public static int StdDevLookback(int optInTimePeriod = 5) => optInTimePeriod is < 2 or > 100000 ? -1 : VarLookback(optInTimePeriod);
}
