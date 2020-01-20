using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlMatchingLow(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
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

            int lookbackTotal = CdlMatchingLowLookback();
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

            double equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int i = equalTrailingIdx;
            while (true)
            {
                double num29;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num29 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num28;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num25;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            double num26;
                            double num27;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num27 = inClose[i - 1];
                            }
                            else
                            {
                                num27 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num26 = inOpen[i - 1];
                            }
                            else
                            {
                                num26 = inClose[i - 1];
                            }

                            num25 = inHigh[i - 1] - num27 + (num26 - inLow[i - 1]);
                        }
                        else
                        {
                            num25 = 0.0;
                        }

                        num28 = num25;
                    }

                    num29 = num28;
                }

                equalPeriodTotal += num29;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_016C:
            if (inClose[i - 1] < inOpen[i - 1] && inClose[i] < inOpen[i])
            {
                double num24;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                {
                    num24 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                }
                else
                {
                    double num23;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                    {
                        num23 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num22;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num19;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

                var num18 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (inClose[i] <= inClose[i - 1] + Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num24 / num18)
                {
                    double num17;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                    {
                        num17 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                    }
                    else
                    {
                        double num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                        {
                            num16 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num15;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num12;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                {
                                    double num13;
                                    double num14;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num14 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num13 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num13 = inClose[i - 1];
                                    }

                                    num12 = inHigh[i - 1] - num14 + (num13 - inLow[i - 1]);
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

                    var num11 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (inClose[i] >= inClose[i - 1] - Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num17 / num11)
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_046A;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_046A:
            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num3 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num2 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[equalTrailingIdx - 1];
                        }

                        num = inHigh[equalTrailingIdx - 1] - num3 + (num2 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            equalPeriodTotal += num10 - num5;
            i++;
            equalTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_016C;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlMatchingLow(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
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

            int lookbackTotal = CdlMatchingLowLookback();
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

            decimal equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int i = equalTrailingIdx;
            while (true)
            {
                decimal num29;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num29 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num28;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num25;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num26;
                            decimal num27;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num27 = inClose[i - 1];
                            }
                            else
                            {
                                num27 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num26 = inOpen[i - 1];
                            }
                            else
                            {
                                num26 = inClose[i - 1];
                            }

                            num25 = inHigh[i - 1] - num27 + (num26 - inLow[i - 1]);
                        }
                        else
                        {
                            num25 = Decimal.Zero;
                        }

                        num28 = num25;
                    }

                    num29 = num28;
                }

                equalPeriodTotal += num29;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_017A:
            if (inClose[i - 1] < inOpen[i - 1] && inClose[i] < inOpen[i])
            {
                decimal num24;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                {
                    num24 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                }
                else
                {
                    decimal num23;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                    {
                        num23 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num22;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num19;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

                var num18 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

                if (inClose[i] <= inClose[i - 1] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num24 / num18)
                {
                    decimal num17;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                    {
                        num17 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                    }
                    else
                    {
                        decimal num16;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                        {
                            num16 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            decimal num15;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                decimal num12;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                {
                                    decimal num13;
                                    decimal num14;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num14 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num13 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num13 = inClose[i - 1];
                                    }

                                    num12 = inHigh[i - 1] - num14 + (num13 - inLow[i - 1]);
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

                    var num11 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (inClose[i] >=
                        inClose[i - 1] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num17 / num11)
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_049C;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_049C:
            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num3 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num2 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[equalTrailingIdx - 1];
                        }

                        num = inHigh[equalTrailingIdx - 1] - num3 + (num2 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            equalPeriodTotal += num10 - num5;
            i++;
            equalTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_017A;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlMatchingLowLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod + 1;
        }
    }
}
