using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlTakuri(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
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

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlTakuriLookback();
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

            double BodyDojiPeriodTotal = 0.0;
            int BodyDojiTrailingIdx = startIdx - Globals.candleSettings[3].avgPeriod;
            double ShadowVeryShortPeriodTotal = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            double ShadowVeryLongPeriodTotal = 0.0;
            int ShadowVeryLongTrailingIdx = startIdx - Globals.candleSettings[5].avgPeriod;
            int i = BodyDojiTrailingIdx;
            while (true)
            {
                double num68;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num68 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num67;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num67 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num64;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                BodyDojiPeriodTotal += num68;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num63;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num63 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num62;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num62 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num59;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                ShadowVeryShortPeriodTotal += num63;
                i++;
            }

            i = ShadowVeryLongTrailingIdx;
            while (true)
            {
                double num58;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[5].rangeType == RangeType.RealBody)
                {
                    num58 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num57;
                    if (Globals.candleSettings[5].rangeType == RangeType.HighLow)
                    {
                        num57 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num54;
                        if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
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

                ShadowVeryLongPeriodTotal += num58;
                i++;
            }

            int outIdx = 0;
            Label_0313:
            if (Globals.candleSettings[3].avgPeriod != 0.0)
            {
                num53 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
            }
            else
            {
                double num52;
                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num52 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num51;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num51 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num48;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            double num49;
                            double num50;
                            if (inClose[i] >= inOpen[i])
                            {
                                num50 = inClose[i];
                            }
                            else
                            {
                                num50 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num49 = inOpen[i];
                            }
                            else
                            {
                                num49 = inClose[i];
                            }

                            num48 = (inHigh[i] - num50) + (num49 - inLow[i]);
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

            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
            {
                num47 = 2.0;
            }
            else
            {
                num47 = 1.0;
            }

            if (Math.Abs((double) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[3].factor * num53) / num47))
            {
                double num39;
                double num45;
                double num46;
                if (inClose[i] >= inOpen[i])
                {
                    num46 = inClose[i];
                }
                else
                {
                    num46 = inOpen[i];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num45 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    double num44;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num44 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num43;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num43 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num40;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                double num41;
                                double num42;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num42 = inClose[i];
                                }
                                else
                                {
                                    num42 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num41 = inOpen[i];
                                }
                                else
                                {
                                    num41 = inClose[i];
                                }

                                num40 = (inHigh[i] - num42) + (num41 - inLow[i]);
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

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num39 = 2.0;
                }
                else
                {
                    num39 = 1.0;
                }

                if ((inHigh[i] - num46) < ((Globals.candleSettings[7].factor * num45) / num39))
                {
                    double num31;
                    double num37;
                    double num38;
                    if (inClose[i] >= inOpen[i])
                    {
                        num38 = inOpen[i];
                    }
                    else
                    {
                        num38 = inClose[i];
                    }

                    if (Globals.candleSettings[5].avgPeriod != 0.0)
                    {
                        num37 = ShadowVeryLongPeriodTotal / ((double) Globals.candleSettings[5].avgPeriod);
                    }
                    else
                    {
                        double num36;
                        if (Globals.candleSettings[5].rangeType == RangeType.RealBody)
                        {
                            num36 = Math.Abs((double) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            double num35;
                            if (Globals.candleSettings[5].rangeType == RangeType.HighLow)
                            {
                                num35 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num32;
                                if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
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

                    if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
                    {
                        num31 = 2.0;
                    }
                    else
                    {
                        num31 = 1.0;
                    }

                    if ((num38 - inLow[i]) > ((Globals.candleSettings[5].factor * num37) / num31))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0704;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0704:
            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num29;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num26;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((double) (inClose[BodyDojiTrailingIdx] - inOpen[BodyDojiTrailingIdx]));
            }
            else
            {
                double num24;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[BodyDojiTrailingIdx] - inLow[BodyDojiTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                        {
                            num23 = inClose[BodyDojiTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[BodyDojiTrailingIdx];
                        }

                        if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                        {
                            num22 = inOpen[BodyDojiTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[BodyDojiTrailingIdx];
                        }

                        num21 = (inHigh[BodyDojiTrailingIdx] - num23) + (num22 - inLow[BodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            BodyDojiPeriodTotal += num30 - num25;
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
            if (Globals.candleSettings[5].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[5].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[5].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[ShadowVeryLongTrailingIdx] - inOpen[ShadowVeryLongTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[5].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[ShadowVeryLongTrailingIdx] - inLow[ShadowVeryLongTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[ShadowVeryLongTrailingIdx] >= inOpen[ShadowVeryLongTrailingIdx])
                        {
                            num3 = inClose[ShadowVeryLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[ShadowVeryLongTrailingIdx];
                        }

                        if (inClose[ShadowVeryLongTrailingIdx] >= inOpen[ShadowVeryLongTrailingIdx])
                        {
                            num2 = inOpen[ShadowVeryLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[ShadowVeryLongTrailingIdx];
                        }

                        num = (inHigh[ShadowVeryLongTrailingIdx] - num3) + (num2 - inLow[ShadowVeryLongTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            ShadowVeryLongPeriodTotal += num10 - num5;
            i++;
            BodyDojiTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            ShadowVeryLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0313;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlTakuri(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
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

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlTakuriLookback();
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

            double BodyDojiPeriodTotal = 0.0;
            int BodyDojiTrailingIdx = startIdx - Globals.candleSettings[3].avgPeriod;
            double ShadowVeryShortPeriodTotal = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            double ShadowVeryLongPeriodTotal = 0.0;
            int ShadowVeryLongTrailingIdx = startIdx - Globals.candleSettings[5].avgPeriod;
            int i = BodyDojiTrailingIdx;
            while (true)
            {
                float num68;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num68 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num67;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num67 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num64;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                BodyDojiPeriodTotal += num68;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num63;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num63 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num62;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num62 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num59;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                ShadowVeryShortPeriodTotal += num63;
                i++;
            }

            i = ShadowVeryLongTrailingIdx;
            while (true)
            {
                float num58;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[5].rangeType == RangeType.RealBody)
                {
                    num58 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num57;
                    if (Globals.candleSettings[5].rangeType == RangeType.HighLow)
                    {
                        num57 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num54;
                        if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
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

                ShadowVeryLongPeriodTotal += num58;
                i++;
            }

            int outIdx = 0;
            Label_033D:
            if (Globals.candleSettings[3].avgPeriod != 0.0)
            {
                num53 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
            }
            else
            {
                float num52;
                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num52 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num51;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num51 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num48;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            float num49;
                            float num50;
                            if (inClose[i] >= inOpen[i])
                            {
                                num50 = inClose[i];
                            }
                            else
                            {
                                num50 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num49 = inOpen[i];
                            }
                            else
                            {
                                num49 = inClose[i];
                            }

                            num48 = (inHigh[i] - num50) + (num49 - inLow[i]);
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

            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
            {
                num47 = 2.0;
            }
            else
            {
                num47 = 1.0;
            }

            if (Math.Abs((float) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[3].factor * num53) / num47))
            {
                double num39;
                double num45;
                float num46;
                if (inClose[i] >= inOpen[i])
                {
                    num46 = inClose[i];
                }
                else
                {
                    num46 = inOpen[i];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num45 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    float num44;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num44 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num43;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num43 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num40;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                float num41;
                                float num42;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num42 = inClose[i];
                                }
                                else
                                {
                                    num42 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num41 = inOpen[i];
                                }
                                else
                                {
                                    num41 = inClose[i];
                                }

                                num40 = (inHigh[i] - num42) + (num41 - inLow[i]);
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

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num39 = 2.0;
                }
                else
                {
                    num39 = 1.0;
                }

                if ((inHigh[i] - num46) < ((Globals.candleSettings[7].factor * num45) / num39))
                {
                    double num31;
                    double num37;
                    float num38;
                    if (inClose[i] >= inOpen[i])
                    {
                        num38 = inOpen[i];
                    }
                    else
                    {
                        num38 = inClose[i];
                    }

                    if (Globals.candleSettings[5].avgPeriod != 0.0)
                    {
                        num37 = ShadowVeryLongPeriodTotal / ((double) Globals.candleSettings[5].avgPeriod);
                    }
                    else
                    {
                        float num36;
                        if (Globals.candleSettings[5].rangeType == RangeType.RealBody)
                        {
                            num36 = Math.Abs((float) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            float num35;
                            if (Globals.candleSettings[5].rangeType == RangeType.HighLow)
                            {
                                num35 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                float num32;
                                if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
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

                    if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
                    {
                        num31 = 2.0;
                    }
                    else
                    {
                        num31 = 1.0;
                    }

                    if ((num38 - inLow[i]) > ((Globals.candleSettings[5].factor * num37) / num31))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0764;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0764:
            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num29;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num26;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((float) (inClose[BodyDojiTrailingIdx] - inOpen[BodyDojiTrailingIdx]));
            }
            else
            {
                float num24;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[BodyDojiTrailingIdx] - inLow[BodyDojiTrailingIdx];
                }
                else
                {
                    float num21;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                    {
                        float num22;
                        float num23;
                        if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                        {
                            num23 = inClose[BodyDojiTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[BodyDojiTrailingIdx];
                        }

                        if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                        {
                            num22 = inOpen[BodyDojiTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[BodyDojiTrailingIdx];
                        }

                        num21 = (inHigh[BodyDojiTrailingIdx] - num23) + (num22 - inLow[BodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0f;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            BodyDojiPeriodTotal += num30 - num25;
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
            if (Globals.candleSettings[5].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[5].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[5].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[ShadowVeryLongTrailingIdx] - inOpen[ShadowVeryLongTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[5].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[ShadowVeryLongTrailingIdx] - inLow[ShadowVeryLongTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[5].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[ShadowVeryLongTrailingIdx] >= inOpen[ShadowVeryLongTrailingIdx])
                        {
                            num3 = inClose[ShadowVeryLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[ShadowVeryLongTrailingIdx];
                        }

                        if (inClose[ShadowVeryLongTrailingIdx] >= inOpen[ShadowVeryLongTrailingIdx])
                        {
                            num2 = inOpen[ShadowVeryLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[ShadowVeryLongTrailingIdx];
                        }

                        num = (inHigh[ShadowVeryLongTrailingIdx] - num3) + (num2 - inLow[ShadowVeryLongTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            ShadowVeryLongPeriodTotal += num10 - num5;
            i++;
            BodyDojiTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            ShadowVeryLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_033D;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlTakuriLookback()
        {
            if (((Globals.candleSettings[3].avgPeriod <= Globals.candleSettings[7].avgPeriod)
                    ? Globals.candleSettings[7].avgPeriod
                    : Globals.candleSettings[3].avgPeriod) > Globals.candleSettings[5].avgPeriod)
            {
                return ((Globals.candleSettings[3].avgPeriod <= Globals.candleSettings[7].avgPeriod)
                    ? Globals.candleSettings[7].avgPeriod
                    : Globals.candleSettings[3].avgPeriod);
            }

            return Globals.candleSettings[5].avgPeriod;
        }
    }
}
