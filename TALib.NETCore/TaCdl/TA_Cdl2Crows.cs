using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl2Crows(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
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

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = Cdl2CrowsLookback();
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
            int bodyLongTrailingIdx = startIdx - 2 - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num24;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num24 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num23;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num23 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num20;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num21;
                            double num22;
                            if (inClose[i] >= inOpen[i])
                            {
                                num22 = inClose[i];
                            }
                            else
                            {
                                num22 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num21 = inOpen[i];
                            }
                            else
                            {
                                num21 = inClose[i];
                            }

                            num20 = inHigh[i] - num22 + (num21 - inLow[i]);
                        }
                        else
                        {
                            num20 = 0.0;
                        }

                        num23 = num20;
                    }

                    num24 = num23;
                }

                bodyLongPeriodTotal += num24;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_014D:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num19 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num18;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num18 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num17;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num17 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num14;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                double num15;
                                double num16;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num16 = inClose[i - 2];
                                }
                                else
                                {
                                    num16 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num15 = inOpen[i - 2];
                                }
                                else
                                {
                                    num15 = inClose[i - 2];
                                }

                                num14 = inHigh[i - 2] - num16 + (num15 - inLow[i - 2]);
                            }
                            else
                            {
                                num14 = 0.0;
                            }

                            num17 = num14;
                        }

                        num18 = num17;
                    }

                    num19 = num18;
                }

                var num13 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num19 / num13 &&
                    inClose[i - 1] < inOpen[i - 1])
                {
                    double num11;
                    double num12;
                    if (inOpen[i - 1] < inClose[i - 1])
                    {
                        num12 = inOpen[i - 1];
                    }
                    else
                    {
                        num12 = inClose[i - 1];
                    }

                    if (inOpen[i - 2] > inClose[i - 2])
                    {
                        num11 = inOpen[i - 2];
                    }
                    else
                    {
                        num11 = inClose[i - 2];
                    }

                    if (num12 > num11 && inClose[i] < inOpen[i] && inOpen[i] < inOpen[i - 1] &&
                        inOpen[i] > inClose[i - 1] && inClose[i] > inOpen[i - 2] && inClose[i] < inClose[i - 2])
                    {
                        outInteger[outIdx] = -100;
                        outIdx++;
                        goto Label_036E;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_036E:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num8 = inClose[i - 2];
                        }
                        else
                        {
                            num8 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num7 = inOpen[i - 2];
                        }
                        else
                        {
                            num7 = inClose[i - 2];
                        }

                        num6 = inHigh[i - 2] - num8 + (num7 - inLow[i - 2]);
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
                num5 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num3 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num2 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx];
                        }

                        num = inHigh[bodyLongTrailingIdx] - num3 + (num2 - inLow[bodyLongTrailingIdx]);
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
                goto Label_014D;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl2Crows(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
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

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = Cdl2CrowsLookback();
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
            int bodyLongTrailingIdx = startIdx - 2 - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num24;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num24 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num23;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num23 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num20;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num21;
                            decimal num22;
                            if (inClose[i] >= inOpen[i])
                            {
                                num22 = inClose[i];
                            }
                            else
                            {
                                num22 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num21 = inOpen[i];
                            }
                            else
                            {
                                num21 = inClose[i];
                            }

                            num20 = inHigh[i] - num22 + (num21 - inLow[i]);
                        }
                        else
                        {
                            num20 = Decimal.Zero;
                        }

                        num23 = num20;
                    }

                    num24 = num23;
                }

                bodyLongPeriodTotal += num24;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_015B:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num19 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num18;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num18 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num17;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num17 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num14;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                decimal num15;
                                decimal num16;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num16 = inClose[i - 2];
                                }
                                else
                                {
                                    num16 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num15 = inOpen[i - 2];
                                }
                                else
                                {
                                    num15 = inClose[i - 2];
                                }

                                num14 = inHigh[i - 2] - num16 + (num15 - inLow[i - 2]);
                            }
                            else
                            {
                                num14 = Decimal.Zero;
                            }

                            num17 = num14;
                        }

                        num18 = num17;
                    }

                    num19 = num18;
                }

                var num13 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num19 / num13
                    && inClose[i - 1] < inOpen[i - 1])
                {
                    decimal num11;
                    decimal num12;
                    if (inOpen[i - 1] < inClose[i - 1])
                    {
                        num12 = inOpen[i - 1];
                    }
                    else
                    {
                        num12 = inClose[i - 1];
                    }

                    if (inOpen[i - 2] > inClose[i - 2])
                    {
                        num11 = inOpen[i - 2];
                    }
                    else
                    {
                        num11 = inClose[i - 2];
                    }

                    if (num12 > num11 && inClose[i] < inOpen[i] && inOpen[i] < inOpen[i - 1] &&
                        inOpen[i] > inClose[i - 1] && inClose[i] > inOpen[i - 2] && inClose[i] < inClose[i - 2])
                    {
                        outInteger[outIdx] = -100;
                        outIdx++;
                        goto Label_03A6;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_03A6:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num8 = inClose[i - 2];
                        }
                        else
                        {
                            num8 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num7 = inOpen[i - 2];
                        }
                        else
                        {
                            num7 = inClose[i - 2];
                        }

                        num6 = inHigh[i - 2] - num8 + (num7 - inLow[i - 2]);
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
                num5 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num3 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num2 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx];
                        }

                        num = inHigh[bodyLongTrailingIdx] - num3 + (num2 - inLow[bodyLongTrailingIdx]);
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
                goto Label_015B;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl2CrowsLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod + 2;
        }
    }
}
