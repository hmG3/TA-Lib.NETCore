using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3WhiteSoldiers(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double num5;
            double num10;
            double[] ShadowVeryShortPeriodTotal = new double[3];
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

            ShadowVeryShortPeriodTotal[2] = 0.0;
            ShadowVeryShortPeriodTotal[1] = 0.0;
            ShadowVeryShortPeriodTotal[0] = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            NearPeriodTotal[2] = 0.0;
            NearPeriodTotal[1] = 0.0;
            NearPeriodTotal[0] = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            FarPeriodTotal[2] = 0.0;
            FarPeriodTotal[1] = 0.0;
            FarPeriodTotal[0] = 0.0;
            int FarTrailingIdx = startIdx - Globals.candleSettings[9].avgPeriod;
            double BodyShortPeriodTotal = 0.0;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num129;
                double num134;
                double num139;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num139 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num138;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num138 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num135;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                ShadowVeryShortPeriodTotal[2] += num139;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num134 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num133;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num133 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num130;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                ShadowVeryShortPeriodTotal[1] += num134;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num129 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num128;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num128 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num125;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                            num125 = (inHigh[i] - num127) + (num126 - inLow[i]);
                        }
                        else
                        {
                            num125 = 0.0;
                        }

                        num128 = num125;
                    }

                    num129 = num128;
                }

                ShadowVeryShortPeriodTotal[0] += num129;
                i++;
            }

            i = NearTrailingIdx;
            while (true)
            {
                double num119;
                double num124;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num124 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num123;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num123 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num120;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            num120 = (inHigh[i - 2] - num122) + (num121 - inLow[i - 2]);
                        }
                        else
                        {
                            num120 = 0.0;
                        }

                        num123 = num120;
                    }

                    num124 = num123;
                }

                NearPeriodTotal[2] += num124;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num119 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num118;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num118 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num115;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            num115 = (inHigh[i - 1] - num117) + (num116 - inLow[i - 1]);
                        }
                        else
                        {
                            num115 = 0.0;
                        }

                        num118 = num115;
                    }

                    num119 = num118;
                }

                NearPeriodTotal[1] += num119;
                i++;
            }

            i = FarTrailingIdx;
            while (true)
            {
                double num109;
                double num114;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num114 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num113;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num113 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num110;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                            num110 = (inHigh[i - 2] - num112) + (num111 - inLow[i - 2]);
                        }
                        else
                        {
                            num110 = 0.0;
                        }

                        num113 = num110;
                    }

                    num114 = num113;
                }

                FarPeriodTotal[2] += num114;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num108;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num105;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                            num105 = (inHigh[i - 1] - num107) + (num106 - inLow[i - 1]);
                        }
                        else
                        {
                            num105 = 0.0;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                FarPeriodTotal[1] += num109;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                double num104;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num104 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num103;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num103 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num100;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                            num100 = (inHigh[i] - num102) + (num101 - inLow[i]);
                        }
                        else
                        {
                            num100 = 0.0;
                        }

                        num103 = num100;
                    }

                    num104 = num103;
                }

                BodyShortPeriodTotal += num104;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_08A5:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num92;
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

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num98 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    double num97;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num97 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num96;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num96 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num93;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                                num93 = (inHigh[i - 2] - num95) + (num94 - inLow[i - 2]);
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

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num92 = 2.0;
                }
                else
                {
                    num92 = 1.0;
                }

                if (((inHigh[i - 2] - num99) < ((Globals.candleSettings[7].factor * num98) / num92)) && (inClose[i - 1] >= inOpen[i - 1]))
                {
                    double num84;
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

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num90 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        double num89;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num89 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            double num88;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num88 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num85;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                                    num85 = (inHigh[i - 1] - num87) + (num86 - inLow[i - 1]);
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

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num84 = 2.0;
                    }
                    else
                    {
                        num84 = 1.0;
                    }

                    if (((inHigh[i - 1] - num91) < ((Globals.candleSettings[7].factor * num90) / num84)) && (inClose[i] >= inOpen[i]))
                    {
                        double num76;
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

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num82 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            double num81;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num81 = Math.Abs((double) (inClose[i] - inOpen[i]));
                            }
                            else
                            {
                                double num80;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num80 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    double num77;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                                        num77 = (inHigh[i] - num79) + (num78 - inLow[i]);
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

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num76 = 2.0;
                        }
                        else
                        {
                            num76 = 1.0;
                        }

                        if ((((inHigh[i] - num83) < ((Globals.candleSettings[7].factor * num82) / num76)) &&
                             (inClose[i] > inClose[i - 1])) && ((inClose[i - 1] > inClose[i - 2]) && (inOpen[i - 1] > inOpen[i - 2])))
                        {
                            double num69;
                            double num75;
                            if (Globals.candleSettings[8].avgPeriod != 0.0)
                            {
                                num75 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
                            }
                            else
                            {
                                double num74;
                                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                {
                                    num74 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                                }
                                else
                                {
                                    double num73;
                                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                    {
                                        num73 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        double num70;
                                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                                            num70 = (inHigh[i - 2] - num72) + (num71 - inLow[i - 2]);
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

                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                num69 = 2.0;
                            }
                            else
                            {
                                num69 = 1.0;
                            }

                            if ((inOpen[i - 1] <= (inClose[i - 2] + ((Globals.candleSettings[8].factor * num75) / num69))) &&
                                (inOpen[i] > inOpen[i - 1]))
                            {
                                double num62;
                                double num68;
                                if (Globals.candleSettings[8].avgPeriod != 0.0)
                                {
                                    num68 = NearPeriodTotal[1] / ((double) Globals.candleSettings[8].avgPeriod);
                                }
                                else
                                {
                                    double num67;
                                    if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                    {
                                        num67 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                                    }
                                    else
                                    {
                                        double num66;
                                        if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                        {
                                            num66 = inHigh[i - 1] - inLow[i - 1];
                                        }
                                        else
                                        {
                                            double num63;
                                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                                                num63 = (inHigh[i - 1] - num65) + (num64 - inLow[i - 1]);
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

                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                {
                                    num62 = 2.0;
                                }
                                else
                                {
                                    num62 = 1.0;
                                }

                                if (inOpen[i] <= (inClose[i - 1] + ((Globals.candleSettings[8].factor * num68) / num62)))
                                {
                                    double num55;
                                    double num61;
                                    if (Globals.candleSettings[9].avgPeriod != 0.0)
                                    {
                                        num61 = FarPeriodTotal[2] / ((double) Globals.candleSettings[9].avgPeriod);
                                    }
                                    else
                                    {
                                        double num60;
                                        if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                                        {
                                            num60 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                                        }
                                        else
                                        {
                                            double num59;
                                            if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                                            {
                                                num59 = inHigh[i - 2] - inLow[i - 2];
                                            }
                                            else
                                            {
                                                double num56;
                                                if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                                                    num56 = (inHigh[i - 2] - num58) + (num57 - inLow[i - 2]);
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

                                    if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                                    {
                                        num55 = 2.0;
                                    }
                                    else
                                    {
                                        num55 = 1.0;
                                    }

                                    if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) >
                                        (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) -
                                         ((Globals.candleSettings[9].factor * num61) / num55)))
                                    {
                                        double num48;
                                        double num54;
                                        if (Globals.candleSettings[9].avgPeriod != 0.0)
                                        {
                                            num54 = FarPeriodTotal[1] / ((double) Globals.candleSettings[9].avgPeriod);
                                        }
                                        else
                                        {
                                            double num53;
                                            if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                                            {
                                                num53 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                                            }
                                            else
                                            {
                                                double num52;
                                                if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                                                {
                                                    num52 = inHigh[i - 1] - inLow[i - 1];
                                                }
                                                else
                                                {
                                                    double num49;
                                                    if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
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

                                                        num49 = (inHigh[i - 1] - num51) + (num50 - inLow[i - 1]);
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

                                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                                        {
                                            num48 = 2.0;
                                        }
                                        else
                                        {
                                            num48 = 1.0;
                                        }

                                        if (Math.Abs((double) (inClose[i] - inOpen[i])) >
                                            (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) -
                                             ((Globals.candleSettings[9].factor * num54) / num48)))
                                        {
                                            double num41;
                                            double num47;
                                            if (Globals.candleSettings[2].avgPeriod != 0.0)
                                            {
                                                num47 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                                            }
                                            else
                                            {
                                                double num46;
                                                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                                                {
                                                    num46 = Math.Abs((double) (inClose[i] - inOpen[i]));
                                                }
                                                else
                                                {
                                                    double num45;
                                                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                                    {
                                                        num45 = inHigh[i] - inLow[i];
                                                    }
                                                    else
                                                    {
                                                        double num42;
                                                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                                                            num42 = (inHigh[i] - num44) + (num43 - inLow[i]);
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

                                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                            {
                                                num41 = 2.0;
                                            }
                                            else
                                            {
                                                num41 = 1.0;
                                            }

                                            if (Math.Abs((double) (inClose[i] - inOpen[i])) >
                                                ((Globals.candleSettings[2].factor * num47) / num41))
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
            totIdx = 2;
            while (totIdx >= 0)
            {
                double num35;
                double num40;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num39;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num36;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs((double) (inClose[ShadowVeryShortTrailingIdx - totIdx] - inOpen[ShadowVeryShortTrailingIdx - totIdx]));
                }
                else
                {
                    double num34;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[ShadowVeryShortTrailingIdx - totIdx] - inLow[ShadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num31;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num32;
                            double num33;
                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num33 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num32 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            num31 = (inHigh[ShadowVeryShortTrailingIdx - totIdx] - num33) +
                                    (num32 - inLow[ShadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                ShadowVeryShortPeriodTotal[totIdx] += num40 - num35;
                totIdx--;
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

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                        num6 = (inHigh[i] - num8) + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[BodyShortTrailingIdx] - inOpen[BodyShortTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyShortTrailingIdx] - inLow[BodyShortTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num3 = inClose[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[BodyShortTrailingIdx];
                        }

                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num2 = inOpen[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[BodyShortTrailingIdx];
                        }

                        num = (inHigh[BodyShortTrailingIdx] - num3) + (num2 - inLow[BodyShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyShortPeriodTotal += num10 - num5;
            i++;
            ShadowVeryShortTrailingIdx++;
            NearTrailingIdx++;
            FarTrailingIdx++;
            BodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_08A5;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3WhiteSoldiers(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            float num5;
            float num10;
            double[] ShadowVeryShortPeriodTotal = new double[3];
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

            ShadowVeryShortPeriodTotal[2] = 0.0;
            ShadowVeryShortPeriodTotal[1] = 0.0;
            ShadowVeryShortPeriodTotal[0] = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            NearPeriodTotal[2] = 0.0;
            NearPeriodTotal[1] = 0.0;
            NearPeriodTotal[0] = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            FarPeriodTotal[2] = 0.0;
            FarPeriodTotal[1] = 0.0;
            FarPeriodTotal[0] = 0.0;
            int FarTrailingIdx = startIdx - Globals.candleSettings[9].avgPeriod;
            double BodyShortPeriodTotal = 0.0;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num129;
                float num134;
                float num139;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num139 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num138;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num138 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num135;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                ShadowVeryShortPeriodTotal[2] += num139;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num134 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num133;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num133 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num130;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                ShadowVeryShortPeriodTotal[1] += num134;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num129 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num128;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num128 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num125;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num126;
                            float num127;
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

                            num125 = (inHigh[i] - num127) + (num126 - inLow[i]);
                        }
                        else
                        {
                            num125 = 0.0f;
                        }

                        num128 = num125;
                    }

                    num129 = num128;
                }

                ShadowVeryShortPeriodTotal[0] += num129;
                i++;
            }

            i = NearTrailingIdx;
            while (true)
            {
                float num119;
                float num124;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num124 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num123;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num123 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num120;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num121;
                            float num122;
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

                            num120 = (inHigh[i - 2] - num122) + (num121 - inLow[i - 2]);
                        }
                        else
                        {
                            num120 = 0.0f;
                        }

                        num123 = num120;
                    }

                    num124 = num123;
                }

                NearPeriodTotal[2] += num124;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num119 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num118;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num118 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num115;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num116;
                            float num117;
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

                            num115 = (inHigh[i - 1] - num117) + (num116 - inLow[i - 1]);
                        }
                        else
                        {
                            num115 = 0.0f;
                        }

                        num118 = num115;
                    }

                    num119 = num118;
                }

                NearPeriodTotal[1] += num119;
                i++;
            }

            i = FarTrailingIdx;
            while (true)
            {
                float num109;
                float num114;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num114 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num113;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num113 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num110;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            float num111;
                            float num112;
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

                            num110 = (inHigh[i - 2] - num112) + (num111 - inLow[i - 2]);
                        }
                        else
                        {
                            num110 = 0.0f;
                        }

                        num113 = num110;
                    }

                    num114 = num113;
                }

                FarPeriodTotal[2] += num114;
                if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num108;
                    if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num105;
                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                        {
                            float num106;
                            float num107;
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

                            num105 = (inHigh[i - 1] - num107) + (num106 - inLow[i - 1]);
                        }
                        else
                        {
                            num105 = 0.0f;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                FarPeriodTotal[1] += num109;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                float num104;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num104 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num103;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num103 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num100;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num101;
                            float num102;
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

                            num100 = (inHigh[i] - num102) + (num101 - inLow[i]);
                        }
                        else
                        {
                            num100 = 0.0f;
                        }

                        num103 = num100;
                    }

                    num104 = num103;
                }

                BodyShortPeriodTotal += num104;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0915:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num92;
                double num98;
                float num99;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num99 = inClose[i - 2];
                }
                else
                {
                    num99 = inOpen[i - 2];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num98 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    float num97;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num97 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num96;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num96 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num93;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                float num94;
                                float num95;
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

                                num93 = (inHigh[i - 2] - num95) + (num94 - inLow[i - 2]);
                            }
                            else
                            {
                                num93 = 0.0f;
                            }

                            num96 = num93;
                        }

                        num97 = num96;
                    }

                    num98 = num97;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num92 = 2.0;
                }
                else
                {
                    num92 = 1.0;
                }

                if (((inHigh[i - 2] - num99) < ((Globals.candleSettings[7].factor * num98) / num92)) && (inClose[i - 1] >= inOpen[i - 1]))
                {
                    double num84;
                    double num90;
                    float num91;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num91 = inClose[i - 1];
                    }
                    else
                    {
                        num91 = inOpen[i - 1];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num90 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        float num89;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num89 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            float num88;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num88 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                float num85;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    float num86;
                                    float num87;
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

                                    num85 = (inHigh[i - 1] - num87) + (num86 - inLow[i - 1]);
                                }
                                else
                                {
                                    num85 = 0.0f;
                                }

                                num88 = num85;
                            }

                            num89 = num88;
                        }

                        num90 = num89;
                    }

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num84 = 2.0;
                    }
                    else
                    {
                        num84 = 1.0;
                    }

                    if (((inHigh[i - 1] - num91) < ((Globals.candleSettings[7].factor * num90) / num84)) && (inClose[i] >= inOpen[i]))
                    {
                        double num76;
                        double num82;
                        float num83;
                        if (inClose[i] >= inOpen[i])
                        {
                            num83 = inClose[i];
                        }
                        else
                        {
                            num83 = inOpen[i];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num82 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            float num81;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num81 = Math.Abs((float) (inClose[i] - inOpen[i]));
                            }
                            else
                            {
                                float num80;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num80 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    float num77;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                    {
                                        float num78;
                                        float num79;
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

                                        num77 = (inHigh[i] - num79) + (num78 - inLow[i]);
                                    }
                                    else
                                    {
                                        num77 = 0.0f;
                                    }

                                    num80 = num77;
                                }

                                num81 = num80;
                            }

                            num82 = num81;
                        }

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num76 = 2.0;
                        }
                        else
                        {
                            num76 = 1.0;
                        }

                        if ((((inHigh[i] - num83) < ((Globals.candleSettings[7].factor * num82) / num76)) &&
                             (inClose[i] > inClose[i - 1])) && ((inClose[i - 1] > inClose[i - 2]) && (inOpen[i - 1] > inOpen[i - 2])))
                        {
                            double num69;
                            double num75;
                            if (Globals.candleSettings[8].avgPeriod != 0.0)
                            {
                                num75 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
                            }
                            else
                            {
                                float num74;
                                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                {
                                    num74 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                                }
                                else
                                {
                                    float num73;
                                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                    {
                                        num73 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        float num70;
                                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                        {
                                            float num71;
                                            float num72;
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

                                            num70 = (inHigh[i - 2] - num72) + (num71 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num70 = 0.0f;
                                        }

                                        num73 = num70;
                                    }

                                    num74 = num73;
                                }

                                num75 = num74;
                            }

                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                num69 = 2.0;
                            }
                            else
                            {
                                num69 = 1.0;
                            }

                            if ((inOpen[i - 1] <= (inClose[i - 2] + ((Globals.candleSettings[8].factor * num75) / num69))) &&
                                (inOpen[i] > inOpen[i - 1]))
                            {
                                double num62;
                                double num68;
                                if (Globals.candleSettings[8].avgPeriod != 0.0)
                                {
                                    num68 = NearPeriodTotal[1] / ((double) Globals.candleSettings[8].avgPeriod);
                                }
                                else
                                {
                                    float num67;
                                    if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                    {
                                        num67 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                                    }
                                    else
                                    {
                                        float num66;
                                        if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                        {
                                            num66 = inHigh[i - 1] - inLow[i - 1];
                                        }
                                        else
                                        {
                                            float num63;
                                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                            {
                                                float num64;
                                                float num65;
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

                                                num63 = (inHigh[i - 1] - num65) + (num64 - inLow[i - 1]);
                                            }
                                            else
                                            {
                                                num63 = 0.0f;
                                            }

                                            num66 = num63;
                                        }

                                        num67 = num66;
                                    }

                                    num68 = num67;
                                }

                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                {
                                    num62 = 2.0;
                                }
                                else
                                {
                                    num62 = 1.0;
                                }

                                if (inOpen[i] <= (inClose[i - 1] + ((Globals.candleSettings[8].factor * num68) / num62)))
                                {
                                    double num55;
                                    double num61;
                                    if (Globals.candleSettings[9].avgPeriod != 0.0)
                                    {
                                        num61 = FarPeriodTotal[2] / ((double) Globals.candleSettings[9].avgPeriod);
                                    }
                                    else
                                    {
                                        float num60;
                                        if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                                        {
                                            num60 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                                        }
                                        else
                                        {
                                            float num59;
                                            if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                                            {
                                                num59 = inHigh[i - 2] - inLow[i - 2];
                                            }
                                            else
                                            {
                                                float num56;
                                                if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                                                {
                                                    float num57;
                                                    float num58;
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

                                                    num56 = (inHigh[i - 2] - num58) + (num57 - inLow[i - 2]);
                                                }
                                                else
                                                {
                                                    num56 = 0.0f;
                                                }

                                                num59 = num56;
                                            }

                                            num60 = num59;
                                        }

                                        num61 = num60;
                                    }

                                    if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                                    {
                                        num55 = 2.0;
                                    }
                                    else
                                    {
                                        num55 = 1.0;
                                    }

                                    if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) >
                                        (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) -
                                         ((Globals.candleSettings[9].factor * num61) / num55)))
                                    {
                                        double num48;
                                        double num54;
                                        if (Globals.candleSettings[9].avgPeriod != 0.0)
                                        {
                                            num54 = FarPeriodTotal[1] / ((double) Globals.candleSettings[9].avgPeriod);
                                        }
                                        else
                                        {
                                            float num53;
                                            if (Globals.candleSettings[9].rangeType == RangeType.RealBody)
                                            {
                                                num53 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                                            }
                                            else
                                            {
                                                float num52;
                                                if (Globals.candleSettings[9].rangeType == RangeType.HighLow)
                                                {
                                                    num52 = inHigh[i - 1] - inLow[i - 1];
                                                }
                                                else
                                                {
                                                    float num49;
                                                    if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                                                    {
                                                        float num50;
                                                        float num51;
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

                                                        num49 = (inHigh[i - 1] - num51) + (num50 - inLow[i - 1]);
                                                    }
                                                    else
                                                    {
                                                        num49 = 0.0f;
                                                    }

                                                    num52 = num49;
                                                }

                                                num53 = num52;
                                            }

                                            num54 = num53;
                                        }

                                        if (Globals.candleSettings[9].rangeType == RangeType.Shadows)
                                        {
                                            num48 = 2.0;
                                        }
                                        else
                                        {
                                            num48 = 1.0;
                                        }

                                        if (Math.Abs((float) (inClose[i] - inOpen[i])) >
                                            (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) -
                                             ((Globals.candleSettings[9].factor * num54) / num48)))
                                        {
                                            double num41;
                                            double num47;
                                            if (Globals.candleSettings[2].avgPeriod != 0.0)
                                            {
                                                num47 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                                            }
                                            else
                                            {
                                                float num46;
                                                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                                                {
                                                    num46 = Math.Abs((float) (inClose[i] - inOpen[i]));
                                                }
                                                else
                                                {
                                                    float num45;
                                                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                                    {
                                                        num45 = inHigh[i] - inLow[i];
                                                    }
                                                    else
                                                    {
                                                        float num42;
                                                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                                        {
                                                            float num43;
                                                            float num44;
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

                                                            num42 = (inHigh[i] - num44) + (num43 - inLow[i]);
                                                        }
                                                        else
                                                        {
                                                            num42 = 0.0f;
                                                        }

                                                        num45 = num42;
                                                    }

                                                    num46 = num45;
                                                }

                                                num47 = num46;
                                            }

                                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                            {
                                                num41 = 2.0;
                                            }
                                            else
                                            {
                                                num41 = 1.0;
                                            }

                                            if (Math.Abs((float) (inClose[i] - inOpen[i])) >
                                                ((Globals.candleSettings[2].factor * num47) / num41))
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
            totIdx = 2;
            while (totIdx >= 0)
            {
                float num35;
                float num40;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num39;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num36;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs((float) (inClose[ShadowVeryShortTrailingIdx - totIdx] - inOpen[ShadowVeryShortTrailingIdx - totIdx]));
                }
                else
                {
                    float num34;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[ShadowVeryShortTrailingIdx - totIdx] - inLow[ShadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num31;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num32;
                            float num33;
                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num33 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num32 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            num31 = (inHigh[ShadowVeryShortTrailingIdx - totIdx] - num33) +
                                    (num32 - inLow[ShadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0f;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                ShadowVeryShortPeriodTotal[totIdx] += num40 - num35;
                totIdx--;
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

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num7;
                        float num8;
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

                        num6 = (inHigh[i] - num8) + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0f;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[BodyShortTrailingIdx] - inOpen[BodyShortTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyShortTrailingIdx] - inLow[BodyShortTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num3 = inClose[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[BodyShortTrailingIdx];
                        }

                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num2 = inOpen[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[BodyShortTrailingIdx];
                        }

                        num = (inHigh[BodyShortTrailingIdx] - num3) + (num2 - inLow[BodyShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyShortPeriodTotal += num10 - num5;
            i++;
            ShadowVeryShortTrailingIdx++;
            NearTrailingIdx++;
            FarTrailingIdx++;
            BodyShortTrailingIdx++;
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
            int num;
            int avgPeriod;
            if (Globals.candleSettings[9].avgPeriod > Globals.candleSettings[8].avgPeriod)
            {
                avgPeriod = Globals.candleSettings[9].avgPeriod;
            }
            else
            {
                avgPeriod = Globals.candleSettings[8].avgPeriod;
            }

            if (((Globals.candleSettings[7].avgPeriod <= Globals.candleSettings[2].avgPeriod)
                    ? Globals.candleSettings[2].avgPeriod
                    : Globals.candleSettings[7].avgPeriod) > avgPeriod)
            {
                num = (Globals.candleSettings[7].avgPeriod <= Globals.candleSettings[2].avgPeriod)
                    ? Globals.candleSettings[2].avgPeriod
                    : Globals.candleSettings[7].avgPeriod;
            }
            else
            {
                num = (Globals.candleSettings[9].avgPeriod <= Globals.candleSettings[8].avgPeriod)
                    ? Globals.candleSettings[8].avgPeriod
                    : Globals.candleSettings[9].avgPeriod;
            }

            return (num + 2);
        }
    }
}
