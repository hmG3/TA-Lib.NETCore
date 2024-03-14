namespace TALib;

public static partial class Functions
{
    public static Core.RetCode TRange(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outReal,
        out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = TRangeLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        int today = startIdx;
        while (today <= endIdx)
        {
            double tempLT = inLow[today];
            double tempHT = inHigh[today];
            double tempCY = inClose[today - 1];
            double greatest = tempHT - tempLT;

            double val2 = Math.Abs(tempCY - tempHT);
            if (val2 > greatest)
            {
                greatest = val2;
            }

            double val3 = Math.Abs(tempCY - tempLT);
            if (val3 > greatest)
            {
                greatest = val3;
            }

            outReal[outIdx++] = greatest;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static Core.RetCode TRange(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outReal,
        out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inHigh == null || inLow == null || inClose == null || outReal == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = TRangeLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        int outIdx = default;
        int today = startIdx;
        while (today <= endIdx)
        {
            decimal tempLT = inLow[today];
            decimal tempHT = inHigh[today];
            decimal tempCY = inClose[today - 1];
            decimal greatest = tempHT - tempLT;

            decimal val2 = Math.Abs(tempCY - tempHT);
            if (val2 > greatest)
            {
                greatest = val2;
            }

            decimal val3 = Math.Abs(tempCY - tempLT);
            if (val3 > greatest)
            {
                greatest = val3;
            }

            outReal[outIdx++] = greatest;
            today++;
        }

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int TRangeLookback() => 1;
}
