using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlAdvanceBlock(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num81;
            double num95;
            double num102;
            double num103;
            double num110;
            double num117;
            double num124;
            var shadowShortPeriodTotal = new double[3];
            var shadowLongPeriodTotal = new double[2];
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

            int lookbackTotal = CdlAdvanceBlockLookback();
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

            int shadowShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
            int shadowLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int farTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
            double bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = shadowShortTrailingIdx;
            while (true)
            {
                double num164;
                double num169;
                double num174;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num174 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num173;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num173 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num170;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            double num171;
                            double num172;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num172 = inClose[i - 2];
                            }
                            else
                            {
                                num172 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num171 = inOpen[i - 2];
                            }
                            else
                            {
                                num171 = inClose[i - 2];
                            }

                            num170 = inHigh[i - 2] - num172 + (num171 - inLow[i - 2]);
                        }
                        else
                        {
                            num170 = 0.0;
                        }

                        num173 = num170;
                    }

                    num174 = num173;
                }

                shadowShortPeriodTotal[2] += num174;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num169 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num168;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num168 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num165;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            double num166;
                            double num167;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num167 = inClose[i - 1];
                            }
                            else
                            {
                                num167 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num166 = inOpen[i - 1];
                            }
                            else
                            {
                                num166 = inClose[i - 1];
                            }

                            num165 = inHigh[i - 1] - num167 + (num166 - inLow[i - 1]);
                        }
                        else
                        {
                            num165 = 0.0;
                        }

                        num168 = num165;
                    }

                    num169 = num168;
                }

                shadowShortPeriodTotal[1] += num169;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num164 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num163;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num163 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num160;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            double num161;
                            double num162;
                            if (inClose[i] >= inOpen[i])
                            {
                                num162 = inClose[i];
                            }
                            else
                            {
                                num162 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num161 = inOpen[i];
                            }
                            else
                            {
                                num161 = inClose[i];
                            }

                            num160 = inHigh[i] - num162 + (num161 - inLow[i]);
                        }
                        else
                        {
                            num160 = 0.0;
                        }

                        num163 = num160;
                    }

                    num164 = num163;
                }

                shadowShortPeriodTotal[0] += num164;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                double num154;
                double num159;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num159 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num158;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num158 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num155;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            double num156;
                            double num157;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num157 = inClose[i - 1];
                            }
                            else
                            {
                                num157 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num156 = inOpen[i - 1];
                            }
                            else
                            {
                                num156 = inClose[i - 1];
                            }

                            num155 = inHigh[i - 1] - num157 + (num156 - inLow[i - 1]);
                        }
                        else
                        {
                            num155 = 0.0;
                        }

                        num158 = num155;
                    }

                    num159 = num158;
                }

                shadowLongPeriodTotal[1] += num159;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num154 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num153;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num153 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num150;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            double num151;
                            double num152;
                            if (inClose[i] >= inOpen[i])
                            {
                                num152 = inClose[i];
                            }
                            else
                            {
                                num152 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num151 = inOpen[i];
                            }
                            else
                            {
                                num151 = inClose[i];
                            }

                            num150 = inHigh[i] - num152 + (num151 - inLow[i]);
                        }
                        else
                        {
                            num150 = 0.0;
                        }

                        num153 = num150;
                    }

                    num154 = num153;
                }

                shadowLongPeriodTotal[0] += num154;
                i++;
            }

            i = nearTrailingIdx;
            while (true)
            {
                double num144;
                double num149;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num149 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num148;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num148 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num145;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num146;
                            double num147;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num147 = inClose[i - 2];
                            }
                            else
                            {
                                num147 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num146 = inOpen[i - 2];
                            }
                            else
                            {
                                num146 = inClose[i - 2];
                            }

                            num145 = inHigh[i - 2] - num147 + (num146 - inLow[i - 2]);
                        }
                        else
                        {
                            num145 = 0.0;
                        }

                        num148 = num145;
                    }

                    num149 = num148;
                }

                nearPeriodTotal[2] += num149;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num144 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num143;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num143 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num140;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num141;
                            double num142;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num142 = inClose[i - 1];
                            }
                            else
                            {
                                num142 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num141 = inOpen[i - 1];
                            }
                            else
                            {
                                num141 = inClose[i - 1];
                            }

                            num140 = inHigh[i - 1] - num142 + (num141 - inLow[i - 1]);
                        }
                        else
                        {
                            num140 = 0.0;
                        }

                        num143 = num140;
                    }

                    num144 = num143;
                }

                nearPeriodTotal[1] += num144;
                i++;
            }

            i = farTrailingIdx;
            while (true)
            {
                double num134;
                double num139;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num139 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num138;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num138 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num135;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
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

                farPeriodTotal[2] += num139;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num134 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num133;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num133 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num130;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
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

                farPeriodTotal[1] += num134;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                double num129;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num129 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num128;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num128 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num125;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num126;
                            double num127;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num127 = inClose[i - 2];
                            }
                            else
                            {
                                num127 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num126 = inOpen[i - 2];
                            }
                            else
                            {
                                num126 = inClose[i - 2];
                            }

                            num125 = inHigh[i - 2] - num127 + (num126 - inLow[i - 2]);
                        }
                        else
                        {
                            num125 = 0.0;
                        }

                        num128 = num125;
                    }

                    num129 = num128;
                }

                bodyLongPeriodTotal += num129;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0B40:
            if (inClose[i - 2] < inOpen[i - 2] || inClose[i - 1] < inOpen[i - 1] || inClose[i] < inOpen[i] ||
                inClose[i] <= inClose[i - 1] || inClose[i - 1] <= inClose[i - 2] || inOpen[i - 1] <= inOpen[i - 2])
            {
                goto Label_1A80;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
            {
                num124 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            }
            else
            {
                double num123;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num123 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num122;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num122 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num119;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num120;
                            double num121;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num121 = inClose[i - 2];
                            }
                            else
                            {
                                num121 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num120 = inOpen[i - 2];
                            }
                            else
                            {
                                num120 = inClose[i - 2];
                            }

                            num119 = inHigh[i - 2] - num121 + (num120 - inLow[i - 2]);
                        }
                        else
                        {
                            num119 = 0.0;
                        }

                        num122 = num119;
                    }

                    num123 = num122;
                }

                num124 = num123;
            }

            var num118 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inOpen[i - 1] > inClose[i - 2] + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num124 / num118 ||
                inOpen[i] <= inOpen[i - 1])
            {
                goto Label_1A80;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
            {
                num117 = nearPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            }
            else
            {
                double num116;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num116 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num115;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num115 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num112;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num113;
                            double num114;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num114 = inClose[i - 1];
                            }
                            else
                            {
                                num114 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num113 = inOpen[i - 1];
                            }
                            else
                            {
                                num113 = inClose[i - 1];
                            }

                            num112 = inHigh[i - 1] - num114 + (num113 - inLow[i - 1]);
                        }
                        else
                        {
                            num112 = 0.0;
                        }

                        num115 = num112;
                    }

                    num116 = num115;
                }

                num117 = num116;
            }

            var num111 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inOpen[i] > inClose[i - 1] + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num117 / num111)
            {
                goto Label_1A80;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num110 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num109;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num108;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num105;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num106;
                            double num107;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num107 = inClose[i - 2];
                            }
                            else
                            {
                                num107 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num106 = inOpen[i - 2];
                            }
                            else
                            {
                                num106 = inClose[i - 2];
                            }

                            num105 = inHigh[i - 2] - num107 + (num106 - inLow[i - 2]);
                        }
                        else
                        {
                            num105 = 0.0;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                num110 = num109;
            }

            var num104 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) <=
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num110 / num104)
            {
                goto Label_1A80;
            }

            if (inClose[i - 2] >= inOpen[i - 2])
            {
                num103 = inClose[i - 2];
            }
            else
            {
                num103 = inOpen[i - 2];
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
            {
                num102 = shadowShortPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
            }
            else
            {
                double num101;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num101 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num100;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num100 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num97;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
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

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num98 = inOpen[i - 2];
                            }
                            else
                            {
                                num98 = inClose[i - 2];
                            }

                            num97 = inHigh[i - 2] - num99 + (num98 - inLow[i - 2]);
                        }
                        else
                        {
                            num97 = 0.0;
                        }

                        num100 = num97;
                    }

                    num101 = num100;
                }

                num102 = num101;
            }

            var num96 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inHigh[i - 2] - num103 >= Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num102 / num96)
            {
                goto Label_1A80;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod != 0)
            {
                num95 = farPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
            }
            else
            {
                double num94;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num94 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num93;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num93 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num90;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            double num91;
                            double num92;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num92 = inClose[i - 2];
                            }
                            else
                            {
                                num92 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num91 = inOpen[i - 2];
                            }
                            else
                            {
                                num91 = inClose[i - 2];
                            }

                            num90 = inHigh[i - 2] - num92 + (num91 - inLow[i - 2]);
                        }
                        else
                        {
                            num90 = 0.0;
                        }

                        num93 = num90;
                    }

                    num94 = num93;
                }

                num95 = num94;
            }

            var num89 = Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <
                Math.Abs(inClose[i - 2] - inOpen[i - 2]) - Globals.CandleSettings[(int) CandleSettingType.Far].Factor * num95 / num89)
            {
                double num88;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                {
                    num88 = nearPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                }
                else
                {
                    double num87;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                    {
                        num87 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num86;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                        {
                            num86 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num83;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                            {
                                double num84;
                                double num85;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num85 = inClose[i - 1];
                                }
                                else
                                {
                                    num85 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num84 = inOpen[i - 1];
                                }
                                else
                                {
                                    num84 = inClose[i - 1];
                                }

                                num83 = inHigh[i - 1] - num85 + (num84 - inLow[i - 1]);
                            }
                            else
                            {
                                num83 = 0.0;
                            }

                            num86 = num83;
                        }

                        num87 = num86;
                    }

                    num88 = num87;
                }

                var num82 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i] - inOpen[i]) <
                    Math.Abs(inClose[i - 1] - inOpen[i - 1]) + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num88 / num82)
                {
                    goto Label_1A71;
                }
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod != 0)
            {
                num81 = farPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
            }
            else
            {
                double num80;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num80 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num79;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num79 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num76;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            double num77;
                            double num78;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num78 = inClose[i - 1];
                            }
                            else
                            {
                                num78 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num77 = inOpen[i - 1];
                            }
                            else
                            {
                                num77 = inClose[i - 1];
                            }

                            num76 = inHigh[i - 1] - num78 + (num77 - inLow[i - 1]);
                        }
                        else
                        {
                            num76 = 0.0;
                        }

                        num79 = num76;
                    }

                    num80 = num79;
                }

                num81 = num80;
            }

            var num75 = Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) >=
                Math.Abs(inClose[i - 1] - inOpen[i - 1]) - Globals.CandleSettings[(int) CandleSettingType.Far].Factor * num81 / num75)
            {
                double num57;
                double num58;
                if (Math.Abs(inClose[i] - inOpen[i]) < Math.Abs(inClose[i - 1] - inOpen[i - 1]) &&
                    Math.Abs(inClose[i - 1] - inOpen[i - 1]) < Math.Abs(inClose[i - 2] - inOpen[i - 2]))
                {
                    double num65;
                    double num66;
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

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
                    {
                        num73 = shadowShortPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
                    }
                    else
                    {
                        double num72;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                        {
                            num72 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num71;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                            {
                                num71 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num68;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                                {
                                    double num69;
                                    double num70;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num70 = inClose[i];
                                    }
                                    else
                                    {
                                        num70 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num69 = inOpen[i];
                                    }
                                    else
                                    {
                                        num69 = inClose[i];
                                    }

                                    num68 = inHigh[i] - num70 + (num69 - inLow[i]);
                                }
                                else
                                {
                                    num68 = 0.0;
                                }

                                num71 = num68;
                            }

                            num72 = num71;
                        }

                        num73 = num72;
                    }

                    var num67 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (inHigh[i] - num74 > Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num73 / num67)
                    {
                        goto Label_1A71;
                    }

                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num66 = inClose[i - 1];
                    }
                    else
                    {
                        num66 = inOpen[i - 1];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
                    {
                        num65 = shadowShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
                    }
                    else
                    {
                        double num64;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                        {
                            num64 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num63;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                            {
                                num63 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num60;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                                {
                                    double num61;
                                    double num62;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num62 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num62 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num61 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num61 = inClose[i - 1];
                                    }

                                    num60 = inHigh[i - 1] - num62 + (num61 - inLow[i - 1]);
                                }
                                else
                                {
                                    num60 = 0.0;
                                }

                                num63 = num60;
                            }

                            num64 = num63;
                        }

                        num65 = num64;
                    }

                    var num59 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (inHigh[i - 1] - num66 > Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num65 / num59)
                    {
                        goto Label_1A71;
                    }
                }

                if (Math.Abs(inClose[i] - inOpen[i]) >= Math.Abs(inClose[i - 1] - inOpen[i - 1]))
                {
                    goto Label_1A80;
                }

                if (inClose[i] >= inOpen[i])
                {
                    num58 = inClose[i];
                }
                else
                {
                    num58 = inOpen[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                {
                    num57 = shadowLongPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                }
                else
                {
                    double num56;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                    {
                        num56 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num55;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                        {
                            num55 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num52;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                            {
                                double num53;
                                double num54;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num54 = inClose[i];
                                }
                                else
                                {
                                    num54 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num53 = inOpen[i];
                                }
                                else
                                {
                                    num53 = inClose[i];
                                }

                                num52 = inHigh[i] - num54 + (num53 - inLow[i]);
                            }
                            else
                            {
                                num52 = 0.0;
                            }

                            num55 = num52;
                        }

                        num56 = num55;
                    }

                    num57 = num56;
                }

                var num51 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (inHigh[i] - num58 <= Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num57 / num51)
                {
                    goto Label_1A80;
                }
            }

            Label_1A71:
            outInteger[outIdx] = -100;
            outIdx++;
            goto Label_1A8C;
            Label_1A80:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_1A8C:
            var totIdx = 2;
            while (totIdx >= 0)
            {
                double num45;
                double num50;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num50 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num49;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num49 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num46;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            double num47;
                            double num48;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num48 = inClose[i - totIdx];
                            }
                            else
                            {
                                num48 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num47 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num47 = inClose[i - totIdx];
                            }

                            num46 = inHigh[i - totIdx] - num48 + (num47 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num46 = 0.0;
                        }

                        num49 = num46;
                    }

                    num50 = num49;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num45 = Math.Abs(inClose[shadowShortTrailingIdx - totIdx] - inOpen[shadowShortTrailingIdx - totIdx]);
                }
                else
                {
                    double num44;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num44 = inHigh[shadowShortTrailingIdx - totIdx] - inLow[shadowShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num41;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            double num42;
                            double num43;
                            if (inClose[shadowShortTrailingIdx - totIdx] >= inOpen[shadowShortTrailingIdx - totIdx])
                            {
                                num43 = inClose[shadowShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num43 = inOpen[shadowShortTrailingIdx - totIdx];
                            }

                            if (inClose[shadowShortTrailingIdx - totIdx] >= inOpen[shadowShortTrailingIdx - totIdx])
                            {
                                num42 = inOpen[shadowShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num42 = inClose[shadowShortTrailingIdx - totIdx];
                            }

                            num41 = inHigh[shadowShortTrailingIdx - totIdx] - num43 + (num42 - inLow[shadowShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num41 = 0.0;
                        }

                        num44 = num41;
                    }

                    num45 = num44;
                }

                shadowShortPeriodTotal[totIdx] += num50 - num45;
                totIdx--;
            }

            for (totIdx = 1; totIdx >= 0; totIdx--)
            {
                double num35;
                double num40;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num39;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num36;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs(inClose[shadowLongTrailingIdx - totIdx] - inOpen[shadowLongTrailingIdx - totIdx]);
                }
                else
                {
                    double num34;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[shadowLongTrailingIdx - totIdx] - inLow[shadowLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num31;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            double num32;
                            double num33;
                            if (inClose[shadowLongTrailingIdx - totIdx] >= inOpen[shadowLongTrailingIdx - totIdx])
                            {
                                num33 = inClose[shadowLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[shadowLongTrailingIdx - totIdx];
                            }

                            if (inClose[shadowLongTrailingIdx - totIdx] >= inOpen[shadowLongTrailingIdx - totIdx])
                            {
                                num32 = inOpen[shadowLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[shadowLongTrailingIdx - totIdx];
                            }

                            num31 = inHigh[shadowLongTrailingIdx - totIdx] - num33 + (num32 - inLow[shadowLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                shadowLongPeriodTotal[totIdx] += num40 - num35;
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
                num5 = Math.Abs(inClose[bodyLongTrailingIdx - 2] - inOpen[bodyLongTrailingIdx - 2]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx - 2] - inLow[bodyLongTrailingIdx - 2];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyLongTrailingIdx - 2] >= inOpen[bodyLongTrailingIdx - 2])
                        {
                            num3 = inClose[bodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx - 2];
                        }

                        if (inClose[bodyLongTrailingIdx - 2] >= inOpen[bodyLongTrailingIdx - 2])
                        {
                            num2 = inOpen[bodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx - 2];
                        }

                        num = inHigh[bodyLongTrailingIdx - 2] - num3 + (num2 - inLow[bodyLongTrailingIdx - 2]);
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
            shadowShortTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0B40;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlAdvanceBlock(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num81;
            decimal num95;
            decimal num102;
            decimal num103;
            decimal num110;
            decimal num117;
            decimal num124;
            var shadowShortPeriodTotal = new decimal[3];
            var shadowLongPeriodTotal = new decimal[2];
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

            int lookbackTotal = CdlAdvanceBlockLookback();
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

            int shadowShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
            int shadowLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int farTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
            decimal bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = shadowShortTrailingIdx;
            while (true)
            {
                decimal num164;
                decimal num169;
                decimal num174;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num174 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num173;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num173 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num170;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            decimal num171;
                            decimal num172;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num172 = inClose[i - 2];
                            }
                            else
                            {
                                num172 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num171 = inOpen[i - 2];
                            }
                            else
                            {
                                num171 = inClose[i - 2];
                            }

                            num170 = inHigh[i - 2] - num172 + (num171 - inLow[i - 2]);
                        }
                        else
                        {
                            num170 = Decimal.Zero;
                        }

                        num173 = num170;
                    }

                    num174 = num173;
                }

                shadowShortPeriodTotal[2] += num174;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num169 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num168;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num168 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num165;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            decimal num166;
                            decimal num167;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num167 = inClose[i - 1];
                            }
                            else
                            {
                                num167 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num166 = inOpen[i - 1];
                            }
                            else
                            {
                                num166 = inClose[i - 1];
                            }

                            num165 = inHigh[i - 1] - num167 + (num166 - inLow[i - 1]);
                        }
                        else
                        {
                            num165 = Decimal.Zero;
                        }

                        num168 = num165;
                    }

                    num169 = num168;
                }

                shadowShortPeriodTotal[1] += num169;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num164 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num163;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num163 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num160;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            decimal num161;
                            decimal num162;
                            if (inClose[i] >= inOpen[i])
                            {
                                num162 = inClose[i];
                            }
                            else
                            {
                                num162 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num161 = inOpen[i];
                            }
                            else
                            {
                                num161 = inClose[i];
                            }

                            num160 = inHigh[i] - num162 + (num161 - inLow[i]);
                        }
                        else
                        {
                            num160 = Decimal.Zero;
                        }

                        num163 = num160;
                    }

                    num164 = num163;
                }

                shadowShortPeriodTotal[0] += num164;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                decimal num154;
                decimal num159;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num159 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num158;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num158 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num155;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            decimal num156;
                            decimal num157;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num157 = inClose[i - 1];
                            }
                            else
                            {
                                num157 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num156 = inOpen[i - 1];
                            }
                            else
                            {
                                num156 = inClose[i - 1];
                            }

                            num155 = inHigh[i - 1] - num157 + (num156 - inLow[i - 1]);
                        }
                        else
                        {
                            num155 = Decimal.Zero;
                        }

                        num158 = num155;
                    }

                    num159 = num158;
                }

                shadowLongPeriodTotal[1] += num159;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num154 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num153;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num153 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num150;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            decimal num151;
                            decimal num152;
                            if (inClose[i] >= inOpen[i])
                            {
                                num152 = inClose[i];
                            }
                            else
                            {
                                num152 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num151 = inOpen[i];
                            }
                            else
                            {
                                num151 = inClose[i];
                            }

                            num150 = inHigh[i] - num152 + (num151 - inLow[i]);
                        }
                        else
                        {
                            num150 = Decimal.Zero;
                        }

                        num153 = num150;
                    }

                    num154 = num153;
                }

                shadowLongPeriodTotal[0] += num154;
                i++;
            }

            i = nearTrailingIdx;
            while (true)
            {
                decimal num144;
                decimal num149;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num149 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num148;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num148 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num145;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num146;
                            decimal num147;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num147 = inClose[i - 2];
                            }
                            else
                            {
                                num147 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num146 = inOpen[i - 2];
                            }
                            else
                            {
                                num146 = inClose[i - 2];
                            }

                            num145 = inHigh[i - 2] - num147 + (num146 - inLow[i - 2]);
                        }
                        else
                        {
                            num145 = Decimal.Zero;
                        }

                        num148 = num145;
                    }

                    num149 = num148;
                }

                nearPeriodTotal[2] += num149;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num144 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num143;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num143 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num140;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num141;
                            decimal num142;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num142 = inClose[i - 1];
                            }
                            else
                            {
                                num142 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num141 = inOpen[i - 1];
                            }
                            else
                            {
                                num141 = inClose[i - 1];
                            }

                            num140 = inHigh[i - 1] - num142 + (num141 - inLow[i - 1]);
                        }
                        else
                        {
                            num140 = Decimal.Zero;
                        }

                        num143 = num140;
                    }

                    num144 = num143;
                }

                nearPeriodTotal[1] += num144;
                i++;
            }

            i = farTrailingIdx;
            while (true)
            {
                decimal num134;
                decimal num139;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num139 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num138;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num138 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num135;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
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

                farPeriodTotal[2] += num139;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num134 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num133;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num133 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num130;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
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

                farPeriodTotal[1] += num134;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num129;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num129 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num128;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num128 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num125;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num126;
                            decimal num127;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num127 = inClose[i - 2];
                            }
                            else
                            {
                                num127 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num126 = inOpen[i - 2];
                            }
                            else
                            {
                                num126 = inClose[i - 2];
                            }

                            num125 = inHigh[i - 2] - num127 + (num126 - inLow[i - 2]);
                        }
                        else
                        {
                            num125 = Decimal.Zero;
                        }

                        num128 = num125;
                    }

                    num129 = num128;
                }

                bodyLongPeriodTotal += num129;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0BCC:
            if (inClose[i - 2] < inOpen[i - 2] || inClose[i - 1] < inOpen[i - 1] || inClose[i] < inOpen[i] ||
                inClose[i] <= inClose[i - 1] || inClose[i - 1] <= inClose[i - 2] || inOpen[i - 1] <= inOpen[i - 2])
            {
                goto Label_1BEE;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
            {
                num124 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            }
            else
            {
                decimal num123;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num123 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num122;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num122 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num119;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num120;
                            decimal num121;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num121 = inClose[i - 2];
                            }
                            else
                            {
                                num121 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num120 = inOpen[i - 2];
                            }
                            else
                            {
                                num120 = inClose[i - 2];
                            }

                            num119 = inHigh[i - 2] - num121 + (num120 - inLow[i - 2]);
                        }
                        else
                        {
                            num119 = Decimal.Zero;
                        }

                        num122 = num119;
                    }

                    num123 = num122;
                }

                num124 = num123;
            }

            var num118 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inOpen[i - 1] > inClose[i - 2] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num124 / num118 ||
                inOpen[i] <= inOpen[i - 1])
            {
                goto Label_1BEE;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
            {
                num117 = nearPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            }
            else
            {
                decimal num116;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num116 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num115;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num115 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num112;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num113;
                            decimal num114;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num114 = inClose[i - 1];
                            }
                            else
                            {
                                num114 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num113 = inOpen[i - 1];
                            }
                            else
                            {
                                num113 = inClose[i - 1];
                            }

                            num112 = inHigh[i - 1] - num114 + (num113 - inLow[i - 1]);
                        }
                        else
                        {
                            num112 = Decimal.Zero;
                        }

                        num115 = num112;
                    }

                    num116 = num115;
                }

                num117 = num116;
            }

            var num111 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inOpen[i] > inClose[i - 1] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num117 / num111)
            {
                goto Label_1BEE;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num110 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num109;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num108;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num105;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num106;
                            decimal num107;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num107 = inClose[i - 2];
                            }
                            else
                            {
                                num107 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num106 = inOpen[i - 2];
                            }
                            else
                            {
                                num106 = inClose[i - 2];
                            }

                            num105 = inHigh[i - 2] - num107 + (num106 - inLow[i - 2]);
                        }
                        else
                        {
                            num105 = Decimal.Zero;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                num110 = num109;
            }

            var num104 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) <=
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num110 / num104)
            {
                goto Label_1BEE;
            }

            if (inClose[i - 2] >= inOpen[i - 2])
            {
                num103 = inClose[i - 2];
            }
            else
            {
                num103 = inOpen[i - 2];
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
            {
                num102 = shadowShortPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
            }
            else
            {
                decimal num101;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num101 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num100;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num100 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num97;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
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

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num98 = inOpen[i - 2];
                            }
                            else
                            {
                                num98 = inClose[i - 2];
                            }

                            num97 = inHigh[i - 2] - num99 + (num98 - inLow[i - 2]);
                        }
                        else
                        {
                            num97 = Decimal.Zero;
                        }

                        num100 = num97;
                    }

                    num101 = num100;
                }

                num102 = num101;
            }

            var num96 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inHigh[i - 2] - num103 >= (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num102 / num96)
            {
                goto Label_1BEE;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod != 0)
            {
                num95 = farPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
            }
            else
            {
                decimal num94;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num94 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num93;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num93 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num90;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            decimal num91;
                            decimal num92;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num92 = inClose[i - 2];
                            }
                            else
                            {
                                num92 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num91 = inOpen[i - 2];
                            }
                            else
                            {
                                num91 = inClose[i - 2];
                            }

                            num90 = inHigh[i - 2] - num92 + (num91 - inLow[i - 2]);
                        }
                        else
                        {
                            num90 = Decimal.Zero;
                        }

                        num93 = num90;
                    }

                    num94 = num93;
                }

                num95 = num94;
            }

            var num89 = Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <
                Math.Abs(inClose[i - 2] - inOpen[i - 2]) -
                (decimal) Globals.CandleSettings[(int) CandleSettingType.Far].Factor * num95 / num89)
            {
                decimal num88;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                {
                    num88 = nearPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                }
                else
                {
                    decimal num87;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                    {
                        num87 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num86;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                        {
                            num86 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num83;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                            {
                                decimal num84;
                                decimal num85;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num85 = inClose[i - 1];
                                }
                                else
                                {
                                    num85 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num84 = inOpen[i - 1];
                                }
                                else
                                {
                                    num84 = inClose[i - 1];
                                }

                                num83 = inHigh[i - 1] - num85 + (num84 - inLow[i - 1]);
                            }
                            else
                            {
                                num83 = Decimal.Zero;
                            }

                            num86 = num83;
                        }

                        num87 = num86;
                    }

                    num88 = num87;
                }

                var num82 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i] - inOpen[i]) <
                    Math.Abs(inClose[i - 1] - inOpen[i - 1]) +
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num88 / num82)
                {
                    goto Label_1BDF;
                }
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod != 0)
            {
                num81 = farPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod;
            }
            else
            {
                decimal num80;
                if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.RealBody)
                {
                    num80 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num79;
                    if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.HighLow)
                    {
                        num79 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num76;
                        if (Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows)
                        {
                            decimal num77;
                            decimal num78;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num78 = inClose[i - 1];
                            }
                            else
                            {
                                num78 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num77 = inOpen[i - 1];
                            }
                            else
                            {
                                num77 = inClose[i - 1];
                            }

                            num76 = inHigh[i - 1] - num78 + (num77 - inLow[i - 1]);
                        }
                        else
                        {
                            num76 = Decimal.Zero;
                        }

                        num79 = num76;
                    }

                    num80 = num79;
                }

                num81 = num80;
            }

            var num75 = Globals.CandleSettings[(int) CandleSettingType.Far].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i] - inOpen[i]) >=
                Math.Abs(inClose[i - 1] - inOpen[i - 1]) -
                (decimal) Globals.CandleSettings[(int) CandleSettingType.Far].Factor * num81 / num75)
            {
                decimal num57;
                decimal num58;
                if (Math.Abs(inClose[i] - inOpen[i]) < Math.Abs(inClose[i - 1] - inOpen[i - 1]) &&
                    Math.Abs(inClose[i - 1] - inOpen[i - 1]) < Math.Abs(inClose[i - 2] - inOpen[i - 2]))
                {
                    decimal num65;
                    decimal num66;
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

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
                    {
                        num73 = shadowShortPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num72;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                        {
                            num72 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num71;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                            {
                                num71 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num68;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num69;
                                    decimal num70;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num70 = inClose[i];
                                    }
                                    else
                                    {
                                        num70 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num69 = inOpen[i];
                                    }
                                    else
                                    {
                                        num69 = inClose[i];
                                    }

                                    num68 = inHigh[i] - num70 + (num69 - inLow[i]);
                                }
                                else
                                {
                                    num68 = Decimal.Zero;
                                }

                                num71 = num68;
                            }

                            num72 = num71;
                        }

                        num73 = num72;
                    }

                    var num67 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (inHigh[i] - num74 > (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num73 / num67)
                    {
                        goto Label_1BDF;
                    }

                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num66 = inClose[i - 1];
                    }
                    else
                    {
                        num66 = inOpen[i - 1];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
                    {
                        num65 = shadowShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num64;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                        {
                            num64 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num63;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                            {
                                num63 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num60;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num61;
                                    decimal num62;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num62 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num62 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num61 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num61 = inClose[i - 1];
                                    }

                                    num60 = inHigh[i - 1] - num62 + (num61 - inLow[i - 1]);
                                }
                                else
                                {
                                    num60 = Decimal.Zero;
                                }

                                num63 = num60;
                            }

                            num64 = num63;
                        }

                        num65 = num64;
                    }

                    var num59 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (inHigh[i - 1] - num66 >
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num65 / num59)
                    {
                        goto Label_1BDF;
                    }
                }

                if (Math.Abs(inClose[i] - inOpen[i]) >= Math.Abs(inClose[i - 1] - inOpen[i - 1]))
                {
                    goto Label_1BEE;
                }

                if (inClose[i] >= inOpen[i])
                {
                    num58 = inClose[i];
                }
                else
                {
                    num58 = inOpen[i];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                {
                    num57 = shadowLongPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                }
                else
                {
                    decimal num56;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                    {
                        num56 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        decimal num55;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                        {
                            num55 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            decimal num52;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                            {
                                decimal num53;
                                decimal num54;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num54 = inClose[i];
                                }
                                else
                                {
                                    num54 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num53 = inOpen[i];
                                }
                                else
                                {
                                    num53 = inClose[i];
                                }

                                num52 = inHigh[i] - num54 + (num53 - inLow[i]);
                            }
                            else
                            {
                                num52 = Decimal.Zero;
                            }

                            num55 = num52;
                        }

                        num56 = num55;
                    }

                    num57 = num56;
                }

                var num51 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (inHigh[i] - num58 <= (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num57 / num51)
                {
                    goto Label_1BEE;
                }
            }

            Label_1BDF:
            outInteger[outIdx] = -100;
            outIdx++;
            goto Label_1BFA;
            Label_1BEE:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_1BFA:
            var totIdx = 2;
            while (totIdx >= Decimal.Zero)
            {
                decimal num45;
                decimal num50;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num50 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num49;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num49 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num46;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            decimal num47;
                            decimal num48;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num48 = inClose[i - totIdx];
                            }
                            else
                            {
                                num48 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num47 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num47 = inClose[i - totIdx];
                            }

                            num46 = inHigh[i - totIdx] - num48 + (num47 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num46 = Decimal.Zero;
                        }

                        num49 = num46;
                    }

                    num50 = num49;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num45 = Math.Abs(inClose[shadowShortTrailingIdx - totIdx] - inOpen[shadowShortTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num44;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num44 = inHigh[shadowShortTrailingIdx - totIdx] - inLow[shadowShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num41;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            decimal num42;
                            decimal num43;
                            if (inClose[shadowShortTrailingIdx - totIdx] >= inOpen[shadowShortTrailingIdx - totIdx])
                            {
                                num43 = inClose[shadowShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num43 = inOpen[shadowShortTrailingIdx - totIdx];
                            }

                            if (inClose[shadowShortTrailingIdx - totIdx] >= inOpen[shadowShortTrailingIdx - totIdx])
                            {
                                num42 = inOpen[shadowShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num42 = inClose[shadowShortTrailingIdx - totIdx];
                            }

                            num41 = inHigh[shadowShortTrailingIdx - totIdx] - num43 + (num42 - inLow[shadowShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num41 = Decimal.Zero;
                        }

                        num44 = num41;
                    }

                    num45 = num44;
                }

                shadowShortPeriodTotal[totIdx] += num50 - num45;
                totIdx--;
            }

            for (totIdx = 1; totIdx >= 0; totIdx--)
            {
                decimal num35;
                decimal num40;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num39;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num36;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs(inClose[shadowLongTrailingIdx - totIdx] - inOpen[shadowLongTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num34;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[shadowLongTrailingIdx - totIdx] - inLow[shadowLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num31;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            decimal num32;
                            decimal num33;
                            if (inClose[shadowLongTrailingIdx - totIdx] >= inOpen[shadowLongTrailingIdx - totIdx])
                            {
                                num33 = inClose[shadowLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[shadowLongTrailingIdx - totIdx];
                            }

                            if (inClose[shadowLongTrailingIdx - totIdx] >= inOpen[shadowLongTrailingIdx - totIdx])
                            {
                                num32 = inOpen[shadowLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[shadowLongTrailingIdx - totIdx];
                            }

                            num31 = inHigh[shadowLongTrailingIdx - totIdx] - num33 + (num32 - inLow[shadowLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = Decimal.Zero;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                shadowLongPeriodTotal[totIdx] += num40 - num35;
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
                num5 = Math.Abs(inClose[bodyLongTrailingIdx - 2] - inOpen[bodyLongTrailingIdx - 2]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx - 2] - inLow[bodyLongTrailingIdx - 2];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyLongTrailingIdx - 2] >= inOpen[bodyLongTrailingIdx - 2])
                        {
                            num3 = inClose[bodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx - 2];
                        }

                        if (inClose[bodyLongTrailingIdx - 2] >= inOpen[bodyLongTrailingIdx - 2])
                        {
                            num2 = inOpen[bodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx - 2];
                        }

                        num = inHigh[bodyLongTrailingIdx - 2] - num3 + (num2 - inLow[bodyLongTrailingIdx - 2]);
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
            shadowShortTrailingIdx++;
            shadowLongTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0BCC;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        private static int CdlAdvanceBlockLookback()
        {
            var avgPeriod = Math.Max(
                Math.Max(
                    Math.Max(Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod,
                        Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod),
                    Math.Max(Globals.CandleSettings[(int) CandleSettingType.Far].AvgPeriod,
                        Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod)),
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod
            );

            return avgPeriod + 2;
        }
    }
}
