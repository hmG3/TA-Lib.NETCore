using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3StarsInSouth(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num25;
            double num30;
            double num35;
            double num40;
            var shadowVeryShortPeriodTotal = new double[2];
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

            int lookbackTotal = Cdl3StarsInSouthLookback();
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

            double bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            double shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            double bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num111;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num111 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num110;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num110 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num107;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num108;
                            double num109;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num109 = inClose[i - 2];
                            }
                            else
                            {
                                num109 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num108 = inOpen[i - 2];
                            }
                            else
                            {
                                num108 = inClose[i - 2];
                            }

                            num107 = inHigh[i - 2] - num109 + (num108 - inLow[i - 2]);
                        }
                        else
                        {
                            num107 = 0.0;
                        }

                        num110 = num107;
                    }

                    num111 = num110;
                }

                bodyLongPeriodTotal += num111;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                double num106;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num106 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num105;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num105 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num102;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            double num103;
                            double num104;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num104 = inClose[i - 2];
                            }
                            else
                            {
                                num104 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num103 = inOpen[i - 2];
                            }
                            else
                            {
                                num103 = inClose[i - 2];
                            }

                            num102 = inHigh[i - 2] - num104 + (num103 - inLow[i - 2]);
                        }
                        else
                        {
                            num102 = 0.0;
                        }

                        num105 = num102;
                    }

                    num106 = num105;
                }

                shadowLongPeriodTotal += num106;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num96;
                double num101;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num101 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num100;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num100 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num97;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num98;
                            double num99;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num99 = inClose[i - 1];
                            }
                            else
                            {
                                num99 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num98 = inOpen[i - 1];
                            }
                            else
                            {
                                num98 = inClose[i - 1];
                            }

                            num97 = inHigh[i - 1] - num99 + (num98 - inLow[i - 1]);
                        }
                        else
                        {
                            num97 = 0.0;
                        }

                        num100 = num97;
                    }

                    num101 = num100;
                }

                shadowVeryShortPeriodTotal[1] += num101;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num96 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num95;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num95 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num92;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            double num93;
                            double num94;
                            if (inClose[i] >= inOpen[i])
                            {
                                num94 = inClose[i];
                            }
                            else
                            {
                                num94 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num93 = inOpen[i];
                            }
                            else
                            {
                                num93 = inClose[i];
                            }

                            num92 = inHigh[i] - num94 + (num93 - inLow[i]);
                        }
                        else
                        {
                            num92 = 0.0;
                        }

                        num95 = num92;
                    }

                    num96 = num95;
                }

                shadowVeryShortPeriodTotal[0] += num96;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                double num91;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num91 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num90;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num90 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num87;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num88;
                            double num89;
                            if (inClose[i] >= inOpen[i])
                            {
                                num89 = inClose[i];
                            }
                            else
                            {
                                num89 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num88 = inOpen[i];
                            }
                            else
                            {
                                num88 = inClose[i];
                            }

                            num87 = inHigh[i] - num89 + (num88 - inLow[i]);
                        }
                        else
                        {
                            num87 = 0.0;
                        }

                        num90 = num87;
                    }

                    num91 = num90;
                }

                bodyShortPeriodTotal += num91;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_052C:
            if (inClose[i - 2] < inOpen[i - 2] && inClose[i - 1] < inOpen[i - 1] && inClose[i] < inOpen[i])
            {
                double num86;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num86 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num85;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num85 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num84;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num84 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num81;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                double num82;
                                double num83;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num83 = inClose[i - 2];
                                }
                                else
                                {
                                    num83 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num82 = inOpen[i - 2];
                                }
                                else
                                {
                                    num82 = inClose[i - 2];
                                }

                                num81 = inHigh[i - 2] - num83 + (num82 - inLow[i - 2]);
                            }
                            else
                            {
                                num81 = 0.0;
                            }

                            num84 = num81;
                        }

                        num85 = num84;
                    }

                    num86 = num85;
                }

                var num80 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num86 / num80)
                {
                    double num78;
                    double num79;
                    if (inClose[i - 2] >= inOpen[i - 2])
                    {
                        num79 = inOpen[i - 2];
                    }
                    else
                    {
                        num79 = inClose[i - 2];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                    {
                        num78 = shadowLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                    }
                    else
                    {
                        double num77;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                        {
                            num77 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                        }
                        else
                        {
                            double num76;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                            {
                                num76 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                double num73;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                                {
                                    double num74;
                                    double num75;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num75 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num75 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num74 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num74 = inClose[i - 2];
                                    }

                                    num73 = inHigh[i - 2] - num75 + (num74 - inLow[i - 2]);
                                }
                                else
                                {
                                    num73 = 0.0;
                                }

                                num76 = num73;
                            }

                            num77 = num76;
                        }

                        num78 = num77;
                    }

                    var num72 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (num79 - inLow[i - 2] > Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num78 / num72 &&
                        Math.Abs(inClose[i - 1] - inOpen[i - 1]) < Math.Abs(inClose[i - 2] - inOpen[i - 2]) &&
                        inOpen[i - 1] > inClose[i - 2] && inOpen[i - 1] <= inHigh[i - 2] &&
                        inLow[i - 1] < inClose[i - 2] && inLow[i - 1] >= inLow[i - 2])
                    {
                        double num70;
                        double num71;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num71 = inOpen[i - 1];
                        }
                        else
                        {
                            num71 = inClose[i - 1];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num70 = shadowVeryShortPeriodTotal[1] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            double num69;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num69 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                double num68;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num68 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num65;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                            num70 = num69;
                        }

                        var num64 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2.0
                            : 1.0;

                        if (num71 - inLow[i - 1] > Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num70 / num64)
                        {
                            double num63;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                            {
                                num63 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                            }
                            else
                            {
                                double num62;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                                {
                                    num62 = Math.Abs(inClose[i] - inOpen[i]);
                                }
                                else
                                {
                                    double num61;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                    {
                                        num61 = inHigh[i] - inLow[i];
                                    }
                                    else
                                    {
                                        double num58;
                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                        {
                                            double num59;
                                            double num60;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num60 = inClose[i];
                                            }
                                            else
                                            {
                                                num60 = inOpen[i];
                                            }

                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num59 = inOpen[i];
                                            }
                                            else
                                            {
                                                num59 = inClose[i];
                                            }

                                            num58 = inHigh[i] - num60 + (num59 - inLow[i]);
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

                            var num57 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows
                                ? 2.0
                                : 1.0;

                            if (Math.Abs(inClose[i] - inOpen[i]) <
                                Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num63 / num57)
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
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows)
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

                                if (num56 - inLow[i] <
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num55 / num49)
                                {
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

                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                                    {
                                        num47 = shadowVeryShortPeriodTotal[0] /
                                                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                                    }
                                    else
                                    {
                                        double num46;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                        {
                                            num46 = Math.Abs(inClose[i] - inOpen[i]);
                                        }
                                        else
                                        {
                                            double num45;
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.HighLow)
                                            {
                                                num45 = inHigh[i] - inLow[i];
                                            }
                                            else
                                            {
                                                double num42;
                                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                    RangeType.Shadows)
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

                                                    num42 = inHigh[i] - num44 + (num43 - inLow[i]);
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

                                    var num41 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows
                                        ? 2.0
                                        : 1.0;

                                    if (inHigh[i] - num48 <
                                        Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num47 / num41 &&
                                        inLow[i] > inLow[i - 1] && inHigh[i] < inHigh[i - 1])
                                    {
                                        outInteger[outIdx] = 100;
                                        outIdx++;
                                        goto Label_0E31;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0E31:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num40 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                double num39;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num39 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num36;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num35 = Math.Abs(inClose[bodyLongTrailingIdx - 2] - inOpen[bodyLongTrailingIdx - 2]);
            }
            else
            {
                double num34;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num34 = inHigh[bodyLongTrailingIdx - 2] - inLow[bodyLongTrailingIdx - 2];
                }
                else
                {
                    double num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num32;
                        double num33;
                        if (inClose[bodyLongTrailingIdx - 2] >= inOpen[bodyLongTrailingIdx - 2])
                        {
                            num33 = inClose[bodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num33 = inOpen[bodyLongTrailingIdx - 2];
                        }

                        if (inClose[bodyLongTrailingIdx - 2] >= inOpen[bodyLongTrailingIdx - 2])
                        {
                            num32 = inOpen[bodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num32 = inClose[bodyLongTrailingIdx - 2];
                        }

                        num31 = inHigh[bodyLongTrailingIdx - 2] - num33 + (num32 - inLow[bodyLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num31 = 0.0;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            bodyLongPeriodTotal += num40 - num35;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                double num29;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
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

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num27 = inOpen[i - 2];
                        }
                        else
                        {
                            num27 = inClose[i - 2];
                        }

                        num26 = inHigh[i - 2] - num28 + (num27 - inLow[i - 2]);
                    }
                    else
                    {
                        num26 = 0.0;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[shadowLongTrailingIdx - 2] - inOpen[shadowLongTrailingIdx - 2]);
            }
            else
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[shadowLongTrailingIdx - 2] - inLow[shadowLongTrailingIdx - 2];
                }
                else
                {
                    double num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[shadowLongTrailingIdx - 2] >= inOpen[shadowLongTrailingIdx - 2])
                        {
                            num23 = inClose[shadowLongTrailingIdx - 2];
                        }
                        else
                        {
                            num23 = inOpen[shadowLongTrailingIdx - 2];
                        }

                        if (inClose[shadowLongTrailingIdx - 2] >= inOpen[shadowLongTrailingIdx - 2])
                        {
                            num22 = inOpen[shadowLongTrailingIdx - 2];
                        }
                        else
                        {
                            num22 = inClose[shadowLongTrailingIdx - 2];
                        }

                        num21 = inHigh[shadowLongTrailingIdx - 2] - num23 + (num22 - inLow[shadowLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            shadowLongPeriodTotal += num30 - num25;
            for (var totIdx = 1; totIdx >= 0; totIdx--)
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
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                        num6 = inHigh[i] - num8 + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyShortTrailingIdx] - inOpen[bodyShortTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyShortTrailingIdx] - inLow[bodyShortTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num3 = inClose[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyShortTrailingIdx];
                        }

                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num2 = inOpen[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyShortTrailingIdx];
                        }

                        num = inHigh[bodyShortTrailingIdx] - num3 + (num2 - inLow[bodyShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyShortPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_052C;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3StarsInSouth(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num25;
            decimal num30;
            decimal num35;
            decimal num40;
            var shadowVeryShortPeriodTotal = new decimal[2];
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

            int lookbackTotal = Cdl3StarsInSouthLookback();
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

            decimal bodyLongPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            decimal shadowLongPeriodTotal = default;
            int shadowLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            decimal bodyShortPeriodTotal = default;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num111;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num111 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num110;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num110 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num107;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num108;
                            decimal num109;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num109 = inClose[i - 2];
                            }
                            else
                            {
                                num109 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num108 = inOpen[i - 2];
                            }
                            else
                            {
                                num108 = inClose[i - 2];
                            }

                            num107 = inHigh[i - 2] - num109 + (num108 - inLow[i - 2]);
                        }
                        else
                        {
                            num107 = Decimal.Zero;
                        }

                        num110 = num107;
                    }

                    num111 = num110;
                }

                bodyLongPeriodTotal += num111;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                decimal num106;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                {
                    num106 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num105;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                    {
                        num105 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num102;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                        {
                            decimal num103;
                            decimal num104;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num104 = inClose[i - 2];
                            }
                            else
                            {
                                num104 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num103 = inOpen[i - 2];
                            }
                            else
                            {
                                num103 = inClose[i - 2];
                            }

                            num102 = inHigh[i - 2] - num104 + (num103 - inLow[i - 2]);
                        }
                        else
                        {
                            num102 = Decimal.Zero;
                        }

                        num105 = num102;
                    }

                    num106 = num105;
                }

                shadowLongPeriodTotal += num106;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num96;
                decimal num101;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num101 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num100;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num100 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num97;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num98;
                            decimal num99;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num99 = inClose[i - 1];
                            }
                            else
                            {
                                num99 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num98 = inOpen[i - 1];
                            }
                            else
                            {
                                num98 = inClose[i - 1];
                            }

                            num97 = inHigh[i - 1] - num99 + (num98 - inLow[i - 1]);
                        }
                        else
                        {
                            num97 = Decimal.Zero;
                        }

                        num100 = num97;
                    }

                    num101 = num100;
                }

                shadowVeryShortPeriodTotal[1] += num101;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num96 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num95;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num95 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num92;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                        {
                            decimal num93;
                            decimal num94;
                            if (inClose[i] >= inOpen[i])
                            {
                                num94 = inClose[i];
                            }
                            else
                            {
                                num94 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num93 = inOpen[i];
                            }
                            else
                            {
                                num93 = inClose[i];
                            }

                            num92 = inHigh[i] - num94 + (num93 - inLow[i]);
                        }
                        else
                        {
                            num92 = Decimal.Zero;
                        }

                        num95 = num92;
                    }

                    num96 = num95;
                }

                shadowVeryShortPeriodTotal[0] += num96;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num91;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num91 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num90;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num90 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num87;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num88;
                            decimal num89;
                            if (inClose[i] >= inOpen[i])
                            {
                                num89 = inClose[i];
                            }
                            else
                            {
                                num89 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num88 = inOpen[i];
                            }
                            else
                            {
                                num88 = inClose[i];
                            }

                            num87 = inHigh[i] - num89 + (num88 - inLow[i]);
                        }
                        else
                        {
                            num87 = Decimal.Zero;
                        }

                        num90 = num87;
                    }

                    num91 = num90;
                }

                bodyShortPeriodTotal += num91;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0572:
            if (inClose[i - 2] < inOpen[i - 2] && inClose[i - 1] < inOpen[i - 1] && inClose[i] < inOpen[i])
            {
                decimal num86;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num86 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num85;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num85 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num84;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num84 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num81;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                decimal num82;
                                decimal num83;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num83 = inClose[i - 2];
                                }
                                else
                                {
                                    num83 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num82 = inOpen[i - 2];
                                }
                                else
                                {
                                    num82 = inClose[i - 2];
                                }

                                num81 = inHigh[i - 2] - num83 + (num82 - inLow[i - 2]);
                            }
                            else
                            {
                                num81 = Decimal.Zero;
                            }

                            num84 = num81;
                        }

                        num85 = num84;
                    }

                    num86 = num85;
                }

                var num80 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num86 / num80)
                {
                    decimal num78;
                    decimal num79;
                    if (inClose[i - 2] >= inOpen[i - 2])
                    {
                        num79 = inOpen[i - 2];
                    }
                    else
                    {
                        num79 = inClose[i - 2];
                    }

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod != 0)
                    {
                        num78 = shadowLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod;
                    }
                    else
                    {
                        decimal num77;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
                        {
                            num77 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                        }
                        else
                        {
                            decimal num76;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                            {
                                num76 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                decimal num73;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                                {
                                    decimal num74;
                                    decimal num75;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num75 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num75 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num74 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num74 = inClose[i - 2];
                                    }

                                    num73 = inHigh[i - 2] - num75 + (num74 - inLow[i - 2]);
                                }
                                else
                                {
                                    num73 = Decimal.Zero;
                                }

                                num76 = num73;
                            }

                            num77 = num76;
                        }

                        num78 = num77;
                    }

                    var num72 = Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (num79 - inLow[i - 2] >
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowLong].Factor * num78 / num72 &&
                        Math.Abs(inClose[i - 1] - inOpen[i - 1]) < Math.Abs(inClose[i - 2] - inOpen[i - 2]) &&
                        inOpen[i - 1] > inClose[i - 2] && inOpen[i - 1] <= inHigh[i - 2] &&
                        inLow[i - 1] < inClose[i - 2] && inLow[i - 1] >= inLow[i - 2])
                    {
                        decimal num70;
                        decimal num71;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num71 = inOpen[i - 1];
                        }
                        else
                        {
                            num71 = inClose[i - 1];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                        {
                            num70 = shadowVeryShortPeriodTotal[1] /
                                    Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num69;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                            {
                                num69 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                decimal num68;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                                {
                                    num68 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    decimal num65;
                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                            num70 = num69;
                        }

                        var num64 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows
                            ? 2m
                            : 1m;

                        if (num71 - inLow[i - 1] >
                            (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num70 / num64)
                        {
                            decimal num63;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                            {
                                num63 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                            }
                            else
                            {
                                decimal num62;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                                {
                                    num62 = Math.Abs(inClose[i] - inOpen[i]);
                                }
                                else
                                {
                                    decimal num61;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                    {
                                        num61 = inHigh[i] - inLow[i];
                                    }
                                    else
                                    {
                                        decimal num58;
                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                        {
                                            decimal num59;
                                            decimal num60;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num60 = inClose[i];
                                            }
                                            else
                                            {
                                                num60 = inOpen[i];
                                            }

                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num59 = inOpen[i];
                                            }
                                            else
                                            {
                                                num59 = inClose[i];
                                            }

                                            num58 = inHigh[i] - num60 + (num59 - inLow[i]);
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

                            var num57 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (Math.Abs(inClose[i] - inOpen[i]) <
                                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num63 / num57)
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
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows)
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

                                if (num56 - inLow[i] < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor *
                                    num55 / num49)
                                {
                                    decimal num47;
                                    decimal num48;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num48 = inClose[i];
                                    }
                                    else
                                    {
                                        num48 = inOpen[i];
                                    }

                                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                                    {
                                        num47 = shadowVeryShortPeriodTotal[0] /
                                                Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                                    }
                                    else
                                    {
                                        decimal num46;
                                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                                        {
                                            num46 = Math.Abs(inClose[i] - inOpen[i]);
                                        }
                                        else
                                        {
                                            decimal num45;
                                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.HighLow)
                                            {
                                                num45 = inHigh[i] - inLow[i];
                                            }
                                            else
                                            {
                                                decimal num42;
                                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                    RangeType.Shadows)
                                                {
                                                    decimal num43;
                                                    decimal num44;
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

                                                    num42 = inHigh[i] - num44 + (num43 - inLow[i]);
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

                                    var num41 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType ==
                                                RangeType.Shadows
                                        ? 2m
                                        : 1m;

                                    if (inHigh[i] - num48 <
                                        (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num47 / num41 &&
                                        inLow[i] > inLow[i - 1] && inHigh[i] < inHigh[i - 1])
                                    {
                                        outInteger[outIdx] = 100;
                                        outIdx++;
                                        goto Label_0EFD;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0EFD:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num40 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                decimal num39;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num39 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    decimal num36;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num35 = Math.Abs(inClose[bodyLongTrailingIdx - 2] - inOpen[bodyLongTrailingIdx - 2]);
            }
            else
            {
                decimal num34;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num34 = inHigh[bodyLongTrailingIdx - 2] - inLow[bodyLongTrailingIdx - 2];
                }
                else
                {
                    decimal num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num32;
                        decimal num33;
                        if (inClose[bodyLongTrailingIdx - 2] >= inOpen[bodyLongTrailingIdx - 2])
                        {
                            num33 = inClose[bodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num33 = inOpen[bodyLongTrailingIdx - 2];
                        }

                        if (inClose[bodyLongTrailingIdx - 2] >= inOpen[bodyLongTrailingIdx - 2])
                        {
                            num32 = inOpen[bodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num32 = inClose[bodyLongTrailingIdx - 2];
                        }

                        num31 = inHigh[bodyLongTrailingIdx - 2] - num33 + (num32 - inLow[bodyLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num31 = Decimal.Zero;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            bodyLongPeriodTotal += num40 - num35;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                decimal num29;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    decimal num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
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

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num27 = inOpen[i - 2];
                        }
                        else
                        {
                            num27 = inClose[i - 2];
                        }

                        num26 = inHigh[i - 2] - num28 + (num27 - inLow[i - 2]);
                    }
                    else
                    {
                        num26 = Decimal.Zero;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[shadowLongTrailingIdx - 2] - inOpen[shadowLongTrailingIdx - 2]);
            }
            else
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[shadowLongTrailingIdx - 2] - inLow[shadowLongTrailingIdx - 2];
                }
                else
                {
                    decimal num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowLong].RangeType == RangeType.Shadows)
                    {
                        decimal num22;
                        decimal num23;
                        if (inClose[shadowLongTrailingIdx - 2] >= inOpen[shadowLongTrailingIdx - 2])
                        {
                            num23 = inClose[shadowLongTrailingIdx - 2];
                        }
                        else
                        {
                            num23 = inOpen[shadowLongTrailingIdx - 2];
                        }

                        if (inClose[shadowLongTrailingIdx - 2] >= inOpen[shadowLongTrailingIdx - 2])
                        {
                            num22 = inOpen[shadowLongTrailingIdx - 2];
                        }
                        else
                        {
                            num22 = inClose[shadowLongTrailingIdx - 2];
                        }

                        num21 = inHigh[shadowLongTrailingIdx - 2] - num23 + (num22 - inLow[shadowLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num21 = Decimal.Zero;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            shadowLongPeriodTotal += num30 - num25;
            for (var totIdx = 1; totIdx >= 0; totIdx--)
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
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
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

                        num6 = inHigh[i] - num8 + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = Decimal.Zero;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyShortTrailingIdx] - inOpen[bodyShortTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyShortTrailingIdx] - inLow[bodyShortTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num3 = inClose[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyShortTrailingIdx];
                        }

                        if (inClose[bodyShortTrailingIdx] >= inOpen[bodyShortTrailingIdx])
                        {
                            num2 = inOpen[bodyShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyShortTrailingIdx];
                        }

                        num = inHigh[bodyShortTrailingIdx] - num3 + (num2 - inLow[bodyShortTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyShortPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0572;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl3StarsInSouthLookback()
        {
            int avgPeriod = Math.Max(
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod),
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod)
            );

            return avgPeriod + 2;
        }
    }
}
