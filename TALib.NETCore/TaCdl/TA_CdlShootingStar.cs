using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlShootingStar(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            double num49;
            double num55;
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

            int lookbackTotal = CdlShootingStarLookback();
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
            int i = BodyTrailingIdx;
            while (true)
            {
                double num70;
                if (i >= startIdx)
                {
                    break;
                }

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

                BodyPeriodTotal += num70;
                i++;
            }

            i = ShadowLongTrailingIdx;
            while (true)
            {
                double num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num64;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num61;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            double num62;
                            double num63;
                            if (inClose[i] >= inOpen[i])
                            {
                                num63 = inClose[i];
                            }
                            else
                            {
                                num63 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num62 = inOpen[i];
                            }
                            else
                            {
                                num62 = inClose[i];
                            }

                            num61 = (inHigh[i] - num63) + (num62 - inLow[i]);
                        }
                        else
                        {
                            num61 = 0.0;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                ShadowLongPeriodTotal += num65;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num60;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num59;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num56;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num57;
                            double num58;
                            if (inClose[i] >= inOpen[i])
                            {
                                num58 = inClose[i];
                            }
                            else
                            {
                                num58 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num57 = inOpen[i];
                            }
                            else
                            {
                                num57 = inClose[i];
                            }

                            num56 = (inHigh[i] - num58) + (num57 - inLow[i]);
                        }
                        else
                        {
                            num56 = 0.0;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                ShadowVeryShortPeriodTotal += num60;
                i++;
            }

            int outIdx = 0;
            Label_0313:
            if (Globals.candleSettings[2].avgPeriod != 0.0)
            {
                num55 = BodyPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
            }
            else
            {
                double num54;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num53;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num50;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
            {
                num49 = 2.0;
            }
            else
            {
                num49 = 1.0;
            }

            if (Math.Abs((double) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num55) / num49))
            {
                double num41;
                double num47;
                double num48;
                if (inClose[i] >= inOpen[i])
                {
                    num48 = inClose[i];
                }
                else
                {
                    num48 = inOpen[i];
                }

                if (Globals.candleSettings[4].avgPeriod != 0.0)
                {
                    num47 = ShadowLongPeriodTotal / ((double) Globals.candleSettings[4].avgPeriod);
                }
                else
                {
                    double num46;
                    if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                    {
                        num46 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num45;
                        if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                        {
                            num45 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num42;
                            if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                {
                    num41 = 2.0;
                }
                else
                {
                    num41 = 1.0;
                }

                if ((inHigh[i] - num48) > ((Globals.candleSettings[4].factor * num47) / num41))
                {
                    double num33;
                    double num39;
                    double num40;
                    if (inClose[i] >= inOpen[i])
                    {
                        num40 = inOpen[i];
                    }
                    else
                    {
                        num40 = inClose[i];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num39 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        double num38;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num38 = Math.Abs((double) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            double num37;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num37 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num34;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    double num35;
                                    double num36;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num36 = inClose[i];
                                    }
                                    else
                                    {
                                        num36 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num35 = inOpen[i];
                                    }
                                    else
                                    {
                                        num35 = inClose[i];
                                    }

                                    num34 = (inHigh[i] - num36) + (num35 - inLow[i]);
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

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num33 = 2.0;
                    }
                    else
                    {
                        num33 = 1.0;
                    }

                    if ((num40 - inLow[i]) < ((Globals.candleSettings[7].factor * num39) / num33))
                    {
                        double num31;
                        double num32;
                        if (inOpen[i] < inClose[i])
                        {
                            num32 = inOpen[i];
                        }
                        else
                        {
                            num32 = inClose[i];
                        }

                        if (inOpen[i - 1] > inClose[i - 1])
                        {
                            num31 = inOpen[i - 1];
                        }
                        else
                        {
                            num31 = inClose[i - 1];
                        }

                        if (num32 > num31)
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_073E;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_073E:
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num29;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num26;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((double) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                double num24;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num23 = inClose[BodyTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[BodyTrailingIdx];
                        }

                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num22 = inOpen[BodyTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[BodyTrailingIdx];
                        }

                        num21 = (inHigh[BodyTrailingIdx] - num23) + (num22 - inLow[BodyTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            BodyPeriodTotal += num30 - num25;
            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((double) (inClose[ShadowLongTrailingIdx] - inOpen[ShadowLongTrailingIdx]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[ShadowLongTrailingIdx] - inLow[ShadowLongTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[ShadowLongTrailingIdx] >= inOpen[ShadowLongTrailingIdx])
                        {
                            num13 = inClose[ShadowLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[ShadowLongTrailingIdx];
                        }

                        if (inClose[ShadowLongTrailingIdx] >= inOpen[ShadowLongTrailingIdx])
                        {
                            num12 = inOpen[ShadowLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[ShadowLongTrailingIdx];
                        }

                        num11 = (inHigh[ShadowLongTrailingIdx] - num13) + (num12 - inLow[ShadowLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            ShadowLongPeriodTotal += num20 - num15;
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
            BodyTrailingIdx++;
            ShadowLongTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0313;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlShootingStar(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
            float num25;
            float num30;
            double num49;
            double num55;
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

            int lookbackTotal = CdlShootingStarLookback();
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
            int i = BodyTrailingIdx;
            while (true)
            {
                float num70;
                if (i >= startIdx)
                {
                    break;
                }

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

                BodyPeriodTotal += num70;
                i++;
            }

            i = ShadowLongTrailingIdx;
            while (true)
            {
                float num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num64;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num61;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            float num62;
                            float num63;
                            if (inClose[i] >= inOpen[i])
                            {
                                num63 = inClose[i];
                            }
                            else
                            {
                                num63 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num62 = inOpen[i];
                            }
                            else
                            {
                                num62 = inClose[i];
                            }

                            num61 = (inHigh[i] - num63) + (num62 - inLow[i]);
                        }
                        else
                        {
                            num61 = 0.0f;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                ShadowLongPeriodTotal += num65;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num60;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num59;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num56;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num57;
                            float num58;
                            if (inClose[i] >= inOpen[i])
                            {
                                num58 = inClose[i];
                            }
                            else
                            {
                                num58 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num57 = inOpen[i];
                            }
                            else
                            {
                                num57 = inClose[i];
                            }

                            num56 = (inHigh[i] - num58) + (num57 - inLow[i]);
                        }
                        else
                        {
                            num56 = 0.0f;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                ShadowVeryShortPeriodTotal += num60;
                i++;
            }

            int outIdx = 0;
            Label_033D:
            if (Globals.candleSettings[2].avgPeriod != 0.0)
            {
                num55 = BodyPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
            }
            else
            {
                float num54;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num53;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num50;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
            {
                num49 = 2.0;
            }
            else
            {
                num49 = 1.0;
            }

            if (Math.Abs((float) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num55) / num49))
            {
                double num41;
                double num47;
                float num48;
                if (inClose[i] >= inOpen[i])
                {
                    num48 = inClose[i];
                }
                else
                {
                    num48 = inOpen[i];
                }

                if (Globals.candleSettings[4].avgPeriod != 0.0)
                {
                    num47 = ShadowLongPeriodTotal / ((double) Globals.candleSettings[4].avgPeriod);
                }
                else
                {
                    float num46;
                    if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                    {
                        num46 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num45;
                        if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                        {
                            num45 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num42;
                            if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                {
                    num41 = 2.0;
                }
                else
                {
                    num41 = 1.0;
                }

                if ((inHigh[i] - num48) > ((Globals.candleSettings[4].factor * num47) / num41))
                {
                    double num33;
                    double num39;
                    float num40;
                    if (inClose[i] >= inOpen[i])
                    {
                        num40 = inOpen[i];
                    }
                    else
                    {
                        num40 = inClose[i];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num39 = ShadowVeryShortPeriodTotal / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        float num38;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num38 = Math.Abs((float) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            float num37;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num37 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                float num34;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    float num35;
                                    float num36;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num36 = inClose[i];
                                    }
                                    else
                                    {
                                        num36 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num35 = inOpen[i];
                                    }
                                    else
                                    {
                                        num35 = inClose[i];
                                    }

                                    num34 = (inHigh[i] - num36) + (num35 - inLow[i]);
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

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num33 = 2.0;
                    }
                    else
                    {
                        num33 = 1.0;
                    }

                    if ((num40 - inLow[i]) < ((Globals.candleSettings[7].factor * num39) / num33))
                    {
                        float num31;
                        float num32;
                        if (inOpen[i] < inClose[i])
                        {
                            num32 = inOpen[i];
                        }
                        else
                        {
                            num32 = inClose[i];
                        }

                        if (inOpen[i - 1] > inClose[i - 1])
                        {
                            num31 = inOpen[i - 1];
                        }
                        else
                        {
                            num31 = inClose[i - 1];
                        }

                        if (num32 > num31)
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_07A8;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_07A8:
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num29;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num26;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((float) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                float num24;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    float num21;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num22;
                        float num23;
                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num23 = inClose[BodyTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[BodyTrailingIdx];
                        }

                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num22 = inOpen[BodyTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[BodyTrailingIdx];
                        }

                        num21 = (inHigh[BodyTrailingIdx] - num23) + (num22 - inLow[BodyTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0f;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            BodyPeriodTotal += num30 - num25;
            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((float) (inClose[ShadowLongTrailingIdx] - inOpen[ShadowLongTrailingIdx]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[ShadowLongTrailingIdx] - inLow[ShadowLongTrailingIdx];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[ShadowLongTrailingIdx] >= inOpen[ShadowLongTrailingIdx])
                        {
                            num13 = inClose[ShadowLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[ShadowLongTrailingIdx];
                        }

                        if (inClose[ShadowLongTrailingIdx] >= inOpen[ShadowLongTrailingIdx])
                        {
                            num12 = inOpen[ShadowLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[ShadowLongTrailingIdx];
                        }

                        num11 = (inHigh[ShadowLongTrailingIdx] - num13) + (num12 - inLow[ShadowLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            ShadowLongPeriodTotal += num20 - num15;
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
            BodyTrailingIdx++;
            ShadowLongTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_033D;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlShootingStarLookback()
        {
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

            return (avgPeriod + 1);
        }
    }
}
