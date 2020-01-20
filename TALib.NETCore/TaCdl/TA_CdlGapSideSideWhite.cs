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

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
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

            double nearPeriodTotal = default;
            double equalPeriodTotal = default;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int i = nearTrailingIdx;
            while (true)
            {
                double num69;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num69 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num68;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num68 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num65;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                            num65 = inHigh[i - 1] - num67 + (num66 - inLow[i - 1]);
                        }
                        else
                        {
                            num65 = 0.0;
                        }

                        num68 = num65;
                    }

                    num69 = num68;
                }

                nearPeriodTotal += num69;
                i++;
            }

            i = equalTrailingIdx;
            while (true)
            {
                double num64;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num64 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num63;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num63 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num60;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

                            num60 = inHigh[i - 1] - num62 + (num61 - inLow[i - 1]);
                        }
                        else
                        {
                            num60 = 0.0;
                        }

                        num63 = num60;
                    }

                    num64 = num63;
                }

                equalPeriodTotal += num64;
                i++;
            }

            i = startIdx;
            int outIdx = default;
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
            if (inClose[i - 1] >= inOpen[i - 1] && inClose[i] >= inOpen[i])
            {
                double num51;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                {
                    num51 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                }
                else
                {
                    double num50;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                    {
                        num50 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num49;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                        {
                            num49 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num46;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                                num46 = inHigh[i - 1] - num48 + (num47 - inLow[i - 1]);
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

                var num45 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i] - inOpen[i]) >=
                    Math.Abs(inClose[i - 1] - inOpen[i - 1]) - Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num51 / num45)
                {
                    double num44;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                    {
                        num44 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                    }
                    else
                    {
                        double num43;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                        {
                            num43 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num42;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                            {
                                num42 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num39;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                                    num39 = inHigh[i - 1] - num41 + (num40 - inLow[i - 1]);
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

                    var num38 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i] - inOpen[i]) <=
                        Math.Abs(inClose[i - 1] - inOpen[i - 1]) +
                        Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num44 / num38)
                    {
                        double num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                        {
                            num37 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                        }
                        else
                        {
                            double num36;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                            {
                                num36 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                double num35;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                {
                                    num35 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num32;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

                                        num32 = inHigh[i - 1] - num34 + (num33 - inLow[i - 1]);
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

                        var num31 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                        if (inOpen[i] >= inOpen[i - 1] - Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num37 / num31)
                        {
                            double num30;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                            {
                                num30 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                            }
                            else
                            {
                                double num29;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                {
                                    num29 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                }
                                else
                                {
                                    double num28;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                    {
                                        num28 = inHigh[i - 1] - inLow[i - 1];
                                    }
                                    else
                                    {
                                        double num25;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

                                            num25 = inHigh[i - 1] - num27 + (num26 - inLow[i - 1]);
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

                            var num24 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                            if (inOpen[i] <= inOpen[i - 1] + Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num30 / num24)
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
            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                        num16 = inHigh[i - 1] - num18 + (num17 - inLow[i - 1]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[nearTrailingIdx - 1] - inOpen[nearTrailingIdx - 1]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[nearTrailingIdx - 1] - inLow[nearTrailingIdx - 1];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[nearTrailingIdx - 1] >= inOpen[nearTrailingIdx - 1])
                        {
                            num13 = inClose[nearTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[nearTrailingIdx - 1];
                        }

                        if (inClose[nearTrailingIdx - 1] >= inOpen[nearTrailingIdx - 1])
                        {
                            num12 = inOpen[nearTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[nearTrailingIdx - 1];
                        }

                        num11 = inHigh[nearTrailingIdx - 1] - num13 + (num12 - inLow[nearTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            nearPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

                        num6 = inHigh[i - 1] - num8 + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num3 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num2 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[equalTrailingIdx - 1];
                        }

                        num = inHigh[equalTrailingIdx - 1] - num3 + (num2 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            equalPeriodTotal += num10 - num5;
            i++;
            nearTrailingIdx++;
            equalTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0272;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlGapSideSideWhite(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num52;
            decimal num53;
            decimal num54;
            decimal num55;
            decimal num58;
            decimal num59;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
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

            decimal nearPeriodTotal = default;
            decimal equalPeriodTotal = default;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int i = nearTrailingIdx;
            while (true)
            {
                decimal num69;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num69 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num68;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num68 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num65;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num66;
                            decimal num67;
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

                            num65 = inHigh[i - 1] - num67 + (num66 - inLow[i - 1]);
                        }
                        else
                        {
                            num65 = Decimal.Zero;
                        }

                        num68 = num65;
                    }

                    num69 = num68;
                }

                nearPeriodTotal += num69;
                i++;
            }

            i = equalTrailingIdx;
            while (true)
            {
                decimal num64;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num64 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num63;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num63 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num60;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num61;
                            decimal num62;
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

                            num60 = inHigh[i - 1] - num62 + (num61 - inLow[i - 1]);
                        }
                        else
                        {
                            num60 = Decimal.Zero;
                        }

                        num63 = num60;
                    }

                    num64 = num63;
                }

                equalPeriodTotal += num64;
                i++;
            }

            i = startIdx;
            int outIdx = default;
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
                decimal num56;
                decimal num57;
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
            if (inClose[i - 1] >= inOpen[i - 1] && inClose[i] >= inOpen[i])
            {
                decimal num51;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                {
                    num51 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                }
                else
                {
                    decimal num50;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                    {
                        num50 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num49;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                        {
                            num49 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num46;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                            {
                                decimal num47;
                                decimal num48;
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

                                num46 = inHigh[i - 1] - num48 + (num47 - inLow[i - 1]);
                            }
                            else
                            {
                                num46 = Decimal.Zero;
                            }

                            num49 = num46;
                        }

                        num50 = num49;
                    }

                    num51 = num50;
                }

                var num45 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i] - inOpen[i]) >=
                    Math.Abs(inClose[i - 1] - inOpen[i - 1]) -
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num51 / num45)
                {
                    decimal num44;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                    {
                        num44 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                    }
                    else
                    {
                        decimal num43;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                        {
                            num43 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num42;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                            {
                                num42 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num39;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                {
                                    decimal num40;
                                    decimal num41;
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

                                    num39 = inHigh[i - 1] - num41 + (num40 - inLow[i - 1]);
                                }
                                else
                                {
                                    num39 = Decimal.Zero;
                                }

                                num42 = num39;
                            }

                            num43 = num42;
                        }

                        num44 = num43;
                    }

                    var num38 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i] - inOpen[i]) <=
                        Math.Abs(inClose[i - 1] - inOpen[i - 1]) +
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num44 / num38)
                    {
                        decimal num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                        {
                            num37 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                        }
                        else
                        {
                            decimal num36;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                            {
                                num36 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                decimal num35;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                {
                                    num35 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    decimal num32;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                    {
                                        decimal num33;
                                        decimal num34;
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

                                        num32 = inHigh[i - 1] - num34 + (num33 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num32 = Decimal.Zero;
                                    }

                                    num35 = num32;
                                }

                                num36 = num35;
                            }

                            num37 = num36;
                        }

                        var num31 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

                        if (inOpen[i] >=
                            inOpen[i - 1] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num37 / num31)
                        {
                            decimal num30;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                            {
                                num30 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                            }
                            else
                            {
                                decimal num29;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                {
                                    num29 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                }
                                else
                                {
                                    decimal num28;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                    {
                                        num28 = inHigh[i - 1] - inLow[i - 1];
                                    }
                                    else
                                    {
                                        decimal num25;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                        {
                                            decimal num26;
                                            decimal num27;
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

                                            num25 = inHigh[i - 1] - num27 + (num26 - inLow[i - 1]);
                                        }
                                        else
                                        {
                                            num25 = Decimal.Zero;
                                        }

                                        num28 = num25;
                                    }

                                    num29 = num28;
                                }

                                num30 = num29;
                            }

                            var num24 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (inOpen[i] <=
                                inOpen[i - 1] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num30 / num24)
                            {
                                int num21;
                                decimal num22;
                                decimal num23;
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
            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        decimal num17;
                        decimal num18;
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

                        num16 = inHigh[i - 1] - num18 + (num17 - inLow[i - 1]);
                    }
                    else
                    {
                        num16 = Decimal.Zero;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[nearTrailingIdx - 1] - inOpen[nearTrailingIdx - 1]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[nearTrailingIdx - 1] - inLow[nearTrailingIdx - 1];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[nearTrailingIdx - 1] >= inOpen[nearTrailingIdx - 1])
                        {
                            num13 = inClose[nearTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[nearTrailingIdx - 1];
                        }

                        if (inClose[nearTrailingIdx - 1] >= inOpen[nearTrailingIdx - 1])
                        {
                            num12 = inOpen[nearTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[nearTrailingIdx - 1];
                        }

                        num11 = inHigh[nearTrailingIdx - 1] - num13 + (num12 - inLow[nearTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            nearPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
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

                        num6 = inHigh[i - 1] - num8 + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = Decimal.Zero;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num3 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num2 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[equalTrailingIdx - 1];
                        }

                        num = inHigh[equalTrailingIdx - 1] - num3 + (num2 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            equalPeriodTotal += num10 - num5;
            i++;
            nearTrailingIdx++;
            equalTrailingIdx++;
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
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod);

            return avgPeriod + 2;
        }
    }
}
