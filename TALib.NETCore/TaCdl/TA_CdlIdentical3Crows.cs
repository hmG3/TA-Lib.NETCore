using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlIdentical3Crows(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            var shadowVeryShortPeriodTotal = new double[3];
            var equalPeriodTotal = new double[3];
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

            int lookbackTotal = CdlIdentical3CrowsLookback();
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
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num87;
                double num92;
                double num97;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num97 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num96;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num96 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num93;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num94;
                            double num95;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num95 = inClose[i - 2];
                            }
                            else
                            {
                                num95 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num94 = inOpen[i - 2];
                            }
                            else
                            {
                                num94 = inClose[i - 2];
                            }

                            num93 = inHigh[i - 2] - num95 + (num94 - inLow[i - 2]);
                        }
                        else
                        {
                            num93 = 0.0;
                        }

                        num96 = num93;
                    }

                    num97 = num96;
                }

                shadowVeryShortPeriodTotal[2] += num97;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num92 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num91;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num91 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num88;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num89;
                            double num90;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num90 = inClose[i - 1];
                            }
                            else
                            {
                                num90 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num89 = inOpen[i - 1];
                            }
                            else
                            {
                                num89 = inClose[i - 1];
                            }

                            num88 = inHigh[i - 1] - num90 + (num89 - inLow[i - 1]);
                        }
                        else
                        {
                            num88 = 0.0;
                        }

                        num91 = num88;
                    }

                    num92 = num91;
                }

                shadowVeryShortPeriodTotal[1] += num92;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num87 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num86;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num86 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num83;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num84;
                            double num85;
                            if (inClose[i] >= inOpen[i])
                            {
                                num85 = inClose[i];
                            }
                            else
                            {
                                num85 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num84 = inOpen[i];
                            }
                            else
                            {
                                num84 = inClose[i];
                            }

                            num83 = inHigh[i] - num85 + (num84 - inLow[i]);
                        }
                        else
                        {
                            num83 = 0.0;
                        }

                        num86 = num83;
                    }

                    num87 = num86;
                }

                shadowVeryShortPeriodTotal[0] += num87;
                i++;
            }

            i = equalTrailingIdx;
            while (true)
            {
                double num77;
                double num82;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num82 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num81;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num81 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num78;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            double num79;
                            double num80;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num80 = inClose[i - 2];
                            }
                            else
                            {
                                num80 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num79 = inOpen[i - 2];
                            }
                            else
                            {
                                num79 = inClose[i - 2];
                            }

                            num78 = inHigh[i - 2] - num80 + (num79 - inLow[i - 2]);
                        }
                        else
                        {
                            num78 = 0.0;
                        }

                        num81 = num78;
                    }

                    num82 = num81;
                }

                equalPeriodTotal[2] += num82;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num77 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num76;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num76 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num73;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            double num74;
                            double num75;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num75 = inClose[i - 1];
                            }
                            else
                            {
                                num75 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num74 = inOpen[i - 1];
                            }
                            else
                            {
                                num74 = inClose[i - 1];
                            }

                            num73 = inHigh[i - 1] - num75 + (num74 - inLow[i - 1]);
                        }
                        else
                        {
                            num73 = 0.0;
                        }

                        num76 = num73;
                    }

                    num77 = num76;
                }

                equalPeriodTotal[1] += num77;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0529:
            if (inClose[i - 2] < inOpen[i - 2])
            {
                double num71;
                double num72;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num72 = inOpen[i - 2];
                }
                else
                {
                    num72 = inClose[i - 2];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num71 = shadowVeryShortPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    double num70;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num70 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num69;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num69 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num66;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                double num67;
                                double num68;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num68 = inClose[i - 2];
                                }
                                else
                                {
                                    num68 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num67 = inOpen[i - 2];
                                }
                                else
                                {
                                    num67 = inClose[i - 2];
                                }

                                num66 = inHigh[i - 2] - num68 + (num67 - inLow[i - 2]);
                            }
                            else
                            {
                                num66 = 0.0;
                            }

                            num69 = num66;
                        }

                        num70 = num69;
                    }

                    num71 = num70;
                }

                var num65 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (num72 - inLow[i - 2] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num71 / num65 &&
                    inClose[i - 1] < inOpen[i - 1])
                {
                    double num63;
                    double num64;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num64 = inOpen[i - 1];
                    }
                    else
                    {
                        num64 = inClose[i - 1];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num63 = shadowVeryShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        double num62;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num62 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num61;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num61 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num58;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    double num59;
                                    double num60;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num60 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num60 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num59 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num59 = inClose[i - 1];
                                    }

                                    num58 = inHigh[i - 1] - num60 + (num59 - inLow[i - 1]);
                                }
                                else
                                {
                                    num58 = 0.0;
                                }

                                num61 = num58;
                            }

                            num62 = num61;
                        }

                        num63 = num62;
                    }

                    var num57 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (num64 - inLow[i - 1] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num63 / num57 &&
                        inClose[i] < inOpen[i])
                    {
                        double num55;
                        double num56;
                        if (inClose[i] >= inOpen[i])
                        {
                            num56 = inOpen[i];
                        }
                        else
                        {
                            num56 = inClose[i];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num55 = shadowVeryShortPeriodTotal[0] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            double num54;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num54 = Math.Abs(inClose[i] - inOpen[i]);
                            }
                            else
                            {
                                double num53;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num53 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    double num50;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                                        num50 = inHigh[i] - num52 + (num51 - inLow[i]);
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

                        var num49 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2.0
                            : 1.0;

                        if (num56 - inLow[i] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num55 / num49 &&
                            inClose[i - 2] > inClose[i - 1] && inClose[i - 1] > inClose[i])
                        {
                            double num48;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                            {
                                num48 = equalPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                            }
                            else
                            {
                                double num47;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                {
                                    num47 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                }
                                else
                                {
                                    double num46;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                    {
                                        num46 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        double num43;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                        {
                                            double num44;
                                            double num45;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num45 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num45 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num44 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num44 = inClose[i - 2];
                                            }

                                            num43 = inHigh[i - 2] - num45 + (num44 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num43 = 0.0;
                                        }

                                        num46 = num43;
                                    }

                                    num47 = num46;
                                }

                                num48 = num47;
                            }

                            var num42 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                            if (inOpen[i - 1] <=
                                inClose[i - 2] + Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num48 / num42)
                            {
                                double num41;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                                {
                                    num41 = equalPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                                }
                                else
                                {
                                    double num40;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                    {
                                        num40 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                    }
                                    else
                                    {
                                        double num39;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                        {
                                            num39 = inHigh[i - 2] - inLow[i - 2];
                                        }
                                        else
                                        {
                                            double num36;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                            {
                                                double num37;
                                                double num38;
                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num38 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num38 = inOpen[i - 2];
                                                }

                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num37 = inOpen[i - 2];
                                                }
                                                else
                                                {
                                                    num37 = inClose[i - 2];
                                                }

                                                num36 = inHigh[i - 2] - num38 + (num37 - inLow[i - 2]);
                                            }
                                            else
                                            {
                                                num36 = 0.0;
                                            }

                                            num39 = num36;
                                        }

                                        num40 = num39;
                                    }

                                    num41 = num40;
                                }

                                var num35 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows
                                    ? 2.0
                                    : 1.0;

                                if (inOpen[i - 1] >=
                                    inClose[i - 2] - Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num41 / num35)
                                {
                                    double num34;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                                    {
                                        num34 = equalPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                                    }
                                    else
                                    {
                                        double num33;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                        {
                                            num33 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                        }
                                        else
                                        {
                                            double num32;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                            {
                                                num32 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                double num29;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                                {
                                                    double num30;
                                                    double num31;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num31 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num31 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num30 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num30 = inClose[i - 1];
                                                    }

                                                    num29 = inHigh[i - 1] - num31 + (num30 - inLow[i - 1]);
                                                }
                                                else
                                                {
                                                    num29 = 0.0;
                                                }

                                                num32 = num29;
                                            }

                                            num33 = num32;
                                        }

                                        num34 = num33;
                                    }

                                    var num28 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows
                                        ? 2.0
                                        : 1.0;

                                    if (inOpen[i] <=
                                        inClose[i - 1] + Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num34 / num28)
                                    {
                                        double num27;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                                        {
                                            num27 = equalPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                                        }
                                        else
                                        {
                                            double num26;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                            {
                                                num26 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                            }
                                            else
                                            {
                                                double num25;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                                {
                                                    num25 = inHigh[i - 1] - inLow[i - 1];
                                                }
                                                else
                                                {
                                                    double num22;
                                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType ==
                                                        RangeType.Shadows)
                                                    {
                                                        double num23;
                                                        double num24;
                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num24 = inClose[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num24 = inOpen[i - 1];
                                                        }

                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num23 = inOpen[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num23 = inClose[i - 1];
                                                        }

                                                        num22 = inHigh[i - 1] - num24 + (num23 - inLow[i - 1]);
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

                                        var num21 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows
                                            ? 2.0
                                            : 1.0;

                                        if (inOpen[i] >=
                                            inClose[i - 1] - Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num27 / num21)
                                        {
                                            outInteger[outIdx] = -100;
                                            outIdx++;
                                            goto Label_0F74;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0F74:
            var totIdx = 2;
            while (totIdx >= 0)
            {
                double num15;
                double num20;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num19;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num17;
                            double num18;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num18 = inClose[i - totIdx];
                            }
                            else
                            {
                                num18 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num17 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num17 = inClose[i - totIdx];
                            }

                            num16 = inHigh[i - totIdx] - num18 + (num17 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num16 = 0.0;
                        }

                        num19 = num16;
                    }

                    num20 = num19;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs(inClose[shadowVeryShortTrailingIdx - totIdx] - inOpen[shadowVeryShortTrailingIdx - totIdx]);
                }
                else
                {
                    double num14;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[shadowVeryShortTrailingIdx - totIdx] - inLow[shadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num11;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num12;
                            double num13;
                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num13 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num12 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }

                            num11 = inHigh[shadowVeryShortTrailingIdx - totIdx] - num13 +
                                    (num12 - inLow[shadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                shadowVeryShortPeriodTotal[totIdx] += num20 - num15;
                totIdx--;
            }

            for (totIdx = 2; totIdx >= 1; totIdx--)
            {
                double num5;
                double num10;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[equalTrailingIdx - totIdx] - inOpen[equalTrailingIdx - totIdx]);
                }
                else
                {
                    double num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[equalTrailingIdx - totIdx] - inLow[equalTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[equalTrailingIdx - totIdx] >= inOpen[equalTrailingIdx - totIdx])
                            {
                                num3 = inClose[equalTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[equalTrailingIdx - totIdx];
                            }

                            if (inClose[equalTrailingIdx - totIdx] >= inOpen[equalTrailingIdx - totIdx])
                            {
                                num2 = inOpen[equalTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[equalTrailingIdx - totIdx];
                            }

                            num = inHigh[equalTrailingIdx - totIdx] - num3 + (num2 - inLow[equalTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                equalPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            shadowVeryShortTrailingIdx++;
            equalTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0529;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlIdentical3Crows(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            var shadowVeryShortPeriodTotal = new decimal[3];
            var equalPeriodTotal = new decimal[3];
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

            int lookbackTotal = CdlIdentical3CrowsLookback();
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
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num87;
                decimal num92;
                decimal num97;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num97 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num96;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num96 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num93;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num94;
                            decimal num95;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num95 = inClose[i - 2];
                            }
                            else
                            {
                                num95 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num94 = inOpen[i - 2];
                            }
                            else
                            {
                                num94 = inClose[i - 2];
                            }

                            num93 = inHigh[i - 2] - num95 + (num94 - inLow[i - 2]);
                        }
                        else
                        {
                            num93 = Decimal.Zero;
                        }

                        num96 = num93;
                    }

                    num97 = num96;
                }

                shadowVeryShortPeriodTotal[2] += num97;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num92 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num91;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num91 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num88;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num89;
                            decimal num90;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num90 = inClose[i - 1];
                            }
                            else
                            {
                                num90 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num89 = inOpen[i - 1];
                            }
                            else
                            {
                                num89 = inClose[i - 1];
                            }

                            num88 = inHigh[i - 1] - num90 + (num89 - inLow[i - 1]);
                        }
                        else
                        {
                            num88 = Decimal.Zero;
                        }

                        num91 = num88;
                    }

                    num92 = num91;
                }

                shadowVeryShortPeriodTotal[1] += num92;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num87 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num86;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num86 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num83;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num84;
                            decimal num85;
                            if (inClose[i] >= inOpen[i])
                            {
                                num85 = inClose[i];
                            }
                            else
                            {
                                num85 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num84 = inOpen[i];
                            }
                            else
                            {
                                num84 = inClose[i];
                            }

                            num83 = inHigh[i] - num85 + (num84 - inLow[i]);
                        }
                        else
                        {
                            num83 = Decimal.Zero;
                        }

                        num86 = num83;
                    }

                    num87 = num86;
                }

                shadowVeryShortPeriodTotal[0] += num87;
                i++;
            }

            i = equalTrailingIdx;
            while (true)
            {
                decimal num77;
                decimal num82;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num82 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num81;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num81 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num78;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num79;
                            decimal num80;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num80 = inClose[i - 2];
                            }
                            else
                            {
                                num80 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num79 = inOpen[i - 2];
                            }
                            else
                            {
                                num79 = inClose[i - 2];
                            }

                            num78 = inHigh[i - 2] - num80 + (num79 - inLow[i - 2]);
                        }
                        else
                        {
                            num78 = Decimal.Zero;
                        }

                        num81 = num78;
                    }

                    num82 = num81;
                }

                equalPeriodTotal[2] += num82;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num77 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num76;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num76 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num73;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num74;
                            decimal num75;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num75 = inClose[i - 1];
                            }
                            else
                            {
                                num75 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num74 = inOpen[i - 1];
                            }
                            else
                            {
                                num74 = inClose[i - 1];
                            }

                            num73 = inHigh[i - 1] - num75 + (num74 - inLow[i - 1]);
                        }
                        else
                        {
                            num73 = Decimal.Zero;
                        }

                        num76 = num73;
                    }

                    num77 = num76;
                }

                equalPeriodTotal[1] += num77;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_056F:
            if (inClose[i - 2] < inOpen[i - 2])
            {
                decimal num71;
                decimal num72;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num72 = inOpen[i - 2];
                }
                else
                {
                    num72 = inClose[i - 2];
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num71 = shadowVeryShortPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    decimal num70;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num70 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num69;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num69 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num66;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                            {
                                decimal num67;
                                decimal num68;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num68 = inClose[i - 2];
                                }
                                else
                                {
                                    num68 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num67 = inOpen[i - 2];
                                }
                                else
                                {
                                    num67 = inClose[i - 2];
                                }

                                num66 = inHigh[i - 2] - num68 + (num67 - inLow[i - 2]);
                            }
                            else
                            {
                                num66 = Decimal.Zero;
                            }

                            num69 = num66;
                        }

                        num70 = num69;
                    }

                    num71 = num70;
                }

                var num65 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (num72 - inLow[i - 2] <
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num71 / num65 &&
                    inClose[i - 1] < inOpen[i - 1])
                {
                    decimal num63;
                    decimal num64;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num64 = inOpen[i - 1];
                    }
                    else
                    {
                        num64 = inClose[i - 1];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num63 = shadowVeryShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num62;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num62 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num61;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num61 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num58;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num59;
                                    decimal num60;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num60 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num60 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num59 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num59 = inClose[i - 1];
                                    }

                                    num58 = inHigh[i - 1] - num60 + (num59 - inLow[i - 1]);
                                }
                                else
                                {
                                    num58 = Decimal.Zero;
                                }

                                num61 = num58;
                            }

                            num62 = num61;
                        }

                        num63 = num62;
                    }

                    var num57 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (num64 - inLow[i - 1] <
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num63 / num57 &&
                        inClose[i] < inOpen[i])
                    {
                        decimal num55;
                        decimal num56;
                        if (inClose[i] >= inOpen[i])
                        {
                            num56 = inOpen[i];
                        }
                        else
                        {
                            num56 = inClose[i];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num55 = shadowVeryShortPeriodTotal[0] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num54;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num54 = Math.Abs(inClose[i] - inOpen[i]);
                            }
                            else
                            {
                                decimal num53;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num53 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    decimal num50;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                    {
                                        decimal num51;
                                        decimal num52;
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

                                        num50 = inHigh[i] - num52 + (num51 - inLow[i]);
                                    }
                                    else
                                    {
                                        num50 = Decimal.Zero;
                                    }

                                    num53 = num50;
                                }

                                num54 = num53;
                            }

                            num55 = num54;
                        }

                        var num49 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2m
                            : 1m;

                        if (num56 - inLow[i] <
                            (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num55 / num49 &&
                            inClose[i - 2] > inClose[i - 1] && inClose[i - 1] > inClose[i])
                        {
                            decimal num48;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                            {
                                num48 = equalPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                            }
                            else
                            {
                                decimal num47;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                {
                                    num47 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                }
                                else
                                {
                                    decimal num46;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                    {
                                        num46 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        decimal num43;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                        {
                                            decimal num44;
                                            decimal num45;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num45 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num45 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num44 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num44 = inClose[i - 2];
                                            }

                                            num43 = inHigh[i - 2] - num45 + (num44 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num43 = Decimal.Zero;
                                        }

                                        num46 = num43;
                                    }

                                    num47 = num46;
                                }

                                num48 = num47;
                            }

                            var num42 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (inOpen[i - 1] <=
                                inClose[i - 2] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num48 / num42)
                            {
                                decimal num41;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                                {
                                    num41 = equalPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                                }
                                else
                                {
                                    decimal num40;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                    {
                                        num40 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                    }
                                    else
                                    {
                                        decimal num39;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                        {
                                            num39 = inHigh[i - 2] - inLow[i - 2];
                                        }
                                        else
                                        {
                                            decimal num36;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                            {
                                                decimal num37;
                                                decimal num38;
                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num38 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num38 = inOpen[i - 2];
                                                }

                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num37 = inOpen[i - 2];
                                                }
                                                else
                                                {
                                                    num37 = inClose[i - 2];
                                                }

                                                num36 = inHigh[i - 2] - num38 + (num37 - inLow[i - 2]);
                                            }
                                            else
                                            {
                                                num36 = Decimal.Zero;
                                            }

                                            num39 = num36;
                                        }

                                        num40 = num39;
                                    }

                                    num41 = num40;
                                }

                                var num35 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

                                if (inOpen[i - 1] >=
                                    inClose[i - 2] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num41 / num35)
                                {
                                    decimal num34;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                                    {
                                        num34 = equalPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                                    }
                                    else
                                    {
                                        decimal num33;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                        {
                                            num33 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                        }
                                        else
                                        {
                                            decimal num32;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                            {
                                                num32 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                decimal num29;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                                {
                                                    decimal num30;
                                                    decimal num31;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num31 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num31 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num30 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num30 = inClose[i - 1];
                                                    }

                                                    num29 = inHigh[i - 1] - num31 + (num30 - inLow[i - 1]);
                                                }
                                                else
                                                {
                                                    num29 = Decimal.Zero;
                                                }

                                                num32 = num29;
                                            }

                                            num33 = num32;
                                        }

                                        num34 = num33;
                                    }

                                    var num28 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows
                                        ? 2m
                                        : 1m;

                                    if (inOpen[i] <=
                                        inClose[i - 1] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor *
                                        num34 / num28)
                                    {
                                        decimal num27;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                                        {
                                            num27 = equalPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                                        }
                                        else
                                        {
                                            decimal num26;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                            {
                                                num26 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                            }
                                            else
                                            {
                                                decimal num25;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                                {
                                                    num25 = inHigh[i - 1] - inLow[i - 1];
                                                }
                                                else
                                                {
                                                    decimal num22;
                                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType ==
                                                        RangeType.Shadows)
                                                    {
                                                        decimal num23;
                                                        decimal num24;
                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num24 = inClose[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num24 = inOpen[i - 1];
                                                        }

                                                        if (inClose[i - 1] >= inOpen[i - 1])
                                                        {
                                                            num23 = inOpen[i - 1];
                                                        }
                                                        else
                                                        {
                                                            num23 = inClose[i - 1];
                                                        }

                                                        num22 = inHigh[i - 1] - num24 + (num23 - inLow[i - 1]);
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

                                        var num21 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows
                                            ? 2m
                                            : 1m;

                                        if (inOpen[i] >=
                                            inClose[i - 1] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor *
                                            num27 / num21)
                                        {
                                            outInteger[outIdx] = -100;
                                            outIdx++;
                                            goto Label_103A;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_103A:
            var totIdx = 2;
            while (totIdx >= 0)
            {
                decimal num15;
                decimal num20;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num19;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num17;
                            decimal num18;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num18 = inClose[i - totIdx];
                            }
                            else
                            {
                                num18 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num17 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num17 = inClose[i - totIdx];
                            }

                            num16 = inHigh[i - totIdx] - num18 + (num17 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num16 = Decimal.Zero;
                        }

                        num19 = num16;
                    }

                    num20 = num19;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs(inClose[shadowVeryShortTrailingIdx - totIdx] - inOpen[shadowVeryShortTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num14;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[shadowVeryShortTrailingIdx - totIdx] - inLow[shadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num11;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num12;
                            decimal num13;
                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num13 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[shadowVeryShortTrailingIdx - totIdx] >= inOpen[shadowVeryShortTrailingIdx - totIdx])
                            {
                                num12 = inOpen[shadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[shadowVeryShortTrailingIdx - totIdx];
                            }

                            num11 = inHigh[shadowVeryShortTrailingIdx - totIdx] - num13 +
                                    (num12 - inLow[shadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = Decimal.Zero;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                shadowVeryShortPeriodTotal[totIdx] += num20 - num15;
                totIdx--;
            }

            for (totIdx = 2; totIdx >= 1; totIdx--)
            {
                decimal num5;
                decimal num10;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[equalTrailingIdx - totIdx] - inOpen[equalTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[equalTrailingIdx - totIdx] - inLow[equalTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num2;
                            decimal num3;
                            if (inClose[equalTrailingIdx - totIdx] >= inOpen[equalTrailingIdx - totIdx])
                            {
                                num3 = inClose[equalTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[equalTrailingIdx - totIdx];
                            }

                            if (inClose[equalTrailingIdx - totIdx] >= inOpen[equalTrailingIdx - totIdx])
                            {
                                num2 = inOpen[equalTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[equalTrailingIdx - totIdx];
                            }

                            num = inHigh[equalTrailingIdx - totIdx] - num3 + (num2 - inLow[equalTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = Decimal.Zero;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                equalPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            shadowVeryShortTrailingIdx++;
            equalTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_056F;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlIdentical3CrowsLookback()
        {
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod);

            return avgPeriod + 2;
        }
    }
}
