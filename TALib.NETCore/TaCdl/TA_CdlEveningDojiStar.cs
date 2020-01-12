using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlEveningDojiStar(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, double optInPenetration, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            double num47;
            double num53;
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
                optInPenetration = 0.3;
            }
            else if ((optInPenetration < 0.0) || (optInPenetration > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlEveningDojiStarLookback(optInPenetration);
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

            double BodyLongPeriodTotal = 0.0;
            double BodyDojiPeriodTotal = 0.0;
            double BodyShortPeriodTotal = 0.0;
            int BodyLongTrailingIdx = (startIdx - 2) - Globals.candleSettings[0].avgPeriod;
            int BodyDojiTrailingIdx = (startIdx - 1) - Globals.candleSettings[3].avgPeriod;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num68;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num68 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num67;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num67 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num64;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num65;
                            double num66;
                            if (inClose[i] >= inOpen[i])
                            {
                                num66 = inClose[i];
                            }
                            else
                            {
                                num66 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inOpen[i];
                            }
                            else
                            {
                                num65 = inClose[i];
                            }

                            num64 = (inHigh[i] - num66) + (num65 - inLow[i]);
                        }
                        else
                        {
                            num64 = 0.0;
                        }

                        num67 = num64;
                    }

                    num68 = num67;
                }

                BodyLongPeriodTotal += num68;
                i++;
            }

            i = BodyDojiTrailingIdx;
            while (true)
            {
                double num63;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num63 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num62;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num62 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num59;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            double num60;
                            double num61;
                            if (inClose[i] >= inOpen[i])
                            {
                                num61 = inClose[i];
                            }
                            else
                            {
                                num61 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num60 = inOpen[i];
                            }
                            else
                            {
                                num60 = inClose[i];
                            }

                            num59 = (inHigh[i] - num61) + (num60 - inLow[i]);
                        }
                        else
                        {
                            num59 = 0.0;
                        }

                        num62 = num59;
                    }

                    num63 = num62;
                }

                BodyDojiPeriodTotal += num63;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                double num58;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num58 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num57;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num57 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num54;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inOpen[i];
                            }
                            else
                            {
                                num55 = inClose[i];
                            }

                            num54 = (inHigh[i] - num56) + (num55 - inLow[i]);
                        }
                        else
                        {
                            num54 = 0.0;
                        }

                        num57 = num54;
                    }

                    num58 = num57;
                }

                BodyShortPeriodTotal += num58;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_035B:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num53 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                double num52;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num52 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num51;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num51 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num48;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num49;
                            double num50;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num50 = inClose[i - 2];
                            }
                            else
                            {
                                num50 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inOpen[i - 2];
                            }
                            else
                            {
                                num49 = inClose[i - 2];
                            }

                            num48 = (inHigh[i - 2] - num50) + (num49 - inLow[i - 2]);
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

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num47 = 2.0;
            }
            else
            {
                num47 = 1.0;
            }

            if ((Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num53) / num47)) &&
                (inClose[i - 2] >= inOpen[i - 2]))
            {
                double num40;
                double num46;
                if (Globals.candleSettings[3].avgPeriod != 0.0)
                {
                    num46 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                }
                else
                {
                    double num45;
                    if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                    {
                        num45 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        double num44;
                        if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                        {
                            num44 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num41;
                            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                            {
                                double num42;
                                double num43;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num43 = inClose[i - 1];
                                }
                                else
                                {
                                    num43 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num42 = inOpen[i - 1];
                                }
                                else
                                {
                                    num42 = inClose[i - 1];
                                }

                                num41 = (inHigh[i - 1] - num43) + (num42 - inLow[i - 1]);
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

                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                {
                    num40 = 2.0;
                }
                else
                {
                    num40 = 1.0;
                }

                if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) <= ((Globals.candleSettings[3].factor * num46) / num40))
                {
                    double num38;
                    double num39;
                    if (inOpen[i - 1] < inClose[i - 1])
                    {
                        num39 = inOpen[i - 1];
                    }
                    else
                    {
                        num39 = inClose[i - 1];
                    }

                    if (inOpen[i - 2] > inClose[i - 2])
                    {
                        num38 = inOpen[i - 2];
                    }
                    else
                    {
                        num38 = inClose[i - 2];
                    }

                    if (num39 > num38)
                    {
                        double num31;
                        double num37;
                        if (Globals.candleSettings[2].avgPeriod != 0.0)
                        {
                            num37 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                        }
                        else
                        {
                            double num36;
                            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                            {
                                num36 = Math.Abs((double) (inClose[i] - inOpen[i]));
                            }
                            else
                            {
                                double num35;
                                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                {
                                    num35 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    double num32;
                                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                    {
                                        double num33;
                                        double num34;
                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num34 = inClose[i];
                                        }
                                        else
                                        {
                                            num34 = inOpen[i];
                                        }

                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num33 = inOpen[i];
                                        }
                                        else
                                        {
                                            num33 = inClose[i];
                                        }

                                        num32 = (inHigh[i] - num34) + (num33 - inLow[i]);
                                    }
                                    else
                                    {
                                        num32 = 0.0;
                                    }

                                    num35 = num32;
                                }

                                num36 = num35;
                            }

                            num37 = num36;
                        }

                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            num31 = 2.0;
                        }
                        else
                        {
                            num31 = 1.0;
                        }

                        if (((Math.Abs((double) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[2].factor * num37) / num31)) &&
                             (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) &&
                            (inClose[i] < (inClose[i - 2] - (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) * optInPenetration))))
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_0800;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0800:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                double num29;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num26;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num27;
                        double num28;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num28 = inClose[i - 2];
                        }
                        else
                        {
                            num28 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num27 = inOpen[i - 2];
                        }
                        else
                        {
                            num27 = inClose[i - 2];
                        }

                        num26 = (inHigh[i - 2] - num28) + (num27 - inLow[i - 2]);
                    }
                    else
                    {
                        num26 = 0.0;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((double) (inClose[BodyLongTrailingIdx] - inOpen[BodyLongTrailingIdx]));
            }
            else
            {
                double num24;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[BodyLongTrailingIdx] - inLow[BodyLongTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num23 = inClose[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[BodyLongTrailingIdx];
                        }

                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num22 = inOpen[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[BodyLongTrailingIdx];
                        }

                        num21 = (inHigh[BodyLongTrailingIdx] - num23) + (num22 - inLow[BodyLongTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            BodyLongPeriodTotal += num30 - num25;
            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num18 = inClose[i - 1];
                        }
                        else
                        {
                            num18 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num17 = inOpen[i - 1];
                        }
                        else
                        {
                            num17 = inClose[i - 1];
                        }

                        num16 = (inHigh[i - 1] - num18) + (num17 - inLow[i - 1]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((double) (inClose[BodyDojiTrailingIdx] - inOpen[BodyDojiTrailingIdx]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyDojiTrailingIdx] - inLow[BodyDojiTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                        {
                            num13 = inClose[BodyDojiTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[BodyDojiTrailingIdx];
                        }

                        if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                        {
                            num12 = inOpen[BodyDojiTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[BodyDojiTrailingIdx];
                        }

                        num11 = (inHigh[BodyDojiTrailingIdx] - num13) + (num12 - inLow[BodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyDojiPeriodTotal += num20 - num15;
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
            BodyLongTrailingIdx++;
            BodyDojiTrailingIdx++;
            BodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_035B;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlEveningDojiStar(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            double optInPenetration, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
            float num25;
            float num30;
            double num47;
            double num53;
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
                optInPenetration = 0.3;
            }
            else if ((optInPenetration < 0.0) || (optInPenetration > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlEveningDojiStarLookback(optInPenetration);
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

            double BodyLongPeriodTotal = 0.0;
            double BodyDojiPeriodTotal = 0.0;
            double BodyShortPeriodTotal = 0.0;
            int BodyLongTrailingIdx = (startIdx - 2) - Globals.candleSettings[0].avgPeriod;
            int BodyDojiTrailingIdx = (startIdx - 1) - Globals.candleSettings[3].avgPeriod;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num68;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num68 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num67;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num67 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num64;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num65;
                            float num66;
                            if (inClose[i] >= inOpen[i])
                            {
                                num66 = inClose[i];
                            }
                            else
                            {
                                num66 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inOpen[i];
                            }
                            else
                            {
                                num65 = inClose[i];
                            }

                            num64 = (inHigh[i] - num66) + (num65 - inLow[i]);
                        }
                        else
                        {
                            num64 = 0.0f;
                        }

                        num67 = num64;
                    }

                    num68 = num67;
                }

                BodyLongPeriodTotal += num68;
                i++;
            }

            i = BodyDojiTrailingIdx;
            while (true)
            {
                float num63;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num63 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num62;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num62 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num59;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            float num60;
                            float num61;
                            if (inClose[i] >= inOpen[i])
                            {
                                num61 = inClose[i];
                            }
                            else
                            {
                                num61 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num60 = inOpen[i];
                            }
                            else
                            {
                                num60 = inClose[i];
                            }

                            num59 = (inHigh[i] - num61) + (num60 - inLow[i]);
                        }
                        else
                        {
                            num59 = 0.0f;
                        }

                        num62 = num59;
                    }

                    num63 = num62;
                }

                BodyDojiPeriodTotal += num63;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                float num58;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num58 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num57;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num57 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num54;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num55;
                            float num56;
                            if (inClose[i] >= inOpen[i])
                            {
                                num56 = inClose[i];
                            }
                            else
                            {
                                num56 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inOpen[i];
                            }
                            else
                            {
                                num55 = inClose[i];
                            }

                            num54 = (inHigh[i] - num56) + (num55 - inLow[i]);
                        }
                        else
                        {
                            num54 = 0.0f;
                        }

                        num57 = num54;
                    }

                    num58 = num57;
                }

                BodyShortPeriodTotal += num58;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0385:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num53 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                float num52;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num52 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num51;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num51 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num48;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num49;
                            float num50;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num50 = inClose[i - 2];
                            }
                            else
                            {
                                num50 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inOpen[i - 2];
                            }
                            else
                            {
                                num49 = inClose[i - 2];
                            }

                            num48 = (inHigh[i - 2] - num50) + (num49 - inLow[i - 2]);
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

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num47 = 2.0;
            }
            else
            {
                num47 = 1.0;
            }

            if ((Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num53) / num47)) &&
                (inClose[i - 2] >= inOpen[i - 2]))
            {
                double num40;
                double num46;
                if (Globals.candleSettings[3].avgPeriod != 0.0)
                {
                    num46 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                }
                else
                {
                    float num45;
                    if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                    {
                        num45 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        float num44;
                        if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                        {
                            num44 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            float num41;
                            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                            {
                                float num42;
                                float num43;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num43 = inClose[i - 1];
                                }
                                else
                                {
                                    num43 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num42 = inOpen[i - 1];
                                }
                                else
                                {
                                    num42 = inClose[i - 1];
                                }

                                num41 = (inHigh[i - 1] - num43) + (num42 - inLow[i - 1]);
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

                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                {
                    num40 = 2.0;
                }
                else
                {
                    num40 = 1.0;
                }

                if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) <= ((Globals.candleSettings[3].factor * num46) / num40))
                {
                    float num38;
                    float num39;
                    if (inOpen[i - 1] < inClose[i - 1])
                    {
                        num39 = inOpen[i - 1];
                    }
                    else
                    {
                        num39 = inClose[i - 1];
                    }

                    if (inOpen[i - 2] > inClose[i - 2])
                    {
                        num38 = inOpen[i - 2];
                    }
                    else
                    {
                        num38 = inClose[i - 2];
                    }

                    if (num39 > num38)
                    {
                        double num31;
                        double num37;
                        if (Globals.candleSettings[2].avgPeriod != 0.0)
                        {
                            num37 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                        }
                        else
                        {
                            float num36;
                            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                            {
                                num36 = Math.Abs((float) (inClose[i] - inOpen[i]));
                            }
                            else
                            {
                                float num35;
                                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                {
                                    num35 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    float num32;
                                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                    {
                                        float num33;
                                        float num34;
                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num34 = inClose[i];
                                        }
                                        else
                                        {
                                            num34 = inOpen[i];
                                        }

                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num33 = inOpen[i];
                                        }
                                        else
                                        {
                                            num33 = inClose[i];
                                        }

                                        num32 = (inHigh[i] - num34) + (num33 - inLow[i]);
                                    }
                                    else
                                    {
                                        num32 = 0.0f;
                                    }

                                    num35 = num32;
                                }

                                num36 = num35;
                            }

                            num37 = num36;
                        }

                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            num31 = 2.0;
                        }
                        else
                        {
                            num31 = 1.0;
                        }

                        if (((Math.Abs((float) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[2].factor * num37) / num31)) &&
                             (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) &&
                            (inClose[i] < (inClose[i - 2] - (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) * optInPenetration))))
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_0874;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0874:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                float num29;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    float num26;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num27;
                        float num28;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num28 = inClose[i - 2];
                        }
                        else
                        {
                            num28 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num27 = inOpen[i - 2];
                        }
                        else
                        {
                            num27 = inClose[i - 2];
                        }

                        num26 = (inHigh[i - 2] - num28) + (num27 - inLow[i - 2]);
                    }
                    else
                    {
                        num26 = 0.0f;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((float) (inClose[BodyLongTrailingIdx] - inOpen[BodyLongTrailingIdx]));
            }
            else
            {
                float num24;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[BodyLongTrailingIdx] - inLow[BodyLongTrailingIdx];
                }
                else
                {
                    float num21;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num22;
                        float num23;
                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num23 = inClose[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[BodyLongTrailingIdx];
                        }

                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num22 = inOpen[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[BodyLongTrailingIdx];
                        }

                        num21 = (inHigh[BodyLongTrailingIdx] - num23) + (num22 - inLow[BodyLongTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0f;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            BodyLongPeriodTotal += num30 - num25;
            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                    {
                        float num17;
                        float num18;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num18 = inClose[i - 1];
                        }
                        else
                        {
                            num18 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num17 = inOpen[i - 1];
                        }
                        else
                        {
                            num17 = inClose[i - 1];
                        }

                        num16 = (inHigh[i - 1] - num18) + (num17 - inLow[i - 1]);
                    }
                    else
                    {
                        num16 = 0.0f;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((float) (inClose[BodyDojiTrailingIdx] - inOpen[BodyDojiTrailingIdx]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyDojiTrailingIdx] - inLow[BodyDojiTrailingIdx];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                        {
                            num13 = inClose[BodyDojiTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[BodyDojiTrailingIdx];
                        }

                        if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                        {
                            num12 = inOpen[BodyDojiTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[BodyDojiTrailingIdx];
                        }

                        num11 = (inHigh[BodyDojiTrailingIdx] - num13) + (num12 - inLow[BodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyDojiPeriodTotal += num20 - num15;
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
            BodyLongTrailingIdx++;
            BodyDojiTrailingIdx++;
            BodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0385;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlEveningDojiStarLookback(double optInPenetration)
        {
            int avgPeriod;
            if (optInPenetration == -4E+37)
            {
                optInPenetration = 0.3;
            }
            else if ((optInPenetration < 0.0) || (optInPenetration > 3E+37))
            {
                return -1;
            }

            if (((Globals.candleSettings[3].avgPeriod <= Globals.candleSettings[0].avgPeriod)
                    ? Globals.candleSettings[0].avgPeriod
                    : Globals.candleSettings[3].avgPeriod) > Globals.candleSettings[2].avgPeriod)
            {
                avgPeriod = (Globals.candleSettings[3].avgPeriod <= Globals.candleSettings[0].avgPeriod)
                    ? Globals.candleSettings[0].avgPeriod
                    : Globals.candleSettings[3].avgPeriod;
            }
            else
            {
                avgPeriod = Globals.candleSettings[2].avgPeriod;
            }

            return (avgPeriod + 2);
        }
    }
}
