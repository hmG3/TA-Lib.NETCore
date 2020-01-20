using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlBeltHold(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            int num21;
            double num28;
            double num29;
            double num44;
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

            int lookbackTotal = CdlBeltHoldLookback();
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
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num54;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num53;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num50;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num51;
                            double num52;
                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inClose[i];
                            }
                            else
                            {
                                num52 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num51 = inOpen[i];
                            }
                            else
                            {
                                num51 = inClose[i];
                            }

                            num50 = inHigh[i] - num52 + (num51 - inLow[i]);
                        }
                        else
                        {
                            num50 = 0.0;
                        }

                        num53 = num50;
                    }

                    num54 = num53;
                }

                bodyLongPeriodTotal += num54;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num46;
                            double num47;
                            if (inClose[i] >= inOpen[i])
                            {
                                num47 = inClose[i];
                            }
                            else
                            {
                                num47 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inOpen[i];
                            }
                            else
                            {
                                num46 = inClose[i];
                            }

                            num45 = inHigh[i] - num47 + (num46 - inLow[i]);
                        }
                        else
                        {
                            num45 = 0.0;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                shadowVeryShortPeriodTotal += num49;
                i++;
            }

            int outIdx = default;
            Label_022E:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num44 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num43;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num43 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num42;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num42 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num39;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num40;
                            double num41;
                            if (inClose[i] >= inOpen[i])
                            {
                                num41 = inClose[i];
                            }
                            else
                            {
                                num41 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num40 = inOpen[i];
                            }
                            else
                            {
                                num40 = inClose[i];
                            }

                            num39 = inHigh[i] - num41 + (num40 - inLow[i]);
                        }
                        else
                        {
                            num39 = 0.0;
                        }

                        num42 = num39;
                    }

                    num43 = num42;
                }

                num44 = num43;
            }

            var num38 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) <= Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num44 / num38)
            {
                goto Label_064A;
            }

            if (inClose[i] >= inOpen[i])
            {
                double num36;
                double num37;
                if (inClose[i] >= inOpen[i])
                {
                    num37 = inOpen[i];
                }
                else
                {
                    num37 = inClose[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num36 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    double num35;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num31;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                double num32;
                                double num33;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num33 = inClose[i];
                                }
                                else
                                {
                                    num33 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num32 = inOpen[i];
                                }
                                else
                                {
                                    num32 = inClose[i];
                                }

                                num31 = inHigh[i] - num33 + (num32 - inLow[i]);
                            }
                            else
                            {
                                num31 = 0.0;
                            }

                            num34 = num31;
                        }

                        num35 = num34;
                    }

                    num36 = num35;
                }

                var num30 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (num37 - inLow[i] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num36 / num30)
                {
                    goto Label_062A;
                }
            }

            if (inClose[i] >= inOpen[i])
            {
                goto Label_064A;
            }

            if (inClose[i] >= inOpen[i])
            {
                num29 = inClose[i];
            }
            else
            {
                num29 = inOpen[i];
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
            {
                num28 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            }
            else
            {
                double num27;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num27 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num26 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num23;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num24;
                            double num25;
                            if (inClose[i] >= inOpen[i])
                            {
                                num25 = inClose[i];
                            }
                            else
                            {
                                num25 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num24 = inOpen[i];
                            }
                            else
                            {
                                num24 = inClose[i];
                            }

                            num23 = inHigh[i] - num25 + (num24 - inLow[i]);
                        }
                        else
                        {
                            num23 = 0.0;
                        }

                        num26 = num23;
                    }

                    num27 = num26;
                }

                num28 = num27;
            }

            var num22 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inHigh[i] - num29 >= Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num28 / num22)
            {
                goto Label_064A;
            }

            Label_062A:
            if (inClose[i] >= inOpen[i])
            {
                num21 = 1;
            }
            else
            {
                num21 = -1;
            }

            outInteger[outIdx] = num21 * 100;
            outIdx++;
            goto Label_0653;
            Label_064A:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0653:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inClose[i];
                        }
                        else
                        {
                            num18 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num17 = inOpen[i];
                        }
                        else
                        {
                            num17 = inClose[i];
                        }

                        num16 = inHigh[i] - num18 + (num17 - inLow[i]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num13 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num12 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyLongTrailingIdx];
                        }

                        num11 = inHigh[bodyLongTrailingIdx] - num13 + (num12 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyLongPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i] >= inOpen[i])
                        {
                            num8 = inClose[i];
                        }
                        else
                        {
                            num8 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num7 = inOpen[i];
                        }
                        else
                        {
                            num7 = inClose[i];
                        }

                        num6 = inHigh[i] - num8 + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx] - inOpen[shadowVeryShortTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryShortTrailingIdx] - inLow[shadowVeryShortTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num3 = inClose[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryShortTrailingIdx];
                        }

                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num2 = inOpen[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryShortTrailingIdx];
                        }

                        num = inHigh[shadowVeryShortTrailingIdx] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryShortPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_022E;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlBeltHold(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            int num21;
            decimal num28;
            decimal num29;
            decimal num44;
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

            int lookbackTotal = CdlBeltHoldLookback();
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
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num54;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num53;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num50;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num51;
                            decimal num52;
                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inClose[i];
                            }
                            else
                            {
                                num52 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num51 = inOpen[i];
                            }
                            else
                            {
                                num51 = inClose[i];
                            }

                            num50 = inHigh[i] - num52 + (num51 - inLow[i]);
                        }
                        else
                        {
                            num50 = Decimal.Zero;
                        }

                        num53 = num50;
                    }

                    num54 = num53;
                }

                bodyLongPeriodTotal += num54;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num46;
                            decimal num47;
                            if (inClose[i] >= inOpen[i])
                            {
                                num47 = inClose[i];
                            }
                            else
                            {
                                num47 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inOpen[i];
                            }
                            else
                            {
                                num46 = inClose[i];
                            }

                            num45 = inHigh[i] - num47 + (num46 - inLow[i]);
                        }
                        else
                        {
                            num45 = Decimal.Zero;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                shadowVeryShortPeriodTotal += num49;
                i++;
            }

            int outIdx = default;
            Label_024A:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num44 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num43;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num43 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num42;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num42 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num39;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num40;
                            decimal num41;
                            if (inClose[i] >= inOpen[i])
                            {
                                num41 = inClose[i];
                            }
                            else
                            {
                                num41 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num40 = inOpen[i];
                            }
                            else
                            {
                                num40 = inClose[i];
                            }

                            num39 = inHigh[i] - num41 + (num40 - inLow[i]);
                        }
                        else
                        {
                            num39 = Decimal.Zero;
                        }

                        num42 = num39;
                    }

                    num43 = num42;
                }

                num44 = num43;
            }

            var num38 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i] - inOpen[i]) <=
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num44 / num38)
            {
                goto Label_06A2;
            }

            if (inClose[i] >= inOpen[i])
            {
                decimal num36;
                decimal num37;
                if (inClose[i] >= inOpen[i])
                {
                    num37 = inOpen[i];
                }
                else
                {
                    num37 = inClose[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num36 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    decimal num35;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        decimal num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            decimal num31;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                decimal num32;
                                decimal num33;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num33 = inClose[i];
                                }
                                else
                                {
                                    num33 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num32 = inOpen[i];
                                }
                                else
                                {
                                    num32 = inClose[i];
                                }

                                num31 = inHigh[i] - num33 + (num32 - inLow[i]);
                            }
                            else
                            {
                                num31 = Decimal.Zero;
                            }

                            num34 = num31;
                        }

                        num35 = num34;
                    }

                    num36 = num35;
                }

                var num30 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (num37 - inLow[i] < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num36 / num30)
                {
                    goto Label_0680;
                }
            }

            if (inClose[i] >= inOpen[i])
            {
                goto Label_06A2;
            }

            if (inClose[i] >= inOpen[i])
            {
                num29 = inClose[i];
            }
            else
            {
                num29 = inOpen[i];
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
            {
                num28 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            }
            else
            {
                decimal num27;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num27 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num26 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num23;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num24;
                            decimal num25;
                            if (inClose[i] >= inOpen[i])
                            {
                                num25 = inClose[i];
                            }
                            else
                            {
                                num25 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num24 = inOpen[i];
                            }
                            else
                            {
                                num24 = inClose[i];
                            }

                            num23 = inHigh[i] - num25 + (num24 - inLow[i]);
                        }
                        else
                        {
                            num23 = Decimal.Zero;
                        }

                        num26 = num23;
                    }

                    num27 = num26;
                }

                num28 = num27;
            }

            var num22 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inHigh[i] - num29 >= (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num28 / num22)
            {
                goto Label_06A2;
            }

            Label_0680:
            if (inClose[i] >= inOpen[i])
            {
                num21 = 1;
            }
            else
            {
                num21 = -1;
            }

            outInteger[outIdx] = num21 * 100;
            outIdx++;
            goto Label_06AB;
            Label_06A2:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_06AB:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num17;
                        decimal num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inClose[i];
                        }
                        else
                        {
                            num18 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num17 = inOpen[i];
                        }
                        else
                        {
                            num17 = inClose[i];
                        }

                        num16 = inHigh[i] - num18 + (num17 - inLow[i]);
                    }
                    else
                    {
                        num16 = Decimal.Zero;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num13 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num12 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyLongTrailingIdx];
                        }

                        num11 = inHigh[bodyLongTrailingIdx] - num13 + (num12 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyLongPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
                        if (inClose[i] >= inOpen[i])
                        {
                            num8 = inClose[i];
                        }
                        else
                        {
                            num8 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num7 = inOpen[i];
                        }
                        else
                        {
                            num7 = inClose[i];
                        }

                        num6 = inHigh[i] - num8 + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = Decimal.Zero;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx] - inOpen[shadowVeryShortTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryShortTrailingIdx] - inLow[shadowVeryShortTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num3 = inClose[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryShortTrailingIdx];
                        }

                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num2 = inOpen[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryShortTrailingIdx];
                        }

                        num = inHigh[shadowVeryShortTrailingIdx] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryShortPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_024A;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        private static int CdlBeltHoldLookback()
        {
            return Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod);
        }
    }
}
