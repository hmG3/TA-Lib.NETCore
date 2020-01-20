using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlLongLeggedDoji(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num43;
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

            int lookbackTotal = CdlLongLeggedDojiLookback();
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

            double bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            double shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
            int i = bodyDojiTrailingIdx;
            while (true)
            {
                double num53;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num53 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num52;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num52 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num49;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num50;
                            double num51;
                            if (inClose[i] >= inOpen[i])
                            {
                                num51 = inClose[i];
                            }
                            else
                            {
                                num51 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num50 = inOpen[i];
                            }
                            else
                            {
                                num50 = inClose[i];
                            }

                            num49 = inHigh[i] - num51 + (num50 - inLow[i]);
                        }
                        else
                        {
                            num49 = 0.0;
                        }

                        num52 = num49;
                    }

                    num53 = num52;
                }

                bodyDojiPeriodTotal += num53;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                double num48;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num48 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num47;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num47 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num44;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            double num45;
                            double num46;
                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inClose[i];
                            }
                            else
                            {
                                num46 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num45 = inOpen[i];
                            }
                            else
                            {
                                num45 = inClose[i];
                            }

                            num44 = inHigh[i] - num46 + (num45 - inLow[i]);
                        }
                        else
                        {
                            num44 = 0.0;
                        }

                        num47 = num44;
                    }

                    num48 = num47;
                }

                shadowLongPeriodTotal += num48;
                i++;
            }

            int outIdx = default;
            Label_022E:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
            {
                num43 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            }
            else
            {
                double num42;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num42 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num41;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num41 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num38;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num39;
                            double num40;
                            if (inClose[i] >= inOpen[i])
                            {
                                num40 = inClose[i];
                            }
                            else
                            {
                                num40 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num39 = inOpen[i];
                            }
                            else
                            {
                                num39 = inClose[i];
                            }

                            num38 = inHigh[i] - num40 + (num39 - inLow[i]);
                        }
                        else
                        {
                            num38 = 0.0;
                        }

                        num41 = num38;
                    }

                    num42 = num41;
                }

                num43 = num42;
            }

            var num37 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) <= Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num43 / num37)
            {
                double num35;
                double num36;
                if (inClose[i] >= inOpen[i])
                {
                    num36 = inOpen[i];
                }
                else
                {
                    num36 = inClose[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                {
                    num35 = shadowLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                }
                else
                {
                    double num34;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                    {
                        num34 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num33;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                        {
                            num33 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num30;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                            {
                                double num31;
                                double num32;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num32 = inClose[i];
                                }
                                else
                                {
                                    num32 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num31 = inOpen[i];
                                }
                                else
                                {
                                    num31 = inClose[i];
                                }

                                num30 = inHigh[i] - num32 + (num31 - inLow[i]);
                            }
                            else
                            {
                                num30 = 0.0;
                            }

                            num33 = num30;
                        }

                        num34 = num33;
                    }

                    num35 = num34;
                }

                var num29 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (num36 - inLow[i] <= Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num35 / num29)
                {
                    double num27;
                    double num28;
                    if (inClose[i] >= inOpen[i])
                    {
                        num28 = inClose[i];
                    }
                    else
                    {
                        num28 = inOpen[i];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                    {
                        num27 = shadowLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                    }
                    else
                    {
                        double num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                        {
                            num26 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num25;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                            {
                                num25 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num22;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                                {
                                    double num23;
                                    double num24;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num24 = inClose[i];
                                    }
                                    else
                                    {
                                        num24 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num23 = inOpen[i];
                                    }
                                    else
                                    {
                                        num23 = inClose[i];
                                    }

                                    num22 = inHigh[i] - num24 + (num23 - inLow[i]);
                                }
                                else
                                {
                                    num22 = 0.0;
                                }

                                num25 = num22;
                            }

                            num26 = num25;
                        }

                        num27 = num26;
                    }

                    var num21 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (inHigh[i] - num28 <= Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num27 / num21)
                    {
                        goto Label_0610;
                    }
                }

                outInteger[outIdx] = 100;
                outIdx++;
                goto Label_0619;
            }

            Label_0610:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0619:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyDojiTrailingIdx] - inOpen[bodyDojiTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyDojiTrailingIdx] - inLow[bodyDojiTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num13 = inClose[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyDojiTrailingIdx];
                        }

                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num12 = inOpen[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyDojiTrailingIdx];
                        }

                        num11 = inHigh[bodyDojiTrailingIdx] - num13 + (num12 - inLow[bodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyDojiPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowLongTrailingIdx] - inOpen[shadowLongTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowLongTrailingIdx] - inLow[shadowLongTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num3 = inClose[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowLongTrailingIdx];
                        }

                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num2 = inOpen[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowLongTrailingIdx];
                        }

                        num = inHigh[shadowLongTrailingIdx] - num3 + (num2 - inLow[shadowLongTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowLongPeriodTotal += num10 - num5;
            i++;
            bodyDojiTrailingIdx++;
            shadowLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_022E;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlLongLeggedDoji(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num43;
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

            int lookbackTotal = CdlLongLeggedDojiLookback();
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

            decimal bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            decimal shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
            int i = bodyDojiTrailingIdx;
            while (true)
            {
                decimal num53;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num53 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num52;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num52 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num49;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num50;
                            decimal num51;
                            if (inClose[i] >= inOpen[i])
                            {
                                num51 = inClose[i];
                            }
                            else
                            {
                                num51 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num50 = inOpen[i];
                            }
                            else
                            {
                                num50 = inClose[i];
                            }

                            num49 = inHigh[i] - num51 + (num50 - inLow[i]);
                        }
                        else
                        {
                            num49 = Decimal.Zero;
                        }

                        num52 = num49;
                    }

                    num53 = num52;
                }

                bodyDojiPeriodTotal += num53;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                decimal num48;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num48 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num47;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num47 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num44;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            decimal num45;
                            decimal num46;
                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inClose[i];
                            }
                            else
                            {
                                num46 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num45 = inOpen[i];
                            }
                            else
                            {
                                num45 = inClose[i];
                            }

                            num44 = inHigh[i] - num46 + (num45 - inLow[i]);
                        }
                        else
                        {
                            num44 = Decimal.Zero;
                        }

                        num47 = num44;
                    }

                    num48 = num47;
                }

                shadowLongPeriodTotal += num48;
                i++;
            }

            int outIdx = default;
            Label_024A:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
            {
                num43 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            }
            else
            {
                decimal num42;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num42 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num41;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num41 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num38;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num39;
                            decimal num40;
                            if (inClose[i] >= inOpen[i])
                            {
                                num40 = inClose[i];
                            }
                            else
                            {
                                num40 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num39 = inOpen[i];
                            }
                            else
                            {
                                num39 = inClose[i];
                            }

                            num38 = inHigh[i] - num40 + (num39 - inLow[i]);
                        }
                        else
                        {
                            num38 = Decimal.Zero;
                        }

                        num41 = num38;
                    }

                    num42 = num41;
                }

                num43 = num42;
            }

            var num37 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i] - inOpen[i]) <=
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num43 / num37)
            {
                decimal num35;
                decimal num36;
                if (inClose[i] >= inOpen[i])
                {
                    num36 = inOpen[i];
                }
                else
                {
                    num36 = inClose[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                {
                    num35 = shadowLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                }
                else
                {
                    decimal num34;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                    {
                        num34 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        decimal num33;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                        {
                            num33 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            decimal num30;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                            {
                                decimal num31;
                                decimal num32;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num32 = inClose[i];
                                }
                                else
                                {
                                    num32 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num31 = inOpen[i];
                                }
                                else
                                {
                                    num31 = inClose[i];
                                }

                                num30 = inHigh[i] - num32 + (num31 - inLow[i]);
                            }
                            else
                            {
                                num30 = Decimal.Zero;
                            }

                            num33 = num30;
                        }

                        num34 = num33;
                    }

                    num35 = num34;
                }

                var num29 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (num36 - inLow[i] <= (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num35 / num29)
                {
                    decimal num27;
                    decimal num28;
                    if (inClose[i] >= inOpen[i])
                    {
                        num28 = inClose[i];
                    }
                    else
                    {
                        num28 = inOpen[i];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                    {
                        num27 = shadowLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                    }
                    else
                    {
                        decimal num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                        {
                            num26 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num25;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                            {
                                num25 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num22;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                                {
                                    decimal num23;
                                    decimal num24;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num24 = inClose[i];
                                    }
                                    else
                                    {
                                        num24 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num23 = inOpen[i];
                                    }
                                    else
                                    {
                                        num23 = inClose[i];
                                    }

                                    num22 = inHigh[i] - num24 + (num23 - inLow[i]);
                                }
                                else
                                {
                                    num22 = Decimal.Zero;
                                }

                                num25 = num22;
                            }

                            num26 = num25;
                        }

                        num27 = num26;
                    }

                    var num21 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (inHigh[i] - num28 <= (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num27 / num21)
                    {
                        goto Label_0662;
                    }
                }

                outInteger[outIdx] = 100;
                outIdx++;
                goto Label_066B;
            }

            Label_0662:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_066B:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyDojiTrailingIdx] - inOpen[bodyDojiTrailingIdx]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyDojiTrailingIdx] - inLow[bodyDojiTrailingIdx];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num13 = inClose[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyDojiTrailingIdx];
                        }

                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num12 = inOpen[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyDojiTrailingIdx];
                        }

                        num11 = inHigh[bodyDojiTrailingIdx] - num13 + (num12 - inLow[bodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyDojiPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowLongTrailingIdx] - inOpen[shadowLongTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowLongTrailingIdx] - inLow[shadowLongTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num3 = inClose[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowLongTrailingIdx];
                        }

                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num2 = inOpen[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowLongTrailingIdx];
                        }

                        num = inHigh[shadowLongTrailingIdx] - num3 + (num2 - inLow[shadowLongTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowLongPeriodTotal += num10 - num5;
            i++;
            bodyDojiTrailingIdx++;
            shadowLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_024A;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlLongLeggedDojiLookback()
        {
            return Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod);
        }
    }
}
