using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3LineStrike(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int num46;
            var nearPeriodTotal = new double[4];
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

            int lookbackTotal = Cdl3LineStrikeLookback();
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

            nearPeriodTotal[3] = 0.0;
            nearPeriodTotal[2] = 0.0;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int i = nearTrailingIdx;
            while (true)
            {
                double num51;
                double num56;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num56 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                }
                else
                {
                    double num55;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num55 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        double num52;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num53;
                            double num54;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num54 = inClose[i - 3];
                            }
                            else
                            {
                                num54 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num53 = inOpen[i - 3];
                            }
                            else
                            {
                                num53 = inClose[i - 3];
                            }

                            num52 = inHigh[i - 3] - num54 + (num53 - inLow[i - 3]);
                        }
                        else
                        {
                            num52 = 0.0;
                        }

                        num55 = num52;
                    }

                    num56 = num55;
                }

                nearPeriodTotal[3] += num56;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num51 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num50;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num50 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num47;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                nearPeriodTotal[2] += num51;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0256:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                num46 = 1;
            }
            else
            {
                num46 = -1;
            }

            if ((inClose[i - 3] < inOpen[i - 3] ? -1 : 1) == num46)
            {
                int num45;
                if (inClose[i - 1] >= inOpen[i - 1])
                {
                    num45 = 1;
                }
                else
                {
                    num45 = -1;
                }

                if ((inClose[i - 2] < inOpen[i - 2] ? -1 : 1) == num45)
                {
                    int num44;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num44 = 1;
                    }
                    else
                    {
                        num44 = -1;
                    }

                    if ((inClose[i] < inOpen[i] ? -1 : 1) == -num44)
                    {
                        double num42;
                        double num43;
                        if (inOpen[i - 3] < inClose[i - 3])
                        {
                            num43 = inOpen[i - 3];
                        }
                        else
                        {
                            num43 = inClose[i - 3];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                        {
                            num42 = nearPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                        }
                        else
                        {
                            double num41;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                            {
                                num41 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                            }
                            else
                            {
                                double num40;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                {
                                    num40 = inHigh[i - 3] - inLow[i - 3];
                                }
                                else
                                {
                                    double num37;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                    {
                                        double num38;
                                        double num39;
                                        if (inClose[i - 3] >= inOpen[i - 3])
                                        {
                                            num39 = inClose[i - 3];
                                        }
                                        else
                                        {
                                            num39 = inOpen[i - 3];
                                        }

                                        if (inClose[i - 3] >= inOpen[i - 3])
                                        {
                                            num38 = inOpen[i - 3];
                                        }
                                        else
                                        {
                                            num38 = inClose[i - 3];
                                        }

                                        num37 = inHigh[i - 3] - num39 + (num38 - inLow[i - 3]);
                                    }
                                    else
                                    {
                                        num37 = 0.0;
                                    }

                                    num40 = num37;
                                }

                                num41 = num40;
                            }

                            num42 = num41;
                        }

                        var num36 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                        if (inOpen[i - 2] >= num43 - Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num42 / num36)
                        {
                            double num34;
                            double num35;
                            if (inOpen[i - 3] > inClose[i - 3])
                            {
                                num35 = inOpen[i - 3];
                            }
                            else
                            {
                                num35 = inClose[i - 3];
                            }

                            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                            {
                                num34 = nearPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                            }
                            else
                            {
                                double num33;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                {
                                    num33 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                                }
                                else
                                {
                                    double num32;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                    {
                                        num32 = inHigh[i - 3] - inLow[i - 3];
                                    }
                                    else
                                    {
                                        double num29;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                        {
                                            double num30;
                                            double num31;
                                            if (inClose[i - 3] >= inOpen[i - 3])
                                            {
                                                num31 = inClose[i - 3];
                                            }
                                            else
                                            {
                                                num31 = inOpen[i - 3];
                                            }

                                            if (inClose[i - 3] >= inOpen[i - 3])
                                            {
                                                num30 = inOpen[i - 3];
                                            }
                                            else
                                            {
                                                num30 = inClose[i - 3];
                                            }

                                            num29 = inHigh[i - 3] - num31 + (num30 - inLow[i - 3]);
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

                            var num28 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                            if (inOpen[i - 2] <= num35 + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num34 / num28)
                            {
                                double num26;
                                double num27;
                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num27 = inOpen[i - 2];
                                }
                                else
                                {
                                    num27 = inClose[i - 2];
                                }

                                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                                {
                                    num26 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                                }
                                else
                                {
                                    double num25;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                    {
                                        num25 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                    }
                                    else
                                    {
                                        double num24;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                        {
                                            num24 = inHigh[i - 2] - inLow[i - 2];
                                        }
                                        else
                                        {
                                            double num21;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                            {
                                                double num22;
                                                double num23;
                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num23 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num23 = inOpen[i - 2];
                                                }

                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num22 = inOpen[i - 2];
                                                }
                                                else
                                                {
                                                    num22 = inClose[i - 2];
                                                }

                                                num21 = inHigh[i - 2] - num23 + (num22 - inLow[i - 2]);
                                            }
                                            else
                                            {
                                                num21 = 0.0;
                                            }

                                            num24 = num21;
                                        }

                                        num25 = num24;
                                    }

                                    num26 = num25;
                                }

                                var num20 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                                if (inOpen[i - 1] >= num27 - Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num26 / num20)
                                {
                                    double num18;
                                    double num19;
                                    if (inOpen[i - 2] > inClose[i - 2])
                                    {
                                        num19 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num19 = inClose[i - 2];
                                    }

                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                                    {
                                        num18 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                                    }
                                    else
                                    {
                                        double num17;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                        {
                                            num17 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                        }
                                        else
                                        {
                                            double num16;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                            {
                                                num16 = inHigh[i - 2] - inLow[i - 2];
                                            }
                                            else
                                            {
                                                double num13;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                                {
                                                    double num14;
                                                    double num15;
                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num15 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num15 = inOpen[i - 2];
                                                    }

                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num14 = inOpen[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num14 = inClose[i - 2];
                                                    }

                                                    num13 = inHigh[i - 2] - num15 + (num14 - inLow[i - 2]);
                                                }
                                                else
                                                {
                                                    num13 = 0.0;
                                                }

                                                num16 = num13;
                                            }

                                            num17 = num16;
                                        }

                                        num18 = num17;
                                    }

                                    var num12 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows
                                        ? 2.0
                                        : 1.0;

                                    if (inOpen[i - 1] <=
                                        num19 + Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num18 / num12 &&
                                        (inClose[i - 1] >= inOpen[i - 1] && inClose[i - 1] > inClose[i - 2] &&
                                         inClose[i - 2] > inClose[i - 3] && inOpen[i] > inClose[i - 1] && inClose[i] < inOpen[i - 3] ||
                                         inClose[i - 1] < inOpen[i - 1] && inClose[i - 1] < inClose[i - 2] &&
                                         inClose[i - 2] < inClose[i - 3] && inOpen[i] < inClose[i - 1] && inClose[i] > inOpen[i - 3]))
                                    {
                                        int num11;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num11 = 1;
                                        }
                                        else
                                        {
                                            num11 = -1;
                                        }

                                        outInteger[outIdx] = num11 * 100;
                                        outIdx++;
                                        goto Label_0975;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0975:
            var totIdx = 3;
            while (totIdx >= 2)
            {
                double num5;
                double num10;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[nearTrailingIdx - totIdx] - inOpen[nearTrailingIdx - totIdx]);
                }
                else
                {
                    double num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[nearTrailingIdx - totIdx] - inLow[nearTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num3 = inClose[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[nearTrailingIdx - totIdx];
                            }

                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num2 = inOpen[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[nearTrailingIdx - totIdx];
                            }

                            num = inHigh[nearTrailingIdx - totIdx] - num3 + (num2 - inLow[nearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                nearPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            nearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0256;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3LineStrike(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int num46;
            var nearPeriodTotal = new decimal[4];
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

            int lookbackTotal = Cdl3LineStrikeLookback();
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

            nearPeriodTotal[3] = Decimal.Zero;
            nearPeriodTotal[2] = Decimal.Zero;
            int nearTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
            int i = nearTrailingIdx;
            while (true)
            {
                decimal num51;
                decimal num56;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num56 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                }
                else
                {
                    decimal num55;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num55 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        decimal num52;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num53;
                            decimal num54;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num54 = inClose[i - 3];
                            }
                            else
                            {
                                num54 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num53 = inOpen[i - 3];
                            }
                            else
                            {
                                num53 = inClose[i - 3];
                            }

                            num52 = inHigh[i - 3] - num54 + (num53 - inLow[i - 3]);
                        }
                        else
                        {
                            num52 = Decimal.Zero;
                        }

                        num55 = num52;
                    }

                    num56 = num55;
                }

                nearPeriodTotal[3] += num56;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num51 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num50;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num50 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num47;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                nearPeriodTotal[2] += num51;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0272:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                num46 = 1;
            }
            else
            {
                num46 = -1;
            }

            if ((inClose[i - 3] < inOpen[i - 3] ? -1 : 1) == num46)
            {
                int num45;
                if (inClose[i - 1] >= inOpen[i - 1])
                {
                    num45 = 1;
                }
                else
                {
                    num45 = -1;
                }

                if ((inClose[i - 2] < inOpen[i - 2] ? -1 : 1) == num45)
                {
                    int num44;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num44 = 1;
                    }
                    else
                    {
                        num44 = -1;
                    }

                    if ((inClose[i] < inOpen[i] ? -1 : 1) == -num44)
                    {
                        decimal num42;
                        decimal num43;
                        if (inOpen[i - 3] < inClose[i - 3])
                        {
                            num43 = inOpen[i - 3];
                        }
                        else
                        {
                            num43 = inClose[i - 3];
                        }

                        if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                        {
                            num42 = nearPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                        }
                        else
                        {
                            decimal num41;
                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                            {
                                num41 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                            }
                            else
                            {
                                decimal num40;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                {
                                    num40 = inHigh[i - 3] - inLow[i - 3];
                                }
                                else
                                {
                                    decimal num37;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                    {
                                        decimal num38;
                                        decimal num39;
                                        if (inClose[i - 3] >= inOpen[i - 3])
                                        {
                                            num39 = inClose[i - 3];
                                        }
                                        else
                                        {
                                            num39 = inOpen[i - 3];
                                        }

                                        if (inClose[i - 3] >= inOpen[i - 3])
                                        {
                                            num38 = inOpen[i - 3];
                                        }
                                        else
                                        {
                                            num38 = inClose[i - 3];
                                        }

                                        num37 = inHigh[i - 3] - num39 + (num38 - inLow[i - 3]);
                                    }
                                    else
                                    {
                                        num37 = Decimal.Zero;
                                    }

                                    num40 = num37;
                                }

                                num41 = num40;
                            }

                            num42 = num41;
                        }

                        var num36 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                        if (inOpen[i - 2] >= num43 - (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num42 / num36)
                        {
                            decimal num34;
                            decimal num35;
                            if (inOpen[i - 3] > inClose[i - 3])
                            {
                                num35 = inOpen[i - 3];
                            }
                            else
                            {
                                num35 = inClose[i - 3];
                            }

                            if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                            {
                                num34 = nearPeriodTotal[3] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                            }
                            else
                            {
                                decimal num33;
                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                {
                                    num33 = Math.Abs(inClose[i - 3] - inOpen[i - 3]);
                                }
                                else
                                {
                                    decimal num32;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                    {
                                        num32 = inHigh[i - 3] - inLow[i - 3];
                                    }
                                    else
                                    {
                                        decimal num29;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                        {
                                            decimal num30;
                                            decimal num31;
                                            if (inClose[i - 3] >= inOpen[i - 3])
                                            {
                                                num31 = inClose[i - 3];
                                            }
                                            else
                                            {
                                                num31 = inOpen[i - 3];
                                            }

                                            if (inClose[i - 3] >= inOpen[i - 3])
                                            {
                                                num30 = inOpen[i - 3];
                                            }
                                            else
                                            {
                                                num30 = inClose[i - 3];
                                            }

                                            num29 = inHigh[i - 3] - num31 + (num30 - inLow[i - 3]);
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

                            var num28 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (inOpen[i - 2] <=
                                num35 + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num34 / num28)
                            {
                                decimal num26;
                                decimal num27;
                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num27 = inOpen[i - 2];
                                }
                                else
                                {
                                    num27 = inClose[i - 2];
                                }

                                if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                                {
                                    num26 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                                }
                                else
                                {
                                    decimal num25;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                    {
                                        num25 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                    }
                                    else
                                    {
                                        decimal num24;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                        {
                                            num24 = inHigh[i - 2] - inLow[i - 2];
                                        }
                                        else
                                        {
                                            decimal num21;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                            {
                                                decimal num22;
                                                decimal num23;
                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num23 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num23 = inOpen[i - 2];
                                                }

                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num22 = inOpen[i - 2];
                                                }
                                                else
                                                {
                                                    num22 = inClose[i - 2];
                                                }

                                                num21 = inHigh[i - 2] - num23 + (num22 - inLow[i - 2]);
                                            }
                                            else
                                            {
                                                num21 = Decimal.Zero;
                                            }

                                            num24 = num21;
                                        }

                                        num25 = num24;
                                    }

                                    num26 = num25;
                                }

                                var num20 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows ? 2m : 1m;

                                if (inOpen[i - 1] >=
                                    num27 - (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num26 / num20)
                                {
                                    decimal num18;
                                    decimal num19;
                                    if (inOpen[i - 2] > inClose[i - 2])
                                    {
                                        num19 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num19 = inClose[i - 2];
                                    }

                                    if (Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod != 0)
                                    {
                                        num18 = nearPeriodTotal[2] / Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod;
                                    }
                                    else
                                    {
                                        decimal num17;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                                        {
                                            num17 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                                        }
                                        else
                                        {
                                            decimal num16;
                                            if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                                            {
                                                num16 = inHigh[i - 2] - inLow[i - 2];
                                            }
                                            else
                                            {
                                                decimal num13;
                                                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                                                {
                                                    decimal num14;
                                                    decimal num15;
                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num15 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num15 = inOpen[i - 2];
                                                    }

                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num14 = inOpen[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num14 = inClose[i - 2];
                                                    }

                                                    num13 = inHigh[i - 2] - num15 + (num14 - inLow[i - 2]);
                                                }
                                                else
                                                {
                                                    num13 = Decimal.Zero;
                                                }

                                                num16 = num13;
                                            }

                                            num17 = num16;
                                        }

                                        num18 = num17;
                                    }

                                    var num12 = Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows
                                        ? 2m
                                        : 1m;

                                    if (inOpen[i - 1] <=
                                        num19 + (decimal) Globals.CandleSettings[(int) CandleSettingType.Near].Factor * num18 / num12 &&
                                        (inClose[i - 1] >= inOpen[i - 1] && inClose[i - 1] > inClose[i - 2] &&
                                         inClose[i - 2] > inClose[i - 3] && inOpen[i] > inClose[i - 1] && inClose[i] < inOpen[i - 3] ||
                                         inClose[i - 1] < inOpen[i - 1] && inClose[i - 1] < inClose[i - 2] &&
                                         inClose[i - 2] < inClose[i - 3] && inOpen[i] < inClose[i - 1] && inClose[i] > inOpen[i - 3]))
                                    {
                                        int num11;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num11 = 1;
                                        }
                                        else
                                        {
                                            num11 = -1;
                                        }

                                        outInteger[outIdx] = num11 * 100;
                                        outIdx++;
                                        goto Label_0A03;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0A03:
            var totIdx = 3;
            while (totIdx >= 2)
            {
                decimal num5;
                decimal num10;
                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[nearTrailingIdx - totIdx] - inOpen[nearTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[nearTrailingIdx - totIdx] - inLow[nearTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num;
                        if (Globals.CandleSettings[(int) CandleSettingType.Near].RangeType == RangeType.Shadows)
                        {
                            decimal num2;
                            decimal num3;
                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num3 = inClose[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[nearTrailingIdx - totIdx];
                            }

                            if (inClose[nearTrailingIdx - totIdx] >= inOpen[nearTrailingIdx - totIdx])
                            {
                                num2 = inOpen[nearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[nearTrailingIdx - totIdx];
                            }

                            num = inHigh[nearTrailingIdx - totIdx] - num3 + (num2 - inLow[nearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = Decimal.Zero;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                nearPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            nearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0272;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl3LineStrikeLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.Near].AvgPeriod + 3;
        }
    }
}
