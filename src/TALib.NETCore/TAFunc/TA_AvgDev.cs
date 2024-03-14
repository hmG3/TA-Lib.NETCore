namespace TALib;

public static partial class Functions
{
    public static Core.RetCode AvgDev(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AvgDevLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        int today = startIdx;
        if (today > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = today;

        int outIdx = default;
        while (today <= endIdx)
        {
            double todaySum = default;
            for (var i = 0; i < optInTimePeriod; i++)
            {
                todaySum += inReal[today - i];
            }

            double todayDev = default;
            for (var i = 0; i < optInTimePeriod; i++)
            {
                todayDev += Math.Abs(inReal[today - i] - todaySum / optInTimePeriod);
            }

            outReal[outIdx++] = todayDev / optInTimePeriod;
            today++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode AvgDev(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
        int optInTimePeriod = 14)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AvgDevLookback(optInTimePeriod);
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        int today = startIdx;
        if (today > endIdx)
        {
            return Core.RetCode.Success;
        }

        outBegIdx = today;

        int outIdx = default;
        while (today <= endIdx)
        {
            decimal todaySum = default;
            for (var i = 0; i < optInTimePeriod; i++)
            {
                todaySum += inReal[today - i];
            }

            decimal todayDev = default;
            for (var i = 0; i < optInTimePeriod; i++)
            {
                todayDev += Math.Abs(inReal[today - i] - todaySum / optInTimePeriod);
            }

            outReal[outIdx++] = todayDev / optInTimePeriod;
            today++;
        }

        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AvgDevLookback(int optInTimePeriod = 14)
    {
        if (optInTimePeriod is < 2 or > 100000)
        {
            return -1;
        }

        return optInTimePeriod - 1;
    }
}
