using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlAdvanceBlock(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double num5;
            double num10;
            double num75;
            double num81;
            double num89;
            double num95;
            double num96;
            double num102;
            double num103;
            double num104;
            double num110;
            double num111;
            double num117;
            double num118;
            double num124;
            double[] ShadowShortPeriodTotal = new double[3];
            double[] ShadowLongPeriodTotal = new double[2];
            double[] NearPeriodTotal = new double[3];
            double[] FarPeriodTotal = new double[3];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inOpen == null) || (inHigh == null)) || ((inLow == null) || (inClose == null)))
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

            ShadowShortPeriodTotal[2] = 0.0;
            ShadowShortPeriodTotal[1] = 0.0;
            ShadowShortPeriodTotal[0] = 0.0;
            int ShadowShortTrailingIdx = startIdx - Globals.candleSettings[6].avgPeriod;
            ShadowLongPeriodTotal[1] = 0.0;
            ShadowLongPeriodTotal[0] = 0.0;
            int ShadowLongTrailingIdx = startIdx - Globals.candleSettings[4].avgPeriod;
            NearPeriodTotal[2] = 0.0;
            NearPeriodTotal[1] = 0.0;
            NearPeriodTotal[0] = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            FarPeriodTotal[2] = 0.0;
            FarPeriodTotal[1] = 0.0;
            FarPeriodTotal[0] = 0.0;
            int FarTrailingIdx = startIdx - Globals.candleSettings[9].avgPeriod;
            double BodyLongPeriodTotal = 0.0;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = ShadowShortTrailingIdx;
            while (true)
            {
                double num164;
                double num169;
                double num174;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num174 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num173;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num173 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num170;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                            num170 = (inHigh[i - 2] - num172) + (num171 - inLow[i - 2]);
                        }
                        else
                        {
                            num170 = 0.0;
                        }

                        num173 = num170;
                    }

                    num174 = num173;
                }

                ShadowShortPeriodTotal[2] += num174;
                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num169 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num168;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num168 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num165;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                            num165 = (inHigh[i - 1] - num167) + (num166 - inLow[i - 1]);
                        }
                        else
                        {
                            num165 = 0.0;
                        }

                        num168 = num165;
                    }

                    num169 = num168;
                }

                ShadowShortPeriodTotal[1] += num169;
                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num164 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num163;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num163 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num160;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                            num160 = (inHigh[i] - num162) + (num161 - inLow[i]);
                        }
                        else
                        {
                            num160 = 0.0;
                        }

                        num163 = num160;
                    }

                    num164 = num163;
                }

                ShadowShortPeriodTotal[0] += num164;
                i++;
            }

            i = ShadowLongTrailingIdx;
            while (true)
            {
                double num154;
                double num159;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num159 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num158;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num158 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num155;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                            num155 = (inHigh[i - 1] - num157) + (num156 - inLow[i - 1]);
                        }
                        else
                        {
                            num155 = 0.0;
                        }

                        num158 = num155;
                    }

                    num159 = num158;
                }

                ShadowLongPeriodTotal[1] += num159;
                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num154 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num153;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num153 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num150;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                            num150 = (inHigh[i] - num152) + (num151 - inLow[i]);
                        }
                        else
                        {
                            num150 = 0.0;
                        }

                        num153 = num150;
                    }

                    num154 = num153;
                }

                ShadowLongPeriodTotal[0] += num154;
                i++;
            }

            i = NearTrailingIdx;
            while (true)
            {
                double num144;
                double num149;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num149 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num148;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num148 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num145;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            num145 = (inHigh[i - 2] - num147) + (num146 - inLow[i - 2]);
                        }
                        else
                        {
                            num145 = 0.0;
                        }

                        num148 = num145;
                    }

                    num149 = num148;
                }

                NearPeriodTotal[2] += num149;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num144 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num143;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num143 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num140;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            num140 = (inHigh[i - 1] - num142) + (num141 - inLow[i - 1]);
                        }
                        else
                        {
                            num140 = 0.0;
                        }

                        num143 = num140;
                    }

                    num144 = num143;
                }

                NearPeriodTotal[1] += num144;
                i++;
            }

            i = FarTrailingIdx;
            while (true)
            {
                double num134;
                double num139;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num139 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num138;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num138 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num135;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                            num135 = (inHigh[i - 2] - num137) + (num136 - inLow[i - 2]);
                        }
                        else
                        {
                            num135 = 0.0;
                        }

                        num138 = num135;
                    }

                    num139 = num138;
                }

                FarPeriodTotal[2] += num139;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num134 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num133;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num133 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num130;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                            num130 = (inHigh[i - 1] - num132) + (num131 - inLow[i - 1]);
                        }
                        else
                        {
                            num130 = 0.0;
                        }

                        num133 = num130;
                    }

                    num134 = num133;
                }

                FarPeriodTotal[1] += num134;
                i++;
            }

            i = BodyLongTrailingIdx;
            while (true)
            {
                double num129;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num129 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num128;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num128 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num125;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                            num125 = (inHigh[i - 2] - num127) + (num126 - inLow[i - 2]);
                        }
                        else
                        {
                            num125 = 0.0;
                        }

                        num128 = num125;
                    }

                    num129 = num128;
                }

                BodyLongPeriodTotal += num129;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0B40:
            if (((((inClose[i - 2] < inOpen[i - 2]) || (inClose[i - 1] < inOpen[i - 1])) || (inClose[i] < inOpen[i])) ||
                 ((inClose[i] <= inClose[i - 1]) || (inClose[i - 1] <= inClose[i - 2]))) || (inOpen[i - 1] <= inOpen[i - 2]))
            {
                goto Label_1A80;
            }

            if (Globals.candleSettings[8].avgPeriod != 0.0)
            {
                num124 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
            }
            else
            {
                double num123;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num123 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num122;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num122 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num119;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            num119 = (inHigh[i - 2] - num121) + (num120 - inLow[i - 2]);
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

            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
            {
                num118 = 2.0;
            }
            else
            {
                num118 = 1.0;
            }

            if ((inOpen[i - 1] > (inClose[i - 2] + ((Globals.candleSettings[8].factor * num124) / num118))) || (inOpen[i] <= inOpen[i - 1]))
            {
                goto Label_1A80;
            }

            if (Globals.candleSettings[8].avgPeriod != 0.0)
            {
                num117 = NearPeriodTotal[1] / ((double) Globals.candleSettings[8].avgPeriod);
            }
            else
            {
                double num116;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num116 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num115;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num115 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num112;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            num112 = (inHigh[i - 1] - num114) + (num113 - inLow[i - 1]);
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

            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
            {
                num111 = 2.0;
            }
            else
            {
                num111 = 1.0;
            }

            if (inOpen[i] > (inClose[i - 1] + ((Globals.candleSettings[8].factor * num117) / num111)))
            {
                goto Label_1A80;
            }

            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num110 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                double num109;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num108;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num105;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                            num105 = (inHigh[i - 2] - num107) + (num106 - inLow[i - 2]);
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

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num104 = 2.0;
            }
            else
            {
                num104 = 1.0;
            }

            if (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) <= ((Globals.candleSettings[0].factor * num110) / num104))
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

            if (Globals.candleSettings[6].avgPeriod != 0.0)
            {
                num102 = ShadowShortPeriodTotal[2] / ((double) Globals.candleSettings[6].avgPeriod);
            }
            else
            {
                double num101;
                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num101 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num100;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num100 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num97;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                            num97 = (inHigh[i - 2] - num99) + (num98 - inLow[i - 2]);
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

            if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
            {
                num96 = 2.0;
            }
            else
            {
                num96 = 1.0;
            }

            if ((inHigh[i - 2] - num103) >= ((Globals.candleSettings[6].factor * num102) / num96))
            {
                goto Label_1A80;
            }

            if (Globals.candleSettings[9].avgPeriod != 0.0)
            {
                num95 = FarPeriodTotal[2] / ((double) Globals.candleSettings[9].avgPeriod);
            }
            else
            {
                double num94;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num94 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num93;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num93 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num90;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                            num90 = (inHigh[i - 2] - num92) + (num91 - inLow[i - 2]);
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

            if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
            {
                num89 = 2.0;
            }
            else
            {
                num89 = 1.0;
            }

            if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) <
                (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) - ((Globals.candleSettings[9].factor * num95) / num89)))
            {
                double num82;
                double num88;
                if (Globals.candleSettings[8].avgPeriod != 0.0)
                {
                    num88 = NearPeriodTotal[1] / ((double) Globals.candleSettings[8].avgPeriod);
                }
                else
                {
                    double num87;
                    if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                    {
                        num87 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        double num86;
                        if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                        {
                            num86 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num83;
                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                                num83 = (inHigh[i - 1] - num85) + (num84 - inLow[i - 1]);
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

                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                {
                    num82 = 2.0;
                }
                else
                {
                    num82 = 1.0;
                }

                if (Math.Abs((double) (inClose[i] - inOpen[i])) <
                    (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) + ((Globals.candleSettings[8].factor * num88) / num82)))
                {
                    goto Label_1A71;
                }
            }

            if (Globals.candleSettings[9].avgPeriod != 0.0)
            {
                num81 = FarPeriodTotal[1] / ((double) Globals.candleSettings[9].avgPeriod);
            }
            else
            {
                double num80;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num80 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num79;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num79 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num76;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                            num76 = (inHigh[i - 1] - num78) + (num77 - inLow[i - 1]);
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

            if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
            {
                num75 = 2.0;
            }
            else
            {
                num75 = 1.0;
            }

            if (Math.Abs((double) (inClose[i] - inOpen[i])) >=
                (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) - ((Globals.candleSettings[9].factor * num81) / num75)))
            {
                double num51;
                double num57;
                double num58;
                if ((Math.Abs((double) (inClose[i] - inOpen[i])) < Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]))) &&
                    (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) < Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]))))
                {
                    double num59;
                    double num65;
                    double num66;
                    double num67;
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

                    if (Globals.candleSettings[6].avgPeriod != 0.0)
                    {
                        num73 = ShadowShortPeriodTotal[0] / ((double) Globals.candleSettings[6].avgPeriod);
                    }
                    else
                    {
                        double num72;
                        if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                        {
                            num72 = Math.Abs((double) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            double num71;
                            if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                            {
                                num71 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num68;
                                if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                                    num68 = (inHigh[i] - num70) + (num69 - inLow[i]);
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

                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                    {
                        num67 = 2.0;
                    }
                    else
                    {
                        num67 = 1.0;
                    }

                    if ((inHigh[i] - num74) > ((Globals.candleSettings[6].factor * num73) / num67))
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

                    if (Globals.candleSettings[6].avgPeriod != 0.0)
                    {
                        num65 = ShadowShortPeriodTotal[1] / ((double) Globals.candleSettings[6].avgPeriod);
                    }
                    else
                    {
                        double num64;
                        if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                        {
                            num64 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            double num63;
                            if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                            {
                                num63 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num60;
                                if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                                    num60 = (inHigh[i - 1] - num62) + (num61 - inLow[i - 1]);
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

                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                    {
                        num59 = 2.0;
                    }
                    else
                    {
                        num59 = 1.0;
                    }

                    if ((inHigh[i - 1] - num66) > ((Globals.candleSettings[6].factor * num65) / num59))
                    {
                        goto Label_1A71;
                    }
                }

                if (Math.Abs((double) (inClose[i] - inOpen[i])) >= Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])))
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

                if (Globals.candleSettings[4].avgPeriod != 0.0)
                {
                    num57 = ShadowLongPeriodTotal[0] / ((double) Globals.candleSettings[4].avgPeriod);
                }
                else
                {
                    double num56;
                    if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                    {
                        num56 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num55;
                        if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                        {
                            num55 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num52;
                            if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                                num52 = (inHigh[i] - num54) + (num53 - inLow[i]);
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

                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                {
                    num51 = 2.0;
                }
                else
                {
                    num51 = 1.0;
                }

                if ((inHigh[i] - num58) <= ((Globals.candleSettings[4].factor * num57) / num51))
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
            totIdx = 2;
            while (totIdx >= 0)
            {
                double num45;
                double num50;
                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num50 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num49;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num49 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num46;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                            num46 = (inHigh[i - totIdx] - num48) + (num47 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num46 = 0.0;
                        }

                        num49 = num46;
                    }

                    num50 = num49;
                }

                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num45 = Math.Abs((double) (inClose[ShadowShortTrailingIdx - totIdx] - inOpen[ShadowShortTrailingIdx - totIdx]));
                }
                else
                {
                    double num44;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num44 = inHigh[ShadowShortTrailingIdx - totIdx] - inLow[ShadowShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num41;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                        {
                            double num42;
                            double num43;
                            if (inClose[ShadowShortTrailingIdx - totIdx] >= inOpen[ShadowShortTrailingIdx - totIdx])
                            {
                                num43 = inClose[ShadowShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num43 = inOpen[ShadowShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowShortTrailingIdx - totIdx] >= inOpen[ShadowShortTrailingIdx - totIdx])
                            {
                                num42 = inOpen[ShadowShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num42 = inClose[ShadowShortTrailingIdx - totIdx];
                            }

                            num41 = (inHigh[ShadowShortTrailingIdx - totIdx] - num43) + (num42 - inLow[ShadowShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num41 = 0.0;
                        }

                        num44 = num41;
                    }

                    num45 = num44;
                }

                ShadowShortPeriodTotal[totIdx] += num50 - num45;
                totIdx--;
            }

            for (totIdx = 1; totIdx >= 0; totIdx--)
            {
                double num35;
                double num40;
                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num39;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num36;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                            num36 = (inHigh[i - totIdx] - num38) + (num37 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num36 = 0.0;
                        }

                        num39 = num36;
                    }

                    num40 = num39;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs((double) (inClose[ShadowLongTrailingIdx - totIdx] - inOpen[ShadowLongTrailingIdx - totIdx]));
                }
                else
                {
                    double num34;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[ShadowLongTrailingIdx - totIdx] - inLow[ShadowLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num31;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            double num32;
                            double num33;
                            if (inClose[ShadowLongTrailingIdx - totIdx] >= inOpen[ShadowLongTrailingIdx - totIdx])
                            {
                                num33 = inClose[ShadowLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[ShadowLongTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowLongTrailingIdx - totIdx] >= inOpen[ShadowLongTrailingIdx - totIdx])
                            {
                                num32 = inOpen[ShadowLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[ShadowLongTrailingIdx - totIdx];
                            }

                            num31 = (inHigh[ShadowLongTrailingIdx - totIdx] - num33) + (num32 - inLow[ShadowLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                ShadowLongPeriodTotal[totIdx] += num40 - num35;
            }

            for (totIdx = 2; totIdx >= 1; totIdx--)
            {
                double num15;
                double num20;
                double num25;
                double num30;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num29;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num26;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                            num26 = (inHigh[i - totIdx] - num28) + (num27 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num26 = 0.0;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs((double) (inClose[FarTrailingIdx - totIdx] - inOpen[FarTrailingIdx - totIdx]));
                }
                else
                {
                    double num24;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[FarTrailingIdx - totIdx] - inLow[FarTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num21;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            double num22;
                            double num23;
                            if (inClose[FarTrailingIdx - totIdx] >= inOpen[FarTrailingIdx - totIdx])
                            {
                                num23 = inClose[FarTrailingIdx - totIdx];
                            }
                            else
                            {
                                num23 = inOpen[FarTrailingIdx - totIdx];
                            }

                            if (inClose[FarTrailingIdx - totIdx] >= inOpen[FarTrailingIdx - totIdx])
                            {
                                num22 = inOpen[FarTrailingIdx - totIdx];
                            }
                            else
                            {
                                num22 = inClose[FarTrailingIdx - totIdx];
                            }

                            num21 = (inHigh[FarTrailingIdx - totIdx] - num23) + (num22 - inLow[FarTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num21 = 0.0;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                FarPeriodTotal[totIdx] += num30 - num25;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num19;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num16;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            num16 = (inHigh[i - totIdx] - num18) + (num17 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num16 = 0.0;
                        }

                        num19 = num16;
                    }

                    num20 = num19;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs((double) (inClose[NearTrailingIdx - totIdx] - inOpen[NearTrailingIdx - totIdx]));
                }
                else
                {
                    double num14;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[NearTrailingIdx - totIdx] - inLow[NearTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num11;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            double num12;
                            double num13;
                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num13 = inClose[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[NearTrailingIdx - totIdx];
                            }

                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num12 = inOpen[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[NearTrailingIdx - totIdx];
                            }

                            num11 = (inHigh[NearTrailingIdx - totIdx] - num13) + (num12 - inLow[NearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                NearPeriodTotal[totIdx] += num20 - num15;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                        num6 = (inHigh[i - 2] - num8) + (num7 - inLow[i - 2]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[BodyLongTrailingIdx - 2] - inOpen[BodyLongTrailingIdx - 2]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyLongTrailingIdx - 2] - inLow[BodyLongTrailingIdx - 2];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[BodyLongTrailingIdx - 2] >= inOpen[BodyLongTrailingIdx - 2])
                        {
                            num3 = inClose[BodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num3 = inOpen[BodyLongTrailingIdx - 2];
                        }

                        if (inClose[BodyLongTrailingIdx - 2] >= inOpen[BodyLongTrailingIdx - 2])
                        {
                            num2 = inOpen[BodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num2 = inClose[BodyLongTrailingIdx - 2];
                        }

                        num = (inHigh[BodyLongTrailingIdx - 2] - num3) + (num2 - inLow[BodyLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyLongPeriodTotal += num10 - num5;
            i++;
            ShadowShortTrailingIdx++;
            ShadowLongTrailingIdx++;
            NearTrailingIdx++;
            FarTrailingIdx++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0B40;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlAdvanceBlock(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            float num5;
            float num10;
            double num75;
            double num81;
            double num89;
            double num95;
            double num96;
            double num102;
            float num103;
            double num104;
            double num110;
            double num111;
            double num117;
            double num118;
            double num124;
            double[] ShadowShortPeriodTotal = new double[3];
            double[] ShadowLongPeriodTotal = new double[2];
            double[] NearPeriodTotal = new double[3];
            double[] FarPeriodTotal = new double[3];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inOpen == null) || (inHigh == null)) || ((inLow == null) || (inClose == null)))
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

            ShadowShortPeriodTotal[2] = 0.0;
            ShadowShortPeriodTotal[1] = 0.0;
            ShadowShortPeriodTotal[0] = 0.0;
            int ShadowShortTrailingIdx = startIdx - Globals.candleSettings[6].avgPeriod;
            ShadowLongPeriodTotal[1] = 0.0;
            ShadowLongPeriodTotal[0] = 0.0;
            int ShadowLongTrailingIdx = startIdx - Globals.candleSettings[4].avgPeriod;
            NearPeriodTotal[2] = 0.0;
            NearPeriodTotal[1] = 0.0;
            NearPeriodTotal[0] = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            FarPeriodTotal[2] = 0.0;
            FarPeriodTotal[1] = 0.0;
            FarPeriodTotal[0] = 0.0;
            int FarTrailingIdx = startIdx - Globals.candleSettings[9].avgPeriod;
            double BodyLongPeriodTotal = 0.0;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = ShadowShortTrailingIdx;
            while (true)
            {
                float num164;
                float num169;
                float num174;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num174 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num173;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num173 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num170;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                        {
                            float num171;
                            float num172;
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

                            num170 = (inHigh[i - 2] - num172) + (num171 - inLow[i - 2]);
                        }
                        else
                        {
                            num170 = 0.0f;
                        }

                        num173 = num170;
                    }

                    num174 = num173;
                }

                ShadowShortPeriodTotal[2] += num174;
                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num169 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num168;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num168 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num165;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                        {
                            float num166;
                            float num167;
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

                            num165 = (inHigh[i - 1] - num167) + (num166 - inLow[i - 1]);
                        }
                        else
                        {
                            num165 = 0.0f;
                        }

                        num168 = num165;
                    }

                    num169 = num168;
                }

                ShadowShortPeriodTotal[1] += num169;
                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num164 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num163;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num163 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num160;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                        {
                            float num161;
                            float num162;
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

                            num160 = (inHigh[i] - num162) + (num161 - inLow[i]);
                        }
                        else
                        {
                            num160 = 0.0f;
                        }

                        num163 = num160;
                    }

                    num164 = num163;
                }

                ShadowShortPeriodTotal[0] += num164;
                i++;
            }

            i = ShadowLongTrailingIdx;
            while (true)
            {
                float num154;
                float num159;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num159 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num158;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num158 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num155;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            float num156;
                            float num157;
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

                            num155 = (inHigh[i - 1] - num157) + (num156 - inLow[i - 1]);
                        }
                        else
                        {
                            num155 = 0.0f;
                        }

                        num158 = num155;
                    }

                    num159 = num158;
                }

                ShadowLongPeriodTotal[1] += num159;
                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num154 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num153;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num153 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num150;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            float num151;
                            float num152;
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

                            num150 = (inHigh[i] - num152) + (num151 - inLow[i]);
                        }
                        else
                        {
                            num150 = 0.0f;
                        }

                        num153 = num150;
                    }

                    num154 = num153;
                }

                ShadowLongPeriodTotal[0] += num154;
                i++;
            }

            i = NearTrailingIdx;
            while (true)
            {
                float num144;
                float num149;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num149 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num148;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num148 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num145;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num146;
                            float num147;
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

                            num145 = (inHigh[i - 2] - num147) + (num146 - inLow[i - 2]);
                        }
                        else
                        {
                            num145 = 0.0f;
                        }

                        num148 = num145;
                    }

                    num149 = num148;
                }

                NearPeriodTotal[2] += num149;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num144 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num143;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num143 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num140;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num141;
                            float num142;
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

                            num140 = (inHigh[i - 1] - num142) + (num141 - inLow[i - 1]);
                        }
                        else
                        {
                            num140 = 0.0f;
                        }

                        num143 = num140;
                    }

                    num144 = num143;
                }

                NearPeriodTotal[1] += num144;
                i++;
            }

            i = FarTrailingIdx;
            while (true)
            {
                float num134;
                float num139;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num139 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num138;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num138 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num135;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            float num136;
                            float num137;
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

                            num135 = (inHigh[i - 2] - num137) + (num136 - inLow[i - 2]);
                        }
                        else
                        {
                            num135 = 0.0f;
                        }

                        num138 = num135;
                    }

                    num139 = num138;
                }

                FarPeriodTotal[2] += num139;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num134 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num133;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num133 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num130;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            float num131;
                            float num132;
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

                            num130 = (inHigh[i - 1] - num132) + (num131 - inLow[i - 1]);
                        }
                        else
                        {
                            num130 = 0.0f;
                        }

                        num133 = num130;
                    }

                    num134 = num133;
                }

                FarPeriodTotal[1] += num134;
                i++;
            }

            i = BodyLongTrailingIdx;
            while (true)
            {
                float num129;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num129 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num128;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num128 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num125;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num126;
                            float num127;
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

                            num125 = (inHigh[i - 2] - num127) + (num126 - inLow[i - 2]);
                        }
                        else
                        {
                            num125 = 0.0f;
                        }

                        num128 = num125;
                    }

                    num129 = num128;
                }

                BodyLongPeriodTotal += num129;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0BCC:
            if (((((inClose[i - 2] < inOpen[i - 2]) || (inClose[i - 1] < inOpen[i - 1])) || (inClose[i] < inOpen[i])) ||
                 ((inClose[i] <= inClose[i - 1]) || (inClose[i - 1] <= inClose[i - 2]))) || (inOpen[i - 1] <= inOpen[i - 2]))
            {
                goto Label_1BEE;
            }

            if (Globals.candleSettings[8].avgPeriod != 0.0)
            {
                num124 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
            }
            else
            {
                float num123;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num123 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num122;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num122 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num119;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num120;
                            float num121;
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

                            num119 = (inHigh[i - 2] - num121) + (num120 - inLow[i - 2]);
                        }
                        else
                        {
                            num119 = 0.0f;
                        }

                        num122 = num119;
                    }

                    num123 = num122;
                }

                num124 = num123;
            }

            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
            {
                num118 = 2.0;
            }
            else
            {
                num118 = 1.0;
            }

            if ((inOpen[i - 1] > (inClose[i - 2] + ((Globals.candleSettings[8].factor * num124) / num118))) || (inOpen[i] <= inOpen[i - 1]))
            {
                goto Label_1BEE;
            }

            if (Globals.candleSettings[8].avgPeriod != 0.0)
            {
                num117 = NearPeriodTotal[1] / ((double) Globals.candleSettings[8].avgPeriod);
            }
            else
            {
                float num116;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num116 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num115;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num115 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num112;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num113;
                            float num114;
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

                            num112 = (inHigh[i - 1] - num114) + (num113 - inLow[i - 1]);
                        }
                        else
                        {
                            num112 = 0.0f;
                        }

                        num115 = num112;
                    }

                    num116 = num115;
                }

                num117 = num116;
            }

            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
            {
                num111 = 2.0;
            }
            else
            {
                num111 = 1.0;
            }

            if (inOpen[i] > (inClose[i - 1] + ((Globals.candleSettings[8].factor * num117) / num111)))
            {
                goto Label_1BEE;
            }

            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num110 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                float num109;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num108;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num105;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num106;
                            float num107;
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

                            num105 = (inHigh[i - 2] - num107) + (num106 - inLow[i - 2]);
                        }
                        else
                        {
                            num105 = 0.0f;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                num110 = num109;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num104 = 2.0;
            }
            else
            {
                num104 = 1.0;
            }

            if (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) <= ((Globals.candleSettings[0].factor * num110) / num104))
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

            if (Globals.candleSettings[6].avgPeriod != 0.0)
            {
                num102 = ShadowShortPeriodTotal[2] / ((double) Globals.candleSettings[6].avgPeriod);
            }
            else
            {
                float num101;
                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num101 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num100;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num100 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num97;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                        {
                            float num98;
                            float num99;
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

                            num97 = (inHigh[i - 2] - num99) + (num98 - inLow[i - 2]);
                        }
                        else
                        {
                            num97 = 0.0f;
                        }

                        num100 = num97;
                    }

                    num101 = num100;
                }

                num102 = num101;
            }

            if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
            {
                num96 = 2.0;
            }
            else
            {
                num96 = 1.0;
            }

            if ((inHigh[i - 2] - num103) >= ((Globals.candleSettings[6].factor * num102) / num96))
            {
                goto Label_1BEE;
            }

            if (Globals.candleSettings[9].avgPeriod != 0.0)
            {
                num95 = FarPeriodTotal[2] / ((double) Globals.candleSettings[9].avgPeriod);
            }
            else
            {
                float num94;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num94 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num93;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num93 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num90;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            float num91;
                            float num92;
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

                            num90 = (inHigh[i - 2] - num92) + (num91 - inLow[i - 2]);
                        }
                        else
                        {
                            num90 = 0.0f;
                        }

                        num93 = num90;
                    }

                    num94 = num93;
                }

                num95 = num94;
            }

            if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
            {
                num89 = 2.0;
            }
            else
            {
                num89 = 1.0;
            }

            if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) <
                (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) - ((Globals.candleSettings[9].factor * num95) / num89)))
            {
                double num82;
                double num88;
                if (Globals.candleSettings[8].avgPeriod != 0.0)
                {
                    num88 = NearPeriodTotal[1] / ((double) Globals.candleSettings[8].avgPeriod);
                }
                else
                {
                    float num87;
                    if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                    {
                        num87 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        float num86;
                        if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                        {
                            num86 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            float num83;
                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                float num84;
                                float num85;
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

                                num83 = (inHigh[i - 1] - num85) + (num84 - inLow[i - 1]);
                            }
                            else
                            {
                                num83 = 0.0f;
                            }

                            num86 = num83;
                        }

                        num87 = num86;
                    }

                    num88 = num87;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                {
                    num82 = 2.0;
                }
                else
                {
                    num82 = 1.0;
                }

                if (Math.Abs((float) (inClose[i] - inOpen[i])) <
                    (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) + ((Globals.candleSettings[8].factor * num88) / num82)))
                {
                    goto Label_1BDF;
                }
            }

            if (Globals.candleSettings[9].avgPeriod != 0.0)
            {
                num81 = FarPeriodTotal[1] / ((double) Globals.candleSettings[9].avgPeriod);
            }
            else
            {
                float num80;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num80 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num79;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num79 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num76;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            float num77;
                            float num78;
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

                            num76 = (inHigh[i - 1] - num78) + (num77 - inLow[i - 1]);
                        }
                        else
                        {
                            num76 = 0.0f;
                        }

                        num79 = num76;
                    }

                    num80 = num79;
                }

                num81 = num80;
            }

            if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
            {
                num75 = 2.0;
            }
            else
            {
                num75 = 1.0;
            }

            if (Math.Abs((float) (inClose[i] - inOpen[i])) >=
                (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) - ((Globals.candleSettings[9].factor * num81) / num75)))
            {
                double num51;
                double num57;
                float num58;
                if ((Math.Abs((float) (inClose[i] - inOpen[i])) < Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]))) &&
                    (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) < Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]))))
                {
                    double num59;
                    double num65;
                    float num66;
                    double num67;
                    double num73;
                    float num74;
                    if (inClose[i] >= inOpen[i])
                    {
                        num74 = inClose[i];
                    }
                    else
                    {
                        num74 = inOpen[i];
                    }

                    if (Globals.candleSettings[6].avgPeriod != 0.0)
                    {
                        num73 = ShadowShortPeriodTotal[0] / ((double) Globals.candleSettings[6].avgPeriod);
                    }
                    else
                    {
                        float num72;
                        if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                        {
                            num72 = Math.Abs((float) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            float num71;
                            if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                            {
                                num71 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                float num68;
                                if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                                {
                                    float num69;
                                    float num70;
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

                                    num68 = (inHigh[i] - num70) + (num69 - inLow[i]);
                                }
                                else
                                {
                                    num68 = 0.0f;
                                }

                                num71 = num68;
                            }

                            num72 = num71;
                        }

                        num73 = num72;
                    }

                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                    {
                        num67 = 2.0;
                    }
                    else
                    {
                        num67 = 1.0;
                    }

                    if ((inHigh[i] - num74) > ((Globals.candleSettings[6].factor * num73) / num67))
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

                    if (Globals.candleSettings[6].avgPeriod != 0.0)
                    {
                        num65 = ShadowShortPeriodTotal[1] / ((double) Globals.candleSettings[6].avgPeriod);
                    }
                    else
                    {
                        float num64;
                        if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                        {
                            num64 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            float num63;
                            if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                            {
                                num63 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                float num60;
                                if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                                {
                                    float num61;
                                    float num62;
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

                                    num60 = (inHigh[i - 1] - num62) + (num61 - inLow[i - 1]);
                                }
                                else
                                {
                                    num60 = 0.0f;
                                }

                                num63 = num60;
                            }

                            num64 = num63;
                        }

                        num65 = num64;
                    }

                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                    {
                        num59 = 2.0;
                    }
                    else
                    {
                        num59 = 1.0;
                    }

                    if ((inHigh[i - 1] - num66) > ((Globals.candleSettings[6].factor * num65) / num59))
                    {
                        goto Label_1BDF;
                    }
                }

                if (Math.Abs((float) (inClose[i] - inOpen[i])) >= Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])))
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

                if (Globals.candleSettings[4].avgPeriod != 0.0)
                {
                    num57 = ShadowLongPeriodTotal[0] / ((double) Globals.candleSettings[4].avgPeriod);
                }
                else
                {
                    float num56;
                    if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                    {
                        num56 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num55;
                        if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                        {
                            num55 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num52;
                            if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                            {
                                float num53;
                                float num54;
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

                                num52 = (inHigh[i] - num54) + (num53 - inLow[i]);
                            }
                            else
                            {
                                num52 = 0.0f;
                            }

                            num55 = num52;
                        }

                        num56 = num55;
                    }

                    num57 = num56;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                {
                    num51 = 2.0;
                }
                else
                {
                    num51 = 1.0;
                }

                if ((inHigh[i] - num58) <= ((Globals.candleSettings[4].factor * num57) / num51))
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
            totIdx = 2;
            while (totIdx >= 0)
            {
                float num45;
                float num50;
                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num50 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num49;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num49 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num46;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                        {
                            float num47;
                            float num48;
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

                            num46 = (inHigh[i - totIdx] - num48) + (num47 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num46 = 0.0f;
                        }

                        num49 = num46;
                    }

                    num50 = num49;
                }

                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num45 = Math.Abs((float) (inClose[ShadowShortTrailingIdx - totIdx] - inOpen[ShadowShortTrailingIdx - totIdx]));
                }
                else
                {
                    float num44;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num44 = inHigh[ShadowShortTrailingIdx - totIdx] - inLow[ShadowShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num41;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                        {
                            float num42;
                            float num43;
                            if (inClose[ShadowShortTrailingIdx - totIdx] >= inOpen[ShadowShortTrailingIdx - totIdx])
                            {
                                num43 = inClose[ShadowShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num43 = inOpen[ShadowShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowShortTrailingIdx - totIdx] >= inOpen[ShadowShortTrailingIdx - totIdx])
                            {
                                num42 = inOpen[ShadowShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num42 = inClose[ShadowShortTrailingIdx - totIdx];
                            }

                            num41 = (inHigh[ShadowShortTrailingIdx - totIdx] - num43) + (num42 - inLow[ShadowShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num41 = 0.0f;
                        }

                        num44 = num41;
                    }

                    num45 = num44;
                }

                ShadowShortPeriodTotal[totIdx] += num50 - num45;
                totIdx--;
            }

            for (totIdx = 1; totIdx >= 0; totIdx--)
            {
                float num35;
                float num40;
                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num39;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num36;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            float num37;
                            float num38;
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

                            num36 = (inHigh[i - totIdx] - num38) + (num37 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num36 = 0.0f;
                        }

                        num39 = num36;
                    }

                    num40 = num39;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs((float) (inClose[ShadowLongTrailingIdx - totIdx] - inOpen[ShadowLongTrailingIdx - totIdx]));
                }
                else
                {
                    float num34;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[ShadowLongTrailingIdx - totIdx] - inLow[ShadowLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num31;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            float num32;
                            float num33;
                            if (inClose[ShadowLongTrailingIdx - totIdx] >= inOpen[ShadowLongTrailingIdx - totIdx])
                            {
                                num33 = inClose[ShadowLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[ShadowLongTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowLongTrailingIdx - totIdx] >= inOpen[ShadowLongTrailingIdx - totIdx])
                            {
                                num32 = inOpen[ShadowLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[ShadowLongTrailingIdx - totIdx];
                            }

                            num31 = (inHigh[ShadowLongTrailingIdx - totIdx] - num33) + (num32 - inLow[ShadowLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0f;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                ShadowLongPeriodTotal[totIdx] += num40 - num35;
            }

            for (totIdx = 2; totIdx >= 1; totIdx--)
            {
                float num15;
                float num20;
                float num25;
                float num30;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num29;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num26;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            float num27;
                            float num28;
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

                            num26 = (inHigh[i - totIdx] - num28) + (num27 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num26 = 0.0f;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs((float) (inClose[FarTrailingIdx - totIdx] - inOpen[FarTrailingIdx - totIdx]));
                }
                else
                {
                    float num24;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[FarTrailingIdx - totIdx] - inLow[FarTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num21;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            float num22;
                            float num23;
                            if (inClose[FarTrailingIdx - totIdx] >= inOpen[FarTrailingIdx - totIdx])
                            {
                                num23 = inClose[FarTrailingIdx - totIdx];
                            }
                            else
                            {
                                num23 = inOpen[FarTrailingIdx - totIdx];
                            }

                            if (inClose[FarTrailingIdx - totIdx] >= inOpen[FarTrailingIdx - totIdx])
                            {
                                num22 = inOpen[FarTrailingIdx - totIdx];
                            }
                            else
                            {
                                num22 = inClose[FarTrailingIdx - totIdx];
                            }

                            num21 = (inHigh[FarTrailingIdx - totIdx] - num23) + (num22 - inLow[FarTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num21 = 0.0f;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                FarPeriodTotal[totIdx] += num30 - num25;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num19;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num16;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num17;
                            float num18;
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

                            num16 = (inHigh[i - totIdx] - num18) + (num17 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num16 = 0.0f;
                        }

                        num19 = num16;
                    }

                    num20 = num19;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs((float) (inClose[NearTrailingIdx - totIdx] - inOpen[NearTrailingIdx - totIdx]));
                }
                else
                {
                    float num14;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[NearTrailingIdx - totIdx] - inLow[NearTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num11;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num12;
                            float num13;
                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num13 = inClose[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[NearTrailingIdx - totIdx];
                            }

                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num12 = inOpen[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[NearTrailingIdx - totIdx];
                            }

                            num11 = (inHigh[NearTrailingIdx - totIdx] - num13) + (num12 - inLow[NearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0f;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                NearPeriodTotal[totIdx] += num20 - num15;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num7;
                        float num8;
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

                        num6 = (inHigh[i - 2] - num8) + (num7 - inLow[i - 2]);
                    }
                    else
                    {
                        num6 = 0.0f;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[BodyLongTrailingIdx - 2] - inOpen[BodyLongTrailingIdx - 2]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyLongTrailingIdx - 2] - inLow[BodyLongTrailingIdx - 2];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[BodyLongTrailingIdx - 2] >= inOpen[BodyLongTrailingIdx - 2])
                        {
                            num3 = inClose[BodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num3 = inOpen[BodyLongTrailingIdx - 2];
                        }

                        if (inClose[BodyLongTrailingIdx - 2] >= inOpen[BodyLongTrailingIdx - 2])
                        {
                            num2 = inOpen[BodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num2 = inClose[BodyLongTrailingIdx - 2];
                        }

                        num = (inHigh[BodyLongTrailingIdx - 2] - num3) + (num2 - inLow[BodyLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyLongPeriodTotal += num10 - num5;
            i++;
            ShadowShortTrailingIdx++;
            ShadowLongTrailingIdx++;
            NearTrailingIdx++;
            FarTrailingIdx++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0BCC;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlAdvanceBlockLookback()
        {
            int num;
            int num4;
            int avgPeriod;
            if (Globals.candleSettings[9].avgPeriod > Globals.candleSettings[8].avgPeriod)
            {
                avgPeriod = Globals.candleSettings[9].avgPeriod;
            }
            else
            {
                avgPeriod = Globals.candleSettings[8].avgPeriod;
            }

            if (((Globals.candleSettings[4].avgPeriod <= Globals.candleSettings[6].avgPeriod)
                    ? Globals.candleSettings[6].avgPeriod
                    : Globals.candleSettings[4].avgPeriod) > avgPeriod)
            {
                num4 = (Globals.candleSettings[4].avgPeriod <= Globals.candleSettings[6].avgPeriod)
                    ? Globals.candleSettings[6].avgPeriod
                    : Globals.candleSettings[4].avgPeriod;
            }
            else
            {
                num4 = (Globals.candleSettings[9].avgPeriod <= Globals.candleSettings[8].avgPeriod)
                    ? Globals.candleSettings[8].avgPeriod
                    : Globals.candleSettings[9].avgPeriod;
            }

            if (num4 > Globals.candleSettings[0].avgPeriod)
            {
                int num2;
                int num3;
                if (Globals.candleSettings[9].avgPeriod > Globals.candleSettings[8].avgPeriod)
                {
                    num3 = Globals.candleSettings[9].avgPeriod;
                }
                else
                {
                    num3 = Globals.candleSettings[8].avgPeriod;
                }

                if (((Globals.candleSettings[4].avgPeriod <= Globals.candleSettings[6].avgPeriod)
                        ? Globals.candleSettings[6].avgPeriod
                        : Globals.candleSettings[4].avgPeriod) > num3)
                {
                    num2 = (Globals.candleSettings[4].avgPeriod <= Globals.candleSettings[6].avgPeriod)
                        ? Globals.candleSettings[6].avgPeriod
                        : Globals.candleSettings[4].avgPeriod;
                }
                else
                {
                    num2 = (Globals.candleSettings[9].avgPeriod <= Globals.candleSettings[8].avgPeriod)
                        ? Globals.candleSettings[8].avgPeriod
                        : Globals.candleSettings[9].avgPeriod;
                }

                num = num2;
            }
            else
            {
                num = Globals.candleSettings[0].avgPeriod;
            }

            return (num + 2);
        }
    }
}
