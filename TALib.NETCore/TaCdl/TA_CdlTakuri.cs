using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlTakuri(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            double num53;
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

            int lookbackTotal = CdlTakuriLookback();
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
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            double shadowVeryLongPeriodTotal = default;
            int shadowVeryLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].AvgPeriod;
            int i = bodyDojiTrailingIdx;
            while (true)
            {
                double num68;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num68 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num67;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num67 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num64;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num65;
                            double num66;
                            if (inClose[i] >= inOpen[i])
                            {
                                num66 = inClose[i];
                            }
                            else
                            {
                                num66 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inOpen[i];
                            }
                            else
                            {
                                num65 = inClose[i];
                            }

                            num64 = inHigh[i] - num66 + (num65 - inLow[i]);
                        }
                        else
                        {
                            num64 = 0.0;
                        }

                        num67 = num64;
                    }

                    num68 = num67;
                }

                bodyDojiPeriodTotal += num68;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num63;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num63 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num62;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num62 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num59;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num60;
                            double num61;
                            if (inClose[i] >= inOpen[i])
                            {
                                num61 = inClose[i];
                            }
                            else
                            {
                                num61 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num60 = inOpen[i];
                            }
                            else
                            {
                                num60 = inClose[i];
                            }

                            num59 = inHigh[i] - num61 + (num60 - inLow[i]);
                        }
                        else
                        {
                            num59 = 0.0;
                        }

                        num62 = num59;
                    }

                    num63 = num62;
                }

                shadowVeryShortPeriodTotal += num63;
                i++;
            }

            i = shadowVeryLongTrailingIdx;
            while (true)
            {
                double num58;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.RealBody)
                {
                    num58 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num57;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.HighLow)
                    {
                        num57 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num54;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows)
                        {
                            double num55;
                            double num56;
                            if (inClose[i] >= inOpen[i])
                            {
                                num56 = inClose[i];
                            }
                            else
                            {
                                num56 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inOpen[i];
                            }
                            else
                            {
                                num55 = inClose[i];
                            }

                            num54 = inHigh[i] - num56 + (num55 - inLow[i]);
                        }
                        else
                        {
                            num54 = 0.0;
                        }

                        num57 = num54;
                    }

                    num58 = num57;
                }

                shadowVeryLongPeriodTotal += num58;
                i++;
            }

            int outIdx = default;
            Label_0313:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
            {
                num53 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            }
            else
            {
                double num52;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num52 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num51;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num51 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num48;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num49;
                            double num50;
                            if (inClose[i] >= inOpen[i])
                            {
                                num50 = inClose[i];
                            }
                            else
                            {
                                num50 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num49 = inOpen[i];
                            }
                            else
                            {
                                num49 = inClose[i];
                            }

                            num48 = inHigh[i] - num50 + (num49 - inLow[i]);
                        }
                        else
                        {
                            num48 = 0.0;
                        }

                        num51 = num48;
                    }

                    num52 = num51;
                }

                num53 = num52;
            }

            var num47 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) <= Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num53 / num47)
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

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num45 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    double num44;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num44 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num43;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num43 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num40;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                double num41;
                                double num42;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num42 = inClose[i];
                                }
                                else
                                {
                                    num42 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num41 = inOpen[i];
                                }
                                else
                                {
                                    num41 = inClose[i];
                                }

                                num40 = inHigh[i] - num42 + (num41 - inLow[i]);
                            }
                            else
                            {
                                num40 = 0.0;
                            }

                            num43 = num40;
                        }

                        num44 = num43;
                    }

                    num45 = num44;
                }

                var num39 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (inHigh[i] - num46 < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num45 / num39)
                {
                    double num37;
                    double num38;
                    if (inClose[i] >= inOpen[i])
                    {
                        num38 = inOpen[i];
                    }
                    else
                    {
                        num38 = inClose[i];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].AvgPeriod != 0)
                    {
                        num37 = shadowVeryLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].AvgPeriod;
                    }
                    else
                    {
                        double num36;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.RealBody)
                        {
                            num36 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num35;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.HighLow)
                            {
                                num35 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num32;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows)
                                {
                                    double num33;
                                    double num34;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num34 = inClose[i];
                                    }
                                    else
                                    {
                                        num34 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num33 = inOpen[i];
                                    }
                                    else
                                    {
                                        num33 = inClose[i];
                                    }

                                    num32 = inHigh[i] - num34 + (num33 - inLow[i]);
                                }
                                else
                                {
                                    num32 = 0.0;
                                }

                                num35 = num32;
                            }

                            num36 = num35;
                        }

                        num37 = num36;
                    }

                    var num31 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (num38 - inLow[i] > Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].Factor * num37 / num31)
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0704;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0704:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num29;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                        if (inClose[i] >= inOpen[i])
                        {
                            num27 = inOpen[i];
                        }
                        else
                        {
                            num27 = inClose[i];
                        }

                        num26 = inHigh[i] - num28 + (num27 - inLow[i]);
                    }
                    else
                    {
                        num26 = 0.0;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[bodyDojiTrailingIdx] - inOpen[bodyDojiTrailingIdx]);
            }
            else
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[bodyDojiTrailingIdx] - inLow[bodyDojiTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num23 = inClose[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[bodyDojiTrailingIdx];
                        }

                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num22 = inOpen[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[bodyDojiTrailingIdx];
                        }

                        num21 = inHigh[bodyDojiTrailingIdx] - num23 + (num22 - inLow[bodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            bodyDojiPeriodTotal += num30 - num25;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[shadowVeryShortTrailingIdx] - inOpen[shadowVeryShortTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[shadowVeryShortTrailingIdx] - inLow[shadowVeryShortTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num13 = inClose[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[shadowVeryShortTrailingIdx];
                        }

                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num12 = inOpen[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[shadowVeryShortTrailingIdx];
                        }

                        num11 = inHigh[shadowVeryShortTrailingIdx] - num13 + (num12 - inLow[shadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            shadowVeryShortPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryLongTrailingIdx] - inOpen[shadowVeryLongTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryLongTrailingIdx] - inLow[shadowVeryLongTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[shadowVeryLongTrailingIdx] >= inOpen[shadowVeryLongTrailingIdx])
                        {
                            num3 = inClose[shadowVeryLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryLongTrailingIdx];
                        }

                        if (inClose[shadowVeryLongTrailingIdx] >= inOpen[shadowVeryLongTrailingIdx])
                        {
                            num2 = inOpen[shadowVeryLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryLongTrailingIdx];
                        }

                        num = inHigh[shadowVeryLongTrailingIdx] - num3 + (num2 - inLow[shadowVeryLongTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryLongPeriodTotal += num10 - num5;
            i++;
            bodyDojiTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            shadowVeryLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0313;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlTakuri(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num25;
            decimal num30;
            decimal num53;
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

            int lookbackTotal = CdlTakuriLookback();
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
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            decimal shadowVeryLongPeriodTotal = default;
            int shadowVeryLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].AvgPeriod;
            int i = bodyDojiTrailingIdx;
            while (true)
            {
                decimal num68;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num68 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num67;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num67 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num64;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num65;
                            decimal num66;
                            if (inClose[i] >= inOpen[i])
                            {
                                num66 = inClose[i];
                            }
                            else
                            {
                                num66 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inOpen[i];
                            }
                            else
                            {
                                num65 = inClose[i];
                            }

                            num64 = inHigh[i] - num66 + (num65 - inLow[i]);
                        }
                        else
                        {
                            num64 = Decimal.Zero;
                        }

                        num67 = num64;
                    }

                    num68 = num67;
                }

                bodyDojiPeriodTotal += num68;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num63;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num63 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num62;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num62 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num59;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num60;
                            decimal num61;
                            if (inClose[i] >= inOpen[i])
                            {
                                num61 = inClose[i];
                            }
                            else
                            {
                                num61 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num60 = inOpen[i];
                            }
                            else
                            {
                                num60 = inClose[i];
                            }

                            num59 = inHigh[i] - num61 + (num60 - inLow[i]);
                        }
                        else
                        {
                            num59 = Decimal.Zero;
                        }

                        num62 = num59;
                    }

                    num63 = num62;
                }

                shadowVeryShortPeriodTotal += num63;
                i++;
            }

            i = shadowVeryLongTrailingIdx;
            while (true)
            {
                decimal num58;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.RealBody)
                {
                    num58 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num57;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.HighLow)
                    {
                        num57 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num54;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows)
                        {
                            decimal num55;
                            decimal num56;
                            if (inClose[i] >= inOpen[i])
                            {
                                num56 = inClose[i];
                            }
                            else
                            {
                                num56 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inOpen[i];
                            }
                            else
                            {
                                num55 = inClose[i];
                            }

                            num54 = inHigh[i] - num56 + (num55 - inLow[i]);
                        }
                        else
                        {
                            num54 = Decimal.Zero;
                        }

                        num57 = num54;
                    }

                    num58 = num57;
                }

                shadowVeryLongPeriodTotal += num58;
                i++;
            }

            int outIdx = default;
            Label_033D:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
            {
                num53 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            }
            else
            {
                decimal num52;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num52 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num51;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num51 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num48;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num49;
                            decimal num50;
                            if (inClose[i] >= inOpen[i])
                            {
                                num50 = inClose[i];
                            }
                            else
                            {
                                num50 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num49 = inOpen[i];
                            }
                            else
                            {
                                num49 = inClose[i];
                            }

                            num48 = inHigh[i] - num50 + (num49 - inLow[i]);
                        }
                        else
                        {
                            num48 = Decimal.Zero;
                        }

                        num51 = num48;
                    }

                    num52 = num51;
                }

                num53 = num52;
            }

            var num47 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i] - inOpen[i]) <=
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num53 / num47)
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

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num45 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    decimal num44;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num44 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        decimal num43;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num43 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            decimal num40;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                decimal num41;
                                decimal num42;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num42 = inClose[i];
                                }
                                else
                                {
                                    num42 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num41 = inOpen[i];
                                }
                                else
                                {
                                    num41 = inClose[i];
                                }

                                num40 = inHigh[i] - num42 + (num41 - inLow[i]);
                            }
                            else
                            {
                                num40 = Decimal.Zero;
                            }

                            num43 = num40;
                        }

                        num44 = num43;
                    }

                    num45 = num44;
                }

                var num39 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (inHigh[i] - num46 < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num45 / num39)
                {
                    decimal num37;
                    decimal num38;
                    if (inClose[i] >= inOpen[i])
                    {
                        num38 = inOpen[i];
                    }
                    else
                    {
                        num38 = inClose[i];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].AvgPeriod != 0)
                    {
                        num37 = shadowVeryLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].AvgPeriod;
                    }
                    else
                    {
                        decimal num36;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.RealBody)
                        {
                            num36 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num35;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.HighLow)
                            {
                                num35 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num32;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows)
                                {
                                    decimal num33;
                                    decimal num34;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num34 = inClose[i];
                                    }
                                    else
                                    {
                                        num34 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num33 = inOpen[i];
                                    }
                                    else
                                    {
                                        num33 = inClose[i];
                                    }

                                    num32 = inHigh[i] - num34 + (num33 - inLow[i]);
                                }
                                else
                                {
                                    num32 = Decimal.Zero;
                                }

                                num35 = num32;
                            }

                            num36 = num35;
                        }

                        num37 = num36;
                    }

                    var num31 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (num38 - inLow[i] > (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].Factor * num37 / num31)
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0764;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0764:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num29;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                        if (inClose[i] >= inOpen[i])
                        {
                            num27 = inOpen[i];
                        }
                        else
                        {
                            num27 = inClose[i];
                        }

                        num26 = inHigh[i] - num28 + (num27 - inLow[i]);
                    }
                    else
                    {
                        num26 = Decimal.Zero;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[bodyDojiTrailingIdx] - inOpen[bodyDojiTrailingIdx]);
            }
            else
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[bodyDojiTrailingIdx] - inLow[bodyDojiTrailingIdx];
                }
                else
                {
                    decimal num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        decimal num22;
                        decimal num23;
                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num23 = inClose[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[bodyDojiTrailingIdx];
                        }

                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num22 = inOpen[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[bodyDojiTrailingIdx];
                        }

                        num21 = inHigh[bodyDojiTrailingIdx] - num23 + (num22 - inLow[bodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num21 = Decimal.Zero;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            bodyDojiPeriodTotal += num30 - num25;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[shadowVeryShortTrailingIdx] - inOpen[shadowVeryShortTrailingIdx]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[shadowVeryShortTrailingIdx] - inLow[shadowVeryShortTrailingIdx];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num13 = inClose[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[shadowVeryShortTrailingIdx];
                        }

                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num12 = inOpen[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[shadowVeryShortTrailingIdx];
                        }

                        num11 = inHigh[shadowVeryShortTrailingIdx] - num13 + (num12 - inLow[shadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            shadowVeryShortPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryLongTrailingIdx] - inOpen[shadowVeryLongTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryLongTrailingIdx] - inLow[shadowVeryLongTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[shadowVeryLongTrailingIdx] >= inOpen[shadowVeryLongTrailingIdx])
                        {
                            num3 = inClose[shadowVeryLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryLongTrailingIdx];
                        }

                        if (inClose[shadowVeryLongTrailingIdx] >= inOpen[shadowVeryLongTrailingIdx])
                        {
                            num2 = inOpen[shadowVeryLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryLongTrailingIdx];
                        }

                        num = inHigh[shadowVeryLongTrailingIdx] - num3 + (num2 - inLow[shadowVeryLongTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryLongPeriodTotal += num10 - num5;
            i++;
            bodyDojiTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            shadowVeryLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_033D;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlTakuriLookback()
        {
            return Math.Max(
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod),
                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryLong].AvgPeriod
            );
        }
    }
}
