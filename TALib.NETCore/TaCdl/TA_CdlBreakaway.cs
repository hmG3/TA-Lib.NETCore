using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlBreakaway(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            int num11;
            double num12;
            double num13;
            int num16;
            int num17;
            int num18;
            double num25;
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

            int lookbackTotal = CdlBreakawayLookback();
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
                double num30;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    double num29;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num27;
                            double num28;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num28 = inClose[i - 4];
                            }
                            else
                            {
                                num28 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num27 = inOpen[i - 4];
                            }
                            else
                            {
                                num27 = inClose[i - 4];
                            }

                            num26 = inHigh[i - 4] - num28 + (num27 - inLow[i - 4]);
                        }
                        else
                        {
                            num26 = 0.0;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                bodyLongPeriodTotal += num30;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0168:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num25 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num24 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    double num23;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num23 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num20;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num21;
                            double num22;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num22 = inClose[i - 4];
                            }
                            else
                            {
                                num22 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num21 = inOpen[i - 4];
                            }
                            else
                            {
                                num21 = inClose[i - 4];
                            }

                            num20 = inHigh[i - 4] - num22 + (num21 - inLow[i - 4]);
                        }
                        else
                        {
                            num20 = 0.0;
                        }

                        num23 = num20;
                    }

                    num24 = num23;
                }

                num25 = num24;
            }

            var num19 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 4] - inOpen[i - 4]) <=
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num25 / num19)
            {
                goto Label_04B7;
            }

            if (inClose[i - 3] >= inOpen[i - 3])
            {
                num18 = 1;
            }
            else
            {
                num18 = -1;
            }

            if ((inClose[i - 4] < inOpen[i - 4] ? -1 : 1) != num18)
            {
                goto Label_04B7;
            }

            if (inClose[i - 1] >= inOpen[i - 1])
            {
                num17 = 1;
            }
            else
            {
                num17 = -1;
            }

            if ((inClose[i - 3] < inOpen[i - 3] ? -1 : 1) != num17)
            {
                goto Label_04B7;
            }

            if (inClose[i] >= inOpen[i])
            {
                num16 = 1;
            }
            else
            {
                num16 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) != -num16)
            {
                goto Label_04B7;
            }

            if (inClose[i - 4] < inOpen[i - 4])
            {
                double num14;
                double num15;
                if (inOpen[i - 3] > inClose[i - 3])
                {
                    num15 = inOpen[i - 3];
                }
                else
                {
                    num15 = inClose[i - 3];
                }

                if (inOpen[i - 4] < inClose[i - 4])
                {
                    num14 = inOpen[i - 4];
                }
                else
                {
                    num14 = inClose[i - 4];
                }

                if (num15 < num14 && inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] && inHigh[i - 1] < inHigh[i - 2] &&
                    inLow[i - 1] < inLow[i - 2] && inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4])
                {
                    goto Label_0497;
                }
            }

            if (inClose[i - 4] < inOpen[i - 4])
            {
                goto Label_04B7;
            }

            if (inOpen[i - 3] < inClose[i - 3])
            {
                num13 = inOpen[i - 3];
            }
            else
            {
                num13 = inClose[i - 3];
            }

            if (inOpen[i - 4] > inClose[i - 4])
            {
                num12 = inOpen[i - 4];
            }
            else
            {
                num12 = inClose[i - 4];
            }

            if (num13 <= num12 || inHigh[i - 2] <= inHigh[i - 3] || inLow[i - 2] <= inLow[i - 3] || inHigh[i - 1] <= inHigh[i - 2] ||
                inLow[i - 1] <= inLow[i - 2] || inClose[i] >= inOpen[i - 3] || inClose[i] <= inClose[i - 4])
            {
                goto Label_04B7;
            }

            Label_0497:
            if (inClose[i] >= inOpen[i])
            {
                num11 = 1;
            }
            else
            {
                num11 = -1;
            }

            outInteger[outIdx] = num11 * 100;
            outIdx++;
            goto Label_04C0;
            Label_04B7:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_04C0:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num8 = inClose[i - 4];
                        }
                        else
                        {
                            num8 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num7 = inOpen[i - 4];
                        }
                        else
                        {
                            num7 = inClose[i - 4];
                        }

                        num6 = inHigh[i - 4] - num8 + (num7 - inLow[i - 4]);
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
                num5 = Math.Abs(inClose[bodyLongTrailingIdx - 4] - inOpen[bodyLongTrailingIdx - 4]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx - 4] - inLow[bodyLongTrailingIdx - 4];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num3 = inClose[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx - 4];
                        }

                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num2 = inOpen[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx - 4];
                        }

                        num = inHigh[bodyLongTrailingIdx - 4] - num3 + (num2 - inLow[bodyLongTrailingIdx - 4]);
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
                goto Label_0168;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlBreakaway(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            int num11;
            decimal num12;
            decimal num13;
            int num16;
            int num17;
            int num18;
            decimal num25;
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

            int lookbackTotal = CdlBreakawayLookback();
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
                decimal num30;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    decimal num29;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        decimal num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num27;
                            decimal num28;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num28 = inClose[i - 4];
                            }
                            else
                            {
                                num28 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num27 = inOpen[i - 4];
                            }
                            else
                            {
                                num27 = inClose[i - 4];
                            }

                            num26 = inHigh[i - 4] - num28 + (num27 - inLow[i - 4]);
                        }
                        else
                        {
                            num26 = Decimal.Zero;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                bodyLongPeriodTotal += num30;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0176:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num25 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num24 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    decimal num23;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num23 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        decimal num20;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num21;
                            decimal num22;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num22 = inClose[i - 4];
                            }
                            else
                            {
                                num22 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num21 = inOpen[i - 4];
                            }
                            else
                            {
                                num21 = inClose[i - 4];
                            }

                            num20 = inHigh[i - 4] - num22 + (num21 - inLow[i - 4]);
                        }
                        else
                        {
                            num20 = Decimal.Zero;
                        }

                        num23 = num20;
                    }

                    num24 = num23;
                }

                num25 = num24;
            }

            var num19 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 4] - inOpen[i - 4]) <=
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num25 / num19)
            {
                goto Label_0518;
            }

            if (inClose[i - 3] >= inOpen[i - 3])
            {
                num18 = 1;
            }
            else
            {
                num18 = -1;
            }

            if ((inClose[i - 4] < inOpen[i - 4] ? -1 : 1) != num18)
            {
                goto Label_0518;
            }

            if (inClose[i - 1] >= inOpen[i - 1])
            {
                num17 = 1;
            }
            else
            {
                num17 = -1;
            }

            if ((inClose[i - 3] < inOpen[i - 3] ? -1 : 1) != num17)
            {
                goto Label_0518;
            }

            if (inClose[i] >= inOpen[i])
            {
                num16 = 1;
            }
            else
            {
                num16 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) != -num16)
            {
                goto Label_0518;
            }

            if (inClose[i - 4] < inOpen[i - 4])
            {
                decimal num14;
                decimal num15;
                if (inOpen[i - 3] > inClose[i - 3])
                {
                    num15 = inOpen[i - 3];
                }
                else
                {
                    num15 = inClose[i - 3];
                }

                if (inOpen[i - 4] < inClose[i - 4])
                {
                    num14 = inOpen[i - 4];
                }
                else
                {
                    num14 = inClose[i - 4];
                }

                if (num15 < num14 && inHigh[i - 2] < inHigh[i - 3] && inLow[i - 2] < inLow[i - 3] && inHigh[i - 1] < inHigh[i - 2] &&
                    inLow[i - 1] < inLow[i - 2] && inClose[i] > inOpen[i - 3] && inClose[i] < inClose[i - 4])
                {
                    goto Label_04F6;
                }
            }

            if (inClose[i - 4] < inOpen[i - 4])
            {
                goto Label_0518;
            }

            if (inOpen[i - 3] < inClose[i - 3])
            {
                num13 = inOpen[i - 3];
            }
            else
            {
                num13 = inClose[i - 3];
            }

            if (inOpen[i - 4] > inClose[i - 4])
            {
                num12 = inOpen[i - 4];
            }
            else
            {
                num12 = inClose[i - 4];
            }

            if (num13 <= num12 || inHigh[i - 2] <= inHigh[i - 3] || inLow[i - 2] <= inLow[i - 3] || inHigh[i - 1] <= inHigh[i - 2] ||
                inLow[i - 1] <= inLow[i - 2] || inClose[i] >= inOpen[i - 3] || inClose[i] <= inClose[i - 4])
            {
                goto Label_0518;
            }

            Label_04F6:
            if (inClose[i] >= inOpen[i])
            {
                num11 = 1;
            }
            else
            {
                num11 = -1;
            }

            outInteger[outIdx] = num11 * 100;
            outIdx++;
            goto Label_0521;
            Label_0518:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0521:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num8 = inClose[i - 4];
                        }
                        else
                        {
                            num8 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num7 = inOpen[i - 4];
                        }
                        else
                        {
                            num7 = inClose[i - 4];
                        }

                        num6 = inHigh[i - 4] - num8 + (num7 - inLow[i - 4]);
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
                num5 = Math.Abs(inClose[bodyLongTrailingIdx - 4] - inOpen[bodyLongTrailingIdx - 4]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx - 4] - inLow[bodyLongTrailingIdx - 4];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num3 = inClose[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx - 4];
                        }

                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num2 = inOpen[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx - 4];
                        }

                        num = inHigh[bodyLongTrailingIdx - 4] - num3 + (num2 - inLow[bodyLongTrailingIdx - 4]);
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
                goto Label_0176;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlBreakawayLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod + 4;
        }
    }
}
