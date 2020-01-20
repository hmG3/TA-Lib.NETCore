using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3Inside(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num39;
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

            int lookbackTotal = Cdl3InsideLookback();
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
                double num49;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                bodyLongPeriodTotal += num49;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                double num44;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num43;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num40;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num41;
                            double num42;
                            if (inClose[i] >= inOpen[i])
                            {
                                num42 = inClose[i];
                            }
                            else
                            {
                                num42 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num41 = inOpen[i];
                            }
                            else
                            {
                                num41 = inClose[i];
                            }

                            num40 = inHigh[i] - num42 + (num41 - inLow[i]);
                        }
                        else
                        {
                            num40 = 0.0;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                bodyShortPeriodTotal += num44;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0238:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num39 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                double num38;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num38 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num37;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num37 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num35;
                            double num36;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num36 = inClose[i - 2];
                            }
                            else
                            {
                                num36 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num35 = inOpen[i - 2];
                            }
                            else
                            {
                                num35 = inClose[i - 2];
                            }

                            num34 = inHigh[i - 2] - num36 + (num35 - inLow[i - 2]);
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

            var num33 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) > Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num39 / num33)
            {
                double num32;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                {
                    num32 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                }
                else
                {
                    double num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                    {
                        num31 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num30;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                        {
                            num30 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num27;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                            {
                                double num28;
                                double num29;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num29 = inClose[i - 1];
                                }
                                else
                                {
                                    num29 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num28 = inOpen[i - 1];
                                }
                                else
                                {
                                    num28 = inClose[i - 1];
                                }

                                num27 = inHigh[i - 1] - num29 + (num28 - inLow[i - 1]);
                            }
                            else
                            {
                                num27 = 0.0;
                            }

                            num30 = num27;
                        }

                        num31 = num30;
                    }

                    num32 = num31;
                }

                var num26 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                    Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num32 / num26)
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

                    if (num25 < num24)
                    {
                        double num22;
                        double num23;
                        if (inClose[i - 1] < inOpen[i - 1])
                        {
                            num23 = inClose[i - 1];
                        }
                        else
                        {
                            num23 = inOpen[i - 1];
                        }

                        if (inClose[i - 2] < inOpen[i - 2])
                        {
                            num22 = inClose[i - 2];
                        }
                        else
                        {
                            num22 = inOpen[i - 2];
                        }

                        if (num23 > num22 &&
                            (inClose[i - 2] >= inOpen[i - 2] && inClose[i] < inOpen[i] && inClose[i] < inOpen[i - 2] ||
                             inClose[i - 2] < inOpen[i - 2] && inClose[i] >= inOpen[i] && inClose[i] > inOpen[i - 2]))
                        {
                            int num21;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num21 = 1;
                            }
                            else
                            {
                                num21 = -1;
                            }

                            outInteger[outIdx] = -num21 * 100;
                            outIdx++;
                            goto Label_0610;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0610:
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

        public static RetCode Cdl3Inside(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num15;
            decimal num20;
            decimal num39;
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

            int lookbackTotal = Cdl3InsideLookback();
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
                decimal num49;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num45;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                bodyLongPeriodTotal += num49;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num44;
                if (i >= startIdx - 1)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num43;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num40;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num41;
                            decimal num42;
                            if (inClose[i] >= inOpen[i])
                            {
                                num42 = inClose[i];
                            }
                            else
                            {
                                num42 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num41 = inOpen[i];
                            }
                            else
                            {
                                num41 = inClose[i];
                            }

                            num40 = inHigh[i] - num42 + (num41 - inLow[i]);
                        }
                        else
                        {
                            num40 = Decimal.Zero;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                bodyShortPeriodTotal += num44;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0254:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
            {
                num39 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            }
            else
            {
                decimal num38;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num38 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num37;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num37 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num35;
                            decimal num36;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num36 = inClose[i - 2];
                            }
                            else
                            {
                                num36 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num35 = inOpen[i - 2];
                            }
                            else
                            {
                                num35 = inClose[i - 2];
                            }

                            num34 = inHigh[i - 2] - num36 + (num35 - inLow[i - 2]);
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

            var num33 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) >
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num39 / num33)
            {
                decimal num32;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                {
                    num32 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                }
                else
                {
                    decimal num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                    {
                        num31 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num30;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                        {
                            num30 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num27;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                            {
                                decimal num28;
                                decimal num29;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num29 = inClose[i - 1];
                                }
                                else
                                {
                                    num29 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num28 = inOpen[i - 1];
                                }
                                else
                                {
                                    num28 = inClose[i - 1];
                                }

                                num27 = inHigh[i - 1] - num29 + (num28 - inLow[i - 1]);
                            }
                            else
                            {
                                num27 = Decimal.Zero;
                            }

                            num30 = num27;
                        }

                        num31 = num30;
                    }

                    num32 = num31;
                }

                var num26 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num32 / num26)
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

                    if (num25 < num24)
                    {
                        decimal num22;
                        decimal num23;
                        if (inClose[i - 1] < inOpen[i - 1])
                        {
                            num23 = inClose[i - 1];
                        }
                        else
                        {
                            num23 = inOpen[i - 1];
                        }

                        if (inClose[i - 2] < inOpen[i - 2])
                        {
                            num22 = inClose[i - 2];
                        }
                        else
                        {
                            num22 = inOpen[i - 2];
                        }

                        if (num23 > num22 &&
                            (inClose[i - 2] >= inOpen[i - 2] && inClose[i] < inOpen[i] && inClose[i] < inOpen[i - 2] ||
                             inClose[i - 2] < inOpen[i - 2] && inClose[i] >= inOpen[i] && inClose[i] > inOpen[i - 2]))
                        {
                            int num21;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num21 = 1;
                            }
                            else
                            {
                                num21 = -1;
                            }

                            outInteger[outIdx] = -num21 * 100;
                            outIdx++;
                            goto Label_0672;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0672:
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

        public static int Cdl3InsideLookback()
        {
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod);

            return avgPeriod + 2;
        }
    }
}
