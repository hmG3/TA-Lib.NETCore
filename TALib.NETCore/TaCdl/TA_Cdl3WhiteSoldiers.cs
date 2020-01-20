using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3WhiteSoldiers(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            var shadowVeryShortPeriodTotal = new double[3];
            var nearPeriodTotal = new double[3];
            var farPeriodTotal = new double[3];
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

            int lookbackTotal = Cdl3WhiteSoldiersLookback();
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

            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int farTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
            double bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num129;
                double num134;
                double num139;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num139 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num138;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num138 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num135;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num136;
                            double num137;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num137 = inClose[i - 2];
                            }
                            else
                            {
                                num137 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num136 = inOpen[i - 2];
                            }
                            else
                            {
                                num136 = inClose[i - 2];
                            }

                            num135 = inHigh[i - 2] - num137 + (num136 - inLow[i - 2]);
                        }
                        else
                        {
                            num135 = 0.0;
                        }

                        num138 = num135;
                    }

                    num139 = num138;
                }

                shadowVeryShortPeriodTotal[2] += num139;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num134 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num133;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num133 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num130;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num131;
                            double num132;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num132 = inClose[i - 1];
                            }
                            else
                            {
                                num132 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num131 = inOpen[i - 1];
                            }
                            else
                            {
                                num131 = inClose[i - 1];
                            }

                            num130 = inHigh[i - 1] - num132 + (num131 - inLow[i - 1]);
                        }
                        else
                        {
                            num130 = 0.0;
                        }

                        num133 = num130;
                    }

                    num134 = num133;
                }

                shadowVeryShortPeriodTotal[1] += num134;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num129 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num128;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num128 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num125;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num126;
                            double num127;
                            if (inClose[i] >= inOpen[i])
                            {
                                num127 = inClose[i];
                            }
                            else
                            {
                                num127 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num126 = inOpen[i];
                            }
                            else
                            {
                                num126 = inClose[i];
                            }

                            num125 = inHigh[i] - num127 + (num126 - inLow[i]);
                        }
                        else
                        {
                            num125 = 0.0;
                        }

                        num128 = num125;
                    }

                    num129 = num128;
                }

                shadowVeryShortPeriodTotal[0] += num129;
                i++;
            }

            i = nearTrailingIdx;
            while (true)
            {
                double num119;
                double num124;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num124 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num123;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num123 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num120;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num121;
                            double num122;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num122 = inClose[i - 2];
                            }
                            else
                            {
                                num122 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num121 = inOpen[i - 2];
                            }
                            else
                            {
                                num121 = inClose[i - 2];
                            }

                            num120 = inHigh[i - 2] - num122 + (num121 - inLow[i - 2]);
                        }
                        else
                        {
                            num120 = 0.0;
                        }

                        num123 = num120;
                    }

                    num124 = num123;
                }

                nearPeriodTotal[2] += num124;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num119 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num118;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num118 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num115;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num116;
                            double num117;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num117 = inClose[i - 1];
                            }
                            else
                            {
                                num117 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num116 = inOpen[i - 1];
                            }
                            else
                            {
                                num116 = inClose[i - 1];
                            }

                            num115 = inHigh[i - 1] - num117 + (num116 - inLow[i - 1]);
                        }
                        else
                        {
                            num115 = 0.0;
                        }

                        num118 = num115;
                    }

                    num119 = num118;
                }

                nearPeriodTotal[1] += num119;
                i++;
            }

            i = farTrailingIdx;
            while (true)
            {
                double num109;
                double num114;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num114 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num113;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num113 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num110;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            double num111;
                            double num112;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num112 = inClose[i - 2];
                            }
                            else
                            {
                                num112 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num111 = inOpen[i - 2];
                            }
                            else
                            {
                                num111 = inClose[i - 2];
                            }

                            num110 = inHigh[i - 2] - num112 + (num111 - inLow[i - 2]);
                        }
                        else
                        {
                            num110 = 0.0;
                        }

                        num113 = num110;
                    }

                    num114 = num113;
                }

                farPeriodTotal[2] += num114;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num108;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num105;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            double num106;
                            double num107;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num107 = inClose[i - 1];
                            }
                            else
                            {
                                num107 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num106 = inOpen[i - 1];
                            }
                            else
                            {
                                num106 = inClose[i - 1];
                            }

                            num105 = inHigh[i - 1] - num107 + (num106 - inLow[i - 1]);
                        }
                        else
                        {
                            num105 = 0.0;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                farPeriodTotal[1] += num109;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                double num104;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num104 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num103;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num103 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num100;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num101;
                            double num102;
                            if (inClose[i] >= inOpen[i])
                            {
                                num102 = inClose[i];
                            }
                            else
                            {
                                num102 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num101 = inOpen[i];
                            }
                            else
                            {
                                num101 = inClose[i];
                            }

                            num100 = inHigh[i] - num102 + (num101 - inLow[i]);
                        }
                        else
                        {
                            num100 = 0.0;
                        }

                        num103 = num100;
                    }

                    num104 = num103;
                }

                bodyShortPeriodTotal += num104;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_08A5:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num98;
                double num99;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num99 = inClose[i - 2];
                }
                else
                {
                    num99 = inOpen[i - 2];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num98 = shadowVeryShortPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    double num97;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num97 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num96;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num96 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num93;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                double num94;
                                double num95;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num95 = inClose[i - 2];
                                }
                                else
                                {
                                    num95 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num94 = inOpen[i - 2];
                                }
                                else
                                {
                                    num94 = inClose[i - 2];
                                }

                                num93 = inHigh[i - 2] - num95 + (num94 - inLow[i - 2]);
                            }
                            else
                            {
                                num93 = 0.0;
                            }

                            num96 = num93;
                        }

                        num97 = num96;
                    }

                    num98 = num97;
                }

                var num92 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (inHigh[i - 2] - num99 < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num98 / num92 &&
                    inClose[i - 1] >= inOpen[i - 1])
                {
                    double num90;
                    double num91;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num91 = inClose[i - 1];
                    }
                    else
                    {
                        num91 = inOpen[i - 1];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num90 = shadowVeryShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        double num89;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num89 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num88;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num88 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num85;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    double num86;
                                    double num87;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num87 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num87 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num86 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num86 = inClose[i - 1];
                                    }

                                    num85 = inHigh[i - 1] - num87 + (num86 - inLow[i - 1]);
                                }
                                else
                                {
                                    num85 = 0.0;
                                }

                                num88 = num85;
                            }

                            num89 = num88;
                        }

                        num90 = num89;
                    }

                    var num84 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (inHigh[i - 1] - num91 < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num90 / num84 &&
                        inClose[i] >= inOpen[i])
                    {
                        double num82;
                        double num83;
                        if (inClose[i] >= inOpen[i])
                        {
                            num83 = inClose[i];
                        }
                        else
                        {
                            num83 = inOpen[i];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num82 = shadowVeryShortPeriodTotal[0] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            double num81;
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

                            num82 = num81;
                        }

                        var num76 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2.0
                            : 1.0;

                        if (inHigh[i] - num83 < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num82 / num76 &&
                            inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] && inOpen[i - 1] > inOpen[i - 2])
                        {
                            double num75;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                            {
                                num75 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                            }
                            else
                            {
                                double num74;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                {
                                    num74 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                }
                                else
                                {
                                    double num73;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                    {
                                        num73 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        double num70;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                        {
                                            double num71;
                                            double num72;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num72 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num72 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num71 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num71 = inClose[i - 2];
                                            }

                                            num70 = inHigh[i - 2] - num72 + (num71 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num70 = 0.0;
                                        }

                                        num73 = num70;
                                    }

                                    num74 = num73;
                                }

                                num75 = num74;
                            }

                            var num69 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                            if (inOpen[i - 1] <=
                                inClose[i - 2] + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num75 / num69 &&
                                inOpen[i] > inOpen[i - 1])
                            {
                                double num68;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                                {
                                    num68 = nearPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                                }
                                else
                                {
                                    double num67;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                    {
                                        num67 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                    }
                                    else
                                    {
                                        double num66;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                        {
                                            num66 = inHigh[i - 1] - inLow[i - 1];
                                        }
                                        else
                                        {
                                            double num63;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                                var num62 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                                if (inOpen[i] <=
                                    inClose[i - 1] + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num68 / num62)
                                {
                                    double num61;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod != 0)
                                    {
                                        num61 = farPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
                                    }
                                    else
                                    {
                                        double num60;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                                        {
                                            num60 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                        }
                                        else
                                        {
                                            double num59;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                                            {
                                                num59 = inHigh[i - 2] - inLow[i - 2];
                                            }
                                            else
                                            {
                                                double num56;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                                                {
                                                    double num57;
                                                    double num58;
                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num58 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num58 = inOpen[i - 2];
                                                    }

                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num57 = inOpen[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num57 = inClose[i - 2];
                                                    }

                                                    num56 = inHigh[i - 2] - num58 + (num57 - inLow[i - 2]);
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

                                    var num55 = Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows
                                        ? 2.0
                                        : 1.0;

                                    if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                                        Math.Abs(inClose[i - 2] - inOpen[i - 2]) -
                                        Globals.CandleSettings[(int) CandleSettingType.Far].Factor * num61 / num55)
                                    {
                                        double num54;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod != 0)
                                        {
                                            num54 = farPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
                                        }
                                        else
                                        {
                                            double num53;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                                            {
                                                num53 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                            }
                                            else
                                            {
                                                double num52;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                                                {
                                                    num52 = inHigh[i - 1] - inLow[i - 1];
                                                }
                                                else
                                                {
                                                    double num49;
                                                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                                                    {
                                                        double num50;
                                                        double num51;
                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num51 = inClose[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num51 = inOpen[i - 1];
                                                        }

                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num50 = inOpen[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num50 = inClose[i - 1];
                                                        }

                                                        num49 = inHigh[i - 1] - num51 + (num50 - inLow[i - 1]);
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

                                        var num48 = Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows
                                            ? 2.0
                                            : 1.0;

                                        if (Math.Abs(inClose[i] - inOpen[i]) >
                                            Math.Abs(inClose[i - 1] - inOpen[i - 1]) -
                                            Globals.CandleSettings[(int) CandleSettingType.Far].Factor * num54 / num48)
                                        {
                                            double num47;
                                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                                            {
                                                num47 = bodyShortPeriodTotal /
                                                        Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                                            }
                                            else
                                            {
                                                double num46;
                                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType ==
                                                    RangeType.RealBody)
                                                {
                                                    num46 = Math.Abs(inClose[i] - inOpen[i]);
                                                }
                                                else
                                                {
                                                    double num45;
                                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType ==
                                                        RangeType.HighLow)
                                                    {
                                                        num45 = inHigh[i] - inLow[i];
                                                    }
                                                    else
                                                    {
                                                        double num42;
                                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType ==
                                                            RangeType.Shadows)
                                                        {
                                                            double num43;
                                                            double num44;
                                                            if (inClose[i] >= inOpen[i])
                                                            {
                                                                num44 = inClose[i];
                                                            }
                                                            else
                                                            {
                                                                num44 = inOpen[i];
                                                            }

                                                            if (inClose[i] >= inOpen[i])
                                                            {
                                                                num43 = inOpen[i];
                                                            }
                                                            else
                                                            {
                                                                num43 = inClose[i];
                                                            }

                                                            num42 = inHigh[i] - num44 + (num43 - inLow[i]);
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

                                            var num41 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType ==
                                                        RangeType.Shadows
                                                ? 2.0
                                                : 1.0;

                                            if (Math.Abs(inClose[i] - inOpen[i]) >
                                                Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num47 / num41)
                                            {
                                                outInteger[outIdx] = 100;
                                                outIdx++;
                                                goto Label_1465;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_1465:
            var totIdx = 2;
            while (totIdx >= 0)
            {
                double num35;
                double num40;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num39;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num36;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num37;
                            double num38;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num38 = inClose[i - totIdx];
                            }
                            else
                            {
                                num38 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num37 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num37 = inClose[i - totIdx];
                            }

                            num36 = inHigh[i - totIdx] - num38 + (num37 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num36 = 0.0;
                        }

                        num39 = num36;
                    }

                    num40 = num39;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs(inClose[shadowVeryShortTrailingIdx - totIdx] - inOpen[shadowVeryShortTrailingIdx - totIdx]);
                }
                else
                {
                    double num34;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[shadowVeryShortTrailingIdx - totIdx] - inLow[shadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num31;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num32;
                            double num33;
                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num33 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num32 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }

                            num31 = inHigh[shadowVeryShortTrailingIdx - totIdx] - num33 +
                                    (num32 - inLow[shadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                shadowVeryShortPeriodTotal[totIdx] += num40 - num35;
                totIdx--;
            }

            for (totIdx = 2; totIdx >= 1; totIdx--)
            {
                double num15;
                double num20;
                double num25;
                double num30;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num29;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            double num27;
                            double num28;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num28 = inClose[i - totIdx];
                            }
                            else
                            {
                                num28 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num27 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num27 = inClose[i - totIdx];
                            }

                            num26 = inHigh[i - totIdx] - num28 + (num27 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num26 = 0.0;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs(inClose[farTrailingIdx - totIdx] - inOpen[farTrailingIdx - totIdx]);
                }
                else
                {
                    double num24;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[farTrailingIdx - totIdx] - inLow[farTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num21;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            double num22;
                            double num23;
                            if (inClose[farTrailingIdx - totIdx] >= inOpen[farTrailingIdx - totIdx])
                            {
                                num23 = inClose[farTrailingIdx - totIdx];
                            }
                            else
                            {
                                num23 = inOpen[farTrailingIdx - totIdx];
                            }

                            if (inClose[farTrailingIdx - totIdx] >= inOpen[farTrailingIdx - totIdx])
                            {
                                num22 = inOpen[farTrailingIdx - totIdx];
                            }
                            else
                            {
                                num22 = inClose[farTrailingIdx - totIdx];
                            }

                            num21 = inHigh[farTrailingIdx - totIdx] - num23 + (num22 - inLow[farTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num21 = 0.0;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                farPeriodTotal[totIdx] += num30 - num25;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num19;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num17;
                            double num18;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num18 = inClose[i - totIdx];
                            }
                            else
                            {
                                num18 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num17 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num17 = inClose[i - totIdx];
                            }

                            num16 = inHigh[i - totIdx] - num18 + (num17 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num16 = 0.0;
                        }

                        num19 = num16;
                    }

                    num20 = num19;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs(inClose[nearTrailingIdx - totIdx] - inOpen[nearTrailingIdx - totIdx]);
                }
                else
                {
                    double num14;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[nearTrailingIdx - totIdx] - inLow[nearTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num11;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num12;
                            double num13;
                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num13 = inClose[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[nearTrailingIdx - totIdx];
                            }

                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num12 = inOpen[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[nearTrailingIdx - totIdx];
                            }

                            num11 = inHigh[nearTrailingIdx - totIdx] - num13 + (num12 - inLow[nearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                nearPeriodTotal[totIdx] += num20 - num15;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyShortTrailingIdx] - inOpen[bodyShortTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyShortTrailingIdx] - inLow[bodyShortTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num3 = inClose[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyShortTrailingIdx];
                        }

                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num2 = inOpen[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyShortTrailingIdx];
                        }

                        num = inHigh[bodyShortTrailingIdx] - num3 + (num2 - inLow[bodyShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyShortPeriodTotal += num10 - num5;
            i++;
            shadowVeryShortTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_08A5;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3WhiteSoldiers(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            var shadowVeryShortPeriodTotal = new decimal[3];
            var nearPeriodTotal = new decimal[3];
            var farPeriodTotal = new decimal[3];
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

            int lookbackTotal = Cdl3WhiteSoldiersLookback();
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

            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int farTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
            decimal bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num129;
                decimal num134;
                decimal num139;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num139 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num138;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num138 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num135;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num136;
                            decimal num137;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num137 = inClose[i - 2];
                            }
                            else
                            {
                                num137 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num136 = inOpen[i - 2];
                            }
                            else
                            {
                                num136 = inClose[i - 2];
                            }

                            num135 = inHigh[i - 2] - num137 + (num136 - inLow[i - 2]);
                        }
                        else
                        {
                            num135 = Decimal.Zero;
                        }

                        num138 = num135;
                    }

                    num139 = num138;
                }

                shadowVeryShortPeriodTotal[2] += num139;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num134 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num133;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num133 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num130;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num131;
                            decimal num132;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num132 = inClose[i - 1];
                            }
                            else
                            {
                                num132 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num131 = inOpen[i - 1];
                            }
                            else
                            {
                                num131 = inClose[i - 1];
                            }

                            num130 = inHigh[i - 1] - num132 + (num131 - inLow[i - 1]);
                        }
                        else
                        {
                            num130 = Decimal.Zero;
                        }

                        num133 = num130;
                    }

                    num134 = num133;
                }

                shadowVeryShortPeriodTotal[1] += num134;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num129 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num128;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num128 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num125;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num126;
                            decimal num127;
                            if (inClose[i] >= inOpen[i])
                            {
                                num127 = inClose[i];
                            }
                            else
                            {
                                num127 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num126 = inOpen[i];
                            }
                            else
                            {
                                num126 = inClose[i];
                            }

                            num125 = inHigh[i] - num127 + (num126 - inLow[i]);
                        }
                        else
                        {
                            num125 = Decimal.Zero;
                        }

                        num128 = num125;
                    }

                    num129 = num128;
                }

                shadowVeryShortPeriodTotal[0] += num129;
                i++;
            }

            i = nearTrailingIdx;
            while (true)
            {
                decimal num119;
                decimal num124;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num124 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num123;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num123 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num120;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num121;
                            decimal num122;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num122 = inClose[i - 2];
                            }
                            else
                            {
                                num122 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num121 = inOpen[i - 2];
                            }
                            else
                            {
                                num121 = inClose[i - 2];
                            }

                            num120 = inHigh[i - 2] - num122 + (num121 - inLow[i - 2]);
                        }
                        else
                        {
                            num120 = Decimal.Zero;
                        }

                        num123 = num120;
                    }

                    num124 = num123;
                }

                nearPeriodTotal[2] += num124;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num119 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num118;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num118 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num115;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num116;
                            decimal num117;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num117 = inClose[i - 1];
                            }
                            else
                            {
                                num117 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num116 = inOpen[i - 1];
                            }
                            else
                            {
                                num116 = inClose[i - 1];
                            }

                            num115 = inHigh[i - 1] - num117 + (num116 - inLow[i - 1]);
                        }
                        else
                        {
                            num115 = Decimal.Zero;
                        }

                        num118 = num115;
                    }

                    num119 = num118;
                }

                nearPeriodTotal[1] += num119;
                i++;
            }

            i = farTrailingIdx;
            while (true)
            {
                decimal num109;
                decimal num114;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num114 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num113;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num113 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num110;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            decimal num111;
                            decimal num112;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num112 = inClose[i - 2];
                            }
                            else
                            {
                                num112 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num111 = inOpen[i - 2];
                            }
                            else
                            {
                                num111 = inClose[i - 2];
                            }

                            num110 = inHigh[i - 2] - num112 + (num111 - inLow[i - 2]);
                        }
                        else
                        {
                            num110 = Decimal.Zero;
                        }

                        num113 = num110;
                    }

                    num114 = num113;
                }

                farPeriodTotal[2] += num114;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num108;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num105;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            decimal num106;
                            decimal num107;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num107 = inClose[i - 1];
                            }
                            else
                            {
                                num107 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num106 = inOpen[i - 1];
                            }
                            else
                            {
                                num106 = inClose[i - 1];
                            }

                            num105 = inHigh[i - 1] - num107 + (num106 - inLow[i - 1]);
                        }
                        else
                        {
                            num105 = Decimal.Zero;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                farPeriodTotal[1] += num109;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num104;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num104 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num103;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num103 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num100;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num101;
                            decimal num102;
                            if (inClose[i] >= inOpen[i])
                            {
                                num102 = inClose[i];
                            }
                            else
                            {
                                num102 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num101 = inOpen[i];
                            }
                            else
                            {
                                num101 = inClose[i];
                            }

                            num100 = inHigh[i] - num102 + (num101 - inLow[i]);
                        }
                        else
                        {
                            num100 = Decimal.Zero;
                        }

                        num103 = num100;
                    }

                    num104 = num103;
                }

                bodyShortPeriodTotal += num104;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0915:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                decimal num98;
                decimal num99;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num99 = inClose[i - 2];
                }
                else
                {
                    num99 = inOpen[i - 2];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num98 = shadowVeryShortPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    decimal num97;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num97 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num96;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num96 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num93;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                decimal num94;
                                decimal num95;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num95 = inClose[i - 2];
                                }
                                else
                                {
                                    num95 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num94 = inOpen[i - 2];
                                }
                                else
                                {
                                    num94 = inClose[i - 2];
                                }

                                num93 = inHigh[i - 2] - num95 + (num94 - inLow[i - 2]);
                            }
                            else
                            {
                                num93 = Decimal.Zero;
                            }

                            num96 = num93;
                        }

                        num97 = num96;
                    }

                    num98 = num97;
                }

                var num92 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (inHigh[i - 2] - num99 <
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num98 / num92 &&
                    inClose[i - 1] >= inOpen[i - 1])
                {
                    decimal num90;
                    decimal num91;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num91 = inClose[i - 1];
                    }
                    else
                    {
                        num91 = inOpen[i - 1];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num90 = shadowVeryShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num89;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num89 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num88;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num88 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num85;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num86;
                                    decimal num87;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num87 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num87 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num86 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num86 = inClose[i - 1];
                                    }

                                    num85 = inHigh[i - 1] - num87 + (num86 - inLow[i - 1]);
                                }
                                else
                                {
                                    num85 = Decimal.Zero;
                                }

                                num88 = num85;
                            }

                            num89 = num88;
                        }

                        num90 = num89;
                    }

                    var num84 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (inHigh[i - 1] - num91 <
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num90 / num84 &&
                        inClose[i] >= inOpen[i])
                    {
                        decimal num82;
                        decimal num83;
                        if (inClose[i] >= inOpen[i])
                        {
                            num83 = inClose[i];
                        }
                        else
                        {
                            num83 = inOpen[i];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num82 = shadowVeryShortPeriodTotal[0] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num81;
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

                            num82 = num81;
                        }

                        var num76 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2m
                            : 1m;

                        if (inHigh[i] - num83 <
                            (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num82 / num76 &&
                            inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2] && inOpen[i - 1] > inOpen[i - 2])
                        {
                            decimal num75;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                            {
                                num75 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                            }
                            else
                            {
                                decimal num74;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                {
                                    num74 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                }
                                else
                                {
                                    decimal num73;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                    {
                                        num73 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        decimal num70;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                        {
                                            decimal num71;
                                            decimal num72;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num72 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num72 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num71 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num71 = inClose[i - 2];
                                            }

                                            num70 = inHigh[i - 2] - num72 + (num71 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num70 = Decimal.Zero;
                                        }

                                        num73 = num70;
                                    }

                                    num74 = num73;
                                }

                                num75 = num74;
                            }

                            var num69 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (inOpen[i - 1] <=
                                inClose[i - 2] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num75 / num69
                                && inOpen[i] > inOpen[i - 1])
                            {
                                decimal num68;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                                {
                                    num68 = nearPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                                }
                                else
                                {
                                    decimal num67;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                    {
                                        num67 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                    }
                                    else
                                    {
                                        decimal num66;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                        {
                                            num66 = inHigh[i - 1] - inLow[i - 1];
                                        }
                                        else
                                        {
                                            decimal num63;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                                var num62 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                                if (inOpen[i] <=
                                    inClose[i - 1] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num68 / num62)
                                {
                                    decimal num61;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod != 0)
                                    {
                                        num61 = farPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
                                    }
                                    else
                                    {
                                        decimal num60;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                                        {
                                            num60 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                        }
                                        else
                                        {
                                            decimal num59;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                                            {
                                                num59 = inHigh[i - 2] - inLow[i - 2];
                                            }
                                            else
                                            {
                                                decimal num56;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                                                {
                                                    decimal num57;
                                                    decimal num58;
                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num58 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num58 = inOpen[i - 2];
                                                    }

                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num57 = inOpen[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num57 = inClose[i - 2];
                                                    }

                                                    num56 = inHigh[i - 2] - num58 + (num57 - inLow[i - 2]);
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

                                    var num55 = Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows
                                        ? 2m
                                        : 1m;

                                    if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                                        Math.Abs(inClose[i - 2] - inOpen[i - 2]) -
                                        (decimal) Globals.CandleSettings[(int) CandleSettingType.Far].Factor * num61 / num55)
                                    {
                                        decimal num54;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod != 0)
                                        {
                                            num54 = farPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
                                        }
                                        else
                                        {
                                            decimal num53;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                                            {
                                                num53 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                            }
                                            else
                                            {
                                                decimal num52;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                                                {
                                                    num52 = inHigh[i - 1] - inLow[i - 1];
                                                }
                                                else
                                                {
                                                    decimal num49;
                                                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                                                    {
                                                        decimal num50;
                                                        decimal num51;
                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num51 = inClose[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num51 = inOpen[i - 1];
                                                        }

                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num50 = inOpen[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num50 = inClose[i - 1];
                                                        }

                                                        num49 = inHigh[i - 1] - num51 + (num50 - inLow[i - 1]);
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

                                        var num48 = Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows
                                            ? 2m
                                            : 1m;

                                        if (Math.Abs(inClose[i] - inOpen[i]) >
                                            Math.Abs(inClose[i - 1] - inOpen[i - 1]) -
                                            (decimal) Globals.CandleSettings[(int) CandleSettingType.Far].Factor * num54 / num48)
                                        {
                                            decimal num47;
                                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                                            {
                                                num47 = bodyShortPeriodTotal /
                                                        Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                                            }
                                            else
                                            {
                                                decimal num46;
                                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType ==
                                                    RangeType.RealBody)
                                                {
                                                    num46 = Math.Abs(inClose[i] - inOpen[i]);
                                                }
                                                else
                                                {
                                                    decimal num45;
                                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType ==
                                                        RangeType.HighLow)
                                                    {
                                                        num45 = inHigh[i] - inLow[i];
                                                    }
                                                    else
                                                    {
                                                        decimal num42;
                                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType ==
                                                            RangeType.Shadows)
                                                        {
                                                            decimal num43;
                                                            decimal num44;
                                                            if (inClose[i] >= inOpen[i])
                                                            {
                                                                num44 = inClose[i];
                                                            }
                                                            else
                                                            {
                                                                num44 = inOpen[i];
                                                            }

                                                            if (inClose[i] >= inOpen[i])
                                                            {
                                                                num43 = inOpen[i];
                                                            }
                                                            else
                                                            {
                                                                num43 = inClose[i];
                                                            }

                                                            num42 = inHigh[i] - num44 + (num43 - inLow[i]);
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

                                            var num41 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType ==
                                                        RangeType.Shadows
                                                ? 2m
                                                : 1m;

                                            if (Math.Abs(inClose[i] - inOpen[i]) >
                                                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num47 / num41)
                                            {
                                                outInteger[outIdx] = 100;
                                                outIdx++;
                                                goto Label_1577;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_1577:
            var totIdx = 2;
            while (totIdx >= 0)
            {
                decimal num35;
                decimal num40;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num39;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num36;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num37;
                            decimal num38;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num38 = inClose[i - totIdx];
                            }
                            else
                            {
                                num38 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num37 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num37 = inClose[i - totIdx];
                            }

                            num36 = inHigh[i - totIdx] - num38 + (num37 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num36 = Decimal.Zero;
                        }

                        num39 = num36;
                    }

                    num40 = num39;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs(inClose[shadowVeryShortTrailingIdx - totIdx] - inOpen[shadowVeryShortTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num34;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[shadowVeryShortTrailingIdx - totIdx] - inLow[shadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num31;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num32;
                            decimal num33;
                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num33 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num32 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }

                            num31 = inHigh[shadowVeryShortTrailingIdx - totIdx] - num33 +
                                    (num32 - inLow[shadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = Decimal.Zero;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                shadowVeryShortPeriodTotal[totIdx] += num40 - num35;
                totIdx--;
            }

            for (totIdx = 2; totIdx >= 1; totIdx--)
            {
                decimal num15;
                decimal num20;
                decimal num25;
                decimal num30;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num29;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            decimal num27;
                            decimal num28;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num28 = inClose[i - totIdx];
                            }
                            else
                            {
                                num28 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num27 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num27 = inClose[i - totIdx];
                            }

                            num26 = inHigh[i - totIdx] - num28 + (num27 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num26 = Decimal.Zero;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs(inClose[farTrailingIdx - totIdx] - inOpen[farTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num24;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[farTrailingIdx - totIdx] - inLow[farTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num21;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            decimal num22;
                            decimal num23;
                            if (inClose[farTrailingIdx - totIdx] >= inOpen[farTrailingIdx - totIdx])
                            {
                                num23 = inClose[farTrailingIdx - totIdx];
                            }
                            else
                            {
                                num23 = inOpen[farTrailingIdx - totIdx];
                            }

                            if (inClose[farTrailingIdx - totIdx] >= inOpen[farTrailingIdx - totIdx])
                            {
                                num22 = inOpen[farTrailingIdx - totIdx];
                            }
                            else
                            {
                                num22 = inClose[farTrailingIdx - totIdx];
                            }

                            num21 = inHigh[farTrailingIdx - totIdx] - num23 + (num22 - inLow[farTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num21 = Decimal.Zero;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                farPeriodTotal[totIdx] += num30 - num25;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num19;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num17;
                            decimal num18;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num18 = inClose[i - totIdx];
                            }
                            else
                            {
                                num18 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num17 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num17 = inClose[i - totIdx];
                            }

                            num16 = inHigh[i - totIdx] - num18 + (num17 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num16 = Decimal.Zero;
                        }

                        num19 = num16;
                    }

                    num20 = num19;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs(inClose[nearTrailingIdx - totIdx] - inOpen[nearTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num14;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[nearTrailingIdx - totIdx] - inLow[nearTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num11;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num12;
                            decimal num13;
                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num13 = inClose[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[nearTrailingIdx - totIdx];
                            }

                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num12 = inOpen[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[nearTrailingIdx - totIdx];
                            }

                            num11 = inHigh[nearTrailingIdx - totIdx] - num13 + (num12 - inLow[nearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = Decimal.Zero;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                nearPeriodTotal[totIdx] += num20 - num15;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyShortTrailingIdx] - inOpen[bodyShortTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyShortTrailingIdx] - inLow[bodyShortTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num3 = inClose[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyShortTrailingIdx];
                        }

                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num2 = inOpen[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyShortTrailingIdx];
                        }

                        num = inHigh[bodyShortTrailingIdx] - num3 + (num2 - inLow[bodyShortTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyShortPeriodTotal += num10 - num5;
            i++;
            shadowVeryShortTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0915;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl3WhiteSoldiersLookback()
        {
            int avgPeriod = Math.Max(
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod),
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod)
            );

            return avgPeriod + 2;
        }
    }
}
