using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlSeparatingLines(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            int num31;
            double num38;
            double num39;
            double num54;
            double num61;
            double num68;
            int num69;
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

            int lookbackTotal = CdlSeparatingLinesLookback();
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

            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            double bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            double equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num84;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num84 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num83;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num83 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num80;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num81;
                            double num82;
                            if (inClose[i] >= inOpen[i])
                            {
                                num82 = inClose[i];
                            }
                            else
                            {
                                num82 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num81 = inOpen[i];
                            }
                            else
                            {
                                num81 = inClose[i];
                            }

                            num80 = inHigh[i] - num82 + (num81 - inLow[i]);
                        }
                        else
                        {
                            num80 = 0.0;
                        }

                        num83 = num80;
                    }

                    num84 = num83;
                }

                shadowVeryShortPeriodTotal += num84;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                double num79;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num79 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num78;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num78 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num75;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num76;
                            double num77;
                            if (inClose[i] >= inOpen[i])
                            {
                                num77 = inClose[i];
                            }
                            else
                            {
                                num77 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num76 = inOpen[i];
                            }
                            else
                            {
                                num76 = inClose[i];
                            }

                            num75 = inHigh[i] - num77 + (num76 - inLow[i]);
                        }
                        else
                        {
                            num75 = 0.0;
                        }

                        num78 = num75;
                    }

                    num79 = num78;
                }

                bodyLongPeriodTotal += num79;
                i++;
            }

            i = equalTrailingIdx;
            while (true)
            {
                double num74;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num74 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num73;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num73 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num70;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            double num71;
                            double num72;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num72 = inClose[i - 1];
                            }
                            else
                            {
                                num72 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num71 = inOpen[i - 1];
                            }
                            else
                            {
                                num71 = inClose[i - 1];
                            }

                            num70 = inHigh[i - 1] - num72 + (num71 - inLow[i - 1]);
                        }
                        else
                        {
                            num70 = 0.0;
                        }

                        num73 = num70;
                    }

                    num74 = num73;
                }

                equalPeriodTotal += num74;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0338:
            if (inClose[i] >= inOpen[i])
            {
                num69 = 1;
            }
            else
            {
                num69 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) != -num69)
            {
                goto Label_0A41;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
            {
                num68 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            }
            else
            {
                double num67;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num66;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num63;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            double num64;
                            double num65;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num65 = inClose[i - 1];
                            }
                            else
                            {
                                num65 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num64 = inOpen[i - 1];
                            }
                            else
                            {
                                num64 = inClose[i - 1];
                            }

                            num63 = inHigh[i - 1] - num65 + (num64 - inLow[i - 1]);
                        }
                        else
                        {
                            num63 = 0.0;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                num68 = num67;
            }

            var num62 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inOpen[i] > inOpen[i - 1] + Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num68 / num62)
            {
                goto Label_0A41;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
            {
                num61 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            }
            else
            {
                double num60;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num59;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num56;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            double num57;
                            double num58;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num58 = inClose[i - 1];
                            }
                            else
                            {
                                num58 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num57 = inOpen[i - 1];
                            }
                            else
                            {
                                num57 = inClose[i - 1];
                            }

                            num56 = inHigh[i - 1] - num58 + (num57 - inLow[i - 1]);
                        }
                        else
                        {
                            num56 = 0.0;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                num61 = num60;
            }

            var num55 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inOpen[i] < inOpen[i - 1] - Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num61 / num55)
            {
                goto Label_0A41;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num54 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num53;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num53 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num52;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num52 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num49;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                num54 = num53;
            }

            var num48 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) <= Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num54 / num48)
            {
                goto Label_0A41;
            }

            if (inClose[i] >= inOpen[i])
            {
                double num46;
                double num47;
                if (inClose[i] >= inOpen[i])
                {
                    num47 = inOpen[i];
                }
                else
                {
                    num47 = inClose[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num46 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    double num45;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num45 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num44;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num44 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num41;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                double num42;
                                double num43;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num43 = inClose[i];
                                }
                                else
                                {
                                    num43 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num42 = inOpen[i];
                                }
                                else
                                {
                                    num42 = inClose[i];
                                }

                                num41 = inHigh[i] - num43 + (num42 - inLow[i]);
                            }
                            else
                            {
                                num41 = 0.0;
                            }

                            num44 = num41;
                        }

                        num45 = num44;
                    }

                    num46 = num45;
                }

                var num40 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (num47 - inLow[i] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num46 / num40)
                {
                    goto Label_0A1E;
                }
            }

            if (inClose[i] >= inOpen[i])
            {
                goto Label_0A41;
            }

            if (inClose[i] >= inOpen[i])
            {
                num39 = inClose[i];
            }
            else
            {
                num39 = inOpen[i];
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
            {
                num38 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            }
            else
            {
                double num37;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num37 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num36;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num36 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num33;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num34;
                            double num35;
                            if (inClose[i] >= inOpen[i])
                            {
                                num35 = inClose[i];
                            }
                            else
                            {
                                num35 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num34 = inOpen[i];
                            }
                            else
                            {
                                num34 = inClose[i];
                            }

                            num33 = inHigh[i] - num35 + (num34 - inLow[i]);
                        }
                        else
                        {
                            num33 = 0.0;
                        }

                        num36 = num33;
                    }

                    num37 = num36;
                }

                num38 = num37;
            }

            var num32 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inHigh[i] - num39 >= Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num38 / num32)
            {
                goto Label_0A41;
            }

            Label_0A1E:
            if (inClose[i] >= inOpen[i])
            {
                num31 = 1;
            }
            else
            {
                num31 = -1;
            }

            outInteger[outIdx] = num31 * 100;
            outIdx++;
            goto Label_0A4D;
            Label_0A41:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0A4D:
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num29;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[shadowVeryShortTrailingIdx] - inOpen[shadowVeryShortTrailingIdx]);
            }
            else
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[shadowVeryShortTrailingIdx] - inLow[shadowVeryShortTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num23 = inClose[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[shadowVeryShortTrailingIdx];
                        }

                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num22 = inOpen[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[shadowVeryShortTrailingIdx];
                        }

                        num21 = inHigh[shadowVeryShortTrailingIdx] - num23 + (num22 - inLow[shadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            shadowVeryShortPeriodTotal += num30 - num25;
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
            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num3 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num2 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[equalTrailingIdx - 1];
                        }

                        num = inHigh[equalTrailingIdx - 1] - num3 + (num2 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            equalPeriodTotal += num10 - num5;
            i++;
            shadowVeryShortTrailingIdx++;
            bodyLongTrailingIdx++;
            equalTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0338;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlSeparatingLines(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num25;
            decimal num30;
            int num31;
            decimal num38;
            decimal num39;
            decimal num54;
            decimal num61;
            decimal num68;
            int num69;
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

            int lookbackTotal = CdlSeparatingLinesLookback();
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

            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            decimal bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            decimal equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num84;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num84 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num83;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num83 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num80;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num81;
                            decimal num82;
                            if (inClose[i] >= inOpen[i])
                            {
                                num82 = inClose[i];
                            }
                            else
                            {
                                num82 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num81 = inOpen[i];
                            }
                            else
                            {
                                num81 = inClose[i];
                            }

                            num80 = inHigh[i] - num82 + (num81 - inLow[i]);
                        }
                        else
                        {
                            num80 = Decimal.Zero;
                        }

                        num83 = num80;
                    }

                    num84 = num83;
                }

                shadowVeryShortPeriodTotal += num84;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num79;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num79 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num78;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num78 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num75;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num76;
                            decimal num77;
                            if (inClose[i] >= inOpen[i])
                            {
                                num77 = inClose[i];
                            }
                            else
                            {
                                num77 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num76 = inOpen[i];
                            }
                            else
                            {
                                num76 = inClose[i];
                            }

                            num75 = inHigh[i] - num77 + (num76 - inLow[i]);
                        }
                        else
                        {
                            num75 = Decimal.Zero;
                        }

                        num78 = num75;
                    }

                    num79 = num78;
                }

                bodyLongPeriodTotal += num79;
                i++;
            }

            i = equalTrailingIdx;
            while (true)
            {
                decimal num74;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num74 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num73;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num73 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num70;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num71;
                            decimal num72;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num72 = inClose[i - 1];
                            }
                            else
                            {
                                num72 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num71 = inOpen[i - 1];
                            }
                            else
                            {
                                num71 = inClose[i - 1];
                            }

                            num70 = inHigh[i - 1] - num72 + (num71 - inLow[i - 1]);
                        }
                        else
                        {
                            num70 = Decimal.Zero;
                        }

                        num73 = num70;
                    }

                    num74 = num73;
                }

                equalPeriodTotal += num74;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0362:
            if (inClose[i] >= inOpen[i])
            {
                num69 = 1;
            }
            else
            {
                num69 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) != -num69)
            {
                goto Label_0ACB;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
            {
                num68 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            }
            else
            {
                decimal num67;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num66;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num63;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num64;
                            decimal num65;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num65 = inClose[i - 1];
                            }
                            else
                            {
                                num65 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num64 = inOpen[i - 1];
                            }
                            else
                            {
                                num64 = inClose[i - 1];
                            }

                            num63 = inHigh[i - 1] - num65 + (num64 - inLow[i - 1]);
                        }
                        else
                        {
                            num63 = Decimal.Zero;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                num68 = num67;
            }

            var num62 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inOpen[i] > inOpen[i - 1] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num68 / num62)
            {
                goto Label_0ACB;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
            {
                num61 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            }
            else
            {
                decimal num60;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num59;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num56;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num57;
                            decimal num58;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num58 = inClose[i - 1];
                            }
                            else
                            {
                                num58 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num57 = inOpen[i - 1];
                            }
                            else
                            {
                                num57 = inClose[i - 1];
                            }

                            num56 = inHigh[i - 1] - num58 + (num57 - inLow[i - 1]);
                        }
                        else
                        {
                            num56 = Decimal.Zero;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                num61 = num60;
            }

            var num55 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inOpen[i] < inOpen[i - 1] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num61 / num55)
            {
                goto Label_0ACB;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num54 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num53;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num53 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num52;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num52 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num49;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                num54 = num53;
            }

            var num48 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i] - inOpen[i]) <=
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num54 / num48)
            {
                goto Label_0ACB;
            }

            if (inClose[i] >= inOpen[i])
            {
                decimal num46;
                decimal num47;
                if (inClose[i] >= inOpen[i])
                {
                    num47 = inOpen[i];
                }
                else
                {
                    num47 = inClose[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num46 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    decimal num45;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num45 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        decimal num44;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num44 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            decimal num41;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                decimal num42;
                                decimal num43;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num43 = inClose[i];
                                }
                                else
                                {
                                    num43 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num42 = inOpen[i];
                                }
                                else
                                {
                                    num42 = inClose[i];
                                }

                                num41 = inHigh[i] - num43 + (num42 - inLow[i]);
                            }
                            else
                            {
                                num41 = Decimal.Zero;
                            }

                            num44 = num41;
                        }

                        num45 = num44;
                    }

                    num46 = num45;
                }

                var num40 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (num47 - inLow[i] < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num46 / num40)
                {
                    goto Label_0AA6;
                }
            }

            if (inClose[i] >= inOpen[i])
            {
                goto Label_0ACB;
            }

            if (inClose[i] >= inOpen[i])
            {
                num39 = inClose[i];
            }
            else
            {
                num39 = inOpen[i];
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
            {
                num38 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            }
            else
            {
                decimal num37;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num37 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num36;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num36 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num33;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num34;
                            decimal num35;
                            if (inClose[i] >= inOpen[i])
                            {
                                num35 = inClose[i];
                            }
                            else
                            {
                                num35 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num34 = inOpen[i];
                            }
                            else
                            {
                                num34 = inClose[i];
                            }

                            num33 = inHigh[i] - num35 + (num34 - inLow[i]);
                        }
                        else
                        {
                            num33 = Decimal.Zero;
                        }

                        num36 = num33;
                    }

                    num37 = num36;
                }

                num38 = num37;
            }

            var num32 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inHigh[i] - num39 >= (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num38 / num32)
            {
                goto Label_0ACB;
            }

            Label_0AA6:
            if (inClose[i] >= inOpen[i])
            {
                num31 = 1;
            }
            else
            {
                num31 = -1;
            }

            outInteger[outIdx] = num31 * 100;
            outIdx++;
            goto Label_0AD7;
            Label_0ACB:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0AD7:
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num29;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[shadowVeryShortTrailingIdx] - inOpen[shadowVeryShortTrailingIdx]);
            }
            else
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[shadowVeryShortTrailingIdx] - inLow[shadowVeryShortTrailingIdx];
                }
                else
                {
                    decimal num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        decimal num22;
                        decimal num23;
                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num23 = inClose[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[shadowVeryShortTrailingIdx];
                        }

                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num22 = inOpen[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[shadowVeryShortTrailingIdx];
                        }

                        num21 = inHigh[shadowVeryShortTrailingIdx] - num23 + (num22 - inLow[shadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num21 = Decimal.Zero;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            shadowVeryShortPeriodTotal += num30 - num25;
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
            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num3 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num2 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[equalTrailingIdx - 1];
                        }

                        num = inHigh[equalTrailingIdx - 1] - num3 + (num2 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            equalPeriodTotal += num10 - num5;
            i++;
            shadowVeryShortTrailingIdx++;
            bodyLongTrailingIdx++;
            equalTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0362;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlSeparatingLinesLookback()
        {
            int avgPeriod = Math.Max(
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod),
                Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod
            );

            return avgPeriod + 1;
        }
    }
}
