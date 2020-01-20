using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlMatHold(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger, double optInPenetration = 0.5)
        {
            double num15;
            double num20;
            double num60;
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

            if (optInPenetration < 0.0)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlMatHoldLookback();
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
                double num70;
                double num75;
                double num80;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num80 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                }
                else
                {
                    double num79;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num79 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        double num76;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num77;
                            double num78;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num78 = inClose[i - 3];
                            }
                            else
                            {
                                num78 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num77 = inOpen[i - 3];
                            }
                            else
                            {
                                num77 = inClose[i - 3];
                            }

                            num76 = inHigh[i - 3] - num78 + (num77 - inLow[i - 3]);
                        }
                        else
                        {
                            num76 = 0.0;
                        }

                        num79 = num76;
                    }

                    num80 = num79;
                }

                bodyPeriodTotal[3] += num80;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num75 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num74;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num74 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num71;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num72;
                            double num73;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num73 = inClose[i - 2];
                            }
                            else
                            {
                                num73 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num72 = inOpen[i - 2];
                            }
                            else
                            {
                                num72 = inClose[i - 2];
                            }

                            num71 = inHigh[i - 2] - num73 + (num72 - inLow[i - 2]);
                        }
                        else
                        {
                            num71 = 0.0;
                        }

                        num74 = num71;
                    }

                    num75 = num74;
                }

                bodyPeriodTotal[2] += num75;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num69;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num66;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num67;
                            double num68;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num68 = inClose[i - 1];
                            }
                            else
                            {
                                num68 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num67 = inOpen[i - 1];
                            }
                            else
                            {
                                num67 = inClose[i - 1];
                            }

                            num66 = inHigh[i - 1] - num68 + (num67 - inLow[i - 1]);
                        }
                        else
                        {
                            num66 = 0.0;
                        }

                        num69 = num66;
                    }

                    num70 = num69;
                }

                bodyPeriodTotal[1] += num70;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                double num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    double num64;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num61;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num62;
                            double num63;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num63 = inClose[i - 4];
                            }
                            else
                            {
                                num63 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num62 = inOpen[i - 4];
                            }
                            else
                            {
                                num62 = inClose[i - 4];
                            }

                            num61 = inHigh[i - 4] - num63 + (num62 - inLow[i - 4]);
                        }
                        else
                        {
                            num61 = 0.0;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                bodyPeriodTotal[4] += num65;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0488:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num60 = bodyPeriodTotal[4] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num59;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num59 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    double num58;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num58 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num55;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num56;
                            double num57;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num57 = inClose[i - 4];
                            }
                            else
                            {
                                num57 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num56 = inOpen[i - 4];
                            }
                            else
                            {
                                num56 = inClose[i - 4];
                            }

                            num55 = inHigh[i - 4] - num57 + (num56 - inLow[i - 4]);
                        }
                        else
                        {
                            num55 = 0.0;
                        }

                        num58 = num55;
                    }

                    num59 = num58;
                }

                num60 = num59;
            }

            var num54 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 4] - inOpen[i - 4]) >
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num60 / num54)
            {
                double num53;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                {
                    num53 = bodyPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                }
                else
                {
                    double num52;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                    {
                        num52 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                    }
                    else
                    {
                        double num51;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                        {
                            num51 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            double num48;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                            {
                                double num49;
                                double num50;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num50 = inClose[i - 3];
                                }
                                else
                                {
                                    num50 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num49 = inOpen[i - 3];
                                }
                                else
                                {
                                    num49 = inClose[i - 3];
                                }

                                num48 = inHigh[i - 3] - num50 + (num49 - inLow[i - 3]);
                            }
                            else
                            {
                                num48 = 0.0;
                            }

                            num51 = num48;
                        }

                        num52 = num51;
                    }

                    num53 = num52;
                }

                var num47 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 3] - inOpen[i - 3]) <
                    Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num53 / num47)
                {
                    double num46;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num46 = bodyPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        double num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num45 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                        }
                        else
                        {
                            double num44;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num44 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                double num41;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    double num42;
                                    double num43;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num43 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num43 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num42 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num42 = inClose[i - 2];
                                    }

                                    num41 = inHigh[i - 2] - num43 + (num42 - inLow[i - 2]);
                                }
                                else
                                {
                                    num41 = 0.0;
                                }

                                num44 = num41;
                            }

                            num45 = num44;
                        }

                        num46 = num45;
                    }

                    var num40 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) <
                        Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num46 / num40)
                    {
                        double num39;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                        {
                            num39 = bodyPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                        }
                        else
                        {
                            double num38;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                            {
                                num38 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                double num37;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                {
                                    num37 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num34;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                    {
                                        double num35;
                                        double num36;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num36 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num36 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num35 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num35 = inClose[i - 1];
                                        }

                                        num34 = inHigh[i - 1] - num36 + (num35 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num34 = 0.0;
                                    }

                                    num37 = num34;
                                }

                                num38 = num37;
                            }

                            num39 = num38;
                        }

                        var num33 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                        if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <
                            Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num39 / num33 &&
                            inClose[i - 4] >= inOpen[i - 4] && inClose[i - 3] < inOpen[i - 3] && inClose[i] >= inOpen[i])
                        {
                            double num31;
                            double num32;
                            if (inOpen[i - 3] < inClose[i - 3])
                            {
                                num32 = inOpen[i - 3];
                            }
                            else
                            {
                                num32 = inClose[i - 3];
                            }

                            if (inOpen[i - 4] > inClose[i - 4])
                            {
                                num31 = inOpen[i - 4];
                            }
                            else
                            {
                                num31 = inClose[i - 4];
                            }

                            if (num32 > num31)
                            {
                                double num30;
                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num30 = inOpen[i - 2];
                                }
                                else
                                {
                                    num30 = inClose[i - 2];
                                }

                                if (num30 < inClose[i - 4])
                                {
                                    double num29;
                                    if (inOpen[i - 1] < inClose[i - 1])
                                    {
                                        num29 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num29 = inClose[i - 1];
                                    }

                                    if (num29 < inClose[i - 4])
                                    {
                                        double num28;
                                        if (inOpen[i - 2] < inClose[i - 2])
                                        {
                                            num28 = inOpen[i - 2];
                                        }
                                        else
                                        {
                                            num28 = inClose[i - 2];
                                        }

                                        if (num28 > inClose[i - 4] - Math.Abs(inClose[i - 4] - inOpen[i - 4]) * optInPenetration)
                                        {
                                            double num27;
                                            if (inOpen[i - 1] < inClose[i - 1])
                                            {
                                                num27 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num27 = inClose[i - 1];
                                            }

                                            if (num27 > inClose[i - 4] - Math.Abs(inClose[i - 4] - inOpen[i - 4]) * optInPenetration)
                                            {
                                                double num26;
                                                if (inClose[i - 2] > inOpen[i - 2])
                                                {
                                                    num26 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num26 = inOpen[i - 2];
                                                }

                                                if (num26 < inOpen[i - 3])
                                                {
                                                    double num24;
                                                    double num25;
                                                    if (inClose[i - 1] > inOpen[i - 1])
                                                    {
                                                        num25 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num25 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 2] > inOpen[i - 2])
                                                    {
                                                        num24 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num24 = inOpen[i - 2];
                                                    }

                                                    if (num25 < num24 && inOpen[i] > inClose[i - 1])
                                                    {
                                                        double num21;
                                                        double num23;
                                                        if (inHigh[i - 3] > inHigh[i - 2])
                                                        {
                                                            num23 = inHigh[i - 3];
                                                        }
                                                        else
                                                        {
                                                            num23 = inHigh[i - 2];
                                                        }

                                                        if (num23 > inHigh[i - 1])
                                                        {
                                                            double num22;
                                                            if (inHigh[i - 3] > inHigh[i - 2])
                                                            {
                                                                num22 = inHigh[i - 3];
                                                            }
                                                            else
                                                            {
                                                                num22 = inHigh[i - 2];
                                                            }

                                                            num21 = num22;
                                                        }
                                                        else
                                                        {
                                                            num21 = inHigh[i - 1];
                                                        }

                                                        if (inClose[i] > num21)
                                                        {
                                                            outInteger[outIdx] = 100;
                                                            outIdx++;
                                                            goto Label_0C54;
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
            Label_0C54:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num18 = inClose[i - 4];
                        }
                        else
                        {
                            num18 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num17 = inOpen[i - 4];
                        }
                        else
                        {
                            num17 = inClose[i - 4];
                        }

                        num16 = inHigh[i - 4] - num18 + (num17 - inLow[i - 4]);
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
                num15 = Math.Abs(inClose[bodyLongTrailingIdx - 4] - inOpen[bodyLongTrailingIdx - 4]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx - 4] - inLow[bodyLongTrailingIdx - 4];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num13 = inClose[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num13 = inOpen[bodyLongTrailingIdx - 4];
                        }

                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num12 = inOpen[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num12 = inClose[bodyLongTrailingIdx - 4];
                        }

                        num11 = inHigh[bodyLongTrailingIdx - 4] - num13 + (num12 - inLow[bodyLongTrailingIdx - 4]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyPeriodTotal[4] += num20 - num15;
            for (var totIdx = 3; totIdx >= 1; totIdx--)
            {
                double num5;
                double num10;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[bodyShortTrailingIdx - totIdx] - inOpen[bodyShortTrailingIdx - totIdx]);
                }
                else
                {
                    double num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[bodyShortTrailingIdx - totIdx] - inLow[bodyShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[bodyShortTrailingIdx - totIdx] >= inOpen[bodyShortTrailingIdx - totIdx])
                            {
                                num3 = inClose[bodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[bodyShortTrailingIdx - totIdx];
                            }

                            if (inClose[bodyShortTrailingIdx - totIdx] >= inOpen[bodyShortTrailingIdx - totIdx])
                            {
                                num2 = inOpen[bodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[bodyShortTrailingIdx - totIdx];
                            }

                            num = inHigh[bodyShortTrailingIdx - totIdx] - num3 + (num2 - inLow[bodyShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                bodyPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0488;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlMatHold(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger, decimal optInPenetration = 0.5m)
        {
            decimal num15;
            decimal num20;
            decimal num60;
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

            if (optInPenetration < Decimal.Zero)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlMatHoldLookback();
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
                decimal num70;
                decimal num75;
                decimal num80;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num80 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                }
                else
                {
                    decimal num79;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num79 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        decimal num76;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num77;
                            decimal num78;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num78 = inClose[i - 3];
                            }
                            else
                            {
                                num78 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num77 = inOpen[i - 3];
                            }
                            else
                            {
                                num77 = inClose[i - 3];
                            }

                            num76 = inHigh[i - 3] - num78 + (num77 - inLow[i - 3]);
                        }
                        else
                        {
                            num76 = Decimal.Zero;
                        }

                        num79 = num76;
                    }

                    num80 = num79;
                }

                bodyPeriodTotal[3] += num80;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num75 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num74;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num74 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num71;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num72;
                            decimal num73;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num73 = inClose[i - 2];
                            }
                            else
                            {
                                num73 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num72 = inOpen[i - 2];
                            }
                            else
                            {
                                num72 = inClose[i - 2];
                            }

                            num71 = inHigh[i - 2] - num73 + (num72 - inLow[i - 2]);
                        }
                        else
                        {
                            num71 = Decimal.Zero;
                        }

                        num74 = num71;
                    }

                    num75 = num74;
                }

                bodyPeriodTotal[2] += num75;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num69;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num66;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num67;
                            decimal num68;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num68 = inClose[i - 1];
                            }
                            else
                            {
                                num68 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num67 = inOpen[i - 1];
                            }
                            else
                            {
                                num67 = inClose[i - 1];
                            }

                            num66 = inHigh[i - 1] - num68 + (num67 - inLow[i - 1]);
                        }
                        else
                        {
                            num66 = Decimal.Zero;
                        }

                        num69 = num66;
                    }

                    num70 = num69;
                }

                bodyPeriodTotal[1] += num70;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    decimal num64;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        decimal num61;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num62;
                            decimal num63;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num63 = inClose[i - 4];
                            }
                            else
                            {
                                num63 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num62 = inOpen[i - 4];
                            }
                            else
                            {
                                num62 = inClose[i - 4];
                            }

                            num61 = inHigh[i - 4] - num63 + (num62 - inLow[i - 4]);
                        }
                        else
                        {
                            num61 = Decimal.Zero;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                bodyPeriodTotal[4] += num65;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_04C0:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num60 = bodyPeriodTotal[4] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num59;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num59 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
                }
                else
                {
                    decimal num58;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num58 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        decimal num55;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num56;
                            decimal num57;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num57 = inClose[i - 4];
                            }
                            else
                            {
                                num57 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num56 = inOpen[i - 4];
                            }
                            else
                            {
                                num56 = inClose[i - 4];
                            }

                            num55 = inHigh[i - 4] - num57 + (num56 - inLow[i - 4]);
                        }
                        else
                        {
                            num55 = Decimal.Zero;
                        }

                        num58 = num55;
                    }

                    num59 = num58;
                }

                num60 = num59;
            }

            var num54 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 4] - inOpen[i - 4]) >
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num60 / num54)
            {
                decimal num53;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                {
                    num53 = bodyPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                }
                else
                {
                    decimal num52;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                    {
                        num52 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                    }
                    else
                    {
                        decimal num51;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                        {
                            num51 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            decimal num48;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                            {
                                decimal num49;
                                decimal num50;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num50 = inClose[i - 3];
                                }
                                else
                                {
                                    num50 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num49 = inOpen[i - 3];
                                }
                                else
                                {
                                    num49 = inClose[i - 3];
                                }

                                num48 = inHigh[i - 3] - num50 + (num49 - inLow[i - 3]);
                            }
                            else
                            {
                                num48 = Decimal.Zero;
                            }

                            num51 = num48;
                        }

                        num52 = num51;
                    }

                    num53 = num52;
                }

                var num47 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 3] - inOpen[i - 3]) <
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num53 / num47)
                {
                    decimal num46;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num46 = bodyPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num45 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                        }
                        else
                        {
                            decimal num44;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num44 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                decimal num41;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num42;
                                    decimal num43;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num43 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num43 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num42 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num42 = inClose[i - 2];
                                    }

                                    num41 = inHigh[i - 2] - num43 + (num42 - inLow[i - 2]);
                                }
                                else
                                {
                                    num41 = Decimal.Zero;
                                }

                                num44 = num41;
                            }

                            num45 = num44;
                        }

                        num46 = num45;
                    }

                    var num40 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) <
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num46 / num40)
                    {
                        decimal num39;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                        {
                            num39 = bodyPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num38;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                            {
                                num38 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                decimal num37;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                {
                                    num37 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    decimal num34;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                    {
                                        decimal num35;
                                        decimal num36;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num36 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num36 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num35 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num35 = inClose[i - 1];
                                        }

                                        num34 = inHigh[i - 1] - num36 + (num35 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num34 = Decimal.Zero;
                                    }

                                    num37 = num34;
                                }

                                num38 = num37;
                            }

                            num39 = num38;
                        }

                        var num33 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                        if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <
                            (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num39 / num33 &&
                            inClose[i - 4] >= inOpen[i - 4] && inClose[i - 3] < inOpen[i - 3] && inClose[i] >= inOpen[i])
                        {
                            decimal num31;
                            decimal num32;
                            if (inOpen[i - 3] < inClose[i - 3])
                            {
                                num32 = inOpen[i - 3];
                            }
                            else
                            {
                                num32 = inClose[i - 3];
                            }

                            if (inOpen[i - 4] > inClose[i - 4])
                            {
                                num31 = inOpen[i - 4];
                            }
                            else
                            {
                                num31 = inClose[i - 4];
                            }

                            if (num32 > num31)
                            {
                                decimal num30;
                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num30 = inOpen[i - 2];
                                }
                                else
                                {
                                    num30 = inClose[i - 2];
                                }

                                if (num30 < inClose[i - 4])
                                {
                                    decimal num29;
                                    if (inOpen[i - 1] < inClose[i - 1])
                                    {
                                        num29 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num29 = inClose[i - 1];
                                    }

                                    if (num29 < inClose[i - 4])
                                    {
                                        decimal num28;
                                        if (inOpen[i - 2] < inClose[i - 2])
                                        {
                                            num28 = inOpen[i - 2];
                                        }
                                        else
                                        {
                                            num28 = inClose[i - 2];
                                        }

                                        if (num28 > inClose[i - 4] - Math.Abs(inClose[i - 4] - inOpen[i - 4]) * optInPenetration)
                                        {
                                            decimal num27;
                                            if (inOpen[i - 1] < inClose[i - 1])
                                            {
                                                num27 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num27 = inClose[i - 1];
                                            }

                                            if (num27 > inClose[i - 4] - Math.Abs(inClose[i - 4] - inOpen[i - 4]) * optInPenetration)
                                            {
                                                decimal num26;
                                                if (inClose[i - 2] > inOpen[i - 2])
                                                {
                                                    num26 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num26 = inOpen[i - 2];
                                                }

                                                if (num26 < inOpen[i - 3])
                                                {
                                                    decimal num24;
                                                    decimal num25;
                                                    if (inClose[i - 1] > inOpen[i - 1])
                                                    {
                                                        num25 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num25 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 2] > inOpen[i - 2])
                                                    {
                                                        num24 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num24 = inOpen[i - 2];
                                                    }

                                                    if (num25 < num24 && inOpen[i] > inClose[i - 1])
                                                    {
                                                        decimal num21;
                                                        decimal num23;
                                                        if (inHigh[i - 3] > inHigh[i - 2])
                                                        {
                                                            num23 = inHigh[i - 3];
                                                        }
                                                        else
                                                        {
                                                            num23 = inHigh[i - 2];
                                                        }

                                                        if (num23 > inHigh[i - 1])
                                                        {
                                                            decimal num22;
                                                            if (inHigh[i - 3] > inHigh[i - 2])
                                                            {
                                                                num22 = inHigh[i - 3];
                                                            }
                                                            else
                                                            {
                                                                num22 = inHigh[i - 2];
                                                            }

                                                            num21 = num22;
                                                        }
                                                        else
                                                        {
                                                            num21 = inHigh[i - 1];
                                                        }

                                                        if (inClose[i] > num21)
                                                        {
                                                            outInteger[outIdx] = 100;
                                                            outIdx++;
                                                            goto Label_0D26;
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
            Label_0D26:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 4] - inOpen[i - 4]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num17;
                        decimal num18;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num18 = inClose[i - 4];
                        }
                        else
                        {
                            num18 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num17 = inOpen[i - 4];
                        }
                        else
                        {
                            num17 = inClose[i - 4];
                        }

                        num16 = inHigh[i - 4] - num18 + (num17 - inLow[i - 4]);
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
                num15 = Math.Abs(inClose[bodyLongTrailingIdx - 4] - inOpen[bodyLongTrailingIdx - 4]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx - 4] - inLow[bodyLongTrailingIdx - 4];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num13 = inClose[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num13 = inOpen[bodyLongTrailingIdx - 4];
                        }

                        if (inClose[bodyLongTrailingIdx - 4] >= inOpen[bodyLongTrailingIdx - 4])
                        {
                            num12 = inOpen[bodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num12 = inClose[bodyLongTrailingIdx - 4];
                        }

                        num11 = inHigh[bodyLongTrailingIdx - 4] - num13 + (num12 - inLow[bodyLongTrailingIdx - 4]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyPeriodTotal[4] += num20 - num15;
            for (var totIdx = 3; totIdx >= 1; totIdx--)
            {
                decimal num5;
                decimal num10;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[bodyShortTrailingIdx - totIdx] - inOpen[bodyShortTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[bodyShortTrailingIdx - totIdx] - inLow[bodyShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num2;
                            decimal num3;
                            if (inClose[bodyShortTrailingIdx - totIdx] >= inOpen[bodyShortTrailingIdx - totIdx])
                            {
                                num3 = inClose[bodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[bodyShortTrailingIdx - totIdx];
                            }

                            if (inClose[bodyShortTrailingIdx - totIdx] >= inOpen[bodyShortTrailingIdx - totIdx])
                            {
                                num2 = inOpen[bodyShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[bodyShortTrailingIdx - totIdx];
                            }

                            num = inHigh[bodyShortTrailingIdx - totIdx] - num3 + (num2 - inLow[bodyShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = Decimal.Zero;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                bodyPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            bodyShortTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_04C0;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlMatHoldLookback()
        {
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.ShadowLong].AvgPeriod);

            return avgPeriod + 4;
        }
    }
}
