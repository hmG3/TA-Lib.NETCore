using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlKicking(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int num68;
            var shadowVeryShortPeriodTotal = new double[2];
            var bodyLongPeriodTotal = new double[2];
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

            int lookbackTotal = CdlKickingLookback();
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
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num83;
                double num88;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num88 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num87;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num87 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num84;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num85;
                            double num86;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num86 = inClose[i - 1];
                            }
                            else
                            {
                                num86 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num85 = inOpen[i - 1];
                            }
                            else
                            {
                                num85 = inClose[i - 1];
                            }

                            num84 = inHigh[i - 1] - num86 + (num85 - inLow[i - 1]);
                        }
                        else
                        {
                            num84 = 0.0;
                        }

                        num87 = num84;
                    }

                    num88 = num87;
                }

                shadowVeryShortPeriodTotal[1] += num88;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num83 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num82;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num82 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num79;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num80;
                            double num81;
                            if (inClose[i] >= inOpen[i])
                            {
                                num81 = inClose[i];
                            }
                            else
                            {
                                num81 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num80 = inOpen[i];
                            }
                            else
                            {
                                num80 = inClose[i];
                            }

                            num79 = inHigh[i] - num81 + (num80 - inLow[i]);
                        }
                        else
                        {
                            num79 = 0.0;
                        }

                        num82 = num79;
                    }

                    num83 = num82;
                }

                shadowVeryShortPeriodTotal[0] += num83;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                double num73;
                double num78;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num78 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num77;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num77 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num74;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num75;
                            double num76;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num76 = inClose[i - 1];
                            }
                            else
                            {
                                num76 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num75 = inOpen[i - 1];
                            }
                            else
                            {
                                num75 = inClose[i - 1];
                            }

                            num74 = inHigh[i - 1] - num76 + (num75 - inLow[i - 1]);
                        }
                        else
                        {
                            num74 = 0.0;
                        }

                        num77 = num74;
                    }

                    num78 = num77;
                }

                bodyLongPeriodTotal[1] += num78;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num73 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num72;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num72 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num69;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num70;
                            double num71;
                            if (inClose[i] >= inOpen[i])
                            {
                                num71 = inClose[i];
                            }
                            else
                            {
                                num71 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num70 = inOpen[i];
                            }
                            else
                            {
                                num70 = inClose[i];
                            }

                            num69 = inHigh[i] - num71 + (num70 - inLow[i]);
                        }
                        else
                        {
                            num69 = 0.0;
                        }

                        num72 = num69;
                    }

                    num73 = num72;
                }

                bodyLongPeriodTotal[0] += num73;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0413:
            if (inClose[i] >= inOpen[i])
            {
                num68 = 1;
            }
            else
            {
                num68 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) == -num68)
            {
                double num67;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num67 = bodyLongPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num66;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num66 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num65;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num65 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num62;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                double num63;
                                double num64;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num64 = inClose[i - 1];
                                }
                                else
                                {
                                    num64 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num63 = inOpen[i - 1];
                                }
                                else
                                {
                                    num63 = inClose[i - 1];
                                }

                                num62 = inHigh[i - 1] - num64 + (num63 - inLow[i - 1]);
                            }
                            else
                            {
                                num62 = 0.0;
                            }

                            num65 = num62;
                        }

                        num66 = num65;
                    }

                    num67 = num66;
                }

                var num61 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num67 / num61)
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

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num59 = shadowVeryShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        double num58;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num58 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num57;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num57 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num54;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    double num55;
                                    double num56;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num56 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num56 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num55 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num55 = inClose[i - 1];
                                    }

                                    num54 = inHigh[i - 1] - num56 + (num55 - inLow[i - 1]);
                                }
                                else
                                {
                                    num54 = 0.0;
                                }

                                num57 = num54;
                            }

                            num58 = num57;
                        }

                        num59 = num58;
                    }

                    var num53 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (inHigh[i - 1] - num60 < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num59 / num53)
                    {
                        double num51;
                        double num52;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num52 = inOpen[i - 1];
                        }
                        else
                        {
                            num52 = inClose[i - 1];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num51 = shadowVeryShortPeriodTotal[1] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            double num50;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num50 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                double num49;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num49 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num46;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                        var num45 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2.0
                            : 1.0;

                        if (num52 - inLow[i - 1] < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num51 / num45)
                        {
                            double num44;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                            {
                                num44 = bodyLongPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                            }
                            else
                            {
                                double num43;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                                {
                                    num43 = Math.Abs(inClose[i] - inOpen[i]);
                                }
                                else
                                {
                                    double num42;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                                    {
                                        num42 = inHigh[i] - inLow[i];
                                    }
                                    else
                                    {
                                        double num39;
                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                        {
                                            double num40;
                                            double num41;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num41 = inClose[i];
                                            }
                                            else
                                            {
                                                num41 = inOpen[i];
                                            }

                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num40 = inOpen[i];
                                            }
                                            else
                                            {
                                                num40 = inClose[i];
                                            }

                                            num39 = inHigh[i] - num41 + (num40 - inLow[i]);
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

                            var num38 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                            if (Math.Abs(inClose[i] - inOpen[i]) >
                                Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num44 / num38)
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

                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                                {
                                    num36 = shadowVeryShortPeriodTotal[0] /
                                            Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                                }
                                else
                                {
                                    double num35;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                    {
                                        num35 = Math.Abs(inClose[i] - inOpen[i]);
                                    }
                                    else
                                    {
                                        double num34;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                        {
                                            num34 = inHigh[i] - inLow[i];
                                        }
                                        else
                                        {
                                            double num31;
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows)
                                            {
                                                double num32;
                                                double num33;
                                                if (inClose[i] >= inOpen[i])
                                                {
                                                    num33 = inClose[i];
                                                }
                                                else
                                                {
                                                    num33 = inOpen[i];
                                                }

                                                if (inClose[i] >= inOpen[i])
                                                {
                                                    num32 = inOpen[i];
                                                }
                                                else
                                                {
                                                    num32 = inClose[i];
                                                }

                                                num31 = inHigh[i] - num33 + (num32 - inLow[i]);
                                            }
                                            else
                                            {
                                                num31 = 0.0;
                                            }

                                            num34 = num31;
                                        }

                                        num35 = num34;
                                    }

                                    num36 = num35;
                                }

                                var num30 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                                    ? 2.0
                                    : 1.0;

                                if (inHigh[i] - num37 < Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num36 /
                                    num30)
                                {
                                    double num28;
                                    double num29;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num29 = inOpen[i];
                                    }
                                    else
                                    {
                                        num29 = inClose[i];
                                    }

                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                                    {
                                        num28 = shadowVeryShortPeriodTotal[0] /
                                                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                                    }
                                    else
                                    {
                                        double num27;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                        {
                                            num27 = Math.Abs(inClose[i] - inOpen[i]);
                                        }
                                        else
                                        {
                                            double num26;
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.HighLow)
                                            {
                                                num26 = inHigh[i] - inLow[i];
                                            }
                                            else
                                            {
                                                double num23;
                                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                    RangeType.Shadows)
                                                {
                                                    double num24;
                                                    double num25;
                                                    if (inClose[i] >= inOpen[i])
                                                    {
                                                        num25 = inClose[i];
                                                    }
                                                    else
                                                    {
                                                        num25 = inOpen[i];
                                                    }

                                                    if (inClose[i] >= inOpen[i])
                                                    {
                                                        num24 = inOpen[i];
                                                    }
                                                    else
                                                    {
                                                        num24 = inClose[i];
                                                    }

                                                    num23 = inHigh[i] - num25 + (num24 - inLow[i]);
                                                }
                                                else
                                                {
                                                    num23 = 0.0;
                                                }

                                                num26 = num23;
                                            }

                                            num27 = num26;
                                        }

                                        num28 = num27;
                                    }

                                    var num22 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows
                                        ? 2.0
                                        : 1.0;

                                    if (num29 - inLow[i] <
                                        Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num28 / num22 &&
                                        (inClose[i - 1] < inOpen[i - 1] && inLow[i] > inHigh[i - 1] ||
                                         inClose[i - 1] >= inOpen[i - 1] && inHigh[i] < inLow[i - 1]))
                                    {
                                        int num21;
                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num21 = 1;
                                        }
                                        else
                                        {
                                            num21 = -1;
                                        }

                                        outInteger[outIdx] = num21 * 100;
                                        outIdx++;
                                        goto Label_0CDA;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0CDA:
            var totIdx = 1;
            while (totIdx >= 0)
            {
                double num5;
                double num10;
                double num15;
                double num20;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num19;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs(inClose[bodyLongTrailingIdx - totIdx] - inOpen[bodyLongTrailingIdx - totIdx]);
                }
                else
                {
                    double num14;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[bodyLongTrailingIdx - totIdx] - inLow[bodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num11;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num12;
                            double num13;
                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num13 = inClose[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[bodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num12 = inOpen[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[bodyLongTrailingIdx - totIdx];
                            }

                            num11 = inHigh[bodyLongTrailingIdx - totIdx] - num13 + (num12 - inLow[bodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                bodyLongPeriodTotal[totIdx] += num20 - num15;
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
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0413;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlKicking(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int num68;
            var shadowVeryShortPeriodTotal = new decimal[2];
            var bodyLongPeriodTotal = new decimal[2];
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

            int lookbackTotal = CdlKickingLookback();
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
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num83;
                decimal num88;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num88 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num87;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num87 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num84;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num85;
                            decimal num86;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num86 = inClose[i - 1];
                            }
                            else
                            {
                                num86 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num85 = inOpen[i - 1];
                            }
                            else
                            {
                                num85 = inClose[i - 1];
                            }

                            num84 = inHigh[i - 1] - num86 + (num85 - inLow[i - 1]);
                        }
                        else
                        {
                            num84 = Decimal.Zero;
                        }

                        num87 = num84;
                    }

                    num88 = num87;
                }

                shadowVeryShortPeriodTotal[1] += num88;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num83 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num82;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num82 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num79;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num80;
                            decimal num81;
                            if (inClose[i] >= inOpen[i])
                            {
                                num81 = inClose[i];
                            }
                            else
                            {
                                num81 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num80 = inOpen[i];
                            }
                            else
                            {
                                num80 = inClose[i];
                            }

                            num79 = inHigh[i] - num81 + (num80 - inLow[i]);
                        }
                        else
                        {
                            num79 = Decimal.Zero;
                        }

                        num82 = num79;
                    }

                    num83 = num82;
                }

                shadowVeryShortPeriodTotal[0] += num83;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num73;
                decimal num78;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num78 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num77;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num77 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num74;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num75;
                            decimal num76;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num76 = inClose[i - 1];
                            }
                            else
                            {
                                num76 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num75 = inOpen[i - 1];
                            }
                            else
                            {
                                num75 = inClose[i - 1];
                            }

                            num74 = inHigh[i - 1] - num76 + (num75 - inLow[i - 1]);
                        }
                        else
                        {
                            num74 = Decimal.Zero;
                        }

                        num77 = num74;
                    }

                    num78 = num77;
                }

                bodyLongPeriodTotal[1] += num78;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num73 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num72;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num72 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num69;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num70;
                            decimal num71;
                            if (inClose[i] >= inOpen[i])
                            {
                                num71 = inClose[i];
                            }
                            else
                            {
                                num71 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num70 = inOpen[i];
                            }
                            else
                            {
                                num70 = inClose[i];
                            }

                            num69 = inHigh[i] - num71 + (num70 - inLow[i]);
                        }
                        else
                        {
                            num69 = Decimal.Zero;
                        }

                        num72 = num69;
                    }

                    num73 = num72;
                }

                bodyLongPeriodTotal[0] += num73;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_044B:
            if (inClose[i] >= inOpen[i])
            {
                num68 = 1;
            }
            else
            {
                num68 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) == -num68)
            {
                decimal num67;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num67 = bodyLongPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num66;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num66 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num65;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num65 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num62;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                decimal num63;
                                decimal num64;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num64 = inClose[i - 1];
                                }
                                else
                                {
                                    num64 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num63 = inOpen[i - 1];
                                }
                                else
                                {
                                    num63 = inClose[i - 1];
                                }

                                num62 = inHigh[i - 1] - num64 + (num63 - inLow[i - 1]);
                            }
                            else
                            {
                                num62 = Decimal.Zero;
                            }

                            num65 = num62;
                        }

                        num66 = num65;
                    }

                    num67 = num66;
                }

                var num61 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num67 / num61)
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

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                    {
                        num59 = shadowVeryShortPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num58;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                        {
                            num58 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num57;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                            {
                                num57 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num54;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num55;
                                    decimal num56;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num56 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num56 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num55 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num55 = inClose[i - 1];
                                    }

                                    num54 = inHigh[i - 1] - num56 + (num55 - inLow[i - 1]);
                                }
                                else
                                {
                                    num54 = Decimal.Zero;
                                }

                                num57 = num54;
                            }

                            num58 = num57;
                        }

                        num59 = num58;
                    }

                    var num53 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (inHigh[i - 1] - num60 <
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num59 / num53)
                    {
                        decimal num51;
                        decimal num52;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num52 = inOpen[i - 1];
                        }
                        else
                        {
                            num52 = inClose[i - 1];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num51 = shadowVeryShortPeriodTotal[1] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num50;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num50 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                decimal num49;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num49 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    decimal num46;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                        var num45 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2m
                            : 1m;

                        if (num52 - inLow[i - 1] <
                            (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num51 / num45)
                        {
                            decimal num44;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                            {
                                num44 = bodyLongPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                            }
                            else
                            {
                                decimal num43;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                                {
                                    num43 = Math.Abs(inClose[i] - inOpen[i]);
                                }
                                else
                                {
                                    decimal num42;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                                    {
                                        num42 = inHigh[i] - inLow[i];
                                    }
                                    else
                                    {
                                        decimal num39;
                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                        {
                                            decimal num40;
                                            decimal num41;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num41 = inClose[i];
                                            }
                                            else
                                            {
                                                num41 = inOpen[i];
                                            }

                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num40 = inOpen[i];
                                            }
                                            else
                                            {
                                                num40 = inClose[i];
                                            }

                                            num39 = inHigh[i] - num41 + (num40 - inLow[i]);
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

                            var num38 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (Math.Abs(inClose[i] - inOpen[i]) >
                                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num44 / num38)
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

                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                                {
                                    num36 = shadowVeryShortPeriodTotal[0] /
                                            Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                                }
                                else
                                {
                                    decimal num35;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                    {
                                        num35 = Math.Abs(inClose[i] - inOpen[i]);
                                    }
                                    else
                                    {
                                        decimal num34;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                        {
                                            num34 = inHigh[i] - inLow[i];
                                        }
                                        else
                                        {
                                            decimal num31;
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows)
                                            {
                                                decimal num32;
                                                decimal num33;
                                                if (inClose[i] >= inOpen[i])
                                                {
                                                    num33 = inClose[i];
                                                }
                                                else
                                                {
                                                    num33 = inOpen[i];
                                                }

                                                if (inClose[i] >= inOpen[i])
                                                {
                                                    num32 = inOpen[i];
                                                }
                                                else
                                                {
                                                    num32 = inClose[i];
                                                }

                                                num31 = inHigh[i] - num33 + (num32 - inLow[i]);
                                            }
                                            else
                                            {
                                                num31 = Decimal.Zero;
                                            }

                                            num34 = num31;
                                        }

                                        num35 = num34;
                                    }

                                    num36 = num35;
                                }

                                var num30 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                                    ? 2m
                                    : 1m;

                                if (inHigh[i] - num37 <
                                    (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num36 / num30)
                                {
                                    decimal num28;
                                    decimal num29;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num29 = inOpen[i];
                                    }
                                    else
                                    {
                                        num29 = inClose[i];
                                    }

                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                                    {
                                        num28 = shadowVeryShortPeriodTotal[0] /
                                                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                                    }
                                    else
                                    {
                                        decimal num27;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                        {
                                            num27 = Math.Abs(inClose[i] - inOpen[i]);
                                        }
                                        else
                                        {
                                            decimal num26;
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.HighLow)
                                            {
                                                num26 = inHigh[i] - inLow[i];
                                            }
                                            else
                                            {
                                                decimal num23;
                                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                    RangeType.Shadows)
                                                {
                                                    decimal num24;
                                                    decimal num25;
                                                    if (inClose[i] >= inOpen[i])
                                                    {
                                                        num25 = inClose[i];
                                                    }
                                                    else
                                                    {
                                                        num25 = inOpen[i];
                                                    }

                                                    if (inClose[i] >= inOpen[i])
                                                    {
                                                        num24 = inOpen[i];
                                                    }
                                                    else
                                                    {
                                                        num24 = inClose[i];
                                                    }

                                                    num23 = inHigh[i] - num25 + (num24 - inLow[i]);
                                                }
                                                else
                                                {
                                                    num23 = Decimal.Zero;
                                                }

                                                num26 = num23;
                                            }

                                            num27 = num26;
                                        }

                                        num28 = num27;
                                    }

                                    var num22 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows
                                        ? 2m
                                        : 1m;

                                    if (num29 - inLow[i] <
                                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num28 / num22 &&
                                        (inClose[i - 1] < inOpen[i - 1] && inLow[i] > inHigh[i - 1] ||
                                         inClose[i - 1] >= inOpen[i - 1] && inHigh[i] < inLow[i - 1]))
                                    {
                                        int num21;
                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num21 = 1;
                                        }
                                        else
                                        {
                                            num21 = -1;
                                        }

                                        outInteger[outIdx] = num21 * 100;
                                        outIdx++;
                                        goto Label_0D8C;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0D8C:
            var totIdx = 1;
            while (totIdx >= 0)
            {
                decimal num5;
                decimal num10;
                decimal num15;
                decimal num20;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num19;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs(inClose[bodyLongTrailingIdx - totIdx] - inOpen[bodyLongTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num14;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[bodyLongTrailingIdx - totIdx] - inLow[bodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num11;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num12;
                            decimal num13;
                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num13 = inClose[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[bodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num12 = inOpen[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[bodyLongTrailingIdx - totIdx];
                            }

                            num11 = inHigh[bodyLongTrailingIdx - totIdx] - num13 + (num12 - inLow[bodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = Decimal.Zero;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                bodyLongPeriodTotal[totIdx] += num20 - num15;
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
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_044B;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlKickingLookback()
        {
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod);

            return avgPeriod + 1;
        }
    }
}
