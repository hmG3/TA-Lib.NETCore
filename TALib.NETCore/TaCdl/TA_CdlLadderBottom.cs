using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlLadderBottom(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
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

            int lookbackTotal = CdlLadderBottomLookback();
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

            double shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num23;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num23 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num22;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num22 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num19;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                shadowVeryShortPeriodTotal += num23;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0168:
            if (inClose[i - 4] < inOpen[i - 4] && inClose[i - 3] < inOpen[i - 3] && inClose[i - 2] < inOpen[i - 2] &&
                inOpen[i - 4] > inOpen[i - 3] && inOpen[i - 3] > inOpen[i - 2] && inClose[i - 4] > inClose[i - 3] &&
                inClose[i - 3] > inClose[i - 2] && inClose[i - 1] < inOpen[i - 1])
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

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num17 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num16 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num15;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num15 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num12;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                var num11 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (inHigh[i - 1] - num18 > Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num17 / num11 &&
                    inClose[i] >= inOpen[i] && inOpen[i] > inOpen[i - 1] && inClose[i] > inHigh[i - 1])
                {
                    outInteger[outIdx] = 100;
                    outIdx++;
                    goto Label_03B0;
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_03B0:
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx - 1] - inOpen[shadowVeryShortTrailingIdx - 1]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryShortTrailingIdx - 1] - inLow[shadowVeryShortTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[shadowVeryShortTrailingIdx - 1] >= inOpen[shadowVeryShortTrailingIdx - 1])
                        {
                            num3 = inClose[shadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryShortTrailingIdx - 1];
                        }

                        if (inClose[shadowVeryShortTrailingIdx - 1] >= inOpen[shadowVeryShortTrailingIdx - 1])
                        {
                            num2 = inOpen[shadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryShortTrailingIdx - 1];
                        }

                        num = inHigh[shadowVeryShortTrailingIdx - 1] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryShortPeriodTotal += num10 - num5;
            i++;
            shadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0168;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlLadderBottom(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
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

            int lookbackTotal = CdlLadderBottomLookback();
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

            decimal shadowVeryShortPeriodTotal = default;
            int shadowVeryShortTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
            int i = shadowVeryShortTrailingIdx;
            while (true)
            {
                decimal num23;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                {
                    num23 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num22;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                    {
                        num22 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num19;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                shadowVeryShortPeriodTotal += num23;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0176:
            if (inClose[i - 4] < inOpen[i - 4] && inClose[i - 3] < inOpen[i - 3] && inClose[i - 2] < inOpen[i - 2] &&
                inOpen[i - 4] > inOpen[i - 3] && inOpen[i - 3] > inOpen[i - 2] && inClose[i - 4] > inClose[i - 3] &&
                inClose[i - 3] > inClose[i - 2] && inClose[i - 1] < inOpen[i - 1])
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

                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod != 0)
                {
                    num17 = shadowVeryShortPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod;
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
                    {
                        num16 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num15;
                        if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                        {
                            num15 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num12;
                            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

                var num11 = Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows ? 2m : 1m;

                if (inHigh[i - 1] - num18 >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].Factor * num17 / num11 &&
                    inClose[i] >= inOpen[i] && inOpen[i] > inOpen[i - 1] && inClose[i] > inHigh[i - 1])
                {
                    outInteger[outIdx] = 100;
                    outIdx++;
                    goto Label_03E6;
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_03E6:
            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
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

            if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx - 1] - inOpen[shadowVeryShortTrailingIdx - 1]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryShortTrailingIdx - 1] - inLow[shadowVeryShortTrailingIdx - 1];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[shadowVeryShortTrailingIdx - 1] >= inOpen[shadowVeryShortTrailingIdx - 1])
                        {
                            num3 = inClose[shadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryShortTrailingIdx - 1];
                        }

                        if (inClose[shadowVeryShortTrailingIdx - 1] >= inOpen[shadowVeryShortTrailingIdx - 1])
                        {
                            num2 = inOpen[shadowVeryShortTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryShortTrailingIdx - 1];
                        }

                        num = inHigh[shadowVeryShortTrailingIdx - 1] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx - 1]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryShortPeriodTotal += num10 - num5;
            i++;
            shadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0176;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlLadderBottomLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.ShadowVeryShort].AvgPeriod + 4;
        }
    }
}
