using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlHikkakeMod(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int outIdx;
            double num5;
            double num10;
            double num18;
            double num30;
            double num35;
            double num42;
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

            int lookbackTotal = CdlHikkakeModLookback();
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
            int nearTrailingIdx = startIdx - 3 - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int i = nearTrailingIdx;
            while (true)
            {
                double num54;
                if (i >= startIdx - 3)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num53;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num50;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num51;
                            double num52;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num52 = inClose[i - 2];
                            }
                            else
                            {
                                num52 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num51 = inOpen[i - 2];
                            }
                            else
                            {
                                num51 = inClose[i - 2];
                            }

                            num50 = inHigh[i - 2] - num52 + (num51 - inLow[i - 2]);
                        }
                        else
                        {
                            num50 = 0.0;
                        }

                        num53 = num50;
                    }

                    num54 = num53;
                }

                nearPeriodTotal += num54;
                i++;
            }

            int patternIdx = default;
            int patternResult = default;
            i = startIdx - 3;
            Label_0174:
            if (i >= startIdx)
            {
                i = startIdx;
                outIdx = 0;
                goto Label_069A;
            }

            if (inHigh[i - 2] >= inHigh[i - 3] || inLow[i - 2] <= inLow[i - 3] ||
                inHigh[i - 1] >= inHigh[i - 2] || inLow[i - 1] <= inLow[i - 2])
            {
                goto Label_04C0;
            }

            if (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1])
            {
                double num49;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                {
                    num49 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                }
                else
                {
                    double num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                    {
                        num48 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num47;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                        {
                            num47 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num44;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                            {
                                double num45;
                                double num46;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num46 = inClose[i - 2];
                                }
                                else
                                {
                                    num46 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num45 = inOpen[i - 2];
                                }
                                else
                                {
                                    num45 = inClose[i - 2];
                                }

                                num44 = inHigh[i - 2] - num46 + (num45 - inLow[i - 2]);
                            }
                            else
                            {
                                num44 = 0.0;
                            }

                            num47 = num44;
                        }

                        num48 = num47;
                    }

                    num49 = num48;
                }

                var num43 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (inClose[i - 2] <= inLow[i - 2] + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num49 / num43)
                {
                    goto Label_04A9;
                }
            }

            if (inHigh[i] <= inHigh[i - 1] || inLow[i] <= inLow[i - 1])
            {
                goto Label_04C0;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
            {
                num42 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            }
            else
            {
                double num41;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num41 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num40;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num40 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num38;
                            double num39;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num39 = inClose[i - 2];
                            }
                            else
                            {
                                num39 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num38 = inOpen[i - 2];
                            }
                            else
                            {
                                num38 = inClose[i - 2];
                            }

                            num37 = inHigh[i - 2] - num39 + (num38 - inLow[i - 2]);
                        }
                        else
                        {
                            num37 = 0.0;
                        }

                        num40 = num37;
                    }

                    num41 = num40;
                }

                num42 = num41;
            }

            var num36 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inClose[i - 2] < inHigh[i - 2] - Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num42 / num36)
            {
                goto Label_04C0;
            }

            Label_04A9:
            patternResult = (inHigh[i] >= inHigh[i - 1] ? -1 : 1) * 100;
            patternIdx = i;
            goto Label_04E9;
            Label_04C0:
            if (i <= patternIdx + 3 && (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] ||
                                        patternResult < 0 && inClose[i] < inLow[patternIdx - 1]))
            {
                patternIdx = 0;
            }

            Label_04E9:
            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num35 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                double num34;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num34 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        double num32;
                        double num33;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num33 = inClose[i - 2];
                        }
                        else
                        {
                            num33 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num32 = inOpen[i - 2];
                        }
                        else
                        {
                            num32 = inClose[i - 2];
                        }

                        num31 = inHigh[i - 2] - num33 + (num32 - inLow[i - 2]);
                    }
                    else
                    {
                        num31 = 0.0;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[nearTrailingIdx - 2] - inOpen[nearTrailingIdx - 2]);
            }
            else
            {
                double num29;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[nearTrailingIdx - 2] - inLow[nearTrailingIdx - 2];
                }
                else
                {
                    double num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        double num27;
                        double num28;
                        if (inClose[nearTrailingIdx - 2] >= inOpen[nearTrailingIdx - 2])
                        {
                            num28 = inClose[nearTrailingIdx - 2];
                        }
                        else
                        {
                            num28 = inOpen[nearTrailingIdx - 2];
                        }

                        if (inClose[nearTrailingIdx - 2] >= inOpen[nearTrailingIdx - 2])
                        {
                            num27 = inOpen[nearTrailingIdx - 2];
                        }
                        else
                        {
                            num27 = inClose[nearTrailingIdx - 2];
                        }

                        num26 = inHigh[nearTrailingIdx - 2] - num28 + (num27 - inLow[nearTrailingIdx - 2]);
                    }
                    else
                    {
                        num26 = 0.0;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            nearPeriodTotal += num35 - num30;
            nearTrailingIdx++;
            i++;
            goto Label_0174;
            Label_069A:
            if (inHigh[i - 2] >= inHigh[i - 3] || inLow[i - 2] <= inLow[i - 3] ||
                inHigh[i - 1] >= inHigh[i - 2] || inLow[i - 1] <= inLow[i - 2])
            {
                goto Label_09E9;
            }

            if (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1])
            {
                double num25;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                {
                    num25 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                }
                else
                {
                    double num24;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                    {
                        num24 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num23;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                        {
                            num23 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num20;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                            {
                                double num21;
                                double num22;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num22 = inClose[i - 2];
                                }
                                else
                                {
                                    num22 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num21 = inOpen[i - 2];
                                }
                                else
                                {
                                    num21 = inClose[i - 2];
                                }

                                num20 = inHigh[i - 2] - num22 + (num21 - inLow[i - 2]);
                            }
                            else
                            {
                                num20 = 0.0;
                            }

                            num23 = num20;
                        }

                        num24 = num23;
                    }

                    num25 = num24;
                }

                var num19 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (inClose[i - 2] <= inLow[i - 2] + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num25 / num19)
                {
                    goto Label_09C8;
                }
            }

            if (inHigh[i] <= inHigh[i - 1] || inLow[i] <= inLow[i - 1])
            {
                goto Label_09E9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
            {
                num18 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            }
            else
            {
                double num17;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num17 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num16 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num13;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num14;
                            double num15;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num15 = inClose[i - 2];
                            }
                            else
                            {
                                num15 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num14 = inOpen[i - 2];
                            }
                            else
                            {
                                num14 = inClose[i - 2];
                            }

                            num13 = inHigh[i - 2] - num15 + (num14 - inLow[i - 2]);
                        }
                        else
                        {
                            num13 = 0.0;
                        }

                        num16 = num13;
                    }

                    num17 = num16;
                }

                num18 = num17;
            }

            var num12 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inClose[i - 2] < inHigh[i - 2] - Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num18 / num12)
            {
                goto Label_09E9;
            }

            Label_09C8:
            patternResult = (inHigh[i] >= inHigh[i - 1] ? -1 : 1) * 100;
            patternIdx = i;
            outInteger[outIdx] = patternResult;
            outIdx++;
            goto Label_0A3A;
            Label_09E9:
            if (i <= patternIdx + 3 && (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] ||
                                        patternResult < 0 && inClose[i] < inLow[patternIdx - 1]))
            {
                int num11;
                if (patternResult > 0)
                {
                    num11 = 1;
                }
                else
                {
                    num11 = -1;
                }

                outInteger[outIdx] = patternResult + num11 * 100;
                outIdx++;
                patternIdx = 0;
            }
            else
            {
                outInteger[outIdx] = 0;
                outIdx++;
            }

            Label_0A3A:
            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num8 = inClose[i - 2];
                        }
                        else
                        {
                            num8 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num7 = inOpen[i - 2];
                        }
                        else
                        {
                            num7 = inClose[i - 2];
                        }

                        num6 = inHigh[i - 2] - num8 + (num7 - inLow[i - 2]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[nearTrailingIdx - 2] - inOpen[nearTrailingIdx - 2]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[nearTrailingIdx - 2] - inLow[nearTrailingIdx - 2];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[nearTrailingIdx - 2] >= inOpen[nearTrailingIdx - 2])
                        {
                            num3 = inClose[nearTrailingIdx - 2];
                        }
                        else
                        {
                            num3 = inOpen[nearTrailingIdx - 2];
                        }

                        if (inClose[nearTrailingIdx - 2] >= inOpen[nearTrailingIdx - 2])
                        {
                            num2 = inOpen[nearTrailingIdx - 2];
                        }
                        else
                        {
                            num2 = inClose[nearTrailingIdx - 2];
                        }

                        num = inHigh[nearTrailingIdx - 2] - num3 + (num2 - inLow[nearTrailingIdx - 2]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            nearPeriodTotal += num10 - num5;
            nearTrailingIdx++;
            i++;
            if (i <= endIdx)
            {
                goto Label_069A;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlHikkakeMod(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int outIdx;
            decimal num5;
            decimal num10;
            decimal num18;
            decimal num30;
            decimal num35;
            decimal num42;
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

            int lookbackTotal = CdlHikkakeModLookback();
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
            int nearTrailingIdx = startIdx - 3 - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int i = nearTrailingIdx;
            while (true)
            {
                decimal num54;
                if (i >= startIdx - 3)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num53;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num50;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num51;
                            decimal num52;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num52 = inClose[i - 2];
                            }
                            else
                            {
                                num52 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num51 = inOpen[i - 2];
                            }
                            else
                            {
                                num51 = inClose[i - 2];
                            }

                            num50 = inHigh[i - 2] - num52 + (num51 - inLow[i - 2]);
                        }
                        else
                        {
                            num50 = Decimal.Zero;
                        }

                        num53 = num50;
                    }

                    num54 = num53;
                }

                nearPeriodTotal += num54;
                i++;
            }

            int patternIdx = default;
            int patternResult = default;
            i = startIdx - 3;
            Label_0182:
            if (i >= startIdx)
            {
                i = startIdx;
                outIdx = 0;
                goto Label_06FA;
            }

            if (inHigh[i - 2] >= inHigh[i - 3] || inLow[i - 2] <= inLow[i - 3] ||
                inHigh[i - 1] >= inHigh[i - 2] || inLow[i - 1] <= inLow[i - 2])
            {
                goto Label_0500;
            }

            if (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1])
            {
                decimal num49;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                {
                    num49 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                }
                else
                {
                    decimal num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                    {
                        num48 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num47;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                        {
                            num47 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num44;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                            {
                                decimal num45;
                                decimal num46;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num46 = inClose[i - 2];
                                }
                                else
                                {
                                    num46 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num45 = inOpen[i - 2];
                                }
                                else
                                {
                                    num45 = inClose[i - 2];
                                }

                                num44 = inHigh[i - 2] - num46 + (num45 - inLow[i - 2]);
                            }
                            else
                            {
                                num44 = Decimal.Zero;
                            }

                            num47 = num44;
                        }

                        num48 = num47;
                    }

                    num49 = num48;
                }

                var num43 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                if (inClose[i - 2] <= inLow[i - 2] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num49 / num43)
                {
                    goto Label_04E7;
                }
            }

            if (inHigh[i] <= inHigh[i - 1] || inLow[i] <= inLow[i - 1])
            {
                goto Label_0500;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
            {
                num42 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            }
            else
            {
                decimal num41;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num41 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num40;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num40 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num38;
                            decimal num39;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num39 = inClose[i - 2];
                            }
                            else
                            {
                                num39 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num38 = inOpen[i - 2];
                            }
                            else
                            {
                                num38 = inClose[i - 2];
                            }

                            num37 = inHigh[i - 2] - num39 + (num38 - inLow[i - 2]);
                        }
                        else
                        {
                            num37 = Decimal.Zero;
                        }

                        num40 = num37;
                    }

                    num41 = num40;
                }

                num42 = num41;
            }

            var num36 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inClose[i - 2] < inHigh[i - 2] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num42 / num36)
            {
                goto Label_0500;
            }

            Label_04E7:
            patternResult = (inHigh[i] >= inHigh[i - 1] ? -1 : 1) * 100;
            patternIdx = i;
            goto Label_052D;
            Label_0500:
            if (i <= patternIdx + 3 && (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] ||
                                        patternResult < 0 && inClose[i] < inLow[patternIdx - 1]))
            {
                patternIdx = 0;
            }

            Label_052D:
            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num35 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                decimal num34;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num34 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    decimal num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        decimal num32;
                        decimal num33;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num33 = inClose[i - 2];
                        }
                        else
                        {
                            num33 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num32 = inOpen[i - 2];
                        }
                        else
                        {
                            num32 = inClose[i - 2];
                        }

                        num31 = inHigh[i - 2] - num33 + (num32 - inLow[i - 2]);
                    }
                    else
                    {
                        num31 = Decimal.Zero;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[nearTrailingIdx - 2] - inOpen[nearTrailingIdx - 2]);
            }
            else
            {
                decimal num29;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[nearTrailingIdx - 2] - inLow[nearTrailingIdx - 2];
                }
                else
                {
                    decimal num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        decimal num27;
                        decimal num28;
                        if (inClose[nearTrailingIdx - 2] >= inOpen[nearTrailingIdx - 2])
                        {
                            num28 = inClose[nearTrailingIdx - 2];
                        }
                        else
                        {
                            num28 = inOpen[nearTrailingIdx - 2];
                        }

                        if (inClose[nearTrailingIdx - 2] >= inOpen[nearTrailingIdx - 2])
                        {
                            num27 = inOpen[nearTrailingIdx - 2];
                        }
                        else
                        {
                            num27 = inClose[nearTrailingIdx - 2];
                        }

                        num26 = inHigh[nearTrailingIdx - 2] - num28 + (num27 - inLow[nearTrailingIdx - 2]);
                    }
                    else
                    {
                        num26 = Decimal.Zero;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            nearPeriodTotal += num35 - num30;
            nearTrailingIdx++;
            i++;
            goto Label_0182;
            Label_06FA:
            if (inHigh[i - 2] >= inHigh[i - 3] || inLow[i - 2] <= inLow[i - 3] ||
                inHigh[i - 1] >= inHigh[i - 2] || inLow[i - 1] <= inLow[i - 2])
            {
                goto Label_0A7B;
            }

            if (inHigh[i] < inHigh[i - 1] && inLow[i] < inLow[i - 1])
            {
                decimal num25;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                {
                    num25 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                }
                else
                {
                    decimal num24;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                    {
                        num24 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num23;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                        {
                            num23 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num20;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                            {
                                decimal num21;
                                decimal num22;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num22 = inClose[i - 2];
                                }
                                else
                                {
                                    num22 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num21 = inOpen[i - 2];
                                }
                                else
                                {
                                    num21 = inClose[i - 2];
                                }

                                num20 = inHigh[i - 2] - num22 + (num21 - inLow[i - 2]);
                            }
                            else
                            {
                                num20 = Decimal.Zero;
                            }

                            num23 = num20;
                        }

                        num24 = num23;
                    }

                    num25 = num24;
                }

                var num19 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                if (inClose[i - 2] <= inLow[i - 2] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num25 / num19)
                {
                    goto Label_0A58;
                }
            }

            if (inHigh[i] <= inHigh[i - 1] || inLow[i] <= inLow[i - 1])
            {
                goto Label_0A7B;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
            {
                num18 = nearPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            }
            else
            {
                decimal num17;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num17 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num16 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num13;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num14;
                            decimal num15;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num15 = inClose[i - 2];
                            }
                            else
                            {
                                num15 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num14 = inOpen[i - 2];
                            }
                            else
                            {
                                num14 = inClose[i - 2];
                            }

                            num13 = inHigh[i - 2] - num15 + (num14 - inLow[i - 2]);
                        }
                        else
                        {
                            num13 = Decimal.Zero;
                        }

                        num16 = num13;
                    }

                    num17 = num16;
                }

                num18 = num17;
            }

            var num12 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

            if (inClose[i - 2] < inHigh[i - 2] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num18 / num12)
            {
                goto Label_0A7B;
            }

            Label_0A58:
            patternResult = (inHigh[i] >= inHigh[i - 1] ? -1 : 1) * 100;
            patternIdx = i;
            outInteger[outIdx] = patternResult;
            outIdx++;
            goto Label_0AD0;
            Label_0A7B:
            if (i <= patternIdx + 3 && (patternResult > 0 && inClose[i] > inHigh[patternIdx - 1] ||
                                        patternResult < 0 && inClose[i] < inLow[patternIdx - 1]))
            {
                int num11;
                if (patternResult > 0)
                {
                    num11 = 1;
                }
                else
                {
                    num11 = -1;
                }

                outInteger[outIdx] = patternResult + num11 * 100;
                outIdx++;
                patternIdx = 0;
            }
            else
            {
                outInteger[outIdx] = 0;
                outIdx++;
            }

            Label_0AD0:
            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num8 = inClose[i - 2];
                        }
                        else
                        {
                            num8 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num7 = inOpen[i - 2];
                        }
                        else
                        {
                            num7 = inClose[i - 2];
                        }

                        num6 = inHigh[i - 2] - num8 + (num7 - inLow[i - 2]);
                    }
                    else
                    {
                        num6 = Decimal.Zero;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[nearTrailingIdx - 2] - inOpen[nearTrailingIdx - 2]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[nearTrailingIdx - 2] - inLow[nearTrailingIdx - 2];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[nearTrailingIdx - 2] >= inOpen[nearTrailingIdx - 2])
                        {
                            num3 = inClose[nearTrailingIdx - 2];
                        }
                        else
                        {
                            num3 = inOpen[nearTrailingIdx - 2];
                        }

                        if (inClose[nearTrailingIdx - 2] >= inOpen[nearTrailingIdx - 2])
                        {
                            num2 = inOpen[nearTrailingIdx - 2];
                        }
                        else
                        {
                            num2 = inClose[nearTrailingIdx - 2];
                        }

                        num = inHigh[nearTrailingIdx - 2] - num3 + (num2 - inLow[nearTrailingIdx - 2]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            nearPeriodTotal += num10 - num5;
            nearTrailingIdx++;
            i++;
            if (i <= endIdx)
            {
                goto Label_06FA;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlHikkakeModLookback()
        {
            int avgPeriod = Math.Max(1, Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod);

            return avgPeriod + 5;
        }
    }
}
