using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlIdentical3Crows(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double[] ShadowVeryShortPeriodTotal = new double[3];
            double[] EqualPeriodTotal = new double[3];
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

            int lookbackTotal = CdlIdentical3CrowsLookback();
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
            EqualPeriodTotal[2] = 0.0;
            EqualPeriodTotal[1] = 0.0;
            EqualPeriodTotal[0] = 0.0;
            int EqualTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            int i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num87;
                double num92;
                double num97;
                if (i >= startIdx)
                {
                    break;
                }

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

                ShadowVeryShortPeriodTotal[2] += num97;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num92 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num91;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num91 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num88;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num89;
                            double num90;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num90 = inClose[i - 1];
                            }
                            else
                            {
                                num90 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num89 = inOpen[i - 1];
                            }
                            else
                            {
                                num89 = inClose[i - 1];
                            }

                            num88 = (inHigh[i - 1] - num90) + (num89 - inLow[i - 1]);
                        }
                        else
                        {
                            num88 = 0.0;
                        }

                        num91 = num88;
                    }

                    num92 = num91;
                }

                ShadowVeryShortPeriodTotal[1] += num92;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num87 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num86;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num86 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num83;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num84;
                            double num85;
                            if (inClose[i] >= inOpen[i])
                            {
                                num85 = inClose[i];
                            }
                            else
                            {
                                num85 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num84 = inOpen[i];
                            }
                            else
                            {
                                num84 = inClose[i];
                            }

                            num83 = (inHigh[i] - num85) + (num84 - inLow[i]);
                        }
                        else
                        {
                            num83 = 0.0;
                        }

                        num86 = num83;
                    }

                    num87 = num86;
                }

                ShadowVeryShortPeriodTotal[0] += num87;
                i++;
            }

            i = EqualTrailingIdx;
            while (true)
            {
                double num77;
                double num82;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num82 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num81;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num81 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num78;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
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

                EqualPeriodTotal[2] += num82;
                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num77 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num76;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num76 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num73;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            double num74;
                            double num75;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num75 = inClose[i - 1];
                            }
                            else
                            {
                                num75 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num74 = inOpen[i - 1];
                            }
                            else
                            {
                                num74 = inClose[i - 1];
                            }

                            num73 = (inHigh[i - 1] - num75) + (num74 - inLow[i - 1]);
                        }
                        else
                        {
                            num73 = 0.0;
                        }

                        num76 = num73;
                    }

                    num77 = num76;
                }

                EqualPeriodTotal[1] += num77;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0529:
            if (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1)
            {
                double num65;
                double num71;
                double num72;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num72 = inOpen[i - 2];
                }
                else
                {
                    num72 = inClose[i - 2];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num71 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    double num70;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num70 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num69;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num69 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num66;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                double num67;
                                double num68;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num68 = inClose[i - 2];
                                }
                                else
                                {
                                    num68 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num67 = inOpen[i - 2];
                                }
                                else
                                {
                                    num67 = inClose[i - 2];
                                }

                                num66 = (inHigh[i - 2] - num68) + (num67 - inLow[i - 2]);
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

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num65 = 2.0;
                }
                else
                {
                    num65 = 1.0;
                }

                if (((num72 - inLow[i - 2]) < ((Globals.candleSettings[7].factor * num71) / num65)) &&
                    (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1))
                {
                    double num57;
                    double num63;
                    double num64;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num64 = inOpen[i - 1];
                    }
                    else
                    {
                        num64 = inClose[i - 1];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num63 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        double num62;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num62 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            double num61;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num61 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num58;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    double num59;
                                    double num60;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num60 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num60 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num59 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num59 = inClose[i - 1];
                                    }

                                    num58 = (inHigh[i - 1] - num60) + (num59 - inLow[i - 1]);
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

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num57 = 2.0;
                    }
                    else
                    {
                        num57 = 1.0;
                    }

                    if (((num64 - inLow[i - 1]) < ((Globals.candleSettings[7].factor * num63) / num57)) &&
                        (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
                    {
                        double num49;
                        double num55;
                        double num56;
                        if (inClose[i] >= inOpen[i])
                        {
                            num56 = inOpen[i];
                        }
                        else
                        {
                            num56 = inClose[i];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num55 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            double num54;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num54 = Math.Abs((double) (inClose[i] - inOpen[i]));
                            }
                            else
                            {
                                double num53;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num53 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    double num50;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                                        num50 = (inHigh[i] - num52) + (num51 - inLow[i]);
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

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num49 = 2.0;
                        }
                        else
                        {
                            num49 = 1.0;
                        }

                        if ((((num56 - inLow[i]) < ((Globals.candleSettings[7].factor * num55) / num49)) &&
                             (inClose[i - 2] > inClose[i - 1])) && (inClose[i - 1] > inClose[i]))
                        {
                            double num42;
                            double num48;
                            if (Globals.candleSettings[10].avgPeriod != 0.0)
                            {
                                num48 = EqualPeriodTotal[2] / ((double) Globals.candleSettings[10].avgPeriod);
                            }
                            else
                            {
                                double num47;
                                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                {
                                    num47 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                                }
                                else
                                {
                                    double num46;
                                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                    {
                                        num46 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        double num43;
                                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                        {
                                            double num44;
                                            double num45;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num45 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num45 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num44 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num44 = inClose[i - 2];
                                            }

                                            num43 = (inHigh[i - 2] - num45) + (num44 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num43 = 0.0;
                                        }

                                        num46 = num43;
                                    }

                                    num47 = num46;
                                }

                                num48 = num47;
                            }

                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                num42 = 2.0;
                            }
                            else
                            {
                                num42 = 1.0;
                            }

                            if (inOpen[i - 1] <= (inClose[i - 2] + ((Globals.candleSettings[10].factor * num48) / num42)))
                            {
                                double num35;
                                double num41;
                                if (Globals.candleSettings[10].avgPeriod != 0.0)
                                {
                                    num41 = EqualPeriodTotal[2] / ((double) Globals.candleSettings[10].avgPeriod);
                                }
                                else
                                {
                                    double num40;
                                    if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                    {
                                        num40 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                                    }
                                    else
                                    {
                                        double num39;
                                        if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                        {
                                            num39 = inHigh[i - 2] - inLow[i - 2];
                                        }
                                        else
                                        {
                                            double num36;
                                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                            {
                                                double num37;
                                                double num38;
                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num38 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num38 = inOpen[i - 2];
                                                }

                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num37 = inOpen[i - 2];
                                                }
                                                else
                                                {
                                                    num37 = inClose[i - 2];
                                                }

                                                num36 = (inHigh[i - 2] - num38) + (num37 - inLow[i - 2]);
                                            }
                                            else
                                            {
                                                num36 = 0.0;
                                            }

                                            num39 = num36;
                                        }

                                        num40 = num39;
                                    }

                                    num41 = num40;
                                }

                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                {
                                    num35 = 2.0;
                                }
                                else
                                {
                                    num35 = 1.0;
                                }

                                if (inOpen[i - 1] >= (inClose[i - 2] - ((Globals.candleSettings[10].factor * num41) / num35)))
                                {
                                    double num28;
                                    double num34;
                                    if (Globals.candleSettings[10].avgPeriod != 0.0)
                                    {
                                        num34 = EqualPeriodTotal[1] / ((double) Globals.candleSettings[10].avgPeriod);
                                    }
                                    else
                                    {
                                        double num33;
                                        if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                        {
                                            num33 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                                        }
                                        else
                                        {
                                            double num32;
                                            if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                            {
                                                num32 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                double num29;
                                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                                {
                                                    double num30;
                                                    double num31;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num31 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num31 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num30 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num30 = inClose[i - 1];
                                                    }

                                                    num29 = (inHigh[i - 1] - num31) + (num30 - inLow[i - 1]);
                                                }
                                                else
                                                {
                                                    num29 = 0.0;
                                                }

                                                num32 = num29;
                                            }

                                            num33 = num32;
                                        }

                                        num34 = num33;
                                    }

                                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                    {
                                        num28 = 2.0;
                                    }
                                    else
                                    {
                                        num28 = 1.0;
                                    }

                                    if (inOpen[i] <= (inClose[i - 1] + ((Globals.candleSettings[10].factor * num34) / num28)))
                                    {
                                        double num21;
                                        double num27;
                                        if (Globals.candleSettings[10].avgPeriod != 0.0)
                                        {
                                            num27 = EqualPeriodTotal[1] / ((double) Globals.candleSettings[10].avgPeriod);
                                        }
                                        else
                                        {
                                            double num26;
                                            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                            {
                                                num26 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                                            }
                                            else
                                            {
                                                double num25;
                                                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                                {
                                                    num25 = inHigh[i - 1] - inLow[i - 1];
                                                }
                                                else
                                                {
                                                    double num22;
                                                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                                    {
                                                        double num23;
                                                        double num24;
                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num24 = inClose[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num24 = inOpen[i - 1];
                                                        }

                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num23 = inOpen[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num23 = inClose[i - 1];
                                                        }

                                                        num22 = (inHigh[i - 1] - num24) + (num23 - inLow[i - 1]);
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

                                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                        {
                                            num21 = 2.0;
                                        }
                                        else
                                        {
                                            num21 = 1.0;
                                        }

                                        if (inOpen[i] >= (inClose[i - 1] - ((Globals.candleSettings[10].factor * num27) / num21)))
                                        {
                                            outInteger[outIdx] = -100;
                                            outIdx++;
                                            goto Label_0F74;
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
            Label_0F74:
            totIdx = 2;
            while (totIdx >= 0)
            {
                double num15;
                double num20;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num19;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num16;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs((double) (inClose[ShadowVeryShortTrailingIdx - totIdx] - inOpen[ShadowVeryShortTrailingIdx - totIdx]));
                }
                else
                {
                    double num14;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[ShadowVeryShortTrailingIdx - totIdx] - inLow[ShadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num11;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num12;
                            double num13;
                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num13 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num12 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            num11 = (inHigh[ShadowVeryShortTrailingIdx - totIdx] - num13) +
                                    (num12 - inLow[ShadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                ShadowVeryShortPeriodTotal[totIdx] += num20 - num15;
                totIdx--;
            }

            for (totIdx = 2; totIdx >= 1; totIdx--)
            {
                double num5;
                double num10;
                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num9;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            double num7;
                            double num8;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num8 = inClose[i - totIdx];
                            }
                            else
                            {
                                num8 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num7 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num7 = inClose[i - totIdx];
                            }

                            num6 = (inHigh[i - totIdx] - num8) + (num7 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num6 = 0.0;
                        }

                        num9 = num6;
                    }

                    num10 = num9;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((double) (inClose[EqualTrailingIdx - totIdx] - inOpen[EqualTrailingIdx - totIdx]));
                }
                else
                {
                    double num4;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[EqualTrailingIdx - totIdx] - inLow[EqualTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[EqualTrailingIdx - totIdx] >= inOpen[EqualTrailingIdx - totIdx])
                            {
                                num3 = inClose[EqualTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[EqualTrailingIdx - totIdx];
                            }

                            if (inClose[EqualTrailingIdx - totIdx] >= inOpen[EqualTrailingIdx - totIdx])
                            {
                                num2 = inOpen[EqualTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[EqualTrailingIdx - totIdx];
                            }

                            num = (inHigh[EqualTrailingIdx - totIdx] - num3) + (num2 - inLow[EqualTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                EqualPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            ShadowVeryShortTrailingIdx++;
            EqualTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0529;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlIdentical3Crows(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double[] ShadowVeryShortPeriodTotal = new double[3];
            double[] EqualPeriodTotal = new double[3];
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

            int lookbackTotal = CdlIdentical3CrowsLookback();
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
            EqualPeriodTotal[2] = 0.0;
            EqualPeriodTotal[1] = 0.0;
            EqualPeriodTotal[0] = 0.0;
            int EqualTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            int i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num87;
                float num92;
                float num97;
                if (i >= startIdx)
                {
                    break;
                }

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

                ShadowVeryShortPeriodTotal[2] += num97;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num92 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num91;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num91 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num88;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num89;
                            float num90;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num90 = inClose[i - 1];
                            }
                            else
                            {
                                num90 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num89 = inOpen[i - 1];
                            }
                            else
                            {
                                num89 = inClose[i - 1];
                            }

                            num88 = (inHigh[i - 1] - num90) + (num89 - inLow[i - 1]);
                        }
                        else
                        {
                            num88 = 0.0f;
                        }

                        num91 = num88;
                    }

                    num92 = num91;
                }

                ShadowVeryShortPeriodTotal[1] += num92;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num87 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num86;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num86 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num83;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num84;
                            float num85;
                            if (inClose[i] >= inOpen[i])
                            {
                                num85 = inClose[i];
                            }
                            else
                            {
                                num85 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num84 = inOpen[i];
                            }
                            else
                            {
                                num84 = inClose[i];
                            }

                            num83 = (inHigh[i] - num85) + (num84 - inLow[i]);
                        }
                        else
                        {
                            num83 = 0.0f;
                        }

                        num86 = num83;
                    }

                    num87 = num86;
                }

                ShadowVeryShortPeriodTotal[0] += num87;
                i++;
            }

            i = EqualTrailingIdx;
            while (true)
            {
                float num77;
                float num82;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num82 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num81;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num81 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num78;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
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

                EqualPeriodTotal[2] += num82;
                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num77 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num76;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num76 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num73;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            float num74;
                            float num75;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num75 = inClose[i - 1];
                            }
                            else
                            {
                                num75 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num74 = inOpen[i - 1];
                            }
                            else
                            {
                                num74 = inClose[i - 1];
                            }

                            num73 = (inHigh[i - 1] - num75) + (num74 - inLow[i - 1]);
                        }
                        else
                        {
                            num73 = 0.0f;
                        }

                        num76 = num73;
                    }

                    num77 = num76;
                }

                EqualPeriodTotal[1] += num77;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_056F:
            if (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1)
            {
                double num65;
                double num71;
                float num72;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num72 = inOpen[i - 2];
                }
                else
                {
                    num72 = inClose[i - 2];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num71 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    float num70;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num70 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num69;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num69 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num66;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                float num67;
                                float num68;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num68 = inClose[i - 2];
                                }
                                else
                                {
                                    num68 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num67 = inOpen[i - 2];
                                }
                                else
                                {
                                    num67 = inClose[i - 2];
                                }

                                num66 = (inHigh[i - 2] - num68) + (num67 - inLow[i - 2]);
                            }
                            else
                            {
                                num66 = 0.0f;
                            }

                            num69 = num66;
                        }

                        num70 = num69;
                    }

                    num71 = num70;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num65 = 2.0;
                }
                else
                {
                    num65 = 1.0;
                }

                if (((num72 - inLow[i - 2]) < ((Globals.candleSettings[7].factor * num71) / num65)) &&
                    (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1))
                {
                    double num57;
                    double num63;
                    float num64;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num64 = inOpen[i - 1];
                    }
                    else
                    {
                        num64 = inClose[i - 1];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num63 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        float num62;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num62 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            float num61;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num61 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                float num58;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    float num59;
                                    float num60;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num60 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num60 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num59 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num59 = inClose[i - 1];
                                    }

                                    num58 = (inHigh[i - 1] - num60) + (num59 - inLow[i - 1]);
                                }
                                else
                                {
                                    num58 = 0.0f;
                                }

                                num61 = num58;
                            }

                            num62 = num61;
                        }

                        num63 = num62;
                    }

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num57 = 2.0;
                    }
                    else
                    {
                        num57 = 1.0;
                    }

                    if (((num64 - inLow[i - 1]) < ((Globals.candleSettings[7].factor * num63) / num57)) &&
                        (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
                    {
                        double num49;
                        double num55;
                        float num56;
                        if (inClose[i] >= inOpen[i])
                        {
                            num56 = inOpen[i];
                        }
                        else
                        {
                            num56 = inClose[i];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num55 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            float num54;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num54 = Math.Abs((float) (inClose[i] - inOpen[i]));
                            }
                            else
                            {
                                float num53;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num53 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    float num50;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                    {
                                        float num51;
                                        float num52;
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

                                        num50 = (inHigh[i] - num52) + (num51 - inLow[i]);
                                    }
                                    else
                                    {
                                        num50 = 0.0f;
                                    }

                                    num53 = num50;
                                }

                                num54 = num53;
                            }

                            num55 = num54;
                        }

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num49 = 2.0;
                        }
                        else
                        {
                            num49 = 1.0;
                        }

                        if ((((num56 - inLow[i]) < ((Globals.candleSettings[7].factor * num55) / num49)) &&
                             (inClose[i - 2] > inClose[i - 1])) && (inClose[i - 1] > inClose[i]))
                        {
                            double num42;
                            double num48;
                            if (Globals.candleSettings[10].avgPeriod != 0.0)
                            {
                                num48 = EqualPeriodTotal[2] / ((double) Globals.candleSettings[10].avgPeriod);
                            }
                            else
                            {
                                float num47;
                                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                {
                                    num47 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                                }
                                else
                                {
                                    float num46;
                                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                    {
                                        num46 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        float num43;
                                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                        {
                                            float num44;
                                            float num45;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num45 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num45 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num44 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num44 = inClose[i - 2];
                                            }

                                            num43 = (inHigh[i - 2] - num45) + (num44 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num43 = 0.0f;
                                        }

                                        num46 = num43;
                                    }

                                    num47 = num46;
                                }

                                num48 = num47;
                            }

                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                num42 = 2.0;
                            }
                            else
                            {
                                num42 = 1.0;
                            }

                            if (inOpen[i - 1] <= (inClose[i - 2] + ((Globals.candleSettings[10].factor * num48) / num42)))
                            {
                                double num35;
                                double num41;
                                if (Globals.candleSettings[10].avgPeriod != 0.0)
                                {
                                    num41 = EqualPeriodTotal[2] / ((double) Globals.candleSettings[10].avgPeriod);
                                }
                                else
                                {
                                    float num40;
                                    if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                    {
                                        num40 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                                    }
                                    else
                                    {
                                        float num39;
                                        if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                        {
                                            num39 = inHigh[i - 2] - inLow[i - 2];
                                        }
                                        else
                                        {
                                            float num36;
                                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                            {
                                                float num37;
                                                float num38;
                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num38 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num38 = inOpen[i - 2];
                                                }

                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num37 = inOpen[i - 2];
                                                }
                                                else
                                                {
                                                    num37 = inClose[i - 2];
                                                }

                                                num36 = (inHigh[i - 2] - num38) + (num37 - inLow[i - 2]);
                                            }
                                            else
                                            {
                                                num36 = 0.0f;
                                            }

                                            num39 = num36;
                                        }

                                        num40 = num39;
                                    }

                                    num41 = num40;
                                }

                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                {
                                    num35 = 2.0;
                                }
                                else
                                {
                                    num35 = 1.0;
                                }

                                if (inOpen[i - 1] >= (inClose[i - 2] - ((Globals.candleSettings[10].factor * num41) / num35)))
                                {
                                    double num28;
                                    double num34;
                                    if (Globals.candleSettings[10].avgPeriod != 0.0)
                                    {
                                        num34 = EqualPeriodTotal[1] / ((double) Globals.candleSettings[10].avgPeriod);
                                    }
                                    else
                                    {
                                        float num33;
                                        if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                        {
                                            num33 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                                        }
                                        else
                                        {
                                            float num32;
                                            if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                            {
                                                num32 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                float num29;
                                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                                {
                                                    float num30;
                                                    float num31;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num31 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num31 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num30 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num30 = inClose[i - 1];
                                                    }

                                                    num29 = (inHigh[i - 1] - num31) + (num30 - inLow[i - 1]);
                                                }
                                                else
                                                {
                                                    num29 = 0.0f;
                                                }

                                                num32 = num29;
                                            }

                                            num33 = num32;
                                        }

                                        num34 = num33;
                                    }

                                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                    {
                                        num28 = 2.0;
                                    }
                                    else
                                    {
                                        num28 = 1.0;
                                    }

                                    if (inOpen[i] <= (inClose[i - 1] + ((Globals.candleSettings[10].factor * num34) / num28)))
                                    {
                                        double num21;
                                        double num27;
                                        if (Globals.candleSettings[10].avgPeriod != 0.0)
                                        {
                                            num27 = EqualPeriodTotal[1] / ((double) Globals.candleSettings[10].avgPeriod);
                                        }
                                        else
                                        {
                                            float num26;
                                            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                            {
                                                num26 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                                            }
                                            else
                                            {
                                                float num25;
                                                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                                {
                                                    num25 = inHigh[i - 1] - inLow[i - 1];
                                                }
                                                else
                                                {
                                                    float num22;
                                                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                                    {
                                                        float num23;
                                                        float num24;
                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num24 = inClose[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num24 = inOpen[i - 1];
                                                        }

                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num23 = inOpen[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num23 = inClose[i - 1];
                                                        }

                                                        num22 = (inHigh[i - 1] - num24) + (num23 - inLow[i - 1]);
                                                    }
                                                    else
                                                    {
                                                        num22 = 0.0f;
                                                    }

                                                    num25 = num22;
                                                }

                                                num26 = num25;
                                            }

                                            num27 = num26;
                                        }

                                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                        {
                                            num21 = 2.0;
                                        }
                                        else
                                        {
                                            num21 = 1.0;
                                        }

                                        if (inOpen[i] >= (inClose[i - 1] - ((Globals.candleSettings[10].factor * num27) / num21)))
                                        {
                                            outInteger[outIdx] = -100;
                                            outIdx++;
                                            goto Label_103A;
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
            Label_103A:
            totIdx = 2;
            while (totIdx >= 0)
            {
                float num15;
                float num20;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num19;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num16;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs((float) (inClose[ShadowVeryShortTrailingIdx - totIdx] - inOpen[ShadowVeryShortTrailingIdx - totIdx]));
                }
                else
                {
                    float num14;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[ShadowVeryShortTrailingIdx - totIdx] - inLow[ShadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num11;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num12;
                            float num13;
                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num13 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num12 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            num11 = (inHigh[ShadowVeryShortTrailingIdx - totIdx] - num13) +
                                    (num12 - inLow[ShadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0f;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                ShadowVeryShortPeriodTotal[totIdx] += num20 - num15;
                totIdx--;
            }

            for (totIdx = 2; totIdx >= 1; totIdx--)
            {
                float num5;
                float num10;
                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num9;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num6;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            float num7;
                            float num8;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num8 = inClose[i - totIdx];
                            }
                            else
                            {
                                num8 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num7 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num7 = inClose[i - totIdx];
                            }

                            num6 = (inHigh[i - totIdx] - num8) + (num7 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num6 = 0.0f;
                        }

                        num9 = num6;
                    }

                    num10 = num9;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((float) (inClose[EqualTrailingIdx - totIdx] - inOpen[EqualTrailingIdx - totIdx]));
                }
                else
                {
                    float num4;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[EqualTrailingIdx - totIdx] - inLow[EqualTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            float num2;
                            float num3;
                            if (inClose[EqualTrailingIdx - totIdx] >= inOpen[EqualTrailingIdx - totIdx])
                            {
                                num3 = inClose[EqualTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[EqualTrailingIdx - totIdx];
                            }

                            if (inClose[EqualTrailingIdx - totIdx] >= inOpen[EqualTrailingIdx - totIdx])
                            {
                                num2 = inOpen[EqualTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[EqualTrailingIdx - totIdx];
                            }

                            num = (inHigh[EqualTrailingIdx - totIdx] - num3) + (num2 - inLow[EqualTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0f;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                EqualPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            ShadowVeryShortTrailingIdx++;
            EqualTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_056F;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlIdentical3CrowsLookback()
        {
            return (((Globals.candleSettings[7].avgPeriod <= Globals.candleSettings[10].avgPeriod)
                        ? Globals.candleSettings[10].avgPeriod
                        : Globals.candleSettings[7].avgPeriod) + 2);
        }
    }
}
