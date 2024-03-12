namespace TALib
{
    public static partial class Core
    {
        public static RetCode Trima(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = TrimaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int middleIdx;
            int trailingIdx;
            int todayIdx;
            int outIdx;
            if (optInTimePeriod % 2 == 1)
            {
                int i = optInTimePeriod >> 1;
                double factor = (i + 1) * (i + 1);
                factor = 1.0 / factor;

                trailingIdx = startIdx - lookbackTotal;
                middleIdx = trailingIdx + i;
                todayIdx = middleIdx + i;
                double numerator = default;
                double numeratorSub = default;
                double tempReal;
                for (i = middleIdx; i >= trailingIdx; i--)
                {
                    tempReal = inReal[i];
                    numeratorSub += tempReal;
                    numerator += numeratorSub;
                }

                double numeratorAdd = default;
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
                int i = optInTimePeriod >> 1;
                double factor = i * (i + 1);
                factor = 1.0 / factor;

                trailingIdx = startIdx - lookbackTotal;
                middleIdx = trailingIdx + i - 1;
                todayIdx = middleIdx + i;
                double numerator = default;

                double numeratorSub = default;
                double tempReal;
                for (i = middleIdx; i >= trailingIdx; i--)
                {
                    tempReal = inReal[i];
                    numeratorSub += tempReal;
                    numerator += numeratorSub;
                }
                double numeratorAdd = default;
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

            return RetCode.Success;
        }

        public static RetCode Trima(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = TrimaLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            int middleIdx;
            int trailingIdx;
            int todayIdx;
            int outIdx;
            if (optInTimePeriod % 2 == 1)
            {
                int i = optInTimePeriod >> 1;
                decimal factor = (i + 1) * (i + 1);
                factor = Decimal.One / factor;

                trailingIdx = startIdx - lookbackTotal;
                middleIdx = trailingIdx + i;
                todayIdx = middleIdx + i;
                decimal numerator = default;
                decimal numeratorSub = default;
                decimal tempReal;
                for (i = middleIdx; i >= trailingIdx; i--)
                {
                    tempReal = inReal[i];
                    numeratorSub += tempReal;
                    numerator += numeratorSub;
                }

                decimal numeratorAdd = default;
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
                int i = optInTimePeriod >> 1;
                decimal factor = i * (i + 1);
                factor = Decimal.One / factor;

                trailingIdx = startIdx - lookbackTotal;
                middleIdx = trailingIdx + i - 1;
                todayIdx = middleIdx + i;
                decimal numerator = default;

                decimal numeratorSub = default;
                decimal tempReal;
                for (i = middleIdx; i >= trailingIdx; i--)
                {
                    tempReal = inReal[i];
                    numeratorSub += tempReal;
                    numerator += numeratorSub;
                }
                decimal numeratorAdd = default;
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

            return RetCode.Success;
        }

        public static int TrimaLookback(int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
