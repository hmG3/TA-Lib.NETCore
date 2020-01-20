using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlConcealBabysWall(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            var shadowVeryShortPeriodTotal = new double[4];
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

            int lookbackTotal = CdlConcealBabysWallLookback();
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
                double num57;
                double num62;
                double num67;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                }
                else
                {
                    double num66;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        double num63;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num64;
                            double num65;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num65 = inClose[i - 3];
                            }
                            else
                            {
                                num65 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num64 = inOpen[i - 3];
                            }
                            else
                            {
                                num64 = inClose[i - 3];
                            }

                            num63 = inHigh[i - 3] - num65 + (num64 - inLow[i - 3]);
                        }
                        else
                        {
                            num63 = 0.0;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                shadowVeryShortPeriodTotal[3] += num67;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num62 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num61;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num61 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num58;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num59;
                            double num60;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num60 = inClose[i - 2];
                            }
                            else
                            {
                                num60 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num59 = inOpen[i - 2];
                            }
                            else
                            {
                                num59 = inClose[i - 2];
                            }

                            num58 = inHigh[i - 2] - num60 + (num59 - inLow[i - 2]);
                        }
                        else
                        {
                            num58 = 0.0;
                        }

                        num61 = num58;
                    }

                    num62 = num61;
                }

                shadowVeryShortPeriodTotal[2] += num62;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num57 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num56;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num56 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num53;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num54;
                            double num55;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num55 = inClose[i - 1];
                            }
                            else
                            {
                                num55 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num54 = inOpen[i - 1];
                            }
                            else
                            {
                                num54 = inClose[i - 1];
                            }

                            num53 = inHigh[i - 1] - num55 + (num54 - inLow[i - 1]);
                        }
                        else
                        {
                            num53 = 0.0;
                        }

                        num56 = num53;
                    }

                    num57 = num56;
                }

                shadowVeryShortPeriodTotal[1] += num57;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0336:
            if (inClose[i - 3] < inOpen[i - 3] && inClose[i - 2] < inOpen[i - 2] &&
                inClose[i - 1] < inOpen[i - 1] && inClose[i] < inOpen[i])
            {
                double num51;
                double num52;
                if (inClose[i - 3] >= inOpen[i - 3])
                {
                    num52 = inOpen[i - 3];
                }
                else
                {
                    num52 = inClose[i - 3];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num51 = shadowVeryShortPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    double num50;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num50 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                    }
                    else
                    {
                        double num49;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num49 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            double num46;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                double num47;
                                double num48;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num48 = inClose[i - 3];
                                }
                                else
                                {
                                    num48 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num47 = inOpen[i - 3];
                                }
                                else
                                {
                                    num47 = inClose[i - 3];
                                }

                                num46 = inHigh[i - 3] - num48 + (num47 - inLow[i - 3]);
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

                var num45 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (num52 - inLow[i - 3] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num51 / num45)
                {
                    double num43;
                    double num44;
                    if (inClose[i - 3] >= inOpen[i - 3])
                    {
                        num44 = inClose[i - 3];
                    }
                    else
                    {
                        num44 = inOpen[i - 3];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num43 = shadowVeryShortPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        double num42;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num42 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                        }
                        else
                        {
                            double num41;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num41 = inHigh[i - 3] - inLow[i - 3];
                            }
                            else
                            {
                                double num38;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    double num39;
                                    double num40;
                                    if (inClose[i - 3] >= inOpen[i - 3])
                                    {
                                        num40 = inClose[i - 3];
                                    }
                                    else
                                    {
                                        num40 = inOpen[i - 3];
                                    }

                                    if (inClose[i - 3] >= inOpen[i - 3])
                                    {
                                        num39 = inOpen[i - 3];
                                    }
                                    else
                                    {
                                        num39 = inClose[i - 3];
                                    }

                                    num38 = inHigh[i - 3] - num40 + (num39 - inLow[i - 3]);
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

                    var num37 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (inHigh[i - 3] - num44 < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num43 / num37)
                    {
                        double num35;
                        double num36;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num36 = inOpen[i - 2];
                        }
                        else
                        {
                            num36 = inClose[i - 2];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num35 = shadowVeryShortPeriodTotal[2] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            double num34;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num34 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                            }
                            else
                            {
                                double num33;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num33 = inHigh[i - 2] - inLow[i - 2];
                                }
                                else
                                {
                                    double num30;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                    {
                                        double num31;
                                        double num32;
                                        if (inClose[i - 2] >= inOpen[i - 2])
                                        {
                                            num32 = inClose[i - 2];
                                        }
                                        else
                                        {
                                            num32 = inOpen[i - 2];
                                        }

                                        if (inClose[i - 2] >= inOpen[i - 2])
                                        {
                                            num31 = inOpen[i - 2];
                                        }
                                        else
                                        {
                                            num31 = inClose[i - 2];
                                        }

                                        num30 = inHigh[i - 2] - num32 + (num31 - inLow[i - 2]);
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

                        var num29 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2.0
                            : 1.0;

                        if (num36 - inLow[i - 2] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num35 / num29)
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

                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                            {
                                num27 = shadowVeryShortPeriodTotal[2] /
                                        Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                            }
                            else
                            {
                                double num26;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                {
                                    num26 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                }
                                else
                                {
                                    double num25;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                    {
                                        num25 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        double num22;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                        {
                                            double num23;
                                            double num24;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num24 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num24 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num23 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num23 = inClose[i - 2];
                                            }

                                            num22 = inHigh[i - 2] - num24 + (num23 - inLow[i - 2]);
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

                            var num21 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                                ? 2.0
                                : 1.0;

                            if (inHigh[i - 2] - num28 <
                                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num27 / num21)
                            {
                                double num19;
                                double num20;
                                if (inOpen[i - 1] > inClose[i - 1])
                                {
                                    num20 = inOpen[i - 1];
                                }
                                else
                                {
                                    num20 = inClose[i - 1];
                                }

                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num19 = inOpen[i - 2];
                                }
                                else
                                {
                                    num19 = inClose[i - 2];
                                }

                                if (num20 < num19)
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

                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                                    {
                                        num17 = shadowVeryShortPeriodTotal[1] /
                                                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                                    }
                                    else
                                    {
                                        double num16;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                        {
                                            num16 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                        }
                                        else
                                        {
                                            double num15;
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.HighLow)
                                            {
                                                num15 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                double num12;
                                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                    RangeType.Shadows)
                                                {
                                                    double num13;
                                                    double num14;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num14 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num14 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num13 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num13 = inClose[i - 1];
                                                    }

                                                    num12 = inHigh[i - 1] - num14 + (num13 - inLow[i - 1]);
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

                                    var num11 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows
                                        ? 2.0
                                        : 1.0;

                                    if (inHigh[i - 1] - num18 >
                                        Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num17 / num11 &&
                                        inHigh[i - 1] > inClose[i - 2] && inHigh[i] > inHigh[i - 1] && inLow[i] < inLow[i - 1])
                                    {
                                        outInteger[outIdx] = 100;
                                        outIdx++;
                                        goto Label_0B63;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0B63:
            var totIdx = 3;
            while (totIdx >= 1)
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
                goto Label_0336;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlConcealBabysWall(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            var shadowVeryShortPeriodTotal = new decimal[4];
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

            int lookbackTotal = CdlConcealBabysWallLookback();
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
                decimal num57;
                decimal num62;
                decimal num67;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                }
                else
                {
                    decimal num66;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        decimal num63;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num64;
                            decimal num65;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num65 = inClose[i - 3];
                            }
                            else
                            {
                                num65 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num64 = inOpen[i - 3];
                            }
                            else
                            {
                                num64 = inClose[i - 3];
                            }

                            num63 = inHigh[i - 3] - num65 + (num64 - inLow[i - 3]);
                        }
                        else
                        {
                            num63 = Decimal.Zero;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                shadowVeryShortPeriodTotal[3] += num67;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num62 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num61;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num61 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num58;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num59;
                            decimal num60;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num60 = inClose[i - 2];
                            }
                            else
                            {
                                num60 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num59 = inOpen[i - 2];
                            }
                            else
                            {
                                num59 = inClose[i - 2];
                            }

                            num58 = inHigh[i - 2] - num60 + (num59 - inLow[i - 2]);
                        }
                        else
                        {
                            num58 = Decimal.Zero;
                        }

                        num61 = num58;
                    }

                    num62 = num61;
                }

                shadowVeryShortPeriodTotal[2] += num62;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num57 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num56;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num56 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num53;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num54;
                            decimal num55;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num55 = inClose[i - 1];
                            }
                            else
                            {
                                num55 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num54 = inOpen[i - 1];
                            }
                            else
                            {
                                num54 = inClose[i - 1];
                            }

                            num53 = inHigh[i - 1] - num55 + (num54 - inLow[i - 1]);
                        }
                        else
                        {
                            num53 = Decimal.Zero;
                        }

                        num56 = num53;
                    }

                    num57 = num56;
                }

                shadowVeryShortPeriodTotal[1] += num57;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0360:
            if (inClose[i - 3] < inOpen[i - 3] && inClose[i - 2] < inOpen[i - 2] &&
                inClose[i - 1] < inOpen[i - 1] && inClose[i] < inOpen[i])
            {
                decimal num51;
                decimal num52;
                if (inClose[i - 3] >= inOpen[i - 3])
                {
                    num52 = inOpen[i - 3];
                }
                else
                {
                    num52 = inClose[i - 3];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num51 = shadowVeryShortPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    decimal num50;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num50 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                    }
                    else
                    {
                        decimal num49;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num49 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            decimal num46;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                decimal num47;
                                decimal num48;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num48 = inClose[i - 3];
                                }
                                else
                                {
                                    num48 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num47 = inOpen[i - 3];
                                }
                                else
                                {
                                    num47 = inClose[i - 3];
                                }

                                num46 = inHigh[i - 3] - num48 + (num47 - inLow[i - 3]);
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

                var num45 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (num52 - inLow[i - 3] < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num51 / num45)
                {
                    decimal num43;
                    decimal num44;
                    if (inClose[i - 3] >= inOpen[i - 3])
                    {
                        num44 = inClose[i - 3];
                    }
                    else
                    {
                        num44 = inOpen[i - 3];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num43 = shadowVeryShortPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num42;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num42 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                        }
                        else
                        {
                            decimal num41;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num41 = inHigh[i - 3] - inLow[i - 3];
                            }
                            else
                            {
                                decimal num38;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num39;
                                    decimal num40;
                                    if (inClose[i - 3] >= inOpen[i - 3])
                                    {
                                        num40 = inClose[i - 3];
                                    }
                                    else
                                    {
                                        num40 = inOpen[i - 3];
                                    }

                                    if (inClose[i - 3] >= inOpen[i - 3])
                                    {
                                        num39 = inOpen[i - 3];
                                    }
                                    else
                                    {
                                        num39 = inClose[i - 3];
                                    }

                                    num38 = inHigh[i - 3] - num40 + (num39 - inLow[i - 3]);
                                }
                                else
                                {
                                    num38 = Decimal.Zero;
                                }

                                num41 = num38;
                            }

                            num42 = num41;
                        }

                        num43 = num42;
                    }

                    var num37 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (inHigh[i - 3] - num44 <
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num43 / num37)
                    {
                        decimal num35;
                        decimal num36;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num36 = inOpen[i - 2];
                        }
                        else
                        {
                            num36 = inClose[i - 2];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num35 = shadowVeryShortPeriodTotal[2] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num34;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num34 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                            }
                            else
                            {
                                decimal num33;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num33 = inHigh[i - 2] - inLow[i - 2];
                                }
                                else
                                {
                                    decimal num30;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                    {
                                        decimal num31;
                                        decimal num32;
                                        if (inClose[i - 2] >= inOpen[i - 2])
                                        {
                                            num32 = inClose[i - 2];
                                        }
                                        else
                                        {
                                            num32 = inOpen[i - 2];
                                        }

                                        if (inClose[i - 2] >= inOpen[i - 2])
                                        {
                                            num31 = inOpen[i - 2];
                                        }
                                        else
                                        {
                                            num31 = inClose[i - 2];
                                        }

                                        num30 = inHigh[i - 2] - num32 + (num31 - inLow[i - 2]);
                                    }
                                    else
                                    {
                                        num30 = Decimal.Zero;
                                    }

                                    num33 = num30;
                                }

                                num34 = num33;
                            }

                            num35 = num34;
                        }

                        var num29 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2m
                            : 1m;

                        if (num36 - inLow[i - 2] < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor *
                            num35 / num29)
                        {
                            decimal num27;
                            decimal num28;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num28 = inClose[i - 2];
                            }
                            else
                            {
                                num28 = inOpen[i - 2];
                            }

                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                            {
                                num27 = shadowVeryShortPeriodTotal[2] /
                                        Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                            }
                            else
                            {
                                decimal num26;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                {
                                    num26 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                }
                                else
                                {
                                    decimal num25;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                    {
                                        num25 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        decimal num22;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                        {
                                            decimal num23;
                                            decimal num24;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num24 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num24 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num23 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num23 = inClose[i - 2];
                                            }

                                            num22 = inHigh[i - 2] - num24 + (num23 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num22 = Decimal.Zero;
                                        }

                                        num25 = num22;
                                    }

                                    num26 = num25;
                                }

                                num27 = num26;
                            }

                            var num21 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                                ? 2m
                                : 1m;

                            if (inHigh[i - 2] - num28 < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor *
                                num27 / num21)
                            {
                                decimal num19;
                                decimal num20;
                                if (inOpen[i - 1] > inClose[i - 1])
                                {
                                    num20 = inOpen[i - 1];
                                }
                                else
                                {
                                    num20 = inClose[i - 1];
                                }

                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num19 = inOpen[i - 2];
                                }
                                else
                                {
                                    num19 = inClose[i - 2];
                                }

                                if (num20 < num19)
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

                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                                    {
                                        num17 = shadowVeryShortPeriodTotal[1] /
                                                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                                    }
                                    else
                                    {
                                        decimal num16;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                        {
                                            num16 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                        }
                                        else
                                        {
                                            decimal num15;
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.HighLow)
                                            {
                                                num15 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                decimal num12;
                                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                    RangeType.Shadows)
                                                {
                                                    decimal num13;
                                                    decimal num14;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num14 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num14 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num13 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num13 = inClose[i - 1];
                                                    }

                                                    num12 = inHigh[i - 1] - num14 + (num13 - inLow[i - 1]);
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

                                    var num11 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows
                                        ? 2m
                                        : 1m;

                                    if (inHigh[i - 1] - num18 >
                                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num17 / num11 &&
                                        inHigh[i - 1] > inClose[i - 2] && inHigh[i] > inHigh[i - 1] && inLow[i] < inLow[i - 1])
                                    {
                                        outInteger[outIdx] = 100;
                                        outIdx++;
                                        goto Label_0BFF;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0BFF:
            var totIdx = 3;
            while (totIdx >= 1)
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
                goto Label_0360;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlConcealBabysWallLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod + 3;
        }
    }
}
