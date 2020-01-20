using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlDarkCloudCover(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger, double optInPenetration = 0.5)
        {
            double num5;
            double num10;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (optInPenetration < 0.0)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlDarkCloudCoverLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num22;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num22 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num21 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num18;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num19;
                            double num20;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num20 = inClose[i - 1];
                            }
                            else
                            {
                                num20 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num19 = inOpen[i - 1];
                            }
                            else
                            {
                                num19 = inClose[i - 1];
                            }

                            num18 = inHigh[i - 1] - num20 + (num19 - inLow[i - 1]);
                        }
                        else
                        {
                            num18 = 0.0;
                        }

                        num21 = num18;
                    }

                    num22 = num21;
                }

                bodyLongPeriodTotal += num22;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_01A3:
            if (inClose[i - 1] >= inOpen[i - 1])
            {
                double num17;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num17 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num16 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num15;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num15 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num12;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                double num13;
                                double num14;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num14 = inClose[i - 1];
                                }
                                else
                                {
                                    num14 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num13 = inOpen[i - 1];
                                }
                                else
                                {
                                    num13 = inClose[i - 1];
                                }

                                num12 = inHigh[i - 1] - num14 + (num13 - inLow[i - 1]);
                            }
                            else
                            {
                                num12 = 0.0;
                            }

                            num15 = num12;
                        }

                        num16 = num15;
                    }

                    num17 = num16;
                }

                var num11 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num17 / num11 && inClose[i] < inOpen[i] &&
                    inOpen[i] > inHigh[i - 1] && inClose[i] > inOpen[i - 1] &&
                    inClose[i] < inClose[i - 1] - Math.Abs(inClose[i - 1] - inOpen[i - 1]) * optInPenetration)
                {
                    outInteger[outIdx] = -100;
                    outIdx++;
                    goto Label_0373;
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0373:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num8 = inClose[i - 1];
                        }
                        else
                        {
                            num8 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num7 = inOpen[i - 1];
                        }
                        else
                        {
                            num7 = inClose[i - 1];
                        }

                        num6 = inHigh[i - 1] - num8 + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyLongTrailingIdx - 1] - inOpen[bodyLongTrailingIdx - 1]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx - 1] - inLow[bodyLongTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num3 = inClose[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx - 1];
                        }

                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num2 = inOpen[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx - 1];
                        }

                        num = inHigh[bodyLongTrailingIdx - 1] - num3 + (num2 - inLow[bodyLongTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyLongPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_01A3;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlDarkCloudCover(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger, decimal optInPenetration = 0.5m)
        {
            decimal num5;
            decimal num10;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (optInPenetration < Decimal.Zero)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlDarkCloudCoverLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            decimal bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num22;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num22 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num21 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num18;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num19;
                            decimal num20;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num20 = inClose[i - 1];
                            }
                            else
                            {
                                num20 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num19 = inOpen[i - 1];
                            }
                            else
                            {
                                num19 = inClose[i - 1];
                            }

                            num18 = inHigh[i - 1] - num20 + (num19 - inLow[i - 1]);
                        }
                        else
                        {
                            num18 = Decimal.Zero;
                        }

                        num21 = num18;
                    }

                    num22 = num21;
                }

                bodyLongPeriodTotal += num22;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_01B1:
            if (inClose[i - 1] >= inOpen[i - 1])
            {
                decimal num17;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num17 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num16 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num15;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num15 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num12;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                decimal num13;
                                decimal num14;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num14 = inClose[i - 1];
                                }
                                else
                                {
                                    num14 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num13 = inOpen[i - 1];
                                }
                                else
                                {
                                    num13 = inClose[i - 1];
                                }

                                num12 = inHigh[i - 1] - num14 + (num13 - inLow[i - 1]);
                            }
                            else
                            {
                                num12 = Decimal.Zero;
                            }

                            num15 = num12;
                        }

                        num16 = num15;
                    }

                    num17 = num16;
                }

                var num11 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num17 / num11 &&
                    inClose[i] < inOpen[i] && inOpen[i] > inHigh[i - 1] && inClose[i] > inOpen[i - 1] &&
                    inClose[i] < inClose[i - 1] - Math.Abs(inClose[i - 1] - inOpen[i - 1]) * optInPenetration)
                {
                    outInteger[outIdx] = -100;
                    outIdx++;
                    goto Label_03A1;
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_03A1:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num8 = inClose[i - 1];
                        }
                        else
                        {
                            num8 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num7 = inOpen[i - 1];
                        }
                        else
                        {
                            num7 = inClose[i - 1];
                        }

                        num6 = inHigh[i - 1] - num8 + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = Decimal.Zero;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyLongTrailingIdx - 1] - inOpen[bodyLongTrailingIdx - 1]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx - 1] - inLow[bodyLongTrailingIdx - 1];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num3 = inClose[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx - 1];
                        }

                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num2 = inOpen[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx - 1];
                        }

                        num = inHigh[bodyLongTrailingIdx - 1] - num3 + (num2 - inLow[bodyLongTrailingIdx - 1]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyLongPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_01B1;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlDarkCloudCoverLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod + 1;
        }
    }
}
