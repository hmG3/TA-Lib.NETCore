namespace TALib;

public static partial class Functions<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode UltOsc(T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx, T[] outReal,
        out int outBegIdx, out int outNbElement, int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null ||
            optInTimePeriod1 < 1 || optInTimePeriod2 < 1 || optInTimePeriod3 < 1)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = UltOscLookback(optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var usedFlag = new bool[3];
        var periods = new[] { optInTimePeriod1, optInTimePeriod2, optInTimePeriod3 };
        var sortedPeriods = new int[3];

        for (var i = 0; i < 3; ++i)
        {
            int longestPeriod = default;
            int longestIndex = default;
            for (var j = 0; j < 3; j++)
            {
                if (!usedFlag[j] && periods[j] > longestPeriod)
                {
                    longestPeriod = periods[j];
                    longestIndex = j;
                }
            }

            usedFlag[longestIndex] = true;
            sortedPeriods[i] = longestPeriod;
        }

        optInTimePeriod1 = sortedPeriods[2];
        optInTimePeriod2 = sortedPeriods[1];
        optInTimePeriod3 = sortedPeriods[0];

        T trueRange;
        T closeMinusTrueLow;

        T a1Total = T.Zero;
        T b1Total = T.Zero;
        for (var i = startIdx - optInTimePeriod1 + 1; i < startIdx; ++i)
        {
            CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
            a1Total += closeMinusTrueLow;
            b1Total += trueRange;
        }

        T a2Total = T.Zero;
        T b2Total = T.Zero;
        for (var i = startIdx - optInTimePeriod2 + 1; i < startIdx; ++i)
        {
            CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
            a2Total += closeMinusTrueLow;
            b2Total += trueRange;
        }

        T a3Total = T.Zero;
        T b3Total = T.Zero;
        for (var i = startIdx - optInTimePeriod3 + 1; i < startIdx; ++i)
        {
            CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
            a3Total += closeMinusTrueLow;
            b3Total += trueRange;
        }

        T TSeven = T.CreateChecked(7);
        var today = startIdx;
        int outIdx = default;
        var trailingIdx1 = today - optInTimePeriod1 + 1;
        var trailingIdx2 = today - optInTimePeriod2 + 1;
        var trailingIdx3 = today - optInTimePeriod3 + 1;
        while (today <= endIdx)
        {
            CalcTerms(inLow, inHigh, inClose, today, out trueRange, out closeMinusTrueLow);
            a1Total += closeMinusTrueLow;
            a2Total += closeMinusTrueLow;
            a3Total += closeMinusTrueLow;
            b1Total += trueRange;
            b2Total += trueRange;
            b3Total += trueRange;

            T output = T.Zero;

            if (!T.IsZero(b1Total))
            {
                output += TFour * (a1Total / b1Total);
            }

            if (!T.IsZero(b2Total))
            {
                output += TTwo * (a2Total / b2Total);
            }

            if (!T.IsZero(b3Total))
            {
                output += a3Total / b3Total;
            }

            CalcTerms(inLow, inHigh, inClose, trailingIdx1, out trueRange, out closeMinusTrueLow);
            a1Total -= closeMinusTrueLow;
            b1Total -= trueRange;

            CalcTerms(inLow, inHigh, inClose, trailingIdx2, out trueRange, out closeMinusTrueLow);
            a2Total -= closeMinusTrueLow;
            b2Total -= trueRange;

            CalcTerms(inLow, inHigh, inClose, trailingIdx3, out trueRange, out closeMinusTrueLow);
            a3Total -= closeMinusTrueLow;
            b3Total -= trueRange;

            outReal[outIdx++] = THundred * (output / TSeven);
            today++;
            trailingIdx1++;
            trailingIdx2++;
            trailingIdx3++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int UltOscLookback(int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28) =>
        optInTimePeriod1 < 1 || optInTimePeriod2 < 1 || optInTimePeriod3 < 1
            ? -1
            : SmaLookback(Math.Max(Math.Max(optInTimePeriod1, optInTimePeriod2), optInTimePeriod3)) + 1;
}
