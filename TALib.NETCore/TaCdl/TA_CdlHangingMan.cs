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
            double num65;
            double num71;
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

            double BodyPeriodTotal = 0.0;
            int BodyTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            double ShadowLongPeriodTotal = 0.0;
            int ShadowLongTrailingIdx = startIdx - Globals.candleSettings[4].avgPeriod;
            double ShadowVeryShortPeriodTotal = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            double NearPeriodTotal = 0.0;
            int NearTrailingIdx = (startIdx - 1) - Globals.candleSettings[8].avgPeriod;
            int i = BodyTrailingIdx;
            while (true)
            {
                double num91;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num91 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num90;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num90 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num87;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                            num87 = (inHigh[i] - num89) + (num88 - inLow[i]);
                        }
                        else
                        {
                            num87 = 0.0;
                        }

                        num90 = num87;
                    }

                    num91 = num90;
                }

                BodyPeriodTotal += num91;
                i++;
            }

            i = ShadowLongTrailingIdx;
            while (true)
            {
                double num86;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num86 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num85;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num85 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num82;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                            num82 = (inHigh[i] - num84) + (num83 - inLow[i]);
                        }
                        else
                        {
                            num82 = 0.0;
                        }

                        num85 = num82;
                    }

                    num86 = num85;
                }

                ShadowLongPeriodTotal += num86;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num81;
                if (i >= startIdx)
                {
                    break;
                }

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

                ShadowVeryShortPeriodTotal += num81;
                i++;
            }

            i = NearTrailingIdx;
            while (true)
            {
                double num76;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num76 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num75;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num75 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num72;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                            num72 = (inHigh[i] - num74) + (num73 - inLow[i]);
                        }
                        else
                        {
                            num72 = 0.0;
                        }

                        num75 = num72;
                    }

                    num76 = num75;
                }

                NearPeriodTotal += num76;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_03FF:
            if (Globals.candleSettings[2].avgPeriod != 0.0)
            {
                num71 = BodyPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
            }
            else
            {
                double num70;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num69;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num66;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                            num66 = (inHigh[i] - num68) + (num67 - inLow[i]);
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

            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
            {
                num65 = 2.0;
            }
            else
            {
                num65 = 1.0;
            }

            if (Math.Abs((double) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num71) / num65))
            {
                double num57;
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

                if (Globals.candleSettings[4].avgPeriod != 0.0)
                {
                    num63 = ShadowLongPeriodTotal / ((double) Globals.candleSettings[4].avgPeriod);
                }
                else
                {
                    double num62;
                    if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                    {
                        num62 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num61;
                        if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                        {
                            num61 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num58;
                            if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                                num58 = (inHigh[i] - num60) + (num59 - inLow[i]);
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

                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                {
                    num57 = 2.0;
                }
                else
                {
                    num57 = 1.0;
                }

                if ((num64 - inLow[i]) > ((Globals.candleSettings[4].factor * num63) / num57))
                {
                    double num49;
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

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num55 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
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

                    if ((inHigh[i] - num56) < ((Globals.candleSettings[7].factor * num55) / num49))
                    {
                        double num41;
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

                        if (Globals.candleSettings[8].avgPeriod != 0.0)
                        {
                            num47 = NearPeriodTotal / ((double) Globals.candleSettings[8].avgPeriod);
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

                        if (num48 >= (inHigh[i - 1] - ((Globals.candleSettings[8].factor * num47) / num41)))
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
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num40 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num39;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num39 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num36;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                        num36 = (inHigh[i] - num38) + (num37 - inLow[i]);
                    }
                    else
                    {
                        num36 = 0.0;
                    }

                    num39 = num36;
                }

                num40 = num39;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num35 = Math.Abs((double) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                double num34;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num34 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    double num31;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num32;
                        double num33;
                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num33 = inClose[BodyTrailingIdx];
                        }
                        else
                        {
                            num33 = inOpen[BodyTrailingIdx];
                        }

                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num32 = inOpen[BodyTrailingIdx];
                        }
                        else
                        {
                            num32 = inClose[BodyTrailingIdx];
                        }

                        num31 = (inHigh[BodyTrailingIdx] - num33) + (num32 - inLow[BodyTrailingIdx]);
                    }
                    else
                    {
                        num31 = 0.0;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            BodyPeriodTotal += num40 - num35;
            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num29;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num26;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                        num26 = (inHigh[i] - num28) + (num27 - inLow[i]);
                    }
                    else
                    {
                        num26 = 0.0;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((double) (inClose[ShadowLongTrailingIdx] - inOpen[ShadowLongTrailingIdx]));
            }
            else
            {
                double num24;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[ShadowLongTrailingIdx] - inLow[ShadowLongTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[ShadowLongTrailingIdx] >= inOpen[ShadowLongTrailingIdx])
                        {
                            num23 = inClose[ShadowLongTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[ShadowLongTrailingIdx];
                        }

                        if (inClose[ShadowLongTrailingIdx] >= inOpen[ShadowLongTrailingIdx])
                        {
                            num22 = inOpen[ShadowLongTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[ShadowLongTrailingIdx];
                        }

                        num21 = (inHigh[ShadowLongTrailingIdx] - num23) + (num22 - inLow[ShadowLongTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            ShadowLongPeriodTotal += num30 - num25;
            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((double) (inClose[ShadowVeryShortTrailingIdx] - inOpen[ShadowVeryShortTrailingIdx]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[ShadowVeryShortTrailingIdx] - inLow[ShadowVeryShortTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[ShadowVeryShortTrailingIdx] >= inOpen[ShadowVeryShortTrailingIdx])
                        {
                            num13 = inClose[ShadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[ShadowVeryShortTrailingIdx];
                        }

                        if (inClose[ShadowVeryShortTrailingIdx] >= inOpen[ShadowVeryShortTrailingIdx])
                        {
                            num12 = inOpen[ShadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[ShadowVeryShortTrailingIdx];
                        }

                        num11 = (inHigh[ShadowVeryShortTrailingIdx] - num13) + (num12 - inLow[ShadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            ShadowVeryShortPeriodTotal += num20 - num15;
            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[NearTrailingIdx] - inOpen[NearTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[NearTrailingIdx] - inLow[NearTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[NearTrailingIdx] >= inOpen[NearTrailingIdx])
                        {
                            num3 = inClose[NearTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[NearTrailingIdx];
                        }

                        if (inClose[NearTrailingIdx] >= inOpen[NearTrailingIdx])
                        {
                            num2 = inOpen[NearTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[NearTrailingIdx];
                        }

                        num = (inHigh[NearTrailingIdx] - num3) + (num2 - inLow[NearTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            NearPeriodTotal += num10 - num5;
            i++;
            BodyTrailingIdx++;
            ShadowLongTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            NearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_03FF;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlHangingMan(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
            float num25;
            float num30;
            float num35;
            float num40;
            double num65;
            double num71;
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

            double BodyPeriodTotal = 0.0;
            int BodyTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            double ShadowLongPeriodTotal = 0.0;
            int ShadowLongTrailingIdx = startIdx - Globals.candleSettings[4].avgPeriod;
            double ShadowVeryShortPeriodTotal = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            double NearPeriodTotal = 0.0;
            int NearTrailingIdx = (startIdx - 1) - Globals.candleSettings[8].avgPeriod;
            int i = BodyTrailingIdx;
            while (true)
            {
                float num91;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num91 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num90;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num90 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num87;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num88;
                            float num89;
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

                            num87 = (inHigh[i] - num89) + (num88 - inLow[i]);
                        }
                        else
                        {
                            num87 = 0.0f;
                        }

                        num90 = num87;
                    }

                    num91 = num90;
                }

                BodyPeriodTotal += num91;
                i++;
            }

            i = ShadowLongTrailingIdx;
            while (true)
            {
                float num86;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num86 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num85;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num85 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num82;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            float num83;
                            float num84;
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

                            num82 = (inHigh[i] - num84) + (num83 - inLow[i]);
                        }
                        else
                        {
                            num82 = 0.0f;
                        }

                        num85 = num82;
                    }

                    num86 = num85;
                }

                ShadowLongPeriodTotal += num86;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num81;
                if (i >= startIdx)
                {
                    break;
                }

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

                ShadowVeryShortPeriodTotal += num81;
                i++;
            }

            i = NearTrailingIdx;
            while (true)
            {
                float num76;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num76 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num75;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num75 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num72;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num73;
                            float num74;
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

                            num72 = (inHigh[i] - num74) + (num73 - inLow[i]);
                        }
                        else
                        {
                            num72 = 0.0f;
                        }

                        num75 = num72;
                    }

                    num76 = num75;
                }

                NearPeriodTotal += num76;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0437:
            if (Globals.candleSettings[2].avgPeriod != 0.0)
            {
                num71 = BodyPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
            }
            else
            {
                float num70;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num69;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num66;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num67;
                            float num68;
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

                            num66 = (inHigh[i] - num68) + (num67 - inLow[i]);
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

            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
            {
                num65 = 2.0;
            }
            else
            {
                num65 = 1.0;
            }

            if (Math.Abs((float) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num71) / num65))
            {
                double num57;
                double num63;
                float num64;
                if (inClose[i] >= inOpen[i])
                {
                    num64 = inOpen[i];
                }
                else
                {
                    num64 = inClose[i];
                }

                if (Globals.candleSettings[4].avgPeriod != 0.0)
                {
                    num63 = ShadowLongPeriodTotal / ((double) Globals.candleSettings[4].avgPeriod);
                }
                else
                {
                    float num62;
                    if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                    {
                        num62 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num61;
                        if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                        {
                            num61 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num58;
                            if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                            {
                                float num59;
                                float num60;
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

                                num58 = (inHigh[i] - num60) + (num59 - inLow[i]);
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

                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                {
                    num57 = 2.0;
                }
                else
                {
                    num57 = 1.0;
                }

                if ((num64 - inLow[i]) > ((Globals.candleSettings[4].factor * num63) / num57))
                {
                    double num49;
                    double num55;
                    float num56;
                    if (inClose[i] >= inOpen[i])
                    {
                        num56 = inClose[i];
                    }
                    else
                    {
                        num56 = inOpen[i];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num55 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
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

                    if ((inHigh[i] - num56) < ((Globals.candleSettings[7].factor * num55) / num49))
                    {
                        double num41;
                        double num47;
                        float num48;
                        if (inClose[i] < inOpen[i])
                        {
                            num48 = inClose[i];
                        }
                        else
                        {
                            num48 = inOpen[i];
                        }

                        if (Globals.candleSettings[8].avgPeriod != 0.0)
                        {
                            num47 = NearPeriodTotal / ((double) Globals.candleSettings[8].avgPeriod);
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

                        if (num48 >= (inHigh[i - 1] - ((Globals.candleSettings[8].factor * num47) / num41)))
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
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num40 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num39;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num39 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num36;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num37;
                        float num38;
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

                        num36 = (inHigh[i] - num38) + (num37 - inLow[i]);
                    }
                    else
                    {
                        num36 = 0.0f;
                    }

                    num39 = num36;
                }

                num40 = num39;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num35 = Math.Abs((float) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                float num34;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num34 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    float num31;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num32;
                        float num33;
                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num33 = inClose[BodyTrailingIdx];
                        }
                        else
                        {
                            num33 = inOpen[BodyTrailingIdx];
                        }

                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num32 = inOpen[BodyTrailingIdx];
                        }
                        else
                        {
                            num32 = inClose[BodyTrailingIdx];
                        }

                        num31 = (inHigh[BodyTrailingIdx] - num33) + (num32 - inLow[BodyTrailingIdx]);
                    }
                    else
                    {
                        num31 = 0.0f;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            BodyPeriodTotal += num40 - num35;
            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num29;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num26;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        float num27;
                        float num28;
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

                        num26 = (inHigh[i] - num28) + (num27 - inLow[i]);
                    }
                    else
                    {
                        num26 = 0.0f;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((float) (inClose[ShadowLongTrailingIdx] - inOpen[ShadowLongTrailingIdx]));
            }
            else
            {
                float num24;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[ShadowLongTrailingIdx] - inLow[ShadowLongTrailingIdx];
                }
                else
                {
                    float num21;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        float num22;
                        float num23;
                        if (inClose[ShadowLongTrailingIdx] >= inOpen[ShadowLongTrailingIdx])
                        {
                            num23 = inClose[ShadowLongTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[ShadowLongTrailingIdx];
                        }

                        if (inClose[ShadowLongTrailingIdx] >= inOpen[ShadowLongTrailingIdx])
                        {
                            num22 = inOpen[ShadowLongTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[ShadowLongTrailingIdx];
                        }

                        num21 = (inHigh[ShadowLongTrailingIdx] - num23) + (num22 - inLow[ShadowLongTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0f;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            ShadowLongPeriodTotal += num30 - num25;
            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((float) (inClose[ShadowVeryShortTrailingIdx] - inOpen[ShadowVeryShortTrailingIdx]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[ShadowVeryShortTrailingIdx] - inLow[ShadowVeryShortTrailingIdx];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[ShadowVeryShortTrailingIdx] >= inOpen[ShadowVeryShortTrailingIdx])
                        {
                            num13 = inClose[ShadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[ShadowVeryShortTrailingIdx];
                        }

                        if (inClose[ShadowVeryShortTrailingIdx] >= inOpen[ShadowVeryShortTrailingIdx])
                        {
                            num12 = inOpen[ShadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[ShadowVeryShortTrailingIdx];
                        }

                        num11 = (inHigh[ShadowVeryShortTrailingIdx] - num13) + (num12 - inLow[ShadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            ShadowVeryShortPeriodTotal += num20 - num15;
            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[NearTrailingIdx] - inOpen[NearTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[NearTrailingIdx] - inLow[NearTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[NearTrailingIdx] >= inOpen[NearTrailingIdx])
                        {
                            num3 = inClose[NearTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[NearTrailingIdx];
                        }

                        if (inClose[NearTrailingIdx] >= inOpen[NearTrailingIdx])
                        {
                            num2 = inOpen[NearTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[NearTrailingIdx];
                        }

                        num = (inHigh[NearTrailingIdx] - num3) + (num2 - inLow[NearTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            NearPeriodTotal += num10 - num5;
            i++;
            BodyTrailingIdx++;
            ShadowLongTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            NearTrailingIdx++;
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
            int num;
            int avgPeriod;
            if (((Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[4].avgPeriod)
                    ? Globals.candleSettings[4].avgPeriod
                    : Globals.candleSettings[2].avgPeriod) > Globals.candleSettings[7].avgPeriod)
            {
                avgPeriod = (Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[4].avgPeriod)
                    ? Globals.candleSettings[4].avgPeriod
                    : Globals.candleSettings[2].avgPeriod;
            }
            else
            {
                avgPeriod = Globals.candleSettings[7].avgPeriod;
            }

            if (avgPeriod > Globals.candleSettings[8].avgPeriod)
            {
                int num2;
                if (((Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[4].avgPeriod)
                        ? Globals.candleSettings[4].avgPeriod
                        : Globals.candleSettings[2].avgPeriod) > Globals.candleSettings[7].avgPeriod)
                {
                    num2 = (Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[4].avgPeriod)
                        ? Globals.candleSettings[4].avgPeriod
                        : Globals.candleSettings[2].avgPeriod;
                }
                else
                {
                    num2 = Globals.candleSettings[7].avgPeriod;
                }

                num = num2;
            }
            else
            {
                num = Globals.candleSettings[8].avgPeriod;
            }

            return (num + 1);
        }
    }
}
