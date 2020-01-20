using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlPiercing(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            var bodyLongPeriodTotal = new double[2];
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

            int lookbackTotal = CdlPiercingLookback();
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
            int i = bodyLongTrailingIdx;
            while (true)
            {
                double num29;
                double num34;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num34 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num33;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num33 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num30;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num31;
                            double num32;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num32 = inClose[i - 1];
                            }
                            else
                            {
                                num32 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num31 = inOpen[i - 1];
                            }
                            else
                            {
                                num31 = inClose[i - 1];
                            }

                            num30 = inHigh[i - 1] - num32 + (num31 - inLow[i - 1]);
                        }
                        else
                        {
                            num30 = 0.0;
                        }

                        num33 = num30;
                    }

                    num34 = num33;
                }

                bodyLongPeriodTotal[1] += num34;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num29 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num28;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num25;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num26;
                            double num27;
                            if (inClose[i] >= inOpen[i])
                            {
                                num27 = inClose[i];
                            }
                            else
                            {
                                num27 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num26 = inOpen[i];
                            }
                            else
                            {
                                num26 = inClose[i];
                            }

                            num25 = inHigh[i] - num27 + (num26 - inLow[i]);
                        }
                        else
                        {
                            num25 = 0.0;
                        }

                        num28 = num25;
                    }

                    num29 = num28;
                }

                bodyLongPeriodTotal[0] += num29;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0237:
            if (inClose[i - 1] < inOpen[i - 1])
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num24 = bodyLongPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num23;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num23 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num22;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num19;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                double num20;
                                double num21;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num21 = inClose[i - 1];
                                }
                                else
                                {
                                    num21 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num20 = inOpen[i - 1];
                                }
                                else
                                {
                                    num20 = inClose[i - 1];
                                }

                                num19 = inHigh[i - 1] - num21 + (num20 - inLow[i - 1]);
                            }
                            else
                            {
                                num19 = 0.0;
                            }

                            num22 = num19;
                        }

                        num23 = num22;
                    }

                    num24 = num23;
                }

                var num18 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num24 / num18 && inClose[i] >= inOpen[i])
                {
                    double num17;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                    {
                        num17 = bodyLongPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                    }
                    else
                    {
                        double num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                        {
                            num16 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num15;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num12;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                {
                                    double num13;
                                    double num14;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num14 = inClose[i];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num13 = inOpen[i];
                                    }
                                    else
                                    {
                                        num13 = inClose[i];
                                    }

                                    num12 = inHigh[i] - num14 + (num13 - inLow[i]);
                                }
                                else
                                {
                                    num12 = 0.0;
                                }

                                num15 = num12;
                            }

                            num16 = num15;
                        }

                        num17 = num16;
                    }

                    var num11 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i] - inOpen[i]) >
                        Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num17 / num11 && inOpen[i] < inLow[i - 1] &&
                        inClose[i] < inOpen[i - 1] && inClose[i] > inClose[i - 1] + Math.Abs(inClose[i - 1] - inOpen[i - 1]) * 0.5)
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0558;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0558:
            var totIdx = 1;
            while (totIdx >= 0)
            {
                double num5;
                double num10;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[bodyLongTrailingIdx - totIdx] - inOpen[bodyLongTrailingIdx - totIdx]);
                }
                else
                {
                    double num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[bodyLongTrailingIdx - totIdx] - inLow[bodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num3 = inClose[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[bodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num2 = inOpen[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[bodyLongTrailingIdx - totIdx];
                            }

                            num = inHigh[bodyLongTrailingIdx - totIdx] - num3 + (num2 - inLow[bodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                bodyLongPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0237;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlPiercing(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            var bodyLongPeriodTotal = new decimal[2];
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

            int lookbackTotal = CdlPiercingLookback();
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
            int i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num29;
                decimal num34;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num34 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num33;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num33 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num30;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num31;
                            decimal num32;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num32 = inClose[i - 1];
                            }
                            else
                            {
                                num32 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num31 = inOpen[i - 1];
                            }
                            else
                            {
                                num31 = inClose[i - 1];
                            }

                            num30 = inHigh[i - 1] - num32 + (num31 - inLow[i - 1]);
                        }
                        else
                        {
                            num30 = Decimal.Zero;
                        }

                        num33 = num30;
                    }

                    num34 = num33;
                }

                bodyLongPeriodTotal[1] += num34;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num29 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num28;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num25;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num26;
                            decimal num27;
                            if (inClose[i] >= inOpen[i])
                            {
                                num27 = inClose[i];
                            }
                            else
                            {
                                num27 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num26 = inOpen[i];
                            }
                            else
                            {
                                num26 = inClose[i];
                            }

                            num25 = inHigh[i] - num27 + (num26 - inLow[i]);
                        }
                        else
                        {
                            num25 = Decimal.Zero;
                        }

                        num28 = num25;
                    }

                    num29 = num28;
                }

                bodyLongPeriodTotal[0] += num29;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0253:
            if (inClose[i - 1] < inOpen[i - 1])
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num24 = bodyLongPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num23;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num23 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num22;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num19;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                decimal num20;
                                decimal num21;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num21 = inClose[i - 1];
                                }
                                else
                                {
                                    num21 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num20 = inOpen[i - 1];
                                }
                                else
                                {
                                    num20 = inClose[i - 1];
                                }

                                num19 = inHigh[i - 1] - num21 + (num20 - inLow[i - 1]);
                            }
                            else
                            {
                                num19 = Decimal.Zero;
                            }

                            num22 = num19;
                        }

                        num23 = num22;
                    }

                    num24 = num23;
                }

                var num18 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num24 / num18 && inClose[i] >= inOpen[i])
                {
                    decimal num17;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                    {
                        num17 = bodyLongPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                    }
                    else
                    {
                        decimal num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                        {
                            num16 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num15;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num12;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                {
                                    decimal num13;
                                    decimal num14;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num14 = inClose[i];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num13 = inOpen[i];
                                    }
                                    else
                                    {
                                        num13 = inClose[i];
                                    }

                                    num12 = inHigh[i] - num14 + (num13 - inLow[i]);
                                }
                                else
                                {
                                    num12 = Decimal.Zero;
                                }

                                num15 = num12;
                            }

                            num16 = num15;
                        }

                        num17 = num16;
                    }

                    var num11 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i] - inOpen[i]) >
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num17 / num11 &&
                        inOpen[i] < inLow[i - 1] && inClose[i] < inOpen[i - 1] &&
                        inClose[i] > inClose[i - 1] + Math.Abs(inClose[i - 1] - inOpen[i - 1]) * 0.5m)
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_05A6;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_05A6:
            var totIdx = 1;
            while (totIdx >= 0)
            {
                decimal num5;
                decimal num10;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[bodyLongTrailingIdx - totIdx] - inOpen[bodyLongTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[bodyLongTrailingIdx - totIdx] - inLow[bodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num2;
                            decimal num3;
                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num3 = inClose[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[bodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num2 = inOpen[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[bodyLongTrailingIdx - totIdx];
                            }

                            num = inHigh[bodyLongTrailingIdx - totIdx] - num3 + (num2 - inLow[bodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = Decimal.Zero;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                bodyLongPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0253;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlPiercingLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod + 1;
        }
    }
}
