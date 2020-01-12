using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlStalledPattern(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double num5;
            double num10;
            double num15;
            double num20;
            double[] BodyLongPeriodTotal = new double[3];
            double[] NearPeriodTotal = new double[3];
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

            int lookbackTotal = CdlStalledPatternLookback();
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

            BodyLongPeriodTotal[2] = 0.0;
            BodyLongPeriodTotal[1] = 0.0;
            BodyLongPeriodTotal[0] = 0.0;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            double BodyShortPeriodTotal = 0.0;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            double ShadowVeryShortPeriodTotal = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            NearPeriodTotal[2] = 0.0;
            NearPeriodTotal[1] = 0.0;
            NearPeriodTotal[0] = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num108;
                double num113;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num113 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num112;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num112 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num109;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num110;
                            double num111;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num111 = inClose[i - 2];
                            }
                            else
                            {
                                num111 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num110 = inOpen[i - 2];
                            }
                            else
                            {
                                num110 = inClose[i - 2];
                            }

                            num109 = (inHigh[i - 2] - num111) + (num110 - inLow[i - 2]);
                        }
                        else
                        {
                            num109 = 0.0;
                        }

                        num112 = num109;
                    }

                    num113 = num112;
                }

                BodyLongPeriodTotal[2] += num113;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num108 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num107;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num107 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num104;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num105;
                            double num106;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num106 = inClose[i - 1];
                            }
                            else
                            {
                                num106 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num105 = inOpen[i - 1];
                            }
                            else
                            {
                                num105 = inClose[i - 1];
                            }

                            num104 = (inHigh[i - 1] - num106) + (num105 - inLow[i - 1]);
                        }
                        else
                        {
                            num104 = 0.0;
                        }

                        num107 = num104;
                    }

                    num108 = num107;
                }

                BodyLongPeriodTotal[1] += num108;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                double num103;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num103 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num102;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num102 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num99;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num100;
                            double num101;
                            if (inClose[i] >= inOpen[i])
                            {
                                num101 = inClose[i];
                            }
                            else
                            {
                                num101 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num100 = inOpen[i];
                            }
                            else
                            {
                                num100 = inClose[i];
                            }

                            num99 = (inHigh[i] - num101) + (num100 - inLow[i]);
                        }
                        else
                        {
                            num99 = 0.0;
                        }

                        num102 = num99;
                    }

                    num103 = num102;
                }

                BodyShortPeriodTotal += num103;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num98;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num98 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num97;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num97 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num94;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num95;
                            double num96;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num96 = inClose[i - 1];
                            }
                            else
                            {
                                num96 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num95 = inOpen[i - 1];
                            }
                            else
                            {
                                num95 = inClose[i - 1];
                            }

                            num94 = (inHigh[i - 1] - num96) + (num95 - inLow[i - 1]);
                        }
                        else
                        {
                            num94 = 0.0;
                        }

                        num97 = num94;
                    }

                    num98 = num97;
                }

                ShadowVeryShortPeriodTotal += num98;
                i++;
            }

            i = NearTrailingIdx;
            while (true)
            {
                double num88;
                double num93;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num93 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num92;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num92 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num89;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            double num90;
                            double num91;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num91 = inClose[i - 2];
                            }
                            else
                            {
                                num91 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num90 = inOpen[i - 2];
                            }
                            else
                            {
                                num90 = inClose[i - 2];
                            }

                            num89 = (inHigh[i - 2] - num91) + (num90 - inLow[i - 2]);
                        }
                        else
                        {
                            num89 = 0.0;
                        }

                        num92 = num89;
                    }

                    num93 = num92;
                }

                NearPeriodTotal[2] += num93;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num88 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num87;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num87 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num84;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            double num85;
                            double num86;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num86 = inClose[i - 1];
                            }
                            else
                            {
                                num86 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num85 = inOpen[i - 1];
                            }
                            else
                            {
                                num85 = inClose[i - 1];
                            }

                            num84 = (inHigh[i - 1] - num86) + (num85 - inLow[i - 1]);
                        }
                        else
                        {
                            num84 = 0.0;
                        }

                        num87 = num84;
                    }

                    num88 = num87;
                }

                NearPeriodTotal[1] += num88;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0656:
            if ((((inClose[i - 2] >= inOpen[i - 2]) && (inClose[i - 1] >= inOpen[i - 1])) && (inClose[i] >= inOpen[i])) &&
                ((inClose[i] > inClose[i - 1]) && (inClose[i - 1] > inClose[i - 2])))
            {
                double num77;
                double num83;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num83 = BodyLongPeriodTotal[2] / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    double num82;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num82 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num81;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num81 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num78;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                double num79;
                                double num80;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num80 = inClose[i - 2];
                                }
                                else
                                {
                                    num80 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num79 = inOpen[i - 2];
                                }
                                else
                                {
                                    num79 = inClose[i - 2];
                                }

                                num78 = (inHigh[i - 2] - num80) + (num79 - inLow[i - 2]);
                            }
                            else
                            {
                                num78 = 0.0;
                            }

                            num81 = num78;
                        }

                        num82 = num81;
                    }

                    num83 = num82;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num77 = 2.0;
                }
                else
                {
                    num77 = 1.0;
                }

                if (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num83) / num77))
                {
                    double num70;
                    double num76;
                    if (Globals.candleSettings[0].avgPeriod != 0.0)
                    {
                        num76 = BodyLongPeriodTotal[1] / ((double) Globals.candleSettings[0].avgPeriod);
                    }
                    else
                    {
                        double num75;
                        if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                        {
                            num75 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            double num74;
                            if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                            {
                                num74 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num71;
                                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                                {
                                    double num72;
                                    double num73;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num73 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num73 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num72 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num72 = inClose[i - 1];
                                    }

                                    num71 = (inHigh[i - 1] - num73) + (num72 - inLow[i - 1]);
                                }
                                else
                                {
                                    num71 = 0.0;
                                }

                                num74 = num71;
                            }

                            num75 = num74;
                        }

                        num76 = num75;
                    }

                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        num70 = 2.0;
                    }
                    else
                    {
                        num70 = 1.0;
                    }

                    if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) > ((Globals.candleSettings[0].factor * num76) / num70))
                    {
                        double num62;
                        double num68;
                        double num69;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num69 = inClose[i - 1];
                        }
                        else
                        {
                            num69 = inOpen[i - 1];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num68 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            double num67;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num67 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                double num66;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num66 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num63;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num62 = 2.0;
                        }
                        else
                        {
                            num62 = 1.0;
                        }

                        if (((inHigh[i - 1] - num69) < ((Globals.candleSettings[7].factor * num68) / num62)) &&
                            (inOpen[i - 1] > inOpen[i - 2]))
                        {
                            double num55;
                            double num61;
                            if (Globals.candleSettings[8].avgPeriod != 0.0)
                            {
                                num61 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
                            }
                            else
                            {
                                double num60;
                                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                {
                                    num60 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                                }
                                else
                                {
                                    double num59;
                                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                    {
                                        num59 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        double num56;
                                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                num55 = 2.0;
                            }
                            else
                            {
                                num55 = 1.0;
                            }

                            if (inOpen[i - 1] <= (inClose[i - 2] + ((Globals.candleSettings[8].factor * num61) / num55)))
                            {
                                double num48;
                                double num54;
                                if (Globals.candleSettings[2].avgPeriod != 0.0)
                                {
                                    num54 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                                }
                                else
                                {
                                    double num53;
                                    if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                                    {
                                        num53 = Math.Abs((double) (inClose[i] - inOpen[i]));
                                    }
                                    else
                                    {
                                        double num52;
                                        if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                        {
                                            num52 = inHigh[i] - inLow[i];
                                        }
                                        else
                                        {
                                            double num49;
                                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                                                num49 = (inHigh[i] - num51) + (num50 - inLow[i]);
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

                                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                {
                                    num48 = 2.0;
                                }
                                else
                                {
                                    num48 = 1.0;
                                }

                                if (Math.Abs((double) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num54) / num48))
                                {
                                    double num41;
                                    double num47;
                                    if (Globals.candleSettings[8].avgPeriod != 0.0)
                                    {
                                        num47 = NearPeriodTotal[1] / ((double) Globals.candleSettings[8].avgPeriod);
                                    }
                                    else
                                    {
                                        double num46;
                                        if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                        {
                                            num46 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                                        }
                                        else
                                        {
                                            double num45;
                                            if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                            {
                                                num45 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                double num42;
                                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                                                    num42 = (inHigh[i - 1] - num44) + (num43 - inLow[i - 1]);
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

                                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                    {
                                        num41 = 2.0;
                                    }
                                    else
                                    {
                                        num41 = 1.0;
                                    }

                                    if (inOpen[i] >= ((inClose[i - 1] - Math.Abs((double) (inClose[i] - inOpen[i]))) -
                                                      ((Globals.candleSettings[8].factor * num47) / num41)))
                                    {
                                        outInteger[outIdx] = -100;
                                        outIdx++;
                                        goto Label_0F20;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0F20:
            totIdx = 2;
            while (totIdx >= 1)
            {
                double num25;
                double num30;
                double num35;
                double num40;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num39;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num36;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs((double) (inClose[BodyLongTrailingIdx - totIdx] - inOpen[BodyLongTrailingIdx - totIdx]));
                }
                else
                {
                    double num34;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[BodyLongTrailingIdx - totIdx] - inLow[BodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num31;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num32;
                            double num33;
                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num33 = inClose[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[BodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num32 = inOpen[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[BodyLongTrailingIdx - totIdx];
                            }

                            num31 = (inHigh[BodyLongTrailingIdx - totIdx] - num33) + (num32 - inLow[BodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                BodyLongPeriodTotal[totIdx] += num40 - num35;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num29;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num26;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs((double) (inClose[NearTrailingIdx - totIdx] - inOpen[NearTrailingIdx - totIdx]));
                }
                else
                {
                    double num24;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[NearTrailingIdx - totIdx] - inLow[NearTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num21;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            double num22;
                            double num23;
                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num23 = inClose[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num23 = inOpen[NearTrailingIdx - totIdx];
                            }

                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num22 = inOpen[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num22 = inClose[NearTrailingIdx - totIdx];
                            }

                            num21 = (inHigh[NearTrailingIdx - totIdx] - num23) + (num22 - inLow[NearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num21 = 0.0;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                NearPeriodTotal[totIdx] += num30 - num25;
                totIdx--;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                        num16 = (inHigh[i] - num18) + (num17 - inLow[i]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((double) (inClose[BodyShortTrailingIdx] - inOpen[BodyShortTrailingIdx]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyShortTrailingIdx] - inLow[BodyShortTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num13 = inClose[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[BodyShortTrailingIdx];
                        }

                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num12 = inOpen[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[BodyShortTrailingIdx];
                        }

                        num11 = (inHigh[BodyShortTrailingIdx] - num13) + (num12 - inLow[BodyShortTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyShortPeriodTotal += num20 - num15;
            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                        num6 = (inHigh[i - 1] - num8) + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[ShadowVeryShortTrailingIdx - 1] - inOpen[ShadowVeryShortTrailingIdx - 1]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[ShadowVeryShortTrailingIdx - 1] - inLow[ShadowVeryShortTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[ShadowVeryShortTrailingIdx - 1] >= inOpen[ShadowVeryShortTrailingIdx - 1])
                        {
                            num3 = inClose[ShadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[ShadowVeryShortTrailingIdx - 1];
                        }

                        if (inClose[ShadowVeryShortTrailingIdx - 1] >= inOpen[ShadowVeryShortTrailingIdx - 1])
                        {
                            num2 = inOpen[ShadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[ShadowVeryShortTrailingIdx - 1];
                        }

                        num = (inHigh[ShadowVeryShortTrailingIdx - 1] - num3) + (num2 - inLow[ShadowVeryShortTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            ShadowVeryShortPeriodTotal += num10 - num5;
            i++;
            BodyLongTrailingIdx++;
            BodyShortTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            NearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0656;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlStalledPattern(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            float num5;
            float num10;
            float num15;
            float num20;
            double[] BodyLongPeriodTotal = new double[3];
            double[] NearPeriodTotal = new double[3];
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

            int lookbackTotal = CdlStalledPatternLookback();
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

            BodyLongPeriodTotal[2] = 0.0;
            BodyLongPeriodTotal[1] = 0.0;
            BodyLongPeriodTotal[0] = 0.0;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            double BodyShortPeriodTotal = 0.0;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            double ShadowVeryShortPeriodTotal = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            NearPeriodTotal[2] = 0.0;
            NearPeriodTotal[1] = 0.0;
            NearPeriodTotal[0] = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num108;
                float num113;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num113 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num112;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num112 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num109;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num110;
                            float num111;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num111 = inClose[i - 2];
                            }
                            else
                            {
                                num111 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num110 = inOpen[i - 2];
                            }
                            else
                            {
                                num110 = inClose[i - 2];
                            }

                            num109 = (inHigh[i - 2] - num111) + (num110 - inLow[i - 2]);
                        }
                        else
                        {
                            num109 = 0.0f;
                        }

                        num112 = num109;
                    }

                    num113 = num112;
                }

                BodyLongPeriodTotal[2] += num113;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num108 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num107;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num107 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num104;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num105;
                            float num106;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num106 = inClose[i - 1];
                            }
                            else
                            {
                                num106 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num105 = inOpen[i - 1];
                            }
                            else
                            {
                                num105 = inClose[i - 1];
                            }

                            num104 = (inHigh[i - 1] - num106) + (num105 - inLow[i - 1]);
                        }
                        else
                        {
                            num104 = 0.0f;
                        }

                        num107 = num104;
                    }

                    num108 = num107;
                }

                BodyLongPeriodTotal[1] += num108;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                float num103;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num103 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num102;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num102 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num99;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num100;
                            float num101;
                            if (inClose[i] >= inOpen[i])
                            {
                                num101 = inClose[i];
                            }
                            else
                            {
                                num101 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num100 = inOpen[i];
                            }
                            else
                            {
                                num100 = inClose[i];
                            }

                            num99 = (inHigh[i] - num101) + (num100 - inLow[i]);
                        }
                        else
                        {
                            num99 = 0.0f;
                        }

                        num102 = num99;
                    }

                    num103 = num102;
                }

                BodyShortPeriodTotal += num103;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num98;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num98 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num97;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num97 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num94;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num95;
                            float num96;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num96 = inClose[i - 1];
                            }
                            else
                            {
                                num96 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num95 = inOpen[i - 1];
                            }
                            else
                            {
                                num95 = inClose[i - 1];
                            }

                            num94 = (inHigh[i - 1] - num96) + (num95 - inLow[i - 1]);
                        }
                        else
                        {
                            num94 = 0.0f;
                        }

                        num97 = num94;
                    }

                    num98 = num97;
                }

                ShadowVeryShortPeriodTotal += num98;
                i++;
            }

            i = NearTrailingIdx;
            while (true)
            {
                float num88;
                float num93;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num93 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num92;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num92 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num89;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num90;
                            float num91;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num91 = inClose[i - 2];
                            }
                            else
                            {
                                num91 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num90 = inOpen[i - 2];
                            }
                            else
                            {
                                num90 = inClose[i - 2];
                            }

                            num89 = (inHigh[i - 2] - num91) + (num90 - inLow[i - 2]);
                        }
                        else
                        {
                            num89 = 0.0f;
                        }

                        num92 = num89;
                    }

                    num93 = num92;
                }

                NearPeriodTotal[2] += num93;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num88 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num87;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num87 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num84;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num85;
                            float num86;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num86 = inClose[i - 1];
                            }
                            else
                            {
                                num86 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num85 = inOpen[i - 1];
                            }
                            else
                            {
                                num85 = inClose[i - 1];
                            }

                            num84 = (inHigh[i - 1] - num86) + (num85 - inLow[i - 1]);
                        }
                        else
                        {
                            num84 = 0.0f;
                        }

                        num87 = num84;
                    }

                    num88 = num87;
                }

                NearPeriodTotal[1] += num88;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_06AA:
            if ((((inClose[i - 2] >= inOpen[i - 2]) && (inClose[i - 1] >= inOpen[i - 1])) && (inClose[i] >= inOpen[i])) &&
                ((inClose[i] > inClose[i - 1]) && (inClose[i - 1] > inClose[i - 2])))
            {
                double num77;
                double num83;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num83 = BodyLongPeriodTotal[2] / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    float num82;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num82 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num81;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num81 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num78;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                float num79;
                                float num80;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num80 = inClose[i - 2];
                                }
                                else
                                {
                                    num80 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num79 = inOpen[i - 2];
                                }
                                else
                                {
                                    num79 = inClose[i - 2];
                                }

                                num78 = (inHigh[i - 2] - num80) + (num79 - inLow[i - 2]);
                            }
                            else
                            {
                                num78 = 0.0f;
                            }

                            num81 = num78;
                        }

                        num82 = num81;
                    }

                    num83 = num82;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num77 = 2.0;
                }
                else
                {
                    num77 = 1.0;
                }

                if (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num83) / num77))
                {
                    double num70;
                    double num76;
                    if (Globals.candleSettings[0].avgPeriod != 0.0)
                    {
                        num76 = BodyLongPeriodTotal[1] / ((double) Globals.candleSettings[0].avgPeriod);
                    }
                    else
                    {
                        float num75;
                        if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                        {
                            num75 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            float num74;
                            if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                            {
                                num74 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                float num71;
                                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                                {
                                    float num72;
                                    float num73;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num73 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num73 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num72 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num72 = inClose[i - 1];
                                    }

                                    num71 = (inHigh[i - 1] - num73) + (num72 - inLow[i - 1]);
                                }
                                else
                                {
                                    num71 = 0.0f;
                                }

                                num74 = num71;
                            }

                            num75 = num74;
                        }

                        num76 = num75;
                    }

                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        num70 = 2.0;
                    }
                    else
                    {
                        num70 = 1.0;
                    }

                    if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) > ((Globals.candleSettings[0].factor * num76) / num70))
                    {
                        double num62;
                        double num68;
                        float num69;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num69 = inClose[i - 1];
                        }
                        else
                        {
                            num69 = inOpen[i - 1];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num68 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            float num67;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num67 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                float num66;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num66 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    float num63;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num62 = 2.0;
                        }
                        else
                        {
                            num62 = 1.0;
                        }

                        if (((inHigh[i - 1] - num69) < ((Globals.candleSettings[7].factor * num68) / num62)) &&
                            (inOpen[i - 1] > inOpen[i - 2]))
                        {
                            double num55;
                            double num61;
                            if (Globals.candleSettings[8].avgPeriod != 0.0)
                            {
                                num61 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
                            }
                            else
                            {
                                float num60;
                                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                {
                                    num60 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                                }
                                else
                                {
                                    float num59;
                                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                    {
                                        num59 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        float num56;
                                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                num55 = 2.0;
                            }
                            else
                            {
                                num55 = 1.0;
                            }

                            if (inOpen[i - 1] <= (inClose[i - 2] + ((Globals.candleSettings[8].factor * num61) / num55)))
                            {
                                double num48;
                                double num54;
                                if (Globals.candleSettings[2].avgPeriod != 0.0)
                                {
                                    num54 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                                }
                                else
                                {
                                    float num53;
                                    if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                                    {
                                        num53 = Math.Abs((float) (inClose[i] - inOpen[i]));
                                    }
                                    else
                                    {
                                        float num52;
                                        if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                        {
                                            num52 = inHigh[i] - inLow[i];
                                        }
                                        else
                                        {
                                            float num49;
                                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                            {
                                                float num50;
                                                float num51;
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

                                                num49 = (inHigh[i] - num51) + (num50 - inLow[i]);
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

                                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                {
                                    num48 = 2.0;
                                }
                                else
                                {
                                    num48 = 1.0;
                                }

                                if (Math.Abs((float) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num54) / num48))
                                {
                                    double num41;
                                    double num47;
                                    if (Globals.candleSettings[8].avgPeriod != 0.0)
                                    {
                                        num47 = NearPeriodTotal[1] / ((double) Globals.candleSettings[8].avgPeriod);
                                    }
                                    else
                                    {
                                        float num46;
                                        if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                        {
                                            num46 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                                        }
                                        else
                                        {
                                            float num45;
                                            if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                            {
                                                num45 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                float num42;
                                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                                {
                                                    float num43;
                                                    float num44;
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

                                                    num42 = (inHigh[i - 1] - num44) + (num43 - inLow[i - 1]);
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

                                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                    {
                                        num41 = 2.0;
                                    }
                                    else
                                    {
                                        num41 = 1.0;
                                    }

                                    if (inOpen[i] >= ((inClose[i - 1] - Math.Abs((float) (inClose[i] - inOpen[i]))) -
                                                      ((Globals.candleSettings[8].factor * num47) / num41)))
                                    {
                                        outInteger[outIdx] = -100;
                                        outIdx++;
                                        goto Label_0FEC;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0FEC:
            totIdx = 2;
            while (totIdx >= 1)
            {
                float num25;
                float num30;
                float num35;
                float num40;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num39;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num36;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs((float) (inClose[BodyLongTrailingIdx - totIdx] - inOpen[BodyLongTrailingIdx - totIdx]));
                }
                else
                {
                    float num34;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[BodyLongTrailingIdx - totIdx] - inLow[BodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num31;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num32;
                            float num33;
                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num33 = inClose[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[BodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num32 = inOpen[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[BodyLongTrailingIdx - totIdx];
                            }

                            num31 = (inHigh[BodyLongTrailingIdx - totIdx] - num33) + (num32 - inLow[BodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0f;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                BodyLongPeriodTotal[totIdx] += num40 - num35;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num29;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num26;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs((float) (inClose[NearTrailingIdx - totIdx] - inOpen[NearTrailingIdx - totIdx]));
                }
                else
                {
                    float num24;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[NearTrailingIdx - totIdx] - inLow[NearTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num21;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num22;
                            float num23;
                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num23 = inClose[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num23 = inOpen[NearTrailingIdx - totIdx];
                            }

                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num22 = inOpen[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num22 = inClose[NearTrailingIdx - totIdx];
                            }

                            num21 = (inHigh[NearTrailingIdx - totIdx] - num23) + (num22 - inLow[NearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num21 = 0.0f;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                NearPeriodTotal[totIdx] += num30 - num25;
                totIdx--;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num17;
                        float num18;
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

                        num16 = (inHigh[i] - num18) + (num17 - inLow[i]);
                    }
                    else
                    {
                        num16 = 0.0f;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((float) (inClose[BodyShortTrailingIdx] - inOpen[BodyShortTrailingIdx]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyShortTrailingIdx] - inLow[BodyShortTrailingIdx];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num13 = inClose[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[BodyShortTrailingIdx];
                        }

                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num12 = inOpen[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[BodyShortTrailingIdx];
                        }

                        num11 = (inHigh[BodyShortTrailingIdx] - num13) + (num12 - inLow[BodyShortTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyShortPeriodTotal += num20 - num15;
            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        float num7;
                        float num8;
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

                        num6 = (inHigh[i - 1] - num8) + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = 0.0f;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[ShadowVeryShortTrailingIdx - 1] - inOpen[ShadowVeryShortTrailingIdx - 1]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[ShadowVeryShortTrailingIdx - 1] - inLow[ShadowVeryShortTrailingIdx - 1];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[ShadowVeryShortTrailingIdx - 1] >= inOpen[ShadowVeryShortTrailingIdx - 1])
                        {
                            num3 = inClose[ShadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[ShadowVeryShortTrailingIdx - 1];
                        }

                        if (inClose[ShadowVeryShortTrailingIdx - 1] >= inOpen[ShadowVeryShortTrailingIdx - 1])
                        {
                            num2 = inOpen[ShadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[ShadowVeryShortTrailingIdx - 1];
                        }

                        num = (inHigh[ShadowVeryShortTrailingIdx - 1] - num3) + (num2 - inLow[ShadowVeryShortTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            ShadowVeryShortPeriodTotal += num10 - num5;
            i++;
            BodyLongTrailingIdx++;
            BodyShortTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            NearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_06AA;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlStalledPatternLookback()
        {
            int num;
            int avgPeriod;
            if (Globals.candleSettings[7].avgPeriod > Globals.candleSettings[8].avgPeriod)
            {
                avgPeriod = Globals.candleSettings[7].avgPeriod;
            }
            else
            {
                avgPeriod = Globals.candleSettings[8].avgPeriod;
            }

            if (((Globals.candleSettings[0].avgPeriod <= Globals.candleSettings[2].avgPeriod)
                    ? Globals.candleSettings[2].avgPeriod
                    : Globals.candleSettings[0].avgPeriod) > avgPeriod)
            {
                num = (Globals.candleSettings[0].avgPeriod <= Globals.candleSettings[2].avgPeriod)
                    ? Globals.candleSettings[2].avgPeriod
                    : Globals.candleSettings[0].avgPeriod;
            }
            else
            {
                num = (Globals.candleSettings[7].avgPeriod <= Globals.candleSettings[8].avgPeriod)
                    ? Globals.candleSettings[8].avgPeriod
                    : Globals.candleSettings[7].avgPeriod;
            }

            return (num + 2);
        }
    }
}
