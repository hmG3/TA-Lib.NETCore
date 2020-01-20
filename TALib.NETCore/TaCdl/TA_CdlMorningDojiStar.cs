using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlMorningDojiStar(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger, double optInPenetration = 0.3)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            double num53;
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

            int lookbackTotal = CdlMorningDojiStarLookback();
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
            double bodyDojiPeriodTotal = default;
            double bodyShortPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - 2 - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int bodyDojiTrailingIdx = startIdx - 1 - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num68;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num68 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num67;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num67 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num64;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num65;
                            double num66;
                            if (inClose[i] >= inOpen[i])
                            {
                                num66 = inClose[i];
                            }
                            else
                            {
                                num66 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inOpen[i];
                            }
                            else
                            {
                                num65 = inClose[i];
                            }

                            num64 = inHigh[i] - num66 + (num65 - inLow[i]);
                        }
                        else
                        {
                            num64 = 0.0;
                        }

                        num67 = num64;
                    }

                    num68 = num67;
                }

                bodyLongPeriodTotal += num68;
                i++;
            }

            i = bodyDojiTrailingIdx;
            while (true)
            {
                double num63;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num63 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num62;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num62 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num59;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num60;
                            double num61;
                            if (inClose[i] >= inOpen[i])
                            {
                                num61 = inClose[i];
                            }
                            else
                            {
                                num61 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num60 = inOpen[i];
                            }
                            else
                            {
                                num60 = inClose[i];
                            }

                            num59 = inHigh[i] - num61 + (num60 - inLow[i]);
                        }
                        else
                        {
                            num59 = 0.0;
                        }

                        num62 = num59;
                    }

                    num63 = num62;
                }

                bodyDojiPeriodTotal += num63;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                double num58;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num58 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num57;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num57 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num54;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num55;
                            double num56;
                            if (inClose[i] >= inOpen[i])
                            {
                                num56 = inClose[i];
                            }
                            else
                            {
                                num56 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inOpen[i];
                            }
                            else
                            {
                                num55 = inClose[i];
                            }

                            num54 = inHigh[i] - num56 + (num55 - inLow[i]);
                        }
                        else
                        {
                            num54 = 0.0;
                        }

                        num57 = num54;
                    }

                    num58 = num57;
                }

                bodyShortPeriodTotal += num58;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_035B:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num53 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num52;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num52 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num51;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num51 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num48;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num49;
                            double num50;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num50 = inClose[i - 2];
                            }
                            else
                            {
                                num50 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inOpen[i - 2];
                            }
                            else
                            {
                                num49 = inClose[i - 2];
                            }

                            num48 = inHigh[i - 2] - num50 + (num49 - inLow[i - 2]);
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

            var num47 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num53 / num47 && inClose[i - 2] < inOpen[i - 2])
            {
                double num46;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                {
                    num46 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                }
                else
                {
                    double num45;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                    {
                        num45 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num44;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                        {
                            num44 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num41;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                            {
                                double num42;
                                double num43;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num43 = inClose[i - 1];
                                }
                                else
                                {
                                    num43 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num42 = inOpen[i - 1];
                                }
                                else
                                {
                                    num42 = inClose[i - 1];
                                }

                                num41 = inHigh[i - 1] - num43 + (num42 - inLow[i - 1]);
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

                var num40 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                    Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num46 / num40)
                {
                    double num38;
                    double num39;
                    if (inOpen[i - 1] > inClose[i - 1])
                    {
                        num39 = inOpen[i - 1];
                    }
                    else
                    {
                        num39 = inClose[i - 1];
                    }

                    if (inOpen[i - 2] < inClose[i - 2])
                    {
                        num38 = inOpen[i - 2];
                    }
                    else
                    {
                        num38 = inClose[i - 2];
                    }

                    if (num39 < num38)
                    {
                        double num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                        {
                            num37 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                        }
                        else
                        {
                            double num36;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                            {
                                num36 = Math.Abs(inClose[i] - inOpen[i]);
                            }
                            else
                            {
                                double num35;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                {
                                    num35 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    double num32;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                    {
                                        double num33;
                                        double num34;
                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num34 = inClose[i];
                                        }
                                        else
                                        {
                                            num34 = inOpen[i];
                                        }

                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num33 = inOpen[i];
                                        }
                                        else
                                        {
                                            num33 = inClose[i];
                                        }

                                        num32 = inHigh[i] - num34 + (num33 - inLow[i]);
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

                        var num31 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                        if (Math.Abs(inClose[i] - inOpen[i]) >
                            Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num37 / num31 && inClose[i] >= inOpen[i] &&
                            inClose[i] > inClose[i - 2] + Math.Abs(inClose[i - 2] - inOpen[i - 2]) * optInPenetration)
                        {
                            outInteger[outIdx] = 100;
                            outIdx++;
                            goto Label_0800;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0800:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                double num29;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num23 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num22 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[bodyLongTrailingIdx];
                        }

                        num21 = inHigh[bodyLongTrailingIdx] - num23 + (num22 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            bodyLongPeriodTotal += num30 - num25;
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyDojiTrailingIdx] - inOpen[bodyDojiTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyDojiTrailingIdx] - inLow[bodyDojiTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num13 = inClose[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyDojiTrailingIdx];
                        }

                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num12 = inOpen[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyDojiTrailingIdx];
                        }

                        num11 = inHigh[bodyDojiTrailingIdx] - num13 + (num12 - inLow[bodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyDojiPeriodTotal += num20 - num15;
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
            bodyDojiTrailingIdx++;
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_035B;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlMorningDojiStar(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger, decimal optInPenetration = 0.3m)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num25;
            decimal num30;
            decimal num53;
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

            int lookbackTotal = CdlMorningDojiStarLookback();
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
            decimal bodyDojiPeriodTotal = default;
            decimal bodyShortPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - 2 - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int bodyDojiTrailingIdx = startIdx - 1 - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num68;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num68 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num67;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num67 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num64;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num65;
                            decimal num66;
                            if (inClose[i] >= inOpen[i])
                            {
                                num66 = inClose[i];
                            }
                            else
                            {
                                num66 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inOpen[i];
                            }
                            else
                            {
                                num65 = inClose[i];
                            }

                            num64 = inHigh[i] - num66 + (num65 - inLow[i]);
                        }
                        else
                        {
                            num64 = Decimal.Zero;
                        }

                        num67 = num64;
                    }

                    num68 = num67;
                }

                bodyLongPeriodTotal += num68;
                i++;
            }

            i = bodyDojiTrailingIdx;
            while (true)
            {
                decimal num63;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num63 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num62;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num62 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num59;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num60;
                            decimal num61;
                            if (inClose[i] >= inOpen[i])
                            {
                                num61 = inClose[i];
                            }
                            else
                            {
                                num61 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num60 = inOpen[i];
                            }
                            else
                            {
                                num60 = inClose[i];
                            }

                            num59 = inHigh[i] - num61 + (num60 - inLow[i]);
                        }
                        else
                        {
                            num59 = Decimal.Zero;
                        }

                        num62 = num59;
                    }

                    num63 = num62;
                }

                bodyDojiPeriodTotal += num63;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num58;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num58 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num57;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num57 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num54;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num55;
                            decimal num56;
                            if (inClose[i] >= inOpen[i])
                            {
                                num56 = inClose[i];
                            }
                            else
                            {
                                num56 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inOpen[i];
                            }
                            else
                            {
                                num55 = inClose[i];
                            }

                            num54 = inHigh[i] - num56 + (num55 - inLow[i]);
                        }
                        else
                        {
                            num54 = Decimal.Zero;
                        }

                        num57 = num54;
                    }

                    num58 = num57;
                }

                bodyShortPeriodTotal += num58;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0385:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num53 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num52;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num52 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num51;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num51 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num48;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num49;
                            decimal num50;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num50 = inClose[i - 2];
                            }
                            else
                            {
                                num50 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inOpen[i - 2];
                            }
                            else
                            {
                                num49 = inClose[i - 2];
                            }

                            num48 = inHigh[i - 2] - num50 + (num49 - inLow[i - 2]);
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

            var num47 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num53 / num47 && inClose[i - 2] < inOpen[i - 2])
            {
                decimal num46;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                {
                    num46 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                }
                else
                {
                    decimal num45;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                    {
                        num45 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num44;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                        {
                            num44 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num41;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                            {
                                decimal num42;
                                decimal num43;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num43 = inClose[i - 1];
                                }
                                else
                                {
                                    num43 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num42 = inOpen[i - 1];
                                }
                                else
                                {
                                    num42 = inClose[i - 1];
                                }

                                num41 = inHigh[i - 1] - num43 + (num42 - inLow[i - 1]);
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

                var num40 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num46 / num40)
                {
                    decimal num38;
                    decimal num39;
                    if (inOpen[i - 1] > inClose[i - 1])
                    {
                        num39 = inOpen[i - 1];
                    }
                    else
                    {
                        num39 = inClose[i - 1];
                    }

                    if (inOpen[i - 2] < inClose[i - 2])
                    {
                        num38 = inOpen[i - 2];
                    }
                    else
                    {
                        num38 = inClose[i - 2];
                    }

                    if (num39 < num38)
                    {
                        decimal num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                        {
                            num37 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                        }
                        else
                        {
                            decimal num36;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                            {
                                num36 = Math.Abs(inClose[i] - inOpen[i]);
                            }
                            else
                            {
                                decimal num35;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                                {
                                    num35 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    decimal num32;
                                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                    {
                                        decimal num33;
                                        decimal num34;
                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num34 = inClose[i];
                                        }
                                        else
                                        {
                                            num34 = inOpen[i];
                                        }

                                        if (inClose[i] >= inOpen[i])
                                        {
                                            num33 = inOpen[i];
                                        }
                                        else
                                        {
                                            num33 = inClose[i];
                                        }

                                        num32 = inHigh[i] - num34 + (num33 - inLow[i]);
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

                        var num31 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                        if (Math.Abs(inClose[i] - inOpen[i]) >
                            (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num37 / num31 &&
                            inClose[i] >= inOpen[i] &&
                            inClose[i] > inClose[i - 2] + Math.Abs(inClose[i - 2] - inOpen[i - 2]) * optInPenetration)
                        {
                            outInteger[outIdx] = 100;
                            outIdx++;
                            goto Label_0874;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0874:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                decimal num29;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    decimal num26;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num24 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    decimal num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num22;
                        decimal num23;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num23 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num22 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[bodyLongTrailingIdx];
                        }

                        num21 = inHigh[bodyLongTrailingIdx] - num23 + (num22 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num21 = Decimal.Zero;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            bodyLongPeriodTotal += num30 - num25;
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyDojiTrailingIdx] - inOpen[bodyDojiTrailingIdx]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyDojiTrailingIdx] - inLow[bodyDojiTrailingIdx];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num13 = inClose[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyDojiTrailingIdx];
                        }

                        if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                        {
                            num12 = inOpen[bodyDojiTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyDojiTrailingIdx];
                        }

                        num11 = inHigh[bodyDojiTrailingIdx] - num13 + (num12 - inLow[bodyDojiTrailingIdx]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyDojiPeriodTotal += num20 - num15;
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
            bodyDojiTrailingIdx++;
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0385;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlMorningDojiStarLookback()
        {
            int avgPeriod = Math.Max(
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod),
                Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod
            );

            return avgPeriod + 2;
        }
    }
}
