using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlHomingPigeon(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
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

            int lookbackTotal = CdlHomingPigeonLookback();
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
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num44;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num43;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num40;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                bodyLongPeriodTotal += num44;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                double num39;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num38;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num35;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                            if (inClose[i] >= inOpen[i])
                            {
                                num36 = inOpen[i];
                            }
                            else
                            {
                                num36 = inClose[i];
                            }

                            num35 = inHigh[i] - num37 + (num36 - inLow[i]);
                        }
                        else
                        {
                            num35 = 0.0;
                        }

                        num38 = num35;
                    }

                    num39 = num38;
                }

                bodyShortPeriodTotal += num39;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_024F:
            if (inClose[i - 1] < inOpen[i - 1] && inClose[i] < inOpen[i])
            {
                double num34;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num34 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num33;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num33 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num32;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num32 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num29;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                double num30;
                                double num31;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num31 = inClose[i - 1];
                                }
                                else
                                {
                                    num31 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num30 = inOpen[i - 1];
                                }
                                else
                                {
                                    num30 = inClose[i - 1];
                                }

                                num29 = inHigh[i - 1] - num31 + (num30 - inLow[i - 1]);
                            }
                            else
                            {
                                num29 = 0.0;
                            }

                            num32 = num29;
                        }

                        num33 = num32;
                    }

                    num34 = num33;
                }

                var num28 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num34 / num28)
                {
                    double num27;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num27 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        double num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num26 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num25;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num25 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num22;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    double num23;
                                    double num24;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num24 = inClose[i];
                                    }
                                    else
                                    {
                                        num24 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num23 = inOpen[i];
                                    }
                                    else
                                    {
                                        num23 = inClose[i];
                                    }

                                    num22 = inHigh[i] - num24 + (num23 - inLow[i]);
                                }
                                else
                                {
                                    num22 = 0.0;
                                }

                                num25 = num22;
                            }

                            num26 = num25;
                        }

                        num27 = num26;
                    }

                    var num21 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i] - inOpen[i]) <=
                        Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num27 / num21 && inOpen[i] < inOpen[i - 1] &&
                        inClose[i] > inClose[i - 1])
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0540;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0540:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyLongTrailingIdx - 1] - inOpen[bodyLongTrailingIdx - 1]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx - 1] - inLow[bodyLongTrailingIdx - 1];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num13 = inClose[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[bodyLongTrailingIdx - 1];
                        }

                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num12 = inOpen[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[bodyLongTrailingIdx - 1];
                        }

                        num11 = inHigh[bodyLongTrailingIdx - 1] - num13 + (num12 - inLow[bodyLongTrailingIdx - 1]);
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
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_024F;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlHomingPigeon(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
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

            int lookbackTotal = CdlHomingPigeonLookback();
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
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int bodyShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num44;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num43;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num40;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                bodyLongPeriodTotal += num44;
                i++;
            }

            i = bodyShortTrailingIdx;
            while (true)
            {
                decimal num39;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num38;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num35;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                            if (inClose[i] >= inOpen[i])
                            {
                                num36 = inOpen[i];
                            }
                            else
                            {
                                num36 = inClose[i];
                            }

                            num35 = inHigh[i] - num37 + (num36 - inLow[i]);
                        }
                        else
                        {
                            num35 = Decimal.Zero;
                        }

                        num38 = num35;
                    }

                    num39 = num38;
                }

                bodyShortPeriodTotal += num39;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_026B:
            if (inClose[i - 1] < inOpen[i - 1] && inClose[i] < inOpen[i])
            {
                decimal num34;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num34 = bodyLongPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num33;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num33 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num32;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num32 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num29;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                decimal num30;
                                decimal num31;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num31 = inClose[i - 1];
                                }
                                else
                                {
                                    num31 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num30 = inOpen[i - 1];
                                }
                                else
                                {
                                    num30 = inClose[i - 1];
                                }

                                num29 = inHigh[i - 1] - num31 + (num30 - inLow[i - 1]);
                            }
                            else
                            {
                                num29 = Decimal.Zero;
                            }

                            num32 = num29;
                        }

                        num33 = num32;
                    }

                    num34 = num33;
                }

                var num28 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num34 / num28)
                {
                    decimal num27;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
                    {
                        num27 = bodyShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
                    }
                    else
                    {
                        decimal num26;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                        {
                            num26 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num25;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                            {
                                num25 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num22;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                                {
                                    decimal num23;
                                    decimal num24;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num24 = inClose[i];
                                    }
                                    else
                                    {
                                        num24 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num23 = inOpen[i];
                                    }
                                    else
                                    {
                                        num23 = inClose[i];
                                    }

                                    num22 = inHigh[i] - num24 + (num23 - inLow[i]);
                                }
                                else
                                {
                                    num22 = Decimal.Zero;
                                }

                                num25 = num22;
                            }

                            num26 = num25;
                        }

                        num27 = num26;
                    }

                    var num21 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i] - inOpen[i]) <=
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num27 / num21 &&
                        inOpen[i] < inOpen[i - 1] && inClose[i] > inClose[i - 1])
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0588;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0588:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyLongTrailingIdx - 1] - inOpen[bodyLongTrailingIdx - 1]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx - 1] - inLow[bodyLongTrailingIdx - 1];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num13 = inClose[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[bodyLongTrailingIdx - 1];
                        }

                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num12 = inOpen[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[bodyLongTrailingIdx - 1];
                        }

                        num11 = inHigh[bodyLongTrailingIdx - 1] - num13 + (num12 - inLow[bodyLongTrailingIdx - 1]);
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
            bodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_026B;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlHomingPigeonLookback()
        {
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod);

            return avgPeriod + 1;
        }
    }
}
