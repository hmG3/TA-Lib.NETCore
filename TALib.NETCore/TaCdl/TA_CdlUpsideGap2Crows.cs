using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlUpsideGap2Crows(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
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

            int lookbackTotal = CdlUpsideGap2CrowsLookback();
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
            double bodyShortPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - 2 - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int bodyShortTrailingIdx = startIdx - 1 - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num46;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num46 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num45;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num45 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num42;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                bodyLongPeriodTotal += num46;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                double num41;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num41 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num40;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num40 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num38;
                            double num39;
                            if (inClose[i] >= inOpen[i])
                            {
                                num39 = inClose[i];
                            }
                            else
                            {
                                num39 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num38 = inOpen[i];
                            }
                            else
                            {
                                num38 = inClose[i];
                            }

                            num37 = inHigh[i] - num39 + (num38 - inLow[i]);
                        }
                        else
                        {
                            num37 = 0.0;
                        }

                        num40 = num37;
                    }

                    num41 = num40;
                }

                bodyShortPeriodTotal += num41;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0238:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num36;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num36 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num35;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num31;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                double num32;
                                double num33;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num33 = inClose[i - 2];
                                }
                                else
                                {
                                    num33 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num32 = inOpen[i - 2];
                                }
                                else
                                {
                                    num32 = inClose[i - 2];
                                }

                                num31 = inHigh[i - 2] - num33 + (num32 - inLow[i - 2]);
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

                var num30 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num36 / num30 && inClose[i - 1] < inOpen[i - 1])
                {
                    double num29;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num29 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        double num28;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num28 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num27;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num27 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num24;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    double num25;
                                    double num26;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num26 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num26 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num25 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num25 = inClose[i - 1];
                                    }

                                    num24 = inHigh[i - 1] - num26 + (num25 - inLow[i - 1]);
                                }
                                else
                                {
                                    num24 = 0.0;
                                }

                                num27 = num24;
                            }

                            num28 = num27;
                        }

                        num29 = num28;
                    }

                    var num23 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                        Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num29 / num23)
                    {
                        double num21;
                        double num22;
                        if (inOpen[i - 1] < inClose[i - 1])
                        {
                            num22 = inOpen[i - 1];
                        }
                        else
                        {
                            num22 = inClose[i - 1];
                        }

                        if (inOpen[i - 2] > inClose[i - 2])
                        {
                            num21 = inOpen[i - 2];
                        }
                        else
                        {
                            num21 = inClose[i - 2];
                        }

                        if (num22 > num21 && inClose[i] < inOpen[i] && inOpen[i] > inOpen[i - 1] &&
                            inClose[i] < inClose[i - 1] && inClose[i] > inClose[i - 2])
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_05B1;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_05B1:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num18 = inClose[i - 2];
                        }
                        else
                        {
                            num18 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num17 = inOpen[i - 2];
                        }
                        else
                        {
                            num17 = inClose[i - 2];
                        }

                        num16 = inHigh[i - 2] - num18 + (num17 - inLow[i - 2]);
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
                num15 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num13 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num12 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyLongTrailingIdx];
                        }

                        num11 = inHigh[bodyLongTrailingIdx] - num13 + (num12 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyLongPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0238;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlUpsideGap2Crows(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
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

            int lookbackTotal = CdlUpsideGap2CrowsLookback();
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
            decimal bodyShortPeriodTotal = default;
            int bodyLongTrailingIdx = startIdx - 2 - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int bodyShortTrailingIdx = startIdx - 1 - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num46;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num46 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num45;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num45 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num42;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                bodyLongPeriodTotal += num46;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num41;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num41 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num40;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num40 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num37;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num38;
                            decimal num39;
                            if (inClose[i] >= inOpen[i])
                            {
                                num39 = inClose[i];
                            }
                            else
                            {
                                num39 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num38 = inOpen[i];
                            }
                            else
                            {
                                num38 = inClose[i];
                            }

                            num37 = inHigh[i] - num39 + (num38 - inLow[i]);
                        }
                        else
                        {
                            num37 = Decimal.Zero;
                        }

                        num40 = num37;
                    }

                    num41 = num40;
                }

                bodyShortPeriodTotal += num41;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0254:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                decimal num36;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num36 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num35;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num31;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                decimal num32;
                                decimal num33;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num33 = inClose[i - 2];
                                }
                                else
                                {
                                    num33 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num32 = inOpen[i - 2];
                                }
                                else
                                {
                                    num32 = inClose[i - 2];
                                }

                                num31 = inHigh[i - 2] - num33 + (num32 - inLow[i - 2]);
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

                var num30 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num36 / num30 &&
                    inClose[i - 1] < inOpen[i - 1])
                {
                    decimal num29;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num29 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num28;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num28 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num27;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num27 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num24;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num25;
                                    decimal num26;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num26 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num26 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num25 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num25 = inClose[i - 1];
                                    }

                                    num24 = inHigh[i - 1] - num26 + (num25 - inLow[i - 1]);
                                }
                                else
                                {
                                    num24 = Decimal.Zero;
                                }

                                num27 = num24;
                            }

                            num28 = num27;
                        }

                        num29 = num28;
                    }

                    var num23 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num29 / num23)
                    {
                        decimal num21;
                        decimal num22;
                        if (inOpen[i - 1] < inClose[i - 1])
                        {
                            num22 = inOpen[i - 1];
                        }
                        else
                        {
                            num22 = inClose[i - 1];
                        }

                        if (inOpen[i - 2] > inClose[i - 2])
                        {
                            num21 = inOpen[i - 2];
                        }
                        else
                        {
                            num21 = inClose[i - 2];
                        }

                        if (num22 > num21 && inClose[i] < inOpen[i] && inOpen[i] > inOpen[i - 1] &&
                            inClose[i] < inClose[i - 1] && inClose[i] > inClose[i - 2])
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_0607;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0607:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num17;
                        decimal num18;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num18 = inClose[i - 2];
                        }
                        else
                        {
                            num18 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num17 = inOpen[i - 2];
                        }
                        else
                        {
                            num17 = inClose[i - 2];
                        }

                        num16 = inHigh[i - 2] - num18 + (num17 - inLow[i - 2]);
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
                num15 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num13 = inClose[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[bodyLongTrailingIdx];
                        }

                        if (inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx])
                        {
                            num12 = inOpen[bodyLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[bodyLongTrailingIdx];
                        }

                        num11 = inHigh[bodyLongTrailingIdx] - num13 + (num12 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyLongPeriodTotal += num20 - num15;
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0254;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlUpsideGap2CrowsLookback()
        {
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod);

            return avgPeriod + 2;
        }
    }
}
