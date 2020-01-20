using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlHangingMan(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            double num35;
            double num40;
            double num71;
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

            int lookbackTotal = CdlHangingManLookback();
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

            double bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            double shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            double nearPeriodTotal = default;
            int nearTrailingIdx = startIdx - 1 - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                double num91;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num91 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num90;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num90 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num87;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num88;
                            double num89;
                            if (inClose[i] >= inOpen[i])
                            {
                                num89 = inClose[i];
                            }
                            else
                            {
                                num89 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num88 = inOpen[i];
                            }
                            else
                            {
                                num88 = inClose[i];
                            }

                            num87 = inHigh[i] - num89 + (num88 - inLow[i]);
                        }
                        else
                        {
                            num87 = 0.0;
                        }

                        num90 = num87;
                    }

                    num91 = num90;
                }

                bodyPeriodTotal += num91;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                double num86;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num86 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num85;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num85 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num82;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            double num83;
                            double num84;
                            if (inClose[i] >= inOpen[i])
                            {
                                num84 = inClose[i];
                            }
                            else
                            {
                                num84 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num83 = inOpen[i];
                            }
                            else
                            {
                                num83 = inClose[i];
                            }

                            num82 = inHigh[i] - num84 + (num83 - inLow[i]);
                        }
                        else
                        {
                            num82 = 0.0;
                        }

                        num85 = num82;
                    }

                    num86 = num85;
                }

                shadowLongPeriodTotal += num86;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num81;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num81 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num80;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num80 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num77;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num78;
                            double num79;
                            if (inClose[i] >= inOpen[i])
                            {
                                num79 = inClose[i];
                            }
                            else
                            {
                                num79 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num78 = inOpen[i];
                            }
                            else
                            {
                                num78 = inClose[i];
                            }

                            num77 = inHigh[i] - num79 + (num78 - inLow[i]);
                        }
                        else
                        {
                            num77 = 0.0;
                        }

                        num80 = num77;
                    }

                    num81 = num80;
                }

                shadowVeryShortPeriodTotal += num81;
                i++;
            }

            i = nearTrailingIdx;
            while (true)
            {
                double num76;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num76 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num75;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num75 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num72;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num73;
                            double num74;
                            if (inClose[i] >= inOpen[i])
                            {
                                num74 = inClose[i];
                            }
                            else
                            {
                                num74 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num73 = inOpen[i];
                            }
                            else
                            {
                                num73 = inClose[i];
                            }

                            num72 = inHigh[i] - num74 + (num73 - inLow[i]);
                        }
                        else
                        {
                            num72 = 0.0;
                        }

                        num75 = num72;
                    }

                    num76 = num75;
                }

                nearPeriodTotal += num76;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_03FF:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
            {
                num71 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            }
            else
            {
                double num70;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num69;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num66;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num67;
                            double num68;
                            if (inClose[i] >= inOpen[i])
                            {
                                num68 = inClose[i];
                            }
                            else
                            {
                                num68 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num67 = inOpen[i];
                            }
                            else
                            {
                                num67 = inClose[i];
                            }

                            num66 = inHigh[i] - num68 + (num67 - inLow[i]);
                        }
                        else
                        {
                            num66 = 0.0;
                        }

                        num69 = num66;
                    }

                    num70 = num69;
                }

                num71 = num70;
            }

            var num65 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) < Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num71 / num65)
            {
                double num63;
                double num64;
                if (inClose[i] >= inOpen[i])
                {
                    num64 = inOpen[i];
                }
                else
                {
                    num64 = inClose[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                {
                    num63 = shadowLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                }
                else
                {
                    double num62;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                    {
                        num62 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num61;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                        {
                            num61 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num58;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                            {
                                double num59;
                                double num60;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num60 = inClose[i];
                                }
                                else
                                {
                                    num60 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num59 = inOpen[i];
                                }
                                else
                                {
                                    num59 = inClose[i];
                                }

                                num58 = inHigh[i] - num60 + (num59 - inLow[i]);
                            }
                            else
                            {
                                num58 = 0.0;
                            }

                            num61 = num58;
                        }

                        num62 = num61;
                    }

                    num63 = num62;
                }

                var num57 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (num64 - inLow[i] > Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num63 / num57)
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

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num55 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        double num54;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num54 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num53;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num53 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num50;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                        num55 = num54;
                    }

                    var num49 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (inHigh[i] - num56 < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num55 / num49)
                    {
                        double num47;
                        double num48;
                        if (inClose[i] < inOpen[i])
                        {
                            num48 = inClose[i];
                        }
                        else
                        {
                            num48 = inOpen[i];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                        {
                            num47 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                        }
                        else
                        {
                            double num46;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                            {
                                num46 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                double num45;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                {
                                    num45 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num42;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                    {
                                        double num43;
                                        double num44;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num44 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num44 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num43 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num43 = inClose[i - 1];
                                        }

                                        num42 = inHigh[i - 1] - num44 + (num43 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num42 = 0.0;
                                    }

                                    num45 = num42;
                                }

                                num46 = num45;
                            }

                            num47 = num46;
                        }

                        var num41 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                        if (num48 >= inHigh[i - 1] - Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num47 / num41)
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_095E;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_095E:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num40 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num39;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num39 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num36;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        double num37;
                        double num38;
                        if (inClose[i] >= inOpen[i])
                        {
                            num38 = inClose[i];
                        }
                        else
                        {
                            num38 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num37 = inOpen[i];
                        }
                        else
                        {
                            num37 = inClose[i];
                        }

                        num36 = inHigh[i] - num38 + (num37 - inLow[i]);
                    }
                    else
                    {
                        num36 = 0.0;
                    }

                    num39 = num36;
                }

                num40 = num39;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num35 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                double num34;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num34 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    double num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        double num32;
                        double num33;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num33 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num33 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num32 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num32 = inClose[bodyTrailingIdx];
                        }

                        num31 = inHigh[bodyTrailingIdx] - num33 + (num32 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num31 = 0.0;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            bodyPeriodTotal += num40 - num35;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num29;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[shadowLongTrailingIdx] - inOpen[shadowLongTrailingIdx]);
            }
            else
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[shadowLongTrailingIdx] - inLow[shadowLongTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num23 = inClose[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[shadowLongTrailingIdx];
                        }

                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num22 = inOpen[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[shadowLongTrailingIdx];
                        }

                        num21 = inHigh[shadowLongTrailingIdx] - num23 + (num22 - inLow[shadowLongTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            shadowLongPeriodTotal += num30 - num25;
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
            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[nearTrailingIdx] - inOpen[nearTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[nearTrailingIdx] - inLow[nearTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[nearTrailingIdx] >= inOpen[nearTrailingIdx])
                        {
                            num3 = inClose[nearTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[nearTrailingIdx];
                        }

                        if (inClose[nearTrailingIdx] >= inOpen[nearTrailingIdx])
                        {
                            num2 = inOpen[nearTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[nearTrailingIdx];
                        }

                        num = inHigh[nearTrailingIdx] - num3 + (num2 - inLow[nearTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            nearPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            nearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_03FF;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlHangingMan(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num25;
            decimal num30;
            decimal num35;
            decimal num40;
            decimal num71;
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

            int lookbackTotal = CdlHangingManLookback();
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

            decimal bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            decimal shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            decimal nearPeriodTotal = default;
            int nearTrailingIdx = startIdx - 1 - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                decimal num91;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num91 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num90;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num90 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num87;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num88;
                            decimal num89;
                            if (inClose[i] >= inOpen[i])
                            {
                                num89 = inClose[i];
                            }
                            else
                            {
                                num89 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num88 = inOpen[i];
                            }
                            else
                            {
                                num88 = inClose[i];
                            }

                            num87 = inHigh[i] - num89 + (num88 - inLow[i]);
                        }
                        else
                        {
                            num87 = Decimal.Zero;
                        }

                        num90 = num87;
                    }

                    num91 = num90;
                }

                bodyPeriodTotal += num91;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                decimal num86;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num86 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num85;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num85 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num82;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            decimal num83;
                            decimal num84;
                            if (inClose[i] >= inOpen[i])
                            {
                                num84 = inClose[i];
                            }
                            else
                            {
                                num84 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num83 = inOpen[i];
                            }
                            else
                            {
                                num83 = inClose[i];
                            }

                            num82 = inHigh[i] - num84 + (num83 - inLow[i]);
                        }
                        else
                        {
                            num82 = Decimal.Zero;
                        }

                        num85 = num82;
                    }

                    num86 = num85;
                }

                shadowLongPeriodTotal += num86;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num81;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num81 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num80;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num80 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num77;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num78;
                            decimal num79;
                            if (inClose[i] >= inOpen[i])
                            {
                                num79 = inClose[i];
                            }
                            else
                            {
                                num79 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num78 = inOpen[i];
                            }
                            else
                            {
                                num78 = inClose[i];
                            }

                            num77 = inHigh[i] - num79 + (num78 - inLow[i]);
                        }
                        else
                        {
                            num77 = Decimal.Zero;
                        }

                        num80 = num77;
                    }

                    num81 = num80;
                }

                shadowVeryShortPeriodTotal += num81;
                i++;
            }

            i = nearTrailingIdx;
            while (true)
            {
                decimal num76;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num76 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num75;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num75 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num72;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num73;
                            decimal num74;
                            if (inClose[i] >= inOpen[i])
                            {
                                num74 = inClose[i];
                            }
                            else
                            {
                                num74 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num73 = inOpen[i];
                            }
                            else
                            {
                                num73 = inClose[i];
                            }

                            num72 = inHigh[i] - num74 + (num73 - inLow[i]);
                        }
                        else
                        {
                            num72 = Decimal.Zero;
                        }

                        num75 = num72;
                    }

                    num76 = num75;
                }

                nearPeriodTotal += num76;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0437:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
            {
                num71 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            }
            else
            {
                decimal num70;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num69;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num66;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num67;
                            decimal num68;
                            if (inClose[i] >= inOpen[i])
                            {
                                num68 = inClose[i];
                            }
                            else
                            {
                                num68 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num67 = inOpen[i];
                            }
                            else
                            {
                                num67 = inClose[i];
                            }

                            num66 = inHigh[i] - num68 + (num67 - inLow[i]);
                        }
                        else
                        {
                            num66 = Decimal.Zero;
                        }

                        num69 = num66;
                    }

                    num70 = num69;
                }

                num71 = num70;
            }

            var num65 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i] - inOpen[i]) <
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num71 / num65)
            {
                decimal num63;
                decimal num64;
                if (inClose[i] >= inOpen[i])
                {
                    num64 = inOpen[i];
                }
                else
                {
                    num64 = inClose[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                {
                    num63 = shadowLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                }
                else
                {
                    decimal num62;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                    {
                        num62 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        decimal num61;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                        {
                            num61 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            decimal num58;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                            {
                                decimal num59;
                                decimal num60;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num60 = inClose[i];
                                }
                                else
                                {
                                    num60 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num59 = inOpen[i];
                                }
                                else
                                {
                                    num59 = inClose[i];
                                }

                                num58 = inHigh[i] - num60 + (num59 - inLow[i]);
                            }
                            else
                            {
                                num58 = Decimal.Zero;
                            }

                            num61 = num58;
                        }

                        num62 = num61;
                    }

                    num63 = num62;
                }

                var num57 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (num64 - inLow[i] > (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num63 / num57)
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

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num55 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num54;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num54 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num53;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num53 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num50;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                        num55 = num54;
                    }

                    var num49 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (inHigh[i] - num56 <
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num55 / num49)
                    {
                        decimal num47;
                        decimal num48;
                        if (inClose[i] < inOpen[i])
                        {
                            num48 = inClose[i];
                        }
                        else
                        {
                            num48 = inOpen[i];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                        {
                            num47 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                        }
                        else
                        {
                            decimal num46;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                            {
                                num46 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                decimal num45;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                {
                                    num45 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    decimal num42;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                    {
                                        decimal num43;
                                        decimal num44;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num44 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num44 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num43 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num43 = inClose[i - 1];
                                        }

                                        num42 = inHigh[i - 1] - num44 + (num43 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num42 = Decimal.Zero;
                                    }

                                    num45 = num42;
                                }

                                num46 = num45;
                            }

                            num47 = num46;
                        }

                        var num41 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                        if (num48 >= inHigh[i - 1] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num47 / num41)
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_09E0;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_09E0:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num40 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num39;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num39 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num36;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num37;
                        decimal num38;
                        if (inClose[i] >= inOpen[i])
                        {
                            num38 = inClose[i];
                        }
                        else
                        {
                            num38 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num37 = inOpen[i];
                        }
                        else
                        {
                            num37 = inClose[i];
                        }

                        num36 = inHigh[i] - num38 + (num37 - inLow[i]);
                    }
                    else
                    {
                        num36 = Decimal.Zero;
                    }

                    num39 = num36;
                }

                num40 = num39;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num35 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                decimal num34;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num34 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    decimal num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num32;
                        decimal num33;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num33 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num33 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num32 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num32 = inClose[bodyTrailingIdx];
                        }

                        num31 = inHigh[bodyTrailingIdx] - num33 + (num32 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num31 = Decimal.Zero;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            bodyPeriodTotal += num40 - num35;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num29;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[shadowLongTrailingIdx] - inOpen[shadowLongTrailingIdx]);
            }
            else
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[shadowLongTrailingIdx] - inLow[shadowLongTrailingIdx];
                }
                else
                {
                    decimal num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                    {
                        decimal num22;
                        decimal num23;
                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num23 = inClose[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[shadowLongTrailingIdx];
                        }

                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num22 = inOpen[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[shadowLongTrailingIdx];
                        }

                        num21 = inHigh[shadowLongTrailingIdx] - num23 + (num22 - inLow[shadowLongTrailingIdx]);
                    }
                    else
                    {
                        num21 = Decimal.Zero;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            shadowLongPeriodTotal += num30 - num25;
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
            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[nearTrailingIdx] - inOpen[nearTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[nearTrailingIdx] - inLow[nearTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[nearTrailingIdx] >= inOpen[nearTrailingIdx])
                        {
                            num3 = inClose[nearTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[nearTrailingIdx];
                        }

                        if (inClose[nearTrailingIdx] >= inOpen[nearTrailingIdx])
                        {
                            num2 = inOpen[nearTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[nearTrailingIdx];
                        }

                        num = inHigh[nearTrailingIdx] - num3 + (num2 - inLow[nearTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            nearPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            nearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0437;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlHangingManLookback()
        {
            int avgPeriod = Math.Max(
                Math.Max(
                    Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod,
                        Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod),
                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod),
                Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod
            );

            return avgPeriod + 1;
        }
    }
}
