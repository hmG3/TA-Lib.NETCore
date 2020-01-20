using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlRiseFall3Methods(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num25;
            double num30;
            double num84;
            var bodyPeriodTotal = new double[5];
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

            int lookbackTotal = CdlRiseFall3MethodsLookback();
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

            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = bodyShortTrailingIdx;
            while (true)
            {
                double num99;
                double num104;
                double num109;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                }
                else
                {
                    double num108;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        double num105;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num106;
                            double num107;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num107 = inClose[i - 3];
                            }
                            else
                            {
                                num107 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num106 = inOpen[i - 3];
                            }
                            else
                            {
                                num106 = inClose[i - 3];
                            }

                            num105 = inHigh[i - 3] - num107 + (num106 - inLow[i - 3]);
                        }
                        else
                        {
                            num105 = 0.0;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                bodyPeriodTotal[3] += num109;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num104 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num103;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num103 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num100;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num101;
                            double num102;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num102 = inClose[i - 2];
                            }
                            else
                            {
                                num102 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num101 = inOpen[i - 2];
                            }
                            else
                            {
                                num101 = inClose[i - 2];
                            }

                            num100 = inHigh[i - 2] - num102 + (num101 - inLow[i - 2]);
                        }
                        else
                        {
                            num100 = 0.0;
                        }

                        num103 = num100;
                    }

                    num104 = num103;
                }

                bodyPeriodTotal[2] += num104;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num99 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num98;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num98 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num95;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num96;
                            double num97;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num97 = inClose[i - 1];
                            }
                            else
                            {
                                num97 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num96 = inOpen[i - 1];
                            }
                            else
                            {
                                num96 = inClose[i - 1];
                            }

                            num95 = inHigh[i - 1] - num97 + (num96 - inLow[i - 1]);
                        }
                        else
                        {
                            num95 = 0.0;
                        }

                        num98 = num95;
                    }

                    num99 = num98;
                }

                bodyPeriodTotal[1] += num99;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                double num89;
                double num94;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num94 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    double num93;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num93 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num90;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num91;
                            double num92;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num92 = inClose[i - 4];
                            }
                            else
                            {
                                num92 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num91 = inOpen[i - 4];
                            }
                            else
                            {
                                num91 = inClose[i - 4];
                            }

                            num90 = inHigh[i - 4] - num92 + (num91 - inLow[i - 4]);
                        }
                        else
                        {
                            num90 = 0.0;
                        }

                        num93 = num90;
                    }

                    num94 = num93;
                }

                bodyPeriodTotal[4] += num94;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num89 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num88;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num88 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num85;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num86;
                            double num87;
                            if (inClose[i] >= inOpen[i])
                            {
                                num87 = inClose[i];
                            }
                            else
                            {
                                num87 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num86 = inOpen[i];
                            }
                            else
                            {
                                num86 = inClose[i];
                            }

                            num85 = inHigh[i] - num87 + (num86 - inLow[i]);
                        }
                        else
                        {
                            num85 = 0.0;
                        }

                        num88 = num85;
                    }

                    num89 = num88;
                }

                bodyPeriodTotal[0] += num89;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_04FF:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num84 = bodyPeriodTotal[4] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num83;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num83 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    double num82;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num82 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num79;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num80;
                            double num81;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num81 = inClose[i - 4];
                            }
                            else
                            {
                                num81 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num80 = inOpen[i - 4];
                            }
                            else
                            {
                                num80 = inClose[i - 4];
                            }

                            num79 = inHigh[i - 4] - num81 + (num80 - inLow[i - 4]);
                        }
                        else
                        {
                            num79 = 0.0;
                        }

                        num82 = num79;
                    }

                    num83 = num82;
                }

                num84 = num83;
            }

            var num78 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 4] - inOpen[i - 4]) >
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num84 / num78)
            {
                double num77;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                {
                    num77 = bodyPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                }
                else
                {
                    double num76;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                    {
                        num76 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                    }
                    else
                    {
                        double num75;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                        {
                            num75 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            double num72;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                            {
                                double num73;
                                double num74;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num74 = inClose[i - 3];
                                }
                                else
                                {
                                    num74 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num73 = inOpen[i - 3];
                                }
                                else
                                {
                                    num73 = inClose[i - 3];
                                }

                                num72 = inHigh[i - 3] - num74 + (num73 - inLow[i - 3]);
                            }
                            else
                            {
                                num72 = 0.0;
                            }

                            num75 = num72;
                        }

                        num76 = num75;
                    }

                    num77 = num76;
                }

                var num71 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 3] - inOpen[i - 3]) <
                    Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num77 / num71)
                {
                    double num70;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num70 = bodyPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        double num69;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num69 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                        }
                        else
                        {
                            double num68;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num68 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                double num65;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    double num66;
                                    double num67;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num67 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num67 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num66 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num66 = inClose[i - 2];
                                    }

                                    num65 = inHigh[i - 2] - num67 + (num66 - inLow[i - 2]);
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

                    var num64 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) <
                        Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num70 / num64)
                    {
                        double num63;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                        {
                            num63 = bodyPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                        }
                        else
                        {
                            double num62;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                            {
                                num62 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                double num61;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                {
                                    num61 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num58;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                        var num57 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                        if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <
                            Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num63 / num57)
                        {
                            double num56;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                            {
                                num56 = bodyPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                            }
                            else
                            {
                                double num55;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                                {
                                    num55 = Math.Abs(inClose[i] - inOpen[i]);
                                }
                                else
                                {
                                    double num54;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                                    {
                                        num54 = inHigh[i] - inLow[i];
                                    }
                                    else
                                    {
                                        double num51;
                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                        {
                                            double num52;
                                            double num53;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num53 = inClose[i];
                                            }
                                            else
                                            {
                                                num53 = inOpen[i];
                                            }

                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num52 = inOpen[i];
                                            }
                                            else
                                            {
                                                num52 = inClose[i];
                                            }

                                            num51 = inHigh[i] - num53 + (num52 - inLow[i]);
                                        }
                                        else
                                        {
                                            num51 = 0.0;
                                        }

                                        num54 = num51;
                                    }

                                    num55 = num54;
                                }

                                num56 = num55;
                            }

                            var num50 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                            if (Math.Abs(inClose[i] - inOpen[i]) >
                                Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num56 / num50)
                            {
                                int num49;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num49 = 1;
                                }
                                else
                                {
                                    num49 = -1;
                                }

                                if ((inClose[i - 4] < inOpen[i - 4] ? -1 : 1) == -num49)
                                {
                                    int num48;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num48 = 1;
                                    }
                                    else
                                    {
                                        num48 = -1;
                                    }

                                    if ((inClose[i - 3] < inOpen[i - 3] ? -1 : 1) == num48)
                                    {
                                        int num47;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num47 = 1;
                                        }
                                        else
                                        {
                                            num47 = -1;
                                        }

                                        if ((inClose[i - 2] < inOpen[i - 2] ? -1 : 1) == num47)
                                        {
                                            int num46;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num46 = 1;
                                            }
                                            else
                                            {
                                                num46 = -1;
                                            }

                                            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) == -num46)
                                            {
                                                double num45;
                                                if (inOpen[i - 3] < inClose[i - 3])
                                                {
                                                    num45 = inOpen[i - 3];
                                                }
                                                else
                                                {
                                                    num45 = inClose[i - 3];
                                                }

                                                if (num45 < inHigh[i - 4])
                                                {
                                                    double num44;
                                                    if (inOpen[i - 3] > inClose[i - 3])
                                                    {
                                                        num44 = inOpen[i - 3];
                                                    }
                                                    else
                                                    {
                                                        num44 = inClose[i - 3];
                                                    }

                                                    if (num44 > inLow[i - 4])
                                                    {
                                                        double num43;
                                                        if (inOpen[i - 2] < inClose[i - 2])
                                                        {
                                                            num43 = inOpen[i - 2];
                                                        }
                                                        else
                                                        {
                                                            num43 = inClose[i - 2];
                                                        }

                                                        if (num43 < inHigh[i - 4])
                                                        {
                                                            double num42;
                                                            if (inOpen[i - 2] > inClose[i - 2])
                                                            {
                                                                num42 = inOpen[i - 2];
                                                            }
                                                            else
                                                            {
                                                                num42 = inClose[i - 2];
                                                            }

                                                            if (num42 > inLow[i - 4])
                                                            {
                                                                double num41;
                                                                if (inOpen[i - 1] < inClose[i - 1])
                                                                {
                                                                    num41 = inOpen[i - 1];
                                                                }
                                                                else
                                                                {
                                                                    num41 = inClose[i - 1];
                                                                }

                                                                if (num41 < inHigh[i - 4])
                                                                {
                                                                    double num40;
                                                                    if (inOpen[i - 1] > inClose[i - 1])
                                                                    {
                                                                        num40 = inOpen[i - 1];
                                                                    }
                                                                    else
                                                                    {
                                                                        num40 = inClose[i - 1];
                                                                    }

                                                                    if (num40 > inLow[i - 4])
                                                                    {
                                                                        int num38;
                                                                        int num39;
                                                                        if (inClose[i - 4] >= inOpen[i - 4])
                                                                        {
                                                                            num39 = 1;
                                                                        }
                                                                        else
                                                                        {
                                                                            num39 = -1;
                                                                        }

                                                                        if (inClose[i - 4] >= inOpen[i - 4])
                                                                        {
                                                                            num38 = 1;
                                                                        }
                                                                        else
                                                                        {
                                                                            num38 = -1;
                                                                        }

                                                                        if (inClose[i - 2] * num39 < inClose[i - 3] * num38)
                                                                        {
                                                                            int num36;
                                                                            int num37;
                                                                            if (inClose[i - 4] >= inOpen[i - 4])
                                                                            {
                                                                                num37 = 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                num37 = -1;
                                                                            }

                                                                            if (inClose[i - 4] >= inOpen[i - 4])
                                                                            {
                                                                                num36 = 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                num36 = -1;
                                                                            }

                                                                            if (inClose[i - 1] * num37 < inClose[i - 2] * num36)
                                                                            {
                                                                                int num34;
                                                                                int num35;
                                                                                if (inClose[i - 4] >= inOpen[i - 4])
                                                                                {
                                                                                    num35 = 1;
                                                                                }
                                                                                else
                                                                                {
                                                                                    num35 = -1;
                                                                                }

                                                                                if (inClose[i - 4] >= inOpen[i - 4])
                                                                                {
                                                                                    num34 = 1;
                                                                                }
                                                                                else
                                                                                {
                                                                                    num34 = -1;
                                                                                }

                                                                                if (inOpen[i] * num35 > inClose[i - 1] * num34)
                                                                                {
                                                                                    int num32;
                                                                                    int num33;
                                                                                    if (inClose[i - 4] >= inOpen[i - 4])
                                                                                    {
                                                                                        num33 = 1;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        num33 = -1;
                                                                                    }

                                                                                    if (inClose[i - 4] >= inOpen[i - 4])
                                                                                    {
                                                                                        num32 = 1;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        num32 = -1;
                                                                                    }

                                                                                    if (inClose[i] * num33 > inClose[i - 4] * num32)
                                                                                    {
                                                                                        int num31;
                                                                                        if (inClose[i - 4] >= inOpen[i - 4])
                                                                                        {
                                                                                            num31 = 1;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            num31 = -1;
                                                                                        }

                                                                                        outInteger[outIdx] = num31 * 100;
                                                                                        outIdx++;
                                                                                        goto Label_0EAB;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
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
            Label_0EAB:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
            }
            else
            {
                double num29;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    double num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num27;
                        double num28;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num28 = inClose[i - 4];
                        }
                        else
                        {
                            num28 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num27 = inOpen[i - 4];
                        }
                        else
                        {
                            num27 = inClose[i - 4];
                        }

                        num26 = inHigh[i - 4] - num28 + (num27 - inLow[i - 4]);
                    }
                    else
                    {
                        num26 = 0.0;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[bodyLongTrailingIdx - 4] - inOpen[bodyLongTrailingIdx - 4]);
            }
            else
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[bodyLongTrailingIdx - 4] - inLow[bodyLongTrailingIdx - 4];
                }
                else
                {
                    double num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num23 = inClose[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num23 = inOpen[bodyLongTrailingIdx - 4];
                        }

                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num22 = inOpen[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num22 = inClose[bodyLongTrailingIdx - 4];
                        }

                        num21 = inHigh[bodyLongTrailingIdx - 4] - num23 + (num22 - inLow[bodyLongTrailingIdx - 4]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            bodyPeriodTotal[4] += num30 - num25;
            for (var totIdx = 3; totIdx >= 1; totIdx--)
            {
                double num15;
                double num20;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num19;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs(inClose[bodyShortTrailingIdx - totIdx] - inOpen[bodyShortTrailingIdx - totIdx]);
                }
                else
                {
                    double num14;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[bodyShortTrailingIdx - totIdx] - inLow[bodyShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num11;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num12;
                            double num13;
                            if (inClose[bodyShortTrailingIdx - totIdx] >= inOpen[bodyShortTrailingIdx - totIdx])
                            {
                                num13 = inClose[bodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[bodyShortTrailingIdx - totIdx];
                            }

                            if (inClose[bodyShortTrailingIdx - totIdx] >= inOpen[bodyShortTrailingIdx - totIdx])
                            {
                                num12 = inOpen[bodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[bodyShortTrailingIdx - totIdx];
                            }

                            num11 = inHigh[bodyShortTrailingIdx - totIdx] - num13 + (num12 - inLow[bodyShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                bodyPeriodTotal[totIdx] += num20 - num15;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num3 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num2 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx];
                        }

                        num = inHigh[bodyLongTrailingIdx] - num3 + (num2 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyPeriodTotal[0] += num10 - num5;
            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_04FF;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlRiseFall3Methods(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num25;
            decimal num30;
            decimal num84;
            var bodyPeriodTotal = new decimal[5];
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

            int lookbackTotal = CdlRiseFall3MethodsLookback();
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

            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num99;
                decimal num104;
                decimal num109;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num109 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                }
                else
                {
                    decimal num108;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num108 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        decimal num105;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num106;
                            decimal num107;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num107 = inClose[i - 3];
                            }
                            else
                            {
                                num107 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num106 = inOpen[i - 3];
                            }
                            else
                            {
                                num106 = inClose[i - 3];
                            }

                            num105 = inHigh[i - 3] - num107 + (num106 - inLow[i - 3]);
                        }
                        else
                        {
                            num105 = Decimal.Zero;
                        }

                        num108 = num105;
                    }

                    num109 = num108;
                }

                bodyPeriodTotal[3] += num109;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num104 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num103;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num103 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num100;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num101;
                            decimal num102;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num102 = inClose[i - 2];
                            }
                            else
                            {
                                num102 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num101 = inOpen[i - 2];
                            }
                            else
                            {
                                num101 = inClose[i - 2];
                            }

                            num100 = inHigh[i - 2] - num102 + (num101 - inLow[i - 2]);
                        }
                        else
                        {
                            num100 = Decimal.Zero;
                        }

                        num103 = num100;
                    }

                    num104 = num103;
                }

                bodyPeriodTotal[2] += num104;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num99 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num98;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num98 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num95;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num96;
                            decimal num97;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num97 = inClose[i - 1];
                            }
                            else
                            {
                                num97 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num96 = inOpen[i - 1];
                            }
                            else
                            {
                                num96 = inClose[i - 1];
                            }

                            num95 = inHigh[i - 1] - num97 + (num96 - inLow[i - 1]);
                        }
                        else
                        {
                            num95 = Decimal.Zero;
                        }

                        num98 = num95;
                    }

                    num99 = num98;
                }

                bodyPeriodTotal[1] += num99;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num89;
                decimal num94;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num94 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    decimal num93;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num93 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        decimal num90;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num91;
                            decimal num92;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num92 = inClose[i - 4];
                            }
                            else
                            {
                                num92 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num91 = inOpen[i - 4];
                            }
                            else
                            {
                                num91 = inClose[i - 4];
                            }

                            num90 = inHigh[i - 4] - num92 + (num91 - inLow[i - 4]);
                        }
                        else
                        {
                            num90 = Decimal.Zero;
                        }

                        num93 = num90;
                    }

                    num94 = num93;
                }

                bodyPeriodTotal[4] += num94;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num89 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num88;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num88 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num85;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num86;
                            decimal num87;
                            if (inClose[i] >= inOpen[i])
                            {
                                num87 = inClose[i];
                            }
                            else
                            {
                                num87 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num86 = inOpen[i];
                            }
                            else
                            {
                                num86 = inClose[i];
                            }

                            num85 = inHigh[i] - num87 + (num86 - inLow[i]);
                        }
                        else
                        {
                            num85 = Decimal.Zero;
                        }

                        num88 = num85;
                    }

                    num89 = num88;
                }

                bodyPeriodTotal[0] += num89;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0545:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num84 = bodyPeriodTotal[4] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num83;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num83 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    decimal num82;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num82 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        decimal num79;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num80;
                            decimal num81;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num81 = inClose[i - 4];
                            }
                            else
                            {
                                num81 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num80 = inOpen[i - 4];
                            }
                            else
                            {
                                num80 = inClose[i - 4];
                            }

                            num79 = inHigh[i - 4] - num81 + (num80 - inLow[i - 4]);
                        }
                        else
                        {
                            num79 = Decimal.Zero;
                        }

                        num82 = num79;
                    }

                    num83 = num82;
                }

                num84 = num83;
            }

            var num78 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 4] - inOpen[i - 4]) >
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num84 / num78)
            {
                decimal num77;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                {
                    num77 = bodyPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                }
                else
                {
                    decimal num76;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                    {
                        num76 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                    }
                    else
                    {
                        decimal num75;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                        {
                            num75 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            decimal num72;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                            {
                                decimal num73;
                                decimal num74;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num74 = inClose[i - 3];
                                }
                                else
                                {
                                    num74 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num73 = inOpen[i - 3];
                                }
                                else
                                {
                                    num73 = inClose[i - 3];
                                }

                                num72 = inHigh[i - 3] - num74 + (num73 - inLow[i - 3]);
                            }
                            else
                            {
                                num72 = Decimal.Zero;
                            }

                            num75 = num72;
                        }

                        num76 = num75;
                    }

                    num77 = num76;
                }

                var num71 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 3] - inOpen[i - 3]) <
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num77 / num71)
                {
                    decimal num70;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num70 = bodyPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num69;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num69 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                        }
                        else
                        {
                            decimal num68;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num68 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                decimal num65;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num66;
                                    decimal num67;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num67 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num67 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num66 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num66 = inClose[i - 2];
                                    }

                                    num65 = inHigh[i - 2] - num67 + (num66 - inLow[i - 2]);
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

                    var num64 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) <
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num70 / num64)
                    {
                        decimal num63;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                        {
                            num63 = bodyPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num62;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                            {
                                num62 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                decimal num61;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                {
                                    num61 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    decimal num58;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                        var num57 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                        if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <
                            (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num63 / num57)
                        {
                            decimal num56;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                            {
                                num56 = bodyPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                            }
                            else
                            {
                                decimal num55;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                                {
                                    num55 = Math.Abs(inClose[i] - inOpen[i]);
                                }
                                else
                                {
                                    decimal num54;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                                    {
                                        num54 = inHigh[i] - inLow[i];
                                    }
                                    else
                                    {
                                        decimal num51;
                                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                        {
                                            decimal num52;
                                            decimal num53;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num53 = inClose[i];
                                            }
                                            else
                                            {
                                                num53 = inOpen[i];
                                            }

                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num52 = inOpen[i];
                                            }
                                            else
                                            {
                                                num52 = inClose[i];
                                            }

                                            num51 = inHigh[i] - num53 + (num52 - inLow[i]);
                                        }
                                        else
                                        {
                                            num51 = Decimal.Zero;
                                        }

                                        num54 = num51;
                                    }

                                    num55 = num54;
                                }

                                num56 = num55;
                            }

                            var num50 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (Math.Abs(inClose[i] - inOpen[i]) >
                                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num56 / num50)
                            {
                                int num49;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num49 = 1;
                                }
                                else
                                {
                                    num49 = -1;
                                }

                                if ((inClose[i - 4] < inOpen[i - 4] ? -1 : 1) == -num49)
                                {
                                    int num48;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num48 = 1;
                                    }
                                    else
                                    {
                                        num48 = -1;
                                    }

                                    if ((inClose[i - 3] < inOpen[i - 3] ? -1 : 1) == num48)
                                    {
                                        int num47;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num47 = 1;
                                        }
                                        else
                                        {
                                            num47 = -1;
                                        }

                                        if ((inClose[i - 2] < inOpen[i - 2] ? -1 : 1) == num47)
                                        {
                                            int num46;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num46 = 1;
                                            }
                                            else
                                            {
                                                num46 = -1;
                                            }

                                            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) == -num46)
                                            {
                                                decimal num45;
                                                if (inOpen[i - 3] < inClose[i - 3])
                                                {
                                                    num45 = inOpen[i - 3];
                                                }
                                                else
                                                {
                                                    num45 = inClose[i - 3];
                                                }

                                                if (num45 < inHigh[i - 4])
                                                {
                                                    decimal num44;
                                                    if (inOpen[i - 3] > inClose[i - 3])
                                                    {
                                                        num44 = inOpen[i - 3];
                                                    }
                                                    else
                                                    {
                                                        num44 = inClose[i - 3];
                                                    }

                                                    if (num44 > inLow[i - 4])
                                                    {
                                                        decimal num43;
                                                        if (inOpen[i - 2] < inClose[i - 2])
                                                        {
                                                            num43 = inOpen[i - 2];
                                                        }
                                                        else
                                                        {
                                                            num43 = inClose[i - 2];
                                                        }

                                                        if (num43 < inHigh[i - 4])
                                                        {
                                                            decimal num42;
                                                            if (inOpen[i - 2] > inClose[i - 2])
                                                            {
                                                                num42 = inOpen[i - 2];
                                                            }
                                                            else
                                                            {
                                                                num42 = inClose[i - 2];
                                                            }

                                                            if (num42 > inLow[i - 4])
                                                            {
                                                                decimal num41;
                                                                if (inOpen[i - 1] < inClose[i - 1])
                                                                {
                                                                    num41 = inOpen[i - 1];
                                                                }
                                                                else
                                                                {
                                                                    num41 = inClose[i - 1];
                                                                }

                                                                if (num41 < inHigh[i - 4])
                                                                {
                                                                    decimal num40;
                                                                    if (inOpen[i - 1] > inClose[i - 1])
                                                                    {
                                                                        num40 = inOpen[i - 1];
                                                                    }
                                                                    else
                                                                    {
                                                                        num40 = inClose[i - 1];
                                                                    }

                                                                    if (num40 > inLow[i - 4])
                                                                    {
                                                                        int num38;
                                                                        int num39;
                                                                        if (inClose[i - 4] >= inOpen[i - 4])
                                                                        {
                                                                            num39 = 1;
                                                                        }
                                                                        else
                                                                        {
                                                                            num39 = -1;
                                                                        }

                                                                        if (inClose[i - 4] >= inOpen[i - 4])
                                                                        {
                                                                            num38 = 1;
                                                                        }
                                                                        else
                                                                        {
                                                                            num38 = -1;
                                                                        }

                                                                        if (inClose[i - 2] * num39 < inClose[i - 3] * num38)
                                                                        {
                                                                            int num36;
                                                                            int num37;
                                                                            if (inClose[i - 4] >= inOpen[i - 4])
                                                                            {
                                                                                num37 = 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                num37 = -1;
                                                                            }

                                                                            if (inClose[i - 4] >= inOpen[i - 4])
                                                                            {
                                                                                num36 = 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                num36 = -1;
                                                                            }

                                                                            if (inClose[i - 1] * num37 < inClose[i - 2] * num36)
                                                                            {
                                                                                int num34;
                                                                                int num35;
                                                                                if (inClose[i - 4] >= inOpen[i - 4])
                                                                                {
                                                                                    num35 = 1;
                                                                                }
                                                                                else
                                                                                {
                                                                                    num35 = -1;
                                                                                }

                                                                                if (inClose[i - 4] >= inOpen[i - 4])
                                                                                {
                                                                                    num34 = 1;
                                                                                }
                                                                                else
                                                                                {
                                                                                    num34 = -1;
                                                                                }

                                                                                if (inOpen[i] * num35 > inClose[i - 1] * num34)
                                                                                {
                                                                                    int num32;
                                                                                    int num33;
                                                                                    if (inClose[i - 4] >= inOpen[i - 4])
                                                                                    {
                                                                                        num33 = 1;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        num33 = -1;
                                                                                    }

                                                                                    if (inClose[i - 4] >= inOpen[i - 4])
                                                                                    {
                                                                                        num32 = 1;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        num32 = -1;
                                                                                    }

                                                                                    if (inClose[i] * num33 > inClose[i - 4] * num32)
                                                                                    {
                                                                                        int num31;
                                                                                        if (inClose[i - 4] >= inOpen[i - 4])
                                                                                        {
                                                                                            num31 = 1;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            num31 = -1;
                                                                                        }

                                                                                        outInteger[outIdx] = num31 * 100;
                                                                                        outIdx++;
                                                                                        goto Label_0FA1;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
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
            Label_0FA1:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
            }
            else
            {
                decimal num29;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    decimal num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num27;
                        decimal num28;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num28 = inClose[i - 4];
                        }
                        else
                        {
                            num28 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num27 = inOpen[i - 4];
                        }
                        else
                        {
                            num27 = inClose[i - 4];
                        }

                        num26 = inHigh[i - 4] - num28 + (num27 - inLow[i - 4]);
                    }
                    else
                    {
                        num26 = Decimal.Zero;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[bodyLongTrailingIdx - 4] - inOpen[bodyLongTrailingIdx - 4]);
            }
            else
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[bodyLongTrailingIdx - 4] - inLow[bodyLongTrailingIdx - 4];
                }
                else
                {
                    decimal num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num22;
                        decimal num23;
                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num23 = inClose[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num23 = inOpen[bodyLongTrailingIdx - 4];
                        }

                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num22 = inOpen[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num22 = inClose[bodyLongTrailingIdx - 4];
                        }

                        num21 = inHigh[bodyLongTrailingIdx - 4] - num23 + (num22 - inLow[bodyLongTrailingIdx - 4]);
                    }
                    else
                    {
                        num21 = Decimal.Zero;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            bodyPeriodTotal[4] += num30 - num25;
            for (var totIdx = 3; totIdx >= 1; totIdx--)
            {
                decimal num15;
                decimal num20;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num19;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs(inClose[bodyShortTrailingIdx - totIdx] - inOpen[bodyShortTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num14;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[bodyShortTrailingIdx - totIdx] - inLow[bodyShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num11;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num12;
                            decimal num13;
                            if (inClose[bodyShortTrailingIdx - totIdx] >= inOpen[bodyShortTrailingIdx - totIdx])
                            {
                                num13 = inClose[bodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[bodyShortTrailingIdx - totIdx];
                            }

                            if (inClose[bodyShortTrailingIdx - totIdx] >= inOpen[bodyShortTrailingIdx - totIdx])
                            {
                                num12 = inOpen[bodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[bodyShortTrailingIdx - totIdx];
                            }

                            num11 = inHigh[bodyShortTrailingIdx - totIdx] - num13 + (num12 - inLow[bodyShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = Decimal.Zero;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                bodyPeriodTotal[totIdx] += num20 - num15;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num3 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num2 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx];
                        }

                        num = inHigh[bodyLongTrailingIdx] - num3 + (num2 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyPeriodTotal[0] += num10 - num5;
            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0545;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlRiseFall3MethodsLookback()
        {
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod);

            return avgPeriod + 4;
        }
    }
}
