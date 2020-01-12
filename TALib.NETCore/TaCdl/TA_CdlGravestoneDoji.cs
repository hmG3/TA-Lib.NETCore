using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlGravestoneDoji(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num37;
            double num43;
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

            int lookbackTotal = CdlGravestoneDojiLookback();
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
            int i = BodyDojiTrailingIdx;
            while (true)
            {
                double num53;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num53 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num52;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num52 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num49;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                BodyDojiPeriodTotal += num53;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num48;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num48 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num47;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num47 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num44;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
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

                            if (inClose[i] >= inOpen[i])
                            {
                                num45 = inOpen[i];
                            }
                            else
                            {
                                num45 = inClose[i];
                            }

                            num44 = (inHigh[i] - num46) + (num45 - inLow[i]);
                        }
                        else
                        {
                            num44 = 0.0;
                        }

                        num47 = num44;
                    }

                    num48 = num47;
                }

                ShadowVeryShortPeriodTotal += num48;
                i++;
            }

            int outIdx = 0;
            Label_022E:
            if (Globals.candleSettings[3].avgPeriod != 0.0)
            {
                num43 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
            }
            else
            {
                double num42;
                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num42 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num41;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num41 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num38;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            double num39;
                            double num40;
                            if (inClose[i] >= inOpen[i])
                            {
                                num40 = inClose[i];
                            }
                            else
                            {
                                num40 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num39 = inOpen[i];
                            }
                            else
                            {
                                num39 = inClose[i];
                            }

                            num38 = (inHigh[i] - num40) + (num39 - inLow[i]);
                        }
                        else
                        {
                            num38 = 0.0;
                        }

                        num41 = num38;
                    }

                    num42 = num41;
                }

                num43 = num42;
            }

            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
            {
                num37 = 2.0;
            }
            else
            {
                num37 = 1.0;
            }

            if (Math.Abs((double) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[3].factor * num43) / num37))
            {
                double num29;
                double num35;
                double num36;
                if (inClose[i] >= inOpen[i])
                {
                    num36 = inOpen[i];
                }
                else
                {
                    num36 = inClose[i];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num35 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    double num34;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num34 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num33;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num33 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num30;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                double num31;
                                double num32;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num32 = inClose[i];
                                }
                                else
                                {
                                    num32 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num31 = inOpen[i];
                                }
                                else
                                {
                                    num31 = inClose[i];
                                }

                                num30 = (inHigh[i] - num32) + (num31 - inLow[i]);
                            }
                            else
                            {
                                num30 = 0.0;
                            }

                            num33 = num30;
                        }

                        num34 = num33;
                    }

                    num35 = num34;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num29 = 2.0;
                }
                else
                {
                    num29 = 1.0;
                }

                if ((num36 - inLow[i]) < ((Globals.candleSettings[7].factor * num35) / num29))
                {
                    double num21;
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

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num27 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        double num26;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num26 = Math.Abs((double) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            double num25;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num25 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num22;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    double num23;
                                    double num24;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num24 = inClose[i];
                                    }
                                    else
                                    {
                                        num24 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num23 = inOpen[i];
                                    }
                                    else
                                    {
                                        num23 = inClose[i];
                                    }

                                    num22 = (inHigh[i] - num24) + (num23 - inLow[i]);
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

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num21 = 2.0;
                    }
                    else
                    {
                        num21 = 1.0;
                    }

                    if ((inHigh[i] - num28) > ((Globals.candleSettings[7].factor * num27) / num21))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0619;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0619:
            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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
            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[ShadowVeryShortTrailingIdx] - inOpen[ShadowVeryShortTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[ShadowVeryShortTrailingIdx] - inLow[ShadowVeryShortTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[ShadowVeryShortTrailingIdx] >= inOpen[ShadowVeryShortTrailingIdx])
                        {
                            num3 = inClose[ShadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[ShadowVeryShortTrailingIdx];
                        }

                        if (inClose[ShadowVeryShortTrailingIdx] >= inOpen[ShadowVeryShortTrailingIdx])
                        {
                            num2 = inOpen[ShadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[ShadowVeryShortTrailingIdx];
                        }

                        num = (inHigh[ShadowVeryShortTrailingIdx] - num3) + (num2 - inLow[ShadowVeryShortTrailingIdx]);
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
            BodyDojiTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_022E;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlGravestoneDoji(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
            double num37;
            double num43;
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

            int lookbackTotal = CdlGravestoneDojiLookback();
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
            int i = BodyDojiTrailingIdx;
            while (true)
            {
                float num53;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num53 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num52;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num52 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num49;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                BodyDojiPeriodTotal += num53;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num48;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num48 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num47;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num47 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num44;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num45;
                            float num46;
                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inClose[i];
                            }
                            else
                            {
                                num46 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num45 = inOpen[i];
                            }
                            else
                            {
                                num45 = inClose[i];
                            }

                            num44 = (inHigh[i] - num46) + (num45 - inLow[i]);
                        }
                        else
                        {
                            num44 = 0.0f;
                        }

                        num47 = num44;
                    }

                    num48 = num47;
                }

                ShadowVeryShortPeriodTotal += num48;
                i++;
            }

            int outIdx = 0;
            Label_024A:
            if (Globals.candleSettings[3].avgPeriod != 0.0)
            {
                num43 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
            }
            else
            {
                float num42;
                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num42 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num41;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num41 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num38;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            float num39;
                            float num40;
                            if (inClose[i] >= inOpen[i])
                            {
                                num40 = inClose[i];
                            }
                            else
                            {
                                num40 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num39 = inOpen[i];
                            }
                            else
                            {
                                num39 = inClose[i];
                            }

                            num38 = (inHigh[i] - num40) + (num39 - inLow[i]);
                        }
                        else
                        {
                            num38 = 0.0f;
                        }

                        num41 = num38;
                    }

                    num42 = num41;
                }

                num43 = num42;
            }

            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
            {
                num37 = 2.0;
            }
            else
            {
                num37 = 1.0;
            }

            if (Math.Abs((float) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[3].factor * num43) / num37))
            {
                double num29;
                double num35;
                float num36;
                if (inClose[i] >= inOpen[i])
                {
                    num36 = inOpen[i];
                }
                else
                {
                    num36 = inClose[i];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num35 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    float num34;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num34 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num33;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num33 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num30;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                float num31;
                                float num32;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num32 = inClose[i];
                                }
                                else
                                {
                                    num32 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num31 = inOpen[i];
                                }
                                else
                                {
                                    num31 = inClose[i];
                                }

                                num30 = (inHigh[i] - num32) + (num31 - inLow[i]);
                            }
                            else
                            {
                                num30 = 0.0f;
                            }

                            num33 = num30;
                        }

                        num34 = num33;
                    }

                    num35 = num34;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num29 = 2.0;
                }
                else
                {
                    num29 = 1.0;
                }

                if ((num36 - inLow[i]) < ((Globals.candleSettings[7].factor * num35) / num29))
                {
                    double num21;
                    double num27;
                    float num28;
                    if (inClose[i] >= inOpen[i])
                    {
                        num28 = inClose[i];
                    }
                    else
                    {
                        num28 = inOpen[i];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num27 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        float num26;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num26 = Math.Abs((float) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            float num25;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num25 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                float num22;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    float num23;
                                    float num24;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num24 = inClose[i];
                                    }
                                    else
                                    {
                                        num24 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num23 = inOpen[i];
                                    }
                                    else
                                    {
                                        num23 = inClose[i];
                                    }

                                    num22 = (inHigh[i] - num24) + (num23 - inLow[i]);
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

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num21 = 2.0;
                    }
                    else
                    {
                        num21 = 1.0;
                    }

                    if ((inHigh[i] - num28) > ((Globals.candleSettings[7].factor * num27) / num21))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_066B;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_066B:
            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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
            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[ShadowVeryShortTrailingIdx] - inOpen[ShadowVeryShortTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[ShadowVeryShortTrailingIdx] - inLow[ShadowVeryShortTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[ShadowVeryShortTrailingIdx] >= inOpen[ShadowVeryShortTrailingIdx])
                        {
                            num3 = inClose[ShadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[ShadowVeryShortTrailingIdx];
                        }

                        if (inClose[ShadowVeryShortTrailingIdx] >= inOpen[ShadowVeryShortTrailingIdx])
                        {
                            num2 = inOpen[ShadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[ShadowVeryShortTrailingIdx];
                        }

                        num = (inHigh[ShadowVeryShortTrailingIdx] - num3) + (num2 - inLow[ShadowVeryShortTrailingIdx]);
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
            BodyDojiTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_024A;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlGravestoneDojiLookback()
        {
            return ((Globals.candleSettings[3].avgPeriod <= Globals.candleSettings[7].avgPeriod)
                ? Globals.candleSettings[7].avgPeriod
                : Globals.candleSettings[3].avgPeriod);
        }
    }
}
