using System;

namespace TALib
{
    public static partial class Core
    {
        public static RetCode UltOsc(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outReal,
            out int outBegIdx, out int outNbElement, int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod1 < 1 ||
                optInTimePeriod1 > 100000 || optInTimePeriod2 < 1 || optInTimePeriod2 > 100000 || optInTimePeriod3 < 1 ||
                optInTimePeriod3 > 100000)
            {
                return RetCode.BadParam;
            }

            var usedFlag = new int[3];
            var periods = new[] { optInTimePeriod1, optInTimePeriod2, optInTimePeriod3 };
            var sortedPeriods = new int[3];

            for (var i = 0; i < 3; ++i)
            {
                int longestPeriod = default;
                int longestIndex = default;
                for (var j = 0; j < 3; j++)
                {
                    if (usedFlag[j] == 0 && periods[j] > longestPeriod)
                    {
                        longestPeriod = periods[j];
                        longestIndex = j;
                    }
                }

                usedFlag[longestIndex] = 1;
                sortedPeriods[i] = longestPeriod;
            }

            optInTimePeriod1 = sortedPeriods[2];
            optInTimePeriod2 = sortedPeriods[1];
            optInTimePeriod3 = sortedPeriods[0];

            int lookbackTotal = UltOscLookback(optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double trueRange;
            double closeMinusTrueLow;

            double a1Total = default;
            double b1Total = default;
            for (int i = startIdx - optInTimePeriod1 + 1; i < startIdx; ++i)
            {
                CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
                a1Total += closeMinusTrueLow;
                b1Total += trueRange;
            }

            double a2Total = default;
            double b2Total = default;
            for (int i = startIdx - optInTimePeriod2 + 1; i < startIdx; ++i)
            {
                CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
                a2Total += closeMinusTrueLow;
                b2Total += trueRange;
            }

            double a3Total = default;
            double b3Total = default;
            for (int i = startIdx - optInTimePeriod3 + 1; i < startIdx; ++i)
            {
                CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
                a3Total += closeMinusTrueLow;
                b3Total += trueRange;
            }

            int today = startIdx;
            int outIdx = default;
            int trailingIdx1 = today - optInTimePeriod1 + 1;
            int trailingIdx2 = today - optInTimePeriod2 + 1;
            int trailingIdx3 = today - optInTimePeriod3 + 1;
            while (today <= endIdx)
            {
                CalcTerms(inLow, inHigh, inClose, today, out trueRange, out closeMinusTrueLow);
                a1Total += closeMinusTrueLow;
                a2Total += closeMinusTrueLow;
                a3Total += closeMinusTrueLow;
                b1Total += trueRange;
                b2Total += trueRange;
                b3Total += trueRange;

                double output = default;

                if (!TA_IsZero(b1Total))
                {
                    output += 4.0 * (a1Total / b1Total);
                }

                if (!TA_IsZero(b2Total))
                {
                    output += 2.0 * (a2Total / b2Total);
                }

                if (!TA_IsZero(b3Total))
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

                outReal[outIdx++] = 100.0 * (output / 7.0);
                today++;
                trailingIdx1++;
                trailingIdx2++;
                trailingIdx3++;
            }

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static RetCode UltOsc(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outReal,
            out int outBegIdx, out int outNbElement, int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod1 < 1 ||
                optInTimePeriod1 > 100000 || optInTimePeriod2 < 1 || optInTimePeriod2 > 100000 || optInTimePeriod3 < 1 ||
                optInTimePeriod3 > 100000)
            {
                return RetCode.BadParam;
            }

            var usedFlag = new int[3];
            var periods = new[] { optInTimePeriod1, optInTimePeriod2, optInTimePeriod3 };
            var sortedPeriods = new int[3];

            for (var i = 0; i < 3; ++i)
            {
                int longestPeriod = default;
                int longestIndex = default;
                for (var j = 0; j < 3; j++)
                {
                    if (usedFlag[j] == 0 && periods[j] > longestPeriod)
                    {
                        longestPeriod = periods[j];
                        longestIndex = j;
                    }
                }

                usedFlag[longestIndex] = 1;
                sortedPeriods[i] = longestPeriod;
            }

            optInTimePeriod1 = sortedPeriods[2];
            optInTimePeriod2 = sortedPeriods[1];
            optInTimePeriod3 = sortedPeriods[0];

            int lookbackTotal = UltOscLookback(optInTimePeriod1, optInTimePeriod2, optInTimePeriod3);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal trueRange;
            decimal closeMinusTrueLow;

            decimal a1Total = default;
            decimal b1Total = default;
            for (int i = startIdx - optInTimePeriod1 + 1; i < startIdx; ++i)
            {
                CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
                a1Total += closeMinusTrueLow;
                b1Total += trueRange;
            }

            decimal a2Total = default;
            decimal b2Total = default;
            for (int i = startIdx - optInTimePeriod2 + 1; i < startIdx; ++i)
            {
                CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
                a2Total += closeMinusTrueLow;
                b2Total += trueRange;
            }

            decimal a3Total = default;
            decimal b3Total = default;
            for (int i = startIdx - optInTimePeriod3 + 1; i < startIdx; ++i)
            {
                CalcTerms(inLow, inHigh, inClose, i, out trueRange, out closeMinusTrueLow);
                a3Total += closeMinusTrueLow;
                b3Total += trueRange;
            }

            int today = startIdx;
            int outIdx = default;
            int trailingIdx1 = today - optInTimePeriod1 + 1;
            int trailingIdx2 = today - optInTimePeriod2 + 1;
            int trailingIdx3 = today - optInTimePeriod3 + 1;
            while (today <= endIdx)
            {
                CalcTerms(inLow, inHigh, inClose, today, out trueRange, out closeMinusTrueLow);
                a1Total += closeMinusTrueLow;
                a2Total += closeMinusTrueLow;
                a3Total += closeMinusTrueLow;
                b1Total += trueRange;
                b2Total += trueRange;
                b3Total += trueRange;

                decimal output = default;

                if (!TA_IsZero(b1Total))
                {
                    output += 4m * (a1Total / b1Total);
                }

                if (!TA_IsZero(b2Total))
                {
                    output += 2m * (a2Total / b2Total);
                }

                if (!TA_IsZero(b3Total))
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

                outReal[outIdx++] = 100m * (output / 7m);
                today++;
                trailingIdx1++;
                trailingIdx2++;
                trailingIdx3++;
            }

            outNbElement = outIdx;
            outBegIdx = startIdx;

            return RetCode.Success;
        }

        public static int UltOscLookback(int optInTimePeriod1 = 7, int optInTimePeriod2 = 14, int optInTimePeriod3 = 28)
        {
            if (optInTimePeriod1 < 1 || optInTimePeriod1 > 100000 || optInTimePeriod2 < 1 || optInTimePeriod2 > 100000 ||
                optInTimePeriod3 < 1 || optInTimePeriod3 > 100000)
            {
                return -1;
            }

            int maxPeriod = Math.Max(Math.Max(optInTimePeriod1, optInTimePeriod2), optInTimePeriod3);

            return SmaLookback(maxPeriod) + 1;
        }
    }
}
