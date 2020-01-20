using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3BlackCrows(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            var shadowVeryShortPeriodTotal = new double[3];
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

            int lookbackTotal = Cdl3BlackCrowsLookback();
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

            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num39;
                double num44;
                double num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num46;
                            double num47;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num47 = inClose[i - 2];
                            }
                            else
                            {
                                num47 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num46 = inOpen[i - 2];
                            }
                            else
                            {
                                num46 = inClose[i - 2];
                            }

                            num45 = inHigh[i - 2] - num47 + (num46 - inLow[i - 2]);
                        }
                        else
                        {
                            num45 = 0.0;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                shadowVeryShortPeriodTotal[2] += num49;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num43;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num40;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                            num40 = inHigh[i - 1] - num42 + (num41 - inLow[i - 1]);
                        }
                        else
                        {
                            num40 = 0.0;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                shadowVeryShortPeriodTotal[1] += num44;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num38;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num35;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num36;
                            double num37;
                            if (inClose[i] >= inOpen[i])
                            {
                                num37 = inClose[i];
                            }
                            else
                            {
                                num37 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num36 = inOpen[i];
                            }
                            else
                            {
                                num36 = inClose[i];
                            }

                            num35 = inHigh[i] - num37 + (num36 - inLow[i]);
                        }
                        else
                        {
                            num35 = 0.0;
                        }

                        num38 = num35;
                    }

                    num39 = num38;
                }

                shadowVeryShortPeriodTotal[0] += num39;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0317:
            if (inClose[i - 3] >= inOpen[i - 3] && inClose[i - 2] < inOpen[i - 2])
            {
                double num33;
                double num34;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num34 = inOpen[i - 2];
                }
                else
                {
                    num34 = inClose[i - 2];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num33 = shadowVeryShortPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    double num32;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num32 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num31;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num31 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num28;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                double num29;
                                double num30;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num30 = inClose[i - 2];
                                }
                                else
                                {
                                    num30 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num29 = inOpen[i - 2];
                                }
                                else
                                {
                                    num29 = inClose[i - 2];
                                }

                                num28 = inHigh[i - 2] - num30 + (num29 - inLow[i - 2]);
                            }
                            else
                            {
                                num28 = 0.0;
                            }

                            num31 = num28;
                        }

                        num32 = num31;
                    }

                    num33 = num32;
                }

                var num27 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (num34 - inLow[i - 2] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num33 / num27 &&
                    inClose[i - 1] < inOpen[i - 1])
                {
                    double num25;
                    double num26;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num26 = inOpen[i - 1];
                    }
                    else
                    {
                        num26 = inClose[i - 1];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num25 = shadowVeryShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        double num24;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num24 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num23;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num23 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num20;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    double num21;
                                    double num22;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num22 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num22 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num21 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num21 = inClose[i - 1];
                                    }

                                    num20 = inHigh[i - 1] - num22 + (num21 - inLow[i - 1]);
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

                    var num19 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (num26 - inLow[i - 1] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num25 / num19 &&
                        inClose[i] < inOpen[i])
                    {
                        double num17;
                        double num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inOpen[i];
                        }
                        else
                        {
                            num18 = inClose[i];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num17 = shadowVeryShortPeriodTotal[0] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            double num16;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num16 = Math.Abs(inClose[i] - inOpen[i]);
                            }
                            else
                            {
                                double num15;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num15 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    double num12;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                    {
                                        double num13;
                                        double num14;
                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num14 = inClose[i];
                                        }
                                        else
                                        {
                                            num14 = inOpen[i];
                                        }

                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num13 = inOpen[i];
                                        }
                                        else
                                        {
                                            num13 = inClose[i];
                                        }

                                        num12 = inHigh[i] - num14 + (num13 - inLow[i]);
                                    }
                                    else
                                    {
                                        num12 = 0.0;
                                    }

                                    num15 = num12;
                                }

                                num16 = num15;
                            }

                            num17 = num16;
                        }

                        var num11 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2.0
                            : 1.0;

                        if (num18 - inLow[i] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num17 / num11 &&
                            inOpen[i - 1] < inOpen[i - 2] && inOpen[i - 1] > inClose[i - 2] &&
                            inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] && inHigh[i - 3] > inClose[i - 2] &&
                            inClose[i - 2] > inClose[i - 1] && inClose[i - 1] > inClose[i])
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_081B;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_081B:
            int totIdx = 2;
            while (totIdx >= 0)
            {
                double num5;
                double num10;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                            num6 = inHigh[i - totIdx] - num8 + (num7 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num6 = 0.0;
                        }

                        num9 = num6;
                    }

                    num10 = num9;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx - totIdx] - inOpen[shadowVeryShortTrailingIdx - totIdx]);
                }
                else
                {
                    double num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[shadowVeryShortTrailingIdx - totIdx] - inLow[shadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num3 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num2 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }

                            num = inHigh[shadowVeryShortTrailingIdx - totIdx] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                shadowVeryShortPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            shadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0317;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3BlackCrows(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            var shadowVeryShortPeriodTotal = new decimal[3];
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

            int lookbackTotal = Cdl3BlackCrowsLookback();
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

            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num39;
                decimal num44;
                decimal num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num46;
                            decimal num47;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num47 = inClose[i - 2];
                            }
                            else
                            {
                                num47 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num46 = inOpen[i - 2];
                            }
                            else
                            {
                                num46 = inClose[i - 2];
                            }

                            num45 = inHigh[i - 2] - num47 + (num46 - inLow[i - 2]);
                        }
                        else
                        {
                            num45 = Decimal.Zero;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                shadowVeryShortPeriodTotal[2] += num49;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num43;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num40;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num41;
                            decimal num42;
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

                            num40 = inHigh[i - 1] - num42 + (num41 - inLow[i - 1]);
                        }
                        else
                        {
                            num40 = Decimal.Zero;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                shadowVeryShortPeriodTotal[1] += num44;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num38;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num35;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num36;
                            decimal num37;
                            if (inClose[i] >= inOpen[i])
                            {
                                num37 = inClose[i];
                            }
                            else
                            {
                                num37 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num36 = inOpen[i];
                            }
                            else
                            {
                                num36 = inClose[i];
                            }

                            num35 = inHigh[i] - num37 + (num36 - inLow[i]);
                        }
                        else
                        {
                            num35 = Decimal.Zero;
                        }

                        num38 = num35;
                    }

                    num39 = num38;
                }

                shadowVeryShortPeriodTotal[0] += num39;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0341:
            if (inClose[i - 3] >= inOpen[i - 3] && inClose[i - 2] < inOpen[i - 2])
            {
                decimal num33;
                decimal num34;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num34 = inOpen[i - 2];
                }
                else
                {
                    num34 = inClose[i - 2];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num33 = shadowVeryShortPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    decimal num32;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num32 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num31;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num31 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num28;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                decimal num29;
                                decimal num30;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num30 = inClose[i - 2];
                                }
                                else
                                {
                                    num30 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num29 = inOpen[i - 2];
                                }
                                else
                                {
                                    num29 = inClose[i - 2];
                                }

                                num28 = inHigh[i - 2] - num30 + (num29 - inLow[i - 2]);
                            }
                            else
                            {
                                num28 = Decimal.Zero;
                            }

                            num31 = num28;
                        }

                        num32 = num31;
                    }

                    num33 = num32;
                }

                var num27 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (num34 - inLow[i - 2] <
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num33 / num27 &&
                    inClose[i - 1] < inOpen[i - 1])
                {
                    decimal num25;
                    decimal num26;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num26 = inOpen[i - 1];
                    }
                    else
                    {
                        num26 = inClose[i - 1];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num25 = shadowVeryShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num24;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num24 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num23;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num23 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num20;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num21;
                                    decimal num22;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num22 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num22 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num21 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num21 = inClose[i - 1];
                                    }

                                    num20 = inHigh[i - 1] - num22 + (num21 - inLow[i - 1]);
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

                    var num19 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (num26 - inLow[i - 1] <
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num25 / num19 &&
                        inClose[i] < inOpen[i])
                    {
                        decimal num17;
                        decimal num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inOpen[i];
                        }
                        else
                        {
                            num18 = inClose[i];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num17 = shadowVeryShortPeriodTotal[0] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num16;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num16 = Math.Abs(inClose[i] - inOpen[i]);
                            }
                            else
                            {
                                decimal num15;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num15 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    decimal num12;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                    {
                                        decimal num13;
                                        decimal num14;
                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num14 = inClose[i];
                                        }
                                        else
                                        {
                                            num14 = inOpen[i];
                                        }

                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num13 = inOpen[i];
                                        }
                                        else
                                        {
                                            num13 = inClose[i];
                                        }

                                        num12 = inHigh[i] - num14 + (num13 - inLow[i]);
                                    }
                                    else
                                    {
                                        num12 = Decimal.Zero;
                                    }

                                    num15 = num12;
                                }

                                num16 = num15;
                            }

                            num17 = num16;
                        }

                        var num11 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2m
                            : 1m;

                        if (num18 - inLow[i] < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num17 /
                            num11 && inOpen[i - 1] < inOpen[i - 2] && inOpen[i - 1] > inClose[i - 2] &&
                            inOpen[i] < inOpen[i - 1] && inOpen[i] > inClose[i - 1] && inHigh[i - 3] > inClose[i - 2] &&
                            inClose[i - 2] > inClose[i - 1] && inClose[i - 1] > inClose[i])
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_0891;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0891:
            int totIdx = 2;
            while (totIdx >= 0)
            {
                decimal num5;
                decimal num10;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num7;
                            decimal num8;
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

                            num6 = inHigh[i - totIdx] - num8 + (num7 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num6 = Decimal.Zero;
                        }

                        num9 = num6;
                    }

                    num10 = num9;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx - totIdx] - inOpen[shadowVeryShortTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[shadowVeryShortTrailingIdx - totIdx] - inLow[shadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num2;
                            decimal num3;
                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num3 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num2 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }

                            num = inHigh[shadowVeryShortTrailingIdx - totIdx] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = Decimal.Zero;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                shadowVeryShortPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            shadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0341;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl3BlackCrowsLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod + 3;
        }
    }
}
