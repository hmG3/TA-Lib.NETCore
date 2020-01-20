using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlShortLine(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num44;
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

            int lookbackTotal = CdlShortLineLookback();
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

            double bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            double shadowPeriodTotal = default;
            int shadowTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                double num54;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num53;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num50;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                bodyPeriodTotal += num54;
                i++;
            }

            i = shadowTrailingIdx;
            while (true)
            {
                double num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            double num46;
                            double num47;
                            if (inClose[i] >= inOpen[i])
                            {
                                num47 = inClose[i];
                            }
                            else
                            {
                                num47 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inOpen[i];
                            }
                            else
                            {
                                num46 = inClose[i];
                            }

                            num45 = inHigh[i] - num47 + (num46 - inLow[i]);
                        }
                        else
                        {
                            num45 = 0.0;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                shadowPeriodTotal += num49;
                i++;
            }

            int outIdx = default;
            Label_022E:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
            {
                num44 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            }
            else
            {
                double num43;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num43 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num42;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num42 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num39;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

            var num38 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) < Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num44 / num38)
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

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
                {
                    num36 = shadowPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
                }
                else
                {
                    double num35;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num31;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
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

                var num30 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (inHigh[i] - num37 < Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num36 / num30)
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

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
                    {
                        num28 = shadowPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
                    }
                    else
                    {
                        double num27;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                        {
                            num27 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num26;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                            {
                                num26 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num23;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
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

                    var num22 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (num29 - inLow[i] < Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num28 / num22)
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
                        goto Label_062D;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_062D:
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
                num15 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num13 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num12 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyTrailingIdx];
                        }

                        num11 = inHigh[bodyTrailingIdx] - num13 + (num12 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowTrailingIdx] - inOpen[shadowTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowTrailingIdx] - inLow[shadowTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[shadowTrailingIdx] >= inOpen[shadowTrailingIdx])
                        {
                            num3 = inClose[shadowTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowTrailingIdx];
                        }

                        if (inClose[shadowTrailingIdx] >= inOpen[shadowTrailingIdx])
                        {
                            num2 = inOpen[shadowTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowTrailingIdx];
                        }

                        num = inHigh[shadowTrailingIdx] - num3 + (num2 - inLow[shadowTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_022E;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlShortLine(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num44;
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

            int lookbackTotal = CdlShortLineLookback();
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

            decimal bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            decimal shadowPeriodTotal = default;
            int shadowTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                decimal num54;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num53;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num50;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                bodyPeriodTotal += num54;
                i++;
            }

            i = shadowTrailingIdx;
            while (true)
            {
                decimal num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                        {
                            decimal num46;
                            decimal num47;
                            if (inClose[i] >= inOpen[i])
                            {
                                num47 = inClose[i];
                            }
                            else
                            {
                                num47 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inOpen[i];
                            }
                            else
                            {
                                num46 = inClose[i];
                            }

                            num45 = inHigh[i] - num47 + (num46 - inLow[i]);
                        }
                        else
                        {
                            num45 = Decimal.Zero;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                shadowPeriodTotal += num49;
                i++;
            }

            int outIdx = default;
            Label_024A:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
            {
                num44 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            }
            else
            {
                decimal num43;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num43 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num42;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num42 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num39;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

            var num38 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i] - inOpen[i]) <
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num44 / num38)
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

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
                {
                    num36 = shadowPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
                }
                else
                {
                    decimal num35;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        decimal num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            decimal num31;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
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

                var num30 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (inHigh[i] - num37 < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num36 / num30)
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

                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod != 0)
                    {
                        num28 = shadowPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num27;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
                        {
                            num27 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num26;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                            {
                                num26 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num23;
                                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
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

                    var num22 = Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (num29 - inLow[i] < (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowShort].Factor * num28 / num22)
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
                        goto Label_0681;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0681:
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
                num15 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num13 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num12 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyTrailingIdx];
                        }

                        num11 = inHigh[bodyTrailingIdx] - num13 + (num12 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowTrailingIdx] - inOpen[shadowTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowTrailingIdx] - inLow[shadowTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowShort].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[shadowTrailingIdx] >= inOpen[shadowTrailingIdx])
                        {
                            num3 = inClose[shadowTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowTrailingIdx];
                        }

                        if (inClose[shadowTrailingIdx] >= inOpen[shadowTrailingIdx])
                        {
                            num2 = inOpen[shadowTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowTrailingIdx];
                        }

                        num = inHigh[shadowTrailingIdx] - num3 + (num2 - inLow[shadowTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_024A;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlShortLineLookback()
        {
            return Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.ShadowShort].AvgPeriod);
        }
    }
}
