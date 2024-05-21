namespace TALib;

public static partial class Candles<T> where T : IFloatingPointIeee754<T>
{
    public static Core.RetCode AdvanceBlock(T[] inOpen, T[] inHigh, T[] inLow, T[] inClose, int startIdx, int endIdx,
        int[] outInteger, out int outBegIdx, out int outNbElement)
    {
        outBegIdx = outNbElement = 0;

        if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
        {
            return Core.RetCode.OutOfRangeStartIndex;
        }

        if (inOpen == null || inHigh == null || inLow == null || inClose == null || outInteger == null)
        {
            return Core.RetCode.BadParam;
        }

        int lookbackTotal = AdvanceBlockLookback();
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        if (startIdx > endIdx)
        {
            return Core.RetCode.Success;
        }

        var shadowShortPeriodTotal = new T[3];
        int shadowShortTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowShort);
        var shadowLongPeriodTotal = new T[2];
        int shadowLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.ShadowLong);
        var nearPeriodTotal = new T[3];
        int nearTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Near);
        var farPeriodTotal = new T[3];
        int farTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.Far);
        T bodyLongPeriodTotal = T.Zero;
        int bodyLongTrailingIdx = startIdx - CandleAveragePeriod(Core.CandleSettingType.BodyLong);
        int i = shadowShortTrailingIdx;
        while (i < startIdx)
        {
            shadowShortPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - 2);
            shadowShortPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - 1);
            shadowShortPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            shadowLongPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - 1);
            shadowLongPeriodTotal[0] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            nearPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 2);
            nearPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - 1);
            i++;
        }

        i = farTrailingIdx;
        while (i < startIdx)
        {
            farPeriodTotal[2] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - 2);
            farPeriodTotal[1] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - 1);
            i++;
        }

        i = bodyLongTrailingIdx;
        while (i < startIdx)
        {
            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2);
            i++;
        }

        i = startIdx;

        int outIdx = default;
        do
        {
            if (CandleColor(inClose, inOpen, i - 2) == Core.CandleColor.White && // 1st white
                CandleColor(inClose, inOpen, i - 1) == Core.CandleColor.White && // 2nd white
                CandleColor(inClose, inOpen, i) == Core.CandleColor.White && // 3rd white
                inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] && // consecutive higher closes
                inOpen[i - 1] > inOpen[i - 2] && // 2nd opens within/near 1st real body
                inOpen[i - 1] <= inClose[i - 2] + CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near,
                    nearPeriodTotal[2], i - 2) &&
                inOpen[i] > inOpen[i - 1] && // 3rd opens within/near 2nd real body
                inOpen[i] <= inClose[i - 1] +
                CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, nearPeriodTotal[1], i - 1) &&
                RealBody(inClose, inOpen, i - 2) > CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong,
                    bodyLongPeriodTotal, i - 2) && // 1st: long real body
                UpperShadow(inHigh, inClose, inOpen, i - 2) < CandleAverage(inOpen, inHigh, inLow, inClose,
                    Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[2], i - 2) &&
                // 1st: short upper shadow
                // ( 2 far smaller than 1 && 3 not longer than 2 )
                // advance blocked with the 2nd, 3rd must not carry on the advance
                (RealBody(inClose, inOpen, i - 1) < RealBody(inClose, inOpen, i - 2) -
                 CandleAverage(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, farPeriodTotal[2], i - 2) &&
                 RealBody(inClose, inOpen, i) < RealBody(inClose, inOpen, i - 1) + CandleAverage(inOpen, inHigh, inLow,
                     inClose, Core.CandleSettingType.Near, nearPeriodTotal[1], i - 1)
                 ||
                 // 3 far smaller than 2
                 // advance blocked with the 3rd
                 RealBody(inClose, inOpen, i) < RealBody(inClose, inOpen, i - 1) - CandleAverage(inOpen, inHigh, inLow,
                     inClose, Core.CandleSettingType.Far, farPeriodTotal[1], i - 1)
                 ||
                 // ( 3 smaller than 2 && 2 smaller than 1 && (3 or 2 not short upper shadow) )
                 // advance blocked with progressively smaller real bodies and some upper shadows
                 RealBody(inClose, inOpen, i) < RealBody(inClose, inOpen, i - 1) &&
                 RealBody(inClose, inOpen, i - 1) < RealBody(inClose, inOpen, i - 2) &&
                 (
                     UpperShadow(inHigh, inClose, inOpen, i) > CandleAverage(inOpen, inHigh, inLow, inClose,
                         Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[0], i) ||
                     UpperShadow(inHigh, inClose, inOpen, i - 1) > CandleAverage(inOpen, inHigh, inLow, inClose,
                         Core.CandleSettingType.ShadowShort, shadowShortPeriodTotal[1], i - 1)
                 ) ||
                 // ( 3 smaller than 2 && 3 long upper shadow )
                 // advance blocked with 3rd candle's long upper shadow and smaller body
                 RealBody(inClose, inOpen, i) < RealBody(inClose, inOpen, i - 1) &&
                 UpperShadow(inHigh, inClose, inOpen, i) > CandleAverage(inOpen, inHigh, inLow, inClose,
                     Core.CandleSettingType.ShadowLong, shadowLongPeriodTotal[0], i)))
            {
                outInteger[outIdx++] = -100;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            for (var totIdx = 2; totIdx >= 0; --totIdx)
            {
                shadowShortPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowShort, shadowShortTrailingIdx - totIdx);
            }

            for (var totIdx = 1; totIdx >= 0; --totIdx)
            {
                shadowLongPeriodTotal[totIdx] +=
                    CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, i - totIdx)
                    - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.ShadowLong, shadowLongTrailingIdx - totIdx);
            }

            for (var totIdx = 2; totIdx >= 1; --totIdx)
            {
                farPeriodTotal[totIdx] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far, i - totIdx)
                                          - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Far,
                                              farTrailingIdx - totIdx);
                nearPeriodTotal[totIdx] += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near, i - totIdx)
                                           - CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.Near,
                                               nearTrailingIdx - totIdx);
            }

            bodyLongPeriodTotal += CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, i - 2) -
                                   CandleRange(inOpen, inHigh, inLow, inClose, Core.CandleSettingType.BodyLong, bodyLongTrailingIdx - 2);
            i++;
            shadowShortTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyLongTrailingIdx++;
        } while (i <= endIdx);

        outBegIdx = startIdx;
        outNbElement = outIdx;

        return Core.RetCode.Success;
    }

    public static int AdvanceBlockLookback() =>
        Math.Max(
            Math.Max(
                Math.Max(CandleAveragePeriod(Core.CandleSettingType.ShadowLong), CandleAveragePeriod(Core.CandleSettingType.ShadowShort)),
                Math.Max(CandleAveragePeriod(Core.CandleSettingType.Far), CandleAveragePeriod(Core.CandleSettingType.Near))),
            CandleAveragePeriod(Core.CandleSettingType.BodyLong)
        ) + 2;
}
