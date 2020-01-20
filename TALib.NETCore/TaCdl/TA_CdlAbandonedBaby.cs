using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlAbandonedBaby(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger, double optInPenetration = 0.3)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            double num52;
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

            int lookbackTotal = CdlAbandonedBabyLookback();
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
                double num67;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num66;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num63;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num64;
                            double num65;
                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inClose[i];
                            }
                            else
                            {
                                num65 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num64 = inOpen[i];
                            }
                            else
                            {
                                num64 = inClose[i];
                            }

                            num63 = inHigh[i] - num65 + (num64 - inLow[i]);
                        }
                        else
                        {
                            num63 = 0.0;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                bodyLongPeriodTotal += num67;
                i++;
            }

            i = bodyDojiTrailingIdx;
            while (true)
            {
                double num62;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num62 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num61;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num61 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num58;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                bodyDojiPeriodTotal += num62;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                double num57;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num57 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num56;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num56 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num53;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num54;
                            double num55;
                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inClose[i];
                            }
                            else
                            {
                                num55 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num54 = inOpen[i];
                            }
                            else
                            {
                                num54 = inClose[i];
                            }

                            num53 = inHigh[i] - num55 + (num54 - inLow[i]);
                        }
                        else
                        {
                            num53 = 0.0;
                        }

                        num56 = num53;
                    }

                    num57 = num56;
                }

                bodyShortPeriodTotal += num57;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_035B:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num52 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num51;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num51 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num50;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num50 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num47;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num48;
                            double num49;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inClose[i - 2];
                            }
                            else
                            {
                                num49 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num48 = inOpen[i - 2];
                            }
                            else
                            {
                                num48 = inClose[i - 2];
                            }

                            num47 = inHigh[i - 2] - num49 + (num48 - inLow[i - 2]);
                        }
                        else
                        {
                            num47 = 0.0;
                        }

                        num50 = num47;
                    }

                    num51 = num50;
                }

                num52 = num51;
            }

            var num46 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num52 / num46)
            {
                double num45;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                {
                    num45 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                }
                else
                {
                    double num44;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                    {
                        num44 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num43;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                        {
                            num43 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num40;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                    num45 = num44;
                }

                var num39 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                    Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num45 / num39)
                {
                    double num38;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num38 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        double num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num37 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num36;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num36 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num33;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    double num34;
                                    double num35;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num35 = inClose[i];
                                    }
                                    else
                                    {
                                        num35 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num34 = inOpen[i];
                                    }
                                    else
                                    {
                                        num34 = inClose[i];
                                    }

                                    num33 = inHigh[i] - num35 + (num34 - inLow[i]);
                                }
                                else
                                {
                                    num33 = 0.0;
                                }

                                num36 = num33;
                            }

                            num37 = num36;
                        }

                        num38 = num37;
                    }

                    var num32 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i] - inOpen[i]) >
                        Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num38 / num32 &&
                        (inClose[i - 2] >= inOpen[i - 2] && inClose[i] < inOpen[i] &&
                         inClose[i] < inClose[i - 2] - Math.Abs(inClose[i - 2] - inOpen[i - 2]) * optInPenetration &&
                         inLow[i - 1] > inHigh[i - 2] && inHigh[i] < inLow[i - 1] ||
                         inClose[i - 2] < inOpen[i - 2] && inClose[i] >= inOpen[i] &&
                         inClose[i] > inClose[i - 2] + Math.Abs(inClose[i - 2] - inOpen[i - 2]) * optInPenetration &&
                         inHigh[i - 1] < inLow[i - 2] && inLow[i] > inHigh[i - 1]))
                    {
                        int num31;
                        if (inClose[i] >= inOpen[i])
                        {
                            num31 = 1;
                        }
                        else
                        {
                            num31 = -1;
                        }

                        outInteger[outIdx] = num31 * 100;
                        outIdx++;
                        goto Label_0844;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0844:
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

        public static RetCode CdlAbandonedBaby(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger, decimal optInPenetration = 0.3m)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num25;
            decimal num30;
            decimal num52;
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

            int lookbackTotal = CdlAbandonedBabyLookback();
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
                decimal num67;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num66;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num63;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num64;
                            decimal num65;
                            if (inClose[i] >= inOpen[i])
                            {
                                num65 = inClose[i];
                            }
                            else
                            {
                                num65 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num64 = inOpen[i];
                            }
                            else
                            {
                                num64 = inClose[i];
                            }

                            num63 = inHigh[i] - num65 + (num64 - inLow[i]);
                        }
                        else
                        {
                            num63 = Decimal.Zero;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                bodyLongPeriodTotal += num67;
                i++;
            }

            i = bodyDojiTrailingIdx;
            while (true)
            {
                decimal num62;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num62 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num61;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num61 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num58;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                bodyDojiPeriodTotal += num62;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num57;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num57 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num56;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num56 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num53;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num54;
                            decimal num55;
                            if (inClose[i] >= inOpen[i])
                            {
                                num55 = inClose[i];
                            }
                            else
                            {
                                num55 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num54 = inOpen[i];
                            }
                            else
                            {
                                num54 = inClose[i];
                            }

                            num53 = inHigh[i] - num55 + (num54 - inLow[i]);
                        }
                        else
                        {
                            num53 = Decimal.Zero;
                        }

                        num56 = num53;
                    }

                    num57 = num56;
                }

                bodyShortPeriodTotal += num57;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0385:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num52 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num51;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num51 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num50;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num50 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num47;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num48;
                            decimal num49;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inClose[i - 2];
                            }
                            else
                            {
                                num49 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num48 = inOpen[i - 2];
                            }
                            else
                            {
                                num48 = inClose[i - 2];
                            }

                            num47 = inHigh[i - 2] - num49 + (num48 - inLow[i - 2]);
                        }
                        else
                        {
                            num47 = Decimal.Zero;
                        }

                        num50 = num47;
                    }

                    num51 = num50;
                }

                num52 = num51;
            }

            var num46 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num52 / num46)
            {
                decimal num45;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                {
                    num45 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                }
                else
                {
                    decimal num44;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                    {
                        num44 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num43;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                        {
                            num43 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num40;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                    num45 = num44;
                }

                var num39 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num45 / num39)
                {
                    decimal num38;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num38 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num37 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num36;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num36 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num33;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num34;
                                    decimal num35;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num35 = inClose[i];
                                    }
                                    else
                                    {
                                        num35 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num34 = inOpen[i];
                                    }
                                    else
                                    {
                                        num34 = inClose[i];
                                    }

                                    num33 = inHigh[i] - num35 + (num34 - inLow[i]);
                                }
                                else
                                {
                                    num33 = Decimal.Zero;
                                }

                                num36 = num33;
                            }

                            num37 = num36;
                        }

                        num38 = num37;
                    }

                    var num32 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i] - inOpen[i]) >
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num38 / num32 &&
                        (inClose[i - 2] >= inOpen[i - 2] && inClose[i] < inOpen[i] &&
                         inClose[i] < inClose[i - 2] - Math.Abs(inClose[i - 2] - inOpen[i - 2]) * optInPenetration &&
                         inLow[i - 1] > inHigh[i - 2] && inHigh[i] < inLow[i - 1] ||
                         inClose[i - 2] < inOpen[i - 2] && inClose[i] >= inOpen[i] &&
                         inClose[i] > inClose[i - 2] + Math.Abs(inClose[i - 2] - inOpen[i - 2]) * optInPenetration &&
                         inHigh[i - 1] < inLow[i - 2] && inLow[i] > inHigh[i - 1]))
                    {
                        int num31;
                        if (inClose[i] >= inOpen[i])
                        {
                            num31 = 1;
                        }
                        else
                        {
                            num31 = -1;
                        }

                        outInteger[outIdx] = num31 * 100;
                        outIdx++;
                        goto Label_08C5;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_08C5:
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

        private static int CdlAbandonedBabyLookback()
        {
            var avgPeriod = Math.Max(
                Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod,
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod),
                Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod
            );

            return avgPeriod + 2;
        }
    }
}
