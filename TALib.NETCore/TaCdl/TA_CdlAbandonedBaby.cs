using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlAbandonedBaby(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            double optInPenetration, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            double num46;
            double num52;
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

            int lookbackTotal = CdlAbandonedBabyLookback(optInPenetration);
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
                double num67;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num66;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num63;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num64;
                            double num65;
                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inClose[i];
                            }
                            else
                            {
                                num65 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num64 = inOpen[i];
                            }
                            else
                            {
                                num64 = inClose[i];
                            }

                            num63 = (inHigh[i] - num65) + (num64 - inLow[i]);
                        }
                        else
                        {
                            num63 = 0.0;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                BodyLongPeriodTotal += num67;
                i++;
            }

            i = BodyDojiTrailingIdx;
            while (true)
            {
                double num62;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num62 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num61;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num61 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num58;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                BodyDojiPeriodTotal += num62;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                double num57;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num57 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num56;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num56 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num53;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num54;
                            double num55;
                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inClose[i];
                            }
                            else
                            {
                                num55 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num54 = inOpen[i];
                            }
                            else
                            {
                                num54 = inClose[i];
                            }

                            num53 = (inHigh[i] - num55) + (num54 - inLow[i]);
                        }
                        else
                        {
                            num53 = 0.0;
                        }

                        num56 = num53;
                    }

                    num57 = num56;
                }

                BodyShortPeriodTotal += num57;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_035B:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num52 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                double num51;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num51 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num50;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num50 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num47;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num48;
                            double num49;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inClose[i - 2];
                            }
                            else
                            {
                                num49 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num48 = inOpen[i - 2];
                            }
                            else
                            {
                                num48 = inClose[i - 2];
                            }

                            num47 = (inHigh[i - 2] - num49) + (num48 - inLow[i - 2]);
                        }
                        else
                        {
                            num47 = 0.0;
                        }

                        num50 = num47;
                    }

                    num51 = num50;
                }

                num52 = num51;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num46 = 2.0;
            }
            else
            {
                num46 = 1.0;
            }

            if (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num52) / num46))
            {
                double num39;
                double num45;
                if (Globals.candleSettings[3].avgPeriod != 0.0)
                {
                    num45 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                }
                else
                {
                    double num44;
                    if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                    {
                        num44 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        double num43;
                        if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                        {
                            num43 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num40;
                            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                            {
                                double num41;
                                double num42;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num42 = inClose[i - 1];
                                }
                                else
                                {
                                    num42 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num41 = inOpen[i - 1];
                                }
                                else
                                {
                                    num41 = inClose[i - 1];
                                }

                                num40 = (inHigh[i - 1] - num42) + (num41 - inLow[i - 1]);
                            }
                            else
                            {
                                num40 = 0.0;
                            }

                            num43 = num40;
                        }

                        num44 = num43;
                    }

                    num45 = num44;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                {
                    num39 = 2.0;
                }
                else
                {
                    num39 = 1.0;
                }

                if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) <= ((Globals.candleSettings[3].factor * num45) / num39))
                {
                    double num32;
                    double num38;
                    if (Globals.candleSettings[2].avgPeriod != 0.0)
                    {
                        num38 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                    }
                    else
                    {
                        double num37;
                        if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                        {
                            num37 = Math.Abs((double) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            double num36;
                            if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                            {
                                num36 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num33;
                                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                {
                                    double num34;
                                    double num35;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num35 = inClose[i];
                                    }
                                    else
                                    {
                                        num35 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num34 = inOpen[i];
                                    }
                                    else
                                    {
                                        num34 = inClose[i];
                                    }

                                    num33 = (inHigh[i] - num35) + (num34 - inLow[i]);
                                }
                                else
                                {
                                    num33 = 0.0;
                                }

                                num36 = num33;
                            }

                            num37 = num36;
                        }

                        num38 = num37;
                    }

                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        num32 = 2.0;
                    }
                    else
                    {
                        num32 = 1.0;
                    }

                    if ((Math.Abs((double) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[2].factor * num38) / num32)) &&
                        ((((inClose[i - 2] >= inOpen[i - 2]) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) &&
                          (((inClose[i] < (inClose[i - 2] - (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) * optInPenetration))) &&
                            (inLow[i - 1] > inHigh[i - 2])) && (inHigh[i] < inLow[i - 1]))) ||
                         ((((((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1) && (inClose[i] >= inOpen[i])) &&
                           ((inClose[i] > (inClose[i - 2] + (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) * optInPenetration))) &&
                            (inHigh[i - 1] < inLow[i - 2]))) && (inLow[i] > inHigh[i - 1]))))
                    {
                        int num31;
                        if (inClose[i] >= inOpen[i])
                        {
                            num31 = 1;
                        }
                        else
                        {
                            num31 = -1;
                        }

                        outInteger[outIdx] = num31 * 100;
                        outIdx++;
                        goto Label_0844;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0844:
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

        public static RetCode CdlAbandonedBaby(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            double optInPenetration, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
            float num25;
            float num30;
            double num46;
            double num52;
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

            int lookbackTotal = CdlAbandonedBabyLookback(optInPenetration);
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
                float num67;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num66;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num63;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num64;
                            float num65;
                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inClose[i];
                            }
                            else
                            {
                                num65 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num64 = inOpen[i];
                            }
                            else
                            {
                                num64 = inClose[i];
                            }

                            num63 = (inHigh[i] - num65) + (num64 - inLow[i]);
                        }
                        else
                        {
                            num63 = 0.0f;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                BodyLongPeriodTotal += num67;
                i++;
            }

            i = BodyDojiTrailingIdx;
            while (true)
            {
                float num62;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num62 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num61;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num61 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num58;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                BodyDojiPeriodTotal += num62;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                float num57;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num57 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num56;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num56 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num53;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num54;
                            float num55;
                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inClose[i];
                            }
                            else
                            {
                                num55 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num54 = inOpen[i];
                            }
                            else
                            {
                                num54 = inClose[i];
                            }

                            num53 = (inHigh[i] - num55) + (num54 - inLow[i]);
                        }
                        else
                        {
                            num53 = 0.0f;
                        }

                        num56 = num53;
                    }

                    num57 = num56;
                }

                BodyShortPeriodTotal += num57;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0385:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num52 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                float num51;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num51 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num50;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num50 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num47;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num48;
                            float num49;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inClose[i - 2];
                            }
                            else
                            {
                                num49 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num48 = inOpen[i - 2];
                            }
                            else
                            {
                                num48 = inClose[i - 2];
                            }

                            num47 = (inHigh[i - 2] - num49) + (num48 - inLow[i - 2]);
                        }
                        else
                        {
                            num47 = 0.0f;
                        }

                        num50 = num47;
                    }

                    num51 = num50;
                }

                num52 = num51;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num46 = 2.0;
            }
            else
            {
                num46 = 1.0;
            }

            if (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num52) / num46))
            {
                double num39;
                double num45;
                if (Globals.candleSettings[3].avgPeriod != 0.0)
                {
                    num45 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                }
                else
                {
                    float num44;
                    if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                    {
                        num44 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        float num43;
                        if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                        {
                            num43 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            float num40;
                            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                            {
                                float num41;
                                float num42;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num42 = inClose[i - 1];
                                }
                                else
                                {
                                    num42 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num41 = inOpen[i - 1];
                                }
                                else
                                {
                                    num41 = inClose[i - 1];
                                }

                                num40 = (inHigh[i - 1] - num42) + (num41 - inLow[i - 1]);
                            }
                            else
                            {
                                num40 = 0.0f;
                            }

                            num43 = num40;
                        }

                        num44 = num43;
                    }

                    num45 = num44;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                {
                    num39 = 2.0;
                }
                else
                {
                    num39 = 1.0;
                }

                if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) <= ((Globals.candleSettings[3].factor * num45) / num39))
                {
                    double num32;
                    double num38;
                    if (Globals.candleSettings[2].avgPeriod != 0.0)
                    {
                        num38 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                    }
                    else
                    {
                        float num37;
                        if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                        {
                            num37 = Math.Abs((float) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            float num36;
                            if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                            {
                                num36 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                float num33;
                                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                {
                                    float num34;
                                    float num35;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num35 = inClose[i];
                                    }
                                    else
                                    {
                                        num35 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num34 = inOpen[i];
                                    }
                                    else
                                    {
                                        num34 = inClose[i];
                                    }

                                    num33 = (inHigh[i] - num35) + (num34 - inLow[i]);
                                }
                                else
                                {
                                    num33 = 0.0f;
                                }

                                num36 = num33;
                            }

                            num37 = num36;
                        }

                        num38 = num37;
                    }

                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        num32 = 2.0;
                    }
                    else
                    {
                        num32 = 1.0;
                    }

                    if ((Math.Abs((float) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[2].factor * num38) / num32)) &&
                        ((((inClose[i - 2] >= inOpen[i - 2]) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) &&
                          (((inClose[i] < (inClose[i - 2] - (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) * optInPenetration))) &&
                            (inLow[i - 1] > inHigh[i - 2])) && (inHigh[i] < inLow[i - 1]))) ||
                         ((((((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1) && (inClose[i] >= inOpen[i])) &&
                           ((inClose[i] > (inClose[i - 2] + (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) * optInPenetration))) &&
                            (inHigh[i - 1] < inLow[i - 2]))) && (inLow[i] > inHigh[i - 1]))))
                    {
                        int num31;
                        if (inClose[i] >= inOpen[i])
                        {
                            num31 = 1;
                        }
                        else
                        {
                            num31 = -1;
                        }

                        outInteger[outIdx] = num31 * 100;
                        outIdx++;
                        goto Label_08C5;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_08C5:
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

        public static int CdlAbandonedBabyLookback(double optInPenetration)
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
