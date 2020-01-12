using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlMatHold(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            double optInPenetration, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num15;
            double num20;
            double num54;
            double num60;
            double[] BodyPeriodTotal = new double[5];
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

            if (optInPenetration == -4E+37)
            {
                optInPenetration = 0.5;
            }
            else if ((optInPenetration < 0.0) || (optInPenetration > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlMatHoldLookback(optInPenetration);
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

            BodyPeriodTotal[4] = 0.0;
            BodyPeriodTotal[3] = 0.0;
            BodyPeriodTotal[2] = 0.0;
            BodyPeriodTotal[1] = 0.0;
            BodyPeriodTotal[0] = 0.0;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = BodyShortTrailingIdx;
            while (true)
            {
                double num70;
                double num75;
                double num80;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num80 = Math.Abs((double) (inClose[i - 3] - inOpen[i - 3]));
                }
                else
                {
                    double num79;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num79 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        double num76;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num77;
                            double num78;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num78 = inClose[i - 3];
                            }
                            else
                            {
                                num78 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num77 = inOpen[i - 3];
                            }
                            else
                            {
                                num77 = inClose[i - 3];
                            }

                            num76 = (inHigh[i - 3] - num78) + (num77 - inLow[i - 3]);
                        }
                        else
                        {
                            num76 = 0.0;
                        }

                        num79 = num76;
                    }

                    num80 = num79;
                }

                BodyPeriodTotal[3] += num80;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num75 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num74;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num74 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num71;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num72;
                            double num73;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num73 = inClose[i - 2];
                            }
                            else
                            {
                                num73 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num72 = inOpen[i - 2];
                            }
                            else
                            {
                                num72 = inClose[i - 2];
                            }

                            num71 = (inHigh[i - 2] - num73) + (num72 - inLow[i - 2]);
                        }
                        else
                        {
                            num71 = 0.0;
                        }

                        num74 = num71;
                    }

                    num75 = num74;
                }

                BodyPeriodTotal[2] += num75;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num69;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num66;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num67;
                            double num68;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num68 = inClose[i - 1];
                            }
                            else
                            {
                                num68 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num67 = inOpen[i - 1];
                            }
                            else
                            {
                                num67 = inClose[i - 1];
                            }

                            num66 = (inHigh[i - 1] - num68) + (num67 - inLow[i - 1]);
                        }
                        else
                        {
                            num66 = 0.0;
                        }

                        num69 = num66;
                    }

                    num70 = num69;
                }

                BodyPeriodTotal[1] += num70;
                i++;
            }

            i = BodyLongTrailingIdx;
            while (true)
            {
                double num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs((double) (inClose[i - 4] - inOpen[i - 4]));
                }
                else
                {
                    double num64;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num61;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num62;
                            double num63;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num63 = inClose[i - 4];
                            }
                            else
                            {
                                num63 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num62 = inOpen[i - 4];
                            }
                            else
                            {
                                num62 = inClose[i - 4];
                            }

                            num61 = (inHigh[i - 4] - num63) + (num62 - inLow[i - 4]);
                        }
                        else
                        {
                            num61 = 0.0;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                BodyPeriodTotal[4] += num65;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0488:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num60 = BodyPeriodTotal[4] / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                double num59;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num59 = Math.Abs((double) (inClose[i - 4] - inOpen[i - 4]));
                }
                else
                {
                    double num58;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num58 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num55;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num56;
                            double num57;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num57 = inClose[i - 4];
                            }
                            else
                            {
                                num57 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num56 = inOpen[i - 4];
                            }
                            else
                            {
                                num56 = inClose[i - 4];
                            }

                            num55 = (inHigh[i - 4] - num57) + (num56 - inLow[i - 4]);
                        }
                        else
                        {
                            num55 = 0.0;
                        }

                        num58 = num55;
                    }

                    num59 = num58;
                }

                num60 = num59;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num54 = 2.0;
            }
            else
            {
                num54 = 1.0;
            }

            if (Math.Abs((double) (inClose[i - 4] - inOpen[i - 4])) > ((Globals.candleSettings[0].factor * num60) / num54))
            {
                double num47;
                double num53;
                if (Globals.candleSettings[2].avgPeriod != 0.0)
                {
                    num53 = BodyPeriodTotal[3] / ((double) Globals.candleSettings[2].avgPeriod);
                }
                else
                {
                    double num52;
                    if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                    {
                        num52 = Math.Abs((double) (inClose[i - 3] - inOpen[i - 3]));
                    }
                    else
                    {
                        double num51;
                        if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                        {
                            num51 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            double num48;
                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                            {
                                double num49;
                                double num50;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num50 = inClose[i - 3];
                                }
                                else
                                {
                                    num50 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num49 = inOpen[i - 3];
                                }
                                else
                                {
                                    num49 = inClose[i - 3];
                                }

                                num48 = (inHigh[i - 3] - num50) + (num49 - inLow[i - 3]);
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

                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                {
                    num47 = 2.0;
                }
                else
                {
                    num47 = 1.0;
                }

                if (Math.Abs((double) (inClose[i - 3] - inOpen[i - 3])) < ((Globals.candleSettings[2].factor * num53) / num47))
                {
                    double num40;
                    double num46;
                    if (Globals.candleSettings[2].avgPeriod != 0.0)
                    {
                        num46 = BodyPeriodTotal[2] / ((double) Globals.candleSettings[2].avgPeriod);
                    }
                    else
                    {
                        double num45;
                        if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                        {
                            num45 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                        }
                        else
                        {
                            double num44;
                            if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                            {
                                num44 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                double num41;
                                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                {
                                    double num42;
                                    double num43;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num43 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num43 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num42 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num42 = inClose[i - 2];
                                    }

                                    num41 = (inHigh[i - 2] - num43) + (num42 - inLow[i - 2]);
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

                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        num40 = 2.0;
                    }
                    else
                    {
                        num40 = 1.0;
                    }

                    if (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) < ((Globals.candleSettings[2].factor * num46) / num40))
                    {
                        double num33;
                        double num39;
                        if (Globals.candleSettings[2].avgPeriod != 0.0)
                        {
                            num39 = BodyPeriodTotal[1] / ((double) Globals.candleSettings[2].avgPeriod);
                        }
                        else
                        {
                            double num38;
                            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                            {
                                num38 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                double num37;
                                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                {
                                    num37 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num34;
                                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                    {
                                        double num35;
                                        double num36;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num36 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num36 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num35 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num35 = inClose[i - 1];
                                        }

                                        num34 = (inHigh[i - 1] - num36) + (num35 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num34 = 0.0;
                                    }

                                    num37 = num34;
                                }

                                num38 = num37;
                            }

                            num39 = num38;
                        }

                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            num33 = 2.0;
                        }
                        else
                        {
                            num33 = 1.0;
                        }

                        if ((((Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) <
                               ((Globals.candleSettings[2].factor * num39) / num33)) && (inClose[i - 4] >= inOpen[i - 4])) &&
                             (((inClose[i - 3] < inOpen[i - 3]) ? -1 : 1) == -1)) && (inClose[i] >= inOpen[i]))
                        {
                            double num31;
                            double num32;
                            if (inOpen[i - 3] < inClose[i - 3])
                            {
                                num32 = inOpen[i - 3];
                            }
                            else
                            {
                                num32 = inClose[i - 3];
                            }

                            if (inOpen[i - 4] > inClose[i - 4])
                            {
                                num31 = inOpen[i - 4];
                            }
                            else
                            {
                                num31 = inClose[i - 4];
                            }

                            if (num32 > num31)
                            {
                                double num30;
                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num30 = inOpen[i - 2];
                                }
                                else
                                {
                                    num30 = inClose[i - 2];
                                }

                                if (num30 < inClose[i - 4])
                                {
                                    double num29;
                                    if (inOpen[i - 1] < inClose[i - 1])
                                    {
                                        num29 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num29 = inClose[i - 1];
                                    }

                                    if (num29 < inClose[i - 4])
                                    {
                                        double num28;
                                        if (inOpen[i - 2] < inClose[i - 2])
                                        {
                                            num28 = inOpen[i - 2];
                                        }
                                        else
                                        {
                                            num28 = inClose[i - 2];
                                        }

                                        if (num28 > (inClose[i - 4] -
                                                     (Math.Abs((double) (inClose[i - 4] - inOpen[i - 4])) * optInPenetration)))
                                        {
                                            double num27;
                                            if (inOpen[i - 1] < inClose[i - 1])
                                            {
                                                num27 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num27 = inClose[i - 1];
                                            }

                                            if (num27 > (inClose[i - 4] -
                                                         (Math.Abs((double) (inClose[i - 4] - inOpen[i - 4])) * optInPenetration)))
                                            {
                                                double num26;
                                                if (inClose[i - 2] > inOpen[i - 2])
                                                {
                                                    num26 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num26 = inOpen[i - 2];
                                                }

                                                if (num26 < inOpen[i - 3])
                                                {
                                                    double num24;
                                                    double num25;
                                                    if (inClose[i - 1] > inOpen[i - 1])
                                                    {
                                                        num25 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num25 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 2] > inOpen[i - 2])
                                                    {
                                                        num24 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num24 = inOpen[i - 2];
                                                    }

                                                    if ((num25 < num24) && (inOpen[i] > inClose[i - 1]))
                                                    {
                                                        double num21;
                                                        double num23;
                                                        if (inHigh[i - 3] > inHigh[i - 2])
                                                        {
                                                            num23 = inHigh[i - 3];
                                                        }
                                                        else
                                                        {
                                                            num23 = inHigh[i - 2];
                                                        }

                                                        if (num23 > inHigh[i - 1])
                                                        {
                                                            double num22;
                                                            if (inHigh[i - 3] > inHigh[i - 2])
                                                            {
                                                                num22 = inHigh[i - 3];
                                                            }
                                                            else
                                                            {
                                                                num22 = inHigh[i - 2];
                                                            }

                                                            num21 = num22;
                                                        }
                                                        else
                                                        {
                                                            num21 = inHigh[i - 1];
                                                        }

                                                        if (inClose[i] > num21)
                                                        {
                                                            outInteger[outIdx] = 100;
                                                            outIdx++;
                                                            goto Label_0C54;
                                                        }
                                                    }
                                                }
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
            Label_0C54:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i - 4] - inOpen[i - 4]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num18 = inClose[i - 4];
                        }
                        else
                        {
                            num18 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num17 = inOpen[i - 4];
                        }
                        else
                        {
                            num17 = inClose[i - 4];
                        }

                        num16 = (inHigh[i - 4] - num18) + (num17 - inLow[i - 4]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((double) (inClose[BodyLongTrailingIdx - 4] - inOpen[BodyLongTrailingIdx - 4]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyLongTrailingIdx - 4] - inLow[BodyLongTrailingIdx - 4];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[BodyLongTrailingIdx - 4] >= inOpen[BodyLongTrailingIdx - 4])
                        {
                            num13 = inClose[BodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num13 = inOpen[BodyLongTrailingIdx - 4];
                        }

                        if (inClose[BodyLongTrailingIdx - 4] >= inOpen[BodyLongTrailingIdx - 4])
                        {
                            num12 = inOpen[BodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num12 = inClose[BodyLongTrailingIdx - 4];
                        }

                        num11 = (inHigh[BodyLongTrailingIdx - 4] - num13) + (num12 - inLow[BodyLongTrailingIdx - 4]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyPeriodTotal[4] += num20 - num15;
            for (int totIdx = 3; totIdx >= 1; totIdx--)
            {
                double num5;
                double num10;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num9;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((double) (inClose[BodyShortTrailingIdx - totIdx] - inOpen[BodyShortTrailingIdx - totIdx]));
                }
                else
                {
                    double num4;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[BodyShortTrailingIdx - totIdx] - inLow[BodyShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[BodyShortTrailingIdx - totIdx] >= inOpen[BodyShortTrailingIdx - totIdx])
                            {
                                num3 = inClose[BodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[BodyShortTrailingIdx - totIdx];
                            }

                            if (inClose[BodyShortTrailingIdx - totIdx] >= inOpen[BodyShortTrailingIdx - totIdx])
                            {
                                num2 = inOpen[BodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[BodyShortTrailingIdx - totIdx];
                            }

                            num = (inHigh[BodyShortTrailingIdx - totIdx] - num3) + (num2 - inLow[BodyShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                BodyPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            BodyShortTrailingIdx++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0488;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlMatHold(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            double optInPenetration, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num15;
            float num20;
            double num54;
            double num60;
            double[] BodyPeriodTotal = new double[5];
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

            if (optInPenetration == -4E+37)
            {
                optInPenetration = 0.5;
            }
            else if ((optInPenetration < 0.0) || (optInPenetration > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlMatHoldLookback(optInPenetration);
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

            BodyPeriodTotal[4] = 0.0;
            BodyPeriodTotal[3] = 0.0;
            BodyPeriodTotal[2] = 0.0;
            BodyPeriodTotal[1] = 0.0;
            BodyPeriodTotal[0] = 0.0;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = BodyShortTrailingIdx;
            while (true)
            {
                float num70;
                float num75;
                float num80;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num80 = Math.Abs((float) (inClose[i - 3] - inOpen[i - 3]));
                }
                else
                {
                    float num79;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num79 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        float num76;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num77;
                            float num78;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num78 = inClose[i - 3];
                            }
                            else
                            {
                                num78 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num77 = inOpen[i - 3];
                            }
                            else
                            {
                                num77 = inClose[i - 3];
                            }

                            num76 = (inHigh[i - 3] - num78) + (num77 - inLow[i - 3]);
                        }
                        else
                        {
                            num76 = 0.0f;
                        }

                        num79 = num76;
                    }

                    num80 = num79;
                }

                BodyPeriodTotal[3] += num80;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num75 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num74;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num74 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num71;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num72;
                            float num73;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num73 = inClose[i - 2];
                            }
                            else
                            {
                                num73 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num72 = inOpen[i - 2];
                            }
                            else
                            {
                                num72 = inClose[i - 2];
                            }

                            num71 = (inHigh[i - 2] - num73) + (num72 - inLow[i - 2]);
                        }
                        else
                        {
                            num71 = 0.0f;
                        }

                        num74 = num71;
                    }

                    num75 = num74;
                }

                BodyPeriodTotal[2] += num75;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num69;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num66;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num67;
                            float num68;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num68 = inClose[i - 1];
                            }
                            else
                            {
                                num68 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num67 = inOpen[i - 1];
                            }
                            else
                            {
                                num67 = inClose[i - 1];
                            }

                            num66 = (inHigh[i - 1] - num68) + (num67 - inLow[i - 1]);
                        }
                        else
                        {
                            num66 = 0.0f;
                        }

                        num69 = num66;
                    }

                    num70 = num69;
                }

                BodyPeriodTotal[1] += num70;
                i++;
            }

            i = BodyLongTrailingIdx;
            while (true)
            {
                float num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs((float) (inClose[i - 4] - inOpen[i - 4]));
                }
                else
                {
                    float num64;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        float num61;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num62;
                            float num63;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num63 = inClose[i - 4];
                            }
                            else
                            {
                                num63 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num62 = inOpen[i - 4];
                            }
                            else
                            {
                                num62 = inClose[i - 4];
                            }

                            num61 = (inHigh[i - 4] - num63) + (num62 - inLow[i - 4]);
                        }
                        else
                        {
                            num61 = 0.0f;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                BodyPeriodTotal[4] += num65;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_04C0:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num60 = BodyPeriodTotal[4] / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                float num59;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num59 = Math.Abs((float) (inClose[i - 4] - inOpen[i - 4]));
                }
                else
                {
                    float num58;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num58 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        float num55;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num56;
                            float num57;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num57 = inClose[i - 4];
                            }
                            else
                            {
                                num57 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num56 = inOpen[i - 4];
                            }
                            else
                            {
                                num56 = inClose[i - 4];
                            }

                            num55 = (inHigh[i - 4] - num57) + (num56 - inLow[i - 4]);
                        }
                        else
                        {
                            num55 = 0.0f;
                        }

                        num58 = num55;
                    }

                    num59 = num58;
                }

                num60 = num59;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num54 = 2.0;
            }
            else
            {
                num54 = 1.0;
            }

            if (Math.Abs((float) (inClose[i - 4] - inOpen[i - 4])) > ((Globals.candleSettings[0].factor * num60) / num54))
            {
                double num47;
                double num53;
                if (Globals.candleSettings[2].avgPeriod != 0.0)
                {
                    num53 = BodyPeriodTotal[3] / ((double) Globals.candleSettings[2].avgPeriod);
                }
                else
                {
                    float num52;
                    if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                    {
                        num52 = Math.Abs((float) (inClose[i - 3] - inOpen[i - 3]));
                    }
                    else
                    {
                        float num51;
                        if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                        {
                            num51 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            float num48;
                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                            {
                                float num49;
                                float num50;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num50 = inClose[i - 3];
                                }
                                else
                                {
                                    num50 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num49 = inOpen[i - 3];
                                }
                                else
                                {
                                    num49 = inClose[i - 3];
                                }

                                num48 = (inHigh[i - 3] - num50) + (num49 - inLow[i - 3]);
                            }
                            else
                            {
                                num48 = 0.0f;
                            }

                            num51 = num48;
                        }

                        num52 = num51;
                    }

                    num53 = num52;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                {
                    num47 = 2.0;
                }
                else
                {
                    num47 = 1.0;
                }

                if (Math.Abs((float) (inClose[i - 3] - inOpen[i - 3])) < ((Globals.candleSettings[2].factor * num53) / num47))
                {
                    double num40;
                    double num46;
                    if (Globals.candleSettings[2].avgPeriod != 0.0)
                    {
                        num46 = BodyPeriodTotal[2] / ((double) Globals.candleSettings[2].avgPeriod);
                    }
                    else
                    {
                        float num45;
                        if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                        {
                            num45 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                        }
                        else
                        {
                            float num44;
                            if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                            {
                                num44 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                float num41;
                                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                {
                                    float num42;
                                    float num43;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num43 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num43 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num42 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num42 = inClose[i - 2];
                                    }

                                    num41 = (inHigh[i - 2] - num43) + (num42 - inLow[i - 2]);
                                }
                                else
                                {
                                    num41 = 0.0f;
                                }

                                num44 = num41;
                            }

                            num45 = num44;
                        }

                        num46 = num45;
                    }

                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        num40 = 2.0;
                    }
                    else
                    {
                        num40 = 1.0;
                    }

                    if (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) < ((Globals.candleSettings[2].factor * num46) / num40))
                    {
                        double num33;
                        double num39;
                        if (Globals.candleSettings[2].avgPeriod != 0.0)
                        {
                            num39 = BodyPeriodTotal[1] / ((double) Globals.candleSettings[2].avgPeriod);
                        }
                        else
                        {
                            float num38;
                            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                            {
                                num38 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                float num37;
                                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                {
                                    num37 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    float num34;
                                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                    {
                                        float num35;
                                        float num36;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num36 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num36 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num35 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num35 = inClose[i - 1];
                                        }

                                        num34 = (inHigh[i - 1] - num36) + (num35 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num34 = 0.0f;
                                    }

                                    num37 = num34;
                                }

                                num38 = num37;
                            }

                            num39 = num38;
                        }

                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            num33 = 2.0;
                        }
                        else
                        {
                            num33 = 1.0;
                        }

                        if ((((Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) < ((Globals.candleSettings[2].factor * num39) / num33)) &&
                              (inClose[i - 4] >= inOpen[i - 4])) && (((inClose[i - 3] < inOpen[i - 3]) ? -1 : 1) == -1)) &&
                            (inClose[i] >= inOpen[i]))
                        {
                            float num31;
                            float num32;
                            if (inOpen[i - 3] < inClose[i - 3])
                            {
                                num32 = inOpen[i - 3];
                            }
                            else
                            {
                                num32 = inClose[i - 3];
                            }

                            if (inOpen[i - 4] > inClose[i - 4])
                            {
                                num31 = inOpen[i - 4];
                            }
                            else
                            {
                                num31 = inClose[i - 4];
                            }

                            if (num32 > num31)
                            {
                                float num30;
                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num30 = inOpen[i - 2];
                                }
                                else
                                {
                                    num30 = inClose[i - 2];
                                }

                                if (num30 < inClose[i - 4])
                                {
                                    float num29;
                                    if (inOpen[i - 1] < inClose[i - 1])
                                    {
                                        num29 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num29 = inClose[i - 1];
                                    }

                                    if (num29 < inClose[i - 4])
                                    {
                                        float num28;
                                        if (inOpen[i - 2] < inClose[i - 2])
                                        {
                                            num28 = inOpen[i - 2];
                                        }
                                        else
                                        {
                                            num28 = inClose[i - 2];
                                        }

                                        if (num28 > (inClose[i - 4] -
                                                     (Math.Abs((float) (inClose[i - 4] - inOpen[i - 4])) * optInPenetration)))
                                        {
                                            float num27;
                                            if (inOpen[i - 1] < inClose[i - 1])
                                            {
                                                num27 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num27 = inClose[i - 1];
                                            }

                                            if (num27 > (inClose[i - 4] -
                                                         (Math.Abs((float) (inClose[i - 4] - inOpen[i - 4])) * optInPenetration)))
                                            {
                                                float num26;
                                                if (inClose[i - 2] > inOpen[i - 2])
                                                {
                                                    num26 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num26 = inOpen[i - 2];
                                                }

                                                if (num26 < inOpen[i - 3])
                                                {
                                                    float num24;
                                                    float num25;
                                                    if (inClose[i - 1] > inOpen[i - 1])
                                                    {
                                                        num25 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num25 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 2] > inOpen[i - 2])
                                                    {
                                                        num24 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num24 = inOpen[i - 2];
                                                    }

                                                    if ((num25 < num24) && (inOpen[i] > inClose[i - 1]))
                                                    {
                                                        float num21;
                                                        float num23;
                                                        if (inHigh[i - 3] > inHigh[i - 2])
                                                        {
                                                            num23 = inHigh[i - 3];
                                                        }
                                                        else
                                                        {
                                                            num23 = inHigh[i - 2];
                                                        }

                                                        if (num23 > inHigh[i - 1])
                                                        {
                                                            float num22;
                                                            if (inHigh[i - 3] > inHigh[i - 2])
                                                            {
                                                                num22 = inHigh[i - 3];
                                                            }
                                                            else
                                                            {
                                                                num22 = inHigh[i - 2];
                                                            }

                                                            num21 = num22;
                                                        }
                                                        else
                                                        {
                                                            num21 = inHigh[i - 1];
                                                        }

                                                        if (inClose[i] > num21)
                                                        {
                                                            outInteger[outIdx] = 100;
                                                            outIdx++;
                                                            goto Label_0D26;
                                                        }
                                                    }
                                                }
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
            Label_0D26:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i - 4] - inOpen[i - 4]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num17;
                        float num18;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num18 = inClose[i - 4];
                        }
                        else
                        {
                            num18 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num17 = inOpen[i - 4];
                        }
                        else
                        {
                            num17 = inClose[i - 4];
                        }

                        num16 = (inHigh[i - 4] - num18) + (num17 - inLow[i - 4]);
                    }
                    else
                    {
                        num16 = 0.0f;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((float) (inClose[BodyLongTrailingIdx - 4] - inOpen[BodyLongTrailingIdx - 4]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyLongTrailingIdx - 4] - inLow[BodyLongTrailingIdx - 4];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[BodyLongTrailingIdx - 4] >= inOpen[BodyLongTrailingIdx - 4])
                        {
                            num13 = inClose[BodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num13 = inOpen[BodyLongTrailingIdx - 4];
                        }

                        if (inClose[BodyLongTrailingIdx - 4] >= inOpen[BodyLongTrailingIdx - 4])
                        {
                            num12 = inOpen[BodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num12 = inClose[BodyLongTrailingIdx - 4];
                        }

                        num11 = (inHigh[BodyLongTrailingIdx - 4] - num13) + (num12 - inLow[BodyLongTrailingIdx - 4]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyPeriodTotal[4] += num20 - num15;
            for (int totIdx = 3; totIdx >= 1; totIdx--)
            {
                float num5;
                float num10;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num9;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num6;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((float) (inClose[BodyShortTrailingIdx - totIdx] - inOpen[BodyShortTrailingIdx - totIdx]));
                }
                else
                {
                    float num4;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[BodyShortTrailingIdx - totIdx] - inLow[BodyShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num2;
                            float num3;
                            if (inClose[BodyShortTrailingIdx - totIdx] >= inOpen[BodyShortTrailingIdx - totIdx])
                            {
                                num3 = inClose[BodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[BodyShortTrailingIdx - totIdx];
                            }

                            if (inClose[BodyShortTrailingIdx - totIdx] >= inOpen[BodyShortTrailingIdx - totIdx])
                            {
                                num2 = inOpen[BodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[BodyShortTrailingIdx - totIdx];
                            }

                            num = (inHigh[BodyShortTrailingIdx - totIdx] - num3) + (num2 - inLow[BodyShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0f;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                BodyPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            BodyShortTrailingIdx++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_04C0;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlMatHoldLookback(double optInPenetration)
        {
            if (optInPenetration == -4E+37)
            {
                optInPenetration = 0.5;
            }
            else if ((optInPenetration < 0.0) || (optInPenetration > 3E+37))
            {
                return -1;
            }

            return (((Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[0].avgPeriod)
                        ? Globals.candleSettings[0].avgPeriod
                        : Globals.candleSettings[2].avgPeriod) + 4);
        }
    }
}
