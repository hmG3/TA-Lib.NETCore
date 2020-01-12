using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlGapSideSideWhite(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num52;
            double num53;
            double num54;
            double num55;
            double num58;
            double num59;
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

            int lookbackTotal = CdlGapSideSideWhiteLookback();
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

            double NearPeriodTotal = 0.0;
            double EqualPeriodTotal = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            int EqualTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            int i = NearTrailingIdx;
            while (true)
            {
                double num69;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num69 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num68;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num68 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num65;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            double num66;
                            double num67;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num67 = inClose[i - 1];
                            }
                            else
                            {
                                num67 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num66 = inOpen[i - 1];
                            }
                            else
                            {
                                num66 = inClose[i - 1];
                            }

                            num65 = (inHigh[i - 1] - num67) + (num66 - inLow[i - 1]);
                        }
                        else
                        {
                            num65 = 0.0;
                        }

                        num68 = num65;
                    }

                    num69 = num68;
                }

                NearPeriodTotal += num69;
                i++;
            }

            i = EqualTrailingIdx;
            while (true)
            {
                double num64;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num64 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num63;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num63 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num60;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            double num61;
                            double num62;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num62 = inClose[i - 1];
                            }
                            else
                            {
                                num62 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num61 = inOpen[i - 1];
                            }
                            else
                            {
                                num61 = inClose[i - 1];
                            }

                            num60 = (inHigh[i - 1] - num62) + (num61 - inLow[i - 1]);
                        }
                        else
                        {
                            num60 = 0.0;
                        }

                        num63 = num60;
                    }

                    num64 = num63;
                }

                EqualPeriodTotal += num64;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0272:
            if (inOpen[i - 1] < inClose[i - 1])
            {
                num59 = inOpen[i - 1];
            }
            else
            {
                num59 = inClose[i - 1];
            }

            if (inOpen[i - 2] > inClose[i - 2])
            {
                num58 = inOpen[i - 2];
            }
            else
            {
                num58 = inClose[i - 2];
            }

            if (num59 > num58)
            {
                double num56;
                double num57;
                if (inOpen[i] < inClose[i])
                {
                    num57 = inOpen[i];
                }
                else
                {
                    num57 = inClose[i];
                }

                if (inOpen[i - 2] > inClose[i - 2])
                {
                    num56 = inOpen[i - 2];
                }
                else
                {
                    num56 = inClose[i - 2];
                }

                if (num57 > num56)
                {
                    goto Label_0373;
                }
            }

            if (inOpen[i - 1] > inClose[i - 1])
            {
                num55 = inOpen[i - 1];
            }
            else
            {
                num55 = inClose[i - 1];
            }

            if (inOpen[i - 2] < inClose[i - 2])
            {
                num54 = inOpen[i - 2];
            }
            else
            {
                num54 = inClose[i - 2];
            }

            if (num55 >= num54)
            {
                goto Label_0990;
            }

            if (inOpen[i] > inClose[i])
            {
                num53 = inOpen[i];
            }
            else
            {
                num53 = inClose[i];
            }

            if (inOpen[i - 2] < inClose[i - 2])
            {
                num52 = inOpen[i - 2];
            }
            else
            {
                num52 = inClose[i - 2];
            }

            if (num53 >= num52)
            {
                goto Label_0990;
            }

            Label_0373:
            if ((inClose[i - 1] >= inOpen[i - 1]) && (inClose[i] >= inOpen[i]))
            {
                double num45;
                double num51;
                if (Globals.candleSettings[8].avgPeriod != 0.0)
                {
                    num51 = NearPeriodTotal / ((double) Globals.candleSettings[8].avgPeriod);
                }
                else
                {
                    double num50;
                    if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                    {
                        num50 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        double num49;
                        if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                        {
                            num49 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num46;
                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                double num47;
                                double num48;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num48 = inClose[i - 1];
                                }
                                else
                                {
                                    num48 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num47 = inOpen[i - 1];
                                }
                                else
                                {
                                    num47 = inClose[i - 1];
                                }

                                num46 = (inHigh[i - 1] - num48) + (num47 - inLow[i - 1]);
                            }
                            else
                            {
                                num46 = 0.0;
                            }

                            num49 = num46;
                        }

                        num50 = num49;
                    }

                    num51 = num50;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                {
                    num45 = 2.0;
                }
                else
                {
                    num45 = 1.0;
                }

                if (Math.Abs((double) (inClose[i] - inOpen[i])) >=
                    (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) - ((Globals.candleSettings[8].factor * num51) / num45)))
                {
                    double num38;
                    double num44;
                    if (Globals.candleSettings[8].avgPeriod != 0.0)
                    {
                        num44 = NearPeriodTotal / ((double) Globals.candleSettings[8].avgPeriod);
                    }
                    else
                    {
                        double num43;
                        if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                        {
                            num43 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            double num42;
                            if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                            {
                                num42 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num39;
                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                {
                                    double num40;
                                    double num41;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num41 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num41 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num40 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num40 = inClose[i - 1];
                                    }

                                    num39 = (inHigh[i - 1] - num41) + (num40 - inLow[i - 1]);
                                }
                                else
                                {
                                    num39 = 0.0;
                                }

                                num42 = num39;
                            }

                            num43 = num42;
                        }

                        num44 = num43;
                    }

                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                    {
                        num38 = 2.0;
                    }
                    else
                    {
                        num38 = 1.0;
                    }

                    if (Math.Abs((double) (inClose[i] - inOpen[i])) <=
                        (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) + ((Globals.candleSettings[8].factor * num44) / num38)))
                    {
                        double num31;
                        double num37;
                        if (Globals.candleSettings[10].avgPeriod != 0.0)
                        {
                            num37 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                        }
                        else
                        {
                            double num36;
                            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                            {
                                num36 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                double num35;
                                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                {
                                    num35 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num32;
                                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                    {
                                        double num33;
                                        double num34;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num34 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num34 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num33 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num33 = inClose[i - 1];
                                        }

                                        num32 = (inHigh[i - 1] - num34) + (num33 - inLow[i - 1]);
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

                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            num31 = 2.0;
                        }
                        else
                        {
                            num31 = 1.0;
                        }

                        if (inOpen[i] >= (inOpen[i - 1] - ((Globals.candleSettings[10].factor * num37) / num31)))
                        {
                            double num24;
                            double num30;
                            if (Globals.candleSettings[10].avgPeriod != 0.0)
                            {
                                num30 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                            }
                            else
                            {
                                double num29;
                                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                {
                                    num29 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                                }
                                else
                                {
                                    double num28;
                                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                    {
                                        num28 = inHigh[i - 1] - inLow[i - 1];
                                    }
                                    else
                                    {
                                        double num25;
                                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                        {
                                            double num26;
                                            double num27;
                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num27 = inClose[i - 1];
                                            }
                                            else
                                            {
                                                num27 = inOpen[i - 1];
                                            }

                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num26 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num26 = inClose[i - 1];
                                            }

                                            num25 = (inHigh[i - 1] - num27) + (num26 - inLow[i - 1]);
                                        }
                                        else
                                        {
                                            num25 = 0.0;
                                        }

                                        num28 = num25;
                                    }

                                    num29 = num28;
                                }

                                num30 = num29;
                            }

                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                num24 = 2.0;
                            }
                            else
                            {
                                num24 = 1.0;
                            }

                            if (inOpen[i] <= (inOpen[i - 1] + ((Globals.candleSettings[10].factor * num30) / num24)))
                            {
                                int num21;
                                double num22;
                                double num23;
                                if (inOpen[i - 1] < inClose[i - 1])
                                {
                                    num23 = inOpen[i - 1];
                                }
                                else
                                {
                                    num23 = inClose[i - 1];
                                }

                                if (inOpen[i - 2] > inClose[i - 2])
                                {
                                    num22 = inOpen[i - 2];
                                }
                                else
                                {
                                    num22 = inClose[i - 2];
                                }

                                if (num23 > num22)
                                {
                                    num21 = 100;
                                }
                                else
                                {
                                    num21 = -100;
                                }

                                outInteger[outIdx] = num21;
                                outIdx++;
                                goto Label_0999;
                            }
                        }
                    }
                }
            }

            Label_0990:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0999:
            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((double) (inClose[NearTrailingIdx - 1] - inOpen[NearTrailingIdx - 1]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[NearTrailingIdx - 1] - inLow[NearTrailingIdx - 1];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[NearTrailingIdx - 1] >= inOpen[NearTrailingIdx - 1])
                        {
                            num13 = inClose[NearTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[NearTrailingIdx - 1];
                        }

                        if (inClose[NearTrailingIdx - 1] >= inOpen[NearTrailingIdx - 1])
                        {
                            num12 = inOpen[NearTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[NearTrailingIdx - 1];
                        }

                        num11 = (inHigh[NearTrailingIdx - 1] - num13) + (num12 - inLow[NearTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            NearPeriodTotal += num20 - num15;
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[EqualTrailingIdx - 1] - inOpen[EqualTrailingIdx - 1]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[EqualTrailingIdx - 1] - inLow[EqualTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num3 = inClose[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[EqualTrailingIdx - 1];
                        }

                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num2 = inOpen[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[EqualTrailingIdx - 1];
                        }

                        num = (inHigh[EqualTrailingIdx - 1] - num3) + (num2 - inLow[EqualTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            EqualPeriodTotal += num10 - num5;
            i++;
            NearTrailingIdx++;
            EqualTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0272;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlGapSideSideWhite(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
            float num52;
            float num53;
            float num54;
            float num55;
            float num58;
            float num59;
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

            int lookbackTotal = CdlGapSideSideWhiteLookback();
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

            double NearPeriodTotal = 0.0;
            double EqualPeriodTotal = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            int EqualTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            int i = NearTrailingIdx;
            while (true)
            {
                float num69;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num69 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num68;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num68 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num65;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num66;
                            float num67;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num67 = inClose[i - 1];
                            }
                            else
                            {
                                num67 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num66 = inOpen[i - 1];
                            }
                            else
                            {
                                num66 = inClose[i - 1];
                            }

                            num65 = (inHigh[i - 1] - num67) + (num66 - inLow[i - 1]);
                        }
                        else
                        {
                            num65 = 0.0f;
                        }

                        num68 = num65;
                    }

                    num69 = num68;
                }

                NearPeriodTotal += num69;
                i++;
            }

            i = EqualTrailingIdx;
            while (true)
            {
                float num64;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num64 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num63;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num63 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num60;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            float num61;
                            float num62;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num62 = inClose[i - 1];
                            }
                            else
                            {
                                num62 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num61 = inOpen[i - 1];
                            }
                            else
                            {
                                num61 = inClose[i - 1];
                            }

                            num60 = (inHigh[i - 1] - num62) + (num61 - inLow[i - 1]);
                        }
                        else
                        {
                            num60 = 0.0f;
                        }

                        num63 = num60;
                    }

                    num64 = num63;
                }

                EqualPeriodTotal += num64;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_028E:
            if (inOpen[i - 1] < inClose[i - 1])
            {
                num59 = inOpen[i - 1];
            }
            else
            {
                num59 = inClose[i - 1];
            }

            if (inOpen[i - 2] > inClose[i - 2])
            {
                num58 = inOpen[i - 2];
            }
            else
            {
                num58 = inClose[i - 2];
            }

            if (num59 > num58)
            {
                float num56;
                float num57;
                if (inOpen[i] < inClose[i])
                {
                    num57 = inOpen[i];
                }
                else
                {
                    num57 = inClose[i];
                }

                if (inOpen[i - 2] > inClose[i - 2])
                {
                    num56 = inOpen[i - 2];
                }
                else
                {
                    num56 = inClose[i - 2];
                }

                if (num57 > num56)
                {
                    goto Label_03B7;
                }
            }

            if (inOpen[i - 1] > inClose[i - 1])
            {
                num55 = inOpen[i - 1];
            }
            else
            {
                num55 = inClose[i - 1];
            }

            if (inOpen[i - 2] < inClose[i - 2])
            {
                num54 = inOpen[i - 2];
            }
            else
            {
                num54 = inClose[i - 2];
            }

            if (num55 >= num54)
            {
                goto Label_0A2E;
            }

            if (inOpen[i] > inClose[i])
            {
                num53 = inOpen[i];
            }
            else
            {
                num53 = inClose[i];
            }

            if (inOpen[i - 2] < inClose[i - 2])
            {
                num52 = inOpen[i - 2];
            }
            else
            {
                num52 = inClose[i - 2];
            }

            if (num53 >= num52)
            {
                goto Label_0A2E;
            }

            Label_03B7:
            if ((inClose[i - 1] >= inOpen[i - 1]) && (inClose[i] >= inOpen[i]))
            {
                double num45;
                double num51;
                if (Globals.candleSettings[8].avgPeriod != 0.0)
                {
                    num51 = NearPeriodTotal / ((double) Globals.candleSettings[8].avgPeriod);
                }
                else
                {
                    float num50;
                    if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                    {
                        num50 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        float num49;
                        if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                        {
                            num49 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            float num46;
                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                float num47;
                                float num48;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num48 = inClose[i - 1];
                                }
                                else
                                {
                                    num48 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num47 = inOpen[i - 1];
                                }
                                else
                                {
                                    num47 = inClose[i - 1];
                                }

                                num46 = (inHigh[i - 1] - num48) + (num47 - inLow[i - 1]);
                            }
                            else
                            {
                                num46 = 0.0f;
                            }

                            num49 = num46;
                        }

                        num50 = num49;
                    }

                    num51 = num50;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                {
                    num45 = 2.0;
                }
                else
                {
                    num45 = 1.0;
                }

                if (Math.Abs((float) (inClose[i] - inOpen[i])) >=
                    (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) - ((Globals.candleSettings[8].factor * num51) / num45)))
                {
                    double num38;
                    double num44;
                    if (Globals.candleSettings[8].avgPeriod != 0.0)
                    {
                        num44 = NearPeriodTotal / ((double) Globals.candleSettings[8].avgPeriod);
                    }
                    else
                    {
                        float num43;
                        if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                        {
                            num43 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            float num42;
                            if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                            {
                                num42 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                float num39;
                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                {
                                    float num40;
                                    float num41;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num41 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num41 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num40 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num40 = inClose[i - 1];
                                    }

                                    num39 = (inHigh[i - 1] - num41) + (num40 - inLow[i - 1]);
                                }
                                else
                                {
                                    num39 = 0.0f;
                                }

                                num42 = num39;
                            }

                            num43 = num42;
                        }

                        num44 = num43;
                    }

                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                    {
                        num38 = 2.0;
                    }
                    else
                    {
                        num38 = 1.0;
                    }

                    if (Math.Abs((float) (inClose[i] - inOpen[i])) <=
                        (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) + ((Globals.candleSettings[8].factor * num44) / num38)))
                    {
                        double num31;
                        double num37;
                        if (Globals.candleSettings[10].avgPeriod != 0.0)
                        {
                            num37 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                        }
                        else
                        {
                            float num36;
                            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                            {
                                num36 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                float num35;
                                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                {
                                    num35 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    float num32;
                                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                    {
                                        float num33;
                                        float num34;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num34 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num34 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num33 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num33 = inClose[i - 1];
                                        }

                                        num32 = (inHigh[i - 1] - num34) + (num33 - inLow[i - 1]);
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

                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            num31 = 2.0;
                        }
                        else
                        {
                            num31 = 1.0;
                        }

                        if (inOpen[i] >= (inOpen[i - 1] - ((Globals.candleSettings[10].factor * num37) / num31)))
                        {
                            double num24;
                            double num30;
                            if (Globals.candleSettings[10].avgPeriod != 0.0)
                            {
                                num30 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                            }
                            else
                            {
                                float num29;
                                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                {
                                    num29 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                                }
                                else
                                {
                                    float num28;
                                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                    {
                                        num28 = inHigh[i - 1] - inLow[i - 1];
                                    }
                                    else
                                    {
                                        float num25;
                                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                        {
                                            float num26;
                                            float num27;
                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num27 = inClose[i - 1];
                                            }
                                            else
                                            {
                                                num27 = inOpen[i - 1];
                                            }

                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num26 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num26 = inClose[i - 1];
                                            }

                                            num25 = (inHigh[i - 1] - num27) + (num26 - inLow[i - 1]);
                                        }
                                        else
                                        {
                                            num25 = 0.0f;
                                        }

                                        num28 = num25;
                                    }

                                    num29 = num28;
                                }

                                num30 = num29;
                            }

                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                num24 = 2.0;
                            }
                            else
                            {
                                num24 = 1.0;
                            }

                            if (inOpen[i] <= (inOpen[i - 1] + ((Globals.candleSettings[10].factor * num30) / num24)))
                            {
                                int num21;
                                float num22;
                                float num23;
                                if (inOpen[i - 1] < inClose[i - 1])
                                {
                                    num23 = inOpen[i - 1];
                                }
                                else
                                {
                                    num23 = inClose[i - 1];
                                }

                                if (inOpen[i - 2] > inClose[i - 2])
                                {
                                    num22 = inOpen[i - 2];
                                }
                                else
                                {
                                    num22 = inClose[i - 2];
                                }

                                if (num23 > num22)
                                {
                                    num21 = 100;
                                }
                                else
                                {
                                    num21 = -100;
                                }

                                outInteger[outIdx] = num21;
                                outIdx++;
                                goto Label_0A37;
                            }
                        }
                    }
                }
            }

            Label_0A2E:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0A37:
            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((float) (inClose[NearTrailingIdx - 1] - inOpen[NearTrailingIdx - 1]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[NearTrailingIdx - 1] - inLow[NearTrailingIdx - 1];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[NearTrailingIdx - 1] >= inOpen[NearTrailingIdx - 1])
                        {
                            num13 = inClose[NearTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[NearTrailingIdx - 1];
                        }

                        if (inClose[NearTrailingIdx - 1] >= inOpen[NearTrailingIdx - 1])
                        {
                            num12 = inOpen[NearTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[NearTrailingIdx - 1];
                        }

                        num11 = (inHigh[NearTrailingIdx - 1] - num13) + (num12 - inLow[NearTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            NearPeriodTotal += num20 - num15;
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[EqualTrailingIdx - 1] - inOpen[EqualTrailingIdx - 1]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[EqualTrailingIdx - 1] - inLow[EqualTrailingIdx - 1];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num3 = inClose[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[EqualTrailingIdx - 1];
                        }

                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num2 = inOpen[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[EqualTrailingIdx - 1];
                        }

                        num = (inHigh[EqualTrailingIdx - 1] - num3) + (num2 - inLow[EqualTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            EqualPeriodTotal += num10 - num5;
            i++;
            NearTrailingIdx++;
            EqualTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_028E;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlGapSideSideWhiteLookback()
        {
            return (((Globals.candleSettings[8].avgPeriod <= Globals.candleSettings[10].avgPeriod)
                        ? Globals.candleSettings[10].avgPeriod
                        : Globals.candleSettings[8].avgPeriod) + 2);
        }
    }
}
