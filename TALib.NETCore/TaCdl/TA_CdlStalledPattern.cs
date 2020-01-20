using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlStalledPattern(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            var bodyLongPeriodTotal = new double[3];
            var nearPeriodTotal = new double[3];
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

            int lookbackTotal = CdlStalledPatternLookback();
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

            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            double bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num108;
                double num113;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num113 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num112;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num112 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num109;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num110;
                            double num111;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num111 = inClose[i - 2];
                            }
                            else
                            {
                                num111 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num110 = inOpen[i - 2];
                            }
                            else
                            {
                                num110 = inClose[i - 2];
                            }

                            num109 = inHigh[i - 2] - num111 + (num110 - inLow[i - 2]);
                        }
                        else
                        {
                            num109 = 0.0;
                        }

                        num112 = num109;
                    }

                    num113 = num112;
                }

                bodyLongPeriodTotal[2] += num113;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num108 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num107;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num107 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num104;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num105;
                            double num106;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num106 = inClose[i - 1];
                            }
                            else
                            {
                                num106 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num105 = inOpen[i - 1];
                            }
                            else
                            {
                                num105 = inClose[i - 1];
                            }

                            num104 = inHigh[i - 1] - num106 + (num105 - inLow[i - 1]);
                        }
                        else
                        {
                            num104 = 0.0;
                        }

                        num107 = num104;
                    }

                    num108 = num107;
                }

                bodyLongPeriodTotal[1] += num108;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                double num103;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num103 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num102;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num102 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num99;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num100;
                            double num101;
                            if (inClose[i] >= inOpen[i])
                            {
                                num101 = inClose[i];
                            }
                            else
                            {
                                num101 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num100 = inOpen[i];
                            }
                            else
                            {
                                num100 = inClose[i];
                            }

                            num99 = inHigh[i] - num101 + (num100 - inLow[i]);
                        }
                        else
                        {
                            num99 = 0.0;
                        }

                        num102 = num99;
                    }

                    num103 = num102;
                }

                bodyShortPeriodTotal += num103;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num98;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num98 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num97;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num97 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num94;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num95;
                            double num96;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num96 = inClose[i - 1];
                            }
                            else
                            {
                                num96 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num95 = inOpen[i - 1];
                            }
                            else
                            {
                                num95 = inClose[i - 1];
                            }

                            num94 = inHigh[i - 1] - num96 + (num95 - inLow[i - 1]);
                        }
                        else
                        {
                            num94 = 0.0;
                        }

                        num97 = num94;
                    }

                    num98 = num97;
                }

                shadowVeryShortPeriodTotal += num98;
                i++;
            }

            i = nearTrailingIdx;
            while (true)
            {
                double num88;
                double num93;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num93 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num92;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num92 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num89;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num90;
                            double num91;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num91 = inClose[i - 2];
                            }
                            else
                            {
                                num91 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num90 = inOpen[i - 2];
                            }
                            else
                            {
                                num90 = inClose[i - 2];
                            }

                            num89 = inHigh[i - 2] - num91 + (num90 - inLow[i - 2]);
                        }
                        else
                        {
                            num89 = 0.0;
                        }

                        num92 = num89;
                    }

                    num93 = num92;
                }

                nearPeriodTotal[2] += num93;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num88 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num87;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num87 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num84;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                nearPeriodTotal[1] += num88;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0656:
            if (inClose[i - 2] >= inOpen[i - 2] && inClose[i - 1] >= inOpen[i - 1] &&
                inClose[i] >= inOpen[i] && inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2])
            {
                double num83;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num83 = bodyLongPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num82;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num82 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num81;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num81 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num78;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                    num83 = num82;
                }

                var num77 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num83 / num77)
                {
                    double num76;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                    {
                        num76 = bodyLongPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                    }
                    else
                    {
                        double num75;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                        {
                            num75 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num74;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                            {
                                num74 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num71;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                {
                                    double num72;
                                    double num73;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num73 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num73 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num72 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num72 = inClose[i - 1];
                                    }

                                    num71 = inHigh[i - 1] - num73 + (num72 - inLow[i - 1]);
                                }
                                else
                                {
                                    num71 = 0.0;
                                }

                                num74 = num71;
                            }

                            num75 = num74;
                        }

                        num76 = num75;
                    }

                    var num70 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                        Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num76 / num70)
                    {
                        double num68;
                        double num69;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num69 = inClose[i - 1];
                        }
                        else
                        {
                            num69 = inOpen[i - 1];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num68 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            double num67;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num67 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                double num66;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num66 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num63;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                    {
                                        double num64;
                                        double num65;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num65 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num65 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num64 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num64 = inClose[i - 1];
                                        }

                                        num63 = inHigh[i - 1] - num65 + (num64 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num63 = 0.0;
                                    }

                                    num66 = num63;
                                }

                                num67 = num66;
                            }

                            num68 = num67;
                        }

                        var num62 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2.0
                            : 1.0;

                        if (inHigh[i - 1] - num69 <
                            Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num68 / num62 &&
                            inOpen[i - 1] > inOpen[i - 2])
                        {
                            double num61;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                            {
                                num61 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                            }
                            else
                            {
                                double num60;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                {
                                    num60 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                }
                                else
                                {
                                    double num59;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                    {
                                        num59 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        double num56;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                        {
                                            double num57;
                                            double num58;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num58 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num58 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num57 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num57 = inClose[i - 2];
                                            }

                                            num56 = inHigh[i - 2] - num58 + (num57 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num56 = 0.0;
                                        }

                                        num59 = num56;
                                    }

                                    num60 = num59;
                                }

                                num61 = num60;
                            }

                            var num55 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                            if (inOpen[i - 1] <=
                                inClose[i - 2] + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num61 / num55)
                            {
                                double num54;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                                {
                                    num54 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                                }
                                else
                                {
                                    double num53;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                                    {
                                        num53 = Math.Abs(inClose[i] - inOpen[i]);
                                    }
                                    else
                                    {
                                        double num52;
                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                        {
                                            num52 = inHigh[i] - inLow[i];
                                        }
                                        else
                                        {
                                            double num49;
                                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                                                num49 = inHigh[i] - num51 + (num50 - inLow[i]);
                                            }
                                            else
                                            {
                                                num49 = 0.0;
                                            }

                                            num52 = num49;
                                        }

                                        num53 = num52;
                                    }

                                    num54 = num53;
                                }

                                var num48 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows
                                    ? 2.0
                                    : 1.0;

                                if (Math.Abs(inClose[i] - inOpen[i]) <
                                    Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num54 / num48)
                                {
                                    double num47;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                                    {
                                        num47 = nearPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                                    }
                                    else
                                    {
                                        double num46;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                        {
                                            num46 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                        }
                                        else
                                        {
                                            double num45;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                            {
                                                num45 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                double num42;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                                {
                                                    double num43;
                                                    double num44;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num44 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num44 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num43 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num43 = inClose[i - 1];
                                                    }

                                                    num42 = inHigh[i - 1] - num44 + (num43 - inLow[i - 1]);
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

                                    var num41 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows
                                        ? 2.0
                                        : 1.0;

                                    if (inOpen[i] >= inClose[i - 1] - Math.Abs(inClose[i] - inOpen[i]) -
                                        Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num47 / num41)
                                    {
                                        outInteger[outIdx] = -100;
                                        outIdx++;
                                        goto Label_0F20;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0F20:
            var totIdx = 2;
            while (totIdx >= 1)
            {
                double num25;
                double num30;
                double num35;
                double num40;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num39;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num36;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num37;
                            double num38;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num38 = inClose[i - totIdx];
                            }
                            else
                            {
                                num38 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num37 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num37 = inClose[i - totIdx];
                            }

                            num36 = inHigh[i - totIdx] - num38 + (num37 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num36 = 0.0;
                        }

                        num39 = num36;
                    }

                    num40 = num39;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs(inClose[bodyLongTrailingIdx - totIdx] - inOpen[bodyLongTrailingIdx - totIdx]);
                }
                else
                {
                    double num34;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[bodyLongTrailingIdx - totIdx] - inLow[bodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num31;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num32;
                            double num33;
                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num33 = inClose[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[bodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num32 = inOpen[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[bodyLongTrailingIdx - totIdx];
                            }

                            num31 = inHigh[bodyLongTrailingIdx - totIdx] - num33 + (num32 - inLow[bodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = 0.0;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                bodyLongPeriodTotal[totIdx] += num40 - num35;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num29;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num27;
                            double num28;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num28 = inClose[i - totIdx];
                            }
                            else
                            {
                                num28 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num27 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num27 = inClose[i - totIdx];
                            }

                            num26 = inHigh[i - totIdx] - num28 + (num27 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num26 = 0.0;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs(inClose[nearTrailingIdx - totIdx] - inOpen[nearTrailingIdx - totIdx]);
                }
                else
                {
                    double num24;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[nearTrailingIdx - totIdx] - inLow[nearTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num21;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num22;
                            double num23;
                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num23 = inClose[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num23 = inOpen[nearTrailingIdx - totIdx];
                            }

                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num22 = inOpen[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num22 = inClose[nearTrailingIdx - totIdx];
                            }

                            num21 = inHigh[nearTrailingIdx - totIdx] - num23 + (num22 - inLow[nearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num21 = 0.0;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                nearPeriodTotal[totIdx] += num30 - num25;
                totIdx--;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                        num16 = inHigh[i] - num18 + (num17 - inLow[i]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyShortTrailingIdx] - inOpen[bodyShortTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyShortTrailingIdx] - inLow[bodyShortTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num13 = inClose[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyShortTrailingIdx];
                        }

                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num12 = inOpen[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyShortTrailingIdx];
                        }

                        num11 = inHigh[bodyShortTrailingIdx] - num13 + (num12 - inLow[bodyShortTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyShortPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx - 1] - inOpen[shadowVeryShortTrailingIdx - 1]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryShortTrailingIdx - 1] - inLow[shadowVeryShortTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[shadowVeryShortTrailingIdx - 1] >= inOpen[shadowVeryShortTrailingIdx - 1])
                        {
                            num3 = inClose[shadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryShortTrailingIdx - 1];
                        }

                        if (inClose[shadowVeryShortTrailingIdx - 1] >= inOpen[shadowVeryShortTrailingIdx - 1])
                        {
                            num2 = inOpen[shadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryShortTrailingIdx - 1];
                        }

                        num = inHigh[shadowVeryShortTrailingIdx - 1] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryShortPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            bodyShortTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            nearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0656;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlStalledPattern(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            var bodyLongPeriodTotal = new decimal[3];
            var nearPeriodTotal = new decimal[3];
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

            int lookbackTotal = CdlStalledPatternLookback();
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

            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            decimal bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num108;
                decimal num113;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num113 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num112;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num112 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num109;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num110;
                            decimal num111;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num111 = inClose[i - 2];
                            }
                            else
                            {
                                num111 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num110 = inOpen[i - 2];
                            }
                            else
                            {
                                num110 = inClose[i - 2];
                            }

                            num109 = inHigh[i - 2] - num111 + (num110 - inLow[i - 2]);
                        }
                        else
                        {
                            num109 = Decimal.Zero;
                        }

                        num112 = num109;
                    }

                    num113 = num112;
                }

                bodyLongPeriodTotal[2] += num113;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num108 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num107;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num107 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num104;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num105;
                            decimal num106;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num106 = inClose[i - 1];
                            }
                            else
                            {
                                num106 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num105 = inOpen[i - 1];
                            }
                            else
                            {
                                num105 = inClose[i - 1];
                            }

                            num104 = inHigh[i - 1] - num106 + (num105 - inLow[i - 1]);
                        }
                        else
                        {
                            num104 = Decimal.Zero;
                        }

                        num107 = num104;
                    }

                    num108 = num107;
                }

                bodyLongPeriodTotal[1] += num108;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num103;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num103 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num102;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num102 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num99;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num100;
                            decimal num101;
                            if (inClose[i] >= inOpen[i])
                            {
                                num101 = inClose[i];
                            }
                            else
                            {
                                num101 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num100 = inOpen[i];
                            }
                            else
                            {
                                num100 = inClose[i];
                            }

                            num99 = inHigh[i] - num101 + (num100 - inLow[i]);
                        }
                        else
                        {
                            num99 = Decimal.Zero;
                        }

                        num102 = num99;
                    }

                    num103 = num102;
                }

                bodyShortPeriodTotal += num103;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num98;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num98 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num97;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num97 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num94;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num95;
                            decimal num96;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num96 = inClose[i - 1];
                            }
                            else
                            {
                                num96 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num95 = inOpen[i - 1];
                            }
                            else
                            {
                                num95 = inClose[i - 1];
                            }

                            num94 = inHigh[i - 1] - num96 + (num95 - inLow[i - 1]);
                        }
                        else
                        {
                            num94 = Decimal.Zero;
                        }

                        num97 = num94;
                    }

                    num98 = num97;
                }

                shadowVeryShortPeriodTotal += num98;
                i++;
            }

            i = nearTrailingIdx;
            while (true)
            {
                decimal num88;
                decimal num93;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num93 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num92;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num92 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num89;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num90;
                            decimal num91;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num91 = inClose[i - 2];
                            }
                            else
                            {
                                num91 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num90 = inOpen[i - 2];
                            }
                            else
                            {
                                num90 = inClose[i - 2];
                            }

                            num89 = inHigh[i - 2] - num91 + (num90 - inLow[i - 2]);
                        }
                        else
                        {
                            num89 = Decimal.Zero;
                        }

                        num92 = num89;
                    }

                    num93 = num92;
                }

                nearPeriodTotal[2] += num93;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num88 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num87;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num87 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num84;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                nearPeriodTotal[1] += num88;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_06AA:
            if (inClose[i - 2] >= inOpen[i - 2] && inClose[i - 1] >= inOpen[i - 1] &&
                inClose[i] >= inOpen[i] && inClose[i] > inClose[i - 1] && inClose[i - 1] > inClose[i - 2])
            {
                decimal num83;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num83 = bodyLongPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num82;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num82 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num81;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num81 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num78;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                    num83 = num82;
                }

                var num77 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num83 / num77)
                {
                    decimal num76;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                    {
                        num76 = bodyLongPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                    }
                    else
                    {
                        decimal num75;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                        {
                            num75 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num74;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                            {
                                num74 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num71;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                {
                                    decimal num72;
                                    decimal num73;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num73 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num73 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num72 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num72 = inClose[i - 1];
                                    }

                                    num71 = inHigh[i - 1] - num73 + (num72 - inLow[i - 1]);
                                }
                                else
                                {
                                    num71 = Decimal.Zero;
                                }

                                num74 = num71;
                            }

                            num75 = num74;
                        }

                        num76 = num75;
                    }

                    var num70 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num76 / num70)
                    {
                        decimal num68;
                        decimal num69;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num69 = inClose[i - 1];
                        }
                        else
                        {
                            num69 = inOpen[i - 1];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num68 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num67;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num67 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                decimal num66;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num66 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    decimal num63;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                                    {
                                        decimal num64;
                                        decimal num65;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num65 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num65 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num64 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num64 = inClose[i - 1];
                                        }

                                        num63 = inHigh[i - 1] - num65 + (num64 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num63 = Decimal.Zero;
                                    }

                                    num66 = num63;
                                }

                                num67 = num66;
                            }

                            num68 = num67;
                        }

                        var num62 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2m
                            : 1m;

                        if (inHigh[i - 1] - num69 <
                            (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num68 / num62 &&
                            inOpen[i - 1] > inOpen[i - 2])
                        {
                            decimal num61;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                            {
                                num61 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                            }
                            else
                            {
                                decimal num60;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                {
                                    num60 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                }
                                else
                                {
                                    decimal num59;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                    {
                                        num59 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        decimal num56;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                        {
                                            decimal num57;
                                            decimal num58;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num58 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num58 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num57 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num57 = inClose[i - 2];
                                            }

                                            num56 = inHigh[i - 2] - num58 + (num57 - inLow[i - 2]);
                                        }
                                        else
                                        {
                                            num56 = Decimal.Zero;
                                        }

                                        num59 = num56;
                                    }

                                    num60 = num59;
                                }

                                num61 = num60;
                            }

                            var num55 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (inOpen[i - 1] <=
                                inClose[i - 2] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num61 / num55)
                            {
                                decimal num54;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                                {
                                    num54 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                                }
                                else
                                {
                                    decimal num53;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                                    {
                                        num53 = Math.Abs(inClose[i] - inOpen[i]);
                                    }
                                    else
                                    {
                                        decimal num52;
                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                        {
                                            num52 = inHigh[i] - inLow[i];
                                        }
                                        else
                                        {
                                            decimal num49;
                                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                            {
                                                decimal num50;
                                                decimal num51;
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

                                                num49 = inHigh[i] - num51 + (num50 - inLow[i]);
                                            }
                                            else
                                            {
                                                num49 = Decimal.Zero;
                                            }

                                            num52 = num49;
                                        }

                                        num53 = num52;
                                    }

                                    num54 = num53;
                                }

                                var num48 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows
                                    ? 2m
                                    : 1m;

                                if (Math.Abs(inClose[i] - inOpen[i]) <
                                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num54 / num48)
                                {
                                    decimal num47;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                                    {
                                        num47 = nearPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                                    }
                                    else
                                    {
                                        decimal num46;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                        {
                                            num46 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                        }
                                        else
                                        {
                                            decimal num45;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                            {
                                                num45 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                decimal num42;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                                {
                                                    decimal num43;
                                                    decimal num44;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num44 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num44 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num43 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num43 = inClose[i - 1];
                                                    }

                                                    num42 = inHigh[i - 1] - num44 + (num43 - inLow[i - 1]);
                                                }
                                                else
                                                {
                                                    num42 = Decimal.Zero;
                                                }

                                                num45 = num42;
                                            }

                                            num46 = num45;
                                        }

                                        num47 = num46;
                                    }

                                    var num41 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows
                                        ? 2m
                                        : 1m;

                                    if (inOpen[i] >= inClose[i - 1] - Math.Abs(inClose[i] - inOpen[i]) -
                                        (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num47 / num41)
                                    {
                                        outInteger[outIdx] = -100;
                                        outIdx++;
                                        goto Label_0FEC;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0FEC:
            var totIdx = 2;
            while (totIdx >= 1)
            {
                decimal num25;
                decimal num30;
                decimal num35;
                decimal num40;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num40 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num39;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num39 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num36;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num37;
                            decimal num38;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num38 = inClose[i - totIdx];
                            }
                            else
                            {
                                num38 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num37 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num37 = inClose[i - totIdx];
                            }

                            num36 = inHigh[i - totIdx] - num38 + (num37 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num36 = Decimal.Zero;
                        }

                        num39 = num36;
                    }

                    num40 = num39;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num35 = Math.Abs(inClose[bodyLongTrailingIdx - totIdx] - inOpen[bodyLongTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num34;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num34 = inHigh[bodyLongTrailingIdx - totIdx] - inLow[bodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num31;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num32;
                            decimal num33;
                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num33 = inClose[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num33 = inOpen[bodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num32 = inOpen[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num32 = inClose[bodyLongTrailingIdx - totIdx];
                            }

                            num31 = inHigh[bodyLongTrailingIdx - totIdx] - num33 + (num32 - inLow[bodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num31 = Decimal.Zero;
                        }

                        num34 = num31;
                    }

                    num35 = num34;
                }

                bodyLongPeriodTotal[totIdx] += num40 - num35;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num29;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num27;
                            decimal num28;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num28 = inClose[i - totIdx];
                            }
                            else
                            {
                                num28 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num27 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num27 = inClose[i - totIdx];
                            }

                            num26 = inHigh[i - totIdx] - num28 + (num27 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num26 = Decimal.Zero;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs(inClose[nearTrailingIdx - totIdx] - inOpen[nearTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num24;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[nearTrailingIdx - totIdx] - inLow[nearTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num21;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num22;
                            decimal num23;
                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num23 = inClose[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num23 = inOpen[nearTrailingIdx - totIdx];
                            }

                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num22 = inOpen[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num22 = inClose[nearTrailingIdx - totIdx];
                            }

                            num21 = inHigh[nearTrailingIdx - totIdx] - num23 + (num22 - inLow[nearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num21 = Decimal.Zero;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                nearPeriodTotal[totIdx] += num30 - num25;
                totIdx--;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num17;
                        decimal num18;
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

                        num16 = inHigh[i] - num18 + (num17 - inLow[i]);
                    }
                    else
                    {
                        num16 = Decimal.Zero;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyShortTrailingIdx] - inOpen[bodyShortTrailingIdx]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyShortTrailingIdx] - inLow[bodyShortTrailingIdx];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num13 = inClose[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyShortTrailingIdx];
                        }

                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num12 = inOpen[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyShortTrailingIdx];
                        }

                        num11 = inHigh[bodyShortTrailingIdx] - num13 + (num12 - inLow[bodyShortTrailingIdx]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyShortPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx - 1] - inOpen[shadowVeryShortTrailingIdx - 1]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryShortTrailingIdx - 1] - inLow[shadowVeryShortTrailingIdx - 1];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[shadowVeryShortTrailingIdx - 1] >= inOpen[shadowVeryShortTrailingIdx - 1])
                        {
                            num3 = inClose[shadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryShortTrailingIdx - 1];
                        }

                        if (inClose[shadowVeryShortTrailingIdx - 1] >= inOpen[shadowVeryShortTrailingIdx - 1])
                        {
                            num2 = inOpen[shadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryShortTrailingIdx - 1];
                        }

                        num = inHigh[shadowVeryShortTrailingIdx - 1] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx - 1]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryShortPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            bodyShortTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            nearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_06AA;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlStalledPatternLookback()
        {
            int avgPeriod = Math.Max(
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod),
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod)
            );

            return avgPeriod + 2;
        }
    }
}
